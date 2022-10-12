using System.Collections.Concurrent;
using System.Collections.Generic;
using Beebyte.Obfuscator;

namespace ms
{
	[Skip]
	public class UIStateCashShop : UIState
	{
		public UIStateCashShop()
		{
			//Window.get().ChangeResloution(Constants.Instance.VIEWWIDTH_CashShop, Constants.Instance.VIEWHEIGHT_CashShop);

			focused = UIElement.Type.NONE;

			emplace<UICashShop>();
		}

		public override void draw(float inter, Point_short cursor)
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

		public override void doubleclick(Point_short pos)
		{
		}
		public override void rightclick(Point_short pos)
		{
		}
		public override void send_key(KeyType.Id type, int action, bool pressed, bool escape, bool pressing = false)
		{
		}
		public override Cursor.State send_cursor(Cursor.State cursorstate, Point_short cursorpos)
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

		public override UIElement.Type pre_add(UIElement.Type type, bool toggled, bool is_focused)
		{
			remove(type);

			if (is_focused)
			{
				focused = type;
			}

			return type;
		}

		public override T actual_add<T> (UIElement.Type type, params object[] args)
		{
			var uiElement = (UIElement)System.Activator.CreateInstance (typeof (T), args);
			elements.TryAdd (type, uiElement);
			return (T)uiElement;
		}

		public override void remove(UIElement.Type type)
		{
			if (focused == type)
			{
				focused = UIElement.Type.NONE;
			}

			elements.TryRemove (type, out var removedElement);
		}
		public override UIElement get(UIElement.Type type)
		{
			return elements.TryGetValue (type);
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
		public override UIElement get_front(Point_short pos)
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

		public void emplace<T> (params object[] args) where T : UIElement
		{
			var type = typeof (T);
			var uiElement_Type = (UIElement.Type)(type.GetField ("TYPE", Constants.get ().bindingFlags_UIElementInfo)?.GetRawConstantValue () ?? UIElement.Type.NONE);
			var uiElement_toggled = (bool)(type.GetField ("TOGGLED", Constants.get ().bindingFlags_UIElementInfo)?.GetRawConstantValue () ?? false);
			var uiElement_focused = (bool)(type.GetField ("FOCUSED", Constants.get ().bindingFlags_UIElementInfo)?.GetRawConstantValue () ?? false);

			var uiElementType_New = pre_add (uiElement_Type, uiElement_toggled, uiElement_focused);
			if (uiElementType_New != UIElement.Type.NONE)
			{
				actual_add<T> (uiElementType_New, args);
			}
		}

		private ConcurrentDictionary<UIElement.Type, UIElement> elements = new ConcurrentDictionary<UIElement.Type,UIElement>();
		private UIElement.Type focused;
	}
}

