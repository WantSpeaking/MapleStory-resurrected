/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_Notice : GComponent
    {
        public Controller _c_NoticeType;
        public GTextField _tet_message;
        public Transition _t_Show;
        public const string URL = "ui://4916gthqc1rbnpb";

        public static FGUI_Notice CreateInstance()
        {
            return (FGUI_Notice)UIPackage.CreateObject("ms_Unity", "Notice");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_NoticeType = GetControllerAt(0);
            _tet_message = (GTextField)GetChildAt(1);
            _t_Show = GetTransitionAt(0);
            OnCreate();

        }
    }
}