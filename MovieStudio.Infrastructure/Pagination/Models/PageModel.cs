namespace MovieStudio.Infrastructure.Pagination.Models;

public class PageModel
{
    public int Count { get; set; }
    public int Page { get; set; }
    public string? LastItem { get; set; }
}