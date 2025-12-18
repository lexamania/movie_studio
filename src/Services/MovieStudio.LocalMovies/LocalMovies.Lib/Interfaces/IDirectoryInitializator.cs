namespace LocalMoviesService.Interfaces;

public interface IDirectoryInitializator
{
    public void AddDirectoryWithVideo(string directoryPath);
    public void RemoveDirectoryWithVideo(string directoryPath);
}
