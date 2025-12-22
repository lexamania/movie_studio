namespace LocalMovies.Api.Endpoints.Models;

public class NewDirectoryModel
{
    public string DirectoryPath { get; set; } 
    public string Caption { get; set; } 
    public bool IncludeInner { get; set; }
}
