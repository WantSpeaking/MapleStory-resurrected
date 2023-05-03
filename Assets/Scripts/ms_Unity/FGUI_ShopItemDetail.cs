/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_ShopItemDetail : GComponent
    {
        public Controller _c_InventoryTypeId;
        public GGraph _ClickToHide;
        public FGUI_Itemed_ListItem _ItemIcon;
        public GRichTextField _txt_equipStat;
        public GButton _Btn_Close;
        public GButton _Btn_Buy;
        public GButton _Btn_Sell;
        public const string URL = "ui://4916gthqfokuolr";

        public static FGUI_ShopItemDetail CreateInstance()
        {
            return (FGUI_ShopItemDetail)UIPackage.CreateObject("ms_Unity", "ShopItemDetail");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_InventoryTypeId = GetControllerAt(0);
            _ClickToHide = (GGraph)GetChildAt(0);
            _ItemIcon = (FGUI_Itemed_ListItem)GetChildAt(2);
            _txt_equipStat = (GRichTextField)GetChildAt(3);
            _Btn_Close = (GButton)GetChildAt(4);
            _Btn_Buy = (GButton)GetChildAt(5);
            _Btn_Sell = (GButton)GetChildAt(6);
            OnCreate();

        }
    }
}