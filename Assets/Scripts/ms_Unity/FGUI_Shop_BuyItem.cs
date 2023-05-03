/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_Shop_BuyItem : GButton
    {
        public GLoader _GLoader_Icon;
        public GTextField _Txt_Name;
        public GTextField _Txt_Price;
        public const string URL = "ui://4916gthqv27polw";

        public static FGUI_Shop_BuyItem CreateInstance()
        {
            return (FGUI_Shop_BuyItem)UIPackage.CreateObject("ms_Unity", "Shop_BuyItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _GLoader_Icon = (GLoader)GetChildAt(3);
            _Txt_Name = (GTextField)GetChildAt(4);
            _Txt_Price = (GTextField)GetChildAt(5);
            OnCreate();

        }
    }
}