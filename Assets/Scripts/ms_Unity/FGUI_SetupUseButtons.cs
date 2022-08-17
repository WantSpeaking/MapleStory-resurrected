/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_SetupUseButtons : GComponent
    {
        public Controller _c_Tab;
        public Controller _c_SetupAction;
        public GList _GList_UseBtns;
        public Transition _t0;
        public const string URL = "ui://4916gthqj4v5okz";

        public static FGUI_SetupUseButtons CreateInstance()
        {
            return (FGUI_SetupUseButtons)UIPackage.CreateObject("ms_Unity", "SetupUseButtons");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_Tab = GetControllerAt(0);
            _c_SetupAction = GetControllerAt(1);
            _GList_UseBtns = (GList)GetChildAt(1);
            _t0 = GetTransitionAt(0);
            OnCreate();

        }
    }
}