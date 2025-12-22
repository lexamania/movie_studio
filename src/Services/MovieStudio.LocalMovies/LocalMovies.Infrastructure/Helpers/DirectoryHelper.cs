namespace LocalMovies.Infrastructure.Helpers;

public static class DirectoryHelper
{
    public static void CreateDirectoryIfNeed(string path)
    {
        if (Path.Exists(path) || path is null)
            return;

        CreateDirectoryIfNeed(Path.GetDirectoryName(path)!);
        Directory.CreateDirectory(path);
    }
}
