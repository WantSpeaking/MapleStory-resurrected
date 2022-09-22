using ms;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace ms_Unity
{

	[Category ("PlayerAction")]
	public class BTA_Attack : PlayerAction_Base
	{

		public Stance.Id stance = Stance.Id.STABO1;
		public BBParameter<int> skillSpeedMultiplier = 2;
		public BBParameter<int> hitCount = 2;

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute ()
		{
			if (player == null)
				return;
			player.Set_SkillSpeedMultiplier (skillSpeedMultiplier.GetValue());
			player.attack (stance, OnAttackEnd);
		}

		private void OnAttackEnd (Char c)
		{
			c.Set_SkillSpeedMultiplier (1);
			EndAction (true);
		}
	}
}