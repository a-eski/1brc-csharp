namespace _1brc_csharp_implementations.Models;

public struct WeatherValues
{
    public WeatherValues(float initialValue)
    {
        Min = initialValue;
        Max = initialValue;
        Total = initialValue;
        Count = 1;
    }

    public float Min { get; private set; }
    public float Max { get; private set; }
    public float Total { get; private set; }
    public int Count { get; private set; }

    public void Apply(float newValue)
    {
        Count++;
        if (newValue < Min) Min = newValue;
        if (newValue > Max) Max = newValue;
        Total += newValue;
    }
}