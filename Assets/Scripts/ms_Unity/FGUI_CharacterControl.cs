/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_CharacterControl : GComponent
    {
        public FGUI_Joystick _Joystick;
        public FGUI_ActionButtons _ActionButtons;
        public const string URL = "ui://4916gthqq6rfnpr";

        public static FGUI_CharacterControl CreateInstance()
        {
            return (FGUI_CharacterControl)UIPackage.CreateObject("ms_Unity", "CharacterControl");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _Joystick = (FGUI_Joystick)GetChildAt(0);
            _ActionButtons = (FGUI_ActionButtons)GetChildAt(1);
            OnCreate();

        }
    }
}