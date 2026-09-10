namespace Lavelle.Diamond;

public partial class DiamondBigInteger
{
    public static DiamondBigInteger ComputeNPrime(DiamondBigInteger N, int k)
    {
        var n0 = N[0];

        var nPrime = 1UL;
        for (int bits = 2; bits <= 32; bits *= 2)
        {
            var mask = (1UL << bits) - 1;
            var temp = (n0 * nPrime) & mask;
            nPrime = (nPrime * ((2UL - temp) & mask)) & mask;
        }
        
        var limbCount = Math.Max(2, (k + 31) / 32);
        var result = new uint[limbCount];
        result[0] = (uint)nPrime;
    
        var needsSecondLimb = ConstantTime.GreaterThan(limbCount, 1);
        result[1] = ConstantTime.Select(needsSecondLimb, (uint)(nPrime >> 32), 0U);
        
        var nPrimeBig = new DiamondBigInteger(result);
        var R = new DiamondBigInteger([1]) << k;
    
        return (R - nPrimeBig) % R;
    }

    public static DiamondBigInteger MontgomeryReduce(DiamondBigInteger T, MontgomeryContext ctx)
    {
        var m = Copy(T);
    
        for (int i = 0; i < ctx.N.LimbCount; i++)
        {
            var u_i = m[0] * ctx.NPrime[0];
            var uN = u_i * ctx.N;
            m += uN;
            m >>= 32;
        }
    
        m = Select(m >= ctx.N, m - ctx.N, m);
        return Trim(m, ctx.N.LimbCount);
    }
    
    public static DiamondBigInteger ModPowWithMontgomery(DiamondBigInteger baseValue, DiamondBigInteger exponent, DiamondBigInteger modulus, MontgomeryContext? ctx = null)
    {
        ctx ??= new MontgomeryContext(modulus);
    
        var baseMont = ctx.ToMontgomery(baseValue);
        var resultMont = ctx.ToMontgomery(1);
    
        var expBits = exponent.GetBits();
    
        for (int i = 0; i < exponent.LogicalBitLength(); i++)
        {
            var bit = expBits[i];
        
            var temp = ctx.Multiply(resultMont, baseMont);
            resultMont = Select(bit, temp, resultMont);
            baseMont = ctx.Multiply(baseMont, baseMont);
        }
    
        return ctx.FromMontgomery(resultMont);
    }
}

public class MontgomeryContext
{
    public DiamondBigInteger N { get; }
    public DiamondBigInteger NPrime { get; }
    public DiamondBigInteger R { get; }
    public int K { get; }

    public MontgomeryContext(DiamondBigInteger n)
    {
        N = n;
        K = (n.LogicalBitLength() + 31) / 32 * 32;
        R = new DiamondBigInteger([1]) << K;
        NPrime = DiamondBigInteger.ComputeNPrime(n, K);
    }
    
    public DiamondBigInteger ToMontgomery(DiamondBigInteger big) => big * R % N;
    public DiamondBigInteger FromMontgomery(DiamondBigInteger big) => DiamondBigInteger.MontgomeryReduce(big, this);

    public DiamondBigInteger Multiply(DiamondBigInteger a, DiamondBigInteger b) => DiamondBigInteger.MontgomeryReduce(a * b, this);
}