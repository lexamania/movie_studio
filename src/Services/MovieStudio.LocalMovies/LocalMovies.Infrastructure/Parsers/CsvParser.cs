using System.Globalization;

using CsvHelper;
using CsvHelper.Configuration;

using LocalMovies.Infrastructure.Interfaces;

namespace LocalMovies.Infrastructure.Parsers;

public class CsvParser : IFileParser
{
    private readonly CultureInfo _culture = CultureInfo.InvariantCulture;

    public string Extension { get; } = ".csv";

    public void CreateFile<T>(string filePath)
    {
        using var stream = new StreamWriter(filePath);
        using var csvStream = new CsvWriter(stream, _culture);
        csvStream.WriteRecords<T>([]);
    }

    public List<T> ParseFile<T>(string filePath)
    {
        using var stream = new StreamReader(filePath);
        using var csvStream = new CsvReader(stream, _culture);
        return [.. csvStream.GetRecords<T>()];
    }

    public List<T> ParseFile<T>(string filePath, int offset, int count)
    {
        using var stream = new StreamReader(filePath);
        using var csvStream = new CsvReader(stream, _culture);
        return [.. csvStream.GetRecords<T>().Skip(offset).Take(count)];
    }

    public void UpdateFile<T>(string filePath, IEnumerable<T> objs)
    {
        using var stream = new StreamWriter(filePath);
        using var csvStream = new CsvWriter(stream, _culture);
        csvStream.WriteRecords(objs);
    }

    public void RemoveFile(string filePath)
    {
        if (Path.Exists(filePath) && Path.HasExtension(filePath))
            File.Delete(filePath);
    }

    public void SaveRecords<T>(string filePath, IEnumerable<T> objs)
    {
        var conf = new CsvConfiguration(_culture)
        {
            HasHeaderRecord = false,
        };
        using var stream = new StreamWriter(filePath, true);
        using var csvStream = new CsvWriter(stream, conf);
        csvStream.WriteRecords(objs);
    }

    public bool ContainsRecord<T>(string filePath, T obj)
    {
        using var stream = new StreamReader(filePath);
        using var csvStream = new CsvReader(stream, _culture);

        foreach(var record in csvStream.GetRecords<T>())
        {
            if (obj.Equals(record))
                return true;
        }

        return false;
    }

    public void DeleteRecords<T>(string filePath, IEnumerable<T> objs)
    {
        var tempPath = Path.Join(Path.GetDirectoryName(filePath), $"{Path.GetRandomFileName()}{Extension}");

        using var streamR = new StreamReader(filePath);
        using var csvStreamR = new CsvReader(streamR, _culture);
        using var streamW = new StreamWriter(tempPath);
        using var csvStreamW = new CsvWriter(streamW, _culture);

        csvStreamW.WriteHeader<T>();
        csvStreamW.NextRecord();

        foreach(var record in csvStreamR.GetRecords<T>())
        {
            if (objs.Contains(record))
                continue;

            csvStreamW.WriteRecord(record);
            csvStreamW.NextRecord();
        }

        File.Delete(filePath);
        File.Move(tempPath, filePath);
    }
}
