namespace LocalMovieStorage.Models;

public class SavedDirectory
{
    public string DirectoryPath { get; set; }

    public bool WithInnerDirs { get; set; }

    public SavedDirectory(string directoryPath, bool withInnerDirs)
    {
        DirectoryPath = directoryPath;
        WithInnerDirs = withInnerDirs;
    }

    public SavedDirectory() { }
}
