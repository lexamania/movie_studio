using LocalMovies.Infrastructure.Configurations;
using LocalMovies.Infrastructure.DirectoryData.Models;
using LocalMovies.Infrastructure.Interfaces;

namespace LocalMovies.Infrastructure.Services;

public class StorageService
{
    private const string FILE_NAME = "saved_directories";

    private readonly IFileParser _parser;
    private readonly string _filePath;
    private readonly string[] _supportedExtensions;
    private readonly List<DirectoryInfoModel> _directories = [];

    public StorageService(IFileParser parser, MovieStorageSettings settings)
    {
        _parser = parser;
        _filePath = Path.Join(settings.WorkingDirectory, $"{FILE_NAME}{parser.Extension}");
        _supportedExtensions = settings.SupportedExtensions;

        UpdateDirectories();
    }

    public void UpdateDirectories()
    {
        _directories.Clear();

        if (!File.Exists(_filePath))
        {
            _parser.CreateFile<DirectoryEntity>(_filePath);
            return;
        }

        var dirs = _parser.ParseFile<DirectoryEntity>(_filePath);
        _directories.AddRange(dirs.Select(GetDirectoryInfo));
    }

    public IReadOnlyCollection<DirectoryInfoModel> GetDirectories()
        => _directories.AsReadOnly();

    public void AddDirectory(string dirPath, string caption, bool includeInner)
    {
        if (_directories.Any(x => x.Directory.Path.Equals(dirPath)))
            return;

        var dir = new DirectoryEntity(dirPath, caption, includeInner);
        _parser.SaveRecords(_filePath, [dir]);
        _directories.Add(GetDirectoryInfo(dir));
    }

    public void RemoveDirectory(string id)
    {
        var dirInfo = _directories.FirstOrDefault(x => x.Directory.Id.Equals(id));
        if (dirInfo is null)
            return;

        _parser.DeleteRecords(_filePath, [dirInfo.Directory]);
        _directories.Remove(dirInfo);
    }

    private DirectoryInfoModel GetDirectoryInfo(DirectoryEntity dir)
    {
        var searchOption = dir.IncludeInner
            ? SearchOption.AllDirectories
            : SearchOption.TopDirectoryOnly;

        var tasks = new Task<string[]>[_supportedExtensions.Length];
        for(int i = 0; i < _supportedExtensions.Length; ++i)
        {
            var ext = _supportedExtensions[i];
            tasks[i] = Task.Run(() => Directory.GetFiles(dir.Path, $"*{ext}", searchOption));
        }
        Task.WaitAll(tasks);

        var files = tasks.SelectMany(x => x.Result).ToList();
        return new(dir) { Files = files };
    }
}
