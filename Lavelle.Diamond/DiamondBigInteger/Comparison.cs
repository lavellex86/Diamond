namespace Lavelle.Diamond;

public partial class DiamondBigInteger
{
    public static uint operator >=(DiamondBigInteger a, DiamondBigInteger b) => GreaterThanOrEqual(a, b);
    public static uint operator <=(DiamondBigInteger a, DiamondBigInteger b) => GreaterThanOrEqual(b, a);
    public static uint operator ==(DiamondBigInteger a, DiamondBigInteger b) => Equal(a, b);
    public static uint operator !=(DiamondBigInteger a, DiamondBigInteger b) => NotEqual(a, b);
    public static uint operator >(DiamondBigInteger a, DiamondBigInteger b) => GreaterThan(a, b);
    public static uint operator <(DiamondBigInteger a, DiamondBigInteger b) => LessThan(a, b);
    
    public static uint GreaterThan(DiamondBigInteger a, DiamondBigInteger b) => ConstantTime.Not(a <= b);
    public static uint LessThan(DiamondBigInteger a, DiamondBigInteger b) => ConstantTime.Not(a >= b);
    
    public static uint IsEven(DiamondBigInteger big) => ConstantTime.IsZero(big[0] & 1);
    public static uint IsOdd(DiamondBigInteger big) => ConstantTime.Not(IsEven(big));


    public static uint GreaterThanOrEqual(DiamondBigInteger a, DiamondBigInteger b)
    {
        var borrow = 0U;
        for (int i = 0; i < a.LimbCount; i++)
        {
            var aVal = a.TryGetLimb(i, 0);
            var bVal = b.TryGetLimb(i, 0);
            var diff = (ulong)aVal - bVal - borrow;
            borrow = ConstantTime.ExtractOverflowBit(diff);
        }
        return 1U - borrow;
    }

    public static uint Equal(DiamondBigInteger a, DiamondBigInteger b)
    {
        var equal = 1U;
        for (int i = 0; i < a.LimbCount; i++)
        {
            var aVal = a.TryGetLimb(i, 0);
            var bVal = b.TryGetLimb(i, 0);
            var isNonZero = ConstantTime.IsNonZero(aVal - bVal);
            equal = ConstantTime.Select(isNonZero, 0U, equal);
        }
        return equal;
    }

    public static uint NotEqual(DiamondBigInteger a, DiamondBigInteger b)
    {
        var equal = Equal(a, b);
        return ConstantTime.Not(equal);
    }
}