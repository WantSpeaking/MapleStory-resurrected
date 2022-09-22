using ms;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace ms_Unity
{

	[Category ("PlayerAction")]
	public class BTA_PlayerMove : ActionTask
	{
		public BBParameter<double> offectX = 100;
		public BBParameter<double> offectY;
		public BBParameter<int> duration = 1;
		private SpecialMove move => blackboard.GetVariable<SpecialMove> (typeof (SpecialMove).Name).value;

		private Player player => Stage.get ().get_player ();

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit ()
		{
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute ()
		{
			var pos = player.get_position ();
			var phobj = player.get_phobj ();
			if (phobj.onground)
			{
				phobj.moveuntil (pos.x () + (player.facing_right ? offectX.value : -offectX.value), pos.y () + offectY.value, (ushort)duration.value);
				phobj.set_flag (PhysicsObject.Flag.TURNATEDGES);//turn at edges
			}

			EndAction (true);
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate ()
		{

		}

		//Called when the task is disabled.
		protected override void OnStop ()
		{

		}

		//Called when the task is paused.
		protected override void OnPause ()
		{

		}
	}
}