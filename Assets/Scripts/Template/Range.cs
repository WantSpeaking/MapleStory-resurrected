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
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
//ORIGINAL LINE: template <class T>
	public class Range<T> where T : unmanaged, IComparable, IComparable<T>, IEquatable<T>
	{
		// Construct a range from the specified values
		public Range (T first, T second)
		{
			this.a = first;
			this.b = second;
		}

		// Construct a range of (0, 0)
		public Range ()
		{
			//this.a = (dynamic)0;
			//this.b = (dynamic)0;
		}

		// Return the first value
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr const T& first() const
		public T first ()
		{
			return a;
		}

		// Return the second value
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr const T& second() const
		public T second ()
		{
			return b;
		}

		// Return the greater value
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr const T& greater() const
		public T greater ()
		{
			return (GenericArithmetic.GreaterThan (a, b))?a : b;
			return ((dynamic)a > b) ? a : b;
		}

		// Return the smaller value
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr const T& smaller() const
		public T smaller ()
		{
			return (GenericArithmetic.GreaterThan (a, b))?b : a;
			return (dynamic)a < b ? a : b;
		}

		// Return the difference between the values
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr T delta() const
		public T delta ()
		{
			return GenericArithmetic.Subtract (a,b);

			return (dynamic)b - (dynamic)a;
		}

		// Return the absolute difference between the values
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr T length() const
		public T length ()
		{
			return GenericArithmetic.Subtract (greater (),smaller ());
			return (dynamic)greater () - (dynamic)smaller ();
		}

		// Return the mean of both values
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr T center() const
		public T center ()
		{
			return GenericArithmetic.DivideSpecific (GenericArithmetic.Add (a, b), 2);
			return ((dynamic)a + (dynamic)b) / 2;
		}

		// Check if both values are equal
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr bool empty() const
		public bool empty ()
		{
			return GenericArithmetic.Equal (a, b);
			return (dynamic)a == (dynamic)b;
		}

		// Check if the range contains a value
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr bool contains(const T& v) const
		public bool contains (T v)
		{
			return GenericArithmetic.GreaterThanOrEqual (v, a) && !GenericArithmetic.GreaterThan (v, b);
			return (dynamic)v >= (dynamic)a  && (dynamic)v <= (dynamic)b;
		}

		// Check if the range contains another range
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr bool contains(const Range<T>& v) const
		public bool contains (Range<T> v)
		{
			return GenericArithmetic.GreaterThanOrEqual (v.a, a) && !GenericArithmetic.GreaterThan (v.b, b);
			return (dynamic)v.a >= (dynamic)a  && (dynamic)v.b <= (dynamic)b;
		}

		// Check if the ranges overlap
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr bool overlaps(const Range<T>& v) const
		public bool overlaps (Range<T> v)
		{
			return contains (v.a) || contains (v.b) || v.contains (a) || v.contains (b);
		}

		// Check whether the range is equivalent to another range
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr bool operator == (const Range<T>& v) const
		public static bool operator == (Range<T> ImpliedObject, Range<T> v)
		{
			return GenericArithmetic.Equal (ImpliedObject.a, v.a) && GenericArithmetic.Equal (ImpliedObject.b, v.b);
			return (dynamic)ImpliedObject.a == (dynamic)v.a && (dynamic)ImpliedObject.b == (dynamic)v.b;
		}

		// Check whether the range is not equivalent to another range
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr bool operator != (const Range<T>& v) const
		public static bool operator != (Range<T> ImpliedObject, Range<T> v)
		{
			return !(ImpliedObject == v);
		}

		// Shift this range by the amounts defined by another range
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr Range<T> operator + (const Range<T>& v) const
		public static Range<T> operator + (Range<T> ImpliedObject, Range<T> v)
		{
			return new Range<T> (GenericArithmetic.Add(ImpliedObject.a , v.a),GenericArithmetic.Add (ImpliedObject.b , v.b));
			
		}

		// Shift this range by the negative amounts defined by another range
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr Range<T> operator - (const Range<T>& v) const
		public static Range<T> operator - (Range<T> ImpliedObject, Range<T> v)
		{
			return new Range<T> (GenericArithmetic.Subtract(ImpliedObject.a , v.a),GenericArithmetic.Subtract (ImpliedObject.b , v.b));

			return new Range<T> ((dynamic)ImpliedObject.a - v.a, (dynamic)ImpliedObject.b - v.b);
		}

		// Return the negative of this range
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr Range<T> operator - () const
		public static Range<T> operator - (Range<T> ImpliedObject)
		{
			return new Range<T> (GenericArithmetic.Multiply (ImpliedObject.a ,(-1).ToT<T> ()),GenericArithmetic.Multiply (ImpliedObject.b ,(-1).ToT<T> ()));
			return new Range<T> (-(dynamic)ImpliedObject.a , -(dynamic)ImpliedObject.b );
		}

		// Construct a symmetric range around mid
		public static Range<T> symmetric (T mid, T tail)
		{
			return new Range<T> (GenericArithmetic.Subtract(mid , tail), GenericArithmetic.Subtract(mid , tail));
			return new Range<T> ((dynamic)mid - tail, (dynamic)mid + tail);
		}

		private T a = new T ();
		private T b = new T ();
	}
}