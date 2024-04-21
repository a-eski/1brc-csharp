using System.Text;
using _1brc_csharp_implementations.Common;
using _1brc_csharp_implementations.Models;

namespace _1brc_csharp_implementations;

/// <summary>
/// Iterating from CalculateAverageNaive, continue trying to reduce allocations and duration without splitting work asynchronously and without using multiple threads.
/// </summary>
public static class CalculateAverageStructFasterConsole
{
    public static void Run()
    {
        var filePath = FilePathGetter.GetPath();
        
        var dictionary = new Dictionary<string, WeatherValues>();
        foreach (var line in File.ReadLines(filePath))
        {
            var lineSpan = line.AsSpan();
            var semicolonIndex = lineSpan.IndexOf(';');
            var weatherStationName = new string(lineSpan[..semicolonIndex]);
            var newValue = float.Parse(lineSpan[(semicolonIndex + 1)..]);

            if (!dictionary.TryGetValue(weatherStationName, out var values) || values.Count == 0)
            {
                dictionary.Add(weatherStationName, WeatherValues.GetNew(newValue));
                return;
            }
            
            values.Apply(newValue);
        }
        
        var sb = new StringBuilder("{");
        var index = 0;
        foreach (var weatherStation in dictionary.OrderBy(x => x.Key))
        {
            sb.Append(weatherStation.Key).Append('=')
                .Append(Math.Round(weatherStation.Value.Min, 1, MidpointRounding.ToZero)).Append(',')
                .Append(Math.Round(weatherStation.Value.Max, 1, MidpointRounding.ToZero)).Append(',')
                .Append(Math.Round(weatherStation.Value.Total / weatherStation.Value.Count, 1, MidpointRounding.ToZero));

            if (++index < dictionary.Count) sb.Append(", ");
        }
        sb.Append('}');

        var result = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(sb.ToString()); 
        Console.OutputEncoding = Encoding.UTF8;
        using var standardOutput = Console.OpenStandardOutput();
        standardOutput.Write(result);
    }
}
