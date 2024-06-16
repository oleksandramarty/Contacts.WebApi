using Contacts.Models.Responses.Base;

namespace Contacts.Models.Responses.Contacts;

public class ContactResponse: BaseDateTimeResponse<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Title { get; set; }
    public string? MiddleInitial { get; set; }
}