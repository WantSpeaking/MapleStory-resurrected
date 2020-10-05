#define USE_NX

using System;

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
	public class Point<T> where T : struct, IComparable, IComparable<T>, IEquatable<T>
	{
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
			return (T)Math.Sqrt ((dynamic)a * a + (dynamic)b * b);
		}

		// Check whether the coordinates are equal
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr bool straight() const
		public bool straight ()
		{
			return (dynamic)a == b;
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
			return new Point<T> (a - (dynamic)v.a, b - (dynamic)v.b).length ();
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
			a = (dynamic)a + v;
		}

		// Shift the y-coordinate by the specified amount
		public void shift_y (T v)
		{
			b = (dynamic)a + v;
		}

		// Shift the coordinates by the specified amounts
		public void shift (T x, T y)
		{
			a = (dynamic)a + x;
			b = (dynamic)a + y;
		}

		// Shift the this point by the amounts defined by another point
		// Equivalent to += operator
		public void shift (Point<T> v)
		{
			a = (dynamic)a + v.a;
			b = (dynamic)a + v.b;
		}

		// Take the absolute value of the point
		public Point<T> abs ()
		{
			return new Point<T> (Math.Abs ((dynamic)a), Math.Abs ((dynamic)b));
		}

		// Check whether point is equivalent to the specified point
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr bool operator == (const Point<T>& v) const
		public static bool operator == (Point<T> ImpliedObject, Point<T> v)
		{
			return (dynamic)ImpliedObject.a == v.a && (dynamic)ImpliedObject.b == v.b;
		}

		// Check whether point is not equivalent to the specified point
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr bool operator != (const Point<T>& v) const
		public static bool operator != (Point<T> ImpliedObject, Point<T> v)
		{
			return (dynamic)ImpliedObject.a != v.a || (dynamic)ImpliedObject.b != v.b;
		}

		/*// Shift the this point by the amounts defined by another point
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

		/*// Return a point whose coordinates are the negation of this point's coordinates
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr Point<T> operator - () const
		public static Point<T> operator - (Point<T> ImpliedObject)
		{
			return
			{
				-ImpliedObject.a, -ImpliedObject.b
			}
			;
		}

		// Return a point whose coordinates have been added the specified amount
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr Point<T> operator + (T v) const
		public static Point<T> operator + (Point<T> ImpliedObject, T v)
		{
			return
			{
				ImpliedObject.a + v, ImpliedObject.b + v
			}
			;
		}

		// Return a point whose coordinates have been subtracted the specified amount
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr Point<T> operator - (T v) const
		public static Point<T> operator - (Point<T> ImpliedObject, T v)
		{
			return
			{
				ImpliedObject.a - v, ImpliedObject.b - v
			}
			;
		}

		// Return a point whose coordinates have been multiplied by the specified amount
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr Point<T> operator * (T v) const
		public static Point<T> operator * (Point<T> ImpliedObject, T v)
		{
			return
			{
				ImpliedObject.a* v, ImpliedObject.b* v
			}
			;
		}

		// Return a point whose coordinates have been divided by the specified amount
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr Point<T> operator / (T v) const
		public static Point<T> operator / (Point<T> ImpliedObject, T v)
		{
			return
			{
				ImpliedObject.a / v, ImpliedObject.b / v
			}
			;
		}

		// Return a point whose coordinates are the sum of this and another points coordinates
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr Point<T> operator + (Point<T> v) const
		public static Point<T> operator + (Point<T> ImpliedObject, Point<T> v)
		{
			return
			{
				ImpliedObject.a + v.a, ImpliedObject.b + v.b
			}
			;
		}

		// Return a point whose coordinates are the difference of this and another points coordinates
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr Point<T> operator - (Point<T> v) const
		public static Point<T> operator - (Point<T> ImpliedObject, Point<T> v)
		{
			return
			{
				ImpliedObject.a - v.a, ImpliedObject.b - v.b
			}
			;
		}

		// Return a point whose coordinates are the product of this and another points coordinates
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr Point<T> operator * (Point<T> v) const
		public static Point<T> operator * (Point<T> ImpliedObject, Point<T> v)
		{
			return
			{
				ImpliedObject.a / v.a, ImpliedObject.b / v.b
			}
			;
		}

		// Return a point whose coordinates are the division of this and another points coordinates
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr Point<T> operator / (Point<T> v) const
		public static Point<T> operator / (Point<T> ImpliedObject, Point<T> v)
		{
			return new Point<T> (ImpliedObject.a / (v.a == 0 ? 1 : v.a), ImpliedObject.b / (v.b == 0 ? 1 : v.b));
			{
				ImpliedObject.a / (v.a == 0 ? 1 : v.a), ImpliedObject.b / (v.b == 0 ? 1 : v.b)
			}
			;
		}*/

		private T a;
		private T b;
	}
}