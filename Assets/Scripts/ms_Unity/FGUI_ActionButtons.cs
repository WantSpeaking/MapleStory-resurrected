/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_ActionButtons : GComponent
    {
        public Controller _c_Tab;
        public Controller _c_SetupAction;
        public FGUI_Btn_Joystick_Acton _Btn_Dodge;
        public FGUI_Btn_Joystick_Acton _Btn_Jump;
        public FGUI_Btn_Joystick_Acton _Btn_PickUp;
        public FGUI_Btn_Joystick_Acton _Btn_Skill1;
        public FGUI_Btn_Joystick_Acton _Btn_Skill2;
        public FGUI_Btn_Joystick_Acton _Btn_Skill3;
        public FGUI_Btn_Joystick_Acton _Btn_Skill4;
        public FGUI_Btn_Joystick_Acton _Btn_Skill5;
        public FGUI_Btn_Joystick_Acton _Btn_Skill6;
        public GButton _Btn_LightAttack;
        public GButton _Btn_HeavyAttack;
        public GList _GList_UseBtns;
        public Transition _t0;
        public const string URL = "ui://4916gthqpef7oky";

        public static FGUI_ActionButtons CreateInstance()
        {
            return (FGUI_ActionButtons)UIPackage.CreateObject("ms_Unity", "ActionButtons");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_Tab = GetControllerAt(0);
            _c_SetupAction = GetControllerAt(1);
            _Btn_Dodge = (FGUI_Btn_Joystick_Acton)GetChildAt(0);
            _Btn_Jump = (FGUI_Btn_Joystick_Acton)GetChildAt(1);
            _Btn_PickUp = (FGUI_Btn_Joystick_Acton)GetChildAt(2);
            _Btn_Skill1 = (FGUI_Btn_Joystick_Acton)GetChildAt(4);
            _Btn_Skill2 = (FGUI_Btn_Joystick_Acton)GetChildAt(5);
            _Btn_Skill3 = (FGUI_Btn_Joystick_Acton)GetChildAt(6);
            _Btn_Skill4 = (FGUI_Btn_Joystick_Acton)GetChildAt(7);
            _Btn_Skill5 = (FGUI_Btn_Joystick_Acton)GetChildAt(8);
            _Btn_Skill6 = (FGUI_Btn_Joystick_Acton)GetChildAt(9);
            _Btn_LightAttack = (GButton)GetChildAt(10);
            _Btn_HeavyAttack = (GButton)GetChildAt(11);
            _GList_UseBtns = (GList)GetChildAt(13);
            _t0 = GetTransitionAt(0);
            OnCreate();

        }
    }
}