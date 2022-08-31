/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_Equipped_ItemDetail : GComponent
    {
        public GButton _Btn_Wear;
        public GImage _bg;
        public GLoader _icon;
        public GRichTextField _txt_equipStat;
        public const string URL = "ui://4916gthqc5k6np6";

        public static FGUI_Equipped_ItemDetail CreateInstance()
        {
            return (FGUI_Equipped_ItemDetail)UIPackage.CreateObject("ms_Unity", "Equipped_ItemDetail");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _Btn_Wear = (GButton)GetChildAt(1);
            _bg = (GImage)GetChildAt(4);
            _icon = (GLoader)GetChildAt(5);
            _txt_equipStat = (GRichTextField)GetChildAt(6);
            OnCreate();

        }
    }
}