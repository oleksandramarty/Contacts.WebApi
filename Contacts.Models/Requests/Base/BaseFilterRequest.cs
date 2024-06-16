namespace Contacts.Models.Requests.Base;

public class BaseFilterRequest
{
    public BaseFilterRequest()
    {
        
    }
    public BaseFilterRequest(BaseFilterRequest query)
    {
        Paginator = query.Paginator;
        Sort = query.Sort;
        Query = query.Query;
    }
    public PaginatorRequest? Paginator { get; set; }
    public BaseSortableRequest? Sort { get; set; }
    
    public string? Query { get; set; }
}
