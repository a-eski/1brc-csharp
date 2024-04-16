# 1brc-csharp

### Credit to Creator and Community
The 1 Billion Row Challenge was originally a competition set up for Java programmers. It has blown up and has taken many different communities by storm. See the original blog here explaining all the details for the challenge: https://www.morling.dev/blog/one-billion-row-challenge/. The results of the original challenge can be found here: https://www.morling.dev/blog/1brc-results-are-in/.

Here is the original 1 Billion Row Challenge (1brc) repo: https://github.com/gunnarmorling/1brc

If you are interested in doing the challenge in a language other than C#, check out this page to references to repos with implementations in tons of great languages like Go, Zig, and Rust! https://1brc.dev/

If you plan on doing the challenge yourself, I would avoid looking at implementations for the language you are writing in. It allows you to explore and learn a lot more than you would going for a hyper-optimized implementation from the beginning.

I have tried not to look at any .NET (C#/F#) implementations for the 1brc in depth, but here are some great repositories I plan on exploring in depth more when I am done with this project.
https://github.com/buybackoff/1brc
https://github.com/nietras/1brc.cs
https://github.com/xoofx/Fast1BRC
https://github.com/praeclarum/1brc

### What is this project?
This project will document my journey working through the 1brc, with C# as the language (the original competition was in Java). I have been wanting to try this out for quite a while! I am interested in pushing the performance of C# to its limits, and am using this project as an experience in optimization.

I wanted this to be C# beginner-friendly, so I have left a series of implementations in the project. Each implementation built upon concepts from the previous, and the I used benchmarking and profiling to determine what kind of changes I wanted to make for the next implementation.

### Notes
I have been working with 10,000 row file for implementations and iterative process. Current implementations are left as they are, so they can be benchmarked them with 1,000,000,000 row file later (at least some of them).

Certain implementations should always be excluded from benchmarks. These implementations, like CalculateAverageNaiveClassConsoleWrite and CalculateAverageNaiveStructConcatenation, were written with the intent to make mistakes inexperienced programmers may make. You will see similar minor mistakes in CalculateAverageNaiveClass, CalculateAverageNaiveStruct, and CalculateAverageNaive. However, as mentioned, I wanted this project to be C# beginner-friendly. So, giving these poor performing examples to beginners and then providing ideas for improvement can provide beginners a great jumping off point for their own explorations.

When originally developing implementations, I always excluded writing to the Console when benchmarking. This made it easier to compare implementations. When I did start benchmarking implementations against each other with Console.Write or Console.WriteLine, I quickly noticed that this was where the bulk or the program execution time was being spent. This is how further implementations like CalculateAverageFasterConsole came about. Since part of the challenge is sending the result to standard output, how you achieve that can also be a subject for optimizations.

None of the implementations are yet compliant with the requirements. It mentions roundTowardsPositive behavior as well as only showing 1 significant digit. I will get around to this eventually. I am also still working on adding testing and I have multiple other ideas for this project!
