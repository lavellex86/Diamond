using ILGPU;
using ILGPU.Runtime;

namespace Lavelle.Diamond;

public partial class DiamondBigInteger
{
    public static DiamondBigInteger operator +(DiamondBigInteger a, DiamondBigInteger b) => Add(a, b);
    public static DiamondBigInteger operator -(DiamondBigInteger a, DiamondBigInteger b) => Subtract(a, b, out _);
    
    public static DiamondBigInteger Add(DiamondBigInteger a, DiamondBigInteger b)
    {
        var maxLen = Math.Max(a.LimbCount, b.LimbCount);
        
        var carry = 0U;
        var result = new uint[maxLen + 1];
        for (int i = 0; i < maxLen; i++)
        {
            var aVal = a.TryGetLimb(i, 0);
            var bVal = b.TryGetLimb(i, 0);
            var sum = (ulong)aVal + bVal + carry;
            result[i] = (uint)sum;
            carry = ConstantTime.ExtractUpperBits(sum);
        }
        result[maxLen] = carry;
        
        return result;
    }
    
    public static DiamondBigInteger Subtract(DiamondBigInteger a, DiamondBigInteger b, out uint borrowOut)
    {
        var maxLen = Math.Max(a.LimbCount, b.LimbCount);
        
        var borrow = 0U;
        var result = new uint[maxLen];
        for (int i = 0; i < maxLen; i++)
        {
            var aVal = a.TryGetLimb(i, 0);
            var bVal = b.TryGetLimb(i, 0);
            var diff = (ulong)aVal - bVal - borrow;
            result[i] = (uint)diff;
            borrow = ConstantTime.ExtractOverflowBit(diff);
        }
        
        borrowOut = borrow;
        return result;
    }

    public static void AddInto(DiamondBigInteger a, int offset, DiamondBigInteger b)
    {
        var carry = 0UL;
        for (int i = 0; i < b.LimbCount; i++)
        {
            var sum = (ulong)a[offset + i] + b.TryGetLimb(i, 0) + carry;
            a[offset + i] = (uint)sum;
            carry = sum >> 32;
        }
    
        for (int i = offset + b.LimbCount; i < a.LimbCount; i++)
        {
            var sum = a[i] + carry;
            a[i] = (uint)sum;
            carry = sum >> 32;
        }
    }
    
    public static void AddOrSubtractInto(DiamondBigInteger a, int offset, DiamondBigInteger b, uint shouldSubtract)
    {
        var mask = ConstantTime.Select(shouldSubtract, 0xFFFFFFFF, 0);
        var carry = ConstantTime.Select(shouldSubtract, 1UL, 0UL);

        for (int i = 0; i < b.LimbCount; i++)
        {
            var aVal = a.TryGetLimb(offset + i, 0);
            var bVal = b.TryGetLimb(i, 0) ^ mask;
            var sum = (ulong)aVal + bVal + carry;
            a.TrySetLimb(offset + i, (uint)sum);
            carry = sum >> 32;
        }
    
        for (int i = offset + b.LimbCount; i < a.LimbCount; i++)
        {
            var aVal = a.TryGetLimb(i, 0);
            var sum = aVal + carry;
            a[i] = (uint)sum;
            carry = sum >> 32;
        }
    }
}