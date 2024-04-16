﻿namespace _1brc_csharp_implementations.Common;

public static class FilePathGetter
{
    private const string WeatherDataFile = "/data/weather_stations.csv";
    private const string NetVersion = "net8.0";
    public static string GetFilePath()
    {
        var currentDirectory = Environment.CurrentDirectory;
        var projectDirectory = Directory.GetParent(currentDirectory)?.Parent?.Parent?.FullName;
        if (projectDirectory?.Contains(NetVersion) is true)
        {
            var index = projectDirectory.IndexOf(NetVersion);
            if (index >= 0) projectDirectory = projectDirectory[..(index + NetVersion.Length)];
        } 
        if (projectDirectory == null) throw new DirectoryNotFoundException($"Could not find project directory for {currentDirectory}");
        return $"{projectDirectory}{WeatherDataFile}";
    }

    public static string GetPath() => $"{Environment.CurrentDirectory}{WeatherDataFile}";
}