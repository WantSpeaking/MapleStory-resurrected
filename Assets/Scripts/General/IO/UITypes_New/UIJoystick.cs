using System;
using System.Collections.Generic;
using System.Text;
using Helper;
using MapleLib.WzLib;

namespace ms
{
    [Beebyte.Obfuscator.Skip]
    public class UIJoystick : UIElement
    {
        public const Type TYPE = UIElement.Type.Joystick;
        public const bool FOCUSED = false;
        public const bool TOGGLED = false;

        enum Buttons
        {
            Left,
            Right,
            Top,
            Bottom,
        }

        public UIJoystick()
        {
            WzObject Login = ms.wz.wzFile_ui["Login.img"];
            WzObject Common = Login["Common"];
            WzObject CharSelect = Login["CharSelect"];

            var offset_Joystick = new Point_short((short)(Constants.get().get_viewwidth() * 1 / 8), (short)(Constants.get().get_viewheight() * 1 / 2));

            var btn_Left = new MapleButton(CharSelect["pageL"], new Point_short(0, 50));
            var btn_Right = new MapleButton(CharSelect["pageR"], new Point_short(100, 50));
            var btn_Top = new MapleButton(CharSelect["pageR"], new Point_short(50, 0));
            var btn_Bottom = new MapleButton(CharSelect["pageR"], new Point_short(50, 100));

            btn_Left.is_PlaySound = false;
            btn_Right.is_PlaySound = false;
            btn_Top.is_PlaySound = false;
            btn_Bottom.is_PlaySound = false;

            btn_Left.OnDown += OnDown_Left;
            btn_Left.OnUp += OnUp_Left;

            btn_Right.OnDown += OnDown_Right;
            btn_Right.OnUp += OnUp_Right;

            btn_Top.OnDown += OnDown_Top;
            btn_Top.OnUp += OnUp_Top;

            btn_Bottom.OnDown += OnDown_Bottom;
            btn_Bottom.OnUp += OnUp_Bottom;

/*            buttons[(int)Buttons.Left] = btn_Left;
            buttons[(int)Buttons.Right] = btn_Right;
            buttons[(int)Buttons.Top] = btn_Top;
            buttons[(int)Buttons.Bottom] = btn_Bottom;*/


            position = offset_Joystick;

            ms_Unity.FGUI_Manager.Instance.OpenFGUI<ms_Unity.FGUI_Joystick> ();
            //dimension = new Point_short((short)(200), (short)(200));
        }

        private void OnUp_Bottom(object sender, object e)
        {
            //send_key(Keys.Down, false);
        }

        private void OnDown_Bottom(object sender, object e)
        {
            //send_key(Keys.Down, true);
        }

        private void OnUp_Top(object sender, object e)
        {
            //send_key(Keys.Up, false);
        }

        private void OnDown_Top(object sender, object e)
        {
            //send_key(Keys.Up, true);
        }

        private void OnUp_Right(object sender, object e)
        {
            //send_key(Keys.Right, false);
        }

        private void OnDown_Right(object sender, object e)
        {
            //send_key(Keys.Right, true);
        }

        private void OnUp_Left(object sender, object e)
        {
            //send_key(Keys.Left, false);
        }

        private void OnDown_Left(object sender, object e)
        {
            //send_key(Keys.Left, true);
        }

        public override Type get_type()
        {
            return TYPE;
        }

     /*   private void send_key(Keys keys, bool pressed)
        {
            UI.get().send_key(GLFW_Util.XNAKeyCodeToGLFW_KEY(keys), pressed);
        }*/

        public override bool is_in_range(Point_short cursorpos)
        {
            foreach (var btit in buttons)
            {
                if (btit.Value.is_active() && btit.Value.bounds(position).expand(5).contains(cursorpos))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
