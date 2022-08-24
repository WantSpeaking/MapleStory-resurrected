/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System;
using FairyGUI;
using FairyGUI.Utils;
using ms;

namespace ms_Unity
{
	public partial class FGUI_Notice
	{
		string message;

		public void OnCreate ()
		{
			
		}

		public static void ShowNotice (string message)
		{
			var thisNotice = ms_Unity.FGUI_Manager.Instance.OpenFGUI<FGUI_Notice> () as FGUI_Notice;

			thisNotice.message = message;
			thisNotice.Center ();
			thisNotice._t_Show.Play ();
		}
	}
}