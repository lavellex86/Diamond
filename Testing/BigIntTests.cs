using System.Diagnostics;
using System.Numerics;
using Lavelle.Diamond;

namespace Testing;

public static class BigIntTests
{
    private static readonly Random Random = new();
    
    #region Add/Subtract
    public static void TestAdd()
    {
        var a = new DiamondBigInteger(43);
        var b = new DiamondBigInteger(12);
        
        var sw = Stopwatch.StartNew();
        var result = a + b;
        sw.Stop();
        Console.WriteLine($"simple addition: {a} + {b} = {result}, took {sw.ElapsedMilliseconds}ms");

        a = GenerateRandomBigInt(20);
        b = GenerateRandomBigInt(10);
        
        sw = Stopwatch.StartNew();
        result = a + b;
        sw.Stop();
        Console.WriteLine($"larger addition: {a} + {b} = {result}, took {sw.ElapsedMilliseconds}ms");
        
        a = GenerateRandomBigInt(1000);
        b = GenerateRandomBigInt(1000);
        
        sw = Stopwatch.StartNew();
        result = a + b;
        sw.Stop();
        Console.WriteLine($"huge addition with size {a.LimbCount} + size {b.LimbCount} took {sw.ElapsedMilliseconds}ms");
    }

    public static void TestSubtract()
    {
        var a = new DiamondBigInteger(43);
        var b = new DiamondBigInteger(12);
        
        var sw = Stopwatch.StartNew();
        var result = a - b;
        sw.Stop();
        Console.WriteLine($"simple subtraction: {a} - {b} = {result}, took {sw.ElapsedMilliseconds}ms");
        
        a = GenerateRandomBigInt(20);
        b = GenerateRandomBigInt(10);
        
        sw = Stopwatch.StartNew();
        result = a - b;
        sw.Stop();
        Console.WriteLine($"larger subtraction: {a} - {b} = {result}, took {sw.ElapsedMilliseconds}ms");
        
        a = GenerateRandomBigInt(1000);
        b = GenerateRandomBigInt(1000);
        
        sw = Stopwatch.StartNew();
        result = a - b;
        sw.Stop();
        Console.WriteLine($"huge subtraction with size {a.LimbCount} - size {b.LimbCount} took {sw.ElapsedMilliseconds}ms");
    }
    #endregion
    #region Multiply/Divide/Mod
    public static void TestMultiply()
    {
        var a = new DiamondBigInteger(43);
        var b = new DiamondBigInteger(12);
        
        var sw = Stopwatch.StartNew();
        var result = a * b;
        sw.Stop();
        Console.WriteLine($"simple multiplication: {a} * {b} = {result}, took {sw.ElapsedMilliseconds}ms");
        
        a = GenerateRandomBigInt(20);
        b = GenerateRandomBigInt(10);
        
        sw = Stopwatch.StartNew();
        result = a * b;
        sw.Stop();
        Console.WriteLine($"larger multiplication: {a} * {b} = {result}, took {sw.ElapsedMilliseconds}ms");

        a = GenerateRandomBigInt(128);
        b = GenerateRandomBigInt(128);
        
        sw = Stopwatch.StartNew();
        result = a * b;
        sw.Stop();
        Console.WriteLine($"big multiplication with size {a.LimbCount} * size {b.LimbCount} took {sw.ElapsedMilliseconds}ms");
        
        a = GenerateRandomBigInt(1000);
        b = GenerateRandomBigInt(1000);
        
        sw = Stopwatch.StartNew();
        result = a * b;
        sw.Stop();
        Console.WriteLine($"huge multiplication with size {a.LimbCount} * size {b.LimbCount} took {sw.ElapsedMilliseconds}ms");
    }

    public static void TestDivide()
    {
        var a = new DiamondBigInteger(43);
        var b = new DiamondBigInteger(12);
        
        var sw = Stopwatch.StartNew();
        var result = a / b;
        sw.Stop();
        Console.WriteLine($"simple division: {a} / {b} = {result}, took {sw.ElapsedMilliseconds}ms");
        
        a = GenerateRandomBigInt(20);
        b = GenerateRandomBigInt(10);
        
        sw = Stopwatch.StartNew();
        result = a / b;
        sw.Stop();
        Console.WriteLine($"larger division: {a} / {b} = {result}, took {sw.ElapsedMilliseconds}ms");
        
        a = GenerateRandomBigInt(128);
        b = GenerateRandomBigInt(128);
        
        sw = Stopwatch.StartNew();
        result = a / b;
        sw.Stop();
        Console.WriteLine($"big division with size {a.LimbCount} / size {b.LimbCount} took {sw.ElapsedMilliseconds}ms");
    }

    public static void TestMod()
    {
        var a = new DiamondBigInteger(43);
        var b = new DiamondBigInteger(12);
        
        var sw = Stopwatch.StartNew();
        var result = a % b;
        sw.Stop();
        Console.WriteLine($"simple modulus: {a} % {b} = {result}, took {sw.ElapsedMilliseconds}ms");
        
        a = GenerateRandomBigInt(20);
        b = GenerateRandomBigInt(10);
        
        sw = Stopwatch.StartNew();
        result = a % b;
        sw.Stop();
        Console.WriteLine($"larger modulus: {a} % {b} = {result}, took {sw.ElapsedMilliseconds}ms");
        
        a = GenerateRandomBigInt(128);
        b = GenerateRandomBigInt(128);
        
        sw = Stopwatch.StartNew();
        result = a % b;
        sw.Stop();
        Console.WriteLine($"big modulus with size {a.LimbCount} % size {b.LimbCount} took {sw.ElapsedMilliseconds}ms");
    }
    #endregion
    #region GCD
    public static void TestGCD()
    {
        var a = new DiamondBigInteger(48);
        var b = new DiamondBigInteger(18);

        var sw = Stopwatch.StartNew();
        var result = DiamondBigInteger.GCD(a, b);
        sw.Stop();
        Console.WriteLine($"GCD({a}, {b}) = {result}, took {sw.ElapsedMilliseconds}ms");

        a = new DiamondBigInteger(101);
        b = new DiamondBigInteger(103);
        
        sw = Stopwatch.StartNew();
        result = DiamondBigInteger.GCD(a, b);
        sw.Stop();
        Console.WriteLine($"GCD({a}, {b}) = {result}, took {sw.ElapsedMilliseconds}ms");
        
        a = GenerateRandomBigInt(20);
        b = GenerateRandomBigInt(10);
        
        sw = Stopwatch.StartNew();
        result = DiamondBigInteger.GCD(a, b);
        sw.Stop();
        Console.WriteLine($"GCD({a}, {b}) = {result}, took {sw.ElapsedMilliseconds}ms");
    }

    public static void TestModInverse()
    {
        var a = new DiamondBigInteger(3);
        var b = new DiamondBigInteger(11);
        
        var sw = Stopwatch.StartNew();
        var result = DiamondBigInteger.ModInverse(a, b);
        sw.Stop();
        Console.WriteLine($"ModInverse({a}, {b}) = {result}, took {sw.ElapsedMilliseconds}ms");

        a = new DiamondBigInteger(10);
        b = new DiamondBigInteger(15);
        
        sw = Stopwatch.StartNew();
        result = DiamondBigInteger.ModInverse(a, b);
        sw.Stop();
        Console.WriteLine($"ModInverse({a}, {b}) = {result}, took {sw.ElapsedMilliseconds}ms");
        
        a = GenerateRandomBigInt(20);
        b = GenerateRandomBigInt(10);
        
        sw = Stopwatch.StartNew();
        result = DiamondBigInteger.ModInverse(a, b);
        sw.Stop();
        Console.WriteLine($"ModInverse({a}, {b}) = {result}, took {sw.ElapsedMilliseconds}ms");
    }
    #endregion
    #region Barrett
    public static void TestBarrett()
    {
        var baseBig = new DiamondBigInteger(7);
        var exponent = new DiamondBigInteger(3);
        var modulus = new DiamondBigInteger(13);
        
        var sw = Stopwatch.StartNew();
        var result = DiamondBigInteger.ModPowWithBarrett(baseBig, exponent, modulus);
        sw.Stop();
        Console.WriteLine($"ModPow({baseBig}, {exponent}, {modulus}) = {result}, expected 0x5 ({sw.ElapsedMilliseconds}ms)");

        baseBig = new DiamondBigInteger(5);
        exponent = new DiamondBigInteger(10);
        modulus = new DiamondBigInteger(221);
        
        sw = Stopwatch.StartNew();
        result = DiamondBigInteger.ModPowWithBarrett(baseBig, exponent, modulus);
        sw.Stop();
        Console.WriteLine($"ModPow({baseBig}, {exponent}, {modulus}) = {result}, expected {BigInteger.ModPow(5, 10, 221)} ({sw.ElapsedMilliseconds}ms)");
    }
    #endregion
    #region Monty
    public static void TestMonty()
    {
        var n = new DiamondBigInteger(13);
        var ctx = new MontgomeryContext(n);

        Console.WriteLine($"N = {n}");
        Console.WriteLine($"K = {ctx.K}");
        Console.WriteLine($"R = {ctx.R}");
        Console.WriteLine($"N' = {ctx.NPrime}");
        
        var a = new DiamondBigInteger(7);
        var aMont = ctx.ToMontgomery(a);
        var aBack = ctx.FromMontgomery(aMont);
        Console.WriteLine($"a = {a}, in Montgomery = {aMont}, back = {aBack}");

        var b = new DiamondBigInteger(5);
        var bMont = ctx.ToMontgomery(b);
        var resultMont = ctx.Multiply(aMont, bMont);
        var result = ctx.FromMontgomery(resultMont);
        Console.WriteLine($"{a} * {b} mod {n} = {result}, expected 0x9");
        
        var baseBig = new DiamondBigInteger(7);
        var exponent = new DiamondBigInteger(3);
        var modulus = new DiamondBigInteger(13);

        var sw = Stopwatch.StartNew();
        result = DiamondBigInteger.ModPowWithMontgomery(baseBig, exponent, modulus);
        sw.Stop();
        Console.WriteLine($"ModPow({baseBig}, {exponent}, {modulus}) = {result}, expected 0x5 ({sw.ElapsedMilliseconds}ms)");

        baseBig = new DiamondBigInteger(5);
        exponent = new DiamondBigInteger(10);
        modulus = new DiamondBigInteger(221);
        
        sw = Stopwatch.StartNew();
        result = DiamondBigInteger.ModPowWithMontgomery(baseBig, exponent, modulus);
        sw.Stop();
        Console.WriteLine($"ModPow({baseBig}, {exponent}, {modulus}) = {result}, expected {BigInteger.ModPow(5, 10, 221)} ({sw.ElapsedMilliseconds}ms)");
    }
    #endregion

    
    public static DiamondBigInteger GenerateRandomBigInt(int wordCount)
    {
        uint[] result = new uint[wordCount];
        for (int i = 0; i < wordCount; i++) 
            result[i] = (uint)Random.Next() | (uint)Random.Next() << 31;
    
        if (result[wordCount - 1] == 0) result[wordCount - 1] = 1;
    
        return new DiamondBigInteger(result);
    }
    
    public static BigInteger ToPositiveBigInteger(DiamondBigInteger sbi)
    {
        var bytes = sbi.ToBytes();
        if (bytes[^1] < 0x80) return new BigInteger(bytes);
        
        var newBytes = new byte[bytes.Length + 1];
        Array.Copy(bytes, newBytes, bytes.Length);
        newBytes[bytes.Length] = 0;
        return new BigInteger(newBytes);
    }
}