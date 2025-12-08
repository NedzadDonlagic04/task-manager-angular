using System.ComponentModel.DataAnnotations;
using Backend.Application.Interfaces.Validators;

namespace Backend.Application.Attributes;

public sealed partial class StrictEmailAddressAttribute : ValidationAttribute
{
    public override string FormatErrorMessage(string name) => ErrorMessage!;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }
        else if (value is string email)
        {
            var emailValidator = (IEmailValidator?)
                validationContext.GetService(typeof(IEmailValidator));

            if (emailValidator is null)
            {
                throw new InvalidOperationException(
                    $"{nameof(IEmailValidator)} is not registered in the DI container"
                );
            }

            var emailValidation = emailValidator.Validate(email);

            if (emailValidation.IsFailure)
            {
                return new ValidationResult(
                    string.Join(", ", emailValidation.Errors.Select(error => error.Detail))
                );
            }

            return ValidationResult.Success;
        }

        return new ValidationResult("Property must be an email");
    }
}
