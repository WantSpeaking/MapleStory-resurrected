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
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////
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
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////


namespace ms
{
	public class RegularAttack : SpecialMove
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply_useeffects(Char& user) const override
		public override void apply_useeffects ( Char user)
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply_actions(Char& user, Attack::Type type) const override
		public override void apply_actions ( Char user, Attack.Type type)
		{
			action.apply (ref user, type);
		}


		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply_stats(const Char& user, Attack& attack) const override
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

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply_hiteffects(const AttackUser& user, Mob& target) const override
		public override void apply_hiteffects (AttackUser user, Mob target)
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Animation get_bullet(const Char& user, int bulletid) const override
		public override Animation get_bullet (Char user, int bulletid)
		{
			return bullet.get (user, bulletid);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_attack() const override
		public override bool is_attack ()
		{
			return true;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_skill() const override
		public override bool is_skill ()
		{
			return false;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_id() const override
		public override int get_id ()
		{
			return 0;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: SpecialMove::ForbidReason can_use(int level, Weapon::Type weapon, const Job& job, ushort hp, ushort mp, ushort bullets) const override
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