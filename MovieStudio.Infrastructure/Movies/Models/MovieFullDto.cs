namespace MovieStudio.Infrastructure.Movies.Models;

public class MovieFullDto(
    string Id,
    string Title,
    string Description,
    string ImageLink,
    int Score,
    List<VideoQualityDto> VideoLinks
);