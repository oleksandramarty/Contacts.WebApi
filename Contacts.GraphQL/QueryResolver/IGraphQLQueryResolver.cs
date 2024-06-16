namespace Contacts.GraphQL.QueryResolver;

public interface IGraphQLQueryResolver
{
    void GetContactById();
    void GetFilteredContacts();
}