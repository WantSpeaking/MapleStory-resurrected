using ms;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;
using FairyGUI;

namespace ms_Unity
{

	[Category ("MobAction")]
	public class BTA_MobThrowBall : ActionTask
	{
		public BBParameter<float> duration = 1f;

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
			mobAttack = mob.get_MobAttack (2);
			ball = mobAttack.ball;
			attackAnimation = mobAttack.mobAttackAni;

			player = ms.Stage.get ().get_player ();
			bullet = new Bullet (ball);
			return null;
		}


		protected override void OnExecute ()
		{
			bullet.settarget (player.get_position ());
			var facingLeft = (player.get_position ().x () - mob.get_position ().x ()) < 0;
			bullet.set_Ray (mob.get_head_position (), facingLeft);

			mob.set_flip (!facingLeft);
			mob.Set_MobAttackAni (attackAnimation);

			GTween.To (mob.get_head_position ().ToUnityVector2 (), player.get_position ().ToUnityVector2 (), duration.value)
				.SetRepeat (1, true)
				.SetSnapping (true)
				.OnStart (t => { })
				.OnUpdate (t => { ball.draw (new Point_short ((short)(t.value.x + viewx), (short)(t.value.y + viewy)), alpha); })
				.OnComplete (t =>
				{
					mob.Set_MobAttackAni (null);
					EndAction (true);
				});
		}

		void OnTweerUpdate (GTweener t)
		{

		}

		protected override void OnUpdate ()
		{
			/*if (bullet.update (player.get_position ()))
			{
				mob.Set_MobAttackAni (null);
				EndAction (true);
			}
			else
			{
				bullet.draw (viewx, viewy, alpha);
			}*/
		}
	}
}