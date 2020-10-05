using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Test
{
    public class ConstrainedNumber<T> where T : struct, IComparable, IComparable<T>, IEquatable<T>
    {
        private T _value;

        public ConstrainedNumber(T value)
        {
            _value = value;
        }

        public static T operator +(ConstrainedNumber<T> x, ConstrainedNumber<T> y)
        {
            return (dynamic)x._value + y._value;
        }
    }
}
