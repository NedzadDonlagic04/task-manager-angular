using Backend.Shared.Records;

namespace Backend.Application.Errors.Tasks;

public sealed class TaskError
{
    public static readonly Error NotFound = new(
        "Task.NotFound",
        "Task not found",
        ErrorType.NotFound
    );

    public static readonly Error IdsMissing = new(
        "Task.IdsMissing",
        "No task IDs were provided",
        ErrorType.Validation
    );
}
