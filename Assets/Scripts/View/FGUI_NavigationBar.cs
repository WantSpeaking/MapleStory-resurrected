/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
    public partial class FGUI_NavigationBar
    {
        void OnCreate()
		{
            _Btn_Home.onClick.Add (OnClick_Btn_Home);

        }
        private void OnClick_Btn_Home (EventContext context)
        {
            ms.UI.get ().get_element<ms.UIItemInventory> ().get ()?.deactivate ();

            ms.UI.get ().get_element<ms.UISkillBook> ().get ()?.deactivate ();
        }

    }
}