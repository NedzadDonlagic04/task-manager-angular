using System.ComponentModel.DataAnnotations;
using DTOs;
using Enums;
using Validation;

namespace UnitTest.DTOs
{
    public class TaskUpdateDTO_Test
    {
        private static TaskUpdateDTO CreateValidTaskDTO()
        {
            return new TaskUpdateDTO
            {
                Title = "Valid Task Title",
                Description = "Valid Task Description",
                Deadline = DateTime.Now.AddDays(7),
                TaskStateId = (int)TaskStateEnum.Pending,
                TagIds = new()
            };
        }

        private static List<ValidationResult> Validate(object taskUpdateDTO)
        {
            var validationContext = new ValidationContext(taskUpdateDTO);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(taskUpdateDTO, validationContext, validationResults, true);

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
        public void TaskUpdateDTO_ValidData_NoValidationErrors()
        {
            var taskUpdateDTO = CreateValidTaskDTO();

            var results = Validate(taskUpdateDTO);

            Assert.Empty(results);
        }

        [Fact]
        public void Title_IsRequired_ReturnsValidationError()
        {
            var taskUpdateDTO = CreateValidTaskDTO();
            taskUpdateDTO.Title = null!;

            var results = Validate(taskUpdateDTO);
            var expectedErrorMessage = GetErrorMessageFromAttribute<TaskUpdateDTO, RequiredAttribute>(nameof(TaskUpdateDTO.Title));

            Assert.Single(results);
            Assert.Equal(expectedErrorMessage, results[0].ErrorMessage);
        }

        [Fact]
        public void Title_TooShort_ReturnsValidationError()
        {
            var taskUpdateDTO = CreateValidTaskDTO();
            taskUpdateDTO.Title = "aa";

            var results = Validate(taskUpdateDTO);
            var expectedErrorMessage = GetErrorMessageFromAttribute<TaskUpdateDTO, StringLengthAttribute>(nameof(TaskUpdateDTO.Title));

            Assert.Single(results);
            Assert.Equal(expectedErrorMessage, results[0].ErrorMessage);
        }

        [Fact]
        public void Title_TooLong_ReturnsValidationError()
        {
            var taskUpdateDTO = CreateValidTaskDTO();
            taskUpdateDTO.Title = new string('a', 51);

            var results = Validate(taskUpdateDTO);
            var expectedErrorMessage = GetErrorMessageFromAttribute<TaskUpdateDTO, StringLengthAttribute>(nameof(TaskUpdateDTO.Title));

            Assert.Single(results);
            Assert.Equal(expectedErrorMessage, results[0].ErrorMessage);
        }

        [Fact]
        public void Description_TooLong_ReturnsValidationError()
        {
            var taskUpdateDTO = CreateValidTaskDTO();
            taskUpdateDTO.Description = new string('a', 1_001);

            var results = Validate(taskUpdateDTO);
            var expectedErrorMessage = GetErrorMessageFromAttribute<TaskUpdateDTO, StringLengthAttribute>(nameof(TaskUpdateDTO.Description));

            Assert.Single(results);
            Assert.Equal(expectedErrorMessage, results[0].ErrorMessage);
        }

        [Fact]
        public void Deadline_NotAnHourAhead_ReturnsValidationError()
        {
            var taskUpdateDTO = CreateValidTaskDTO();
            taskUpdateDTO.Deadline = DateTime.UtcNow;

            var results = Validate(taskUpdateDTO);
            var expectedErrorMessage = GetErrorMessageFromAttribute<TaskUpdateDTO, MinimumFutureOffsetAttribute>(nameof(TaskUpdateDTO.Deadline));

            Assert.Single(results);
            Assert.Equal(expectedErrorMessage, results[0].ErrorMessage);
        }

        [Fact]
        public void TagIds_IsRequired_ReturnsValidationError()
        {
            var taskUpdateDTO = CreateValidTaskDTO();
            taskUpdateDTO.TagIds = null!;

            var results = Validate(taskUpdateDTO);
            var expectedErrorMessage = GetErrorMessageFromAttribute<TaskUpdateDTO, RequiredAttribute>(nameof(TaskUpdateDTO.TagIds));

            Assert.Single(results);
            Assert.Equal(expectedErrorMessage, results[0].ErrorMessage);
        }

        [Fact]
        public void TaskStateId_IsValidEnumValue_ReturnsValidationError()
        {
            var taskUpdateDTO = CreateValidTaskDTO();
            taskUpdateDTO.TaskStateId = 0;

            var results = Validate(taskUpdateDTO);
            var expectedErrorMessage = GetErrorMessageFromAttribute<TaskUpdateDTO, EnumValidationAttribute>(nameof(TaskUpdateDTO.TaskStateId));

            Assert.Single(results);
            Assert.Equal(expectedErrorMessage, results[0].ErrorMessage);
        }
    }
}
