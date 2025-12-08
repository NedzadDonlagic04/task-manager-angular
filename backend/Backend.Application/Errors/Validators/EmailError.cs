using Backend.Application.Constants;
using Backend.Shared.Records;

namespace Backend.Application.Errors.Validators;

public static class EmailError
{
    public static readonly Error MissingAtSymbol = new(
        "EmailError.MissingAtSymbol",
        "Email is missing @ symbol",
        ErrorType.Validation
    );

    public static class LocalPart
    {
        public static readonly Error TooShort = new(
            "EmailError.LocalPart.TooShort",
            $"Local part of email must be at least {EmailConstants.LocalPart.MinLength} character long",
            ErrorType.Validation
        );

        public static readonly Error TooLong = new(
            "EmailError.LocalPart.TooLong",
            $"Local part of email exceeds {EmailConstants.LocalPart.MaxLength} characters",
            ErrorType.Validation
        );

        public static readonly Error StartsWithDot = new(
            "EmailError.LocalPart.StartsWithDot",
            "Local part of email cannot start with a dot '.'",
            ErrorType.Validation
        );

        public static readonly Error EndsWithDot = new(
            "EmailError.LocalPart.EndsWithDot",
            "Local part of email cannot end with a dot '.'",
            ErrorType.Validation
        );

        public static readonly Error ConsecutiveDots = new(
            "EmailError.LocalPart.ConsecutiveDots",
            "Local part of email cannot contain consecutive dots '..'",
            ErrorType.Validation
        );

        public static readonly Error InvalidCharacters = new(
            "EmailError.LocalPart.InvalidCharacters",
            "Local part of email can only contain: A-Z, a-z, 0-9, -, ., +",
            ErrorType.Validation
        );
    }

    public static class DomainPart
    {
        public static readonly Error TooShort = new(
            "EmailError.DomainPart.TooShort",
            $"Domain part of email must be at least {EmailConstants.DomainPart.MinLength} character long",
            ErrorType.Validation
        );

        public static readonly Error TooLong = new(
            "EmailError.DomainPart.TooLong",
            $"Domain part of email exceeds {EmailConstants.DomainPart.MaxLength} characters",
            ErrorType.Validation
        );

        public static readonly Error StartsWithDot = new(
            "EmailError.DomainPart.StartsWithDot",
            "Domain part of email cannot start with a dot '.'",
            ErrorType.Validation
        );

        public static readonly Error EndsWithDot = new(
            "EmailError.DomainPart.EndsWithDot",
            "Domain part of email cannot end with a dot '.'",
            ErrorType.Validation
        );

        public static readonly Error ConsecutiveDots = new(
            "EmailError.DomainPart.ConsecutiveDots",
            "Domain part of email cannot contain consecutive dots '..'",
            ErrorType.Validation
        );

        public static readonly Error InvalidCharacters = new(
            "EmailError.DomainPart.InvalidCharacters",
            "Domain part of email can only contain: A-Z, a-z, 0-9, -, .",
            ErrorType.Validation
        );

        public static readonly Error TooFewLabels = new(
            "EmailError.DomainPart.TooFewLabels",
            $"Domain part of email contains less than {EmailConstants.DomainPart.MinLabelsCount} labels (e.g., 'example.com')",
            ErrorType.Validation
        );

        public static readonly Error TooManyLabels = new(
            "EmailError.DomainPart.TooManyLabels",
            $"Domain part of email contains more than {EmailConstants.DomainPart.MaxLabelsCount} labels",
            ErrorType.Validation
        );

        public static class Label
        {
            public static readonly Error TooShort = new(
                "EmailError.DomainPart.Label.TooShort",
                $"Domain label of email must be at least {EmailConstants.DomainPart.Label.MinLength} character long",
                ErrorType.Validation
            );

            public static readonly Error TooLong = new(
                "EmailError.DomainPart.Label.TooLong",
                $"Domain label of email exceeds {EmailConstants.DomainPart.Label.MaxLength} characters",
                ErrorType.Validation
            );

            public static readonly Error StartsWithHyphen = new(
                "EmailError.DomainPart.Label.StartsWithHyphen",
                "Domain label of email cannot start with a hyphen '-'",
                ErrorType.Validation
            );

            public static readonly Error EndsWithHyphen = new(
                "EmailError.DomainPart.Label.EndsWithHyphen",
                "Domain label of email cannot end with a hyphen '-'",
                ErrorType.Validation
            );
        }

        public static class TopLevel
        {
            public static readonly Error NonLetterCharacters = new(
                "EmailError.DomainPart.TopLevel.NonLetterCharacters",
                "Domain top level domain contains non letter characters",
                ErrorType.Validation
            );
        }
    }
}
