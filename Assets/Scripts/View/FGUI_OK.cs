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


		}

		public static void ShowNotice (string message, NoticeType t = NoticeType.OK, Text.Alignment a = Text.Alignment.CENTER, int max = 0, int count = 0, System.Action<bool> okhandler = null)
		{
			var thisNotice = ms_Unity.FGUI_Manager.Instance.OpenFGUI<FGUI_OK> () as FGUI_OK;

			thisNotice.message = message;
			thisNotice.t = t;
			thisNotice.a = a;
			thisNotice.max = max;
			thisNotice.count = count;
			thisNotice.okhandler = okhandler;

			thisNotice._c_NoticeType.selectedIndex = (int)t;
			thisNotice.Center ();

		}
	}
}