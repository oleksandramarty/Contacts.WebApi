using AutoMapper;
using Contacts.Constants.Errors;
using Contacts.Domain.Models.Contacts;
using Contacts.Interfaces;
using Contacts.Mediatr.Mediatr.Contacts.Command;
using Contacts.Mediatr.Validators.Contacts;
using Contacts.Models.Responses.Contacts;
using MediatR;

namespace Contacts.Mediatr.Mediatr.Contacts.Handlers;

public class CreateOrUpdateContactCommandHandler: IRequestHandler<CreateOrUpdateContactCommand, ContactResponse>
{
    private readonly IMapper mapper;
    private readonly IEntityValidator entityValidator;
    private readonly IGenericRepository<Contact> contactRepository;

    public CreateOrUpdateContactCommandHandler(
        IMapper mapper, 
        IEntityValidator entityValidator, 
        IGenericRepository<Contact> contactRepository)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.contactRepository = contactRepository;
    }

    public async Task<ContactResponse> Handle(CreateOrUpdateContactCommand command, CancellationToken cancellationToken)
    {
        this.entityValidator.ValidateRequest<CreateOrUpdateContactCommand, ContactResponse>(command, () => new CreateOrUpdateContactCommandValidator(!command.Id.HasValue));
        
        await this.entityValidator.ValidateExistParamAsync<Contact>(u => (command.Id.HasValue && u.Id != command.Id.Value || !command.Id.HasValue) && u.Email == command.Email, String.Format(ErrorMessages.UserWithParamExist, "Email"), cancellationToken);
        await this.entityValidator.ValidateExistParamAsync<Contact>(u => (command.Id.HasValue && u.Id != command.Id.Value || !command.Id.HasValue) && u.Phone == command.Phone, String.Format(ErrorMessages.UserWithParamExist, "Phone"), cancellationToken);

        Contact contact;
        
        if (command.Id.HasValue)
        {
            contact = await this.contactRepository.GetByIdAsync(command.Id.Value, cancellationToken);
            
            this.entityValidator.ValidateExist<Contact, int>(contact, command.Id.Value);

            Contact updateContact = this.mapper.Map<CreateOrUpdateContactCommand, Contact>(command, contact, opt => opt.Items["Id"] = command.Id);
            
            await this.contactRepository.UpdateAsync(updateContact, cancellationToken);

            return this.mapper.Map<Contact, ContactResponse>(updateContact);
        }

        contact = this.mapper.Map<CreateOrUpdateContactCommand, Contact>(command, opt => opt.Items["Id"] = command.Id);
        
        await this.contactRepository.AddAsync(contact, cancellationToken);
        
        return this.mapper.Map<Contact, ContactResponse>(contact);
    }
}
