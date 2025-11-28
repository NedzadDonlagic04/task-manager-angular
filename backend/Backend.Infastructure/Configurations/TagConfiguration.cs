using Backend.Domain.Entities.Tasks;
using Backend.Infastructure.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infastructure.Configurations;

internal sealed class TagConfiguration : EntityTypeConfigurationAndSeeding<TagEntity>
{
    protected override void ConfigureEntity(EntityTypeBuilder<TagEntity> builder)
    {
        builder.HasKey(tag => tag.Id);
        builder.Property(tag => tag.Id).ValueGeneratedOnAdd();

        builder.Property(tag => tag.Name).IsRequired().HasMaxLength(30);
        builder.HasIndex(tag => tag.Name).IsUnique();

        builder.Property(tag => tag.CreatedAt).IsRequired().ValueGeneratedOnAdd();
        builder.Property(tag => tag.UpdatedAt).IsRequired(false).ValueGeneratedOnAddOrUpdate();

        builder.ToTable("Tag");
    }

    protected override void SeedData(EntityTypeBuilder<TagEntity> builder)
    {
        var fixedSeedTime = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);

        List<TagEntity> tagData =
        [
            new TagEntity
            {
                Id = Guid.Parse("e280113f-a704-4e96-b909-30b322cc08b4"),
                Name = "job",
                CreatedAt = fixedSeedTime,
                UpdatedAt = null,
            },
            new TagEntity
            {
                Id = Guid.Parse("1ea6c135-be0f-4cd8-8029-863975601330"),
                Name = "hobby",
                CreatedAt = fixedSeedTime,
                UpdatedAt = null,
            },
            new TagEntity
            {
                Id = Guid.Parse("345b41fc-0019-46e0-b35d-e5a61ad76a4b"),
                Name = "school",
                CreatedAt = fixedSeedTime,
                UpdatedAt = null,
            },
            new TagEntity
            {
                Id = Guid.Parse("ea56ef56-0c7b-4995-9f7f-1333a363a9db"),
                Name = "house",
                CreatedAt = fixedSeedTime,
                UpdatedAt = null,
            },
        ];

        builder.HasData(tagData);
    }
}
