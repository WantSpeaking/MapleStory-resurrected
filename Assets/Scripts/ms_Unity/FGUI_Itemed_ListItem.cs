/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_Itemed_ListItem : GButton
    {
        public Controller _c_count;
        public Controller _c_InventoryTypeId;
        public Controller _c_EquipStatus;
        public Controller _c_ShowEnchanceCount;
        public GImage _bg;
        public GTextField _txt_EnchanceCount;
        public const string URL = "ui://4916gthq10dqm3a";

        public static FGUI_Itemed_ListItem CreateInstance()
        {
            return (FGUI_Itemed_ListItem)UIPackage.CreateObject("ms_Unity", "Itemed_ListItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_count = GetControllerAt(1);
            _c_InventoryTypeId = GetControllerAt(2);
            _c_EquipStatus = GetControllerAt(3);
            _c_ShowEnchanceCount = GetControllerAt(4);
            _bg = (GImage)GetChildAt(1);
            _txt_EnchanceCount = (GTextField)GetChildAt(13);
            OnCreate();

        }
    }
}