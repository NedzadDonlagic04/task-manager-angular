using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Entity> Set<Entity>()
        where Entity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
