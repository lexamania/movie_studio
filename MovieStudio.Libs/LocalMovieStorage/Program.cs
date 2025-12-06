using System;

using LocalMovieStorage.Services;

var storage = new StorageService();
storage.AddDirectory(@"C:\Lexamania\Videos\Records", true);

var directories = storage.GetDirectories();
Console.WriteLine($"Directories:");
foreach (var dir in directories)
    Console.WriteLine($" - {dir}");

var files = storage.GetAllVideoFiles();
Console.WriteLine($"\nVideos:");
foreach (var file in files)
    Console.WriteLine($" - {file}");