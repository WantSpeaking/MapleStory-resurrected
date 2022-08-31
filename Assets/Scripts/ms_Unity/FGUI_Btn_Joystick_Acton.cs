/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_Btn_Joystick_Acton : GButton
    {
        public Controller _c_SetupAction;
        public GImage _Img_BG;
        public GImage _Img_Fill;
        public GLoader _GLoader_Icon;
        public Transition _t0;
        public const string URL = "ui://4916gthqq6rfnpu";

        public static FGUI_Btn_Joystick_Acton CreateInstance()
        {
            return (FGUI_Btn_Joystick_Acton)UIPackage.CreateObject("ms_Unity", "Btn_Joystick_Acton");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_SetupAction = GetControllerAt(1);
            _Img_BG = (GImage)GetChildAt(0);
            _Img_Fill = (GImage)GetChildAt(1);
            _GLoader_Icon = (GLoader)GetChildAt(2);
            _t0 = GetTransitionAt(0);
            OnCreate();

        }
    }
}