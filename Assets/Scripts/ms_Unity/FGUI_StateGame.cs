/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_StateGame : GComponent
    {
        public FGUI_CharacterControl _CharacterControl;
        public FGUI_ChatBar _ChatBar;
        public GProgressBar _ProgressBar_Quarter;
        public FGUI_StatusBar _StatusBar;
        public const string URL = "ui://4916gthqq03inph";

        public static FGUI_StateGame CreateInstance()
        {
            return (FGUI_StateGame)UIPackage.CreateObject("ms_Unity", "StateGame");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _CharacterControl = (FGUI_CharacterControl)GetChildAt(0);
            _ChatBar = (FGUI_ChatBar)GetChildAt(1);
            _ProgressBar_Quarter = (GProgressBar)GetChildAt(2);
            _StatusBar = (FGUI_StatusBar)GetChildAt(3);
            OnCreate();

        }
    }
}