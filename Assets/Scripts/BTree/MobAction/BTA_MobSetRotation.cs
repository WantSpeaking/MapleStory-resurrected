using ms;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;


namespace ms_Unity
{

	[Category ("MobAction")]
	public class BTA_MobSetRotation : ActionTask
	{
		public BBParameter<Vector3> initialAngle;

		private Mob mob => blackboard.GetVariable<Mob> (typeof (Mob).Name).value;

		protected override void OnExecute ()
		{
			
		}

		protected override void OnUpdate ()
		{
			for (int i = 0; i < mob.MapGameObject.transform.childCount; i++)
			{
				var child = mob.MapGameObject.transform.GetChild (i);
				child.localRotation = Quaternion.Euler (initialAngle.value.x, initialAngle.value.y, initialAngle.value.z);
			}

			EndAction (true);
		}
	}
}