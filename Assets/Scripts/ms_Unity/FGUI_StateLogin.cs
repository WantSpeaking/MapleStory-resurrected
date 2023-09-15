/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_StateLogin : GComponent
    {
        public Controller _c_State;
        public GTextField _Txt_Message;
        public GProgressBar _ProgressBar_CheckData;
        public GGroup _CheckData;
        public FGUI_Button_TitleAtlas_0 _Btn_Start;
        public GGroup _ReadyToStart;
        public const string URL = "ui://4916gthqqzhhol2";

        public static FGUI_StateLogin CreateInstance()
        {
            return (FGUI_StateLogin)UIPackage.CreateObject("ms_Unity", "StateLogin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_State = GetControllerAt(0);
            _Txt_Message = (GTextField)GetChildAt(2);
            _ProgressBar_CheckData = (GProgressBar)GetChildAt(3);
            _CheckData = (GGroup)GetChildAt(4);
            _Btn_Start = (FGUI_Button_TitleAtlas_0)GetChildAt(5);
            _ReadyToStart = (GGroup)GetChildAt(6);
            OnCreate();

        }
    }
}