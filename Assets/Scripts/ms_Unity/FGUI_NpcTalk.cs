/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_NpcTalk : GComponent
    {
        public Controller _c_TalkType;
        public GRichTextField _Txt_text;
        public GButton _Btn_Close;
        public GLoader _GLoader_speaker;
        public GLoader _GLoader_nametag;
        public GTextField _Txt_name;
        public GButton _Btn_No;
        public GButton _Btn_Decline;
        public GButton _Btn_Prev;
        public GButton _Btn_OK;
        public GButton _Btn_YES;
        public GButton _Btn_Accept;
        public GButton _Btn_Next;
        public const string URL = "ui://4916gthqq75iol7";

        public static FGUI_NpcTalk CreateInstance()
        {
            return (FGUI_NpcTalk)UIPackage.CreateObject("ms_Unity", "NpcTalk");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_TalkType = GetControllerAt(0);
            _Txt_text = (GRichTextField)GetChildAt(1);
            _Btn_Close = (GButton)GetChildAt(2);
            _GLoader_speaker = (GLoader)GetChildAt(3);
            _GLoader_nametag = (GLoader)GetChildAt(4);
            _Txt_name = (GTextField)GetChildAt(5);
            _Btn_No = (GButton)GetChildAt(6);
            _Btn_Decline = (GButton)GetChildAt(7);
            _Btn_Prev = (GButton)GetChildAt(8);
            _Btn_OK = (GButton)GetChildAt(9);
            _Btn_YES = (GButton)GetChildAt(10);
            _Btn_Accept = (GButton)GetChildAt(11);
            _Btn_Next = (GButton)GetChildAt(12);
            OnCreate();

        }
    }
}