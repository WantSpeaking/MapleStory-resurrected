/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System;
using FairyGUI;
using FairyGUI.Utils;
using ms;

namespace ms_Unity
{
	public partial class FGUI_SetupUseButtons
	{

		void OnCreate ()
		{
			for (int i = 0; i < ms.Constants.get().UseBtnKeys.Length; i++)
			{
				var key = ms.Constants.get().UseBtnKeys[i];
				if (i < _GList_UseBtns.numChildren)
				{
					if (_GList_UseBtns.GetChildAt (i) is FGUI_Btn_Joystick_Acton _Btn_Joystick_Acton)
					{
						_Btn_Joystick_Acton.Key = key;
					}
				}
			}

		}

		public void UpdateIcon ()
		{
			foreach (var child in _GList_UseBtns.GetChildren ())
			{
				if (child is FGUI_Btn_Joystick_Acton _ActonBtn)
				{
					_ActonBtn.UpdateIcon ();
				}
			}
		}

	}
}