using Contacts.Domain.Models.Contacts;
using Contacts.Mediatr.Mediatr.Base;
using Contacts.Models.Responses.Base;
using Contacts.Models.Responses.Contacts;
using MediatR;

namespace Contacts.Mediatr.Mediatr.Contacts.Command;

public class CreateOrUpdateContactCommand: BaseIdCommand<int?>, IRequest<ContactResponse>
{
    public CreateOrUpdateContactCommand()
    {
        
    }
    public CreateOrUpdateContactCommand(int? id, string firstName, string lastName, string email, string phone, string title, string? middleInitial)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Title = title;
        MiddleInitial = middleInitial;
    }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Title { get; set; }
    public string? MiddleInitial { get; set; }
}