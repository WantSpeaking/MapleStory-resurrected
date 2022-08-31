/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System;
using FairyGUI;
using FairyGUI.Utils;
using ms;

namespace ms_Unity
{
	public partial class FGUI_SetupUseButtons
	{
		KeyConfig.Key[] keys = new KeyConfig.Key[]
		{
			KeyConfig.Key.NUM5, KeyConfig.Key.NUM6, KeyConfig.Key.NUM7, KeyConfig.Key.NUM8,
			KeyConfig.Key.NUM9, KeyConfig.Key.NUM0, KeyConfig.Key.MINUS, KeyConfig.Key.EQUAL,
			KeyConfig.Key.X, KeyConfig.Key.C, KeyConfig.Key.V, KeyConfig.Key.B
		};

		void OnCreate ()
		{
			for (int i = 0; i < keys.Length; i++)
			{
				var key = keys[i];
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