/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_Shop : GComponent
    {
        public Controller _c_Sell;
        public Controller _c_Buy;
        public GButton _Btn_Leave;
        public GList _GList_Buy;
        public GLoader _GLoader_Npc;
        public GGroup _SellTab;
        public GButton _Btn_Meso;
        public GList _GList_Sell;
        public GLoader _GLoader_Player;
        public FGUI_ShopItemDetail _ShopItemDetail;
        public const string URL = "ui://4916gthqsqkc38";

        public static FGUI_Shop CreateInstance()
        {
            return (FGUI_Shop)UIPackage.CreateObject("ms_Unity", "Shop");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_Sell = GetControllerAt(0);
            _c_Buy = GetControllerAt(1);
            _Btn_Leave = (GButton)GetChildAt(7);
            _GList_Buy = (GList)GetChildAt(8);
            _GLoader_Npc = (GLoader)GetChildAt(9);
            _SellTab = (GGroup)GetChildAt(16);
            _Btn_Meso = (GButton)GetChildAt(17);
            _GList_Sell = (GList)GetChildAt(18);
            _GLoader_Player = (GLoader)GetChildAt(19);
            _ShopItemDetail = (FGUI_ShopItemDetail)GetChildAt(20);
            OnCreate();

        }
    }
}