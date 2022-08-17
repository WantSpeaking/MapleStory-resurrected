/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_ItemInventory : GComponent
    {
        public Controller _c_InventoryTypeId;
        public GList _GList_Equip;
        public GList _GList_Use;
        public GList _GList_Setup;
        public GList _GList_Etc;
        public GList _GList_Cash;
        public GButton _Btn_Meso;
        public GButton _Btn_MaplePoints;
        public GButton _Btn_Gather;
        public GGroup _Bot;
        public GList _GList_Equipped;
        public GLoader _GLoader_Player;
        public FGUI_Itemed_ItemDetail _Itemed_ItemDetail;
        public FGUI_StatsInfo _StatsInfo;
        public FGUI_SetupUseButtons _SetupUseButtons;
        public const string URL = "ui://4916gthqsqkc38";

        public static FGUI_ItemInventory CreateInstance()
        {
            return (FGUI_ItemInventory)UIPackage.CreateObject("ms_Unity", "ItemInventory");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_InventoryTypeId = GetControllerAt(0);
            _GList_Equip = (GList)GetChildAt(9);
            _GList_Use = (GList)GetChildAt(10);
            _GList_Setup = (GList)GetChildAt(11);
            _GList_Etc = (GList)GetChildAt(12);
            _GList_Cash = (GList)GetChildAt(13);
            _Btn_Meso = (GButton)GetChildAt(16);
            _Btn_MaplePoints = (GButton)GetChildAt(17);
            _Btn_Gather = (GButton)GetChildAt(18);
            _Bot = (GGroup)GetChildAt(19);
            _GList_Equipped = (GList)GetChildAt(20);
            _GLoader_Player = (GLoader)GetChildAt(21);
            _Itemed_ItemDetail = (FGUI_Itemed_ItemDetail)GetChildAt(22);
            _StatsInfo = (FGUI_StatsInfo)GetChildAt(23);
            _SetupUseButtons = (FGUI_SetupUseButtons)GetChildAt(24);
            OnCreate();

        }
    }
}