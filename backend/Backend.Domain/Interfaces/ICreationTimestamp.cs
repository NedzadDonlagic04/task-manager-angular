namespace Backend.Domain.Interfaces;

public interface ICreationTimestamp
{
    DateTimeOffset CreatedAt { get; set; }
}
