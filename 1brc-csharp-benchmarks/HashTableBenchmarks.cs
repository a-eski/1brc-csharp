using System.Runtime.InteropServices;
using _1brc_csharp_implementations.Common;
using _1brc_csharp_implementations.Models;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace _1brc_csharp_benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
public class HashTableBenchmarks
{
    private const string WeatherStationName = nameof(WeatherStationName);
    [Benchmark]
    public void HashTable()
    {
        var ht = new HashTable
        {
            Keys = new string[100],
            Values = new WeatherValues[100]
        };

        ref var values = ref ht.AddOrGet(WeatherStationName);
        if (values.Count == 0)
        {
            values.Count = 1;
            values.Max = 1;
            values.Min = 1;
            values.Total = 1;
        }

        ref var valuesAfter = ref ht.AddOrGet(WeatherStationName);
        if (valuesAfter.Count == 0)
        {
            valuesAfter.Count++;
        }
    }

    [Benchmark]
    public void Dictionary()
    {
        var dictionary = new Dictionary<string, WeatherValues>();
        ref var values = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, WeatherStationName, out var exists);

        if (!exists)
        {
            values.Count = 1;
            values.Max = 1;
            values.Min = 1;
            values.Total = 1;
        }
        
        ref var valuesAfter = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, WeatherStationName, out var existsAfter);
        if (existsAfter)
            valuesAfter.Count++;
    }
}