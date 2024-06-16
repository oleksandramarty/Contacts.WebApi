using Contacts.Models.Requests.Base;

namespace Contacts.Interfaces;

public interface IFilterProvider<T>
{
    IQueryable<T> Apply(IQueryable<T> query, BaseFilterRequest filter);
}