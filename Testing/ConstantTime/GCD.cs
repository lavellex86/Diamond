using Lavelle.Diamond;

namespace Testing.ConstantTime;

public static partial class Analytics
{
    public static void TestGCD()
    {
        Console.WriteLine("Testing GCD with coprime and non-coprime inputs...");
        var random = new Random();
        const int wordCount = 4;
        const int warmup = 1000;
        const int iterationsMultiplier = 1;

        Console.WriteLine($"Generating {warmup} random number pairs...");
        
        List<DiamondBigInteger> coprimeA = [];
        List<DiamondBigInteger> coprimeB = [];
        List<DiamondBigInteger> nonCoprimeA = [];
        List<DiamondBigInteger> nonCoprimeB = [];
        
        for (int i = 0; i < warmup; i++)
        {
            var a = GenerateRandomBigInt(wordCount, random) | 1;
            var b = GenerateRandomBigInt(wordCount, random) | 1;
            coprimeA.Add(a);
            coprimeB.Add(b);
            
            var commonFactor = GenerateRandomBigInt(2, random) | 1;
            var x = GenerateRandomBigInt(wordCount - 2, random) | 1;
            var y = GenerateRandomBigInt(wordCount - 2, random) | 1;
            nonCoprimeA.Add(x * commonFactor);
            nonCoprimeB.Add(y * commonFactor);
        }

        Console.WriteLine("Warming up...");
        for (int i = 0; i < warmup; i++)
        {
            DiamondBigInteger.GCD(coprimeA[i], coprimeB[i]);
            DiamondBigInteger.GCD(nonCoprimeA[i], nonCoprimeB[i]);
        }

        List<long> coprimeTimes = [];
        List<long> nonCoprimeTimes = [];

        var samples = iterationsMultiplier * warmup;
        Console.WriteLine($"Running {samples} samples...");
        
        for (int i = 0; i < samples; i++)
        {
            var sample = i % warmup;
            coprimeTimes.Add(TimeOperation(() => DiamondBigInteger.GCD(coprimeA[sample], coprimeB[sample])));
            nonCoprimeTimes.Add(TimeOperation(() => DiamondBigInteger.GCD(nonCoprimeA[sample], nonCoprimeB[sample])));
        }
        
        AnalyzeResults("GCD Coprime vs Non-Coprime", coprimeTimes, nonCoprimeTimes);
    }

    public static void TestGCDEvenVsOdd()
    {
        Console.WriteLine("Testing GCD with even vs odd inputs...");
        var random = new Random();
        const int wordCount = 4;
        const int warmup = 1000;
        const int iterationsMultiplier = 1;

        List<DiamondBigInteger> oddA = [];
        List<DiamondBigInteger> oddB = [];
        List<DiamondBigInteger> evenA = [];
        List<DiamondBigInteger> evenB = [];

        for (int i = 0; i < warmup; i++)
        {
            oddA.Add(GenerateRandomBigInt(wordCount, random) | 1);
            oddB.Add(GenerateRandomBigInt(wordCount, random) | 1);

            var shiftAmount = random.Next(1, 10);
            evenA.Add(GenerateRandomBigInt(wordCount, random) << shiftAmount);
            evenB.Add(GenerateRandomBigInt(wordCount, random) << shiftAmount);
        }
        
        Console.WriteLine("Warming up...");
        for (int i = 0; i < warmup; i++)
        {
            DiamondBigInteger.GCD(oddA[i], oddB[i]);
            DiamondBigInteger.GCD(evenA[i], evenB[i]);
        }
        
        List<long> evenTimes = [];
        List<long> oddTimes = [];

        var samples = iterationsMultiplier * warmup;
        Console.WriteLine($"Running {samples} samples...");
        
        for (int i = 0; i < samples; i++)
        {
            var sample = i % warmup;
            evenTimes.Add(TimeOperation(() => DiamondBigInteger.GCD(evenA[sample], evenB[sample])));
            oddTimes.Add(TimeOperation(() => DiamondBigInteger.GCD(oddA[sample], oddB[sample])));
        }
        
        AnalyzeResults("GCD Even vs Odd", evenTimes, oddTimes);
    }
}