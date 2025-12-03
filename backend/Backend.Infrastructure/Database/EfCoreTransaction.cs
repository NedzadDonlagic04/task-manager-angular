using Backend.Application.Interfaces.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend.Infrastructure.Database;

public sealed class EfCoreTransaction(IDbContextTransaction transaction) : ITransaction
{
    public Task CommitAsync(CancellationToken cancellationToken = default) =>
        transaction.CommitAsync(cancellationToken);

    public Task RollbackAsync(CancellationToken cancellationToken = default) =>
        transaction.RollbackAsync(cancellationToken);

    public ValueTask DisposeAsync() => transaction.DisposeAsync();
}
