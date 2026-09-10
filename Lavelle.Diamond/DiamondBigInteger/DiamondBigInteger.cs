using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lavelle.Diamond
{
    public partial class DiamondBigInteger
    {
        private readonly uint[] _limbs;

        public DiamondBigInteger(uint[] limbs) => _limbs = limbs;
        public DiamondBigInteger(uint limb) => _limbs = [limb];
        public DiamondBigInteger(byte[] bytes)
        {
            var limbCount = (bytes.Length + 3) / 4;
            _limbs = new uint[limbCount];

            for (int i = 0; i < bytes.Length; i++)
            {
                var limbIndex = i / 4;
                var byteInLimb = i % 4;
                _limbs[limbIndex] |= (uint)bytes[i] << (byteInLimb * 8);
            }
        }

        public int LimbCount => _limbs.Length;

        private uint TryGetLimb(int i, uint elseValue) => ConstantTime.TryGetLimb(_limbs, i, elseValue);
        private void TrySetLimb(int i, uint value) => ConstantTime.TrySetLimb(_limbs, i, value);
        private uint this[int index]
        {
            get => _limbs[index];
            set => _limbs[index] = value;
        }

        public override string ToString()
        {
            int firstNonZero = _limbs.Length - 1;
            while (firstNonZero > 0 && _limbs[firstNonZero] == 0) firstNonZero--;

            var sb = new StringBuilder("0x");
            sb.Append(_limbs[firstNonZero].ToString("x"));
            for (int i = firstNonZero - 1; i >= 0; i--)
                sb.Append(_limbs[i].ToString("x8"));

            return sb.ToString();
        }

        public byte[] ToBytes()
        {
            var lastNonZeroIndex = _limbs.Length - 1;
            while (lastNonZeroIndex > 0 && _limbs[lastNonZeroIndex] == 0)
                lastNonZeroIndex--;

            var byteCount = (lastNonZeroIndex + 1) * 4;
            var bytes = new byte[byteCount];

            for (int i = 0; i <= lastNonZeroIndex; i++)
            {
                var limb = _limbs[i];
                bytes[i * 4] = (byte)limb;
                bytes[i * 4 + 1] = (byte)(limb >> 8);
                bytes[i * 4 + 2] = (byte)(limb >> 16);
                bytes[i * 4 + 3] = (byte)(limb >> 24);
            }

            return bytes;
        }

        public static implicit operator DiamondBigInteger(uint u) => new([u]);
        public static implicit operator DiamondBigInteger(uint[] u) => new(u);
    }
}
