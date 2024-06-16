using Contacts.Domain.Models.Contacts;
using Contacts.GraphQL.Models.Contacts.InputType;
using Contacts.GraphQL.Models.Contacts.Type;
using Contacts.Mediatr.Mediatr.Contacts.Command;
using Contacts.Models.Responses.Contacts;
using GraphQL.Types;

namespace Contacts.GraphQL.MutationResolver;

public class RootMutation: GraphQLMutationResolver
{
    public RootMutation()
    {
        Name = "Mutation";
        
        this.CreateEntity<ContactType, ContactInputType, CreateOrUpdateContactCommand, ContactResponse, int, IntGraphType>("createContact");
        this.UpdateEntity<ContactType, ContactInputType, CreateOrUpdateContactCommand, ContactResponse, int, IntGraphType>("updateContact");
        this.DeleteEntity<DeleteContactCommand, int, IntGraphType>("deleteContact");
    }
}