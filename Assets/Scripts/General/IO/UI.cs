using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Attributes;
using Beebyte.Obfuscator;
using Helper;

namespace ms
{
    [Skip]
    public class UI : Singleton<UI>
    {
        public enum State
        {
            LOGIN,
            GAME,
            CASHSHOP
        }

        public UI()
        {
            state = new UIStateNull();
            enabled = true;
            canDraw = true;
        }

        public void init()
        {
            try
            {
                AppDebug.Log ("start UI.init");
                cursor.init ();
                change_state (State.LOGIN);
            }
            catch (Exception ex)
            {
                AppDebug.LogError (ex.Message);
            }
            
        }

        public void draw(float alpha)
        {
            if (!canDraw) return;

            state.draw(alpha, cursor.get_position());

            scrollingnotice.draw(alpha);

            cursor.draw(alpha);
        }

        public void update()
        {
            state.update();

            scrollingnotice.update();

            cursor.update();
            //AppDebug.Log ($"focusedtextfield :{focusedtextfield == true}\t text:{focusedtextfield.get ()?.get_text ()}");
        }

        public void enable()
        {
            enabled = true;
        }

        public void disable()
        {
            enabled = false;
        }

        public void enable_draw()
        {
            canDraw = true;
        }

        public void disable_draw()
        {
            canDraw = false;
        }

        public void change_state(State id)
        {
            switch (id)
            {
                case State.LOGIN:
                    state = new UIStateLogin();
                    break;
                case State.GAME:
                    state = new UIStateGame();
                    break;
                case State.CASHSHOP:
                    state = new UIStateCashShop();
                    break;
            }
        }

        public void quit()
        {
            quitted = true;
        }

        public bool not_quitted()
        {
            return !quitted;
        }

        public void send_cursor(Point_short pos)
        {
            send_cursor(new Point_short(pos), cursor.get_state());
        }

        public void send_cursor(bool pressed)
        {
            Cursor.State cursorstate = (pressed && enabled) ? Cursor.State.CLICKING : Cursor.State.IDLE;
            Point_short cursorpos = cursor.get_position();

            if (focusedtextfield && pressed)//todo 2 might focuse in send_cursor (new Point_short (cursorpos), cursorstate);,however if removed, focus is always on textField; now try remove focus before send_cursor to ui
            {
                Cursor.State tstate = focusedtextfield.get().send_cursor(cursorpos, pressed);

                switch (tstate)
                {
                    case Cursor.State.IDLE:
                        //AppDebug.Log ($"send_cursor Reset focused");
                        focusedtextfield = new Optional<Textfield>();
                        break;
                }
            }

            send_cursor(new Point_short(cursorpos), cursorstate);


        }

        public void send_cursor(Point_short cursorpos, Cursor.State cursorstate)
        {
            Cursor.State nextstate = state.send_cursor(cursorstate, cursorpos);
            //AppDebug.Log($"send_cursor state:{nextstate} ");

            cursor.set_state(nextstate);
            cursor.set_position(new Point_short(cursorpos));
        }

        public void send_focus(int focused)
        {
            if (focused != 0)
            {
                // The window gained input focus
                byte sfxvolume = Setting<SFXVolume>.get().load();
                Sound.set_sfxvolume(sfxvolume);

                byte bgmvolume = Setting<BGMVolume>.get().load();
                Music.get().set_bgmvolume(bgmvolume);
            }
            else
            {
                // The window lost input focus
                Sound.set_sfxvolume(0);
                Music.get().set_bgmvolume(0);
            }
        }

        public void send_scroll(double yoffset)
        {
            state.send_scroll(yoffset);
        }

        public void send_close()
        {
            state.send_close();
        }

        public void rightclick()
        {
            Point_short pos = cursor.get_position();
            state.rightclick(pos);
        }

        public void doubleclick()
        {
            Point_short pos = cursor.get_position();
            state.doubleclick(pos);
        }

        public void send_key(int keycode, bool pressed, bool isMapleKeycode = false, bool pressing = false)
        {
            if (canDraw == false) return;
            if ((is_key_down[GLFW_KEY.GLFW_KEY_LEFT_ALT] || is_key_down[GLFW_KEY.GLFW_KEY_RIGHT_ALT]) && (is_key_down[GLFW_KEY.GLFW_KEY_ENTER] || is_key_down[GLFW_KEY.GLFW_KEY_KP_ENTER]))
            {
                Window.get().toggle_fullscreen();

                is_key_down[GLFW_KEY.GLFW_KEY_LEFT_ALT] = false;
                is_key_down[GLFW_KEY.GLFW_KEY_RIGHT_ALT] = false;
                is_key_down[GLFW_KEY.GLFW_KEY_ENTER] = false;
                is_key_down[GLFW_KEY.GLFW_KEY_KP_ENTER] = false;

                return;
            }

            if (is_key_down[(GLFW_KEY)keyboard.capslockcode()])
            {
                caps_lock_enabled = !caps_lock_enabled;
            }

            if (focusedtextfield)
            {
                bool ctrl = is_key_down[(GLFW_KEY)keyboard.leftctrlcode()] || is_key_down[(GLFW_KEY)keyboard.rightctrlcode()];

                if (ctrl && pressed)
                {
                    KeyAction.Id action = keyboard.get_ctrl_action(keycode);

                    if (action == KeyAction.Id.COPY || action == KeyAction.Id.PASTE)
                    {
                        if (focusedtextfield.get().can_copy_paste())
                        {
                            switch (action)
                            {
                                case KeyAction.Id.COPY:
                                    //todo 2 Window.get().setclipboard(focusedtextfield.Dereference().get_text());
                                    break;
                                case KeyAction.Id.PASTE:
                                    //todo 2 focusedtextfield.Dereference().add_string(Window.get().getclipboard());
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    bool shift = is_key_down[(GLFW_KEY)keyboard.leftshiftcode()] || is_key_down[(GLFW_KEY)keyboard.rightshiftcode()] || caps_lock_enabled;
                    Keyboard.Mapping mapping = keyboard.get_text_mapping(keycode, shift);
                    focusedtextfield.get().send_key(mapping.type, mapping.action, pressed);
                }
            }
            else
            {
                Keyboard.Mapping mapping = isMapleKeycode ? keyboard.get_maple_mapping(keycode) : keyboard.get_mapping(keycode);

                bool sent = false;
                LinkedList<UIElement.Type> types = new LinkedList<UIElement.Type>();

                bool escape = keycode == (int)GLFW_Util.GLFW_KEY_ESCAPE;
                bool tab = keycode == (int)GLFW_Util.GLFW_KEY_TAB;
                bool enter = keycode == (int)GLFW_Util.GLFW_KEY_ENTER || keycode == (int)GLFW_Util.GLFW_KEY_KP_ENTER;
                bool up_down = keycode == (int)GLFW_Util.GLFW_KEY_UP || keycode == (int)GLFW_Util.GLFW_KEY_DOWN;
                bool left_right = keycode == (int)GLFW_Util.GLFW_KEY_LEFT || keycode == (int)GLFW_Util.GLFW_KEY_RIGHT;
                bool arrows = up_down || left_right;

                var statusbar = UI.get().get_element<UIStatusBar>();
                var channel = UI.get().get_element<UIChannel>();
                var worldmap = UI.get().get_element<UIWorldMap>();
                var optionmenu = UI.get().get_element<UIOptionMenu>();
                var shop = UI.get().get_element<UIShop>();
                var joypad = UI.get().get_element<UIJoypad>();
                var rank = UI.get().get_element<UIRank>();
                var quit = UI.get().get_element<UIQuit>();
                var npctalk = UI.get().get_element<UINpcTalk>();
                //var report = UI::get().get_element<UIReport>();
                //var whisper = UI::get().get_element<UIWhisper>();

                if (npctalk && npctalk.get().is_active())
                {
                    npctalk.get().send_key(mapping.action, pressed, escape);
                    sent = true;
                }
                else if (statusbar && statusbar.get().is_menu_active())
                {
                    statusbar.get().send_key(mapping.action, pressed, escape);
                    sent = true;
                }
                else if (channel && channel.get().is_active() && mapping.action != (int)KeyAction.Id.CHANGECHANNEL)
                {
                    channel.get().send_key(mapping.action, pressed, escape);
                    sent = true;
                }
                else if (worldmap && worldmap.get().is_active() && mapping.action != (int)KeyAction.Id.WORLDMAP)
                {
                    worldmap.get().send_key(mapping.action, pressed, escape);
                    sent = true;
                }
                else if (optionmenu && optionmenu.get().is_active())
                {
                    optionmenu.get().send_key(mapping.action, pressed, escape);
                    sent = true;
                }
                else if (shop && shop.get().is_active())
                {
                    shop.get().send_key(mapping.action, pressed, escape);
                    sent = true;
                }
                else if (joypad && joypad.get().is_active())
                {
                    joypad.get().send_key(mapping.action, pressed, escape);
                    sent = true;
                }
                else if (rank && rank.get().is_active())
                {
                    rank.get().send_key(mapping.action, pressed, escape);
                    sent = true;
                }
                else if (quit && quit.get().is_active())
                {
                    quit.get().send_key(mapping.action, pressed, escape);
                    sent = true;
                }
                else
                {
                    // All
                    if (escape || tab || enter || arrows)
                    {
                        // Login
                        types.AddLast(UIElement.Type.WORLDSELECT);
                        types.AddLast(UIElement.Type.CHARSELECT);
                        types.AddLast(UIElement.Type.RACESELECT); // No tab
                        types.AddLast(UIElement.Type.CLASSCREATION); // No tab (No arrows, but shouldn't send else where)
                        types.AddLast(UIElement.Type.LOGINNOTICE); // No tab (No arrows, but shouldn't send else where)
                        types.AddLast(UIElement.Type.LOGINNOTICE_CONFIRM); // No tab (No arrows, but shouldn't send else where)
                        types.AddLast(UIElement.Type.LOGINWAIT); // No tab (No arrows, but shouldn't send else where)
                    }

                    if (escape)
                    {
                        // Login
                        types.AddLast(UIElement.Type.SOFTKEYBOARD);

                        // Game
                        types.AddLast(UIElement.Type.NOTICE);
                        types.AddLast(UIElement.Type.KEYCONFIG);
                        types.AddLast(UIElement.Type.CHAT);
                        types.AddLast(UIElement.Type.EVENT);
                        types.AddLast(UIElement.Type.STATSINFO);
                        types.AddLast(UIElement.Type.ITEMINVENTORY);
                        types.AddLast(UIElement.Type.EQUIPINVENTORY);
                        types.AddLast(UIElement.Type.SKILLBOOK);
                        types.AddLast(UIElement.Type.QUESTLOG);
                        types.AddLast(UIElement.Type.USERLIST);
                        types.AddLast(UIElement.Type.NPCTALK);
                        types.AddLast(UIElement.Type.CHARINFO);
                    }
                    else if (enter)
                    {
                        // Login
                        types.AddLast(UIElement.Type.SOFTKEYBOARD);

                        // Game
                        types.AddLast(UIElement.Type.NOTICE);
                    }
                    else if (tab)
                    {
                        // Game
                        types.AddLast(UIElement.Type.ITEMINVENTORY);
                        types.AddLast(UIElement.Type.EQUIPINVENTORY);
                        types.AddLast(UIElement.Type.SKILLBOOK);
                        types.AddLast(UIElement.Type.QUESTLOG);
                        types.AddLast(UIElement.Type.USERLIST);
                    }

                    if (types.Count > 0)
                    {
                        var element = state.get_front(types);

                        if (element != null)
                        {
                            element.send_key(mapping.action, pressed, escape);
                            sent = true;
                        }
                    }
                }

                if (!sent)
                {
                    var chatbar = UI.get().get_element<UIChatBar>();

                    if (escape)
                    {
                        if (chatbar && chatbar.get().is_chatopen())
                        {
                            chatbar.get().send_key(mapping.action, pressed, escape);
                        }
                        else
                        {
                            state.send_key(mapping.type, mapping.action, pressed, escape);
                        }
                    }
                    else if (enter)
                    {
                        if (chatbar)
                        {
                            chatbar.get().send_key(mapping.action, pressed, escape);
                        }
                        else
                        {
                            state.send_key(mapping.type, mapping.action, pressed, escape);
                        }
                    }
                    else
                    {
                        //change_state (State.GAME);//todo 2 remove later
                        state.send_key(mapping.type, mapping.action, pressed, escape, pressing);
                    }
                }
            }

            is_key_down[(GLFW_KEY)keycode] = pressed;
        }

        public void send_key(KeyType.Id type, int action, bool pressed, bool escape)
        {
            state.send_key(type, action, pressed, escape);
        }
        public void set_scrollnotice(string notice)
        {
            scrollingnotice.setnotice(notice);
        }

        public void focus_textfield(Textfield tofocus)
        {
            if (focusedtextfield)
            {
                focusedtextfield.get().set_state(Textfield.State.NORMAL);
            }
            //AppDebug.Log ($"focus_textfield  focused");

            focusedtextfield = tofocus;
        }

        public void remove_textfield()
        {
            if (focusedtextfield)
            {
                focusedtextfield.get().set_state(Textfield.State.NORMAL);
            }
            //AppDebug.Log ($"remove_textfield Reset focused");

            focusedtextfield = new Optional<Textfield>();
        }

        public void drag_icon(Icon icon)
        {
            state.drag_icon(icon);
        }

        public void add_keymapping(byte no, byte type, int action)
        {
            keyboard.assign(no, type, action);
        }

        public void clear_tooltip(Tooltip.Parent parent)
        {
            state.clear_tooltip(parent);
        }

        public void show_equip(Tooltip.Parent parent, short slot)
        {
            state.show_equip(parent, slot);
        }

        public void show_item(Tooltip.Parent parent, int item_id)
        {
            state.show_item(parent, item_id);
        }

        public void show_skill(Tooltip.Parent parent, int skill_id, int level, int masterlevel, long expiration)
        {
            state.show_skill(parent, skill_id, level, masterlevel, expiration);
        }

        public void show_text(Tooltip.Parent parent, string text)
        {
            state.show_text(parent, text);
        }

        public void show_map(Tooltip.Parent parent, string name, string description, int mapid, bool bolded)
        {
            state.show_map(parent, name, description, mapid, bolded);
        }

        public Keyboard get_keyboard()
        {
            return keyboard;
        }


        public Optional<T> emplace<T>(params object[] args) where T : UIElement
        {
            var type = typeof(T);
            var uiElementType = (UIElement.Type)(type.GetField("TYPE", Constants.get().bindingFlags_UIElementInfo)?.GetRawConstantValue() ?? UIElement.Type.NONE);
            var toggled = (bool)(type.GetField("TOGGLED", Constants.get().bindingFlags_UIElementInfo)?.GetRawConstantValue() ?? false);
            var focused = (bool)(type.GetField("FOCUSED", Constants.get().bindingFlags_UIElementInfo)?.GetRawConstantValue() ?? false);

            var uiElementType_New = state.pre_add(uiElementType, toggled, focused);
            if (uiElementType_New != UIElement.Type.NONE)
            {
                state.actual_add<T>(uiElementType_New, args);
            }

            return (T)state.get(uiElementType);
        }

        public Optional<T> get_element<T>() where T : UIElement
        {
            var type = typeof(T);
            var uiElementType = (UIElement.Type)(type.GetField("TYPE", Constants.get().bindingFlags_UIElementInfo)?.GetRawConstantValue() ?? UIElement.Type.NONE);
            UIElement element = state.get(uiElementType);

            return (T)element;
        }

        public Optional<T> get_or_create_element<T>(bool showAfterCreate, params object[] args) where T : UIElement
        {
            var result = get_element<T>();
            if (!result)
            {
                result = emplace<T>(args);
            }
            if (showAfterCreate)
            {
                result.get().makeactive();
            }
            else
            {
                result.get().deactivate();
            }
            return result;
        }
        public void remove(UIElement.Type type)
        {
            focusedtextfield = new Optional<Textfield>();
            //AppDebug.Log ($"remove Reset focused");

            state.remove(type);
        }

        private UIState state;

        private Keyboard keyboard = new Keyboard();
        private Cursor cursor = new Cursor();
        private ScrollingNotice scrollingnotice = new ScrollingNotice();

        private Optional<Textfield> focusedtextfield = new Optional<Textfield>();
        private EnumMap<GLFW_KEY, bool> is_key_down = new EnumMap<GLFW_KEY, bool>();

        private bool enabled;
        private bool canDraw;
        private bool quitted;
        private bool caps_lock_enabled = false;
    }
}