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
	// Base class for different button types.
	public abstract class Button : System.IDisposable
	{
		public enum State
		{
			NORMAL,
			DISABLED,
			MOUSEOVER,
			PRESSED,
			IDENTITY,
			NUM_STATES
		}

		public virtual void Dispose()
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void draw(Point<short> parentpos) const = 0;
		public abstract void draw(Point<short> parentpos);
		public abstract void update();
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual Rectangle<short> bounds(Point<short> parentpos) const = 0;
		public abstract Rectangle<short> bounds(Point<short> parentpos);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual short width() const = 0;
		public abstract short width();
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual Point<short> origin() const = 0;
		public abstract Point<short> origin();
		public abstract Cursor.State send_cursor(bool clicked, Point<short> cursorpos);
		public virtual bool in_combobox(Point<short> cursorpos)
		{
			return false;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual ushort get_selected() const
		public virtual ushort get_selected()
		{
			return 0;
		}

		public void set_position(Point<short> pos)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: position = pos;
			position=(pos);
		}
		public void set_state(State s)
		{
			if (s == Button.State.IDENTITY)
			{
				return;
			}

			state = s;
		}
		public void set_active(bool a)
		{
			active = a;
		}
		public void toggle_pressed()
		{
			pressed = !pressed;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_active() const
		public bool is_active()
		{
			return active && state != Button.State.DISABLED;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Button::State get_state() const
		public Button.State get_state()
		{
			return state;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_pressed() const
		public bool is_pressed()
		{
			return pressed;
		}

		protected State state;
		protected Point<short> position = new Point<short>();
		protected bool active;
		protected bool pressed;
	}
}
