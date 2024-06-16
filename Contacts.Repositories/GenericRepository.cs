using AutoMapper;
using System.Linq.Expressions;
using Contacts.Domain;
using Contacts.Interfaces;
using Contacts.Models.Requests.Base;
using Contacts.Models.Responses.Base;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DataContext context;
    private readonly DbSet<T> dbSet;
    private readonly IFilterProvider<T> filterProvider;
    private readonly IMapper mapper;

    public GenericRepository(
        DataContext context, 
        IFilterProvider<T> filterProvider, 
        IMapper mapper)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.dbSet = this.context.Set<T>();
        this.filterProvider = filterProvider;
        this.mapper = mapper;
    }

    public async Task<T> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken)
    {
        T entity = await dbSet.FindAsync(id, cancellationToken);
        
        return entity;
    }
    
    public async Task<ListWithIncludeResponse<TResponse>> GetListWithIncludeAsync<TResponse>(
        Expression<Func<T, bool>>? condition,
        BaseFilterRequest filter,
        CancellationToken cancellationToken,
        params Func<IQueryable<T>, IQueryable<T>>[]? includeFuncs)
    {
        IQueryable<T> query = dbSet;

        if (includeFuncs != null)
        {
            foreach (var includeFunc in includeFuncs)
            {
                query = includeFunc(query);
            }            
        }

        var queryResult = condition != null ? query.Where(condition) : query;       
        
        int total = await queryResult.CountAsync(cancellationToken);
        
        queryResult = this.filterProvider.Apply(queryResult, filter);

        List<T> entities = await queryResult.ToListAsync(cancellationToken);

        return new ListWithIncludeResponse<TResponse>
        {
            Entities = entities.Select(x => this.mapper.Map<T, TResponse>(x)).ToList(),
            Paginator = this.mapper.Map<PaginatorRequest, PaginatorResponse>(filter.Paginator),
            TotalCount = total
        };
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await this.dbSet.AddAsync(entity, cancellationToken);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        this.context.Entry(entity).State = EntityState.Modified;
        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync<TId>(TId id, CancellationToken cancellationToken)
    {
        var entity = await this.GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            this.dbSet.Remove(entity);
            await this.context.SaveChangesAsync(cancellationToken);
        }
    }
}