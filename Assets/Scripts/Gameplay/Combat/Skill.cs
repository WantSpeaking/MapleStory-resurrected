#define USE_NX

using System;
using System.Linq;
using MapleLib.WzLib;

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
	// The skill implementation of special move
	public class Skill : SpecialMove
	{ 
		public Skill (int id)
		{
			this.skillid = id;
			SkillData data = SkillData.get (skillid);

			string strid;

			if (skillid < 10000000)
			{
				strid = string_format.extend_id (skillid, 7);
			}
			else
			{
				strid = Convert.ToString (skillid);
			}

			WzObject src = nl.nx.wzFile_skill[strid.Substring (0, 3) + ".img"]["skill"][strid];

			projectile = true;
			overregular = false;

			//GlobalMembers.sound = new SingleSkillSound(strid);

			bool byleveleffect = src["CharLevel"]["10"]["effect"].Any ();
			bool multieffect = src["effect0"].Any ();

			if (byleveleffect)
			{
				useeffect = new ByLevelUseEffect (src);
			}
			else if (multieffect)
			{
				useeffect = new MultiUseEffect (src);
			}
			else
			{
				bool isanimation = src["effect"]["0"] == WzPropertyType.Canvas;
				bool haseffect1 = src["effect"]["1"].Any ();

				if (isanimation)
				{
					useeffect = new SingleUseEffect (src);
				}
				else if (haseffect1)
				{
					useeffect = new TwoHandedUseEffect (src);
				}
				else
				{
					switch ((SkillId.Id)skillid)
					{
						case SkillId.Id.IRON_BODY:
						case SkillId.Id.MAGIC_ARMOR:
							useeffect = new IronBodyUseEffect ();
							break;
						default:
							useeffect = new NoUseEffect ();
							break;
					}
				}
			}

			bool bylevelhit = src["CharLevel"]["10"]["hit"].Any ();
			bool byskilllevelhit = src["level"]["1"]["hit"].Any ();
			bool hashit0 = src["hit"]["0"].Any ();
			bool hashit1 = src["hit"]["1"].Any ();

			if (bylevelhit)
			{
				if (hashit0 && hashit1)
				{
					hiteffect = new ByLevelTwoHHitEffect (src);
				}
				else
				{
					hiteffect = new ByLevelHitEffect (src);
				}
			}
			else if (byskilllevelhit)
			{
				hiteffect = new BySkillLevelHitEffect (src);
			}
			else if (hashit0 && hashit1)
			{
				hiteffect = new TwoHandedHitEffect (src);
			}
			else if (hashit0)
			{
				hiteffect = new SingleHitEffect (src);
			}
			else
			{
				hiteffect = new NoHitEffect ();
			}

			bool hasaction0 = src["action"]["0"] == WzPropertyType.String;
			bool hasaction1 = src["action"]["1"] == WzPropertyType.String;

			if (hasaction0 && hasaction1)
			{
				action = new TwoHandedAction (src);
			}
			else if (hasaction0)
			{
				action = new SingleAction (src);
			}
			else if (data.is_attack ())
			{
				bool bylevel = src["level"]["1"]["action"] == WzPropertyType.String;

				if (bylevel)
				{
					action = new ByLevelAction (src, skillid);
				}
				else
				{
					action = new RegularAction ();
					overregular = true;
				}
			}
			else
			{
				action = new NoAction ();
			}

			bool hasball = src["ball"].Any ();
			bool bylevelball = src["level"]["1"]["ball"].Any ();

			if (bylevelball)
			{
				bullet = new BySkillLevelBullet (src, skillid);
			}
			else if (hasball)
			{
				bullet = new SingleBullet (src);
			}
			else
			{
				bullet = new RegularBullet ();
				projectile = false;
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply_useeffects(Char& user) const override
		public override void apply_useeffects ( Char user)
		{
			useeffect.apply (ref user);

			//GlobalMembers.sound.play_use();
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
			attack.skill = skillid;

			int level = user.get_skilllevel (skillid);
			SkillData.Stats stats = SkillData.get (skillid).get_stats (level);

			if (stats.fixdamage != 0)
			{
				attack.fixdamage = stats.fixdamage;
				attack.damagetype = Attack.DamageType.DMG_FIXED;
			}
			else if (stats.matk != 0)
			{
				attack.matk += stats.matk;
				attack.damagetype = Attack.DamageType.DMG_MAGIC;
			}
			else
			{
				attack.mindamage *= stats.damage;
				attack.maxdamage *= stats.damage;
				attack.damagetype = Attack.DamageType.DMG_WEAPON;
			}

			attack.critical += stats.critical;
			attack.ignoredef += stats.ignoredef;
			attack.mobcount = stats.mobcount;
			attack.hrange = stats.hrange;

			switch (attack.type)
			{
				case Attack.Type.RANGED:
					attack.hitcount = stats.bulletcount;
					break;
				default:
					attack.hitcount = stats.attackcount;
					break;
			}

			if (!stats.range.empty ())
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: attack.range = stats.range;
				attack.range = (stats.range);
			}

			if (projectile && attack.bullet == 0)
			{
				switch ((SkillId.Id)skillid)
				{
					case SkillId.Id.THREE_SNAILS:
						switch (level)
						{
							case 1:
								attack.bullet = 4000019;
								break;
							case 2:
								attack.bullet = 4000000;
								break;
							case 3:
								attack.bullet = 4000016;
								break;
						}

						break;
					default:
						attack.bullet = skillid;
						break;
				}
			}

			if (overregular)
			{
				attack.stance = (byte)user.get_look ().get_stance ();

				if (attack.type == Attack.Type.CLOSE && !projectile)
				{
					attack.range = user.get_afterimage ().get_range ();
				}
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply_hiteffects(const AttackUser& user, Mob& target) const override
		public override void apply_hiteffects (AttackUser user, Mob target)
		{
			hiteffect.apply (user, target);

			//GlobalMembers.sound.play_hit();
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
			return SkillData.get (skillid).is_attack ();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_skill() const override
		public override bool is_skill ()
		{
			return true;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_id() const override
		public override int get_id ()
		{
			return skillid;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: SpecialMove::ForbidReason can_use(int level, Weapon::Type weapon, const Job& job, ushort hp, ushort mp, ushort bullets) const override
		public override SpecialMove.ForbidReason can_use (int level, Weapon.Type weapon, Job job, ushort hp, ushort mp, ushort bullets)
		{
			if (level <= 0 || level > SkillData.get (skillid).get_masterlevel ())
			{
				return ForbidReason.FBR_OTHER;
			}

			if (job.can_use (skillid) == false)
			{
				return ForbidReason.FBR_OTHER;
			}

			SkillData.Stats stats = SkillData.get (skillid).get_stats (level);

			if (hp <= stats.hpcost)
			{
				return ForbidReason.FBR_HPCOST;
			}

			if (mp < stats.mpcost)
			{
				return ForbidReason.FBR_MPCOST;
			}

			Weapon.Type reqweapon = SkillData.get (skillid).get_required_weapon ();

			if (weapon != reqweapon && reqweapon != Weapon.Type.NONE)
			{
				return ForbidReason.FBR_WEAPONTYPE;
			}

			switch (weapon)
			{
				case Weapon.Type.BOW:
				case Weapon.Type.CROSSBOW:
				case Weapon.Type.CLAW:
				case Weapon.Type.GUN:
					return (bullets >= stats.bulletcost) ? ForbidReason.FBR_NONE : ForbidReason.FBR_BULLETCOST;
				default:
					return ForbidReason.FBR_NONE;
			}
		}

		private SkillAction action;

		private SkillBullet bullet;

		//private new SkillSound sound = new new SkillSound();
		private SkillUseEffect useeffect;
		private SkillHitEffect hiteffect;

		private int skillid;
		private bool overregular;
		private bool projectile;
	}
}


#if USE_NX
#endif