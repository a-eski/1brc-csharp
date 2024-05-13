using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Diagnostics.Windows.Configs;

namespace _1brc_csharp_benchmarks;

//[SimpleJob(RuntimeMoniker.Mono)]
//[SimpleJob(RuntimeMoniker.Net472)]
//[SimpleJob(RuntimeMoniker.Net481)]
//[SimpleJob(RuntimeMoniker.Net70, baseline: true)]
[SimpleJob(RuntimeMoniker.Net80)]
//[SimpleJob(RuntimeMoniker.NativeAot80)]//not setup for AOT
//[RPlotExporter]//needs R installed
//[EtwProfiler]//produces a trace file
[MemoryDiagnoser]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class Benchmarks
{
    //[Benchmark]//don't bother benchmarking
    //public void CalculateAverageNaiveClassConsoleWrite_1() => _1brc_csharp_implementations.CalculateAverageNaiveClassConsoleWrite.Run();
    //[Benchmark]//3 orders of magnitude slower and a ton more memory than other naive implementations, excluded for purposes of comparing other benchmarks quicker
    //public void CalculateAverageNaiveStructConcatenation_2() => _1brc_csharp_implementations.CalculateAverageNaiveStructConcatenation.Run();
    // [Benchmark]
    // public void CalculateAverageNaiveList_3() => _1brc_csharp_implementations.CalculateAverageNaiveList.Run();
    // [Benchmark]
    // public void CalculateAverageNaiveClass_4() => _1brc_csharp_implementations.CalculateAverageNaiveStruct.Run();
    // [Benchmark]
    // public void CalculateAverageNaiveStruct_5() => _1brc_csharp_implementations.CalculateAverageNaiveStruct.Run();
    [Benchmark]
    public void CalculateAverageNaive_6() => _1brc_csharp_implementations.CalculateAverageNaive.Run();
    [Benchmark]
    public void CalculateAverageStringBuilder_7() => _1brc_csharp_implementations.CalculateAverageStringBuilder.Run();
    [Benchmark]
    public void CalculateAverageSpan_8() => _1brc_csharp_implementations.CalculateAverageSpan.Run();
    [Benchmark]
    public void CalculateAverageSortedDictionary_9() => _1brc_csharp_implementations.CalculateAverageSortedDictionary.Run();
    // [Benchmark]  //doesn't calculate values correctly
    // public void CalculateAverageStruct_10() => _1brc_csharp_implementations.CalculateAverageStruct.Run();
    [Benchmark]
    public void CalculateAverageSpan2_11() => _1brc_csharp_implementations.CalculateAverageSpan2.Run();
    [Benchmark]
    public async Task CalculateAverageAsync_12() => await _1brc_csharp_implementations.CalculateAverageAsync.Run();
    [Benchmark]
    public void CalculateAverageStreamReader_13() => _1brc_csharp_implementations.CalculateAverageStreamReader.Run();
    [Benchmark]
    public void CalculateAverageFasterConsole_14() => _1brc_csharp_implementations.CalculateAverageFasterConsole.Run();
    // [Benchmark]  //doesn't calculate values correctly
    // public void CalculateAverageStructFasterConsole_15() => _1brc_csharp_implementations.CalculateAverageStructFasterConsole.Run();
    [Benchmark]
    public void CalculateAverageRefDictionary_16() => _1brc_csharp_implementations.CalculateAverageRefDictionary.Run(); 
    [Benchmark]
    public void CalculateAverageStruct2_17() => _1brc_csharp_implementations.CalculateAverageStruct2.Run(); 
}

/*
 benchmarking with 1 million row measurements.txt
| Method                             | Mean     | Error    | StdDev   | Median   | Gen0       | Gen1      | Gen2     | Allocated |
|----------------------------------- |---------:|---------:|---------:|---------:|-----------:|----------:|---------:|----------:|
| CalculateAverageNaive_6            | 142.6 ms |  1.46 ms |  1.29 ms | 142.6 ms | 10000.0000 | 1000.0000 | 750.0000 | 164.21 MB |
| CalculateAverageStringBuilder_7    | 134.3 ms |  1.24 ms |  1.03 ms | 134.4 ms | 10250.0000 | 1000.0000 | 750.0000 | 164.89 MB |
| CalculateAverageSpan_8             | 111.7 ms |  1.24 ms |  1.16 ms | 111.6 ms |  5800.0000 | 1000.0000 | 800.0000 |  96.22 MB |
| CalculateAverageSortedDictionary_9 | 604.4 ms | 12.42 ms | 36.62 ms | 615.6 ms |  5000.0000 | 1000.0000 |        - |   95.7 MB |
| CalculateAverageSpan2_11           | 109.8 ms |  0.99 ms |  0.88 ms | 109.8 ms |  5800.0000 | 1000.0000 | 800.0000 |  96.22 MB |
| CalculateAverageAsync_12           | 185.7 ms |  3.69 ms |  3.95 ms | 185.7 ms | 10000.0000 | 1000.0000 | 500.0000 | 165.71 MB |
| CalculateAverageStreamReader_13    | 111.0 ms |  1.13 ms |  1.06 ms | 111.1 ms |  5800.0000 | 1000.0000 | 800.0000 |  96.22 MB |
| CalculateAverageFasterConsole_14   | 108.0 ms |  0.57 ms |  0.54 ms | 108.1 ms |  5800.0000 | 1000.0000 | 800.0000 |  95.63 MB |
| CalculateAverageRefDictionary_16   | 107.9 ms |  0.73 ms |  0.69 ms | 107.9 ms |  5800.0000 | 1000.0000 | 800.0000 |  95.63 MB |
| CalculateAverageStruct2_17         | 106.4 ms |  1.08 ms |  1.01 ms | 106.1 ms |  5800.0000 | 1000.0000 | 800.0000 |  95.62 MB |
*/