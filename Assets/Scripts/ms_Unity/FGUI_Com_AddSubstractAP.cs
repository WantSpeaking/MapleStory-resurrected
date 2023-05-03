/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_Com_AddSubstractAP : GLabel
    {
        public GButton _Btn_Substrate;
        public GButton _Btn_Add;
        public const string URL = "ui://4916gthqszt2oly";

        public static FGUI_Com_AddSubstractAP CreateInstance()
        {
            return (FGUI_Com_AddSubstractAP)UIPackage.CreateObject("ms_Unity", "Com_AddSubstractAP");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _Btn_Substrate = (GButton)GetChildAt(1);
            _Btn_Add = (GButton)GetChildAt(2);
            OnCreate();

        }
    }
}