using System.ComponentModel.DataAnnotations;
using DTOs;
using Validation;

namespace Test.DTOs
{
    public class TaskCreateDTO_Test
    {
        private static TaskCreateDTO CreateValidTaskDTO()
        {
            return new TaskCreateDTO
            {
                Title = "Valid Task Title",
                Description = "Valid Task Description",
                Deadline = DateTime.Now.AddDays(7),
                TagIds = new()
            };
        }

        private static List<ValidationResult> Validate(object taskCreateDTO)
        {
            var validationContext = new ValidationContext(taskCreateDTO);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(taskCreateDTO, validationContext, validationResults, true);

            return validationResults;
        }

        private static string GetErrorMessageFromAttribute<TPropertyOwner, TAttribute>(string propertyName)
            where TAttribute : ValidationAttribute
        {
            var property = typeof(TPropertyOwner).GetProperty(propertyName);

            if (property == null)
            {
                throw new ArgumentException($"Property '{propertyName}' doesn't exist on type '{typeof(TPropertyOwner).Name}'");
            }

            var attribute = property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            var errorMessage = attribute?.FormatErrorMessage(propertyName) ?? $"The {propertyName} field is not valid.";

            return errorMessage;
        }

        [Fact]
        public void TaskCreateDTO_ValidData_NoValidationErrors()
        {
            var taskCreateDTO = CreateValidTaskDTO();

            var results = Validate(taskCreateDTO);

            Assert.Empty(results);
        }

        [Fact]
        public void Title_IsRequired_ReturnsValidationError()
        {
            var taskCreateDTO = CreateValidTaskDTO();
            taskCreateDTO.Title = null!;

            var results = Validate(taskCreateDTO);
            var expectedErrorMessage = GetErrorMessageFromAttribute<TaskCreateDTO, RequiredAttribute>(nameof(TaskCreateDTO.Title));

            Assert.Single(results);
            Assert.Equal(expectedErrorMessage, results[0].ErrorMessage);
        }

        [Fact]
        public void Title_TooShort_ReturnsValidationError()
        {
            var taskCreateDTO = CreateValidTaskDTO();
            taskCreateDTO.Title = "aa";

            var results = Validate(taskCreateDTO);
            var expectedErrorMessage = GetErrorMessageFromAttribute<TaskCreateDTO, StringLengthAttribute>(nameof(TaskCreateDTO.Title));

            Assert.Single(results);
            Assert.Equal(expectedErrorMessage, results[0].ErrorMessage);
        }

        [Fact]
        public void Title_TooLong_ReturnsValidationError()
        {
            var taskCreateDTO = CreateValidTaskDTO();
            taskCreateDTO.Title = new string('a', 51);

            var results = Validate(taskCreateDTO);
            var expectedErrorMessage = GetErrorMessageFromAttribute<TaskCreateDTO, StringLengthAttribute>(nameof(TaskCreateDTO.Title));

            Assert.Single(results);
            Assert.Equal(expectedErrorMessage, results[0].ErrorMessage);
        }

        [Fact]
        public void Description_TooLong_ReturnsValidationError()
        {
            var taskCreateDTO = CreateValidTaskDTO();
            taskCreateDTO.Description = new string('a', 1_001);

            var results = Validate(taskCreateDTO);
            var expectedErrorMessage = GetErrorMessageFromAttribute<TaskCreateDTO, StringLengthAttribute>(nameof(TaskCreateDTO.Description));

            Assert.Single(results);
            Assert.Equal(expectedErrorMessage, results[0].ErrorMessage);
        }

        [Fact]
        public void Deadline_NotAnHourAhead_ReturnsValidationError()
        {
            var taskCreateDTO = CreateValidTaskDTO();
            taskCreateDTO.Deadline = DateTime.UtcNow;

            var results = Validate(taskCreateDTO);
            var expectedErrorMessage = GetErrorMessageFromAttribute<TaskCreateDTO, MinimumFutureOffsetAttribute>(nameof(TaskCreateDTO.Deadline));

            Assert.Single(results);
            Assert.Equal(expectedErrorMessage, results[0].ErrorMessage);
        }

        [Fact]
        public void TagIds_IsRequired_ReturnsValidationError()
        {
            var taskCreateDTO = CreateValidTaskDTO();
            taskCreateDTO.TagIds = null!;

            var results = Validate(taskCreateDTO);
            var expectedErrorMessage = GetErrorMessageFromAttribute<TaskCreateDTO, RequiredAttribute>(nameof(TaskCreateDTO.TagIds));

            Assert.Single(results);
            Assert.Equal(expectedErrorMessage, results[0].ErrorMessage);
        }
    }
}
