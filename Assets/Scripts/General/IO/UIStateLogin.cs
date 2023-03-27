using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Beebyte.Obfuscator;

namespace ms
{
    [Skip]
    public class UIStateLogin : UIState
    {
        public UIStateLogin()
        {
            //Window.get().ChangeResloution(Constants.Instance.VIEWWIDTH_Login, Constants.Instance.VIEWHEIGHT_Login);

            focused = UIElement.Type.NONE;

            bool start_shown = Configuration.get().get_start_shown();

            /*if (!start_shown)
			{
				emplace<UILogo> ();
			}
			else*/
            {
                emplace<UILogin>();
            }

            //ms_Unity.FGUI_Manager.Instance.EnterState(ms_Unity.FGUI_StateType.Login);
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

            if (tooltip)
            {
                tooltip.get().draw(cursor + new Point_short(0, 22));
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
            var charselect = UI.get().get_element<UICharSelect>();
            if (charselect)
            {
                charselect.get().doubleclick(pos);
            }
        }

        public override void rightclick(Point_short UnnamedParameter1)
        {
        }

        public override void send_key(KeyType.Id type, int action, bool pressed, bool escape, bool pressing = false)
        {
            //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
            UIElement focusedelement = get(focused);
            if (focusedelement != null)
            {
                if (focusedelement.is_active())
                {
                    focusedelement.send_key(action, pressed, escape);
                    return;
                }
                else
                {
                    focused = UIElement.Type.NONE;

                    return;
                }
            }
        }

        public override Cursor.State send_cursor(Cursor.State cursorstate, Point_short cursorpos)
        {
            bool clicked = cursorstate == Cursor.State.CLICKING || cursorstate == Cursor.State.VSCROLLIDLE;
            var focusedelement = get(focused);
            if (focusedelement != null)
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
                var front = get_front();
                if (front != null)
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

        public override void send_scroll(double UnnamedParameter1)
        {
        }

        public override void send_close()
        {
            var logo = UI.get().get_element<UILogo>();
            var login = UI.get().get_element<UILogin>();
            var region = UI.get().get_element<UIRegion>();

            if (logo && logo.get().is_active() || login && login.get().is_active() || region && region.get().is_active())
            {
                UI.get().quit();
            }
            else
            {
                emplace<UIQuitConfirm>();
            }
        }

        public override void drag_icon(Icon UnnamedParameter1)
        {
        }

        public override void clear_tooltip(Tooltip.Parent parent)
        {
            if (parent == tooltipparent)
            {
                tetooltip.set_text("");
                tooltip = new Optional<Tooltip>();
                tooltipparent = Tooltip.Parent.NONE;
            }
        }

        public override void show_equip(Tooltip.Parent UnnamedParameter1, short UnnamedParameter2)
        {
        }

        public override void show_item(Tooltip.Parent UnnamedParameter1, int UnnamedParameter2)
        {
        }

        public override void show_skill(Tooltip.Parent UnnamedParameter1, int UnnamedParameter2, int UnnamedParameter3, int UnnamedParameter4, long UnnamedParameter5)
        {
        }

        public override void show_text(Tooltip.Parent parent, string text)
        {
            tetooltip.set_text(text);

            if (!string.IsNullOrEmpty(text))
            {
                tooltip = tetooltip;
                tooltipparent = parent;
            }
        }

        public override void show_map(Tooltip.Parent UnnamedParameter1, string UnnamedParameter2, string UnnamedParameter3, int UnnamedParameter4, bool UnnamedParameter5)
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

        public override T actual_add<T>(UIElement.Type type, params object[] args)
        {
            if (elements.TryGetValue(type, out var uiElement) && uiElement != null)
            {
                uiElement.makeactive();
            }
            else
            {
                uiElement = (UIElement)System.Activator.CreateInstance(typeof(T), args);
                elements.TryAdd(type, uiElement);
            }
            uiElement?.OnAdd();

            return (T)uiElement;
        }

        public override void remove(UIElement.Type type)
        {
            if (focused == type)
            {
                focused = UIElement.Type.NONE;
            }

            var element = elements.TryGetValue(type);
            if (element != null)
            {
                element.deactivate();
                //element.Dispose ();
                element?.OnRemove();
            }
        }

        public override UIElement get(UIElement.Type type)
        {
            if (!elements.TryGetValue(type, out var uiElement))
            {
                //AppDebug.Log ($"UIStateLogin get() can't find uiElement by type:{type}");
            }

            return uiElement;
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
            foreach (var type in types)
            {
                var element = elements.TryGetValue(type);

                if (element != null && element.is_active())
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

        public override UIElement get_front(Point_short pos)
        {
            foreach (var pair in elements)
            {
                var element = pair.Value;

                if (element != null && element.is_active() && element.is_in_range(pos))
                {
                    return element;
                }
            }

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

        private void emplace<T>(params object[] args) where T : UIElement
        {
            var type = typeof(T);
            var uiElementType = (UIElement.Type)(type.GetField("TYPE", Constants.get().bindingFlags_UIElementInfo)?.GetRawConstantValue() ?? UIElement.Type.NONE);
            var toggled = (bool)(type.GetField("TOGGLED", Constants.get().bindingFlags_UIElementInfo)?.GetRawConstantValue() ?? false);
            var focused = (bool)(type.GetField("FOCUSED", Constants.get().bindingFlags_UIElementInfo)?.GetRawConstantValue() ?? false);
            var uiElementType_New = pre_add(uiElementType, toggled, focused);
            if (uiElementType_New != UIElement.Type.NONE)
            {
                actual_add<T>(uiElementType_New, args);
            }
        }

        private ConcurrentDictionary<UIElement.Type, UIElement> elements = new ConcurrentDictionary<UIElement.Type, UIElement>();
        private UIElement.Type focused;

        private TextTooltip tetooltip = new TextTooltip();
        private Optional<Tooltip> tooltip = new Optional<Tooltip>();
        private Tooltip.Parent tooltipparent;
    }
}