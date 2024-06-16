using AutoMapper;
using Contacts.Domain.Models.Contacts;
using Contacts.Interfaces;
using Contacts.Kernel.Extensions;
using Contacts.Mediatr.Mediatr.Contacts.Command;
using Contacts.Models.Responses.Base;
using Contacts.Models.Responses.Contacts;
using MediatR;

namespace Contacts.Mediatr.Mediatr.Contacts.Handlers;

public class GetFilteredContactsQueryHandler: IRequestHandler<GetFilteredContactsQuery, ListWithIncludeResponse<ContactResponse>>
{
    private readonly IReadGenericRepository<Contact> contactRepository;
    
    public GetFilteredContactsQueryHandler(IReadGenericRepository<Contact> contactRepository)
    {
        this.contactRepository = contactRepository;
    }

    public async Task<ListWithIncludeResponse<ContactResponse>> Handle(GetFilteredContactsQuery query,
        CancellationToken cancellationToken)
    {
        return await this.contactRepository.GetListWithIncludeAsync<ContactResponse>(
            query.Query.NotNullOrEmpty() ? c =>
                    (c.FirstName.ToLower().Contains(query.Query.ToLower())) ||
                    (c.LastName.ToLower().Contains(query.Query.ToLower())) ||
                    (c.Phone.ToLower().Contains(query.Query.ToLower())) ||
                    (c.Email.ToLower().Contains(query.Query.ToLower())) ||
                    (c.Title.ToLower().Contains(query.Query.ToLower())) ||
                    (c.MiddleInitial != null && c.MiddleInitial.ToLower().Contains(query.Query.ToLower()))
                : null,
            query,
            cancellationToken,
            null
        );
    }
}