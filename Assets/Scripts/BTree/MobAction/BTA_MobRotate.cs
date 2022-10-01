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
		public BBParameter<Vector3> initialAngle;
		public BBParameter<float> rotateSpeed = 1f;
		public BBParameter<float> rotateTime = 99f;

		private Mob mob => blackboard.GetVariable<Mob> (typeof (Mob).Name).value;

		protected override void OnExecute ()
		{
			timer = 0;

			for (int i = 0; i < mob.MapGameObject.transform.childCount; i++)
			{
				var child = mob.MapGameObject.transform.GetChild (i);
				child.localRotation = Quaternion.Euler (initialAngle.value.x, initialAngle.value.y, initialAngle.value.z);
			}
		}

		float timer = 0;
		protected override void OnUpdate ()
		{
			for (int i = 0; i < mob.MapGameObject.transform.childCount; i++)
			{
				var child = mob.MapGameObject.transform.GetChild (i);
				child.Rotate (axis.value, rotateSpeed.value, Space.Self);
			}

			timer += Time.deltaTime;

			if (timer > rotateTime.value)
			{
				EndAction (true);
			}
		}
	}
}