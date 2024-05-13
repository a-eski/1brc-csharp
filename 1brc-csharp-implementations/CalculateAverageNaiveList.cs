using System.Text;
using _1brc_csharp_implementations.Common;

namespace _1brc_csharp_implementations;

/// <summary>
/// Initial implementation that illustrates how important selecting correct data structure is for performance.
/// In this case, using a list instead of a dictionary means we have to iterate the list instead of accessing by key to check if values exist.
/// </summary>
public class WeatherInfo
{
    public string StationName { get; set; }
    public double[] Data { get; set; } // array is length 3. count, min, max, total. mean calculated at end, to avoid unnecessary division operations.
}

public static class CalculateAverageNaiveList
{
    public static void Run()
    {
        var filePath = FilePathGetter.GetFilePath();

        var list = new List<WeatherInfo>();
        foreach (var line in File.ReadLines(filePath))
        {
            ProcessLine(line, list);
        }

        var sb = new StringBuilder("{");
        var index = 0;
        foreach (var weatherStation in list.OrderBy(x => x.StationName))
        {
            sb.Append(
                $"{weatherStation.StationName}={weatherStation.Data[1]:##.#},{weatherStation.Data[2]:##.#},{(weatherStation.Data[3] / weatherStation.Data[0]):##.#}");
            if (index++ < list.Count - 1) sb.Append(", ");
        }

        sb.Append('}');

        Console.WriteLine(sb.ToString());
    }

    private static void ProcessLine(string line, List<WeatherInfo> list)
    {
        var data = line.Split(';');
        var newValue = double.Parse(data[1]);

        var weatherInfo = list.FirstOrDefault(x => x.StationName == data[0]);
        if (weatherInfo == null)
        {
            list.Add(new WeatherInfo { StationName = data[0], Data = [1.0f, newValue, newValue, newValue] });
            return;
        }

        weatherInfo.Data[0]++;
        if (newValue < weatherInfo.Data[1]) weatherInfo.Data[1] = newValue;
        if (newValue > weatherInfo.Data[2]) weatherInfo.Data[2] = newValue;
        weatherInfo.Data[3] += newValue;
    }
}