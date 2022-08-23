/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_QuestLog : GComponent
    {
        public Controller _c_QuestState;
        public Controller _c_SetupAction;
        public FGUI_NavigationBar _NavigationBar;
        public GTextField _Txt_Desc;
        public GList _GList_QuestInfo_Available;
        public GList _GList_QuestInfo_in_progress;
        public GButton _Btn_ForfeitQuest;
        public GGroup _In_progress;
        public GList _GList_QuestInfo_completed;
        public const string URL = "ui://4916gthqx5bhol5";

        public static FGUI_QuestLog CreateInstance()
        {
            return (FGUI_QuestLog)UIPackage.CreateObject("ms_Unity", "QuestLog");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_QuestState = GetControllerAt(0);
            _c_SetupAction = GetControllerAt(1);
            _NavigationBar = (FGUI_NavigationBar)GetChildAt(1);
            _Txt_Desc = (GTextField)GetChildAt(7);
            _GList_QuestInfo_Available = (GList)GetChildAt(8);
            _GList_QuestInfo_in_progress = (GList)GetChildAt(9);
            _Btn_ForfeitQuest = (GButton)GetChildAt(10);
            _In_progress = (GGroup)GetChildAt(11);
            _GList_QuestInfo_completed = (GList)GetChildAt(12);
            OnCreate();

        }
    }
}