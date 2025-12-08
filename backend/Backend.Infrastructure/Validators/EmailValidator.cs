using System.Text.RegularExpressions;
using Backend.Application.Constants;
using Backend.Application.Errors.Validators;
using Backend.Application.Interfaces.Validators;
using Backend.Shared.Classes;

namespace Backend.Infrastructure.Validators;

public sealed partial class EmailValidator : IEmailValidator
{
    public Result Validate(string email)
    {
        email = email?.Trim() ?? "";

        return Validate(email.AsSpan());
    }

    private static bool ContainsConsecutiveChar(ReadOnlySpan<char> span, char c)
    {
        for (int i = 1; i < span.Length; ++i)
        {
            if (span[i - 1] == c && span[i] == c)
            {
                return true;
            }
        }

        return false;
    }

    [GeneratedRegex(
        @"^[a-z0-9\-.+]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase
    )]
    private static partial Regex LocalPartAllowedCharsRegex();

    [GeneratedRegex(
        @"^[a-z0-9\-.]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase
    )]
    private static partial Regex DomainAllowedCharsRegex();

    [GeneratedRegex(
        @"^[a-z]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase
    )]
    private static partial Regex TopLevelDomainRegex();

    private static Result Validate(ReadOnlySpan<char> email)
    {
        int separatorIndex = email.IndexOf('@');

        if (separatorIndex == -1)
        {
            return EmailError.MissingAtSymbol;
        }

        ReadOnlySpan<char> emailLocalPart = email[..separatorIndex];
        ReadOnlySpan<char> emailDomainPart = email[(separatorIndex + 1)..];

        var emailLocalPartValidation = ValidateEmailLocalPart(emailLocalPart);
        if (emailLocalPartValidation.IsFailure)
        {
            return emailLocalPartValidation;
        }

        var emailDomainPartValidation = ValidateEmailDomainPart(emailDomainPart);
        if (emailDomainPartValidation.IsFailure)
        {
            return emailDomainPartValidation;
        }

        return Result.Success();
    }

    private static Result ValidateEmailLocalPart(ReadOnlySpan<char> localPart)
    {
        if (localPart.Length < EmailConstants.LocalPart.MinLength)
        {
            return EmailError.LocalPart.TooShort;
        }
        else if (localPart.Length > EmailConstants.LocalPart.MaxLength)
        {
            return EmailError.LocalPart.TooLong;
        }
        else if (localPart.StartsWith("."))
        {
            return EmailError.LocalPart.StartsWithDot;
        }
        else if (localPart.EndsWith("."))
        {
            return EmailError.LocalPart.EndsWithDot;
        }
        else if (ContainsConsecutiveChar(localPart, '.'))
        {
            return EmailError.LocalPart.ConsecutiveDots;
        }
        else if (!LocalPartAllowedCharsRegex().IsMatch(localPart))
        {
            return EmailError.LocalPart.InvalidCharacters;
        }

        return Result.Success();
    }

    private static Result ValidateEmailDomainPart(ReadOnlySpan<char> domainPart)
    {
        if (domainPart.Length < EmailConstants.DomainPart.MinLength)
        {
            return EmailError.DomainPart.TooShort;
        }
        else if (domainPart.Length > EmailConstants.DomainPart.MaxLength)
        {
            return EmailError.DomainPart.TooLong;
        }
        else if (domainPart.StartsWith("."))
        {
            return EmailError.DomainPart.StartsWithDot;
        }
        else if (domainPart.EndsWith("."))
        {
            return EmailError.DomainPart.EndsWithDot;
        }
        else if (ContainsConsecutiveChar(domainPart, '.'))
        {
            return EmailError.DomainPart.ConsecutiveDots;
        }
        else if (!DomainAllowedCharsRegex().IsMatch(domainPart))
        {
            return EmailError.DomainPart.InvalidCharacters;
        }

        Span<Range> domainLabelRanges = stackalloc Range[EmailConstants.DomainPart.MaxLabelsCount];
        int domainLabelRangesSize = domainPart.Split(domainLabelRanges, '.');
        int expectedDomainLabelRangesSize = domainPart.Count('.') + 1;

        if (domainLabelRangesSize != expectedDomainLabelRangesSize)
        {
            return EmailError.DomainPart.TooManyLabels;
        }
        else if (domainLabelRangesSize < EmailConstants.DomainPart.MinLabelsCount)
        {
            return EmailError.DomainPart.TooFewLabels;
        }

        for (int i = 0; i < domainLabelRangesSize; ++i)
        {
            ReadOnlySpan<char> domainLabel = domainPart[domainLabelRanges[i]];

            var domainLabelValidation = ValidateDomainLabel(domainLabel);

            if (domainLabelValidation.IsFailure)
            {
                return domainLabelValidation;
            }
        }

        Range topLevelDomainRange = domainLabelRanges[domainLabelRangesSize - 1];
        ReadOnlySpan<char> topLevelDomain = domainPart[topLevelDomainRange];

        var topLevelDomainValidation = ValidateTopLevelDomain(topLevelDomain);

        if (topLevelDomainValidation.IsFailure)
        {
            return topLevelDomainValidation;
        }

        return Result.Success();
    }

    private static Result ValidateDomainLabel(ReadOnlySpan<char> domainLabel)
    {
        if (domainLabel.Length < EmailConstants.DomainPart.Label.MinLength)
        {
            return EmailError.DomainPart.Label.TooShort;
        }
        else if (domainLabel.Length > EmailConstants.DomainPart.Label.MaxLength)
        {
            return EmailError.DomainPart.Label.TooLong;
        }
        else if (domainLabel.StartsWith("-"))
        {
            return EmailError.DomainPart.Label.StartsWithHyphen;
        }
        else if (domainLabel.EndsWith("-"))
        {
            return EmailError.DomainPart.Label.EndsWithHyphen;
        }

        return Result.Success();
    }

    private static Result ValidateTopLevelDomain(ReadOnlySpan<char> topLevelDomain)
    {
        if (!TopLevelDomainRegex().IsMatch(topLevelDomain))
        {
            return EmailError.DomainPart.TopLevel.NonLetterCharacters;
        }

        return Result.Success();
    }
}
