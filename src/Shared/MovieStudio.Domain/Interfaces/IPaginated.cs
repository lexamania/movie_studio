using MovieStudio.Domain.Models;

namespace MovieStudio.Domain.Interfaces;

public interface IPaginated
{
    PaginationModel Pagination { get; }
}
