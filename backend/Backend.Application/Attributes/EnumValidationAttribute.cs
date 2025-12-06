using System.ComponentModel.DataAnnotations;

namespace Backend.Application.Attributes;

public sealed class EnumValidationAttribute : ValidationAttribute
{
    private readonly Type _enumType;

    public EnumValidationAttribute(Type enumType)
    {
        if (enumType is null || !enumType.IsEnum)
        {
            throw new ArgumentException("The provided type must be an enum", nameof(enumType));
        }

        _enumType = enumType;

        ErrorMessage =
            $"Invalid value for the enum '{_enumType.Name}', valid enum values: {string.Join(", ", Enum.GetNames(_enumType))}";
    }

    public override string FormatErrorMessage(string name) => ErrorMessage!;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }
        else if (Enum.IsDefined(_enumType, value))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage);
    }
}
