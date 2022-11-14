/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using ms;

namespace ms_Unity
{
	public partial class FGUI_Btn_Joystick_Acton
	{
		public KeyConfig.Key Key { get; set; } = KeyConfig.Key.NONE;
		public int SkillId { get; set; }
		//public keymapping

		public void OnCreate ()
		{

		}


		public void UpdateIcon ()
		{
			if (Key == KeyConfig.Key.NONE)
				return;

			var ref_UIKeyConfig = UI.get ().get_element<UIKeyConfig> ();
			if (ref_UIKeyConfig)
			{
				var icon = ref_UIKeyConfig.get ().GetStagedIcon ((int)Key);
				_GLoader_Icon.texture = ref_UIKeyConfig.get ().GetStagedIcon ((int)Key)?.nTexture;
				text = icon?.showcount ?? false ? icon.get_count ().ToString () : "";
			}
		}
	}
}