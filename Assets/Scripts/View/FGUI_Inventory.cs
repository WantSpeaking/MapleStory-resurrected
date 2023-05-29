
using System;
using FairyGUI;
using FairyGUI.Utils;
using ms;

namespace ms_Unity
{
	public partial class FGUI_Inventory
	{
		public InventoryWindow MonoBehaviour_View;

		public void OnCreate ()
		{
		}

        public void OnVisiblityChanged(bool isVisible)
        {
            ms_Unity.FGUI_Manager.Instance.PanelOpening = isVisible;
			//_ItemInventory._StatsInfo.RefreshStatsUI();
        }
    }
}