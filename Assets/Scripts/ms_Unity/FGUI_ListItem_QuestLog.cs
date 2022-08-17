/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_ListItem_QuestLog : GButton
    {
        public Controller _c_count;
        public Controller _c_InventoryTypeId;
        public GImage _bg;
        public GTextField _Txt_Name;
        public const string URL = "ui://4916gthqx5bhol6";

        public static FGUI_ListItem_QuestLog CreateInstance()
        {
            return (FGUI_ListItem_QuestLog)UIPackage.CreateObject("ms_Unity", "ListItem_QuestLog");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_count = GetControllerAt(1);
            _c_InventoryTypeId = GetControllerAt(2);
            _bg = (GImage)GetChildAt(0);
            _Txt_Name = (GTextField)GetChildAt(6);
            OnCreate();

        }
    }
}