using System.Text;
using _1brc_csharp_implementations.Common;
using _1brc_csharp_implementations.Models;

namespace _1brc_csharp_implementations;

/// <summary>
/// This is a contrived implementation, intentionally trying to implement like a newer programmer.
/// </summary>
public static class CalculateAverageNaiveClass
{
    public static void Run()
    {
        var filePath = FilePathGetter.GetFilePath();

        var dictionary = new Dictionary<string, NaiveClass>();// array is length 4. count, min, max, total. mean calculated at end, to avoid unnecessary division operations.
        foreach (var line in File.ReadLines(filePath))
        {
            ProcessLine(line, dictionary);
        }

        var sb = new StringBuilder("{");
        var index = 0;
        foreach (var weatherStation in dictionary.OrderBy(x => x.Key))
        {
            sb.Append($"{weatherStation.Key}={weatherStation.Value.Min:##.#},{weatherStation.Value.Max:##.#},{weatherStation.Value.Mean:##.#}");
            if (index++ < dictionary.Count - 1) sb.Append(", ");
        }
        sb.Append('}');

        Console.WriteLine(sb.ToString());
    }

    private static void ProcessLine(string line, Dictionary<string, NaiveClass> dictionary)
    {
        var data = line.Split(';');
        var newValue = double.Parse(data[1]);

        dictionary.TryGetValue(data[0], out var values);
        if (values == null)
        {
            var initialValues = new NaiveClass { Count = 1.0, Min = newValue, Max = newValue, Mean = newValue, Total = newValue };
            dictionary.Add(data[0], initialValues);
            return;
        }

        var newValues = CalculateValues(values, newValue);
        dictionary[data[0]] = newValues;
    }

    private static NaiveClass CalculateValues(NaiveClass values, double newValue)
    {
        values.Count++;//increment count
        if (newValue < values.Min) values.Min = newValue;//track min
        if (newValue > values.Max) values.Max = newValue;//track max
        values.Total += newValue;//track total
        values.Mean = values.Total / values.Count;//keep track of current mean as well

        return values;
    }
}
