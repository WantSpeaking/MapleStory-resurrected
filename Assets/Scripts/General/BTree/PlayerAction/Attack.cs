using ms;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace ms_Unity
{

	[Category ("PlayerAction")]
	public class Attack : PlayerAction_Base
	{

		public Stance.Id stance = Stance.Id.STABO1;


		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute ()
		{
			if (player == null)
				return;
			player.attack (stance, OnAttackEnd);
		}

		private void OnAttackEnd ()
		{
			EndAction (true);
		}
	}
}