//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////

using System;
using Helper;
using ms.Helper;

namespace ms
{
    public class Range_short
    {
        // Construct a range from the specified values
        public Range_short(short first, short second)
        {
            this.a = first;
            this.b = second;
        }

        public Range_short(Range_short src)
        {
            this.a = src.a;
            this.b = src.b;
        }

        // Construct a range of (0, 0)
        public Range_short()
        {
            //this.a = (dynamic)0;
            //this.b = (dynamic)0;
        }

        public void Set(short first, short second)
        {
            a = first;
            b = second;
        }

        // Return the first value
        public short first()
        {
            return a;
        }

        // Return the second value
        public short second()
        {
            return b;
        }

        // Return the greater value
        public short greater()
        {
            return a > b ? a : b;
        }

        // Return the smaller value
        public short smaller()
        {
            return a > b ? b : a;
        }

        // Return the difference between the values
        public short delta()
        {
            return ((short)(a - b));
        }

        // Return the absolute difference between the values
        public short length()
        {
            return (short)(greater() - smaller());
        }

        // Return the mean of both values
        public short center()
        {
            return (short)((a + b) / 2);
        }

        // Check if both values are equal
        public bool empty()
        {
            return a == b;

        }

        // Check if the range contains a value
        public bool contains(short v)
        {
            return v >= a && v <= b;
        }

        // Check if the range contains another range
        public bool contains(Range_short v)
        {
            return v.a >= a && v.b <= b;
        }

        // Check if the ranges overlap
        public bool overlaps(Range_short v)
        {
            return contains(v.a) || contains(v.b) || v.contains(a) || v.contains(b);
        }

        // Check whether the range is equivalent to another range
        public static bool operator ==(Range_short ImpliedObject, Range_short v)
        {
            return ImpliedObject.a == v.a && ImpliedObject.b == v.b;
        }

        // Check whether the range is not equivalent to another range
        public static bool operator !=(Range_short ImpliedObject, Range_short v)
        {
            return !(ImpliedObject == v);
        }

        // Shift this range by the amounts defined by another range
        public static Range_short operator +(Range_short ImpliedObject, Range_short v)
        {
            return new Range_short((short)(ImpliedObject.a + v.a), (short)(ImpliedObject.b + v.b));
        }

        // Shift this range by the negative amounts defined by another range
        public static Range_short operator -(Range_short ImpliedObject, Range_short v)
        {
            return new Range_short((short)(ImpliedObject.a - v.a), (short)(ImpliedObject.b - v.b));
        }

        // Return the negative of this range
        public static Range_short operator -(Range_short ImpliedObject)
        {
            return new Range_short((short)-ImpliedObject.a, (short)-ImpliedObject.b);
        }

        // Construct a symmetric range around mid
        public static Range_short symmetric(short mid, short tail)
        {
            return new Range_short((short)(mid - tail), (short)(mid + tail));
        }

        private short a = new short();
        private short b = new short();
    }
    public class Range_ushort
    {
        // Construct a range from the specified values
        public Range_ushort(ushort first, ushort second)
        {
            this.a = first;
            this.b = second;
        }

        public Range_ushort(Range_ushort src)
        {
            this.a = src.a;
            this.b = src.b;
        }

        // Construct a range of (0, 0)
        public Range_ushort()
        {
            //this.a = (dynamic)0;
            //this.b = (dynamic)0;
        }

        public void Set(ushort first, ushort second)
        {
            a = first;
            b = second;
        }

        // Return the first value
        public ushort first()
        {
            return a;
        }

        // Return the second value
        public ushort second()
        {
            return b;
        }

        // Return the greater value
        public ushort greater()
        {
            return (a > b) ? a : b;
        }

        // Return the smaller value
        public ushort smaller()
        {
            return (a > b) ? b : a;

        }

        // Return the difference between the values
        public ushort delta()
        {
            return (ushort)(a - b);
        }

        // Return the absolute difference between the values
        public ushort length()
        {
            return (ushort)(greater() - smaller());
        }

        // Return the mean of both values
        public ushort center()
        {
            return (ushort)((a + b) / 2);
        }

        // Check if both values are equal
        public bool empty()
        {
            return a == b;
        }

        // Check if the range contains a value
        public bool contains(ushort v)
        {
            return v >= a && v <= b;
        }

        // Check if the range contains another range
        public bool contains(Range_ushort v)
        {
            return v.a >= a && v.b <= b;
        }

        // Check if the ranges overlap
        public bool overlaps(Range_ushort v)
        {
            return contains(v.a) || contains(v.b) || v.contains(a) || v.contains(b);
        }

        // Check whether the range is equivalent to another range
        public static bool operator ==(Range_ushort ImpliedObject, Range_ushort v)
        {
            return ImpliedObject.a == v.a && ImpliedObject.b == v.b;
        }

        // Check whether the range is not equivalent to another range
        public static bool operator !=(Range_ushort ImpliedObject, Range_ushort v)
        {
            return !(ImpliedObject == v);
        }

        // Shift this range by the amounts defined by another range
        public static Range_ushort operator +(Range_ushort ImpliedObject, Range_ushort v)
        {
            return new Range_ushort((ushort)(ImpliedObject.a + v.a), (ushort)(ImpliedObject.b + v.b));
        }

        // Shift this range by the negative amounts defined by another range
        public static Range_ushort operator -(Range_ushort ImpliedObject, Range_ushort v)
        {
            return new Range_ushort((ushort)(ImpliedObject.a - v.a), (ushort)(ImpliedObject.b - v.b));
        }

        // Return the negative of this range
        public static Range_ushort operator -(Range_ushort ImpliedObject)
        {
            return new Range_ushort((ushort)(-ImpliedObject.a), (ushort)(-ImpliedObject.b));
        }

        // Construct a symmetric range around mid
        public static Range_ushort symmetric(ushort mid, ushort tail)
        {
            return new Range_ushort((ushort)(mid - tail), (ushort)(mid + tail));
        }

        private ushort a = new ushort();
        private ushort b = new ushort();
    }
    public class Range_double
    {
        // Construct a range from the specified values
        public Range_double(double first, double second)
        {
            this.a = first;
            this.b = second;
        }

        public Range_double(Range_double src)
        {
            this.a = src.a;
            this.b = src.b;
        }

        // Construct a range of (0, 0)
        public Range_double()
        {
            //this.a = (dynamic)0;
            //this.b = (dynamic)0;
        }

        public void Set(double first, double second)
        {
            a = first;
            b = second;
        }

        // Return the first value
        public double first()
        {
            return a;
        }

        // Return the second value
        public double second()
        {
            return b;
        }

        // Return the greater value
        public double greater()
        {
            return a > b ? a : b;
        }

        // Return the smaller value
        public double smaller()
        {
            return a > b ? b : a;
        }

        // Return the difference between the values
        public double delta()
        {
            return (double)(a - b);
        }

        // Return the absolute difference between the values
        public double length()
        {
            return (double)(greater() - smaller());
        }

        // Return the mean of both values
        public double center()
        {
            return (a + b) / 2;
        }

        // Check if both values are equal
        public bool empty()
        {
            return a == b;
        }

        // Check if the range contains a value
        public bool contains(double v)
        {
            return v >= a && v <= b;
        }

        // Check if the range contains another range
        public bool contains(Range_double v)
        {
            return v.a >= a && v.b <= b;
        }

        // Check if the ranges overlap
        public bool overlaps(Range_double v)
        {
            return contains(v.a) || contains(v.b) || v.contains(a) || v.contains(b);
        }

        // Check whether the range is equivalent to another range
        public static bool operator ==(Range_double ImpliedObject, Range_double v)
        {
            return ImpliedObject.a == v.a && ImpliedObject.b == v.b;
        }

        // Check whether the range is not equivalent to another range
        public static bool operator !=(Range_double ImpliedObject, Range_double v)
        {
            return !(ImpliedObject == v);
        }

        // Shift this range by the amounts defined by another range
        public static Range_double operator +(Range_double ImpliedObject, Range_double v)
        {
            return new Range_double((ImpliedObject.a + v.a), (ImpliedObject.b + v.b));
        }

        // Shift this range by the negative amounts defined by another range
        public static Range_double operator -(Range_double ImpliedObject, Range_double v)
        {
            return new Range_double((ImpliedObject.a - v.a), (ImpliedObject.b - v.b));
        }

        // Return the negative of this range
        public static Range_double operator -(Range_double ImpliedObject)
        {
            return new Range_double(-(ImpliedObject.a), -(ImpliedObject.b));
        }

        // Construct a symmetric range around mid
        public static Range_double symmetric(double mid, double tail)
        {
            return new Range_double((mid - tail), (mid + tail));
        }

        private double a = new double();
        private double b = new double();
    }
    /*public class Range<T> where T : unmanaged, IComparable, IComparable<T>, IEquatable<T>
	{
		// Construct a range from the specified values
		public Range (T first, T second)
		{
			this.a = first;
			this.b = second;
		}

		public Range(Range<T> src)
		{
			this.a = src.a;
			this.b = src.b;
		}

		// Construct a range of (0, 0)
		public Range ()
		{
			//this.a = (dynamic)0;
			//this.b = (dynamic)0;
		}

		public void Set (T first, T second)
		{
			a = first;
			b = second;
		}

		// Return the first value
		public T first ()
		{
			return a;
		}

		// Return the second value
		public T second ()
		{
			return b;
		}

		// Return the greater value
		public T greater ()
		{
			return (GenericArithmetic.GreaterThan (a, b)) ? a : b;
		}

		// Return the smaller value
		public T smaller ()
		{
			return (GenericArithmetic.GreaterThan (a, b)) ? b : a;
		}

		// Return the difference between the values
		public T delta ()
		{
			return GenericArithmetic.Subtract (a, b);
		}

		// Return the absolute difference between the values
		public T length ()
		{
			return GenericArithmetic.Subtract (greater (), smaller ());
		}

		// Return the mean of both values
		public T center ()
		{
			return GenericArithmetic.Divide (GenericArithmetic.Add (a, b), 2.ToT<T> ());
		}

		// Check if both values are equal
		public bool empty ()
		{
			return GenericArithmetic.Equal (a, b);
		}

		// Check if the range contains a value
		public bool contains (T v)
		{
			return GenericArithmetic.GreaterThanOrEqual (v, a) && !GenericArithmetic.GreaterThan (v, b);
		}

		// Check if the range contains another range
		public bool contains (Range<T> v)
		{
			return GenericArithmetic.GreaterThanOrEqual (v.a, a) && !GenericArithmetic.GreaterThan (v.b, b);
		}

		// Check if the ranges overlap
		public bool overlaps (Range<T> v)
		{
			return contains (v.a) || contains (v.b) || v.contains (a) || v.contains (b);
		}

		// Check whether the range is equivalent to another range
		public static bool operator == (Range<T> ImpliedObject, Range<T> v)
		{
			return GenericArithmetic.Equal (ImpliedObject.a, v.a) && GenericArithmetic.Equal (ImpliedObject.b, v.b);
		}

		// Check whether the range is not equivalent to another range
		public static bool operator != (Range<T> ImpliedObject, Range<T> v)
		{
			return !(ImpliedObject == v);
		}

		// Shift this range by the amounts defined by another range
		public static Range<T> operator + (Range<T> ImpliedObject, Range<T> v)
		{
			return new Range<T> (GenericArithmetic.Add (ImpliedObject.a, v.a), GenericArithmetic.Add (ImpliedObject.b, v.b));
		}

		// Shift this range by the negative amounts defined by another range
		public static Range<T> operator - (Range<T> ImpliedObject, Range<T> v)
		{
			return new Range<T> (GenericArithmetic.Subtract (ImpliedObject.a, v.a), GenericArithmetic.Subtract (ImpliedObject.b, v.b));

		}

		// Return the negative of this range
		public static Range<T> operator - (Range<T> ImpliedObject)
		{
			return new Range<T> (GenericArithmetic.Multiply (ImpliedObject.a, (-1).ToT<T> ()), GenericArithmetic.Multiply (ImpliedObject.b, (-1).ToT<T> ()));
		}

		// Construct a symmetric range around mid
		public static Range<T> symmetric (T mid, T tail)
		{
			return new Range<T> (GenericArithmetic.Subtract (mid, tail), GenericArithmetic.Add (mid, tail));
		}

		private T a = new T ();
		private T b = new T ();
	}*/
}