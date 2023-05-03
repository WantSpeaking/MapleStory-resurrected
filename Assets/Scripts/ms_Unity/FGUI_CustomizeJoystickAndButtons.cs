/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_CustomizeJoystickAndButtons : GComponent
    {
        public Controller _c_Tab;
        public Controller _c_SetupAction;
        public FGUI_ActionButtons _ActionButtons;
        public FGUI_Joystick _Joystick;
        public GButton _Btn_OK;
        public GButton _Btn_Cancel;
        public GButton _Btn_Reset;
        public GGroup _TopRight;
        public Transition _t0;
        public const string URL = "ui://4916gthqd7reolz";

        public static FGUI_CustomizeJoystickAndButtons CreateInstance()
        {
            return (FGUI_CustomizeJoystickAndButtons)UIPackage.CreateObject("ms_Unity", "CustomizeJoystickAndButtons");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_Tab = GetControllerAt(0);
            _c_SetupAction = GetControllerAt(1);
            _ActionButtons = (FGUI_ActionButtons)GetChildAt(1);
            _Joystick = (FGUI_Joystick)GetChildAt(2);
            _Btn_OK = (GButton)GetChildAt(4);
            _Btn_Cancel = (GButton)GetChildAt(5);
            _Btn_Reset = (GButton)GetChildAt(6);
            _TopRight = (GGroup)GetChildAt(7);
            _t0 = GetTransitionAt(0);
            OnCreate();

        }
    }
}