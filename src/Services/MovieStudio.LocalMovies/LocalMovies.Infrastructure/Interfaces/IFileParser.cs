namespace LocalMovies.Infrastructure.Interfaces;

public interface IFileParser
{
    /// <summary> format: .{ext} </summary>
    string Extension { get; }

    void CreateFile<T>(string filePath);
    List<T> ParseFile<T>(string filePath);
    List<T> ParseFile<T>(string filePath, int offset, int count);
    void UpdateFile<T>(string filePath, IEnumerable<T> objs);
    void RemoveFile(string filePath);


    void SaveRecords<T>(string filePath, IEnumerable<T> objs);
    bool ContainsRecord<T>(string filePath, T obj);
    /// <typeparam name="T">should implement default equality comparer</typeparam>
    void DeleteRecords<T>(string filePath, IEnumerable<T> objs);
}
