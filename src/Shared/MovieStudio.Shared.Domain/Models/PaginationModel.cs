namespace MovieStudio.Shared.Domain.Models;

public class PaginationModel
{
    public const int MAX_PAGE_SIZE = 100;
    public const int NORMAL_PAGE_SIZE = 20;

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
