namespace LocalMovies.Infrastructure.DirectoryData.Models;

public class DirectoryEntity
{
    public string Id { get; set; }
    public string Path { get; set; }
    public string Caption { get; set; }
    public bool IncludeInner { get; set; }

    public DirectoryEntity(string path, string caption, bool includeInner)
    {
        Id = Guid.NewGuid().ToString();
        Path = path;
        Caption = caption;
        IncludeInner = includeInner;
    }

    public DirectoryEntity() { }

    public override bool Equals(object obj)
    {
        if (obj == null || obj is not DirectoryEntity typedObj)
            return false;
        
        return typedObj.Id == Id;
    }
    
    public override int GetHashCode()
        => Path.GetHashCode();
}
