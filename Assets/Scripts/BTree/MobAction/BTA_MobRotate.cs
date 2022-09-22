using ms;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;


namespace ms_Unity
{

	[Category ("MobAction")]
	public class BTA_MobRotate : ActionTask
	{
		public BBParameter<Vector3> axis; 
		public BBParameter<float> rotateSpeed = 1f;
		private Mob mob => blackboard.GetVariable<Mob> (typeof (Mob).Name).value;

		protected override void OnExecute ()
		{
			base.OnExecute ();
		}

		protected override void OnUpdate ()
		{
			for (int i = 0; i < mob.MapGameObject.transform.childCount; i++)
			{
				var child =	mob.MapGameObject.transform.GetChild (i);
				child.Rotate (axis.value, rotateSpeed.value, Space.Self);
			}
		}
	}
}