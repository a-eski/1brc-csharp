namespace _1brc_csharp_implementations.Common;

public static class FilePathGetter
{
    private const string WeatherDataFile = "/data/measurements_1_000_000.txt";
    
    //Naive
    public static string GetFilePath()
    {
        var currentDirectory = Environment.CurrentDirectory;
        var projectDirectory = Directory.GetParent(currentDirectory)?.Parent?.Parent?.FullName;
        if (projectDirectory == null) throw new DirectoryNotFoundException($"Could not find project directory for {currentDirectory}");
        return $"{projectDirectory}/..{WeatherDataFile}";
    }

    //Simplified
    public static string GetPath() => $"{Environment.CurrentDirectory}/../../../..{WeatherDataFile}";
}