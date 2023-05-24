/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_ListItem_QuestLogMini : GButton
    {
        public Controller _c_count;
        public Controller _c_InventoryTypeId;
        public GTextField _Txt_Name;
        public GTextField _Txt_Desc;
        public const string URL = "ui://4916gthqh0zzomw";

        public static FGUI_ListItem_QuestLogMini CreateInstance()
        {
            return (FGUI_ListItem_QuestLogMini)UIPackage.CreateObject("ms_Unity", "ListItem_QuestLogMini");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_count = GetControllerAt(1);
            _c_InventoryTypeId = GetControllerAt(2);
            _Txt_Name = (GTextField)GetChildAt(1);
            _Txt_Desc = (GTextField)GetChildAt(2);
            OnCreate();

        }
    }
}