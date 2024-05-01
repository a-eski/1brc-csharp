﻿using System.IO.MemoryMappedFiles;
using System.Text;
using _1brc_csharp_implementations.Common;

namespace _1brc_csharp_implementations;

/// <summary>
/// Iterating from CalculateAverageFasterConsole, continue trying to reduce allocations and duration without splitting work asynchronously or with multiple threads.
/// Memory mapped files utilize lower level win32APIs.
/// </summary>
public static class CalculateAverageMemoryMappedFile
{
    private const int BufferLength = 4096;
    private const byte Separator = (byte)';';
    
    public static void Run()
    {
        var filePath = FilePathGetter.GetPath();
        var fileInfo = new FileInfo(filePath);
        var fileLength = fileInfo.Length;

        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None, BufferLength, FileOptions.SequentialScan);
        using var mmf = MemoryMappedFile.CreateFromFile(fs, null, 0, MemoryMappedFileAccess.Read, HandleInheritability.None, true);
        using var accessor = mmf.CreateViewAccessor();

        using var sr = File.OpenText(filePath);
        var dictionary = new Dictionary<string, float[]>();//array is length 4. count, min, max, total. mean calculated at end, to avoid unnecessary division operations.
        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();
            var lineSpan = line.AsSpan();
            var semicolonIndex = lineSpan.IndexOf(';');
            var weatherStationName = new string(lineSpan[..semicolonIndex]);
            var newValue = float.Parse(lineSpan[(semicolonIndex + 1)..]);

            if (!dictionary.TryGetValue(weatherStationName, out var values))
            {
                dictionary.Add(weatherStationName, [1.0f, newValue, newValue, newValue]);
                continue;
            }

            values[0]++;
            if (newValue < values[1]) values[1] = newValue;
            if (newValue > values[2]) values[2] = newValue;
            values[3] += newValue;
        }

        var sb = new StringBuilder("{");
        var index = 0;
        foreach (var weatherStation in dictionary.OrderBy(x => x.Key))
        {
            sb.Append(weatherStation.Key).Append('=')
                .Append(Math.Round(weatherStation.Value[1], 1, MidpointRounding.ToZero)).Append(',')
                .Append(Math.Round(weatherStation.Value[2], 1, MidpointRounding.ToZero)).Append(',')
                .Append(Math.Round(weatherStation.Value[3] / weatherStation.Value[0], 1, MidpointRounding.ToZero));

            if (++index < dictionary.Count) sb.Append(", ");
        }
        sb.Append('}');

        var result = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(sb.ToString());
        Console.OutputEncoding = Encoding.UTF8;
        using var standardOutput = Console.OpenStandardOutput();
        standardOutput.Write(result);
    }
}
