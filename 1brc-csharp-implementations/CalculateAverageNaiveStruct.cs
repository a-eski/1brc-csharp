using System.Text;
using _1brc_csharp_implementations.Common;
using _1brc_csharp_implementations.Models;

namespace _1brc_csharp_implementations;

/// <summary>
/// Initial implementation. Most experienced developers probably end up somewhere like here for first iteration (i.e. just try to do it fast synchronously).
/// CalculateAverageNaive is a very similar approach.
/// </summary>
public static class CalculateAverageNaiveStruct
{
    public static void Run()
    {
        var filePath = FilePathGetter.GetFilePath();
        
        var dictionary = new Dictionary<string, NaiveStruct>();
        foreach (var line in File.ReadLines(filePath))
        {
            if (line.StartsWith('#')) continue;

            ProcessLine(line, dictionary);
        }

        var sb = new StringBuilder("{");
        var index = 0;
        foreach (var weatherStation in dictionary.OrderBy(x => x.Key))
        {
            var separator = index++ < dictionary.Count - 1 ? "," : "";
            sb.Append($"{weatherStation.Key}={weatherStation.Value.Min},{weatherStation.Value.Max},{weatherStation.Value.Total / weatherStation.Value.Count}{separator}");
        }
        sb.Append('}');

        Console.WriteLine(sb.ToString());
    }

    private static void ProcessLine(string line, Dictionary<string, NaiveStruct> dictionary)
    {
        var data = line.Split(';');
        var newValue = double.Parse(data[1]);

        dictionary.TryGetValue(data[0], out var values);
        if (values.Count == 0)
        {
            var initialValues = new NaiveStruct { Count = 1.0, Min = newValue, Max = newValue, Total = newValue };
            dictionary.Add(data[0], initialValues);
            return;
        }

        var newValues = CalculateValues(values, newValue);
        dictionary[data[0]] = newValues;
    }

    private static NaiveStruct CalculateValues(NaiveStruct values, double newValue)
    {
        values.Count++;//increment count
        if (newValue < values.Min) values.Min = newValue;//track min
        if (newValue > values.Max) values.Max = newValue;//track max
        values.Total += newValue;//track total

        return values;
    }
}
