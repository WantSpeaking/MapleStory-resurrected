using ms;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using FairyGUI;
using System.Collections.Generic;

namespace ms_Unity
{

	[Category ("PlayerAction")]
	public class BTA_EnterChargePhase : PlayerAction_Base
	{

		public BBParameter<Dictionary<string, string>> Phase_SkillId_Name_Dict;

		public GProgressBar progressBar_Quarter => FGUI_Manager.Instance.fgui_StateGame._ProgressBar_Quarter;
		private FGUI_Joystick _Joystick => ms_Unity.FGUI_Manager.Instance.GetFGUI<ms_Unity.FGUI_Joystick> ();

		protected override void OnExecute ()
		{
			_Joystick.EnterChargePhase (Phase_SkillId_Name_Dict.value);

			EndAction (true);
		}

		protected override string info
		{
			get
			{
				if (Phase_SkillId_Name_Dict != null)
				{
					return "—°‘Ò Õ∑≈£∫" + Phase_SkillId_Name_Dict.value?.Values.ToDebugLog ();
				}
				return "Dict is null";
			}
		}
	}
}