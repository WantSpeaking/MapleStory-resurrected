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
    // Can be used to generate random numbers.
    public class Randomizer
    {
        Random rd = new Random(DateTime.Now.Millisecond);
        public bool next_bool()
        {
            return next_int(2) == 1;
        }

        public bool below(float percent)
        {
            return next_real(1.0f) < percent;
        }

        public bool above(float percent)
        {
            return next_real(1.0f) > percent;
        }

        public double next_real(double to)
        {
            return next_real(0, to);
        }

        public double next_real(double from, double to)
        {
            if (from >= to)
            {
                return from;
            }
            var rdIndex = rd.NextDouble() * (to - from) + from;
            return rdIndex;
        }
        public int next_int(int to)
        {
            return next_int(0, to);
        }

        public int next_int(int from, int to)
        {
            if (from >= to)
            {
                return from;
            }
            int rdIndex = rd.Next(from, to);
            return rdIndex;
        }

        public E next_enum<E>(E to) where E : Enum
        {
            return next_enum<E>(default(E), to);
        }
        public E next_enum<E>(E from, E to) where E : Enum
        {
            var next_underlying = next_int((int)Convert.ChangeType(from, typeof(int)), (int)Convert.ChangeType(to, typeof(int)));

            return (E)Enum.ToObject(typeof(E), next_underlying);
/*
            return (E)Convert.ChangeType(next_underlying, typeof(E));

            var next_underlying = next_int((int)from, (int)Convert.ChangeType(to, typeof(int)));
            return (E)Convert.ChangeType(next_underlying, typeof(E));*/
        }
    }
    /*public class Randomizer
	{
		Random rd = new Random(DateTime.Now.Millisecond);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool next_bool() const
		public bool next_bool ()
		{
			return next_int (2) == 1;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool below(float percent) const
		public bool below (float percent)
		{
			return next_real (1.0f) < percent;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool above(float percent) const
		public bool above (float percent)
		{
			return next_real (1.0f) > percent;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
//ORIGINAL LINE: template <class T>
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: T next_real(T to) const
		public T next_real<T> (T to) where T : unmanaged
		{
			return next_real<T> (0.ToT<T> (), to);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
//ORIGINAL LINE: template <class T>
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: T next_real(T from, T to) const
		public T next_real<T> (T from, T to) where T : unmanaged
		{
			if (GenericArithmetic.GreaterThanOrEqual(from,to))
			{
				return from;
			}
			int rdIndex = rd.Next(from.ToT<int> (), to.ToT<int> ());
			return rdIndex.ToT<T> ();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
//ORIGINAL LINE: template <class T>
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: T next_int(T to) const
		public T next_int<T> (T to) where T : unmanaged
		{
			return next_int<T> ((T)Convert.ChangeType (0, typeof (T)), to);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
//ORIGINAL LINE: template <class T>
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: T next_int(T from, T to) const
		public T next_int<T> (T from, T to) where T : unmanaged
		{
			if (GenericArithmetic.GreaterThanOrEqual(from,to))
			{
				return from;
			}
			Random rd = new Random(DateTime.Now.Millisecond);
			int rdIndex = rd.Next(from.ToT<int> (), to.ToT<int> ());
			return rdIndex.ToT<T> ();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
//ORIGINAL LINE: template <class E>
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: E next_enum(E to = E::LENGTH) const
		public E next_enum<E> (E to) where E : unmanaged
		{
			return next_enum (default (E), to);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
//ORIGINAL LINE: template <class E>
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: E next_enum(E from, E to) const
		public E next_enum<E> (E from, E to) where E : unmanaged
		{
			var next_underlying = next_int (from, to);

			return (E)next_underlying;
		}
	}*/
}