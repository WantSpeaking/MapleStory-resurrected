/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_EquipInventory : GComponent
    {
        public Controller _c_BackpackTab;
        public GList _GList_Equip;
        public GList _GList_Cash;
        public GList _GList_Pet;
        public GList _GList_Android;
        public FGUI_Itemed_ItemDetail _EquipmentDetail;
        public const string URL = "ui://4916gthqc5k6np8";

        public static FGUI_EquipInventory CreateInstance()
        {
            return (FGUI_EquipInventory)UIPackage.CreateObject("ms_Unity", "EquipInventory");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _c_BackpackTab = GetControllerAt(0);
            _GList_Equip = (GList)GetChildAt(4);
            _GList_Cash = (GList)GetChildAt(5);
            _GList_Pet = (GList)GetChildAt(6);
            _GList_Android = (GList)GetChildAt(7);
            _EquipmentDetail = (FGUI_Itemed_ItemDetail)GetChildAt(8);
            OnCreate();

        }
    }
}