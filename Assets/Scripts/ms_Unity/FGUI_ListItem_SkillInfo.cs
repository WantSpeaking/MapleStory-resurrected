/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_ListItem_SkillInfo : GButton
    {
        public Controller _c_count;
        public Controller _c_InventoryTypeId;
        public GImage _bg;
        public GLoader _GLoader_Icon;
        public GTextField _Txt_Name;
        public GTextField _Txt_Level;
        public GButton _Btn_BT_SPUP0;
        public const string URL = "ui://4916gthqsxcsokv";

        public static FGUI_ListItem_SkillInfo CreateInstance()
        {
            return (FGUI_ListItem_SkillInfo)UIPackage.CreateObject("ms_Unity", "ListItem_SkillInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_count = GetControllerAt(1);
            _c_InventoryTypeId = GetControllerAt(2);
            _bg = (GImage)GetChildAt(0);
            _GLoader_Icon = (GLoader)GetChildAt(6);
            _Txt_Name = (GTextField)GetChildAt(11);
            _Txt_Level = (GTextField)GetChildAt(12);
            _Btn_BT_SPUP0 = (GButton)GetChildAt(13);
            OnCreate();

        }
    }
}