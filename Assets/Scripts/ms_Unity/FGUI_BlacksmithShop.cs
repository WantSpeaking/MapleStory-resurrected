/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_BlacksmithShop : GComponent
    {
        public Controller _c_Sell;
        public Controller _c_Buy;
        public GLoader _GLoader_ToEnchanceEquip;
        public GButton _Btn_Close;
        public GTextField _txt_Meso_Own;
        public GTextField _txt_ToEnchanceEquip;
        public GTextField _txt_EnchanceLevelChange;
        public GTextField _txt_EnchanceDesc;
        public GTextField _txt_EnchanceChance;
        public GTextField _txt_Meso_Cost;
        public GButton _Btn_Enchance;
        public GList _GList_Equip;
        public FGUI_BlacksmithShopItemDetail _ItemDetail;
        public GList _GList_Scroll;
        public const string URL = "ui://4916gthqwhiton2";

        public static FGUI_BlacksmithShop CreateInstance()
        {
            return (FGUI_BlacksmithShop)UIPackage.CreateObject("ms_Unity", "BlacksmithShop");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_Sell = GetControllerAt(0);
            _c_Buy = GetControllerAt(1);
            _GLoader_ToEnchanceEquip = (GLoader)GetChildAt(11);
            _Btn_Close = (GButton)GetChildAt(12);
            _txt_Meso_Own = (GTextField)GetChildAt(15);
            _txt_ToEnchanceEquip = (GTextField)GetChildAt(16);
            _txt_EnchanceLevelChange = (GTextField)GetChildAt(17);
            _txt_EnchanceDesc = (GTextField)GetChildAt(18);
            _txt_EnchanceChance = (GTextField)GetChildAt(19);
            _txt_Meso_Cost = (GTextField)GetChildAt(21);
            _Btn_Enchance = (GButton)GetChildAt(22);
            _GList_Equip = (GList)GetChildAt(23);
            _ItemDetail = (FGUI_BlacksmithShopItemDetail)GetChildAt(24);
            _GList_Scroll = (GList)GetChildAt(25);
            OnCreate();

        }
    }
}