namespace Lavelle.Diamond;

public partial class DiamondBigInteger
{
    public static DiamondBigInteger operator *(DiamondBigInteger a, DiamondBigInteger b) => KaratsubaMultiply(a, b);
    
    #region Schoolbook
    public static DiamondBigInteger StandardMultiply(DiamondBigInteger a, DiamondBigInteger b)
    {
        var resultSize = a.LimbCount + b.LimbCount;
        var result = new uint[resultSize];

        for (int i = 0; i < a.LimbCount; i++)
        {
            var aVal = a.TryGetLimb(i, 0);
            var carry = 0UL;
            for (int j = 0; j < b.LimbCount; j++)
            {
                var bVal = b.TryGetLimb(j, 0);
                var product = (ulong)aVal * bVal + result[i + j] + carry;
                result[i + j] = (uint)product;
                carry = product >> 32;
            }
            result[i + b.LimbCount] = (uint)carry;
        }
        
        return result;
    }
    #endregion
    #region Karatsuba
    private static DiamondBigInteger CombineKaratsuba(DiamondBigInteger z2, DiamondBigInteger z1, DiamondBigInteger z0, int halfSize, uint z1Negative)
    {
        var resultSize = 2 * halfSize + z2.LimbCount + 1;
        var result = new uint[resultSize];
        
        AddInto(result, 0, z0);
        AddOrSubtractInto(result, halfSize, z1, z1Negative);
        AddInto(result, 2 * halfSize, z2);

        return new(result);
    }

    public static DiamondBigInteger KaratsubaMultiply(DiamondBigInteger a, DiamondBigInteger b)
    {
        if (a.LimbCount < 32 || b.LimbCount < 32) return StandardMultiply(a, b);
        
        var halfSize = Math.Max(a.LimbCount, b.LimbCount) / 2;
        
        var a0 = GetLow(a, halfSize);
        var a1 = GetHigh(a, halfSize);
        var b0 = GetLow(b, halfSize);
        var b1 = GetHigh(b, halfSize);
        
        var z0 = KaratsubaMultiply(a0, b0);
        var z2 = KaratsubaMultiply(a1, b1);
        var z1 = KaratsubaMultiply(a0 + a1, b0 + b1);
        z1 = Subtract(z1 - z0, z2, out var borrow);
        
        return CombineKaratsuba(z2, z1, z0, halfSize, borrow);
    }
    #endregion
}