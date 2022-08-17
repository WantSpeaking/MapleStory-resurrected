using System.Collections.Generic;

//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY = without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////


namespace ms
{
	public class Attack
	{
		public enum Type
		{
			CLOSE,
			RANGED,
			MAGIC
		}

		public enum DamageType
		{
			DMG_WEAPON,
			DMG_MAGIC,
			DMG_FIXED
		}

		public Type type = Type.CLOSE;
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
	}

	public class MobAttack
	{
		public Attack.Type type = Attack.Type.CLOSE;
		public int watk = 0;
		public int matk = 0;
		public int mobid = 0;
		public int oid = 0;
		public Point_short origin = new Point_short ();
		public bool valid = false; 

		// Create a mob attack for touch damage
		public MobAttack ()
		{
			this.valid = false;
		}

		public MobAttack (int watk, Point_short origin, int mobid, int oid)
		{
			this.type = Attack.Type.CLOSE;
			this.watk = watk;
			this.origin = new ms.Point_short(origin);
			this.mobid = mobid;
			this.oid = oid;
			this.valid = true;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: explicit operator bool() const
		public static explicit operator bool (MobAttack ImpliedObject)
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
			hitcount = attack.hitcount;
			skill = attack.skill;
			speed = attack.speed;
			stance = attack.stance;
			bullet = attack.bullet;
			toleft = attack.toleft;
		}

		public Attack.Type type;
		public int attacker = 0;
		public byte mobcount = 0;
		public byte hitcount = 1;
		public int skill = 0;
		public int charge = 0;
		public int bullet = 0;
		public byte level = 0;
		public byte display = 0;
		public byte stance = 0;
		public byte speed = 0;
		public bool toleft = false;
		public Dictionary<int, List<System.Tuple<int, bool>>> damagelines = new Dictionary<int, List<System.Tuple<int, bool>>> ();
		public int first_oid;
		public int last_oid;
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