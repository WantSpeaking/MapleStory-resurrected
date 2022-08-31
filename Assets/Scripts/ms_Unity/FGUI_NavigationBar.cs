/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_NavigationBar : GComponent
    {
        public Controller _c_Tab;
        public GButton _Btn_Home;
        public const string URL = "ui://4916gthqpbmlol8";

        public static FGUI_NavigationBar CreateInstance()
        {
            return (FGUI_NavigationBar)UIPackage.CreateObject("ms_Unity", "NavigationBar");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_Tab = GetControllerAt(0);
            _Btn_Home = (GButton)GetChildAt(6);
            OnCreate();

        }
    }
}