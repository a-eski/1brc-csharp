using System.Text;
using _1brc_csharp_implementations.Common;

namespace _1brc_csharp_implementations;

/// <summary>
/// Iterating from CalculateAverageNaive, continue trying to reduce allocations and duration without splitting work asynchronously and without using multiple threads.
/// </summary>
public static class CalculateAverageStringBuilder
{
    public static void Run()
    {
        var filePath = FilePathGetter.GetFilePath();
        
        var dictionary = new Dictionary<string, float[]>();// array is length 4. count, min, max, total. mean calculated at end, to avoid unnecessary division operations.
        foreach (var line in File.ReadLines(filePath))
        {
            ProcessLine(line, dictionary);
        }

        var sb = new StringBuilder("{");
        var index = 0;
        foreach (var weatherStation in dictionary.OrderBy(x => x.Key))
        {
            sb.Append(weatherStation.Key).Append('=')
                .Append(weatherStation.Value[1].ToString("##.#")).Append(',')
                .Append(weatherStation.Value[2].ToString("##.#")).Append(',')
                .Append((weatherStation.Value[3] / weatherStation.Value[0]).ToString("##.#"));
            
            if (++index < dictionary.Count) sb.Append(", ");
        }
        sb.Append('}');

        Console.WriteLine(sb.ToString());
    }

    private static void ProcessLine(string line, Dictionary<string, float[]> dictionary)
    {
        var data = line.Split(';');
        var newValue = float.Parse(data[1]);

        dictionary.TryGetValue(data[0], out var values);
        if (values == null || values.Length == 0)
        {
            var initialValues = new[] { 1.0f, newValue, newValue, newValue };
            dictionary.Add(data[0], initialValues);
            return;
        }

        CalculateValues(values, newValue);
    }

    private static void CalculateValues(float[] values, float newValue)
    {
        values[0]++;
        if (newValue < values[1]) values[1] = newValue;
        if (newValue > values[2]) values[2] = newValue;
        values[3] += newValue;
    }
}
