using Backend.Infrastructure.Validators;
using Backend.Shared.Records;
using Xunit.Abstractions;

namespace Backend.UnitTests.Infrastructure.Validators;

public sealed class EmailValidatorTest(ITestOutputHelper outputHelper)
{
    private readonly ITestOutputHelper _outputHelper = outputHelper;
    private static readonly EmailValidator Validator = new();

    public static readonly TheoryData<string, string> ValidEmails = new()
    {
        { "Basic common email", "example@gmail.com" },
        { "Dot in local part", "john.doe@example.org" },
        { "Hyphen in local part", "alice-bob@company.net" },
        { "Subdomain present", "user@sub.example.com" },
        { "Plus-tag allowed", "user+tag@domain.com" },
        { "Minimum local and domain", "a@b.com" },
        { "Uppercase allowed", "FIRST.LAST@EXAMPLE.COM" },
        { "Mixed characters", "mix.Of-Things+123@domain.co" },
        { "Multiple subdomains", "user@service.mail.example" },
        { "Deep subdomain chain", "dev@api.backend.server.org" },
        { "Hyphenated domain", "user@mail-server.com" },
        { "Long domain name", "user@my-super-long-domain.net" },
        { "Hyphenated subdomain", "user@sub.mail-server.co" },
        {
            "Long complex local part",
            "very.long.local-part+tag123@example-domain-with-hyphens.com"
        },
        { "Valid short TLD", "user@Example.XY" },
        { "Max-length TLD (63 chars)", $"admin@domain.{new string('a', 63)}" },
        { "Mixed-case domain", "user@SERVER.TeSt" },
        { "Many local-part segments", "localpart.with.many.sections@domain.com" },
        { "Complex but valid local part", "valid-123+test.local@letters-only.TLD" },
        { "Alphabetic TLD", "user@domain.alpha" },
        { "Long TLD", "contact@company.engineering" },
    };

    public static readonly TheoryData<string, string> InvalidLocalPartEmails = new()
    {
        { "Local part has invalid characters", "...invalid local part...@domain.com" },
        { "Missing local part", "@domain.com" },
        {
            "Local part exceeds 64 characters",
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@domain.com"
        },
        { "Starts with dot", ".user@example.com" },
        { "Ends with dot", "user.@example.com" },
        { "Invalid symbol (!)", "us!er@example.com" },
        { "Contains space", "user name@example.com" },
        { "Non-ASCII character (ü)", "müller@example.com" },
        { "Consecutive dots", "....@example.com" },
    };

    public static readonly TheoryData<string, string> InvalidDomainPartEmails = new()
    {
        { "Missing domain part", "user@" },
        { "Domain part too short", "user@a" },
        { "Domain part exceeds 255 characters", $"user@{new string('a', 256)}" },
        { "Domain starts with dot", "user@.example.com" },
        { "Domain ends with dot", "user@example.com." },
        { "Consecutive dots", "user@exa..mple.com" },
        { "Invalid character in domain (!)", "user@ex!mple.com" },
        { "Space in domain", "user@exa mple.com" },
        { "Non-ASCII in domain (ü)", "user@exampüle.com" },
        { "Label starts with hyphen", "user@-label.example" },
        { "Label ends with hyphen", "user@label-.example" },
        { "Label too long (63+ chars)", $"user@{new string('a', 64)}.com" },
        { "Only one label", "user@example" },
        { "Too many labels", "user@a.b.c.d.e.f.g.h.i.j.k.l.m.n.o.p.q.r.s.t.u.com" },
    };

    public static readonly TheoryData<string, string> InvalidTopLevelDomainEmails = new()
    {
        { "TLD contains number", "user@example.c0m" },
        { "TLD contains hyphen", "user@example.co-m" },
        { "TLD contains invalid character (!)", "user@example.c!m" },
        { "TLD contains space", "user@example.c m" },
        { "TLD contains non-ASCII (ü)", "user@example.cöm" },
        { "TLD empty after last dot", "user@example." },
        { "TLD too long (64+ chars)", $"user@example.{new string('a', 64)}" },
        { "Domain OK, TLD invalid", "user@abc.def.123" },
    };

    [Theory, MemberData(nameof(ValidEmails))]
    public void ValidEmailData_NoValidationError(string info, string email)
    {
        var result = Validator.Validate(email);

        _outputHelper.WriteLine($"Debug: Info = {info}");

        Assert.True(
            result.IsSuccess,
            $"Expected success but got failure '{FormatErrors(result.Errors)}'"
        );
    }

    [Theory]
    [MemberData(nameof(InvalidLocalPartEmails))]
    [MemberData(nameof(InvalidDomainPartEmails))]
    [MemberData(nameof(InvalidTopLevelDomainEmails))]
    public void InvalidData_ValidationError(string info, string email)
    {
        var result = Validator.Validate(email);

        _outputHelper.WriteLine($"Debug: Info = {info}");

        Assert.True(result.IsFailure, "Expected failure but got success");
    }

    private static string FormatErrors(IEnumerable<Error> errors) =>
        string.Join(", ", errors.Select(error => error.Detail));
}
