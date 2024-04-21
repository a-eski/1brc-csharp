using _1brc_csharp_implementations.Common;
using _1brc_csharp_implementations.Models;

namespace _1brc_csharp_implementations;

/// <summary>
/// This is a contrived implementation, intentionally trying to implement like a newer programmer.
/// </summary>
public static class CalculateAverageNaiveClassConsoleWrite
{
    public static void Run()
    {
        var filePath = FilePathGetter.GetFilePath();

        var dictionary = new Dictionary<string, NaiveClass>();
        foreach (var line in File.ReadLines(filePath))
        {
            ProcessLine(line, dictionary);
        }

        Console.Write("{");
        var i = 0;
        foreach (var weatherStation in dictionary.OrderBy(x => x.Key))
        {
            Console.Write($"{weatherStation.Key}={weatherStation.Value.Min:##.#},{weatherStation.Value.Max:##.#},{weatherStation.Value.Mean:##.#}");
            if (i < dictionary.Count - 1) Console.Write(", ");
            i++;
        }
        Console.Write("}");
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
        values.Count++;
        if (newValue < values.Min) values.Min = newValue;
        if (newValue > values.Max) values.Max = newValue;
        values.Total += newValue;
        values.Mean = values.Total / values.Count;//keep track of current mean as well. Future implementations will not do this to reduce division operations.

        return values;
    }
}
