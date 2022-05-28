using System;
using System.Collections.Generic;
using System.Text;

namespace Util
{
    class MathUtil
    {
        private static void ThrowMinMaxException<T>(T min, T max)
        {
            throw new ArgumentException($"min:{min} max:{max}");
        }

        public static float Clamp(float value, float min, float max)
        {
            if (min > max)
            {
                ThrowMinMaxException(min, max);
            }

            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }

            return value;
        }
    }
}
