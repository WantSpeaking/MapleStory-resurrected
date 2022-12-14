using ms;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace ms_Unity
{

	[Category("PlayerAction")]
	public class BTA_SetAttackInfo : ActionTask
	{
		public BBParameter<byte> mobcount = 1;
		public BBParameter<byte> hitcount = 1;
		public BBParameter<float> hforce = 0;
		public BBParameter<float> vforce = 0;
		private Player player => Stage.get ().get_player ();

		private Attack attack => blackboard.GetVariable<Attack> (typeof (Attack).Name).value;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit()
		{
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute()
		{
			//move.apply_stats (player, attack);

			attack.damagetype = Attack.DamageType.DMG_WEAPON;
			attack.skill = 0;
			attack.mobcount = mobcount.value;
			attack.hitcount = hitcount.value;
			attack.stance = (byte)player.get_look ().get_stance ();
			attack.hforce = hforce.value;
			attack.vforce = vforce.value;

			if (attack.type == Attack.Type.Close_Range)
			{
				attack.range = player.get_afterimage ().get_range ();
			}

			EndAction (true);
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate()
		{
			
		}

		//Called when the task is disabled.
		protected override void OnStop()
		{
			
		}

		//Called when the task is paused.
		protected override void OnPause()
		{
			
		}
	}
}