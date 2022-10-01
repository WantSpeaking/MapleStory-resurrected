using ms;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;
using FairyGUI;

namespace ms_Unity
{

	[Category ("MobAction")]
	public class BTA_MobJumpToPlayer : ActionTask
	{
		public BBParameter<float> jumpXDelta = 50f;
		public BBParameter<int> moveXTime = 200;
		public BBParameter<bool> isJump = true;

		private Mob mob;
		private Player player;
		private MobAttack mobAttack;
		private ms.Animation attackAnimation;

		public double viewx => ms.Stage.get ().viewx;
		public double viewy => ms.Stage.get ().viewy;
		public float alpha => ms.Stage.get ().alpha;
		protected override string OnInit ()
		{
			mob = blackboard.GetVariable<Mob> (typeof (Mob).Name).value;
			mobAttack = mob.get_MobAttack (1);
			attackAnimation = mobAttack.mobAttackAni;

			player = ms.Stage.get ().get_player ();
			return null;
		}

		private bool hasLanded = false;

		protected override void OnExecute ()
		{
			mob.Set_MobAttackAni (attackAnimation);

			GTween.To (0, 1, moveXTime.value / 1000f).OnComplete (t =>
			{
				mob.Set_MobAttackAni (null);
				EndAction (true);
			});

			var facingRight = (player.get_position ().x () - mob.get_position ().x ()) > 0;
			mob.set_flip (facingRight);

			//mob.GetPhysicsObject ().hforce = facingRight ? horizontalSpeed.value : -horizontalSpeed.value;
			//mob.GetPhysicsObject ().vforce = -verticalSpeed.value;
			//mob.get_phobj ().clear_flag (PhysicsObject.Flag.TURNATEDGES);
			mob.get_phobj ().vforce = isJump.value ? -moveXTime.value / 2 / Constants.TIMESTEP * ms.Physics.GRAVFORCE : 0;//一半时间 从vforce 变为 0
			mob.get_phobj ().movexuntil (player.get_position ().x () + (jumpXDelta.value * player.get_phobj ().hspeed / player.get_walkforce ()), (ushort)(moveXTime.value));
			//AppDebug.Log (player.get_phobj ().hspeed);
			//EndAction (true);
		}

		protected override void OnUpdate ()
		{

		}
	}
}