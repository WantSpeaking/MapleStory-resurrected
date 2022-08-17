/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_StateLogin : GComponent
    {
        public GGraph _BG;
        public const string URL = "ui://4916gthqqzhhol2";

        public static FGUI_StateLogin CreateInstance()
        {
            return (FGUI_StateLogin)UIPackage.CreateObject("ms_Unity", "StateLogin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _BG = (GGraph)GetChildAt(0);
            OnCreate();

        }
    }
}