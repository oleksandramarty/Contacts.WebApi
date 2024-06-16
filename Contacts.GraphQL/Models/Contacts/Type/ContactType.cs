using Contacts.Models.Responses.Contacts;
using GraphQL.Types;

namespace Contacts.GraphQL.Models.Contacts.Type;

public class ContactType: ObjectGraphType<ContactResponse>
{
    public ContactType()
    {
        Field(x => x.Id);
        Field(x => x.FirstName);
        Field(x => x.LastName);
        Field(x => x.Email);
        Field(x => x.Phone);
        Field(x => x.Title);
        Field(x => x.MiddleInitial, nullable: true);
    }
}