/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_ListItem_Party : GButton
    {
        public Controller _c_count;
        public Controller _c_InventoryTypeId;
        public GTextField _Txt_Name;
        public GProgressBar _ProgressBar_HP;
        public const string URL = "ui://4916gthqwhitone";

        public static FGUI_ListItem_Party CreateInstance()
        {
            return (FGUI_ListItem_Party)UIPackage.CreateObject("ms_Unity", "ListItem_Party");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_count = GetControllerAt(1);
            _c_InventoryTypeId = GetControllerAt(2);
            _Txt_Name = (GTextField)GetChildAt(1);
            _ProgressBar_HP = (GProgressBar)GetChildAt(2);
            OnCreate();

        }
    }
}