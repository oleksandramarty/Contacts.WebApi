using Contacts.Domain.Models.Contacts;
using Contacts.GraphQL.Models;
using Contacts.GraphQL.Models.Contacts.Type;
using Contacts.Mediatr.Mediatr.Contacts.Command;
using Contacts.Models.Requests.Base;
using Contacts.Models.Responses.Base;
using Contacts.Models.Responses.Contacts;
using GraphQL;
using GraphQL.Types;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ContactResponse = Contacts.Models.Responses.Contacts.ContactResponse;

namespace Contacts.GraphQL.QueryResolver;

public class GraphQLQueryResolver: ObjectGraphType, IGraphQLQueryResolver
{
    public void GetContactById()
    {
        Field<ContactType>("contact")
            .Arguments(new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }))
            .ResolveAsync(async context =>
            {
                try
                {
                    var mediator = context.RequestServices.GetRequiredService<IMediator>();
                    var cancellationToken = context.CancellationToken;
                    ContactResponse result = 
                        await mediator.Send(
                            new GetContactByIdQuery(context.GetArgument<int>("id", default)), 
                            cancellationToken);
                    return result;
                }
                catch (Exception e)
                {
                    throw new ExecutionError(e.Message);
                }
            });
    }
    
    public void GetFilteredContacts()
    {
        Field<ListWithIncludeType<ContactResponse, ContactType>>("contacts")
            .Arguments(new QueryArguments(GetPageableQueryArguments()))
            .ResolveAsync(async context => await this.GetPageableResponse<ContactResponse, GetFilteredContactsQuery>(context, new GetFilteredContactsQuery(this.GetBaseFilterRequest(context))));
    }

    private async Task<ListWithIncludeResponse<T>> GetPageableResponse<T, TQuery>(IResolveFieldContext<object?> context, TQuery query)
        where TQuery : BaseFilterRequest, IRequest<ListWithIncludeResponse<T>>
    {
        try
        {
            var cancellationToken = context.CancellationToken;
            var mediator = context.RequestServices.GetRequiredService<IMediator>();
            var result = await mediator.Send(query, cancellationToken);
            return result;
        }
        catch (Exception e)
        {
            throw new ExecutionError(e.Message);
        }
    }
    
    private IEnumerable<QueryArgument> GetPageableQueryArguments()
    {
        return new QueryArguments(
            
            new QueryArgument<StringGraphType> { Name = "query" },
            new QueryArgument<BooleanGraphType> { Name = "isFull" },
            new QueryArgument<IntGraphType> { Name = "pageNumber" },
            new QueryArgument<IntGraphType> { Name = "pageSize" },
            new QueryArgument<StringGraphType> { Name = "column" },
            new QueryArgument<StringGraphType> { Name = "direction" });
    }
    
    private BaseFilterRequest GetBaseFilterRequest(IResolveFieldContext<object?> context)
    {
        BaseFilterRequest query = new BaseFilterRequest();
        query.Paginator = new PaginatorRequest(
            context.GetArgument<int?>("pageNumber") ?? 1,
            context.GetArgument<int?>("pageSize") ?? 5,
            context.GetArgument<bool?>("isFull") ?? false
        );
        query.Sort = new BaseSortableRequest(
            context.GetArgument<string?>("column") ?? "Id",
            context.GetArgument<string?>("direction") ?? "desc");

        query.Query = context.GetArgument<string?>("query") ?? null;

        return query;
    }
}