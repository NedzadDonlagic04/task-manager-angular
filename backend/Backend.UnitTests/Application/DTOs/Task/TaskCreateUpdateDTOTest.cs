using System.ComponentModel.DataAnnotations;
using Backend.Application.Attributes;
using Backend.Application.DTOs.Task;

namespace Backend.UnitTests.Application.DTOs.Task;

public sealed class TaskCreateUpdateDTOTest
{
    public static readonly TheoryData<string> TitlesOutsideAllowedRange =
    [
        new string('a', 2),
        new string('a', 51),
    ];

    [Fact]
    public void ValidData_NoValidationError()
    {
        var taskCreateUpdateDTO = CreateValidTaskCreateUpdateDTO();

        var validationResults = Validate(taskCreateUpdateDTO);

        Assert.Empty(validationResults);
    }

    [Fact]
    public void TitleAtMinLength_NoValidationError()
    {
        var taskCreateUpdateDTO = CreateValidTaskCreateUpdateDTO() with
        {
            Title = new string('a', 3),
        };

        var validationResults = Validate(taskCreateUpdateDTO);

        Assert.Empty(validationResults);
    }

    [Fact]
    public void TitleAtMaxLength_NoValidationError()
    {
        var taskCreateUpdateDTO = CreateValidTaskCreateUpdateDTO() with
        {
            Title = new string('a', 50),
        };

        var validationResults = Validate(taskCreateUpdateDTO);

        Assert.Empty(validationResults);
    }

    [Fact]
    public void DescriptionAtMaxLength_NoValidationError()
    {
        var taskCreateUpdateDTO = CreateValidTaskCreateUpdateDTO() with
        {
            Description = new string('a', 1_000),
        };

        var validationResults = Validate(taskCreateUpdateDTO);

        Assert.Empty(validationResults);
    }

    [Fact]
    public void DeadlineNull_NoValidationError()
    {
        var taskCreateUpdateDTO = CreateValidTaskCreateUpdateDTO() with { Deadline = null };

        var validationResults = Validate(taskCreateUpdateDTO);

        Assert.Empty(validationResults);
    }

    [Theory]
    [MemberData(nameof(TitlesOutsideAllowedRange))]
    public void InvalidTitleLength_ReturnsValidationError(string title)
    {
        var taskCreateUpdateDTO = CreateValidTaskCreateUpdateDTO() with { Title = title };

        var validationResults = Validate(taskCreateUpdateDTO);

        string expectedErrorMessage = GetErrorMessageFromAttribute<
            TaskCreateUpdateDTO,
            StringLengthAttribute
        >(nameof(TaskCreateUpdateDTO.Title));

        Assert.Single(validationResults);
        Assert.Equal(expectedErrorMessage, validationResults[0].ErrorMessage);
    }

    [Fact]
    public void InvalidDescriptionLength_ReturnsValidationError()
    {
        var taskCreateUpdateDTO = CreateValidTaskCreateUpdateDTO() with
        {
            Description = new string('a', 1_001),
        };

        var validationResults = Validate(taskCreateUpdateDTO);

        string expectedErrorMessage = GetErrorMessageFromAttribute<
            TaskCreateUpdateDTO,
            StringLengthAttribute
        >(nameof(TaskCreateUpdateDTO.Description));

        Assert.Single(validationResults);
        Assert.Equal(expectedErrorMessage, validationResults[0].ErrorMessage);
    }

    [Fact]
    public void NotAnHourAhead_ReturnsValidationError()
    {
        var taskCreateUpdateDTO = CreateValidTaskCreateUpdateDTO() with
        {
            Deadline = DateTimeOffset.UtcNow,
        };

        var validationResults = Validate(taskCreateUpdateDTO);

        string expectedErrorMessage = GetErrorMessageFromAttribute<
            TaskCreateUpdateDTO,
            MinimumFutureOffsetAttribute
        >(nameof(TaskCreateUpdateDTO.Deadline));

        Assert.Single(validationResults);
        Assert.Equal(expectedErrorMessage, validationResults[0].ErrorMessage);
    }

    private static string GetErrorMessageFromAttribute<TPropertyOwner, TAttribute>(
        string propertyName
    )
        where TAttribute : ValidationAttribute
    {
        ArgumentException.ThrowIfNullOrEmpty(propertyName, nameof(propertyName));

        var property = typeof(TPropertyOwner).GetProperty(propertyName);

        ArgumentNullException.ThrowIfNull(property, nameof(property));

        var attribute =
            property.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault() as TAttribute;

        ArgumentNullException.ThrowIfNull(attribute, nameof(attribute));

        string errorMessage = attribute.FormatErrorMessage(propertyName);

        return errorMessage;
    }

    private static List<ValidationResult> Validate(TaskCreateUpdateDTO taskCreateUpdateDTO)
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

    private static TaskCreateUpdateDTO CreateValidTaskCreateUpdateDTO()
    {
        var taskCreateUpdateDTO = new TaskCreateUpdateDTO
        {
            Title = "Valid Task Title",
            Description = "Valid Task Description",
            Deadline = DateTimeOffset.UtcNow.AddDays(7),
            TagIds = [],
        };

        return taskCreateUpdateDTO;
    }
}
