namespace LocalMovies.Infrastructure.DirectoryData.Models;

public record DirectoryInfoModel(DirectoryEntity Directory)
{
    public List<string> Files { get; init; } = [];
}
