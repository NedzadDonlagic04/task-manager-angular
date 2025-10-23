using System.ComponentModel.DataAnnotations;

namespace Validation;

public sealed class EnumValidationAttribute : ValidationAttribute
{
    private readonly Type _enumType;

    public EnumValidationAttribute(Type enumType)
    {
        if (enumType == null || !enumType.IsEnum)
        {
            throw new ArgumentException("The provided type must be an enum.", nameof(enumType));
        }

        _enumType = enumType;

        ErrorMessage = $"Invalid value for the enum '{_enumType.Name}'. Valid enum values: {string.Join(", ", Enum.GetNames(_enumType))}";
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

        return new ValidationResult(ErrorMessage);
    }

    public override string FormatErrorMessage(string name) => ErrorMessage!;
}
