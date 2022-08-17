
using System;
using FairyGUI;
using FairyGUI.Utils;

namespace ms_Unity
{
	public partial class FGUI_Inventory
	{
		public InventoryWindow MonoBehaviour_View;

		public void OnCreate ()
		{
			/* MonoBehaviour_View = this.displayObject.gameObject.AddComponent<InventoryWindow> ();
             MonoBehaviour_View.FGUI_View = this;
             return this;*/
			//_GLoader_Player.texture = new NTexture (Launcher.Instance.playerRenderTexture);
			_Btn_Home.onClick.Add (OnClick_Btn_Home);
		}

		private void OnClick_Btn_Home (EventContext context)
		{
			ms.UI.get ().emplace<ms.UIItemInventory> ();
		}
	}
}