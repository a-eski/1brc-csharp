using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;
using _1brc_csharp_implementations.Common;
using _1brc_csharp_implementations.Models;

namespace _1brc_csharp_implementations;

/// <summary>
/// Iterating from CalculateAverageRefDictionary, continue trying to reduce allocations and duration without splitting work asynchronously or with multiple threads.
/// Memory mapped files utilize lower level win32APIs.
/// </summary>
public static class CalculateAverageMemoryMappedFile
{
    private const int PageLength = 1024; //4kb buffer length

    public static void Run()
    {
        var filePath = FilePathGetter.GetPath();
        var fileHandle = File.OpenHandle(filePath);
        var fileLength = RandomAccess.GetLength(fileHandle);

        using var memoryMappedFile = MemoryMappedFile.CreateFromFile(fileHandle, Path.GetFileName(filePath), fileLength,
            MemoryMappedFileAccess.Read, HandleInheritability.None, leaveOpen: true);
        using var viewAccessor = memoryMappedFile.CreateViewAccessor(0, fileLength, MemoryMappedFileAccess.Read);
        var memoryMappedViewHandle = viewAccessor.SafeMemoryMappedViewHandle;
        var dictionary = new Dictionary<string, WeatherValues>();

        unsafe
        {
            byte* pointer = null;
            memoryMappedViewHandle.AcquirePointer(ref pointer);

            var buffer = new byte[PageLength];
            var bufferPosition = 0;
            var unprocessedLine = "";
            for (long i = 0; i < fileLength; i += PageLength - 1)
            {
                for (long j = i; j < i + PageLength - 1; j++)
                {
                    buffer[bufferPosition++] = pointer[j];
                }

                unprocessedLine = ProcessBuffer(buffer, bufferPosition, dictionary, unprocessedLine);
                bufferPosition = 0;
            }
        }

        var sb = new StringBuilder("{");
        var index = 0;
        foreach (var weatherStation in dictionary.OrderBy(x => x.Key))
        {
            sb.Append(weatherStation.Key).Append('=')
                .Append(Math.Round(weatherStation.Value.Min, 1, MidpointRounding.ToZero)).Append(',')
                .Append(Math.Round(weatherStation.Value.Max, 1, MidpointRounding.ToZero)).Append(',')
                .Append(Math.Round(weatherStation.Value.Total / weatherStation.Value.Count, 1, MidpointRounding.ToZero));

            if (++index < dictionary.Count) sb.Append(", ");
        }
        sb.Append('}');

        var result = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(sb.ToString());
        Console.OutputEncoding = Encoding.UTF8;
        using var standardOutput = Console.OpenStandardOutput();
        standardOutput.Write(result);
    }

        
    private static string ProcessBuffer(byte[] buffer, int bytesRead, Dictionary<string, WeatherValues> dictionary, string unprocessedLine)
    {
        var bufferAsString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        var lines = bufferAsString.Split([Environment.NewLine], StringSplitOptions.None);

        if (unprocessedLine != "")
            lines[0] = unprocessedLine + lines[0];
        if (lines[0].Length > 0 && lines[0][0] == '\n')
            lines[0] = lines[1..lines.Length].ToString()!;

        for (var index = 0; index < lines.Length; index++)
        {
            if (lines[index].Length == 0)
                continue;
            var lineSpan = lines[index].AsSpan();
            var semicolonIndex = lineSpan.IndexOf(';');
            if (semicolonIndex == -1 || semicolonIndex + 1 >= lineSpan.Length)
                return lines[index];
            var dotIndex = lineSpan.IndexOf('.');
            if (dotIndex == -1 || lineSpan[^1] == '.')
                return lines[index];
            
            var weatherStationName = new string(lineSpan[..semicolonIndex]);
            var newValue = float.Parse(lineSpan[(semicolonIndex + 1)..]);
            if (newValue > 99.901 || newValue < -99.901)//outside valid range, probably some line data still in next chunk.
                return lines[index];

            ref var values = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, weatherStationName, out var exists);

            if (!exists)
            {
                values.Count++;
                values.Min = newValue;
                values.Max = newValue;
                values.Total = newValue;
                continue;
            }

            values.Count++;
            if (newValue < values.Min) values.Min = newValue;
            if (newValue > values.Max) values.Max = newValue;
            values.Total += newValue;
        }

        return "";
    }
}
