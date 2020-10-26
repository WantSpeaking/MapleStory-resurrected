using System;
using System.Collections.Concurrent;
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
	public class UIStateLogin : UIState
	{
		public UIStateLogin ()
		{
			focused = UIElement.Type.NONE;

			bool start_shown = Configuration.get ().get_start_shown ();

			if (!start_shown)
			{
				emplace<UILogo> ();
			}
			else
			{
				emplace<UILogin> ();
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float inter, Point<short> cursor) const override
		public override void draw (float inter, Point<short> cursor)
		{
			foreach (var iter in elements)
			{
				UIElement element = iter.Value;

				if (element != null && element.is_active ())
				{
					element.draw (inter);
				}
			}

			if (tooltip != null)
			{
				tooltip.Dereference ().draw (cursor + new Point<short> (0, 22));
			}
		}

		public override void update ()
		{
			foreach (var iter in elements)
			{
				UIElement element = iter.Value;

				if (element != null && element.is_active ())
				{
					element.update ();
				}
			}
		}

		public override void doubleclick (Point<short> pos)
		{
			var charselect = UI.get ().get_element<UICharSelect> ();
			if (charselect != null)
			{
				charselect.get ().doubleclick (pos);
			}
		}

		public override void rightclick (Point<short> UnnamedParameter1)
		{
		}

		public override void send_key (KeyType.Id type, int action, bool pressed, bool escape)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
			UIElement focusedelement = get (focused);
			if (focusedelement != null)
			{
				if (focusedelement.is_active ())
				{
					focusedelement.send_key (action, pressed, escape);
					return;
				}
				else
				{
					focused = UIElement.Type.NONE;

					return;
				}
			}
		}

		public override Cursor.State send_cursor (Cursor.State cursorstate, Point<short> cursorpos)
		{
			bool clicked = cursorstate == Cursor.State.CLICKING || cursorstate == Cursor.State.VSCROLLIDLE;
			var focusedelement = get (focused);
			if (focusedelement != null)
			{
				if (focusedelement.is_active ())
				{
					remove_cursor (focusedelement.get_type ());

					return focusedelement.send_cursor (clicked, cursorpos);
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
				if (front != null)
				{
					remove_cursor (front.get_type ());

					return front.send_cursor (clicked, cursorpos);
				}
				else
				{
					return Cursor.State.IDLE;
				}
			}
		}

		public override void send_scroll (double UnnamedParameter1)
		{
		}

		public override void send_close ()
		{
			var logo = UI.get ().get_element<UILogo> ();
			var login = UI.get ().get_element<UILogin> ();
			var region = UI.get ().get_element<UIRegion> ();

			if (logo != null && logo.Dereference ().is_active () || login != null && login.Dereference ().is_active () || region != null && region.Dereference ().is_active ())
			{
				UI.get ().quit ();
			}
			else
			{
				emplace<UIQuitConfirm> ();
			}
		}

		public override void drag_icon (Icon UnnamedParameter1)
		{
		}

		public override void clear_tooltip (Tooltip.Parent parent)
		{
			if (parent == tooltipparent)
			{
				tetooltip.set_text ("");
				tooltip = new Optional<Tooltip> ();
				tooltipparent = Tooltip.Parent.NONE;
			}
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

		public override void show_text (Tooltip.Parent parent, string text)
		{
			tetooltip.set_text (text);

			if (!string.IsNullOrEmpty (text))
			{
				tooltip = tetooltip;
				tooltipparent = parent;
			}
		}

		public override void show_map (Tooltip.Parent UnnamedParameter1, string UnnamedParameter2, string UnnamedParameter3, int UnnamedParameter4, bool UnnamedParameter5)
		{
		}

		public override ConcurrentDictionary<UIElement.Type, UIElement> pre_add (UIElement.Type type, bool toggled, bool is_focused)
		{
			remove (type);

			if (is_focused)
			{
				focused = type;
			}

			return elements;
		}

		public override void remove (UIElement.Type type)
		{
			if (focused == type)
			{
				focused = UIElement.Type.NONE;
			}

			var element = elements.TryGetValue (type);
			if (element != null)
			{
				element.deactivate ();
				//element.Dispose ();
			}
		}

		public override UIElement get (UIElement.Type type)
		{
			return elements[type];
		}

		public UIElement get_front ()
		{
			UIElement front = null;

			foreach (var iter in elements)
			{
				var element = iter.Value;

				if (element != null && element.is_active ())
				{
					front = element;
				}
			}

			return front;
		}

		public override UIElement get_front (LinkedList<UIElement.Type> types)
		{
			foreach (var type in types)
			{
				var element = elements[type];

				if (element != null && element.is_active ())
				{
					return element;
				}
			}
			/*var begin = types.First;
			var end = types.Last;

			for (var iter = begin; iter != end; ++iter)
			{
				var element = elements[*iter];

				if (element != null && element.is_active ())
				{
					return element.get ();
				}
			}*/

			return null;
		}

		public override UIElement get_front (Point<short> pos)
		{
			foreach (var pair in elements)
			{
				var element = pair.Value;

				if (element != null && element.is_active () && element.is_in_range (pos))
				{
					return element;
				}
			}

			return null;
		}

		private void remove_cursor (UIElement.Type type)
		{
			foreach (var iter in elements)
			{
				var element = iter.Value;

				if (element != null && element.is_active () && element.get_type () != type)
				{
					element.remove_cursor ();
				}
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no equivalent in C# to C++11 variadic templates:
		private void emplace<T> (params object[] args) where T : UIElement
		{
			/*var type = typeof (T);
			var uiElementType = (UIElement.Type)type.GetField ("TYPE").GetRawConstantValue ();
			var toggled = (bool)type.GetField ("TOGGLED").GetRawConstantValue ();
			var focused = (bool)type.GetField ("FOCUSED").GetRawConstantValue ();

			var map = state.pre_add (uiElementType, toggled, focused);
			map.values.TryAdd (uiElementType, (T)System.Activator.CreateInstance (type, args));
			return (T)state.get (uiElementType);*/

			var type = typeof (T);
			var uiElementType = (UIElement.Type)type.GetField ("TYPE").GetRawConstantValue ();
			var toggled = (bool)type.GetField ("TOGGLED").GetRawConstantValue ();
			var focused = (bool)type.GetField ("FOCUSED").GetRawConstantValue ();
			var iter = pre_add (uiElementType, toggled, focused);
			if (iter.TryGetValue (uiElementType, out var uiElement) && uiElement != null)
			{
				uiElement.makeactive ();
			}
			else
			{
				iter.TryAdd (uiElementType, (UIElement)System.Activator.CreateInstance (type, args));
			}
		}
		private ConcurrentDictionary<UIElement.Type, UIElement> elements = new ConcurrentDictionary<UIElement.Type, UIElement> ();
		private UIElement.Type focused;

		private TextTooltip tetooltip = new TextTooltip ();
		private Optional<Tooltip> tooltip = new Optional<Tooltip> ();
		private Tooltip.Parent tooltipparent;
	}
}