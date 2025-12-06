using System.Collections.Generic;

namespace LocalMovieStorage.Models;

public class DirectoryModel
{
    public string Name { get; set; }
    public string Path { get; set; }
    public List<DirectoryModel> ChildrenDirectories { get; } = [];
    public List<FileModel> ChildrenFiles { get; } = [];
}
