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


namespace ms
{
	public abstract class UIState : System.IDisposable
	{
		public virtual void Dispose ()
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void draw(float inter, Point<short> cursor) const = 0;
		public abstract void draw (float inter, Point<short> cursor);
		public abstract void update ();

		public abstract void doubleclick (Point<short> pos);
		public abstract void rightclick (Point<short> pos);
		public abstract void send_key (KeyType.Id type, int action, bool pressed, bool escape);
		//public abstract Cursor.State send_cursor (Cursor.State mst, Point<short> pos);
		public abstract void send_scroll (double yoffset);
		public abstract void send_close ();

		//public abstract void drag_icon (Icon icon);
		public abstract void clear_tooltip (Tooltip.Parent parent);
		public abstract void show_equip (Tooltip.Parent parent, short slot);
		public abstract void show_item (Tooltip.Parent parent, int itemid);
		public abstract void show_skill (Tooltip.Parent parent, int skill_id, int level, int masterlevel, long expiration);
		public abstract void show_text (Tooltip.Parent parent, string text);
		public abstract void show_map (Tooltip.Parent parent, string name, string description, int mapid, bool bolded);

		public abstract EnumMap<UIElement.Type, UIElement> pre_add (UIElement.Type type, bool toggled, bool focused);
		public abstract void remove (UIElement.Type type);
		public abstract UIElement get (UIElement.Type type);
		public abstract UIElement get_front (LinkedList<UIElement.Type> types);
		public abstract UIElement get_front (Point<short> pos);
	}

	public class UIStateNull : UIState
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float, Point<short>) const override
		public override void draw (float UnnamedParameter1, Point<short> UnnamedParameter2)
		{
		}

		public override void update ()
		{
		}

		public override void doubleclick (Point<short> UnnamedParameter1)
		{
		}

		public override void rightclick (Point<short> UnnamedParameter1)
		{
		}

		public override void send_key (KeyType.Id UnnamedParameter1, int UnnamedParameter2, bool UnnamedParameter3, bool UnnamedParameter4)
		{
		}

		/*public override Cursor.State send_cursor (Cursor.State UnnamedParameter1, Point<short> UnnamedParameter2)
		{
			return Cursor.State.IDLE;
		}*/

		public override void send_scroll (double UnnamedParameter1)
		{
		}

		public override void send_close ()
		{
		}

		/*public override void drag_icon (Icon UnnamedParameter1)
		{
		}*/

		public override void clear_tooltip (Tooltip.Parent UnnamedParameter1)
		{
		}

		public override void show_equip (Tooltip.Parent UnnamedParameter1, short UnnamedParameter2)
		{
		}

		public override void show_item (Tooltip.Parent UnnamedParameter1, int UnnamedParameter2)
		{
		}

		public override void show_skill (Tooltip.Parent UnnamedParameter1, int UnnamedParameter2, int UnnamedParameter3, int UnnamedParameter4, long UnnamedParameter5)
		{
		}

		public override void show_text (Tooltip.Parent UnnamedParameter1, string UnnamedParameter2)
		{
		}

		public override void show_map (Tooltip.Parent UnnamedParameter1, string UnnamedParameter2, string UnnamedParameter3, int UnnamedParameter4, bool UnnamedParameter5)
		{
		}

		public override EnumMap<UIElement.Type, UIElement> pre_add (UIElement.Type UnnamedParameter1, bool UnnamedParameter2, bool UnnamedParameter3)
		{
			return null;
		}

		public override void remove (UIElement.Type UnnamedParameter1)
		{
		}

		public override UIElement get (UIElement.Type UnnamedParameter1)
		{
			return null;
		}

		public override UIElement get_front (LinkedList<UIElement.Type> UnnamedParameter1)
		{
			return null;
		}

		public override UIElement get_front (Point<short> UnnamedParameter1)
		{
			return null;
		}
	}
}