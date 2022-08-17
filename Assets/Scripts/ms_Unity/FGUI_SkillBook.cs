/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_SkillBook : GComponent
    {
        public Controller _c_BT_TAB;
        public Controller _c_Tab_Visible;
        public Controller _c_SetupAction;
        public GButton _Btn_Home;
        public GList _GList_SkillInfo;
        public GTextField _Txt_RemainSP;
        public GTextField _Txt_Desc;
        public GButton _Btn_SetupSkill;
        public FGUI_SetupActionButtons _SetupActionButtons;
        public const string URL = "ui://4916gthqsxcsokt";

        public static FGUI_SkillBook CreateInstance()
        {
            return (FGUI_SkillBook)UIPackage.CreateObject("ms_Unity", "SkillBook");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_BT_TAB = GetControllerAt(0);
            _c_Tab_Visible = GetControllerAt(1);
            _c_SetupAction = GetControllerAt(2);
            _Btn_Home = (GButton)GetChildAt(7);
            _GList_SkillInfo = (GList)GetChildAt(15);
            _Txt_RemainSP = (GTextField)GetChildAt(16);
            _Txt_Desc = (GTextField)GetChildAt(17);
            _Btn_SetupSkill = (GButton)GetChildAt(18);
            _SetupActionButtons = (FGUI_SetupActionButtons)GetChildAt(19);
            OnCreate();

        }
    }
}