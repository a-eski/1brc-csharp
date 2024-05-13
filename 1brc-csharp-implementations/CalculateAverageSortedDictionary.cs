using System.Text;
using _1brc_csharp_implementations.Common;
using _1brc_csharp_implementations.Constants;

namespace _1brc_csharp_implementations;

/// <summary>
/// Iterating from CalculateAverageSpan, continue trying to reduce allocations and duration without splitting work asynchronously and without using multiple threads.
/// </summary>
public static class CalculateAverageSortedDictionary
{
    public static void Run()
    {
        var filePath = FilePathGetter.GetFilePath();
        
        var dictionary = new SortedDictionary<string, float[]>();// SortedDictionary reduces allocations a ton, but at the cost of a bit of speed.
        foreach (var line in File.ReadLines(filePath))
        {
            ProcessLine(line, dictionary);
        }

        var sb = new StringBuilder("{");
        var index = 0;
        foreach (var weatherStation in dictionary.ToList())
        {
            sb.Append(weatherStation.Key).Append('=')
                .Append(weatherStation.Value[Indices.Minimum].ToString("##.#")).Append(',')
                .Append(weatherStation.Value[Indices.Maximum].ToString("##.#")).Append(',')
                .Append((weatherStation.Value[Indices.Total] / weatherStation.Value[Indices.Count]).ToString("##.#"));
            
            if (++index < dictionary.Count) sb.Append(", ");
        }
        sb.Append('}');

        Console.WriteLine(sb.ToString());
    }

    private static void ProcessLine(string line, SortedDictionary<string, float[]> dictionary)
    {
        var lineSpan = line.AsSpan();
        var semicolonIndex = lineSpan.IndexOf(';');
        var weatherStationName = new string(lineSpan[..semicolonIndex]);
        var newValue = float.Parse(lineSpan[(semicolonIndex + 1)..]);

        if (!dictionary.TryGetValue(weatherStationName, out var values) || values.Length == 0)
        {
            dictionary.Add(weatherStationName, [1, newValue, newValue, newValue]);
            return;
        }
        
        values[Indices.Count]++;
        if (newValue < values[Indices.Minimum]) values[Indices.Minimum] = newValue;
        if (newValue > values[Indices.Maximum]) values[Indices.Maximum] = newValue;
        values[Indices.Total] += newValue;
    }
}
