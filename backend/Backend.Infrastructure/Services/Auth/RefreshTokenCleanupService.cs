using Backend.Application.Interfaces.Database;
using Backend.Domain.Entities.Auth;
using Backend.Shared.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Backend.Infrastructure.Services.Auth;

public sealed class RefreshTokenCleanupService(
    IServiceProvider serviceProvider,
    ILogger<RefreshTokenCleanupService> logger,
    IOptions<RefreshTokenCleanupOptions> refreshTokenCleanupOptions
) : BackgroundService
{
    private readonly RefreshTokenCleanupOptions _refreshTokenCleanupOptions =
        refreshTokenCleanupOptions.Value;

    private readonly TimeSpan _cleanupInterval = TimeSpan.FromHours(
        refreshTokenCleanupOptions.Value.CleanupHours
    );

    protected override async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var timer = new PeriodicTimer(_cleanupInterval);

        if (_refreshTokenCleanupOptions.RunOnStart)
        {
            await CleanupExpiredRefreshTokens(cancellationToken);
        }

        while (await timer.WaitForNextTickAsync(cancellationToken))
        {
            await CleanupExpiredRefreshTokens(cancellationToken);
        }
    }

    private async Task CleanupExpiredRefreshTokens(CancellationToken cancellationToken = default)
    {
        try
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

            int deletedRefreshTokenCount = await context
                .Set<RefreshTokenEntity>()
                .Where(refreshToken => refreshToken.ExpiresAt < DateTimeOffset.UtcNow)
                .ExecuteDeleteAsync(cancellationToken);

            logger.LogInformation(
                "Expired refresh token cleanup completed, {Count} token(s) removed",
                deletedRefreshTokenCount
            );
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Stopping service...");
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Unexpected error occurred while cleaning up refresh tokens"
            );
        }
    }
}
