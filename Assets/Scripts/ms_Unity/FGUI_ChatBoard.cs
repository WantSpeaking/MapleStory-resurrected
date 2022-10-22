/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_ChatBoard : GComponent
    {
        public GRichTextField _txt_Chat;
        public GTextInput _txt_ChatInput;
        public GButton _Btn_SendMessage;
        public const string URL = "ui://4916gthqsxcsokx";

        public static FGUI_ChatBoard CreateInstance()
        {
            return (FGUI_ChatBoard)UIPackage.CreateObject("ms_Unity", "ChatBoard");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _txt_Chat = (GRichTextField)GetChildAt(2);
            _txt_ChatInput = (GTextInput)GetChildAt(3);
            _Btn_SendMessage = (GButton)GetChildAt(4);
            OnCreate();

        }
    }
}