﻿using Backend.Domain.Interfaces;

namespace Backend.Domain.Entities;

public sealed class User : ITimeStampedEntity
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string HashedPassword { get; set; } = null!;

    public Guid UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; } = null!;

    public List<Task> Tasks { get; set; } = [];

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
