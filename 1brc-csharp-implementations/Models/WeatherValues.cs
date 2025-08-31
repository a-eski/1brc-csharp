namespace _1brc_csharp_implementations.Models;

public record struct WeatherValues
{
    public float Min { get; set; }
    public float Max { get; set; }
    public float Total { get; set; }
    public int Count { get; set; }
}