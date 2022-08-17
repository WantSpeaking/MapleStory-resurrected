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
	public class BoolPair<T>
	{
		public BoolPair (T f, T s)
		{
			this.first = f;
			this.second = s;
		}
		public BoolPair ()
		{
		}

		public void set (bool b, T t)
		{
			if (b)
			{
				first = t;
			}
			else
			{
				second = t;
			}
		}

		/*public void set (bool b, T t1, T t2)
		{
			if (b)
			{
				first = t1;
				second = t2;
			}
			else
			{
				first = t2;
				second = t1;
			}
		}*/

		public T this [bool b]
		{
			get => b ? first : second;
			set
			{
				if (b)
				{
					first = value;
				}
				else
				{
					second = value;
				}
			}
		}

		private T first;
		private T second;
	}
	
	public class BoolPairNew<T> where T:new ()
	{
		public BoolPairNew (T f, T s)
		{
			this.first = f;
			this.second = s;
		}

		public BoolPairNew (BoolPairNew<T> src)
		{
			
		}
		
		public BoolPairNew ()
		{
			this.first = new T();
			this.second = new T();
		}

		public void set (bool b, T t)
		{
			if (b)
			{
				first = t;
			}
			else
			{
				second = t;
			}
		}

		/*public void set (bool b, T t1, T t2)
		{
			if (b)
			{
				first = t1;
				second = t2;
			}
			else
			{
				first = t2;
				second = t1;
			}
		}*/

		public T this [bool b]
		{
			get => b ? first : second;
			set
			{
				if (b)
				{
					first = value;
				}
				else
				{
					second = value;
				}
			}
		}

		private T first;
		private T second;
	}
}