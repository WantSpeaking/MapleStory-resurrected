using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ms.Helper
{
    public static class TypeExt
    {
        public static bool ToBool(this int src)
        {
            return src == 1;
        }

        public static Byte ToByte<T>(this T src)
        {
            Byte.TryParse(src.ToString(), out var result);
            return result;
        }
    }
}
