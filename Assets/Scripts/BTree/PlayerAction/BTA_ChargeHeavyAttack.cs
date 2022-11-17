using ms;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using FairyGUI;

namespace ms_Unity
{

	[Category ("PlayerAction")]
	public class BTA_ChargeHeavyAttack : PlayerAction_Base
	{

		public BBParameter<float> chargeSpeed = 0.5f;
		public BBParameter<double> chargeProgress = 0;

		public GProgressBar progressBar_Quarter => FGUI_Manager.Instance.fgui_StateGame._ProgressBar_Quarter;

		protected override void OnExecute ()
		{
			progressBar_Quarter.visible = true;
			progressBar_Quarter.value = 0;
		}

		protected override void OnStop ()
		{
			progressBar_Quarter.visible = false;
			progressBar_Quarter.value = 0;
		}

		protected override void OnUpdate ()
		{
			chargeProgress.value += chargeSpeed.value;

			progressBar_Quarter.value = chargeProgress.value;

			var screenPos = UnityEngine.Camera.main.WorldToScreenPoint (new UnityEngine.Vector3 (player.absp.x (), player.absp.y () + 20, 1));
			screenPos.y = screenPos.y - UnityEngine.Screen.height;
			progressBar_Quarter.position = GRoot.inst.GlobalToLocal (screenPos);
			//EndAction (true);
		}

		protected override string info => $"chargeProgress:{chargeProgress.value:f0}";
	}
}