/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_Shop_SellItem : GButton
    {
        public Controller _c_IsCharge;
        public Controller _c_count;
        public GLoader _GLoader_Icon;
        public GTextField _Txt_Name;
        public GTextField _Txt_Price;
        public GButton _Btn_Charge;
        public const string URL = "ui://4916gthqv27polv";

        public static FGUI_Shop_SellItem CreateInstance()
        {
            return (FGUI_Shop_SellItem)UIPackage.CreateObject("ms_Unity", "Shop_SellItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_IsCharge = GetControllerAt(1);
            _c_count = GetControllerAt(2);
            _GLoader_Icon = (GLoader)GetChildAt(3);
            _Txt_Name = (GTextField)GetChildAt(4);
            _Txt_Price = (GTextField)GetChildAt(5);
            _Btn_Charge = (GButton)GetChildAt(6);
            OnCreate();

        }
    }
}