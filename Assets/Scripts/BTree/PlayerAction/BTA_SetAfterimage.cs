using ms;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace ms_Unity
{

	[Category ("PlayerAction")]
	public class BTA_SetAfterimage : ActionTask
	{
		[AfterimgIdSelector]
		public Afterimage.Id AfterimageId = Afterimage.Id.none;

		[StanceIdSelector]
		public Stance.Id StanceId = Stance.Id.SWINGO1;

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
			if (AfterimageId == Afterimage.Id.none)
			{
				player.set_afterimage (move.get_id ());
			}
			else
			{
				//player.set_afterimage (move.get_id (), $"{AfterimageId}.img", StanceId.ToString ());
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