/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_ChatBar : GComponent
    {
        public FGUI_ChatBoard _ChatBoard;
        public const string URL = "ui://4916gthqq6rfokr";

        public static FGUI_ChatBar CreateInstance()
        {
            return (FGUI_ChatBar)UIPackage.CreateObject("ms_Unity", "ChatBar");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _ChatBoard = (FGUI_ChatBoard)GetChildAt(0);
            OnCreate();

        }
    }
}