/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_Button_TitleAtlas_0 : GButton
    {
        public Transition _t0;
        public const string URL = "ui://4916gthqhbu7oqb";

        public static FGUI_Button_TitleAtlas_0 CreateInstance()
        {
            return (FGUI_Button_TitleAtlas_0)UIPackage.CreateObject("ms_Unity", "Button_TitleAtlas_0");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _t0 = GetTransitionAt(0);
            OnCreate();

        }
    }
}