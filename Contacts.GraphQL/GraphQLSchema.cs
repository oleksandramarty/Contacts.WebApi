using Contacts.GraphQL.MutationResolver;
using Contacts.GraphQL.QueryResolver;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.GraphQL;

public class GraphQLSchema: Schema
{
    public GraphQLSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<RootQuery>();
        Mutation = serviceProvider.GetRequiredService<RootMutation>();
    }
}