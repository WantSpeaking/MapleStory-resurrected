/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using ms;

namespace ms_Unity
{
	public partial class FGUI_YesNo
	{
		public string message;
		public NoticeType t;
		public Text.Alignment a;
		public int count;
		public int max;
		public System.Action<bool> yesnohandler;
		
		public void OnCreate ()
		{


		}

		public static void ShowNotice (string message, NoticeType t = NoticeType.ENTERNUMBER, Text.Alignment a = Text.Alignment.CENTER, int max = 0, int count = 0, System.Action<bool> yesnohandler = null)
		{
			var thisNotice = ms_Unity.FGUI_Manager.Instance.OpenFGUI<FGUI_YesNo> () as FGUI_YesNo;

			thisNotice.message = message;
			thisNotice.t = t;
			thisNotice.a = a;
			thisNotice.max = max;
			thisNotice.count = count;
			thisNotice.yesnohandler = yesnohandler;

			thisNotice._c_NoticeType.selectedIndex = (int)t;
			thisNotice.Center ();

		}
	}
}