/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System;
using FairyGUI;
using FairyGUI.Utils;
using ms;

namespace ms_Unity
{
    public partial class FGUI_SetupActionButtons
    {
        public void OnCreate()
		{
			_Btn_Skill1.Key = KeyConfig.Key.NUM1;
			_Btn_Skill2.Key = KeyConfig.Key.NUM2;
			_Btn_Skill3.Key = KeyConfig.Key.NUM3;
			_Btn_Skill4.Key = KeyConfig.Key.NUM4;
		}
		public void UpdateIcon ()
		{
			foreach (var child in this.GetChildren ())
			{
				if (child is FGUI_Btn_Joystick_Acton _ActonBtn)
				{
					_ActonBtn.UpdateIcon ();
				}
			}
		}
	}
}