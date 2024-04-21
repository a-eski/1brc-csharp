﻿using System.Diagnostics.CodeAnalysis;
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
    [Benchmark]
    public void CalculateAverageNaiveClass_3() => _1brc_csharp_implementations.CalculateAverageNaiveStruct.Run();
    [Benchmark]
    public void CalculateAverageNaiveStruct_4() => _1brc_csharp_implementations.CalculateAverageNaiveStruct.Run();
    [Benchmark]
    public void CalculateAverageNaive_5() => _1brc_csharp_implementations.CalculateAverageNaive.Run();
    [Benchmark]
    public void CalculateAverageStringBuilder_6() => _1brc_csharp_implementations.CalculateAverageStringBuilder.Run();
    [Benchmark]
    public void CalculateAverageSpan_7() => _1brc_csharp_implementations.CalculateAverageSpan.Run();
    [Benchmark]
    public void CalculateAverageSortedDictionary_8() => _1brc_csharp_implementations.CalculateAverageSortedDictionary.Run();
    [Benchmark]
    public void CalculateAverageStruct_9() => _1brc_csharp_implementations.CalculateAverageStruct.Run();
    [Benchmark]
    public void CalculateAverageSpan2_10() => _1brc_csharp_implementations.CalculateAverageSpan2.Run();
    [Benchmark]
    public async Task CalculateAverageAsync_11() => await _1brc_csharp_implementations.CalculateAverageAsync.Run();
    [Benchmark]
    public void CalculateAverageStreamReader_12() => _1brc_csharp_implementations.CalculateAverageStreamReader.Run();
    [Benchmark]
    public void CalculateAverageFasterConsole_13() => _1brc_csharp_implementations.CalculateAverageFasterConsole.Run();
    [Benchmark]
    public void CalculateAverageStructFasterConsole_14() => _1brc_csharp_implementations.CalculateAverageStructFasterConsole.Run();
}

/*
 benchmarking with 1 million row measurements.txt
| Method                             | Mean     | Error   | StdDev  | Gen0       | Gen1      | Gen2     | Allocated |
|----------------------------------- |---------:|--------:|--------:|-----------:|----------:|---------:|----------:|
| CalculateAverageNaiveClass_3       | 141.8 ms | 0.95 ms | 0.89 ms | 10250.0000 | 1500.0000 | 750.0000 | 165.04 MB |
| CalculateAverageNaiveStruct_4      | 141.8 ms | 0.80 ms | 0.75 ms | 10250.0000 | 1500.0000 | 750.0000 | 165.04 MB |
| CalculateAverageNaive_5            | 148.1 ms | 1.66 ms | 1.55 ms | 10000.0000 | 1000.0000 | 750.0000 | 164.21 MB |
| CalculateAverageStringBuilder_6    | 130.8 ms | 0.71 ms | 0.67 ms | 10250.0000 | 1000.0000 | 750.0000 | 164.89 MB |
| CalculateAverageSpan_7             | 110.6 ms | 0.68 ms | 0.64 ms |  5800.0000 | 1000.0000 | 800.0000 |  96.22 MB |
| CalculateAverageSortedDictionary_8 | 490.5 ms | 3.03 ms | 2.53 ms |  5000.0000 | 1000.0000 |        - |   95.7 MB |
| CalculateAverageStruct_9           | 104.8 ms | 0.53 ms | 0.47 ms |  5800.0000 | 1000.0000 | 800.0000 |  96.24 MB |
| CalculateAverageSpan2_10           | 109.1 ms | 0.47 ms | 0.44 ms |  5800.0000 | 1000.0000 | 800.0000 |  96.22 MB |
| CalculateAverageAsync_11           | 185.9 ms | 3.68 ms | 6.14 ms | 10000.0000 | 1000.0000 | 500.0000 | 165.71 MB |
| CalculateAverageStreamReader_12    | 109.2 ms | 0.61 ms | 0.54 ms |  5800.0000 | 1000.0000 | 800.0000 |  96.22 MB |
| CalculateAverageFasterConsole_13   | 107.3 ms | 0.73 ms | 0.69 ms |  5800.0000 | 1000.0000 | 800.0000 |  95.63 MB |



| Method                                 | Mean          | Error        | StdDev       | Gen0       | Gen1      | Gen2     | Allocated    |
|--------------------------------------- |--------------:|-------------:|-------------:|-----------:|----------:|---------:|-------------:|
| CalculateAverageNaiveClass_3           | 144,984.09 us | 2,610.135 us | 2,313.816 us | 10250.0000 | 1500.0000 | 750.0000 | 168997.49 KB |
| CalculateAverageNaiveStruct_4          | 142,437.92 us | 1,675.444 us | 1,567.212 us | 10250.0000 | 1500.0000 | 750.0000 | 168997.49 KB |
| CalculateAverageNaive_5                | 144,582.17 us | 1,664.242 us | 1,556.733 us | 10000.0000 | 1000.0000 | 750.0000 | 168155.15 KB |
| CalculateAverageStringBuilder_6        | 132,490.67 us | 1,763.786 us | 1,649.847 us | 10250.0000 | 1000.0000 | 750.0000 | 168845.05 KB |
| CalculateAverageSpan_7                 | 111,669.02 us |   714.823 us |   633.672 us |  5800.0000 | 1000.0000 | 800.0000 |  98532.55 KB |
| CalculateAverageSortedDictionary_8     | 490,377.52 us | 2,089.426 us | 1,852.222 us |  5000.0000 | 1000.0000 |        - |  97994.79 KB |
| CalculateAverageStruct_9               | 106,984.73 us | 1,098.930 us | 1,027.940 us |  5800.0000 | 1000.0000 | 800.0000 |   98547.5 KB |
| CalculateAverageSpan2_10               | 110,799.42 us | 1,008.438 us |   842.091 us |  5800.0000 | 1000.0000 | 800.0000 |  98532.55 KB |
| CalculateAverageAsync_11               | 184,982.13 us | 3,449.327 us | 3,226.502 us | 10000.0000 | 1000.0000 | 500.0000 | 169685.85 KB |
| CalculateAverageStreamReader_12        | 109,377.37 us |   729.767 us |   609.388 us |  5800.0000 | 1000.0000 | 800.0000 |   98532.5 KB |
| CalculateAverageFasterConsole_13       | 106,799.83 us |   555.635 us |   519.741 us |  5800.0000 | 1000.0000 | 800.0000 |  97927.23 KB |
| CalculateAverageStructFasterConsole_14 |      12.61 us |     0.116 us |     0.109 us |     0.6104 |    0.0153 |        - |     10.18 KB |

*/