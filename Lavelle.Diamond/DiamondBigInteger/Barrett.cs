namespace Lavelle.Diamond;

public partial class DiamondBigInteger
{
    public static DiamondBigInteger ComputeBarrettMu(DiamondBigInteger n)
    {
        var k = n.LogicalBitLength();
        var twoTo2k = new DiamondBigInteger([1]) << 2 * k;
        return twoTo2k / n;
    }

    public static DiamondBigInteger BarrettReduce(DiamondBigInteger a, DiamondBigInteger n, DiamondBigInteger mu)
    {
        var k = n.LogicalBitLength();
        var q = a * mu >> 2 * k;
        var r = a - q * n;

        r = Select(r >= n, r - n, r);
        r = Select(r >= n, r - n, r);

        return Trim(r, n.LimbCount); 
    }
    
    public static DiamondBigInteger ModPowWithBarrett(DiamondBigInteger baseValue, DiamondBigInteger exponent, DiamondBigInteger modulus, DiamondBigInteger? mu = null)
    {
        mu ??= ComputeBarrettMu(modulus);
    
        var result = new DiamondBigInteger([1]);
        var baseBig = BarrettReduce(baseValue, modulus, mu);

        var expBits = exponent.GetBits();
    
        for (int i = 0; i < exponent.LimbCount * 32; i++)
        {
            var bit = expBits[i];
        
            var temp = result * baseBig;
            temp = BarrettReduce(temp, modulus, mu);
            result = Select(bit, temp, result);
        
            baseBig *= baseBig;
            baseBig = BarrettReduce(baseBig, modulus, mu);
        }
    
        return result;
    }
}