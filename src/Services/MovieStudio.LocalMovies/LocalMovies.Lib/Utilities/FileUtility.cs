using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using CsvHelper;
using CsvHelper.Configuration;

using LocalMoviesService.Models;

namespace LocalMoviesService.Utilities;

public static class FileUtility
{
    public static void InitializeCsvFile<T>(string filePath)
    {
        using var stream = new StreamWriter(filePath);
        using var csvStream = new CsvWriter(stream, CultureInfo.InvariantCulture);
        csvStream.WriteRecords<T>([]);
    }

    public static void SaveRecordsToCsvFile<T>(string filePath, IEnumerable<T> objs)
    {
        using var stream = new StreamWriter(filePath);
        using var csvStream = new CsvWriter(stream, CultureInfo.InvariantCulture);
        csvStream.WriteRecords(objs);
    }

    public static void AddRecordToCsvFile<T>(string filePath, T obj)
    {
        var conf = new CsvConfiguration(CultureInfo.InstalledUICulture)
        {
            HasHeaderRecord = false
        };
        using var stream = new StreamWriter(filePath, true);
        using var csvStream = new CsvWriter(stream, conf);
        csvStream.WriteRecords([obj]);
    }

    public static List<T> ParseCsvFile<T>(string filePath)
    {
        using var stream = new StreamReader(filePath);
        using var csvStream = new CsvReader(stream, CultureInfo.InvariantCulture);
        return [.. csvStream.GetRecords<T>()];
    }

    public static bool IsSupportedFile(string filePath, string[] supportedFormats)
    {
        var ext = Path.GetExtension(filePath);
        return supportedFormats.Contains(ext);
    }

    public static IEnumerable<string> ReadDirectory(SavedDirectory directory)
    {
        yield return directory.DirectoryPath;

        if (directory.WithInnerDirs)
        {
            foreach (var dir in Directory.GetDirectories(directory.DirectoryPath, "*", SearchOption.AllDirectories))
                yield return dir;
        }
    }
}
