using System.Diagnostics;
using _1brc_csharp_implementations;

var stopwatch = new Stopwatch();
stopwatch.Start();

//CalculateAverageNaiveClassConsoleWrite.Run(); 
//CalculateAverageNaiveStructConcatenation.Run();
//CalculateAverageNaiveList.Run();
//CalculateAverageNaiveStruct.Run();
//CalculateAverageNaiveClass.Run();
//CalculateAverageNaive.Run();
//CalculateAverageStringBuilder.Run();
//CalculateAverageSpan.Run();
//CalculateAverageSortedDictionary.Run();
//CalculateAverageStruct.Run();     //doesn't produce correct results
//CalculateAverageSpan2.Run();
//await CalculateAverageAsync.Run();
//CalculateAverageStreamReader.Run(); 
//CalculateAverageFasterConsole.Run();
//CalculateAverageStructFasterConsole.Run();  //doesn't produce correct results
//CalculateAverageRefDictionary.Run();
CalculateAverageStruct2.Run();
//CalculateAverageMemoryMappedFile.Run();

stopwatch.Stop();
Console.WriteLine($"Time elapsed: {stopwatch.ElapsedMilliseconds}ms");