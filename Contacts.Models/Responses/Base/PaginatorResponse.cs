using Contacts.Models.Common;

namespace Contacts.Models.Responses.Base;

public class PaginatorResponse: PaginatorModel
{
    public PaginatorResponse(int pageNumber, int pageSize, bool isFull) : base(pageNumber, pageSize, isFull)
    {
    }
}