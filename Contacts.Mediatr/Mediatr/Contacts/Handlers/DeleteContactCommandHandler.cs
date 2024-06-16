using Contacts.Domain.Models.Contacts;
using Contacts.Interfaces;
using Contacts.Mediatr.Mediatr.Contacts.Command;
using MediatR;

namespace Contacts.Mediatr.Mediatr.Contacts.Handlers;

public class DeleteContactCommandHandler: IRequestHandler<DeleteContactCommand>
{
    private readonly IGenericRepository<Contact> contactRepository;
    private readonly IEntityValidator entityValidator;
    
    public DeleteContactCommandHandler(
        IGenericRepository<Contact> contactRepository, 
        IEntityValidator entityValidator)
    {
        this.contactRepository = contactRepository;
        this.entityValidator = entityValidator;
    }
    
    public async Task Handle(DeleteContactCommand command, CancellationToken cancellationToken)
    {
        Contact contact = await this.contactRepository.GetByIdAsync(command.Id, cancellationToken);

        this.entityValidator.ValidateExist<Contact, int>(contact, command.Id);
        
        await this.contactRepository.DeleteAsync<int>(command.Id, cancellationToken);
    }
}