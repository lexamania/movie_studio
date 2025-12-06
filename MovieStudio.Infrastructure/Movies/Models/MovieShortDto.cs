namespace MovieStudio.Infrastructure.Movies.Models;

public record MovieShortDto(
    string Id,
    string Title,
    string Description,
    string ImageLink
);