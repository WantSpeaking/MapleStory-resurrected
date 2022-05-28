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


namespace ms
{
	//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
	//ORIGINAL LINE: template <typename T>
	public class Optional<T>
	{
		public static Optional<T> Empty = new Optional<T> ();
		public Optional (T p)
		{
			this.val = p;
		}

		public Optional () : this (default)
		{
		}

		/*public static bool operator == (Optional<T> person1, Optional<T> person2)
		{
	
			if ((person1 as object) != null && (person2 as object) != null && person1.val != null && person2.val != null)
				return object.Equals (person1.val, person2.val); //只有当两个对象参数都不为空时才比较
			else if ((person1 as object) == null && (person2 as object) == null)
				return true; //两个都为空的话就当作它们相等了
			else
				return false; //除此之外，都认为他们不相等了
		}*/

		/*public static bool operator != (Optional<T> person1, Optional<T> person2)
		{
			if ((person1 as object) != null && (person2 as object) != null && person1.val != null && person2.val != null)
				return !object.Equals (person1.val, person2.val); //只有当两个对象参数都不为空时才比较
			else if (((person1 as object) == null && (person2 as object) == null) || ((person1 as object) != null && person1.val == null && (person2 as object) == null))
				return false; //两个都为空的话就当作它们相等了
			else
				return true; //除此之外，都认为他们不相等了
		}*/

		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: explicit operator bool() const
		public static implicit operator bool (Optional<T> ImpliedObject)
		{
			return ImpliedObject != null && ImpliedObject.val != null;
		}

		public static implicit operator T (Optional<T> ImpliedObject)
		{
			return ImpliedObject.val;
		}

		public static implicit operator Optional<T> (T ImpliedObject)
		{
			return new Optional<T> (ImpliedObject);
		}


		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: T* get() const
		public T get ()
		{
			return val;
		}

		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: T* operator ->() const
		/*public T Dereference ()
		{
			return val;
		}*/

		/*//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: T& operator *() const
		        public T Indirection()
		        {
		            return val[0];
		        }*/

		private T val;
	}
}