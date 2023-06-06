/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_Boss_HPBar : GComponent
    {
        public GProgressBar _ProgressBar_HP;
        public GTextField _txt_Name;
        public const string URL = "ui://4916gthqvjteonf";

        public static FGUI_Boss_HPBar CreateInstance()
        {
            return (FGUI_Boss_HPBar)UIPackage.CreateObject("ms_Unity", "Boss_HPBar");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _ProgressBar_HP = (GProgressBar)GetChildAt(0);
            _txt_Name = (GTextField)GetChildAt(2);
            OnCreate();

        }
    }
}