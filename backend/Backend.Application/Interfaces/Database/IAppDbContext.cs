using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Interfaces.Database;

public interface IAppDbContext
{
    DbSet<Entity> Set<Entity>()
        where Entity : class;

    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
