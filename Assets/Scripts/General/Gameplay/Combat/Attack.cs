using System.Collections.Generic;
using MapleLib.WzLib;
using provider;
using UnityEngine;

namespace ms
{
	public class Attack
	{
		public enum Type
		{
			Close_Range,
			Ranged,
			Magic
		}

		public enum DamageType
		{
			DMG_WEAPON,
			DMG_MAGIC,
			DMG_FIXED
		}

		public Type type = Type.Close_Range;
		public DamageType damagetype = DamageType.DMG_WEAPON;

		public double mindamage = 1.0;
		public double maxdamage = 1.0;
		public float critical = 0.0f;
		public float ignoredef = 0.0f;
		public int matk = 0;
		public int accuracy = 0;
		public int fixdamage = 0;
		public short playerlevel = 1;

		public byte hitcount = 0;
		public byte mobcount = 0;
		public byte speed = 0;
		public byte stance = 0;
		public int skill = 0;
		public int bullet = 0;

		public Point_short origin = new Point_short ();
		public Rectangle_short range = new Rectangle_short ();
		public float hrange = 1.0f;
		public bool toleft = false;

		public float hforce;
		public float vforce;
		
		public int x = 0;
		public int y = 0;
	}

	public class MobAttack
	{
		public Attack.Type type = Attack.Type.Close_Range;
		public int PaDamage = 0;
		public int matk = 0;
		public int mobid = 0;
		public int oid = 0;
		public Point_short origin = new Point_short ();
		public bool valid = false;

		public bool hasBall;
		public bool hasEffect;

		public Rectangle_short range = new Rectangle_short ();
		public Animation effect = new Animation ();
		public Animation hit = new Animation ();
		public Animation ball = new Animation ();
		public Animation mobAttackAni = new Animation ();

		public int conMP = 0;
		public int effectAfter = 0;
		public int attackAfter = 0;
		public int magic = 0;
		public int deadlyAttack = 0;
		public int doFirst = 0;

		// Create a mob attack for touch damage
		public MobAttack ()
		{
			this.valid = false;
		}

		public MobAttack (int PADamage, Point_short origin, int mobid, int oid)
		{
			this.type = Attack.Type.Close_Range;
			this.PaDamage = PADamage;
			this.origin = origin;
			this.mobid = mobid;
			this.oid = oid;
			this.valid = true;
		}

		public MobAttack (WzObject src, int mobid, int oid)
		{
			mobAttackAni = src;

			var info = src["info"];
			if (info?["range"]?["r"])
			{
				short r = src["info"]["range"]["r"];
				range = new ms.Rectangle_short (new Point_short ((short)-r, (short)-r), new Point_short (r, r));
			}
			else
			{
				range = new ms.Rectangle_short (src["info"]["range"]);
			}

			hasBall = info?["ball"] != null;
			hasEffect = info?["effect"] != null;

			effect = info?["effect"];
			hit = info?["hit"];
			ball = info?["ball"];

			conMP = info?["conMP"];
			effectAfter = info?["effectAfter"];
			attackAfter = info?["attackAfter"];
			magic = info?["magic"];
			deadlyAttack = info?["deadlyAttack"];
			doFirst = info?["doFirst"];
			PaDamage = info?["PaDamage"];

			type = (Attack.Type)(int)(info?["type"] ?? 0);

			this.mobid = mobid;
			this.oid = oid;
			valid = true;
		}
        public MobAttack(MapleData src, int mobid, int oid)
        {
            mobAttackAni = src;

            var info = src["info"];
            if (info?["range"]?["r"])
            {
                short r = src["info"]["range"]["r"];
                range = new ms.Rectangle_short(new Point_short((short)-r, (short)-r), new Point_short(r, r));
            }
            else
            {
                range = new ms.Rectangle_short(src["info"]["range"]);
            }

            hasBall = info?["ball"] != null;
            hasEffect = info?["effect"] != null;

            effect = info?["effect"];
            hit = info?["hit"];
            ball = info?["ball"];

            conMP = info?["conMP"];
            effectAfter = info?["effectAfter"];
            attackAfter = info?["attackAfter"];
            magic = info?["magic"];
            deadlyAttack = info?["deadlyAttack"];
            doFirst = info?["doFirst"];
            PaDamage = info?["PaDamage"];

            type = (Attack.Type)(int)(info?["type"] ?? 0);

            this.mobid = mobid;
            this.oid = oid;
            valid = true;
        }
        public Rectangle_short get_range()
		{
			return new Rectangle_short(range);
		}

		public static implicit operator bool (MobAttack ImpliedObject)
		{
			return ImpliedObject.valid;
		}
	}

	public class MobAttackResult
	{
		public int damage;
		public int mobid;
		public int oid;
		public byte direction;

		public MobAttackResult (MobAttack attack, int damage, byte direction)
		{
			this.damage = damage;
			this.direction = direction;
			this.mobid = attack.mobid;
			this.oid = attack.oid;
		}
	}

	public class AttackResult
	{
		public AttackResult ()
		{
		}

		public AttackResult (Attack attack)
		{
			type = attack.type;
			hitcount = (byte)Mathf.Clamp (attack.hitcount, 0, 15);//server clamp numdamage 
			skill = attack.skill;
			speed = attack.speed;
			stance = attack.stance;
			bulletId = attack.bullet;
			toleft = attack.toleft;
			hforce = attack.hforce;
			vforce = attack.vforce;
            range = attack.range;

        }

		public Attack.Type type;
		public int attacker = 0;
		/// <summary>
		/// 攻击到的怪物数量
		/// </summary>
		public byte mobcount = 0;
		/// <summary>
		/// 最大16 server clamp numdamage 
		/// </summary>
		public byte hitcount = 1;
		public int skill = 0;
		public int charge = 0;
		public int bulletId = 0;
		public byte skilllevel = 0;
		public byte display = 0;
		public byte stance = 0;
		public byte speed = 0;
		public bool toleft = false;
		public Dictionary<int, List<System.Tuple<int, bool>>> damagelines = new Dictionary<int, List<System.Tuple<int, bool>>> ();
		public int first_oid;
		public int last_oid;
		public float hforce;
		public float vforce;
        public Rectangle_short range = new Rectangle_short();
    }

	public struct AttackUser
	{
		public int skilllevel;
		public ushort level;
		public bool secondweapon;
		public bool flip;
		public Char user;
		public AttackUser (int skilllevel, ushort level, bool secondweapon, bool flip, Char user)
		{
			this.skilllevel = skilllevel;
			this.level = level;
			this.secondweapon = secondweapon;
			this.flip = flip;
			this.user = user;
		}
	}
}