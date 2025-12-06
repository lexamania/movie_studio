namespace MovieStudio.Infrastructure.Pagination.Models;

public class PageModel(int page, int count, string? lastItem = null)
{
    public int Page { get; } = page;
    public int Count { get; } = count;
    public string? LastItem { get; } = lastItem;
}