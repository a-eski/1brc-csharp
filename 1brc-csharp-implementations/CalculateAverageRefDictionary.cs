using System.Runtime.InteropServices;
using System.Text;
using _1brc_csharp_implementations.Common;
using _1brc_csharp_implementations.Constants;
using _1brc_csharp_implementations.Models;

namespace _1brc_csharp_implementations;

/// <summary>
/// Iterating from CalculateAverageStreamReader, continue trying to reduce allocations and duration without splitting work asynchronously or with multiple threads.
/// </summary>
public static class CalculateAverageRefDictionary
{
    public static void Run()
    {
        var filePath = FilePathGetter.GetPath();

        using var sr = File.OpenText(filePath);
        var dictionary = new Dictionary<string, float[]>(); //array is length 4. count, min, max, total. mean calculated at end, to avoid unnecessary division operations.
        while (!sr.EndOfStream)
        {
            var lineSpan = sr.ReadLine()!.AsSpan();
            var semicolonIndex = lineSpan.IndexOf(';');
            var weatherStationName = new string(lineSpan[..semicolonIndex]);
            var newValue = float.Parse(lineSpan[(semicolonIndex + 1)..]);

            ref var values = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, weatherStationName, out var exists);

            if (!exists || values == null)
            {
                values = [1.0f, newValue, newValue, newValue];
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
                .Append(Math.Round(weatherStation.Value[Indices.Minimum], 1, MidpointRounding.ToZero)).Append(',')
                .Append(Math.Round(weatherStation.Value[Indices.Maximum], 1, MidpointRounding.ToZero)).Append(',')
                .Append(Math.Round(weatherStation.Value[Indices.Total] / weatherStation.Value[Indices.Count], 1, MidpointRounding.ToZero));

            if (++index < dictionary.Count) sb.Append(", ");
        }

        sb.Append('}');

        var result = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(sb.ToString());
        Console.OutputEncoding = Encoding.UTF8;
        using var standardOutput = Console.OpenStandardOutput();
        standardOutput.Write(result);
    }
}