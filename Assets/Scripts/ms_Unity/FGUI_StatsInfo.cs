/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_StatsInfo : GComponent
    {
        public GTextField _Txt_RemainAP;
        public GTextField _Txt_StatsInfo;
        public FGUI_Com_AddSubstractAP _Label_Dex;
        public FGUI_Com_AddSubstractAP _Label_Int;
        public FGUI_Com_AddSubstractAP _Label_Str;
        public FGUI_Com_AddSubstractAP _Label_Luck;
        public const string URL = "ui://4916gthqsxcsoks";

        public static FGUI_StatsInfo CreateInstance()
        {
            return (FGUI_StatsInfo)UIPackage.CreateObject("ms_Unity", "StatsInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _Txt_RemainAP = (GTextField)GetChildAt(0);
            _Txt_StatsInfo = (GTextField)GetChildAt(1);
            _Label_Dex = (FGUI_Com_AddSubstractAP)GetChildAt(2);
            _Label_Int = (FGUI_Com_AddSubstractAP)GetChildAt(3);
            _Label_Str = (FGUI_Com_AddSubstractAP)GetChildAt(4);
            _Label_Luck = (FGUI_Com_AddSubstractAP)GetChildAt(5);
            OnCreate();

        }
    }
}