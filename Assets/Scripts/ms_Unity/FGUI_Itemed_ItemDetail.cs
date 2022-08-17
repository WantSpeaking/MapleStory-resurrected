/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_Itemed_ItemDetail : GComponent
    {
        public Controller _c_InventoryTypeId;
        public GGraph _ClickToHide;
        public FGUI_Itemed_ListItem _ItemIcon;
        public GRichTextField _txt_equipStat;
        public GButton _Btn_Drop;
        public GButton _Btn_Close;
        public GButton _Btn_Equip;
        public GButton _Btn_Use;
        public GButton _Btn_SetToUseSlot;
        public GGroup _Use;
        public GButton _Btn_UnEquip;
        public const string URL = "ui://4916gthqclppm49";

        public static FGUI_Itemed_ItemDetail CreateInstance()
        {
            return (FGUI_Itemed_ItemDetail)UIPackage.CreateObject("ms_Unity", "Itemed_ItemDetail");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_InventoryTypeId = GetControllerAt(0);
            _ClickToHide = (GGraph)GetChildAt(0);
            _ItemIcon = (FGUI_Itemed_ListItem)GetChildAt(2);
            _txt_equipStat = (GRichTextField)GetChildAt(3);
            _Btn_Drop = (GButton)GetChildAt(4);
            _Btn_Close = (GButton)GetChildAt(5);
            _Btn_Equip = (GButton)GetChildAt(6);
            _Btn_Use = (GButton)GetChildAt(7);
            _Btn_SetToUseSlot = (GButton)GetChildAt(8);
            _Use = (GGroup)GetChildAt(9);
            _Btn_UnEquip = (GButton)GetChildAt(10);
            OnCreate();

        }
    }
}