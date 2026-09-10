namespace Lavelle.Diamond;

public partial class DiamondBigInteger
{
    public static DiamondBigInteger Select(uint condition, DiamondBigInteger a, DiamondBigInteger b)
    {
        var maxLen = Math.Max(a.LimbCount, a.LimbCount);
        var result = new uint[maxLen];

        for (int i = 0; i < maxLen; i++)
        {
            var aVal = a.TryGetLimb(i, 0);
            var bVal = b.TryGetLimb(i, 0);
            var selected = ConstantTime.Select(condition, aVal, bVal);
            result[i] = selected;
        }
        
        return result;
    }
}