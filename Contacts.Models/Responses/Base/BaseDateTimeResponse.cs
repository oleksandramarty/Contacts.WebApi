namespace Contacts.Models.Responses.Base;

public class BaseDateTimeResponse<TId> : BaseIdEntityResponse<TId>
{
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}