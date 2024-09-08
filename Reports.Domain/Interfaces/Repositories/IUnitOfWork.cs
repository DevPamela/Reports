namespace Reports.Domain.Interfaces.Repositories;
public interface IUnitOfWork
{
    /// <summary>
    /// Persists changes in the data store.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}