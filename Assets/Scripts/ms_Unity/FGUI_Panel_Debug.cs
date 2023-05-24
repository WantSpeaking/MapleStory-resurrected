/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_Panel_Debug : GComponent
    {
        public Controller _c1;
        public GRichTextField _Txt_Log;
        public GButton _Btn_Close;
        public const string URL = "ui://4916gthqcduloms";

        public static FGUI_Panel_Debug CreateInstance()
        {
            return (FGUI_Panel_Debug)UIPackage.CreateObject("ms_Unity", "Panel_Debug");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c1 = GetControllerAt(0);
            _Txt_Log = (GRichTextField)GetChildAt(1);
            _Btn_Close = (GButton)GetChildAt(2);
            OnCreate();

        }
    }
}