using System.Linq.Expressions;
using Contacts.Models.Requests.Base;
using Contacts.Models.Responses.Base;

namespace Contacts.Interfaces;

public interface IReadGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync<TId>(TId id,  CancellationToken cancellationToken);

    Task<ListWithIncludeResponse<TResponse>> GetListWithIncludeAsync<TResponse>(Expression<Func<T, bool>>? condition,
        BaseFilterRequest filter,
        CancellationToken cancellationToken,
        params Func<IQueryable<T>, IQueryable<T>>[]? includeFuncs);
}