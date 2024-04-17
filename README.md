# 1brc-csharp

### Credit to Creators
The 1 Billion Row Challenge was originally a competition set up for Java programmers. It has blown up and has taken many MANY different communities by storm. Instead of going through the challenge and all its details here, please reference the original:
* Here is the original 1 Billion Row Challenge (1brc) repo: https://github.com/gunnarmorling/1brc. This has all the details and requirements of the challenge.
* See the original blog here explaining all the details for the challenge: https://www.morling.dev/blog/one-billion-row-challenge/.
* The results of the original Java challenge can be found here: https://www.morling.dev/blog/1brc-results-are-in/.

### Interesting Implementations by the .NET Community
If you plan on doing the challenge yourself, I would avoid looking at implementations for the language you are writing in. It allows you to explore and learn a lot more than you would going for a hyper-optimized implementation from the beginning.

I have tried not to look at any .NET (C#/F#) implementations for the 1brc in depth, but here are some great repositories I plan on exploring in depth more when I am further along with this project.
https://github.com/buybackoff/1brc
https://github.com/nietras/1brc.cs
https://github.com/xoofx/Fast1BRC
https://github.com/praeclarum/1brc

### 1brc.dev
If you are interested in doing the challenge in a language other than C#, check out this page with references to repos with implementations in tons of great languages like Go, Zig, and Rust: https://1brc.dev/.

### What is this project?
This project will document my journey working through the 1brc, with C# as the language (the original competition was in Java). I have been wanting to try this out for quite a while! I am interested in pushing the performance of C# to its limits, and am using this project as an experience in optimizations.

I wanted this to be C# beginner-friendly, so I have left a series of implementations in the project 1brc-csharp-implementations. Each implementation built upon concepts or ideas from previous implementations, and I used benchmarking and profiling to determine what kind of changes I wanted to make for the different implementations.

### Notes
I had been working with 10,000 row weather_data.csv file for implementations and iterative process. I now set up 10,000 row measurements.txt files in  Current implementations are left as they are, so they can be benchmarked them with 1,000,000,000 row file later (at least some of them).

Certain implementations should always be excluded from benchmarks. These implementations, like CalculateAverageNaiveClassConsoleWrite and CalculateAverageNaiveStructConcatenation, were written with the intent to make mistakes inexperienced programmers may make. You will see similar minor mistakes in CalculateAverageNaiveClass, CalculateAverageNaiveStruct, and CalculateAverageNaive. However, as mentioned, I wanted this project to be C# beginner-friendly. So, giving these poor performing examples to beginners and then providing ideas for improvement can provide beginners a great jumping off point for their own explorations.

When originally developing implementations, I always excluded writing to the Console when benchmarking. This made it easier to compare implementations. When I did start benchmarking implementations against each other with Console.Write or Console.WriteLine, I quickly noticed that this was where the bulk or the program execution time was being spent. This is how further implementations like CalculateAverageFasterConsole came about. Since part of the challenge is sending the result to standard output, how you achieve that can also be a subject for optimizations.

None of the implementations are yet compliant with the requirements. It mentions roundTowardsPositive behavior as well as only showing 1 significant digit. I will get around to this eventually. I am also still working on adding testing and I have multiple other ideas for this project!

### Generating measurements.txt's
I didn't want to include measurements.txt with 10,000, 100,000, 1,000,000, and 1,000,000,000 rows in this repo. Hence, I have a modified version of the original challenge's python script to generate measurements.txt in python/create_measurements.py. The challenge notes that this is the non-authoritative way to generate measurements.txt, but it works just fine for our purposes. Maybe I will set up the java version or rewrite it in C# at some point.

To generate data, you will need python installed. The only arg you need to pass in is the number of rows you want in the output measurements.txt file. Please note, I only tested this script on Windows (the file from the java challenge wasn't working, I had to specify encoding of weather_data and measurements files in the python script).

To run, navigate to "1brc-csharp\python". The weather_data.csv file must be present in the directory as well. Then, run this to generate a measurements.txt file (changing 10_000 to the number of rows you want):
```
py ./create_measurements.py 10_000
```