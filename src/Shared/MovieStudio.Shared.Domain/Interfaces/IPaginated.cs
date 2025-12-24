using MovieStudio.Shared.Domain.Models;

namespace MovieStudio.Shared.Domain.Interfaces;

public interface IPaginated
{
    PaginationModel Pagination { get; }
}
