/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_QuestLog : GComponent
    {
        public Controller _c_BT_TAB;
        public Controller _c_Tab_Visible;
        public Controller _c_SetupAction;
        public GButton _Btn_Home;
        public GList _GList_QuestInfo_Available;
        public GList _GList_QuestInfo_in_progress;
        public GList _GList_QuestInfo_completed;
        public GTextField _Txt_Desc;
        public GButton _Btn_SetupSkill;
        public const string URL = "ui://4916gthqx5bhol5";

        public static FGUI_QuestLog CreateInstance()
        {
            return (FGUI_QuestLog)UIPackage.CreateObject("ms_Unity", "QuestLog");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_BT_TAB = GetControllerAt(0);
            _c_Tab_Visible = GetControllerAt(1);
            _c_SetupAction = GetControllerAt(2);
            _Btn_Home = (GButton)GetChildAt(7);
            _GList_QuestInfo_Available = (GList)GetChildAt(13);
            _GList_QuestInfo_in_progress = (GList)GetChildAt(14);
            _GList_QuestInfo_completed = (GList)GetChildAt(15);
            _Txt_Desc = (GTextField)GetChildAt(16);
            _Btn_SetupSkill = (GButton)GetChildAt(17);
            OnCreate();

        }
    }
}