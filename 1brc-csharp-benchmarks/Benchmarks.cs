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
//[SimpleJob(RuntimeMoniker.NativeAot80)]
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
    // public void CalculateAverageNaiveClass_3() => _1brc_csharp_implementations.CalculateAverageNaiveStruct.Run();
    // [Benchmark]
    // public void CalculateAverageNaiveStruct_4() => _1brc_csharp_implementations.CalculateAverageNaiveStruct.Run();
    // [Benchmark]
    // public void CalculateAverageNaive_5() => _1brc_csharp_implementations.CalculateAverageNaive.Run();
    // [Benchmark]
    // public void CalculateAverageStringBuilder_6() => _1brc_csharp_implementations.CalculateAverageStringBuilder.Run();
    // [Benchmark]
    // public void CalculateAverageSpan_7() => _1brc_csharp_implementations.CalculateAverageSpan.Run();
    // [Benchmark]
    // public void CalculateAverageSortedDictionary_8() => _1brc_csharp_implementations.CalculateAverageSortedDictionary.Run();
    // [Benchmark]
    // public void CalculateAverageStruct_9() => _1brc_csharp_implementations.CalculateAverageStruct.Run();
    // [Benchmark]
    // public void CalculateAverageSpan2_10() => _1brc_csharp_implementations.CalculateAverageSpan2.Run();
    // [Benchmark]
    // public async Task CalculateAverageAsync_11() => await _1brc_csharp_implementations.CalculateAverageAsync.Run();
    // [Benchmark]
    // public void CalculateAverageStreamReader_12() => _1brc_csharp_implementations.CalculateAverageStreamReader.Run();
    [Benchmark]
    public void CalculateAverageFasterConsole_13() => _1brc_csharp_implementations.CalculateAverageFasterConsole.Run();
    [Benchmark]
    public void CalculateAverageSplitOutput_14() => _1brc_csharp_implementations.CalculateAverageSplitOutput.Run();
}

/* 
| Method                             | Mean     | Error    | StdDev   | Median   | Gen0      | Gen1      | Gen2      | Allocated |
|----------------------------------- |---------:|---------:|---------:|---------:|----------:|----------:|----------:|----------:|
| CalculateAverageStringBuilder_6    | 53.18 ms | 0.415 ms | 0.368 ms | 53.15 ms | 1400.0000 | 1400.0000 |  900.0000 |  19.59 MB |
| CalculateAverageSpan_7             | 39.84 ms | 0.258 ms | 0.241 ms | 39.76 ms | 1615.3846 | 1615.3846 | 1153.8462 |  16.18 MB |
| CalculateAverageSortedDictionary_8 | 63.22 ms | 0.236 ms | 0.197 ms | 63.19 ms |  875.0000 |  875.0000 |  875.0000 |  14.03 MB |
| CalculateAverageStruct_9           | 43.25 ms | 0.864 ms | 2.335 ms | 44.14 ms | 1416.6667 | 1333.3333 | 1000.0000 |  16.02 MB |
| CalculateAverageSpan2_10           | 39.82 ms | 0.161 ms | 0.135 ms | 39.78 ms | 1538.4615 | 1538.4615 | 1076.9231 |  16.18 MB |
| CalculateAverageAsync_11           | 52.69 ms | 0.726 ms | 0.679 ms | 52.87 ms | 1300.0000 | 1300.0000 |  900.0000 |  19.29 MB |
| CalculateAverageStreamReader_12    | 39.52 ms | 0.420 ms | 0.392 ms | 39.35 ms | 1538.4615 | 1538.4615 | 1076.9231 |  16.18 MB |
*/