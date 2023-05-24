/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_EnterNumber : GComponent
    {
        public GTextField _tet_Title;
        public GTextInput _gTextInput_Number;
        public GButton _Btn_No;
        public GButton _Btn_Yes;
        public GButton _Btn_1_4;
        public GButton _Btn_2_4;
        public GButton _Btn_3_4;
        public GButton _Btn_Max;
        public const string URL = "ui://4916gthqc1rbnpd";

        public static FGUI_EnterNumber CreateInstance()
        {
            return (FGUI_EnterNumber)UIPackage.CreateObject("ms_Unity", "EnterNumber");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _tet_Title = (GTextField)GetChildAt(2);
            _gTextInput_Number = (GTextInput)GetChildAt(3);
            _Btn_No = (GButton)GetChildAt(4);
            _Btn_Yes = (GButton)GetChildAt(5);
            _Btn_1_4 = (GButton)GetChildAt(6);
            _Btn_2_4 = (GButton)GetChildAt(7);
            _Btn_3_4 = (GButton)GetChildAt(8);
            _Btn_Max = (GButton)GetChildAt(9);
            OnCreate();

        }
    }
}