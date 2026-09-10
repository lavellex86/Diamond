using ILGPU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavelle.Diamond
{
    public static class Extensions
    {
        public static byte[] FromString(this string str) => Encoding.UTF8.GetBytes(str);
        public static string HexString(this byte[] bytes) => Convert.ToHexStringLower(bytes);
        public static string HexString(this uint[] words) => "[" + string.Join(", ", words.Select(word => word.ToString("X8"))) + "]";
        public static string HexString(this ulong[] words) => "[" + string.Join(", ", words.Select(word => word.ToString("X16"))) + "]";
    }
}
