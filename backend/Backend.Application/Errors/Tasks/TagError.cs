using Backend.Shared.Records;

namespace Backend.Application.Errors.Tasks;

public sealed class TagError
{
    public static readonly Error NotFound = new(
        "TagError.NotFound",
        "Tag not found",
        ErrorType.NotFound
    );
}
