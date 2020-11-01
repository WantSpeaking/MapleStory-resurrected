#define USE_NX

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Helper;
using ms.Helper;
using UnityEngine;
using Object = System.Object;

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


#if USE_NX
#else
#endif

namespace ms
{
	//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
	//ORIGINAL LINE: template <class T>
	public class Point<T> where T : unmanaged, IComparable, IComparable<T>, IEquatable<T>, IConvertible
	{
		public static Point<T> zero = new Point<T> (default, default);

		/*protected bool Equals (Point<T> other)
		{
			return a.Equals (other.a) && b.Equals (other.b);
		}

		public override bool Equals (object obj)
		{
			if (ReferenceEquals (null, obj)) return false;
			if (ReferenceEquals (this, obj)) return true;
			if (obj.GetType () != this.GetType ()) return false;
			return Equals ((Point<T>)obj);
		}*/

		public override int GetHashCode ()
		{
			unchecked
			{
				return (a.GetHashCode () * 397) ^ b.GetHashCode ();
			}
		}

		public override string ToString ()
		{
			return $"({a},{b})";
		}

		// Construct a point from a vector property
		/*public Point(System.Drawing.Point src)
		{
			a = (T)src.x();
			b = (T)src.y();
		}*/

		// Construct a point from the specified coordinates
		public Point ()
		{
		}

		public Point (T first, T second)
		{
			this.a = first;
			this.b = second;
		}

		public void Set (T first, T second)
		{
			a = first;
			b = second;
		}

		// Return the x-coordinate
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: constexpr T x() const
		public T x ()
		{
			return a;
		}

		// Return the y-coordinate
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: constexpr T y() const
		public T y ()
		{
			return b;
		}

		// Return the inner product
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: constexpr T length() const
		public T length ()
		{
			return Math.Sqrt (GenericArithmetic.Add (GenericArithmetic.Multiply (a, a), GenericArithmetic.Multiply (b, b)).ToT<double> ()).ToT<T> ();
		}

		// Check whether the coordinates are equal
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: constexpr bool straight() const
		public bool straight ()
		{
			return GenericArithmetic.Equal (a, b);
		}

		// Return a string representation of the point
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: string to_string() const
		public string to_string ()
		{
			return "(" + Convert.ToString (a) + "," + Convert.ToString (b) + ")";
		}

		// Return the distance to another point
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: constexpr T distance(Point<T> v) const
		public T distance (Point<T> v)
		{
			return new Point<T> (GenericArithmetic.Subtract (a, v.a), GenericArithmetic.Subtract (b, v.b)).length ();
		}

		// Set the x-coordinate
		public void set_x (T v)
		{
			a = v;
		}

		// Set the y-coordinate
		public void set_y (T v)
		{
			b = v;
		}

		// Shift the x-coordinate by the specified amount
		public void shift_x (T v)
		{
			a = GenericArithmetic.Add (a, v);
		}

		// Shift the y-coordinate by the specified amount
		public void shift_y (T v)
		{
			b = GenericArithmetic.Add (b, v);
		}

		// Shift the coordinates by the specified amounts
		public void shift (T x, T y)
		{
			a = GenericArithmetic.Add (a, x);
			b = GenericArithmetic.Add (b, y);
		}

		// Shift the this point by the amounts defined by another point
		// Equivalent to += operator
		public void shift (Point<T> v)
		{
			a = GenericArithmetic.Add (a, v.a);
			b = GenericArithmetic.Add (b, v.b);
		}

		// Take the absolute value of the point
		public Point<T> abs ()
		{
			return new Point<T> (GenericArithmetic.Abs (a), GenericArithmetic.Abs (b));
		}

		// Check whether point is equivalent to the specified point
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: constexpr bool operator == (const Point<T>& v) const
		/*public static bool operator == (Point<T> ImpliedObject, Point<T> v)
		{
			//Debug.Log ($"ImpliedObject:{ImpliedObject==null} v:{v==null}");
			//return ImpliedObject.a.Equals (v.a) && ImpliedObject.b.Equals (v.b);
			//return (dynamic)ImpliedObject.a == v.a && (dynamic)ImpliedObject.b == v.b;
			return true;
		}*/

		// Check whether point is not equivalent to the specified point
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: constexpr bool operator != (const Point<T>& v) const
		/*public static bool operator != (Point<T> ImpliedObject, Point<T> v)
		{
			return (dynamic)ImpliedObject.a != v.a || (dynamic)ImpliedObject.b != v.b;
		}*/

/*        // Shift the this point by the amounts defined by another point
        //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The += operator cannot be overloaded in C#:
        public static void operator += (Point<T> v)
        {
            a += v.a;
            b += v.b;
        }

        // Shift the this point in reverse direction by the amounts defined by another point
        //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The -= operator cannot be overloaded in C#:
        public static void operator -= (Point<T> v)
        {
            a -= v.a;
            b -= v.b;
        }*/

		// Return a point whose coordinates are the negation of this point's coordinates
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr Point<T> operator - () const
		public static Point<T> operator - (Point<T> x)
		{
			/*var temp_a = (T)Convert.ChangeType ((-x.a.ToDouble (NumberFormatInfo.CurrentInfo)), typeof (T));
			var temp_b = (T)Convert.ChangeType ((-x.b.ToDouble (NumberFormatInfo.CurrentInfo)), typeof (T));
			return new Point<T> (temp_a, temp_b);*/
			return new Point<T> (GenericArithmetic.Multiply (x.a, (-1).ToT<T> ()), GenericArithmetic.Multiply (x.b, (-1).ToT<T> ()));

			//return new Point<T> (-(dynamic)ImpliedObject.a, -(dynamic)ImpliedObject.b);
		}

		public static Point<T> operator + (Point<T> ImpliedObject, T v)
		{
			return new Point<T> (GenericArithmetic.Add (ImpliedObject.a, v), GenericArithmetic.Add (ImpliedObject.b, v));
		}

		public static Point<T> operator - (Point<T> ImpliedObject, T v)
		{
			return new Point<T> (GenericArithmetic.Subtract (ImpliedObject.a, v), GenericArithmetic.Subtract (ImpliedObject.b, v));
		}

		public static Point<T> operator * (Point<T> ImpliedObject, T v)
		{
			return new Point<T> (GenericArithmetic.Multiply (ImpliedObject.a, v), GenericArithmetic.Multiply (ImpliedObject.b, v));
		}

		public static Point<T> operator / (Point<T> ImpliedObject, T v)
		{
			return new Point<T> (GenericArithmetic.Divide (ImpliedObject.a, v), GenericArithmetic.Divide (ImpliedObject.b, v));
		}

		// Return a point whose coordinates are the sum of this and another points coordinates
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: constexpr Point<T> operator + (Point<T> v) const
		public static Point<T> operator + (Point<T> x, Point<T> y)
		{
			/*if (x == null || y == null) return new Point<T> ();
			var temp_a = (T)Convert.ChangeType ((x.a.ToDouble (NumberFormatInfo.CurrentInfo) + y.a.ToDouble (NumberFormatInfo.CurrentInfo)), typeof (T));
			var temp_b = (T)Convert.ChangeType ((x.b.ToDouble (NumberFormatInfo.CurrentInfo) + y.b.ToDouble (NumberFormatInfo.CurrentInfo)), typeof (T));
			return new Point<T> (temp_a, temp_b);*/

			return new Point<T> (GenericArithmetic.Add (x.a, y.a), GenericArithmetic.Add (x.b, y.b));
		}

		// Return a point whose coordinates are the difference of this and another points coordinates
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: constexpr Point<T> operator - (Point<T> v) const
		public static Point<T> operator - (Point<T> x, Point<T> y)
		{
			/*//if (x == null || y == null) return new Point<T> ();
			Debug.Log ($"x:{x == null} y:{y == null} x.a:{x.a} y.a:{y.a}");
			var temp_a = (T)Convert.ChangeType ((x.a.ToDouble (NumberFormatInfo.CurrentInfo) - y.a.ToDouble (NumberFormatInfo.CurrentInfo)), typeof (T));
			var temp_b = (T)Convert.ChangeType ((x.b.ToDouble (NumberFormatInfo.CurrentInfo) - y.b.ToDouble (NumberFormatInfo.CurrentInfo)), typeof (T));
			return new Point<T> ((T)(Object)(x.a.ToDouble (NumberFormatInfo.CurrentInfo) - y.a.ToDouble (NumberFormatInfo.CurrentInfo)), (T)(Object)(x.b.ToDouble (NumberFormatInfo.CurrentInfo) - y.b.ToDouble (NumberFormatInfo.CurrentInfo)));
			*/

			/*dynamic temp_left_a = left.a;
			dynamic temp_left_b = left.b;
			dynamic temp_right_a = right.a;
			dynamic temp_right_b = right.b;
			return new Point<T> ((T)(temp_left_a - temp_right_a), (T)temp_left_b - temp_right_b);*/

			//Debug.Log ($"x:{x == null} y:{y == null}");
			return new Point<T> (GenericArithmetic.Subtract (x.a, y.a), GenericArithmetic.Subtract (x.b, y.b));
		}

		// Return a point whose coordinates are the product of this and another points coordinates
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: constexpr Point<T> operator * (Point<T> v) const
		public static Point<T> operator * (Point<T> x, Point<T> y)
		{
			if (x == null || y == null) return new Point<T> ();
			var temp_a = (T)Convert.ChangeType ((x.a.ToDouble (NumberFormatInfo.CurrentInfo) * y.a.ToDouble (NumberFormatInfo.CurrentInfo)), typeof (T));
			var temp_b = (T)Convert.ChangeType ((x.b.ToDouble (NumberFormatInfo.CurrentInfo) * y.b.ToDouble (NumberFormatInfo.CurrentInfo)), typeof (T));
			return new Point<T> (temp_a, temp_b);
		}

		public static Point<T> operator / (Point<T> ImpliedObject, Point<T> v)
		{
			return new Point<T>
			(
				GenericArithmetic.Divide (ImpliedObject.a, GenericArithmetic.Equal (v.a, 0.ToT<T> ()) ? 1.ToT<T> () : v.a),
				GenericArithmetic.Divide (ImpliedObject.b, GenericArithmetic.Equal (v.b, 0.ToT<T> ()) ? 1.ToT<T> () : v.b)
			);
		}

		private T a;
		private T b;
	}
}