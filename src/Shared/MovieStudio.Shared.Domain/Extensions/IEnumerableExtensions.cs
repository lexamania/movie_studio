using MovieStudio.Shared.Domain.Models;

namespace MovieStudio.Shared.Domain.Extensions;

public static class IEnumerableExtensions
{
    public static IEnumerable<T> WithPagination<T>(this IEnumerable<T> collection, PaginationModel pagination)
    {
        var pageSize = pagination.PageSize <= 0
            ? PaginationModel.NORMAL_PAGE_SIZE
            : Math.Min(pagination.PageSize, PaginationModel.MAX_PAGE_SIZE);
        var offset = pagination.PageNumber <= 1
            ? 0
            : (pagination.PageNumber - 1) * pageSize;
        return collection.Skip(offset).Take(pageSize);
    }
}
