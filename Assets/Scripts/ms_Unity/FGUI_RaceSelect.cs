/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_RaceSelect : GComponent
    {
        public Controller _c1;
        public GList _GList_Job;
        public GTextField _Txt_JobName;
        public GButton _Btn_Back;
        public GButton _Btn_Choose;
        public const string URL = "ui://4916gthqh11soma";

        public static FGUI_RaceSelect CreateInstance()
        {
            return (FGUI_RaceSelect)UIPackage.CreateObject("ms_Unity", "RaceSelect");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c1 = GetControllerAt(0);
            _GList_Job = (GList)GetChildAt(2);
            _Txt_JobName = (GTextField)GetChildAt(5);
            _Btn_Back = (GButton)GetChildAt(6);
            _Btn_Choose = (GButton)GetChildAt(7);
            OnCreate();

        }
    }
}