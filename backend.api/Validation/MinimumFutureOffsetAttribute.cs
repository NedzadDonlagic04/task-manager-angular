using System.ComponentModel.DataAnnotations;

namespace Validation;

public sealed class MinimumFutureOffsetAttribute : ValidationAttribute
{
    private readonly double _hours;

    public MinimumFutureOffsetAttribute(double hours)
    {
        _hours = hours;
        ErrorMessage = $"The date must be at least {_hours} hours in the future.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }
        else if (value is DateTime dateTimeValue)
        {
            DateTime minimumValidDateTime = DateTime.UtcNow.AddHours(_hours);

            if (dateTimeValue >= minimumValidDateTime)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }

        return new ValidationResult("Property must be a valid date.");
    }

    public override string FormatErrorMessage(string name) => ErrorMessage!;
}
