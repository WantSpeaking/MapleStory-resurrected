#define USE_NX

using System;
using System.Collections.Generic;
using Beebyte.Obfuscator;
using MapleLib.WzLib;
using provider;

namespace ms
{
    [Skip]
    public class UIOptionMenu : UIDragElement<PosOPTIONMENU>
    {
        public const Type TYPE = UIElement.Type.OPTIONMENU;
        public const bool FOCUSED = true;
        public const bool TOGGLED = false;

        public UIOptionMenu(params object[] args) : this()
        {
        }

        public UIOptionMenu()
        {
            //this.UIDragElement<PosOPTIONMENU> = new <type missing>();
            this.selected_tab = 0;
            MapleData OptionMenu = ms.wz.wzProvider_ui["StatusBar.img"]["OptionMenu"];
            MapleData backgrnd = OptionMenu["backgrnd"];

            sprites.Add(backgrnd);
            sprites.Add(OptionMenu["backgrnd2"]);

            MapleData graphic = OptionMenu["graphic"];

            tab_background[(int)Buttons.TAB0] = graphic["layer:backgrnd"];
            tab_background[(int)Buttons.TAB1] = OptionMenu["sound"]["layer:backgrnd"];
            tab_background[(int)Buttons.TAB2] = OptionMenu["game"]["layer:backgrnd"];
            tab_background[(int)Buttons.TAB3] = OptionMenu["invite"]["layer:backgrnd"];
            tab_background[(int)Buttons.TAB4] = OptionMenu["screenshot"]["layer:backgrnd"];

            buttons[(int)Buttons.CANCEL] = new MapleButton(OptionMenu["button:Cancel"]);
            buttons[(int)Buttons.OK] = new MapleButton(OptionMenu["button:OK"]);
            buttons[(int)Buttons.UIRESET] = new MapleButton(OptionMenu["button:UIReset"]);

            MapleData tab = OptionMenu["tab"];
            MapleData tab_disabled = tab["disabled"];
            MapleData tab_enabled = tab["enabled"];

            for (uint i = (int)Buttons.TAB0; i < (ulong)Buttons.CANCEL; i++)
            {
                buttons[i] = new TwoSpriteButton(tab_disabled[i.ToString()], tab_enabled[i.ToString()]);
            }

            string sButtonUOL = graphic["combo:resolution"]["sButtonUOL"].ToString();
            string ctype = new string(sButtonUOL.back(), 1);
            MapleComboBox.Type type = (MapleComboBox.Type)Convert.ToInt32(ctype);

            List<string> resolutions = new List<string>() { "800 x 600 ( 4 : 3 )", "1024 x 768 ( 4 : 3 )", "1280 x 720 ( 16 : 9 )", "1366 x 768 ( 16 : 9 )", "1920 x 1080 ( 16 : 9 ) - Beta" };

            short max_width = Configuration.get().get_max_width();
            short max_height = Configuration.get().get_max_height();

            if (max_width >= 1920 && max_height >= 1200)
            {
                resolutions.Add("1920 x 1200 ( 16 : 10 ) - Beta");
            }

            ushort default_option = 0;
            short screen_width = Constants.get().get_viewwidth();
            short screen_height = Constants.get().get_viewheight();

            switch (screen_width)
            {
                case 800:
                    default_option = 0;
                    break;
                case 1024:
                    default_option = 1;
                    break;
                case 1280:
                    default_option = 2;
                    break;
                case 1366:
                    default_option = 3;
                    break;
                case 1920:
                    switch (screen_height)
                    {
                        case 1080:
                            default_option = 4;
                            break;
                        case 1200:
                            default_option = 5;
                            break;
                    }

                    break;
            }

            long combobox_width = graphic["combo:resolution"]["boxWidth"];
            Point_short lt = graphic["combo:resolution"]["lt"];

            buttons[(int)Buttons.SELECT_RES] = new MapleComboBox(type, resolutions, default_option, position, lt, combobox_width);

            Point_short bg_dimensions = new Texture(backgrnd).get_dimensions();

            dimension = new Point_short(bg_dimensions);
            dragarea = new Point_short(bg_dimensions.x(), 20);

            change_tab((ushort)Buttons.TAB2);
        }

        //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void draw(float inter) const override
        public override void draw(float inter)
        {
            base.draw_sprites(inter);

            tab_background[selected_tab].draw(position);

            base.draw_buttons(inter);
        }

        public override Cursor.State send_cursor(bool clicked, Point_short cursorpos)
        {
            Cursor.State dstate = base.send_cursor(clicked, new Point_short(cursorpos));

            if (dragged)
            {
                return dstate;
            }

            var button = buttons[(int)Buttons.SELECT_RES];

            if (button.is_pressed())
            {
                if (button.in_combobox(cursorpos))
                {
                    //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
                    Cursor.State new_state = button.send_cursor(clicked, cursorpos);
                    if (new_state != Cursor.State.IDLE)
                    {
                        return new_state;
                    }
                }
                else
                {
                    remove_cursor();
                }
            }

            return base.send_cursor(clicked, new Point_short(cursorpos));
        }

        public override void send_key(int keycode, bool pressed, bool escape)
        {
            if (pressed)
            {
                if (escape)
                {
                    deactivate();
                }
                else if (keycode == (int)KeyAction.Id.RETURN)
                {
                    button_pressed((ushort)Buttons.OK);
                }
            }
        }

        //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: UIElement::Type get_type() const override
        public override UIElement.Type get_type()
        {
            return TYPE;
        }

        public override Button.State button_pressed(ushort buttonid)
        {
            switch ((Buttons)buttonid)
            {
                case Buttons.TAB0:
                case Buttons.TAB1:
                case Buttons.TAB2:
                case Buttons.TAB3:
                case Buttons.TAB4:
                    change_tab(buttonid);
                    return Button.State.IDENTITY;
                case Buttons.CANCEL:
                    deactivate();
                    return Button.State.NORMAL;
                case Buttons.OK:
                    switch ((Buttons)selected_tab)
                    {
                        case Buttons.TAB0:
                            {
                                ushort selected_value = buttons[(int)Buttons.SELECT_RES].get_selected();

                                short width = Constants.get().get_viewwidth();
                                short height = Constants.get().get_viewheight();

                                switch (selected_value)
                                {
                                    case 0:
                                        width = 800;
                                        height = 600;
                                        break;
                                    case 1:
                                        width = 1024;
                                        height = 768;
                                        break;
                                    case 2:
                                        width = 1280;
                                        height = 720;
                                        break;
                                    case 3:
                                        width = 1366;
                                        height = 768;
                                        break;
                                    case 4:
                                        width = 1920;
                                        height = 1080;
                                        break;
                                    case 5:
                                        width = 1920;
                                        height = 1200;
                                        break;
                                }

                                Setting<Width>.get().save((ushort)width);
                                Setting<Height>.get().save((ushort)height);

                                //Window.get().ChangeResloution((short)width, (short)height);
                            }
                            break;
                        case Buttons.TAB1:
                        case Buttons.TAB2:
                        case Buttons.TAB3:
                        case Buttons.TAB4:
                        default:
                            break;
                    }

                    deactivate();
                    return Button.State.NORMAL;
                case Buttons.UIRESET:
                    return Button.State.DISABLED;
                case Buttons.SELECT_RES:
                    buttons[(int)Buttons.SELECT_RES].toggle_pressed();
                    return Button.State.NORMAL;
                default:
                    return Button.State.DISABLED;
            }
        }

        private void change_tab(ushort tabid)
        {
            buttons[selected_tab].set_state(Button.State.NORMAL);
            buttons[tabid].set_state(Button.State.PRESSED);

            selected_tab = tabid;

            switch ((Buttons)tabid)
            {
                case Buttons.TAB0:
                    buttons[(int)Buttons.SELECT_RES].set_active(true);
                    break;
                case Buttons.TAB1:
                case Buttons.TAB2:
                case Buttons.TAB3:
                case Buttons.TAB4:
                    buttons[(int)Buttons.SELECT_RES].set_active(false);
                    break;
                default:
                    break;
            }
        }

        private enum Buttons : ushort
        {
            TAB0,
            TAB1,
            TAB2,
            TAB3,
            TAB4,
            CANCEL,
            OK,
            UIRESET,
            SELECT_RES
        }

        private ushort selected_tab;
        private Texture[] tab_background = new Texture[(int)Buttons.CANCEL];

        public override void Dispose ()
        {
            base.Dispose ();
            foreach (var t in tab_background)
            {
                t?.Dispose ();
            }
        }
    }
}


#if USE_NX
#endif