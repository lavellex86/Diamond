namespace Lavelle.Diamond;

public partial class DiamondBigInteger
{
    public static DiamondBigInteger operator &(DiamondBigInteger a, DiamondBigInteger b) => BitwiseAnd(a, b);
    public static DiamondBigInteger operator |(DiamondBigInteger a, DiamondBigInteger b) => BitwiseOr(a, b);
    public static DiamondBigInteger operator ^(DiamondBigInteger a, DiamondBigInteger b) => BitwiseXor(a, b);
    public static DiamondBigInteger operator ~(DiamondBigInteger a) => BitwiseNot(a);
    
    public uint[] GetBits()
    {
        var result = new uint[LimbCount * 32];
        for (int i = 0; i < LimbCount * 32; i++) 
            result[i] = GetBit(i);
        
        return result;
    }

    public uint GetBit(int i)
    {
        var limbIndex = i / 32;
        var bitPosition = i % 32;
        var limb = _limbs[limbIndex];
        return limb >> bitPosition & 1u;
    }

    public void ClearBit(int i)
    {
        var limbIndex = i / 32;
        var bitPosition = i % 32;
        _limbs[limbIndex] &= ~(1u << bitPosition);
    }

    public int LogicalBitLength()
    {
        var bitLength = 0;
        var foundNonZero = 0U;
        
        for (int i = LimbCount - 1; i >= 0; i--)
        {
            var limbIsNonZero = ConstantTime.IsNonZero(_limbs[i]);
            var limbBits = 32 - ConstantTime.CountLeadingZeros(_limbs[i]);
            
            var thisLimbBitLength = i * 32 + limbBits;
            
            var shouldUpdate = limbIsNonZero & ~foundNonZero;
            bitLength = (int)ConstantTime.Select(shouldUpdate, (uint)thisLimbBitLength, (uint)bitLength);
            
            foundNonZero |= limbIsNonZero;
        }
        
        return bitLength;
    }
    
    public static DiamondBigInteger BitwiseAnd(DiamondBigInteger a, DiamondBigInteger b)
    {
        int maxLen = Math.Max(a.LimbCount, b.LimbCount);
        var result = new uint[maxLen];
    
        for (int i = 0; i < maxLen; i++)
        {
            var aVal = a.TryGetLimb(i, 0);
            var bVal = b.TryGetLimb(i, 0);
            result[i] = aVal & bVal;
        }
    
        return result;
    }
    public static DiamondBigInteger BitwiseOr(DiamondBigInteger a, DiamondBigInteger b)
    {
        int maxLen = Math.Max(a.LimbCount, b.LimbCount);
        var result = new uint[maxLen];
    
        for (int i = 0; i < maxLen; i++)
        {
            var aVal = a.TryGetLimb(i, 0);
            var bVal = b.TryGetLimb(i, 0);
            result[i] = aVal | bVal;
        }
    
        return result;
    }
    public static DiamondBigInteger BitwiseXor(DiamondBigInteger a, DiamondBigInteger b)
    {
        int maxLen = Math.Max(a.LimbCount, b.LimbCount);
        var result = new uint[maxLen];
    
        for (int i = 0; i < maxLen; i++)
        {
            var aVal = a.TryGetLimb(i, 0);
            var bVal = b.TryGetLimb(i, 0);
            result[i] = aVal ^ bVal;
        }
    
        return result;
    }
    public static DiamondBigInteger BitwiseNot(DiamondBigInteger a)
    {
        var result = new uint[a.LimbCount];
        for (int i = 0; i < a.LimbCount; i++) result[i] = ~a[i];
        return result;
    }
}