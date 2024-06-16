using AutoMapper;
using Contacts.Domain.Models.Contacts;
using Contacts.Interfaces;
using Contacts.Mediatr.Mediatr.Contacts.Command;
using Contacts.Models.Responses.Base;
using Contacts.Models.Responses.Contacts;
using MediatR;

namespace Contacts.Mediatr.Mediatr.Contacts.Handlers;

public class GetContactByIdQueryHandler: IRequestHandler<GetContactByIdQuery, ContactResponse>
{
    private readonly IMapper mapper;
    private readonly IReadGenericRepository<Contact> contactRepository;
    private readonly IEntityValidator entityValidator;
    
    public GetContactByIdQueryHandler(
        IMapper mapper,
        IReadGenericRepository<Contact> contactRepository, 
        IEntityValidator entityValidator)
    {
        this.mapper = mapper;
        this.contactRepository = contactRepository;
        this.entityValidator = entityValidator;
    }

    public async Task<ContactResponse> Handle(GetContactByIdQuery query,
        CancellationToken cancellationToken)
    {
        Contact contact = await this.contactRepository.GetByIdAsync(query.Id, cancellationToken);
        
        this.entityValidator.ValidateExist<Contact, int>(contact, query.Id);
        
        return this.mapper.Map<Contact, ContactResponse>(contact);
    }
}