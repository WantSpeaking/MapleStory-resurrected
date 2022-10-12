using System;
using System.Collections.Generic;
using Beebyte.Obfuscator;

namespace ms
{
    [Skip]
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
            NUM_TYPES,
            FadeYesNo_PartyInvite,
            ContextMenu,
            PartySideMenu,
            PartyMember_HP,
            Joystick,
            ActionButton,
            KEYSELECT,
        }

        protected UIElement(Point_short p, Point_short d, bool a)
        {
            this.position = new ms.Point_short(p);
            this.dimension = new ms.Point_short(d);
            //AppDebug.Log($"UIElement().active:{a}");
            this.active = a;
        }

        protected UIElement(Point_short p, Point_short d) : this(p, d, true)
        {
        }

        protected UIElement() : this(new Point_short(), new Point_short())
        {
        }


        public virtual void draw(float alpha)
        {
            draw_sprites(alpha);
            draw_buttons(alpha);
        }

        protected void draw_sprites(float alpha)
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.draw(new Point_short(position), alpha);
            }
        }

        protected virtual void draw_buttons(float alpha)
        {
            foreach (var iter in buttons)
            {
                iter.Value?.draw(position);
                /*if (const Button* button = iter.Value.get ())
				{
					button.draw (position);
				}*/
            }
        }

        public virtual void update()
        {
            foreach (var sprite in sprites)
            {
                sprite.update();
            }

            foreach (var iter in buttons)
            {
                iter.Value?.update();
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
            return Button.State.NORMAL; //todo 2 why is button disabled
        }

        public virtual Button.State button_up(ushort buttonid, Button.State returnedButtoonState = Button.State.NORMAL)
        {
            return Button.State.NORMAL; //todo 2 why is button disabled
        }

        public virtual bool send_icon(Icon icon, Point_short cursorpos)
        {
            return true;
        }

        public virtual void doubleclick(Point_short cursorpos)
        {
        }

        public virtual void rightclick(Point_short cursorpos)
        {
        }

        public virtual bool is_in_range(Point_short cursorpos)
        {
            var bounds = new Rectangle_short(new Point_short(position), position + dimension);

            return bounds.contains(cursorpos);
        }

        public virtual void remove_cursor()
        {
            foreach (var btit in buttons)
            {
                var button = btit.Value;

                if (button.get_state() == Button.State.MOUSEOVER)
                {
                    button.set_state(Button.State.NORMAL);
                }
            }
        }

        public virtual Cursor.State send_cursor(bool down, Point_short pos)
        {
            Cursor.State ret = down ? Cursor.State.CLICKING : Cursor.State.IDLE;

            foreach (var btit in buttons)
            {
                if (btit.Value is MapleButton mapleButton)
                {
                    //AppDebug.Log($"{mapleButton.ToString ()}\t is_active:{btit.Value.is_active ()}\t eleposition:{position}\t bounds:{btit.Value.bounds (position)} pos:{pos}\t contains:{btit.Value.bounds (position).contains (pos)}");
                }

                if (btit.Value.is_active() && btit.Value.bounds(position).contains(pos))
                {
                    if (btit.Value.get_state() == Button.State.NORMAL)
                    {
                        btit.Value.playSound(Sound.Name.BUTTONOVER);

                        btit.Value.set_state(Button.State.MOUSEOVER);
                        ret = Cursor.State.CANCLICK;
                    }
                    else if (btit.Value.get_state() == Button.State.MOUSEOVER)
                    {
                        if (down)
                        {
                            btit.Value.playSound(Sound.Name.BUTTONCLICK);

                            btit.Value.set_state(button_pressed((ushort)btit.Key));

                            ret = Cursor.State.IDLE;
                        }
                        else
                        {
                            ret = Cursor.State.CANCLICK;
                        }
                    }
                }
                else if (btit.Value.get_state() == Button.State.MOUSEOVER)
                {
                    btit.Value.set_state(Button.State.NORMAL);
                }
            }


            foreach (var btit in buttons)
            {
                if (btit.Value.is_active() && btit.Value.bounds(position).contains(pos))
                {
                    if (btit.Value.eventState == Button.EventState.UP)
                    {
                        btit.Value.eventState = Button.EventState.MOUSEOVER;
                        btit.Value.RollIn(btit.Value, null);
                    }
                    else if (btit.Value.eventState == Button.EventState.MOUSEOVER)
                    {
                        if (down)
                        {
                            btit.Value.eventState = Button.EventState.DOWN;
                            btit.Value.Down(btit.Value, null);
                        }
                        else
                        {
                            btit.Value.RollOver(btit.Value, null);
                        }
                    }
                    else if (btit.Value.eventState == Button.EventState.DOWN)
                    {
                        if (down)
                        {
                            btit.Value.Pressing(btit.Value, null);
                        }
                        else
                        {
                            btit.Value.eventState = Button.EventState.MOUSEOVER;
                            btit.Value.Up(btit.Value, null);
                            btit.Value.Click(btit.Value, null);
                        }
                    }
                }
                else if (btit.Value.eventState == Button.EventState.MOUSEOVER)
                {
                    btit.Value.eventState = Button.EventState.UP;
                    btit.Value.RollOut(btit.Value, null);
                }
                else if (btit.Value.eventState == Button.EventState.DOWN)
                {
                    btit.Value.eventState = Button.EventState.UP;
                    btit.Value.RollOut(btit.Value, null);
                    btit.Value.Up(btit.Value, null);
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

        public abstract UIElement.Type get_type();

        public virtual void Dispose()
        {
        }


        protected SortedDictionary<uint, Button> buttons = new SortedDictionary<uint, Button>();
        protected List<Sprite> sprites = new List<Sprite>();
        protected Point_short position = new Point_short();
        protected Point_short dimension = new Point_short();
        private bool _active = true;

        protected bool active
        {
            get
            {
                //AppDebug.Log ($"get:{this.GetType ()} \t active:{_active}");
                return _active;
            }
            set
            {
                if (_active != value)
                    OnActivityChange(value);
                //AppDebug.Log ($"set:{this.GetType ()} \t active:{value}");
                _active = value;
            }
        }

        public virtual Type TYPE => UIElement.Type.NONE;

        public virtual void OnActivityChange(bool isActive)
        {
        }

        public virtual void OnGet()
        {
        }

        public virtual void OnRemove()
        {
        }

        public virtual void OnAdd()
        {
        }
    }
}