using Backend.Shared.Records;

namespace Backend.Application.Errors.Tasks;

public static class TaskStateError
{
    public static readonly Error NotFound = new(
        "TaskState.NotFound",
        "Task state not found",
        ErrorType.NotFound
    );
}
