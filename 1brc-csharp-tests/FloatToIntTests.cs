using FluentAssertions;

namespace _1brc_csharp_tests;

/// <summary>
/// Develop a way to store the float data points in measurements.txt as integers. This is possible because all the floating point values have 1 decimal place.
/// It would be possible with more decimal places as well, just a bit more added complexity.
/// Storing in an integer instead of a float takes half as much memory, but operations on integers are also extremely fast, hence the reason for trying this.
/// </summary>
public class FloatToIntTests
{
    [Fact]
    public void One_Data_Point_Per_Station_Have_Matching_Results()
    {
        var weatherValuesFloat = WeatherValuesFloat.GetNew(20.5f);
        var weatherValuesInt = WeatherValuesInt.GetNew(20.5f);

        weatherValuesFloat.GetMean().Should().Be(weatherValuesInt.GetMean());
    }
    
    [Fact]
    public void Multiple_Data_Points_Per_Station_Have_Matching_Results()
    {
        var weatherValuesFloat = WeatherValuesFloat.GetNew(20.5f);
        var weatherValuesInt = WeatherValuesInt.GetNew(20.5f);
        
        weatherValuesFloat.Apply(-32.4f);
        weatherValuesInt.Apply(-32.4f);
        
        weatherValuesFloat.Apply(41.5f);
        weatherValuesInt.Apply(41.5f);

        var meanFloat = weatherValuesFloat.GetMean();
        var roundedMeanFloat = Math.Round(meanFloat, 1, MidpointRounding.ToZero);
        var meanInt = weatherValuesInt.GetMean();
        var roundedMeanInt = Math.Round(meanInt, 1, MidpointRounding.ToZero);

        roundedMeanFloat.Should().Be(roundedMeanInt);
    }
}

internal class WeatherValuesFloat
{
    public float Min { get; private set; }
    public float Max { get; private set; }
    public float Total { get; private set; }
    public int Count { get; private set; }
    
    public static WeatherValuesFloat GetNew(float initialValue) =>
        new WeatherValuesFloat() { Count = 1, Min = initialValue, Max = initialValue, Total = initialValue };

    public void Apply(float newValue)
    {
        Count++;
        if (newValue < Min) Min = newValue;
        if (newValue > Max) Max = newValue;
        Total += newValue;
    }

    public float GetMean() => Total / Count;
}

internal class WeatherValuesInt
{
    private int _min;
    private int _max;
    private int _total;
    private int _count;

    public static WeatherValuesInt GetNew(float initialValue)
    {
        var initialValueInt = GetInt(initialValue);
        return new WeatherValuesInt() { _count = 1, _min = initialValueInt, _max = initialValueInt, _total = initialValueInt };
    }

    public void Apply(float newValue)
    {
        var newValueInt = GetInt(newValue);
        _count++;
        if (newValue < _min) _min = newValueInt;
        if (newValue > _max) _max = newValueInt;
        _total += newValueInt;
    }

    public float GetMin() => _min / 10.0f;
    public float GetMax() => _max / 10.0f;
    public float GetMean()
    {
        //var numberOfDigits = (int)Math.Ceiling(Math.Log10(Total));
        //var totalFloat = Total / MathF.Pow(10, 1);
        var totalFloat = _total / 10.0f;
        return totalFloat / _count;
    }

    private static int GetInt(float newValue) => (int)(newValue * 10);
}