/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_QuestLogMini : GComponent
    {
        public Controller _c_QuestState;
        public Controller _c_SetupAction;
        public GList _GList_QuestInfo_in_progress;
        public GGroup _In_progress;
        public const string URL = "ui://4916gthqhagdomv";

        public static FGUI_QuestLogMini CreateInstance()
        {
            return (FGUI_QuestLogMini)UIPackage.CreateObject("ms_Unity", "QuestLogMini");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_QuestState = GetControllerAt(0);
            _c_SetupAction = GetControllerAt(1);
            _GList_QuestInfo_in_progress = (GList)GetChildAt(0);
            _In_progress = (GGroup)GetChildAt(1);
            OnCreate();

        }
    }
}