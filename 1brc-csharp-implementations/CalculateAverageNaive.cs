using System.Text;
using _1brc_csharp_implementations.Common;

namespace _1brc_csharp_implementations;

/// <summary>
/// Initial implementation. Most experienced developers probably end up somewhere like here for first iteration (i.e. just try to do it fast synchronously).
/// CalculateAveratgeNaiveStruct_3 is a very similar approach.
/// </summary>
public static class CalculateAverageNaive
{
    public static void Run()
    {
        var filePath = FilePathGetter.GetFilePath();

        var dictionary = new Dictionary<string, double[]>();// array is length 4. count, min, max, total. mean calculated at end, to avoid unnecessary division operations.
        foreach (var line in File.ReadLines(filePath))
        {
            ProcessLine(line, dictionary);
        }

        var sb = new StringBuilder("{");
        var index = 0;
        foreach (var weatherStation in dictionary.OrderBy(x => x.Key))
        {
            sb.Append($"{weatherStation.Key}={weatherStation.Value[1]:##.#},{weatherStation.Value[2]:##.#},{(weatherStation.Value[3] / weatherStation.Value[0]):##.#}");
            if (index++ < dictionary.Count - 1) sb.Append(", ");
        }
        sb.Append('}');

        Console.WriteLine(sb.ToString());
    }

    private static void ProcessLine(string line, Dictionary<string, double[]> dictionary)
    {
        var data = line.Split(';');
        var newValue = double.Parse(data[1]);

        dictionary.TryGetValue(data[0], out var values);
        if (values == null || values.Length == 0)
        {
            var initialValues = new[] { 1.0, newValue, newValue, newValue };
            dictionary.Add(data[0], initialValues);
            return;
        }

        var newValues = CalculateValues(values, newValue);
        dictionary[data[0]] = newValues;
    }

    private static double[] CalculateValues(double[] values, double newValue)
    {
        values[0]++;//increment count
        if (newValue < values[1]) values[1] = newValue;//track min
        if (newValue > values[2]) values[2] = newValue;//track max
        values[3] += newValue;//track total

        return values;
    }
}
