using ms;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace ms_Unity
{

	[Category("PlayerAction")]
	public class BTA_ApplyUseEffect : ActionTask
	{
		private Player player => Stage.get ().get_player ();
		private SpecialMove move => blackboard.GetVariable<SpecialMove> (typeof (SpecialMove).Name).value;

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
			move.apply_useeffects (player);

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