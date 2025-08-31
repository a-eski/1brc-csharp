using _1brc_csharp_implementations.Common;
using _1brc_csharp_implementations.Models;
using FluentAssertions;

namespace _1brc_csharp_tests;

public class HashTableTests
{
    [Fact]
    public void TestHashTable()
    {
        var ht = new HashTable
        {
            Keys = new string[1000],
            Values = new WeatherValues[1000]
        };

        ref var values = ref ht.AddOrGet("WeatherStationName");
        values.Count.Should().Be(0);
        values.Count = 1;
        values.Max = 1;
        values.Min = 1;
        values.Total = 1;
        
        ref var valuesAfter = ref ht.AddOrGet("WeatherStationName");
        valuesAfter.Count.Should().Be(1);
        valuesAfter.Max.Should().Be(1);
        valuesAfter.Min.Should().Be(1);
        valuesAfter.Total.Should().Be(1);
    }
}