using System.ComponentModel.DataAnnotations;

namespace Validation
{
    public class MinimumFutureOffsetAttribute : ValidationAttribute
    {
        private readonly double _hours;

        public MinimumFutureOffsetAttribute(double hours)
        {
            _hours = hours;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is DateTime dateTimeValue)
            {
                DateTime minimumValidDateTime = DateTime.UtcNow.AddHours(_hours);

                if (dateTimeValue >= minimumValidDateTime)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    if (ErrorMessage == null)
                    {
                        ErrorMessage = "The date must be at least {0} hours in the future.";
                    }
                    return new ValidationResult(string.Format(ErrorMessage, _hours));
                }
            }

            return new ValidationResult("Property must be a valid date.");
        }
    }
}
