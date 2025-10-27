﻿using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Backend.Infastructure.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infastructure.Configurations;

internal sealed class TaskStateConfiguration : EntityTypeConfigurationAndSeeding<TaskState>
{
    protected override void ConfigureEntity(EntityTypeBuilder<TaskState> builder)
    {
        builder.HasKey(taskState => taskState.Id);
        builder.Property(taskState => taskState.Id).ValueGeneratedNever();

        builder.Property(taskState => taskState.Name).IsRequired().HasMaxLength(30);
        builder.HasIndex(taskState => taskState.Name).IsUnique();

        builder.Property(taskState => taskState.CreatedAt).IsRequired().ValueGeneratedOnAdd();
        builder
            .Property(taskState => taskState.UpdatedAt)
            .IsRequired(false)
            .ValueGeneratedOnAddOrUpdate();

        builder
            .HasMany(taskState => taskState.Tasks)
            .WithOne(task => task.TaskState)
            .HasForeignKey(task => task.TaskStateId)
            .IsRequired();

        builder.ToTable("TaskState");
    }

    protected override void SeedData(EntityTypeBuilder<TaskState> builder)
    {
        var fixedSeedTime = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);

        var taskStatesData = Enum.GetValues(typeof(TaskStateEnum))
            .Cast<TaskStateEnum>()
            .Where(taskStateEnum => taskStateEnum != TaskStateEnum.Uninitialized)
            .Select(taskStateEnum => new TaskState
            {
                Id = (int)taskStateEnum,
                Name = taskStateEnum.ToString(),
                CreatedAt = fixedSeedTime,
                UpdatedAt = null,
            })
            .ToArray();

        builder.HasData(taskStatesData);
    }
}
