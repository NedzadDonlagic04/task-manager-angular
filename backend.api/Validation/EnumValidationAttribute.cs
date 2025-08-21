using System.ComponentModel.DataAnnotations;

namespace Validation
{
    public class EnumValidationAttribute : ValidationAttribute
    {
        private readonly Type _enumType;

        public EnumValidationAttribute(Type enumType)
        {
            if (enumType == null || !enumType.IsEnum)
            {
                throw new ArgumentException("The provided type must be an enum.", nameof(enumType));
            }

            _enumType = enumType;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            else if (Enum.IsDefined(_enumType, value))
            {
                return ValidationResult.Success;
            }

            var memberNames = string.Join(", ", Enum.GetNames(_enumType));
            ErrorMessage = $"The value for '{validationContext.DisplayName}' is not valid. Valid values are: {memberNames}.";

            return new ValidationResult(ErrorMessage);
        }
    }
}
