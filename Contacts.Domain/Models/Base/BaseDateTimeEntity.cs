namespace Contacts.Domain.Models.Base;

public class BaseDateTimeEntity<TId>: BaseIdEntity<TId>
{
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
