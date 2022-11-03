/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using ms;

namespace ms_Unity
{
	public partial class FGUI_OK
	{
		public string message;
		public NoticeType t;
		public Text.Alignment a;
		public int count;
		public int max;
		public System.Action<bool> okhandler;

		public void OnCreate ()
		{
			this._Btn_Yes.onClick.Add (deactivate);
		}
		private void deactivate ()
		{
			okhandler?.Invoke (true);
			ms_Unity.FGUI_Manager.Instance.CloseFGUI<FGUI_OK> ();
			GRoot.inst.HidePopup (this);
		}
		public static void ShowNotice (string message, System.Action<bool> okhandler = null)
		{
			var thisNotice = ms_Unity.FGUI_Manager.Instance.OpenFGUI<FGUI_OK> () as FGUI_OK;

			thisNotice.message = message;
			thisNotice.okhandler = okhandler;

			thisNotice._tet_message.text = message;

			thisNotice.Center ();
		}
	}
}