namespace Lavelle.Diamond;

public partial class DiamondBigInteger
{
    public static DiamondBigInteger operator /(DiamondBigInteger a, DiamondBigInteger b) => LongDivide(a, b).quotient;
    public static DiamondBigInteger operator %(DiamondBigInteger a, DiamondBigInteger n) => LongDivide(a, n).remainder;

    public static (DiamondBigInteger quotient, DiamondBigInteger remainder) LongDivide(DiamondBigInteger a, DiamondBigInteger b)
    {
        var quotient = new DiamondBigInteger(new uint[a.LimbCount]);
        var remainder = new DiamondBigInteger(new uint[a.LimbCount + 1]);

        for (int bitPos = a.LimbCount * 32 - 1; bitPos >= 0; bitPos--)
        {
            remainder <<= 1;

            var dividendBit = a.GetBit(bitPos);
            remainder[0] |= dividendBit;

            var canSubtract = remainder >= b;
            remainder = Select(canSubtract, remainder - b, remainder);

            var quotientLimb = bitPos / 32;
            var quotientBit = bitPos % 32;
            quotient[quotientLimb] |= canSubtract << quotientBit;
        }

        return (quotient, remainder);
    }
}