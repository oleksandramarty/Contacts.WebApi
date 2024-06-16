namespace Contacts.Models.Responses.Base;

public class ListWithIncludeResponse<TResponse>
{
    public ListWithIncludeResponse()
    {
        
    }
    
    public ListWithIncludeResponse(IList<TResponse> entities, PaginatorResponse paginator, long totalCount)
    {
        Entities = entities;
        Paginator = paginator;
        TotalCount = totalCount;
    }

    public IList<TResponse> Entities { get; set; }
    public PaginatorResponse Paginator { get; set; }
    public long TotalCount { get; set; }
}