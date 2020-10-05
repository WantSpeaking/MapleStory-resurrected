using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Helper
{
   public static class TypeExt
    {
        public static bool ToBool (this int src)
        {
            return src == 1;
        }
    }
}
