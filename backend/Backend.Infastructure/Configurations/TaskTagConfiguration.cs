﻿using Backend.Domain.Entities;
using Backend.Infastructure.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infastructure.Configurations;

internal sealed class TaskTagConfiguration : EntityTypeConfigurationAndSeeding<TaskTag>
{
    protected override void ConfigureEntity(EntityTypeBuilder<TaskTag> builder)
    {
        builder.HasKey(taskTag => new { taskTag.TaskId, taskTag.TagId });

        builder
            .HasOne(taskTag => taskTag.Task)
            .WithMany()
            .HasForeignKey(taskTag => taskTag.TaskId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasOne(taskTag => taskTag.Tag)
            .WithMany()
            .HasForeignKey(taskTag => taskTag.TagId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(taskTag => taskTag.CreatedAt).IsRequired().ValueGeneratedOnAdd();
        builder
            .Property(taskTag => taskTag.UpdatedAt)
            .IsRequired(false)
            .ValueGeneratedOnAddOrUpdate();

        builder.ToTable("TaskTag");
    }
}
