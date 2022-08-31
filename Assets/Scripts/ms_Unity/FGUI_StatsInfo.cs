/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_StatsInfo : GComponent
    {
        public GTextField _Txt_StatsInfo;
        public const string URL = "ui://4916gthqsxcsoks";

        public static FGUI_StatsInfo CreateInstance()
        {
            return (FGUI_StatsInfo)UIPackage.CreateObject("ms_Unity", "StatsInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _Txt_StatsInfo = (GTextField)GetChildAt(0);
            OnCreate();

        }
    }
}