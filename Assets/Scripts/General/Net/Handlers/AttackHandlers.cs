



namespace ms
{
	public class AttackHandler : PacketHandler
	{
		public override void handle(InPacket recv)
		{
			AttackResult attack = new AttackResult();
			
			int cid = recv.read_int();
			byte numAttackedAndDamage = (byte)recv.read_byte();
			recv.skip(1);
			attack.skilllevel = (byte)recv.read_byte();
			attack.skill = (attack.skilllevel > 0) ? recv.read_int() : 0;

			attack.display = (byte)recv.read_byte();
			attack.toleft = recv.read_bool();
			attack.stance = (byte)recv.read_byte();
			attack.speed = (byte)recv.read_byte();
			recv.skip(1);
			attack.bulletId = recv.read_int();

			attack.type = type;
			attack.attacker = cid;
			attack.mobcount = (byte)((numAttackedAndDamage >> 4) & 0xF);
			attack.hitcount = (byte)(numAttackedAndDamage & 0xF);

			if(attack.hitcount == 0) return;
			for (byte i = 0; i < attack.mobcount; i++)
			{
				int oid = recv.read_int();

				recv.skip(1);

				byte length = (attack.skill == (int)SkillId.Id.MESO_EXPLOSION) ? (byte)recv.read_byte() : attack.hitcount;

				for (byte j = 0; j < length; j++)
				{
					int damage = recv.read_int();
					bool critical = false; // TODO: ?
					var singledamage = System.Tuple.Create(damage, critical);
					attack.damagelines.TryAdd (oid);
					attack.damagelines[oid].Add(singledamage);
				}
			}

			var user = Stage.get ().get_character (cid).get ();
			AttackUser attackuser = new AttackUser (attack.skilllevel, user?.get_level ()??0, user?.is_twohanded ()??false, attack.toleft, user);

			AppDebug.Log ($"attack.damagelines.Count:{attack.damagelines.Count}\t toleft:{attack.toleft}");
			Stage.get().get_combat().push_attack(attack);
			//Stage.get().get_combat().push_damageEffect(attack,attackuser);
		}

		protected AttackHandler(Attack.Type t)
		{
			type = t;
		}

		private Attack.Type type;
	}

	public class CloseAttackHandler : AttackHandler
	{
		public CloseAttackHandler() : base(Attack.Type.Close_Range)
		{
		}
	}

	public class RangedAttackHandler : AttackHandler
	{
		public RangedAttackHandler() : base(Attack.Type.Ranged)
		{
		}
	}

	public class MagicAttackHandler : AttackHandler
	{
		public MagicAttackHandler() : base(Attack.Type.Magic)
		{
		}
	}
}

