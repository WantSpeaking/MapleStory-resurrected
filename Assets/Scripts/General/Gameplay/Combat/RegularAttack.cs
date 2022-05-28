namespace ms
{
	public class RegularAttack : SpecialMove
	{
		public override void apply_useeffects ( Char user)
		{
		}

		public override void apply_actions ( Char user, Attack.Type type)
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

			if (attack.type == Attack.Type.CLOSE)
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

		public override SpecialMove.ForbidReason can_use (int level, Weapon.Type weapon, Job job, ushort hp, ushort mp, ushort bullets)
		{
			switch (weapon)
			{
				case Weapon.Type.BOW:
				case Weapon.Type.CROSSBOW:
				case Weapon.Type.CLAW:
				case Weapon.Type.GUN:
					return bullets != 0 ? ForbidReason.FBR_NONE : ForbidReason.FBR_BULLETCOST;
				default:
					return ForbidReason.FBR_NONE;
			}
		}

		private RegularAction action = new RegularAction ();
		private RegularBullet bullet = new RegularBullet ();
	}
}