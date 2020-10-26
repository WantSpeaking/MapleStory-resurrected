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
	public class UIStateCashShop : UIState
	{
		public UIStateCashShop()
		{
			focused = UIElement.Type.NONE;

			UI.get ().emplace<UICashShop>();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float inter, Point<short> cursor) const override
		public override void draw(float inter, Point<short> cursor)
		{
			foreach (var iter in elements)
			{
				UIElement element = iter.Value;

				if (element != null && element.is_active())
				{
					element.draw(inter);
				}
			}
		}
		public override void update()
		{
			foreach (var iter in elements)
			{
				UIElement element = iter.Value;

				if (element != null && element.is_active())
				{
					element.update();
				}
			}
		}

		public override void doubleclick(Point<short> pos)
		{
		}
		public override void rightclick(Point<short> pos)
		{
		}
		public override void send_key(KeyType.Id type, int action, bool pressed, bool escape)
		{
		}
		public override Cursor.State send_cursor(Cursor.State cursorstate, Point<short> cursorpos)
		{
			bool clicked = cursorstate == Cursor.State.CLICKING || cursorstate == Cursor.State.VSCROLLIDLE;
			var focusedelement = get (focused);
			if (focusedelement!=null)
			{
				if (focusedelement.is_active())
				{
					remove_cursor(focusedelement.get_type());

					return focusedelement.send_cursor(clicked, cursorpos);
				}
				else
				{
					focused = UIElement.Type.NONE;

					return cursorstate;
				}
			}
			else
			{
				var front = get_front ();
				if (front!=null)
				{
					remove_cursor(front.get_type());

					return front.send_cursor(clicked, cursorpos);
				}
				else
				{
					return Cursor.State.IDLE;
				}
			}
		}
		public override void send_scroll(double yoffset)
		{
		}
		public override void send_close()
		{
		}

		public override void drag_icon(Icon icon)
		{
		}
		public override void clear_tooltip(Tooltip.Parent parent)
		{
		}
		public override void show_equip(Tooltip.Parent parent, short slot)
		{
		}
		public override void show_item(Tooltip.Parent parent, int itemid)
		{
		}
		public override void show_skill(Tooltip.Parent parent, int skill_id, int level, int masterlevel, long expiration)
		{
		}
		public override void show_text(Tooltip.Parent parent, string text)
		{
		}
		public override void show_map(Tooltip.Parent parent, string name, string description, int mapid, bool bolded)
		{
		}

		public override EnumMap<UIElement.Type, UIElement> pre_add(UIElement.Type type, bool toggled, bool is_focused)
		{
			remove(type);

			if (is_focused)
			{
				focused = type;
			}

			return elements;
		}
		public override void remove(UIElement.Type type)
		{
			if (focused == type)
			{
				focused = UIElement.Type.NONE;
			}

			var element = elements[type];
			if (element!=null)
			{
				element.deactivate();
				element.Dispose ();
			}
		}
		public override UIElement get(UIElement.Type type)
		{
			return elements[type];
		}
		public UIElement get_front()
		{
			UIElement front = null;

			foreach (var iter in elements)
			{
				var element = iter.Value;

				if (element != null && element.is_active())
				{
					front = element;
				}
			}

			return front;
		}
		public override UIElement get_front(LinkedList<UIElement.Type> types)
		{
			return null;
		}
		public override UIElement get_front(Point<short> pos)
		{
			return null;
		}

		private void remove_cursor(UIElement.Type type)
		{
			foreach (var iter in elements)
			{
				var element = iter.Value;

				if (element != null && element.is_active() && element.get_type() != type)
				{
					element.remove_cursor();
				}
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no equivalent in C# to C++11 variadic templates:
		/*private void emplace<T, typename...Args>(Args & ...args)
		{
			if (auto iter = pre_add(T.TYPE, T.TOGGLED, T.FOCUSED))
			{
				iter.second = std::make_unique<T>(std::forward<Args>(args)...);
			}
		}*/

		private EnumMap<UIElement.Type, UIElement> elements = new EnumMap<UIElement.Type,UIElement>();
		private UIElement.Type focused;
	}
}

