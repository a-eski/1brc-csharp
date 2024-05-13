using System.Runtime.InteropServices;
using System.Text;
using _1brc_csharp_implementations.Common;

namespace _1brc_csharp_implementations;

/// <summary>
/// Iterating from CalculateAverageStreamReader, continue trying to reduce allocations and duration without splitting work asynchronously or with multiple threads.
/// </summary>
public static class CalculateAverageStruct2
{
    public static void Run()
    {
        var filePath = FilePathGetter.GetPath();

        using var sr = File.OpenText(filePath);
        var dictionary =
            new Dictionary<string, WeatherValues>();
        while (!sr.EndOfStream)
        {
            var lineSpan = sr.ReadLine()!.AsSpan();
            var semicolonIndex = lineSpan.IndexOf(';');
            var weatherStationName = new string(lineSpan[..semicolonIndex]);
            var newValue = float.Parse(lineSpan[(semicolonIndex + 1)..]);

            ref var values =
                ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, weatherStationName, out var exists);

            if (!exists)
            {
                values.Count++;
                values.Min = newValue;
                values.Max = newValue;
                values.Total = newValue;
                continue;
            }

            values.Count++;
            if (newValue < values.Min) values.Min = newValue;
            if (newValue > values.Max) values.Max = newValue;
            values.Total += newValue;
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

public record struct WeatherValues
{
    public float Min { get; set; }
    public float Max { get; set; }
    public float Total { get; set; }
    public int Count { get; set; }
}