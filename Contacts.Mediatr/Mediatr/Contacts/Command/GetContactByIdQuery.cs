using Contacts.Domain.Models.Contacts;
using Contacts.Mediatr.Mediatr.Base;
using Contacts.Models.Responses.Base;
using Contacts.Models.Responses.Contacts;
using MediatR;

namespace Contacts.Mediatr.Mediatr.Contacts.Command;

public class GetContactByIdQuery: BaseIdQuery<int>, IRequest<ContactResponse>
{
    public GetContactByIdQuery(int id)
    {
        Id = id;
    }   
}