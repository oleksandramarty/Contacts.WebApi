using Contacts.Models.Common;

namespace Contacts.Models.Requests.Base;

public class PaginatorRequest: PaginatorModel
{
    public PaginatorRequest(int pageNumber, int pageSize, bool isFull) : base(pageNumber, pageSize, isFull)
    {
    }
}