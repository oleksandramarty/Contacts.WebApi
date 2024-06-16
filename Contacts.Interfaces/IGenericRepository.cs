namespace Contacts.Interfaces;

public interface IGenericRepository<T> : IReadGenericRepository<T> where T : class
{
    Task AddAsync(T entity, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    Task DeleteAsync<TId>(TId id, CancellationToken cancellationToken);
}