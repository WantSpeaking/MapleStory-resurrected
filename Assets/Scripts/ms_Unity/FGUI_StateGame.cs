/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_StateGame : GComponent
    {
        public GProgressBar _ProgressBar_Quarter;
        public const string URL = "ui://4916gthqq03inph";

        public static FGUI_StateGame CreateInstance()
        {
            return (FGUI_StateGame)UIPackage.CreateObject("ms_Unity", "StateGame");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _ProgressBar_Quarter = (GProgressBar)GetChildAt(0);
            OnCreate();

        }
    }
}