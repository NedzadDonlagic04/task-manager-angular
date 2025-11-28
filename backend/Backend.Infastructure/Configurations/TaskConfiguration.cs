using Backend.Domain.Entities.Tasks;
using Backend.Infastructure.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infastructure.Configurations;

internal sealed class TaskConfiguration
    : EntityTypeConfigurationAndSeeding<Domain.Entities.Tasks.TaskEntity>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<Domain.Entities.Tasks.TaskEntity> builder
    )
    {
        builder.HasKey(task => task.Id);
        builder.Property(task => task.Id).ValueGeneratedOnAdd();

        builder.Property(task => task.Title).IsRequired().HasMaxLength(50);

        builder.Property(task => task.Description).IsRequired().HasMaxLength(1_000);

        builder.Property(task => task.Deadline).IsRequired(false);

        builder.Property(task => task.CreatedAt).IsRequired().ValueGeneratedOnAdd();
        builder.Property(task => task.UpdatedAt).IsRequired(false).ValueGeneratedOnAddOrUpdate();

        builder
            .HasMany(task => task.Tags)
            .WithMany(task => task.Tasks)
            .UsingEntity<TaskTagEntity>();

        builder.ToTable("Task");
    }
}
