namespace Contacts.Models.Common;

public class PaginatorModel
{
    public PaginatorModel(int pageNumber, int pageSize, bool isFull)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        IsFull = isFull;
    }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public bool IsFull { get; set; } = false;
}