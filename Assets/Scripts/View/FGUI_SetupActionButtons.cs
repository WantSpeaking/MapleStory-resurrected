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
            _Btn_Skill5.Key = KeyConfig.Key.NUM5;
            _Btn_Skill6.Key = KeyConfig.Key.NUM6;
            _Btn_Skill7.Key = KeyConfig.Key.NUM7;
            _Btn_Skill8.Key = KeyConfig.Key.NUM8;
            _Btn_Skill9.Key = KeyConfig.Key.NUM9;
            _Btn_Skill10.Key = KeyConfig.Key.NUM0;
            _Btn_Skill11.Key = KeyConfig.Key.MINUS;
            _Btn_Skill12.Key = KeyConfig.Key.EQUAL;
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