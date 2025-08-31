using System.Runtime.InteropServices;
using System.Text;
using _1brc_csharp_implementations.Common;
using _1brc_csharp_implementations.Models;

namespace _1brc_csharp_implementations;

/// <summary>
/// Iterating from CalculateAverageSb, continue trying to reduce allocations and duration without splitting work asynchronously or with multiple threads.
/// </summary>
public static class CalculateAverageSb2
{
    public static void Run()
    {
        var filePath = FilePathGetter.GetPath();

        using var sr = File.OpenText(filePath);
        var dictionary = new Dictionary<string, WeatherValues>();
        while (!sr.EndOfStream)
        {
            var lineSpan = sr.ReadLine()!.AsSpan();
            var semicolonIndex = lineSpan.IndexOf(';');
            var weatherStationName = new string(lineSpan[..semicolonIndex]);
            var newValue = float.Parse(lineSpan[(semicolonIndex + 1)..]);

            ref var values = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, weatherStationName, out var exists);

            if (!exists)
            {
                values.Count++;
                values.Min = newValue;
                values.Max = newValue;
                values.Total = newValue;
                continue;
            }

            values.Count++;
            values.Min = Math.Min(values.Min, newValue);
            values.Max = Math.Max(values.Max, newValue);
            values.Total += newValue;
        }

        var sb = new StringBuilder("{");
        foreach (var weatherStation in dictionary.OrderBy(x => x.Key))
        {
            var min = (float)(Math.Truncate(weatherStation.Value.Min * 1E1) / 1E1);
            var max = (float)(Math.Truncate(weatherStation.Value.Max * 1E1) / 1E1);
            var total = (float)(Math.Truncate(weatherStation.Value.Total / weatherStation.Value.Count * 1E1) / 1E1);
            sb.Append(weatherStation.Key).Append('=')
                            .Append(min).Append(',')
                            .Append(max).Append(',')
                            .Append(total).Append(", ");
        }
        sb.Append('}');
        sb.Remove(sb.Length - 3, 2);

        var result = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(sb.ToString());
        Console.OutputEncoding = Encoding.UTF8;
        using var standardOutput = Console.OpenStandardOutput();
        standardOutput.Write(result);
    }
}
