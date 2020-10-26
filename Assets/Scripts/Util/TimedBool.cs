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
	public class TimedBool
	{
		public TimedBool()
		{
			value = false;
			delay = 0;
			last = 0;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: explicit operator bool() const
		public static explicit operator bool(TimedBool ImpliedObject)
		{
			return ImpliedObject.value;
		}

		public void set_for(long millis)
		{
			last = millis;
			delay = millis;
			value = true;
		}

		public void update()
		{
			update(Constants.TIMESTEP);
		}

		public void update(ushort timestep)
		{
			if (value)
			{
				if (timestep >= delay)
				{
					value = false;
					delay = 0;
				}
				else
				{
					delay -= timestep;
				}
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This 'CopyFrom' method was converted from the original copy assignment operator:
//ORIGINAL LINE: void operator = (bool b)
		public void CopyFrom (bool b)
		{
			value = b;
			delay = 0;
			last = 0;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator == (bool b) const
		public static bool operator == (TimedBool ImpliedObject, bool b)
		{
			return ImpliedObject.value == b;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator != (bool b) const
		public static bool operator != (TimedBool ImpliedObject, bool b)
		{
			return ImpliedObject.value != b;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float alpha() const
		public float alpha()
		{
			return 1.0f - (float)((float)delay / last);
		}

		private long last;
		private long delay;
		private bool value;
	}
}