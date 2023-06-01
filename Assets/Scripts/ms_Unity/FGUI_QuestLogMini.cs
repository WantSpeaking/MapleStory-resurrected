/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_QuestLogMini : GComponent
    {
        public Controller _c_Page;
        public Controller _c_ShowOrHide;
        public Controller _c_PartyStatus;
        public GList _GList_QuestInfo_in_progress;
        public GGroup _Quest;
        public GButton _Btn_CreateParty;
        public GGroup _Create;
        public GList _GList_Party;
        public GButton _Btn_QuitParty;
        public GGroup _InParty;
        public GGroup _Party;
        public const string URL = "ui://4916gthqhagdomv";

        public static FGUI_QuestLogMini CreateInstance()
        {
            return (FGUI_QuestLogMini)UIPackage.CreateObject("ms_Unity", "QuestLogMini");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_Page = GetControllerAt(0);
            _c_ShowOrHide = GetControllerAt(1);
            _c_PartyStatus = GetControllerAt(2);
            _GList_QuestInfo_in_progress = (GList)GetChildAt(5);
            _Quest = (GGroup)GetChildAt(6);
            _Btn_CreateParty = (GButton)GetChildAt(8);
            _Create = (GGroup)GetChildAt(11);
            _GList_Party = (GList)GetChildAt(12);
            _Btn_QuitParty = (GButton)GetChildAt(13);
            _InParty = (GGroup)GetChildAt(14);
            _Party = (GGroup)GetChildAt(15);
            OnCreate();

        }
    }
}