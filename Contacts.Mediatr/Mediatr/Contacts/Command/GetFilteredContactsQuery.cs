using Contacts.Models.Requests.Base;
using Contacts.Models.Responses.Base;
using Contacts.Models.Responses.Contacts;
using MediatR;

namespace Contacts.Mediatr.Mediatr.Contacts.Command;

public class GetFilteredContactsQuery: BaseFilterRequest, IRequest<ListWithIncludeResponse<ContactResponse>>
{
    public GetFilteredContactsQuery()
    {
        
    }
    public GetFilteredContactsQuery(BaseFilterRequest query) : base(query)
    {
    }
}