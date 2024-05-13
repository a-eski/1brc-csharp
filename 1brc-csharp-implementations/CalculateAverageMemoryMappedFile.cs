using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;
using _1brc_csharp_implementations.Common;
using _1brc_csharp_implementations.Constants;

namespace _1brc_csharp_implementations;

/// <summary>
/// Iterating from CalculateAverageRefDictionary, continue trying to reduce allocations and duration without splitting work asynchronously or with multiple threads.
/// Memory mapped files utilize lower level win32APIs.
/// </summary>
public static class CalculateAverageMemoryMappedFile
{
    private const int BufferLength = 4096; //4kb buffer length

    public static void Run()
    {
        //still in progress, working through implementation
        var filePath = FilePathGetter.GetPath();
        //var length = new FileInfo(filePath).Length;

        using var mmf = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open, null, 0, MemoryMappedFileAccess.Read);
        using var stream = mmf.CreateViewStream(0, 0, MemoryMappedFileAccess.Read);
        var buffer = new byte[BufferLength];
        int bytesRead;
        var dictionary =
            new Dictionary<string, float[]>(); //array is length 4. count, min, max, total. mean calculated at end, to avoid unnecessary division operations.
        var unprocessedLine = "";
        
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            unprocessedLine = ProcessBuffer(buffer, bytesRead, dictionary, unprocessedLine);
        }

        var sb = new StringBuilder("{");
        var index = 0;
        foreach (var weatherStation in dictionary.OrderBy(x => x.Key))
        {
            sb.Append(weatherStation.Key).Append('=')
                .Append(Math.Round(weatherStation.Value[Indices.Minimum], 1, MidpointRounding.ToZero)).Append(',')
                .Append(Math.Round(weatherStation.Value[Indices.Maximum], 1, MidpointRounding.ToZero)).Append(',')
                .Append(Math.Round(weatherStation.Value[Indices.Total] / weatherStation.Value[Indices.Count], 1, MidpointRounding.ToZero));

            if (++index < dictionary.Count) sb.Append(", ");
        }

        sb.Append('}');

        var result = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(sb.ToString());
        Console.OutputEncoding = Encoding.UTF8;
        using var standardOutput = Console.OpenStandardOutput();
        standardOutput.Write(result);
    }

    private static string ProcessBuffer(byte[] buffer, int bytesRead, Dictionary<string, float[]> dictionary, string unprocessedLine)
    {
        var bufferAsString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        var lines = bufferAsString.Split([Environment.NewLine], StringSplitOptions.None);

        for (var index = 0; index < lines.Length; index++)
        {
            //if (index == 0 && !string.IsNullOrWhiteSpace(unprocessedLine))
            if (lines[index].Length == 0)
                continue;
            var lineSpan = lines[index].AsSpan();
            var semicolonIndex = lineSpan.IndexOf(';');
            if (semicolonIndex == -1 || semicolonIndex + 1 >= lineSpan.Length)//no semicolon or only 1 value after semicolon
                return lines[index];
            
            var weatherStationName = new string(lineSpan[..semicolonIndex]);
            var newValue = float.Parse(lineSpan[(semicolonIndex + 1)..]);
            if (newValue > 99.9 || newValue < -99.9)//outside valid range, probably some line data still in next chunk.
                return lines[index];
                

            ref var values =
                ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, weatherStationName, out var exists);

            if (!exists || values == null)
            {
                values = [1.0f, newValue, newValue, newValue];
                continue;
            }

            values[Indices.Count]++;
            if (newValue < values[Indices.Minimum]) values[Indices.Minimum] = newValue;
            if (newValue > values[Indices.Maximum]) values[Indices.Maximum] = newValue;
            values[Indices.Total] += newValue;
        }

        return "";
    }
}