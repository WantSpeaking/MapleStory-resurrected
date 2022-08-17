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
        public const string URL = "ui://4916gthqq03i0";

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
            OnCreate();

        }
    }
}