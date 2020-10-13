using System.Collections.Generic;

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
	// Base class for all types of user interfaces on screen.
	public abstract class UIElement : System.IDisposable
	{

		public enum Type
		{
			NONE,
			START,
			LOGIN,
			TOS,
			GENDER,
			WORLDSELECT,
			REGION,
			CHARSELECT,
			LOGINWAIT,
			RACESELECT,
			CLASSCREATION,
			SOFTKEYBOARD,
			LOGINNOTICE,
			LOGINNOTICE_CONFIRM,
			STATUSMESSENGER,
			STATUSBAR,
			CHATBAR,
			BUFFLIST,
			NOTICE,
			NPCTALK,
			SHOP,
			STATSINFO,
			ITEMINVENTORY,
			EQUIPINVENTORY,
			SKILLBOOK,
			QUESTLOG,
			WORLDMAP,
			USERLIST,
			MINIMAP,
			CHANNEL,
			CHAT,
			CHATRANK,
			JOYPAD,
			EVENT,
			KEYCONFIG,
			OPTIONMENU,
			QUIT,
			CHARINFO,
			CASHSHOP,
			NUM_TYPES
		}

		public virtual void Dispose()
		{
		}

/*
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void draw(float alpha) const
		public virtual void draw(float alpha)
		{
			draw_sprites(alpha);
			draw_buttons(alpha);
		}
		public virtual void update()
		{
			foreach (var sprite in sprites)
			{
				sprite.update();
			}

			foreach (var iter in buttons)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
				if (Button * button = iter.second.get())
				{
					button.update();
				}
			}
		}
		public virtual void update_screen(short new_width, short new_height)
		{
		}

		public void makeactive()
		{
			active = true;
		}
		public void deactivate()
		{
			active = false;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_active() const
		public bool is_active()
		{
			return active;
		}

		public virtual void toggle_active()
		{
			if (active)
			{
				deactivate();
			}
			else
			{
				makeactive();
			}
		}
		public virtual Button.State button_pressed(ushort buttonid)
		{
			return Button.State.DISABLED;
		}
		public virtual bool send_icon(Icon icon, Point<short> cursorpos)
		{
			return true;
		}

		public virtual void doubleclick(Point<short> cursorpos)
		{
		}
		public virtual void rightclick(Point<short> cursorpos)
		{
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual bool is_in_range(Point<short> cursorpos) const
		public virtual bool is_in_range(Point<short> cursorpos)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: auto bounds = Rectangle<short>(position, position + dimension);
			var bounds = new Rectangle<short>(new ms.Point(new ms.Point(position)), position + dimension);

			return bounds.contains(cursorpos);
		}
		public virtual void remove_cursor()
		{
			foreach (var btit in buttons)
			{
				var button = btit.second.get();

				if (button.get_state() == Button.State.MOUSEOVER)
				{
					button.set_state(Button.State.NORMAL);
				}
			}
		}
		public virtual Cursor.State send_cursor(bool down, Point<short> pos)
		{
			Cursor.State ret = down ? Cursor.State.CLICKING : Cursor.State.IDLE;

			foreach (var btit in buttons)
			{
				if (btit.second.is_active() && btit.second.bounds(position).contains(pos))
				{
					if (btit.second.get_state() == Button.State.NORMAL)
					{
						Sound(Sound.Name.BUTTONOVER).play();

						btit.second.set_state(Button.State.MOUSEOVER);
						ret = Cursor.State.CANCLICK;
					}
					else if (btit.second.get_state() == Button.State.MOUSEOVER)
					{
						if (down)
						{
							Sound(Sound.Name.BUTTONCLICK).play();

							btit.second.set_state(button_pressed(btit.first));

							ret = Cursor.State.IDLE;
						}
						else
						{
							ret = Cursor.State.CANCLICK;
						}
					}
				}
				else if (btit.second.get_state() == Button.State.MOUSEOVER)
				{
					btit.second.set_state(Button.State.NORMAL);
				}
			}

			return ret;
		}
		public virtual void send_scroll(double yoffset)
		{
		}
		public virtual void send_key(int keycode, bool pressed, bool escape)
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual UIElement::Type get_type() const = 0;
		public abstract UIElement.Type get_type();

		protected UIElement(Point<short> p, Point<short> d, bool a)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this.position = new ms.Point<short>(p);
			this.position = new ms.Point<short>(new ms.Point(p));
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this.dimension = new ms.Point<short>(d);
			this.dimension = new ms.Point<short>(new ms.Point(d));
			this.active = a;
		}
		protected UIElement(Point<short> p, Point<short> d) : this(p, d, true)
		{
		}
		protected UIElement() : this(new Point<short>(), new Point<short>())
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw_sprites(float alpha) const
		protected void draw_sprites(float alpha)
		{
			foreach (Sprite sprite in sprites)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: sprite.draw(position, alpha);
				sprite.draw(new ms.Point(position), alpha);
			}
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw_buttons(float alpha) const
		protected void draw_buttons(float alpha)
		{
			foreach (var iter in buttons)
			{
				if (const Button * button = iter.second.get())
				{
					button.draw(position);
				}
			}
		}

		protected SortedDictionary<ushort, std::unique_ptr<Button>> buttons = new SortedDictionary<ushort, std::unique_ptr<Button>>();
		protected List<Sprite> sprites = new List<Sprite>();
		protected Point<short> position = new Point<short>();
		protected Point<short> dimension = new Point<short>();
		protected bool active;*/
	}
}

