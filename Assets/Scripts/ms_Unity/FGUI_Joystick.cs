/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_Joystick : GComponent
    {
        public GImage _joystick_center;
        public FGUI_circle _joystick;
        public GGraph _joystick_touch;
        public GGroup _JoyStick;
        public FGUI_Btn_Joystick_Acton _Btn_Skill_Up;
        public FGUI_Btn_Joystick_Acton _Btn_Skill_Down;
        public FGUI_Btn_Joystick_Acton _Btn_Skill_Left;
        public FGUI_Btn_Joystick_Acton _Btn_Skill_Right;
        public GGroup _Btn_Skills;
        public Transition _t_ShowBtnSkill;
        public Transition _t_HideBtnSkill;
        public const string URL = "ui://4916gthqebjoolb";

        public static FGUI_Joystick CreateInstance()
        {
            return (FGUI_Joystick)UIPackage.CreateObject("ms_Unity", "Joystick");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _joystick_center = (GImage)GetChildAt(0);
            _joystick = (FGUI_circle)GetChildAt(1);
            _joystick_touch = (GGraph)GetChildAt(2);
            _JoyStick = (GGroup)GetChildAt(5);
            _Btn_Skill_Up = (FGUI_Btn_Joystick_Acton)GetChildAt(6);
            _Btn_Skill_Down = (FGUI_Btn_Joystick_Acton)GetChildAt(7);
            _Btn_Skill_Left = (FGUI_Btn_Joystick_Acton)GetChildAt(8);
            _Btn_Skill_Right = (FGUI_Btn_Joystick_Acton)GetChildAt(9);
            _Btn_Skills = (GGroup)GetChildAt(10);
            _t_ShowBtnSkill = GetTransitionAt(0);
            _t_HideBtnSkill = GetTransitionAt(1);
            OnCreate();

        }
    }
}