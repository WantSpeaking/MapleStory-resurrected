/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_ChatBalloon : GLabel
    {
        public GGraph _BG;
        public const string URL = "ui://4916gthqj7bgol1";

        public static FGUI_ChatBalloon CreateInstance()
        {
            return (FGUI_ChatBalloon)UIPackage.CreateObject("ms_Unity", "ChatBalloon");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _BG = (GGraph)GetChildAt(0);
            OnCreate();

        }
    }
}