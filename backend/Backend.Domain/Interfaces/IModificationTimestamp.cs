namespace Backend.Domain.Interfaces;

public interface IModificationTimestamp
{
    DateTimeOffset? UpdatedAt { get; set; }
}
