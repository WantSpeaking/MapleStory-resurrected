using ms;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;


namespace ms_Unity
{

	[Category ("MobAction")]
	public class BTA_MobNextMove : ActionTask
	{
		private Mob mob => blackboard.GetVariable<Mob> (typeof (Mob).Name).value;

		protected override void OnExecute ()
		{
			base.OnExecute ();
		}

		protected override void OnUpdate ()
		{
			base.OnUpdate ();
		}
	}
}