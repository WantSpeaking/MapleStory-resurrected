/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_circle : GButton
    {
        public GImage _thumb;
        public const string URL = "ui://4916gthqq03i2";

        public static FGUI_circle CreateInstance()
        {
            return (FGUI_circle)UIPackage.CreateObject("ms_Unity", "circle");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _thumb = (GImage)GetChildAt(0);
            OnCreate();

        }
    }
}