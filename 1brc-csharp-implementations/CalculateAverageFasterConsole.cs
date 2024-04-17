using System.Text;
using _1brc_csharp_implementations.Common;

namespace _1brc_csharp_implementations;

/// <summary>
/// Iterating from CalculateAverageStreamReader, continue trying to reduce allocations and duration without splitting work asynchronously or with multiple threads.
/// </summary>
public static class CalculateAverageFasterConsole
{
    public static void Run()
    {
        var filePath = FilePathGetter.GetPath();
        using var sr = File.OpenText(filePath);
        var dictionary = new Dictionary<string, float[]>();//array is length 4. count, min, max, total. mean calculated at end, to avoid unnecessary division operations.
        while (!sr.EndOfStream)
        {
            var lineSpan = sr.ReadLine()!.AsSpan();
            var semicolonIndex = lineSpan.IndexOf(';');
            var weatherStationName = new string(lineSpan[..semicolonIndex]);
            var newValue = float.Parse(lineSpan[(semicolonIndex + 1)..]);
            
            if (!dictionary.TryGetValue(weatherStationName, out var values))
            {
                dictionary.Add(weatherStationName, [1.0f, newValue, newValue, newValue]);
                continue;
            }
            
            values[0]++;//increment count
            if (newValue < values[1]) values[1] = newValue;//track min
            if (newValue > values[2]) values[2] = newValue;//track max
            values[3] += newValue;//track total
        }

        var sb = new StringBuilder("{");
        var index = 0;
        foreach (var weatherStation in dictionary.OrderBy(x => x.Key))
        {
            sb.Append(weatherStation.Key).Append('=')
                .Append(weatherStation.Value[1]).Append(',')
                .Append(weatherStation.Value[2]).Append(',')
                .Append(weatherStation.Value[3] / weatherStation.Value[0]);

            if (++index < dictionary.Count) sb.Append(',');
        }
        sb.Append('}');

        var result = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(sb.ToString());
        Console.OutputEncoding = Encoding.UTF8;
        using var standardOutput = Console.OpenStandardOutput();
        standardOutput.Write(result);
    }
}
