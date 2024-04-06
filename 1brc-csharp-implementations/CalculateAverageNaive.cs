using System.IO;
using System.IO.MemoryMappedFiles;

namespace _1brc_csharp_implementations;

public static class CalculateAverageNaive
{
    private const string WeatherDataFile = "/data/weather_stations.csv";
    public static void Run()
    {
        var currentDirectory = Environment.CurrentDirectory;
        var projectDirectory = Directory.GetParent(currentDirectory)?.Parent?.Parent?.FullName;
        if (projectDirectory == null)
            throw new DirectoryNotFoundException($"Could not find project directory for {currentDirectory}");
        var filePath = $"{projectDirectory}{WeatherDataFile}";

        foreach (var line in File.ReadLines(filePath))
        {
            ProcessLine(line);
            //ThreadPool.QueueUserWorkItem(ProcessLine, line);
        }
    }

    private static void ProcessLine(string line)
    {
        Console.WriteLine(line);
    }

    private static void ProcessLine(object? line)
    {
        Console.WriteLine(line?.ToString());
    }
    
    // await using var fileStream = File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read); //look at if different options for file.Open affect performance, look at if memoryMappedFiles improves performance
    // var cancellationToken = new CancellationToken();
    // var buffer = new Memory<byte>();
    // var bytes = await fileStream.ReadAsync(buffer, cancellationToken);
}