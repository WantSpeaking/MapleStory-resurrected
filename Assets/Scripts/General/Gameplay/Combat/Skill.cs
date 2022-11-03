#define USE_NX

using System;
using System.Linq;
using MapleLib.WzLib;




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
			string jobid;
			if (skillid < 10000000)
			{
				strid = string_format.extend_id (skillid, 7);
				jobid = strid.Substring (0, 3);
			}
			else
			{
				strid = Convert.ToString (skillid);
				jobid = (id / 10000).ToString ();
			}

			WzObject src = ms.wz.findSkillImage (jobid + ".img")["skill"][strid];

			projectile = true;
			overregular = false;

			sound = new SingleSkillSound (strid);

			bool byleveleffect = src["CharLevel"]?["10"]?["effect"]?.Any () ?? false; //todo 2 src["CharLevel"]?["10"]?["effect"] == null
			bool multieffect = src["effect0"]?.Any () ?? false; //todo 2 src["effect0"] == null

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
				bool isanimation = src["effect"]?["0"]?.IsTexture () ?? false;
				bool haseffect0 = src["effect"]?["0"]?.Any () ?? false;
				bool haseffect1 = src["effect"]?["1"]?.Any () ?? false;

				if (isanimation)
				{
					useeffect = new SingleUseEffect (src);
				}
				else if (haseffect0 && !haseffect1)
				{
					useeffect = new OneHandedUseEffect (src);
				}
				else if (haseffect0 && haseffect1)
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
						case SkillId.Id.CHARGE:
							useeffect = new White_Knight_Charge_1211002_UseEffect (src);
							break;
						default:
							useeffect = new NoUseEffect ();
							break;
					}
				}
			}

			bool bylevelhit = src["CharLevel"]?["10"]?["hit"]?.Any () ?? false;
			bool byskilllevelhit = src["level"]?["1"]?["hit"]?.Any () ?? false;
			bool hashit0 = src["hit"]?["0"]?.Any () ?? false;
			bool hashit1 = src["hit"]?["1"]?.Any () ?? false;

			bool is_hit0_animation = src["hit"]?["0"]["0"]?.IsTexture () ?? false;


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
			else if (hashit0 && is_hit0_animation)
			{
				hiteffect = new SingleHitEffect (src);
			}
			else
			{
				switch ((SkillId.Id)skillid)
				{
					case SkillId.Id.CHARGE:
						hiteffect = new White_Knight_Charge_1211002_HitEffect (src);
						break;
					default:
						hiteffect = new NoHitEffect ();
						break;
				}
			}

			bool hasaction0 = src["action"]?["0"] == WzPropertyType.String;
			bool hasaction1 = src["action"]?["1"] == WzPropertyType.String;

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
				bool bylevel = src["level"]?["1"]?["action"] == WzPropertyType.String;

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

			if (src["prepare"] != null)
			{
				prepareEffect = new PrepareEffect (src["prepare"]);
				action = new SingleAction (src["prepare"]["action"].ToString (), false);
			}
			if (src["keydown"] != null)
			{
				keydownEffect = new OnKeyDownEffect (src["keydown"]);
			}
			if (src["keydownend"] != null)
			{
				keydownendEffect = new OnKeyDownEndEffect (src["keydownend"]);
			}

			bool hasball = src["ball"]?.Any () ?? false;
			bool bylevelball = src["level"]?["1"]?["ball"]?.Any () ?? false;

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

		public override void apply_useeffects (Char user)
		{
			useeffect.apply (user);

			sound.play_use ();
		}
		public override void apply_prepareEffect (Char user)
		{
			prepareEffect?.apply (user);
		}
		public override void apply_keydownEffect (Char user)
		{
			keydownEffect?.apply (user);
		}
		public override void apply_keydownendEffect (Char user)
		{
			keydownendEffect?.apply (user);
		}
		public override void apply_actions (Char user, Attack.Type type)
		{
			action.apply (ref user, type);
		}

		public override void apply_stats (Char user, Attack attack)
		{
			attack.skill = skillid;

			int level = user.get_skilllevel (skillid);
			var skillData = SkillData.get (skillid);
			SkillData.Stats stats = skillData.get_stats (level);
			var hasCommonNode = skillData.hasCommonNode ();


			if (hasCommonNode)
			{
				var fixdamage = skillData.skillInfo.getValue (SkillStat.fixdamage, level);
				var mad = skillData.skillInfo.getValue (SkillStat.mad, level);
				var damage = skillData.skillInfo.getValue (SkillStat.damage, level);
				var cr = skillData.skillInfo.getValue (SkillStat.cr, level);
				var mobCount = skillData.skillInfo.getValue (SkillStat.mobCount, level);
				var range = skillData.skillInfo.getValue (SkillStat.range, level);
				var bulletCount = skillData.skillInfo.getValue (SkillStat.bulletCount, level);
				var attackCount = skillData.skillInfo.getValue (SkillStat.attackCount, level);

				if (fixdamage != 0)
				{
					attack.fixdamage = fixdamage;
					attack.damagetype = Attack.DamageType.DMG_FIXED;
				}
				else if (mad != 0)
				{
					attack.matk += mad;
					attack.damagetype = Attack.DamageType.DMG_MAGIC;
				}
				else
				{
					attack.mindamage *= damage;
					attack.maxdamage *= damage;
					attack.damagetype = Attack.DamageType.DMG_WEAPON;
				}

				attack.critical += cr;
				//attack.ignoredef += stats.ignoredef;
				attack.mobcount = (byte)(mobCount);
				attack.hrange = (range == 0 ? 100 : range) / 100;

				switch (attack.type)
				{
					case Attack.Type.RANGED:
						attack.hitcount = (byte)bulletCount;
						break;
					default:
						attack.hitcount = (byte)attackCount;
						break;
				}

				var range1 = skillData.skillInfo.range;
				if (!range1.empty ())
				{
					attack.range = new Rectangle_short (range1);
				}
			}
			else
			{
				if (stats.fixdamage != 0)
				{
					attack.fixdamage = stats.fixdamage;
					attack.damagetype = Attack.DamageType.DMG_FIXED;
				}
				else if (stats.mad != 0)
				{
					attack.matk += stats.mad;
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
				attack.mobcount = (byte)(stats.mobcount);
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

				var range = stats.range;
				if (!range.empty ())
				{
					attack.range = new Rectangle_short (range);
				}
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

		public override void apply_hiteffects (AttackUser user, Mob target)
		{
			hiteffect.apply (user, target);

			//sound.play_hit();
		}

		public override Animation get_bullet (Char user, int bulletid)
		{
			return bullet.get (user, bulletid);
		}

		public override bool is_attack ()
		{
			return SkillData.get (skillid).is_attack ();
		}

		public override bool is_skill ()
		{
			return true;
		}

		public override int get_id ()
		{
			return skillid;
		}

		public override bool has_skillPrepareEffect ()
		{
			return prepareEffect != null;
		}
		Player player => ms.Stage.get ().get_player ();
		public override SpecialMove.ForbidReason can_use (int level, Weapon.Type weapon, Job job, ushort hp, ushort mp, ushort bullets)
		{
			if (level <= 0 || level > SkillData.get (skillid).get_masterlevel ())
			{
				return ForbidReason.FBR_SkillLevel_LessThan0_or_GreateThanMaxLevel;
			}

			if (job.can_use (skillid) == false)
			{
				return ForbidReason.FBR_JobCanNotUseThisSkill;
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
					if (player.can_useBow_withoutArrows () || (bullets >= stats.bulletcost))
					{
						return ForbidReason.FBR_NONE;
					}
					else
					{
						return ForbidReason.FBR_BULLETCOST;
					}
				case Weapon.Type.CLAW:
				case Weapon.Type.GUN:
					return (bullets >= stats.bulletcost) ? ForbidReason.FBR_NONE : ForbidReason.FBR_BULLETCOST;
				default:
					return ForbidReason.FBR_NONE;
			}
		}

		public override SkillAction get_action (Char user)
		{
			return action;
		}

		public OnKeyDownEffect GetKeydownEffect ()
		{
			return keydownEffect;
		}
		private SkillAction action;

		private SkillBullet bullet;

		private SkillSound sound;
		private SkillUseEffect useeffect;
		private SkillHitEffect hiteffect;
		private SkillUseEffect prepareEffect;
		private OnKeyDownEffect keydownEffect;
		private SkillUseEffect keydownendEffect;

		private int skillid;
		private bool overregular;
		private bool projectile;
	}
}