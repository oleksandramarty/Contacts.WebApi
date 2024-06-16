using Contacts.Interfaces;
using Contacts.Models.Requests.Base;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Repositories;

public class FilterProvider<T>: IFilterProvider<T> where T : class
{
    public IQueryable<T> Apply(IQueryable<T> query, BaseFilterRequest filter)
    {
        if (filter == null)
        {
            return query;
        }

        query = this.ApplySort(query, filter.Sort);
        return this.ApplyPagination(query, filter.Paginator);
    }
    
    private IQueryable<T> ApplyPagination(IQueryable<T> query, PaginatorRequest filter)
    {
        if (filter == null)
        {
            return query;
        }

        if (filter.PageNumber < 1)
        {
            filter.PageNumber = 1;
        }

        if (filter.PageSize < 1)
        {
            filter.PageSize = 5;
        }

        if (!filter.IsFull)
        {
            return query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);
        }

        return query;
    }

    private IQueryable<T> ApplySort(IQueryable<T> query, BaseSortableRequest filter)
    {
        if (filter == null)
        {
            return query;
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Column))
        {
            if (string.Equals(filter.Direction, "asc", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderBy(x => EF.Property<object>(x, filter.Column));
            }
            else if (string.Equals(filter.Direction, "desc", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderByDescending(x => EF.Property<object>(x, filter.Column));
            }
        }
        
        return query;
    }
}