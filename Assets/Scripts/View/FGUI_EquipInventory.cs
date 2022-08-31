/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using ms;

namespace ms_Unity
{
	public partial class FGUI_EquipInventory
	{
		Inventory inventory;
		void OnCreate ()
		{
			inventory = ms.Stage.get ()?.get_player ()?.get_inventory ();
			if (inventory != null)
			{

			}
		}
	}
}