using Backend.Application.Interfaces.Database;
using Backend.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Backend.Infrastructure.Services.Auth;

public sealed class RefreshTokenCleanupService(
    IServiceProvider serviceProvider,
    ILogger<RefreshTokenCleanupService> logger
) : BackgroundService
{
    private static readonly TimeSpan CleanupInterval = TimeSpan.FromHours(6);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var timer = new PeriodicTimer(CleanupInterval);

        await CleanupExpiredRefreshTokens(cancellationToken);

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
                "RefreshTokenCleanupService removed {Count} expired refresh tokens",
                deletedRefreshTokenCount
            );
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("RefreshTokenCleanupService stopping...");
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
