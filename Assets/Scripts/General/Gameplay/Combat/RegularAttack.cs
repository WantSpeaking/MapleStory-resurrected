using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;

namespace ms
{
	public class RegularAttack : SpecialMove
	{
		public override void apply_useeffects (Char user)
		{
		}
		public override void apply_prepareEffect (Char user)
		{
		}
		public override void apply_keydownEffect (Char user)
		{
		}
		public override void apply_keydownendEffect (Char user)
		{
		}
		public override void apply_actions (Char user, Attack.Type type)
		{
			action.apply (ref user, type);
		}

		public override void apply_stats (Char user, Attack attack)
		{
			attack.damagetype = Attack.DamageType.DMG_WEAPON;
			attack.skill = 0;
			attack.mobcount = 1;
			attack.hitcount = 1;
			attack.stance = (byte)user.get_look ().get_stance ();

			if (attack.type == Attack.Type.Close_Range)
			{
				attack.range = user.get_afterimage ().get_range ();
			}
		}

		public override void apply_hiteffects (AttackUser user, Mob target)
		{
		}

		public override Animation get_bullet (Char user, int bulletid)
		{
			return bullet.get (user, bulletid);
		}

		public override bool is_attack ()
		{
			return true;
		}

		public override bool is_skill ()
		{
			return false;
		}

		public override int get_id ()
		{
			return 0;
		}

		public override bool has_skillPrepareEffect ()
		{
			return false;
		}
		public override SpecialMove.ForbidReason can_use (int level, Weapon.Type weapon, Job job, ushort hp, ushort mp, ushort bullets)
		{
			switch (weapon)
			{
				case Weapon.Type.BOW:
				case Weapon.Type.CROSSBOW:
					if (ms.Stage.get ().get_player ().can_useBow_withoutArrows () || (bullets != 0))
					{
						return ForbidReason.FBR_NONE;
					}
					else
					{
						return ForbidReason.FBR_BULLETCOST;
					}
				case Weapon.Type.CLAW:
				case Weapon.Type.GUN:
					return bullets != 0 ? ForbidReason.FBR_NONE : ForbidReason.FBR_BULLETCOST;
				default:
					return ForbidReason.FBR_NONE;
			}
		}

		public override SkillAction get_action (Char user)
		{
			return action;
		}
		private RegularAction action = new RegularAction ();
		private RegularBullet bullet = new RegularBullet ();

		public override BehaviourTree BTree
		{
			get
			{
				switch (ms.Stage.get ().get_player ().get_job ().get_MapleJob ())
				{
					case MapleJob.DAWNWARRIOR1:
					case MapleJob.DAWNWARRIOR2:
					case MapleJob.DAWNWARRIOR3:
					case MapleJob.DAWNWARRIOR4:
						return ResourcesManager.Instance.GetSkillBTree (((int)MapleJob.DAWNWARRIOR1).ToString());
					case MapleJob.IL_WIZARD:
					case MapleJob.IL_MAGE:
					case MapleJob.IL_ARCHMAGE:
						return ResourcesManager.Instance.GetSkillBTree (((int)MapleJob.IL_WIZARD).ToString ());
				}

				return base.BTree;
			}
		}
	}
}