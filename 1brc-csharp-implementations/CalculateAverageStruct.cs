﻿using System.Text;
using _1brc_csharp_implementations.Common;
using _1brc_csharp_implementations.Models;

namespace _1brc_csharp_implementations;

/// <summary>
/// Iterating from CalculateAverageNaive, continue trying to reduce allocations and duration without splitting work asynchronously and without using multiple threads.
/// </summary>
public static class CalculateAverageStruct
{
    public static void Run()
    {
        var filePath = FilePathGetter.GetFilePath();
        
        var dictionary = new Dictionary<string, WeatherData>();// array is length 4. count, min, max, total. mean calculated at end, to avoid unnecessary division operations.
        foreach (var line in File.ReadLines(filePath))
        {
            ProcessLine(line, dictionary);
        }

        var sb = new StringBuilder("{");
        var index = 0;
        foreach (var weatherStation in dictionary.OrderBy(x => x.Key))
        {
            sb.Append(weatherStation.Key).Append('=')
                .Append(weatherStation.Value.Min).Append(',')
                .Append(weatherStation.Value.Max).Append(',')
                .Append(weatherStation.Value.Total / weatherStation.Value.Count);
            
            if (++index < dictionary.Count) sb.Append(',');
        }
        sb.Append('}');

        Console.WriteLine(sb.ToString());
    }

    private static void ProcessLine(string line, Dictionary<string, WeatherData> dictionary)
    {
        var lineSpan = line.AsSpan();
        if (lineSpan[0] == '#') return;
        var semicolonIndex = lineSpan.IndexOf(';');
        var weatherStationName = new string(lineSpan[..semicolonIndex]);
        var newValue = float.Parse(lineSpan[(semicolonIndex + 1)..]);

        if (!dictionary.TryGetValue(weatherStationName, out var values) || values.Count == 0)
        {
            dictionary.Add(weatherStationName, new WeatherData { Count = 1, Min = newValue, Max = newValue, Total = newValue });
            return;
        }
        
        values.Count++;//increment count
        if (newValue < values.Min) values.Min = newValue;//track min
        if (newValue > values.Max) values.Max = newValue;//track max
        values.Total += newValue;//track total
    }
}