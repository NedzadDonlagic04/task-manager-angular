namespace Backend.Shared.Interfaces;

public interface IModificationTimestamp
{
    DateTimeOffset? UpdatedAt { get; set; }
}
