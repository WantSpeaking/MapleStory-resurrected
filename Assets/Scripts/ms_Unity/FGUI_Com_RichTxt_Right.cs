/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_Com_RichTxt_Right : GComponent
    {
        public GRichTextField _Txt_text;
        public Transition _t0;
        public const string URL = "ui://4916gthqgikwolo";

        public static FGUI_Com_RichTxt_Right CreateInstance()
        {
            return (FGUI_Com_RichTxt_Right)UIPackage.CreateObject("ms_Unity", "Com_RichTxt_Right");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _Txt_text = (GRichTextField)GetChildAt(0);
            _t0 = GetTransitionAt(0);
            OnCreate();

        }
    }
}