using Contacts.Models.Responses.Base;
using GraphQL.Types;

namespace Contacts.GraphQL.Models;

public class ListWithIncludeType<TResponse, TType> : ObjectGraphType<ListWithIncludeResponse<TResponse>>
    where TResponse : class
    where TType : ObjectGraphType<TResponse>, new()
{
    public ListWithIncludeType()
    {
        Field<ListGraphType<TType>>("entities", resolve: context => context.Source.Entities);
        Field<PaginatorType>("paginator", resolve: context => context.Source.Paginator);
        Field(x => x.TotalCount);
    }
}