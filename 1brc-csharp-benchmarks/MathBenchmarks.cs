using _1brc_csharp_implementations.Models;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace _1brc_csharp_benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
public class MathBenchmarks
{
    private static WeatherValues _values = new()
    {
        Max = 50,
        Min = 10,
        Count = 4,
        Total = 100
    };

    // [Benchmark]
    public void MathMinMax() // is slightly faster than Arithmetic masks.
    {
        _values.Max = Math.Min(_values.Max, 60);
        _values.Min = Math.Max(_values.Min, 60);
        _values.Count++;
        _values.Total += 60;
    }

    // [Benchmark]
    public void ArithmeticMasks()
    {
        _values.Max = _values.Max < 60 ? 60 : _values.Max;
        _values.Min = _values.Min < 60 ? _values.Min : 60;
        _values.Count++;
        _values.Total += 60;
    }

    [Benchmark]
    public void Truncate()
    {
        var values = new WeatherValues
        {
            Max = (float)(Math.Truncate(_values.Max * 1E1) / 1E1),
            Min = (float)(Math.Truncate(_values.Max * 1E1) / 1E1),
            Total = (float)(Math.Truncate(_values.Total / _values.Count * 1E1) / 1E1),
            Count = _values.Count
        };
        
        _values.Max = values.Max;
        _values.Min = values.Min;
        _values.Count = values.Count;
        _values.Total = values.Total;
    }
    
    [Benchmark]
    public void Round()
    {
        var values = new WeatherValues
        {
            Max = (float)Math.Round(_values.Max, MidpointRounding.ToZero),
            Min = (float)Math.Round(_values.Max, MidpointRounding.ToZero),
            Total = (float)Math.Round(_values.Total / _values.Count, MidpointRounding.ToZero)
        };
        
        _values.Max = values.Max;
        _values.Min = values.Min;
        _values.Count = values.Count;
        _values.Total = values.Total;
    }
}