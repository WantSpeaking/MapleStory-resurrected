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
        public FGUI_NavigationBar _NavigationBar;
        public GButton _BT_TAB0;
        public GButton _BT_TAB1;
        public GButton _BT_TAB2;
        public GButton _BT_TAB3;
        public GButton _BT_TAB4;
        public GTextField _Txt_RemainSP;
        public GTextField _Txt_Desc;
        public GButton _Btn_SetupSkill;
        public FGUI_SetupActionButtons _SetupActionButtons;
        public GList _GList_SkillInfo;
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
            _NavigationBar = (FGUI_NavigationBar)GetChildAt(1);
            _BT_TAB0 = (GButton)GetChildAt(3);
            _BT_TAB1 = (GButton)GetChildAt(4);
            _BT_TAB2 = (GButton)GetChildAt(5);
            _BT_TAB3 = (GButton)GetChildAt(6);
            _BT_TAB4 = (GButton)GetChildAt(7);
            _Txt_RemainSP = (GTextField)GetChildAt(9);
            _Txt_Desc = (GTextField)GetChildAt(10);
            _Btn_SetupSkill = (GButton)GetChildAt(11);
            _SetupActionButtons = (FGUI_SetupActionButtons)GetChildAt(12);
            _GList_SkillInfo = (GList)GetChildAt(13);
            OnCreate();

        }
    }
}