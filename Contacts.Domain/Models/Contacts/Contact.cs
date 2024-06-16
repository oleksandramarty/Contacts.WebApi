using Contacts.Domain.Models.Base;

namespace Contacts.Domain.Models.Contacts;

public class Contact: BaseDateTimeEntity<int>
{
    public Contact()
    {
        
    }
    
    public Contact(string firstName, string lastName, string email, string phone, string title, string? middleInitial)
    {
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Phone = phone ?? throw new ArgumentNullException(nameof(phone));
        Title = title ?? throw new ArgumentNullException(nameof(title));
        MiddleInitial = middleInitial;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Title { get; set; }
    public string? MiddleInitial { get; set; }
}