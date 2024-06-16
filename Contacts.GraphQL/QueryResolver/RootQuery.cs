namespace Contacts.GraphQL.QueryResolver;

public class RootQuery: GraphQLQueryResolver
{
    public RootQuery()
    {
        this.GetContactById();
        this.GetFilteredContacts();
    }
}  