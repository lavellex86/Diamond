namespace Lavelle.Diamond;

public partial class DiamondBigInteger
{
    public static DiamondBigInteger Copy(DiamondBigInteger source, int sourceStart, int destStart, int LimbCount)
    {
        var result = new uint[LimbCount];
        ConstantTime.Copy(source._limbs, sourceStart, result, destStart, LimbCount);
        return new(result);
    }

    public static DiamondBigInteger Pad(DiamondBigInteger source, int LimbCount)
    {
        var result = new uint[LimbCount];
        var minLen = Math.Min(source.LimbCount, LimbCount);
        for (int i = 0; i < minLen; i++) result[i] = source[i];
        return new(result);
    }
    public DiamondBigInteger PadTo(int LimbCount) => Pad(this, LimbCount);
    
    public static DiamondBigInteger Trim(DiamondBigInteger source, int LimbCount) => Copy(source, 0, 0, LimbCount);
    public static DiamondBigInteger Copy(DiamondBigInteger source) => Copy(source, 0, 0, source.LimbCount);

    private static DiamondBigInteger GetLow(DiamondBigInteger x, int halfSize) => Pad(x, halfSize);
    private static DiamondBigInteger GetHigh(DiamondBigInteger x, int halfSize)
    {
        if (x.LimbCount <= halfSize) return 0;
        var result = new uint[x.LimbCount - halfSize];
        ConstantTime.Copy(x._limbs, halfSize, result, 0, result.Length);
        return new DiamondBigInteger(result);
    }
}