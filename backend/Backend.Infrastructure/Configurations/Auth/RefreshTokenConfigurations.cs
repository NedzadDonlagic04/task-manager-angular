using Backend.Domain.Entities.Auth;
using Backend.Infrastructure.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Configurations.Auth;

internal sealed class RefreshTokenConfigurations
    : EntityTypeConfigurationAndSeeding<RefreshTokenEntity>
{
    protected override void ConfigureEntity(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.HasKey(refreshToken => refreshToken.TokenHash);

        builder.Property(refreshToken => refreshToken.TokenHash).IsRequired();

        builder.Property(refreshToken => refreshToken.ExpiresAt).IsRequired();

        builder.Property(task => task.CreatedAt).IsRequired().ValueGeneratedOnAdd();
        builder.Property(task => task.UpdatedAt).IsRequired(false).ValueGeneratedOnAddOrUpdate();

        builder.ToTable("RefreshTokens");
    }
}
