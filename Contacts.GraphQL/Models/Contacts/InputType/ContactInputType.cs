using GraphQL.Types;

namespace Contacts.GraphQL.Models.Contacts.InputType;

public class ContactInputType: InputObjectGraphType
{
    public ContactInputType()
    {
        Name = "ContactInputType";
        Field<NonNullGraphType<StringGraphType>>("firstName");
        Field<NonNullGraphType<StringGraphType>>("lastName");
        Field<NonNullGraphType<StringGraphType>>("email");
        Field<NonNullGraphType<StringGraphType>>("phone");
        Field<NonNullGraphType<StringGraphType>>("title");
        Field<StringGraphType>("middleInitial");
    }
}