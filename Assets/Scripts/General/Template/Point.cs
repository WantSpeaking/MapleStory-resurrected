
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Helper;
using ms.Helper;

using Object = System.Object;

namespace ms
{
    public class Point_short
    {
        public static Point_short zero = new Point_short(default, default);

        public override string ToString()
        {
            return $"({a},{b})";
        }


        // Construct a point from the specified coordinates
        public Point_short()
        {
        }
        public Point_short(Point_short src)
        {
            this.a = src.a;
            this.b = src.b;
        }
        public Point_short(short first, short second)
        {
            this.a = first;
            this.b = second;
        }

        public void Set(short first, short second)
        {
            a = first;
            b = second;
        }
        public Point_short(string str)
        {
            var arr = str.Replace("(", "").Replace(")", "").Split(",");
            if (arr.Length == 2)
            {
                short.TryParse(arr[0], out a);
                short.TryParse(arr[1], out b);
            }
        }
        // Return the x-coordinate
        public short x()
        {
            return a;
        }

        // Return the y-coordinate
        public short y()
        {
            return b;
        }

        // Return the inner product
        public short length()
        {
            return (short)Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
        }

        // Check whether the coordinates are equal
        public bool straight()
        {
            return a == b;
        }

        // Return a string representation of the point
        public string to_string()
        {
            return "(" + Convert.ToString(a) + "," + Convert.ToString(b) + ")";
        }

        // Return the distance to another point
        public short distance(Point_short v)
        {
            return new Point_short((short)(a - v.a), (short)(b - v.b)).length();
        }

        // Set the x-coordinate
        public void set_x(short v)
        {
            a = v;
        }

        // Set the y-coordinate
        public void set_y(short v)
        {
            b = v;
        }

        // Shift the x-coordinate by the specified amount
        public Point_short shift_x(short v)
        {
            a += v;
            return this;
        }

        // Shift the y-coordinate by the specified amount
        public Point_short shift_y(short v)
        {
            b += v;
            return this;
        }

        // Shift the coordinates by the specified amounts
        public void shift(short x, short y)
        {
            a += x;
            b += y;
        }

        // Shift the this point by the amounts defined by another point
        // Equivalent to += operator
        public void shift(Point_short v)
        {
            a += v.a;
            b += v.b;
        }

        // Take the absolute value of the point
        public Point_short abs()
        {
            return new Point_short(Math.Abs(a), Math.Abs(b));
        }

        public static Point_short operator -(Point_short a)
        {
            return new Point_short((short)-a.x(), (short)-a.y());
        }

        public static Point_short operator +(Point_short a, short v)
        {
            if (a == null) return new Point_short();
            return new Point_short((short)(a.x() + v), (short)(a.y() + v));
        }

        public static Point_short operator -(Point_short a, short v)
        {
            if (a == null) return new Point_short();
            return new Point_short((short)(a.x() - v), (short)(a.y() - v));
        }

        public static Point_short operator *(Point_short a, short v)
        {
            if (a == null) return new Point_short();
            return new Point_short((short)(a.x() * v), (short)(a.y() * v));
        }

        public static Point_short operator /(Point_short a, short v)
        {
            if (a == null) return new Point_short();
            return new Point_short((short)(a.x() / v), (short)(a.y() / v));
        }

        public static Point_short operator +(Point_short a, Point_short b)
        {
            if (a == null || b == null) return new Point_short();
            return new Point_short((short)(a.x() + b.x()), (short)(a.y() + b.y()));
        }

        public static Point_short operator -(Point_short a, Point_short b)
        {
            if (a == null || b == null) return new Point_short();
            return new Point_short((short)(a.x() - b.x()), (short)(a.y() - b.y()));
        }

        public static Point_short operator *(Point_short a, Point_short b)
        {
            if (a == null || b == null) return new Point_short();
            return new Point_short((short)(a.x() * b.x()), (short)(a.y() * b.y()));
        }

        public static Point_short operator /(Point_short a, Point_short b)
        {
            if (a == null || b == null) return new Point_short();
            return new Point_short((short)(a.x() / (b.x() == 0 ? 1 : b.x())), (short)(a.y() / (b.y() == 0 ? 1 : b.y())));
        }
        public static bool operator == (Point_short a, Point_short b)
		{
            return a?.x() == b?.x() &&　a?.y() == b?.y();
		}
        public static bool operator != (Point_short a, Point_short b)
        {
            return a?.x ()!= b?.x () || a?.y () != b?.y ();
        }
        /*	public static implicit operator Vector2 (Point_short x)
			{
				return new Vector2 (x.a.ToT<float> (), x.b.ToT<float> ());
			}*/

        private short a;
        private short b;
    }
    /*public class Point
    {
        public static Point zero = new Point (default, default);

        public override string ToString ()
        {
            return $"({a},{b})";
        }


        // Construct a point from the specified coordinates
        public Point ()
        {
        }
        public Point (Point src)
        {
            this.a = src.a;
            this.b = src.b;
        }
        public Point (int first, int second)
        {
            this.a = first;
            this.b = second;
        }

        public void Set (int first, int second)
        {
            a = first;
            b = second;
        }

        // Return the x-coordinate
        public int x ()
        {
            return a;
        }

        // Return the y-coordinate
        public int y ()
        {
            return b;
        }

        // Return the inner product
        public int length ()
        {
            return (int)Math.Sqrt (Math.Pow (a, 2) + Math.Pow (b, 2));
        }

        // Check whether the coordinates are equal
        public bool straight ()
        {
            return a == b;
        }

        // Return a string representation of the point
        public string to_string ()
        {
            return "(" + Convert.ToString (a) + "," + Convert.ToString (b) + ")";
        }

        // Return the distance to another point
        public int distance (Point v)
        {
            return new Point ((int)(a - v.a), (int)(b - v.b)).length ();
        }

        // Set the x-coordinate
        public void set_x (int v)
        {
            a = v;
        }

        // Set the y-coordinate
        public void set_y (int v)
        {
            b = v;
        }

        // Shift the x-coordinate by the specified amount
        public Point shift_x (int v)
        {
            a += v;
            return this;
        }

        // Shift the y-coordinate by the specified amount
        public Point shift_y (int v)
        {
            b += v;
            return this;
        }

        // Shift the coordinates by the specified amounts
        public void shift (int x, int y)
        {
            a += x;
            b += y;
        }

        // Shift the this point by the amounts defined by another point
        // Equivalent to += operator
        public void shift (Point v)
        {
            a += v.a;
            b += v.b;
        }

        // Take the absolute value of the point
        public Point abs ()
        {
            return new Point (Math.Abs (a), Math.Abs (b));
        }

        public static Point operator - (Point a)
        {
            return new Point ((int)-a.x (), (int)-a.y ());
        }

        public static Point operator + (Point a, int v)
        {
            if (a == null)
                return new Point ();
            return new Point ((int)(a.x () + v), (int)(a.y () + v));
        }

        public static Point operator - (Point a, int v)
        {
            if (a == null)
                return new Point ();
            return new Point ((int)(a.x () - v), (int)(a.y () - v));
        }

        public static Point operator * (Point a, int v)
        {
            if (a == null)
                return new Point ();
            return new Point ((int)(a.x () * v), (int)(a.y () * v));
        }

        public static Point operator / (Point a, int v)
        {
            if (a == null)
                return new Point ();
            return new Point ((int)(a.x () / v), (int)(a.y () / v));
        }

        public static Point operator + (Point a, Point b)
        {
            if (a == null || b == null)
                return new Point ();
            return new Point ((int)(a.x () + b.x ()), (int)(a.y () + b.y ()));
        }

        public static Point operator - (Point a, Point b)
        {
            if (a == null || b == null)
                return new Point ();
            return new Point ((int)(a.x () - b.x ()), (int)(a.y () - b.y ()));
        }

        public static Point operator * (Point a, Point b)
        {
            if (a == null || b == null)
                return new Point ();
            return new Point ((int)(a.x () * b.x ()), (int)(a.y () * b.y ()));
        }

        public static Point operator / (Point a, Point b)
        {
            if (a == null || b == null)
                return new Point ();
            return new Point ((int)(a.x () / (b.x () == 0 ? 1 : b.x ())), (int)(a.y () / (b.y () == 0 ? 1 : b.y ())));
        }

        /*	public static implicit operator Vector2 (Point x)
			{
				return new Vector2 (x.a.ToT<float> (), x.b.ToT<float> ());
			}#1#

        private int a;
        private int b;
    }*/
    public class Point_double
    {
        public static Point_double zero = new Point_double(default, default);

        public override string ToString()
        {
            return $"({a},{b})";
        }


        // Construct a point from the specified coordinates
        public Point_double()
        {
        }
        public Point_double(Point_double src)
        {
            this.a = src.a;
            this.b = src.b;
        }
        public Point_double(double first, double second)
        {
            this.a = first;
            this.b = second;
        }

        public void Set(double first, double second)
        {
            a = first;
            b = second;
        }

        // Return the x-coordinate
        public double x()
        {
            return a;
        }

        // Return the y-coordinate
        public double y()
        {
            return b;
        }

        // Return the inner product
        public double length()
        {
            return Math.Sqrt(a * a + b * b);
        }

        // Check whether the coordinates are equal
        public bool straight()
        {
            return a == b;

        }

        // Return a string representation of the point
        public string to_string()
        {
            return "(" + Convert.ToString(a) + "," + Convert.ToString(b) + ")";
        }

        // Return the distance to another point
        public double distance(Point_double v)
        {
            return new Point_short((short)(a - v.a), (short)(b - v.b)).length();
        }

        // Set the x-coordinate
        public void set_x(double v)
        {
            a = v;
        }

        // Set the y-coordinate
        public void set_y(double v)
        {
            b = v;
        }

        // Shift the x-coordinate by the specified amount
        public Point_double shift_x(double v)
        {
            a += v;
            return this;
        }

        // Shift the y-coordinate by the specified amount
        public Point_double shift_y(double v)
        {
            b += v;
            return this;
        }

        // Shift the coordinates by the specified amounts
        public void shift(double x, double y)
        {
            a += x;
            b += y;
        }

        // Shift the this point by the amounts defined by another point
        // Equivalent to += operator
        public void shift(Point_double v)
        {
            a += v.a;
            b += v.b;
        }

        // Take the absolute value of the point
        public Point_double abs()
        {
            return new Point_double(Math.Abs(a), Math.Abs(b));
        }

        public static Point_double operator -(Point_double a)
        {
            return new Point_double((double)-a.x(), (double)-a.y());
        }

        public static Point_double operator +(Point_double a, double v)
        {
            if (a == null) return new Point_double();
            return new Point_double((double)(a.x() + v), (double)(a.y() + v));
        }

        public static Point_double operator -(Point_double a, double v)
        {
            if (a == null) return new Point_double();
            return new Point_double((double)(a.x() - v), (double)(a.y() - v));
        }

        public static Point_double operator *(Point_double a, double v)
        {
            if (a == null) return new Point_double();
            return new Point_double((double)(a.x() * v), (double)(a.y() * v));
        }

        public static Point_double operator /(Point_double a, double v)
        {
            if (a == null) return new Point_double();
            return new Point_double((double)(a.x() / v), (double)(a.y() / v));
        }

        public static Point_double operator +(Point_double a, Point_double b)
        {
            if (a == null || b == null) return new Point_double();
            return new Point_double((double)(a.x() + b.x()), (double)(a.y() + b.y()));
        }

        public static Point_double operator -(Point_double a, Point_double b)
        {
            if (a == null || b == null) return new Point_double();
            return new Point_double((double)(a.x() - b.x()), (double)(a.y() - b.y()));
        }

        public static Point_double operator *(Point_double a, Point_double b)
        {
            if (a == null || b == null) return new Point_double();
            return new Point_double((double)(a.x() * b.x()), (double)(a.y() * b.y()));
        }

        public static Point_double operator /(Point_double a, Point_double b)
        {
            if (a == null || b == null) return new Point_double();
            return new Point_double((double)(a.x() / b.x()), (double)(a.y() / b.y()));
        }

        /*	public static implicit operator Vector2 (Point_double x)
			{
				return new Vector2 (x.a.ToT<float> (), x.b.ToT<float> ());
			}*/

        private double a;
        private double b;
    }

    /*public class Point<T> where T : unmanaged, IComparable, IComparable<T>, IEquatable<T>, IConvertible
	{
		public static Point<T> zero = new Point<T> (default, default);

		*//*protected bool Equals (Point<T> other)
		{
			return a.Equals (other.a) && b.Equals (other.b);
		}

		public override bool Equals (object obj)
		{
			if (ReferenceEquals (null, obj)) return false;
			if (ReferenceEquals (this, obj)) return true;
			if (obj.GetType () != this.GetType ()) return false;
			return Equals ((Point<T>)obj);
		}*//*

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
		*//*public Point(MapleLib.WzLib.Point src)
		{
			a = (T)src.x();
			b = (T)src.y();
		}*//*

		// Construct a point from the specified coordinates
		public Point ()
		{
		}
		public Point (Point<T> src)
		{
			this.a = src.a;
			this.b = src.b;
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
		public T x ()
		{
			return a;
		}

		// Return the y-coordinate
		public T y ()
		{
			return b;
		}

		// Return the inner product
		public T length ()
		{
			//return GenericArithmetic.Sqrt (a,b);
			var temp = Math.Sqrt(GenericArithmetic.Add (GenericArithmetic.Multiply (a, a), GenericArithmetic.Multiply (b, b)).ToT<double> ());
			if (double.IsNaN (temp))
			{
				AppDebug.Log ($"point length is NaN");
				return short.MaxValue.ToT<T> ();
			}
			else
			{
				return (temp).ToT<T> ();
			}
		}

		// Check whether the coordinates are equal
		public bool straight ()
		{
			return GenericArithmetic.Equal (a, b);
		}

		// Return a string representation of the point
		public string to_string ()
		{
			return "(" + Convert.ToString (a) + "," + Convert.ToString (b) + ")";
		}

		// Return the distance to another point
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
		public Point<T> shift_x (T v)
		{
			a = GenericArithmetic.Add (a, v);
			return this;
		}

		// Shift the y-coordinate by the specified amount
		public Point<T> shift_y (T v)
		{
			b = GenericArithmetic.Add (b, v);
			return this;
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
		*//*public static bool operator == (Point<T> ImpliedObject, Point<T> v)
		{
			//AppDebug.Log ($"ImpliedObject:{ImpliedObject==null} v:{v==null}");
			//return ImpliedObject.a.Equals (v.a) && ImpliedObject.b.Equals (v.b);
			//return (dynamic)ImpliedObject.a == v.a && (dynamic)ImpliedObject.b == v.b;
			return true;
		}*//*

		// Check whether point is not equivalent to the specified point
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: constexpr bool operator != (const Point<T>& v) const
		*//*public static bool operator != (Point<T> ImpliedObject, Point<T> v)
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
			}*//*

			// Return a point whose coordinates are the negation of this point's coordinates
	//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
	//ORIGINAL LINE: constexpr Point<T> operator - () const
			public static Point<T> operator - (Point<T> x)
			{
				*//*var temp_a = (T)Convert.ChangeType ((-x.a.ToDouble (NumberFormatInfo.CurrentInfo)), typeof (T));
				var temp_b = (T)Convert.ChangeType ((-x.b.ToDouble (NumberFormatInfo.CurrentInfo)), typeof (T));
				return new Point<T> (temp_a, temp_b);*//*
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
				*//*if (x == null || y == null) return new Point<T> ();
				var temp_a = (T)Convert.ChangeType ((x.a.ToDouble (NumberFormatInfo.CurrentInfo) + y.a.ToDouble (NumberFormatInfo.CurrentInfo)), typeof (T));
				var temp_b = (T)Convert.ChangeType ((x.b.ToDouble (NumberFormatInfo.CurrentInfo) + y.b.ToDouble (NumberFormatInfo.CurrentInfo)), typeof (T));
				return new Point<T> (temp_a, temp_b);*//*

				return new Point<T> (GenericArithmetic.Add (x.a, y.a), GenericArithmetic.Add (x.b, y.b));
			}

			// Return a point whose coordinates are the difference of this and another points coordinates
			//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
			//ORIGINAL LINE: constexpr Point<T> operator - (Point<T> v) const
			public static Point<T> operator - (Point<T> x, Point<T> y)
			{
				*//*//if (x == null || y == null) return new Point<T> ();
				AppDebug.Log ($"x:{x == null} y:{y == null} x.a:{x.a} y.a:{y.a}");
				var temp_a = (T)Convert.ChangeType ((x.a.ToDouble (NumberFormatInfo.CurrentInfo) - y.a.ToDouble (NumberFormatInfo.CurrentInfo)), typeof (T));
				var temp_b = (T)Convert.ChangeType ((x.b.ToDouble (NumberFormatInfo.CurrentInfo) - y.b.ToDouble (NumberFormatInfo.CurrentInfo)), typeof (T));
				return new Point<T> ((T)(Object)(x.a.ToDouble (NumberFormatInfo.CurrentInfo) - y.a.ToDouble (NumberFormatInfo.CurrentInfo)), (T)(Object)(x.b.ToDouble (NumberFormatInfo.CurrentInfo) - y.b.ToDouble (NumberFormatInfo.CurrentInfo)));
				*/

    /*dynamic temp_left_a = left.a;
	dynamic temp_left_b = left.b;
	dynamic temp_right_a = right.a;
	dynamic temp_right_b = right.b;
	return new Point<T> ((T)(temp_left_a - temp_right_a), (T)temp_left_b - temp_right_b);*//*

	//AppDebug.Log ($"x:{x == null} y:{y == null}");
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

*//*	public static implicit operator Vector2 (Point<T> x)
	{
		return new Vector2 (x.a.ToT<float> (), x.b.ToT<float> ());
	}*//*

	private T a;
	private T b;
}*/
}