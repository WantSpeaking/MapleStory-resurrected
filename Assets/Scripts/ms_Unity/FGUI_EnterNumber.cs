/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_EnterNumber : GComponent
    {
        public Controller _c_NoticeType;
        public GTextField _tet_Title;
        public GTextField _tet_Content;
        public GTextInput _gTextInput_Number;
        public GButton _Btn_No;
        public GButton _Btn_Yes;
        public const string URL = "ui://4916gthqc1rbnpd";

        public static FGUI_EnterNumber CreateInstance()
        {
            return (FGUI_EnterNumber)UIPackage.CreateObject("ms_Unity", "EnterNumber");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_NoticeType = GetControllerAt(0);
            _tet_Title = (GTextField)GetChildAt(2);
            _tet_Content = (GTextField)GetChildAt(3);
            _gTextInput_Number = (GTextInput)GetChildAt(4);
            _Btn_No = (GButton)GetChildAt(5);
            _Btn_Yes = (GButton)GetChildAt(6);
            OnCreate();

        }
    }
}