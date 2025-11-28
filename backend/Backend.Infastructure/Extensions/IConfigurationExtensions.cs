using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace Backend.Infastructure.Extensions;

public static class IConfigurationExtensions
{
    public static OptionsType GetValidatedSection<OptionsType>(
        this IConfiguration config,
        string sectionName
    )
    {
        ArgumentNullException.ThrowIfNullOrEmpty(sectionName, nameof(sectionName));

        var options = config.GetSection(sectionName).Get<OptionsType>();

        if (options is null)
        {
            throw new InvalidOperationException(
                $"Configuration section '{sectionName}' could not be found"
            );
        }

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(options);

        if (!Validator.TryValidateObject(options, validationContext, validationResults))
        {
            string errorMessages = string.Join(
                ", ",
                validationResults.Select(validationResult => validationResult.ErrorMessage)
            );

            throw new InvalidOperationException(
                $"Invalid configuration for section '{sectionName}', errors: {errorMessages}"
            );
        }

        return options;
    }
}
