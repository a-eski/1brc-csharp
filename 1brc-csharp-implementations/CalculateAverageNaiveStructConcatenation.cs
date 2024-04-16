using _1brc_csharp_implementations.Common;
using _1brc_csharp_implementations.Models;

namespace _1brc_csharp_implementations;

/// <summary>
/// This is a contrived implementation, intentionally trying to implement like a newer programmer.
/// </summary>
public static class CalculateAverageNaiveStructConcatenation
{
    private const string WeatherDataFile = "/data/weather_stations.csv";

    public static void Run()
    {
        var filePath = FilePathGetter.GetFilePath();
        
        var dictionary = new Dictionary<string, NaiveStruct>();// array is length 4. count, min, max, total. mean calculated at end, to avoid unnecessary division operations.
        foreach (var line in File.ReadLines(filePath))
        {
            if (line.StartsWith('#')) continue;

            ProcessLine(line, dictionary);
        }

        var result = "{";
        var i = 0;
        foreach (var weatherStation in dictionary.OrderBy(x => x.Key))
        {
            result += $"{weatherStation.Key}={weatherStation.Value.Min},{weatherStation.Value.Max},{weatherStation.Value.Total / weatherStation.Value.Count}";
            if (i < dictionary.Count - 1) result += ",";
            i++;
        }

        result += "}";

        Console.WriteLine(result);
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
        values.Mean = values.Total / values.Count;//track mean. excluded from further implementations, is a huge optimization

        return values;
    }
}
