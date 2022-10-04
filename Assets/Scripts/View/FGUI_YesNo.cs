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

		public static void ShowNotice (string message, System.Action<bool> yh = null, Text.Alignment alignment = Text.Alignment.CENTER)
		{
			var thisNotice = ms_Unity.FGUI_Manager.Instance.OpenFGUI<FGUI_YesNo> () as FGUI_YesNo;

			thisNotice.message = message;
			thisNotice.a = alignment;
			thisNotice.yesnohandler = yh;

			thisNotice._tet_message.text = message;

			thisNotice.Center ();

		}
	}
}