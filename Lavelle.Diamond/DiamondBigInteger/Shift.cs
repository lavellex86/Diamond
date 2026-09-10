namespace Lavelle.Diamond;

public partial class DiamondBigInteger
{
    public static DiamondBigInteger operator <<(DiamondBigInteger big, int shift) => LeftShift(big, shift);
    public static DiamondBigInteger operator >>(DiamondBigInteger big, int shift) => RightShift(big, shift);
    public static DiamondBigInteger LeftShift(DiamondBigInteger big, int totalShift)
    {
        if (totalShift < 0) return RightShift(big, -totalShift);
        var limbShift = totalShift / 32;
        var bitShift = totalShift % 32;

        var resultLength = big.LimbCount + limbShift + (int)ConstantTime.IsPositive(bitShift);
        var result = new uint[resultLength];
    
        for (int i = 0; i < resultLength; i++)
        {
            var source = i - limbShift;

            var low = big.TryGetLimb(source, 0);
            var high = big.TryGetLimb(source - 1, 0);
        
            var needsHigh = (uint)(-bitShift >> 31) & 1;
            var rightShiftAmount = ConstantTime.Select(needsHigh, (uint)(32 - bitShift), 0U);
            var highPart = ConstantTime.Select(needsHigh, high >> (int)rightShiftAmount, 0U);
        
            var shifted = low << bitShift | highPart;
            result[i] = shifted;
        }

        return new(result);
    }
    
    public static DiamondBigInteger RightShift(DiamondBigInteger big, int totalShift)
    {
        if (totalShift < 0) return LeftShift(big, -totalShift);
        var limbShift = totalShift / 32;
        var bitShift = totalShift % 32;
    
        var resultLength = Math.Max(1, big.LimbCount - limbShift);
        var result = new uint[resultLength];
    
        for (int i = 0; i < resultLength; i++)
        {
            var source = i + limbShift;

            var low = big.TryGetLimb(source, 0);
            var high = big.TryGetLimb(source + 1, 0);
        
            var needsHigh = (uint)(-bitShift >> 31) & 1;
            var leftShiftAmount = ConstantTime.Select(needsHigh, (uint)(32 - bitShift), 0U);
            var highPart = ConstantTime.Select(needsHigh, high << (int)leftShiftAmount, 0U);
        
            var shifted = low >> bitShift | highPart;
            result[i] = shifted;
        }

        return new(result);
    }
    
    #region Safe
    public static DiamondBigInteger SafeLeftShift(DiamondBigInteger value, int shiftBits, int maxShiftBits)
    {
        if (shiftBits < 0) return RightShift(value, -shiftBits);
        
        var maxShiftLimbs = maxShiftBits / 32;
        var resultSize = value.LimbCount + maxShiftLimbs + 1;
        var result = new uint[resultSize];
    
        var shiftLimbs = shiftBits / 32;
        var shiftRemainder = shiftBits % 32;
        var leftShift = 32 - shiftRemainder;
    
        var carry = 0U;
        for (int i = 0; i < value.LimbCount; i++)
        {
            var val = value[i];
            var shifted = (val << shiftRemainder) | carry;
            carry = ConstantTime.Select(ConstantTime.IsNonZero((uint)shiftRemainder), val >> leftShift, 0U);
            result[i + shiftLimbs] = shifted;
        }
        result[value.LimbCount + shiftLimbs] = carry;
    
        return new(result);
    }
    #endregion
}