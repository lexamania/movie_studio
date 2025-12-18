using System.Collections.Generic;
using System.IO;
using System.Linq;

using LocalMoviesService.Data;
using LocalMoviesService.Models;
using LocalMoviesService.Utilities;

namespace LocalMoviesService.Services;

public class StorageService
{
    private List<string> _directories;
    private static string SavingPath => VideoData.SavingPath;

    public StorageService()
    {
        UpdateDirectories();
    }

    public void AddDirectory(string dirPath, bool withInnerDirs)
    {
        if (_directories.Contains(dirPath))
            return;

        FileUtility.AddRecordToCsvFile(SavingPath, new SavedDirectory(dirPath, withInnerDirs));
        UpdateDirectories();
    }

    public string[] GetDirectories()
        => [.. _directories];

    public string[] GetAllVideoFiles()
    {
        var files = _directories.SelectMany(x =>
                Directory.GetFiles(x)
                .Where(x => FileUtility.IsSupportedFile(x, VideoData.SupportedFormats)));
        return [.. files];
    }

    private void UpdateDirectories()
    {
        var filePath = SavingPath;

        if (!File.Exists(filePath))
        {
            FileUtility.InitializeCsvFile<SavedDirectory>(filePath);
        }

        _directories = GetAllDirectories(filePath);
    }

    private static List<string> GetAllDirectories(string filePath)
    {
        var baseDirs = FileUtility.ParseCsvFile<SavedDirectory>(filePath);
        var result = baseDirs.SelectMany(x => FileUtility.ReadDirectory(x)).ToList();
        return result;
    }
}
