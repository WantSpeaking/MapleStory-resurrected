using ms;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;
using FairyGUI;

namespace ms_Unity
{

	[Category ("MobAction")]
	public class BTA_MobMoveToPlayer : ActionTask
	{
		public BBParameter<Vector3> axis;
		public BBParameter<float> moveSpeed = 1f;
		public BBParameter<float> distanceThreshold = 1f;
		public BBParameter<float> moveXDelta = 50f;

		private Mob mob;
		private Player player;
		private MobAttack mobAttack;
		private ms.Animation ball;
		private Bullet bullet;
		private ms.Animation attackAnimation;

		public double viewx => ms.Stage.get ().viewx;
		public double viewy => ms.Stage.get ().viewy;
		public float alpha => ms.Stage.get ().alpha;
		protected override string OnInit ()
		{
			mob = blackboard.GetVariable<Mob> (typeof (Mob).Name).value;
			mobAttack = mob.get_MobAttack (1);
			player = ms.Stage.get ().get_player ();
			return null;
		}


		protected override void OnExecute ()
		{

		}

		protected override void OnUpdate ()
		{
			if (player == null)
				EndAction (false);

			var distanceX = player.get_position ().x () - mob.get_position ().x ();
			var facingRight = distanceX > 0;

			Debug.Log (distanceX);
			if (Mathf.Abs (distanceX) > distanceThreshold.value)
			{
				mob.get_phobj ().hforce = facingRight ? moveSpeed.value : -moveSpeed.value;
			}
			else
			{
				mob.get_phobj ().hforce = 0;
				EndAction (true);
			}
		}
	}
}