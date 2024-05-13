using System.Text;
using _1brc_csharp_implementations.Common;
using _1brc_csharp_implementations.Constants;

namespace _1brc_csharp_implementations;

/// <summary>
/// Iterating from CalculateAverageSpan, continue trying to reduce allocations and duration without splitting work asynchronously and without using multiple threads.
/// </summary>
public static class CalculateAverageSpan2
{
    public static void Run()
    {
        var filePath = FilePathGetter.GetFilePath(); 

        var dictionary = new Dictionary<string, float[]>();// array is length 4. count, min, max, total. mean calculated at end, to avoid unnecessary division operations.
        foreach (var line in File.ReadLines(filePath))
        {
            var lineSpan = line.AsSpan();
            var semicolonIndex = lineSpan.IndexOf(';');
            var weatherStationName = new string(lineSpan[..semicolonIndex]);
            var newValue = float.Parse(lineSpan[(semicolonIndex + 1)..]);

            if (!dictionary.TryGetValue(weatherStationName, out var values))
            {
                dictionary.Add(weatherStationName, [1.0f, newValue, newValue, newValue]);
                continue;
            }
        
            values[Indices.Count]++;
            if (newValue < values[Indices.Minimum]) values[Indices.Minimum] = newValue;
            if (newValue > values[Indices.Maximum]) values[Indices.Maximum] = newValue;
            values[Indices.Total] += newValue;
        }

        var sb = new StringBuilder("{");
        var index = 0;
        foreach (var weatherStation in dictionary.OrderBy(x => x.Key))
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
}
