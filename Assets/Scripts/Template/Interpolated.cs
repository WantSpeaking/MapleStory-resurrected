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
	public class Nominal<T> where T : unmanaged /*, IComparable, IComparable<T>, IEquatable<T>*/
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
	}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
//ORIGINAL LINE: template <typename T>
	public class Linear<T> where T : unmanaged, IComparable, IComparable<T>, IEquatable<T>
	{
		public Linear ()
		{
		}

		public Linear (T value)
		{
			set (value);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: T get() const
		public T get ()
		{
			return now;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: T get(float alpha) const
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

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: T last() const
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

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool normalized() const
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

/*//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The += operator cannot be overloaded in C#:
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
		}*/

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator == (T value) const
		public static bool operator == (Linear<T> ImpliedObject, T value)
		{
			return GenericArithmetic.Equal (ImpliedObject?.now ?? default, value);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator != (T value) const
		public static bool operator != (Linear<T> ImpliedObject, T value)
		{
			return !GenericArithmetic.Equal (ImpliedObject?.now ?? default, value);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator < (T value) const
		public static bool operator < (Linear<T> ImpliedObject, T value)
		{
			return !GenericArithmetic.GreaterThanOrEqual (ImpliedObject.now, value);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator <= (T value) const
		public static bool operator <= (Linear<T> ImpliedObject, T value)
		{
			return !GenericArithmetic.GreaterThan (ImpliedObject.now, value);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator > (T value) const
		public static bool operator > (Linear<T> ImpliedObject, T value)
		{
			return GenericArithmetic.GreaterThan (ImpliedObject.now, value);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator >= (T value) const
		public static bool operator >= (Linear<T> ImpliedObject, T value)
		{
			return GenericArithmetic.GreaterThanOrEqual (ImpliedObject.now, value);
		}

		/*public static Linear<T> operator + (Linear<T> ImpliedObject, T value)
		{
			ImpliedObject.now = (dynamic)ImpliedObject.now + value;
			return ImpliedObject;
		}*/

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: T operator + (T value) const
		/*public static T operator + (T value,Linear<T> ImpliedObject)
		{
			return (dynamic)ImpliedObject.now + value;
		}*/
		public static T operator + (Linear<T> ImpliedObject, T value)
		{
			return GenericArithmetic.Add (ImpliedObject.now, value);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: T operator - (T value) const
		public static T operator - (Linear<T> ImpliedObject, T value)
		{
			return GenericArithmetic.Subtract (ImpliedObject.now, value);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: T operator * (T value) const
		public static T operator * (Linear<T> ImpliedObject, T value)
		{
			return GenericArithmetic.Multiply (ImpliedObject.now, value);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: T operator / (T value) const
		public static T operator / (Linear<T> ImpliedObject, T value)
		{
			return GenericArithmetic.Subtract (ImpliedObject.now, value);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: T operator + (Linear<T> value) const
		public static T operator + (Linear<T> ImpliedObject, Linear<T> value)
		{
			return GenericArithmetic.Add (ImpliedObject.now, value.get ());
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: T operator - (Linear<T> value) const
		public static T operator - (Linear<T> ImpliedObject, Linear<T> value)
		{
			return GenericArithmetic.Subtract (ImpliedObject.now, value.get ());
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: T operator * (Linear<T> value) const
		public static T operator * (Linear<T> ImpliedObject, Linear<T> value)
		{
			return GenericArithmetic.Multiply (ImpliedObject.now, value.get ());
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: T operator / (Linear<T> value) const
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
	}
}