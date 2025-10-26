using System.ComponentModel.DataAnnotations;
using Backend.Application.DTOs.TaskDTO;
using Backend.Application.Validation;

namespace Backend.UnitTests.DTOs.TaskDTO;

public class TaskCreateUpdateDTO_Test
{
    private static TaskCreateUpdateDTO CreateValidTaskCreateUpdateDTO()
    {
        return new TaskCreateUpdateDTO
        {
            Title = "Valid Task Title",
            Description = "Valid Task Description",
            Deadline = DateTime.Now.AddDays(7),
            TagIds = [],
        };
    }

    private static List<ValidationResult> Validate(object taskCreateUpdateDTO)
    {
        var validationContext = new ValidationContext(taskCreateUpdateDTO);
        var validationResults = new List<ValidationResult>();

        Validator.TryValidateObject(
            taskCreateUpdateDTO,
            validationContext,
            validationResults,
            true
        );

        return validationResults;
    }

    private static string GetErrorMessageFromAttribute<TPropertyOwner, TAttribute>(
        string propertyName
    )
        where TAttribute : ValidationAttribute
    {
        var property = typeof(TPropertyOwner).GetProperty(propertyName);

        if (property == null)
        {
            throw new ArgumentException(
                $"Property '{propertyName}' doesn't exist on type '{typeof(TPropertyOwner).Name}'"
            );
        }

        var attribute =
            property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
        var errorMessage =
            attribute?.FormatErrorMessage(propertyName)
            ?? $"The {propertyName} field is not valid.";

        return errorMessage;
    }

    [Fact]
    public void TaskCreateUpdateDTO_ValidData_NoValidationErrors()
    {
        var taskCreateUpdateDTO = CreateValidTaskCreateUpdateDTO();

        var results = Validate(taskCreateUpdateDTO);

        Assert.Empty(results);
    }

    [Fact]
    public void Title_IsRequired_ReturnsValidationError()
    {
        var taskCreateUpdateDTO = CreateValidTaskCreateUpdateDTO() with { Title = null! };

        var results = Validate(taskCreateUpdateDTO);
        var expectedErrorMessage = GetErrorMessageFromAttribute<
            TaskCreateUpdateDTO,
            RequiredAttribute
        >(nameof(TaskCreateUpdateDTO.Title));

        Assert.Single(results);
        Assert.Equal(expectedErrorMessage, results[0].ErrorMessage);
    }

    [Fact]
    public void Title_TooShort_ReturnsValidationError()
    {
        var taskCreateUpdateDTO = CreateValidTaskCreateUpdateDTO() with { Title = "aa" };

        var results = Validate(taskCreateUpdateDTO);
        var expectedErrorMessage = GetErrorMessageFromAttribute<
            TaskCreateUpdateDTO,
            StringLengthAttribute
        >(nameof(TaskCreateUpdateDTO.Title));

        Assert.Single(results);
        Assert.Equal(expectedErrorMessage, results[0].ErrorMessage);
    }

    [Fact]
    public void Title_TooLong_ReturnsValidationError()
    {
        var taskCreateUpdateDTO = CreateValidTaskCreateUpdateDTO() with
        {
            Title = new string('a', 51),
        };

        var results = Validate(taskCreateUpdateDTO);
        var expectedErrorMessage = GetErrorMessageFromAttribute<
            TaskCreateUpdateDTO,
            StringLengthAttribute
        >(nameof(TaskCreateUpdateDTO.Title));

        Assert.Single(results);
        Assert.Equal(expectedErrorMessage, results[0].ErrorMessage);
    }

    [Fact]
    public void Description_TooLong_ReturnsValidationError()
    {
        var taskCreateUpdateDTO = CreateValidTaskCreateUpdateDTO() with
        {
            Description = new string('a', 1_001),
        };

        var results = Validate(taskCreateUpdateDTO);
        var expectedErrorMessage = GetErrorMessageFromAttribute<
            TaskCreateUpdateDTO,
            StringLengthAttribute
        >(nameof(TaskCreateUpdateDTO.Description));

        Assert.Single(results);
        Assert.Equal(expectedErrorMessage, results[0].ErrorMessage);
    }

    [Fact]
    public void Deadline_NotAnHourAhead_ReturnsValidationError()
    {
        var taskCreateUpdateDTO = CreateValidTaskCreateUpdateDTO() with
        {
            Deadline = DateTime.UtcNow,
        };

        var results = Validate(taskCreateUpdateDTO);
        var expectedErrorMessage = GetErrorMessageFromAttribute<
            TaskCreateUpdateDTO,
            MinimumFutureOffsetAttribute
        >(nameof(TaskCreateUpdateDTO.Deadline));

        Assert.Single(results);
        Assert.Equal(expectedErrorMessage, results[0].ErrorMessage);
    }

    [Fact]
    public void TagIds_IsRequired_ReturnsValidationError()
    {
        var taskCreateUpdateDTO = CreateValidTaskCreateUpdateDTO() with { TagIds = null! };

        var results = Validate(taskCreateUpdateDTO);
        var expectedErrorMessage = GetErrorMessageFromAttribute<
            TaskCreateUpdateDTO,
            RequiredAttribute
        >(nameof(TaskCreateUpdateDTO.TagIds));

        Assert.Single(results);
        Assert.Equal(expectedErrorMessage, results[0].ErrorMessage);
    }
}
