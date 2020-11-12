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
//ORIGINAL LINE: template <typename T>
	/*public class Nominal<T> where T : unmanaged /*, IComparable, IComparable<T>, IEquatable<T>#1#
	{
		public Nominal () : this (default, default, 0)
		{
		}

		public Nominal (T _now, T _before, float _threshold)
		{
			now = _now;
			before = _before;
			threshold = _threshold;
		}

		public T get ()
		{
			return now;
		}

		public T get (float alpha)
		{
			return alpha >= threshold ? now : before;
		}

		public T last ()
		{
			return before;
		}

		public void set (T value)
		{
			now = value;
			before = value;
		}

		public void normalize ()
		{
			before = now;
		}

		public bool normalized ()
		{
			return GenericArithmetic.Equal (before, now);
		}

		public void next (T value, float thrs)
		{
			before = now;
			now = value;
			threshold = thrs;
		}

		public static bool operator == (Nominal<T> ImpliedObject, T value)
		{
			return GenericArithmetic.Equal (ImpliedObject?.now ?? default, value);
		}

		public static bool operator != (Nominal<T> ImpliedObject, T value)
		{
			return !GenericArithmetic.Equal (ImpliedObject?.now ?? default, value);
		}

		public static T operator + (Nominal<T> ImpliedObject, T value)
		{
			return GenericArithmetic.Add (ImpliedObject.now, value);
		}

		public static T operator - (Nominal<T> ImpliedObject, T value)
		{
			return GenericArithmetic.Subtract (ImpliedObject.now, value);
		}

		public static T operator * (Nominal<T> ImpliedObject, T value)
		{
			return GenericArithmetic.Multiply (ImpliedObject.now, value);
		}

		public static T operator / (Nominal<T> ImpliedObject, T value)
		{
			return GenericArithmetic.Divide (ImpliedObject.now, value);
		}

		private T now = new T ();
		private T before = new T ();
		private float threshold;
	}*/

	public class Nominal_byte
	{
		protected bool Equals (Nominal_byte other)
		{
			return now == other.now && before == other.before && threshold.Equals (other.threshold);
		}

		public override bool Equals (object obj)
		{
			if (ReferenceEquals (null, obj)) return false;
			if (ReferenceEquals (this, obj)) return true;
			if (obj.GetType () != this.GetType ()) return false;
			return Equals ((Nominal_byte)obj);
		}

		public override int GetHashCode ()
		{
			unchecked
			{
				var hashCode = now.GetHashCode ();
				hashCode = (hashCode * 397) ^ before.GetHashCode ();
				hashCode = (hashCode * 397) ^ threshold.GetHashCode ();
				return hashCode;
			}
		}

		public Nominal_byte () : this (default, default, 0)
		{
		}

		public Nominal_byte (byte _now, byte _before, float _threshold)
		{
			now = _now;
			before = _before;
			threshold = _threshold;
		}

		public byte get ()
		{
			return now;
		}

		public byte get (float alpha)
		{
			return alpha >= threshold ? now : before;
		}

		public byte last ()
		{
			return before;
		}

		public void set (byte value)
		{
			now = value;
			before = value;
		}

		public void normalize ()
		{
			before = now;
		}

		public bool normalized ()
		{
			return GenericArithmetic.Equal (before, now);
		}

		public void next (byte value, float thrs)
		{
			before = now;
			now = value;
			threshold = thrs;
		}

		public static bool operator == (Nominal_byte ImpliedObject, byte value)
		{
			return ImpliedObject?.now == value;
		}

		public static bool operator != (Nominal_byte ImpliedObject, byte value)
		{
			return ImpliedObject?.now != value;
		}

		public static byte operator + (Nominal_byte ImpliedObject, byte value)
		{
			return (byte)((ImpliedObject?.now ?? 0) + value);
		}

		public static byte operator - (Nominal_byte ImpliedObject, byte value)
		{
			return (byte)((ImpliedObject?.now ?? 0) - value);
		}

		public static byte operator * (Nominal_byte ImpliedObject, byte value)
		{
			return (byte)((ImpliedObject?.now ?? 0) * value);
		}

		public static byte operator / (Nominal_byte ImpliedObject, byte value)
		{
			return (byte)((ImpliedObject?.now ?? 0) / value);
		}

		private byte now = new byte ();
		private byte before = new byte ();
		private float threshold;
	}

	public class Nominal_short
	{
		protected bool Equals (Nominal_short other)
		{
			return now == other.now && before == other.before && threshold.Equals (other.threshold);
		}

		public override bool Equals (object obj)
		{
			if (ReferenceEquals (null, obj)) return false;
			if (ReferenceEquals (this, obj)) return true;
			if (obj.GetType () != this.GetType ()) return false;
			return Equals ((Nominal_short)obj);
		}

		public override int GetHashCode ()
		{
			unchecked
			{
				var hashCode = now.GetHashCode ();
				hashCode = (hashCode * 397) ^ before.GetHashCode ();
				hashCode = (hashCode * 397) ^ threshold.GetHashCode ();
				return hashCode;
			}
		}

		public Nominal_short () : this (default, default, 0)
		{
		}

		public Nominal_short (short _now, short _before, float _threshold)
		{
			now = _now;
			before = _before;
			threshold = _threshold;
		}

		public short get ()
		{
			return now;
		}

		public short get (float alpha)
		{
			return alpha >= threshold ? now : before;
		}

		public short last ()
		{
			return before;
		}

		public void set (short value)
		{
			now = value;
			before = value;
		}

		public void normalize ()
		{
			before = now;
		}

		public bool normalized ()
		{
			return GenericArithmetic.Equal (before, now);
		}

		public void next (short value, float thrs)
		{
			before = now;
			now = value;
			threshold = thrs;
		}

		public static bool operator == (Nominal_short ImpliedObject, short value)
		{
			return ImpliedObject?.now == value;
		}

		public static bool operator != (Nominal_short ImpliedObject, short value)
		{
			return ImpliedObject?.now != value;
		}

		public static short operator + (Nominal_short ImpliedObject, short value)
		{
			return (short)((ImpliedObject?.now ?? 0) + value);
		}

		public static short operator - (Nominal_short ImpliedObject, short value)
		{
			return (short)((ImpliedObject?.now ?? 0) - value);
		}

		public static short operator * (Nominal_short ImpliedObject, short value)
		{
			return (short)((ImpliedObject?.now ?? 0) * value);
		}

		public static short operator / (Nominal_short ImpliedObject, short value)
		{
			return (short)((ImpliedObject?.now ?? 0) / value);
		}

		private short now = new short ();
		private short before = new short ();
		private float threshold;
	}

	public class Nominal_int
	{
		protected bool Equals (Nominal_int other)
		{
			return now == other.now && before == other.before && threshold.Equals (other.threshold);
		}

		public override bool Equals (object obj)
		{
			if (ReferenceEquals (null, obj)) return false;
			if (ReferenceEquals (this, obj)) return true;
			if (obj.GetType () != this.GetType ()) return false;
			return Equals ((Nominal_int)obj);
		}

		public override int GetHashCode ()
		{
			unchecked
			{
				var hashCode = now;
				hashCode = (hashCode * 397) ^ before;
				hashCode = (hashCode * 397) ^ threshold.GetHashCode ();
				return hashCode;
			}
		}

		public Nominal_int () : this (default, default, 0)
		{
		}

		public Nominal_int (int _now, int _before, float _threshold)
		{
			now = _now;
			before = _before;
			threshold = _threshold;
		}

		public int get ()
		{
			return now;
		}

		public int get (float alpha)
		{
			return alpha >= threshold ? now : before;
		}

		public int last ()
		{
			return before;
		}

		public void set (int value)
		{
			now = value;
			before = value;
		}

		public void normalize ()
		{
			before = now;
		}

		public bool normalized ()
		{
			return GenericArithmetic.Equal (before, now);
		}

		public void next (int value, float thrs)
		{
			before = now;
			now = value;
			threshold = thrs;
		}

		public static bool operator == (Nominal_int ImpliedObject, int value)
		{
			return ImpliedObject?.now == value;
		}

		public static bool operator != (Nominal_int ImpliedObject, int value)
		{
			return ImpliedObject?.now != value;
		}

		public static int operator + (Nominal_int ImpliedObject, int value)
		{
			return (int)((ImpliedObject?.now ?? 0) + value);
		}

		public static int operator - (Nominal_int ImpliedObject, int value)
		{
			return (int)((ImpliedObject?.now ?? 0) - value);
		}

		public static int operator * (Nominal_int ImpliedObject, int value)
		{
			return (int)((ImpliedObject?.now ?? 0) * value);
		}

		public static int operator / (Nominal_int ImpliedObject, int value)
		{
			return (int)((ImpliedObject?.now ?? 0) / value);
		}

		private int now = new int ();
		private int before = new int ();
		private float threshold;
	}
	/*public class Linear<T> where T : unmanaged, IComparable, IComparable<T>, IEquatable<T>
	{
		public Linear ()
		{
		}

		public Linear (T value)
		{
			set (value);
		}

		public T get ()
		{
			return now;
		}

		public T get (float alpha)
		{
			return lerp (before, now, alpha);
		}

		public static T lerp (T first, T second, float alpha)
		{
			return alpha <= 0.0f ? first
				: alpha >= 1.0f ? second
				: GenericArithmetic.Equal (first, second) ? first
				: GenericArithmetic.Add (GenericArithmetic.MultiplySpecific (first, (1.0f - alpha)), GenericArithmetic.MultiplySpecific (second, alpha));
		}

		public T last ()
		{
			return before;
		}

		public void set (T value)
		{
			now = value;
			before = value;
		}

		public void normalize ()
		{
			before = now;
		}

		public bool normalized ()
		{
			return GenericArithmetic.Equal (before, now);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This 'CopyFrom' method was converted from the original copy assignment operator:
//ORIGINAL LINE: void operator = (T value)
		public void CopyFrom (T value)
		{
			before = now;
			now = value;
		}

/#1#/C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The += operator cannot be overloaded in C#:
		public static void operator += (T value)
		{
			before = now;
			now += value;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The -= operator cannot be overloaded in C#:
		public static void operator -= (T value)
		{
			before = now;
			now -= value;
		}#1#

		public static bool operator == (Linear<T> ImpliedObject, T value)
		{
			return GenericArithmetic.Equal (ImpliedObject?.now ?? default, value);
		}

		public static bool operator != (Linear<T> ImpliedObject, T value)
		{
			return !GenericArithmetic.Equal (ImpliedObject?.now ?? default, value);
		}

		public static bool operator < (Linear<T> ImpliedObject, T value)
		{
			return !GenericArithmetic.GreaterThanOrEqual (ImpliedObject.now, value);
		}

		public static bool operator <= (Linear<T> ImpliedObject, T value)
		{
			return !GenericArithmetic.GreaterThan (ImpliedObject.now, value);
		}

		public static bool operator > (Linear<T> ImpliedObject, T value)
		{
			return GenericArithmetic.GreaterThan (ImpliedObject.now, value);
		}

		public static bool operator >= (Linear<T> ImpliedObject, T value)
		{
			return GenericArithmetic.GreaterThanOrEqual (ImpliedObject.now, value);
		}

		/*public static Linear<T> operator + (Linear<T> ImpliedObject, T value)
		{
			ImpliedObject.now = (dynamic)ImpliedObject.now + value;
			return ImpliedObject;
		}#1#

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: T operator + (T value) const
		/*public static T operator + (T value,Linear<T> ImpliedObject)
		{
			return (dynamic)ImpliedObject.now + value;
		}#1#
		public static T operator + (Linear<T> ImpliedObject, T value)
		{
			return GenericArithmetic.Add (ImpliedObject.now, value);
		}

		public static T operator - (Linear<T> ImpliedObject, T value)
		{
			return GenericArithmetic.Subtract (ImpliedObject.now, value);
		}

		public static T operator * (Linear<T> ImpliedObject, T value)
		{
			return GenericArithmetic.Multiply (ImpliedObject.now, value);
		}

		public static T operator / (Linear<T> ImpliedObject, T value)
		{
			return GenericArithmetic.Subtract (ImpliedObject.now, value);
		}

		public static T operator + (Linear<T> ImpliedObject, Linear<T> value)
		{
			return GenericArithmetic.Add (ImpliedObject.now, value.get ());
		}

		public static T operator - (Linear<T> ImpliedObject, Linear<T> value)
		{
			return GenericArithmetic.Subtract (ImpliedObject.now, value.get ());
		}

		public static T operator * (Linear<T> ImpliedObject, Linear<T> value)
		{
			return GenericArithmetic.Multiply (ImpliedObject.now, value.get ());
		}

		public static T operator / (Linear<T> ImpliedObject, Linear<T> value)
		{
			return GenericArithmetic.Subtract (ImpliedObject.now, value.get ());
		}

		public static implicit operator Linear<T> (T value)
		{
			return new Linear<T> (value);
		}

		private T now;
		private T before;
	}*/

	public class Linear_float
	{
		protected bool Equals (Linear_float other)
		{
			return now.Equals (other.now) && before.Equals (other.before);
		}

		public override bool Equals (object obj)
		{
			if (ReferenceEquals (null, obj)) return false;
			if (ReferenceEquals (this, obj)) return true;
			if (obj.GetType () != this.GetType ()) return false;
			return Equals ((Linear_float)obj);
		}

		public override int GetHashCode ()
		{
			unchecked
			{
				return (now.GetHashCode () * 397) ^ before.GetHashCode ();
			}
		}

		public Linear_float ()
		{
		}

		public Linear_float (float value)
		{
			set (value);
		}

		public float get ()
		{
			return now;
		}

		public float get (float alpha)
		{
			return lerp (before, now, alpha);
		}

		public static float lerp (float first, float second, float alpha)
		{
			return alpha <= 0.0f ? first
				: alpha >= 1.0f ? second
				: first == second ? first
				: (1.0f - alpha) * first + alpha * second;
		}

		public float last ()
		{
			return before;
		}

		public void set (float value)
		{
			now = value;
			before = value;
		}

		public void normalize ()
		{
			before = now;
		}

		public bool normalized ()
		{
			return before == now;
		}

//C++ floatO C# CONVERfloatER CRACKED BY X-CRACKER 2017 NOfloatE: floathis 'CopyFrom' method was converted from the original copy assignment operator:
//ORIGINAL LINE: void operator = (float value)
		public void CopyFrom (float value)
		{
			before = now;
			now = value;
		}

/*//C++ floatO C# CONVERfloatER CRACKED BY X-CRACKER 2017 floatODO floatASK: floathe += operator cannot be overloaded in C#:
		public static void operator += (float value)
		{
			before = now;
			now += value;
		}

//C++ floatO C# CONVERfloatER CRACKED BY X-CRACKER 2017 floatODO floatASK: floathe -= operator cannot be overloaded in C#:
		public static void operator -= (float value)
		{
			before = now;
			now -= value;
		}*/

		public static bool operator == (Linear_float ImpliedObject, float value)
		{
			return GenericArithmetic.Equal (ImpliedObject?.now ?? default, value);
		}

		public static bool operator != (Linear_float ImpliedObject, float value)
		{
			return !GenericArithmetic.Equal (ImpliedObject?.now ?? default, value);
		}

		public static bool operator < (Linear_float ImpliedObject, float value)
		{
			return ImpliedObject.now < value;
		}

		public static bool operator <= (Linear_float ImpliedObject, float value)
		{
			return ImpliedObject.now <= value;
		}

		public static bool operator > (Linear_float ImpliedObject, float value)
		{
			return ImpliedObject.now > value;
		}

		public static bool operator >= (Linear_float ImpliedObject, float value)
		{
			return ImpliedObject.now >= value;
		}

		/*public static Linear_float operator + (Linear_float ImpliedObject, float value)
		{
			ImpliedObject.now = (dynamic)ImpliedObject.now + value;
			return ImpliedObject;
		}*/

		public static float operator + (float value, Linear_float ImpliedObject)
		{
			return ImpliedObject.now + value;
		}

		public static float operator + (Linear_float ImpliedObject, float value)
		{
			return ImpliedObject.now + value;
		}

		public static float operator - (Linear_float ImpliedObject, float value)
		{
			return ImpliedObject.now - value;
		}

		public static float operator * (Linear_float ImpliedObject, float value)
		{
			return ImpliedObject.now * value;
		}

		public static float operator / (Linear_float ImpliedObject, float value)
		{
			return ImpliedObject.now / value;
		}

		public static float operator + (Linear_float ImpliedObject, Linear_float value)
		{
			return ImpliedObject.now + value.get ();
		}

		public static float operator - (Linear_float ImpliedObject, Linear_float value)
		{
			return ImpliedObject.now - value.get ();
		}

		public static float operator * (Linear_float ImpliedObject, Linear_float value)
		{
			return ImpliedObject.now * value.get ();
		}

		public static float operator / (Linear_float ImpliedObject, Linear_float value)
		{
			return ImpliedObject.now / value.get ();
		}

		public static implicit operator Linear_float (float value)
		{
			return new Linear_float (value);
		}

		private float now;
		private float before;
	}

	public class Linear_double
	{
		protected bool Equals (Linear_double other)
		{
			return now.Equals (other.now) && before.Equals (other.before);
		}

		public override bool Equals (object obj)
		{
			if (ReferenceEquals (null, obj)) return false;
			if (ReferenceEquals (this, obj)) return true;
			if (obj.GetType () != this.GetType ()) return false;
			return Equals ((Linear_double)obj);
		}

		public override int GetHashCode ()
		{
			unchecked
			{
				return (now.GetHashCode () * 397) ^ before.GetHashCode ();
			}
		}

		public Linear_double ()
		{
		}

		public Linear_double (double value)
		{
			set (value);
		}

		public double get ()
		{
			return now;
		}

		public double get (double alpha)
		{
			return lerp (before, now, alpha);
		}

		public static double lerp (double first, double second, double alpha)
		{
			return alpha <= 0.0f ? first
				: alpha >= 1.0f ? second
				: first == second ? first
				: (1.0f - alpha) * first + alpha * second;
		}

		public double last ()
		{
			return before;
		}

		public void set (double value)
		{
			now = value;
			before = value;
		}

		public void normalize ()
		{
			before = now;
		}

		public bool normalized ()
		{
			return before == now;
		}

//C++ doubleO C# CONVERdoubleER CRACKED BY X-CRACKER 2017 NOdoubleE: doublehis 'CopyFrom' method was converted from the original copy assignment operator:
//ORIGINAL LINE: void operator = (double value)
		public void CopyFrom (double value)
		{
			before = now;
			now = value;
		}

/*//C++ doubleO C# CONVERdoubleER CRACKED BY X-CRACKER 2017 doubleODO doubleASK: doublehe += operator cannot be overloaded in C#:
		public static void operator += (double value)
		{
			before = now;
			now += value;
		}

//C++ doubleO C# CONVERdoubleER CRACKED BY X-CRACKER 2017 doubleODO doubleASK: doublehe -= operator cannot be overloaded in C#:
		public static void operator -= (double value)
		{
			before = now;
			now -= value;
		}*/

		public static bool operator == (Linear_double ImpliedObject, double value)
		{
			return GenericArithmetic.Equal (ImpliedObject?.now ?? default, value);
		}

		public static bool operator != (Linear_double ImpliedObject, double value)
		{
			return !GenericArithmetic.Equal (ImpliedObject?.now ?? default, value);
		}

		public static bool operator < (Linear_double ImpliedObject, double value)
		{
			return ImpliedObject.now < value;
		}

		public static bool operator <= (Linear_double ImpliedObject, double value)
		{
			return ImpliedObject.now <= value;
		}

		public static bool operator > (Linear_double ImpliedObject, double value)
		{
			return ImpliedObject.now > value;
		}

		public static bool operator >= (Linear_double ImpliedObject, double value)
		{
			return ImpliedObject.now >= value;
		}

		/*public static Linear_double operator + (Linear_double ImpliedObject, double value)
		{
			ImpliedObject.now = (dynamic)ImpliedObject.now + value;
			return ImpliedObject;
		}*/

		public static double operator + (double value, Linear_double ImpliedObject)
		{
			return ImpliedObject.now + value;
		}

		public static double operator + (Linear_double ImpliedObject, double value)
		{
			return ImpliedObject.now + value;
		}

		public static double operator - (Linear_double ImpliedObject, double value)
		{
			return ImpliedObject.now - value;
		}

		public static double operator * (Linear_double ImpliedObject, double value)
		{
			return ImpliedObject.now * value;
		}

		public static double operator / (Linear_double ImpliedObject, double value)
		{
			return ImpliedObject.now / value;
		}

		public static double operator + (Linear_double ImpliedObject, Linear_double value)
		{
			return ImpliedObject.now + value.get ();
		}

		public static double operator - (Linear_double ImpliedObject, Linear_double value)
		{
			return ImpliedObject.now - value.get ();
		}

		public static double operator * (Linear_double ImpliedObject, Linear_double value)
		{
			return ImpliedObject.now * value.get ();
		}

		public static double operator / (Linear_double ImpliedObject, Linear_double value)
		{
			return ImpliedObject.now / value.get ();
		}

		public static implicit operator Linear_double (double value)
		{
			return new Linear_double (value);
		}

		private double now;
		private double before;
	}
}