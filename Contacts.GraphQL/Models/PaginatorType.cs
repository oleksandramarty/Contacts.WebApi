using Contacts.Models.Responses.Base;
using GraphQL.Types;

namespace Contacts.GraphQL.Models;

public class PaginatorType : ObjectGraphType<PaginatorResponse>
{
    public PaginatorType()
    {
        Field(x => x.PageNumber);
        Field(x => x.PageSize);
        Field(x => x.IsFull);
    }
}