/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_StateCashShop : GComponent
    {
        public GGraph _BG;
        public const string URL = "ui://4916gthqqzhhol3";

        public static FGUI_StateCashShop CreateInstance()
        {
            return (FGUI_StateCashShop)UIPackage.CreateObject("ms_Unity", "StateCashShop");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _BG = (GGraph)GetChildAt(0);
            OnCreate();

        }
    }
}