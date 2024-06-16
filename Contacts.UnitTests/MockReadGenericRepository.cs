using System.Linq.Expressions;
using AutoMapper;
using Contacts.Constants.Errors;
using Contacts.Interfaces;
using Contacts.Models.Requests.Base;
using Contacts.Models.Responses.Base;

namespace Contacts.UnitTests;

public class MockReadGenericRepository<T> : IReadGenericRepository<T> where T : class
{
    private readonly List<T> _data;
    private readonly IMapper mapper;

    public MockReadGenericRepository(
        List<T> data,
        IMapper mapper)
    {
        _data = data ?? new List<T>();
        this.mapper = mapper;
    }

    public Task<T> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken)
    {
        var result = _data.FirstOrDefault(item => EqualityComparer<TId>.Default.Equals(GetIdFromEntity<TId>(item), id));
        this.CheckEntityExistsAsync(result, cancellationToken);
        return Task.FromResult(result);
    }

    public Task<ListWithIncludeResponse<TResponse>> GetListWithIncludeAsync<TResponse>(
        Expression<Func<T, bool>> condition,
        BaseFilterRequest filter,
        CancellationToken cancellationToken,
        params Func<IQueryable<T>, IQueryable<T>>[] includeFuncs)
    {
        var result = new ListWithIncludeResponse<TResponse>
        {
            Entities = new List<TResponse>(),
            Paginator = this.mapper.Map<PaginatorRequest, PaginatorResponse>(filter.Paginator),
            TotalCount = 100
        };

        return Task.FromResult(result);
    }


    private TId GetIdFromEntity<TId>(T entity)
    {
        var idProperty = typeof(T).GetProperty("Id");
        return (TId)idProperty.GetValue(entity);
    }
    
    private async Task CheckEntityExistsAsync(T entity, CancellationToken cancellationToken)
    {
        if (entity == null)
        {
            throw new Exception($"{typeof(T).Name} {ErrorMessages.NotFound}");
        }
    }
}