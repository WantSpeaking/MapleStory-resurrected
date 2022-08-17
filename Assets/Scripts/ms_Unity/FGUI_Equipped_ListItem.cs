/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_Equipped_ListItem : GButton
    {
        public GImage _bg;
        public const string URL = "ui://4916gthqc5k6np9";

        public static FGUI_Equipped_ListItem CreateInstance()
        {
            return (FGUI_Equipped_ListItem)UIPackage.CreateObject("ms_Unity", "Equipped_ListItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _bg = (GImage)GetChildAt(0);
            OnCreate();

        }
    }
}