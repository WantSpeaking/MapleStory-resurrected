﻿using System.Collections.Generic;
using ms.Helper;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using ms;
using constants.skills;
using System.Linq;
using org.mariuszgromada.math.mxparser;
using Expression = org.mariuszgromada.math.mxparser.Expression;
using Jint;
using System;

namespace ms
{
	public static class SkillStatExt
	{

	}

	public enum SkillStat
	{
		None,
		x,
		y,
		z,
		q,
		s,
		u,
		w,
		attackCount,
		rb,
		damage,
		lt,
		mpCon,
		mobCount,
		damAbsorbShieldR,
		pddX,
		mhpR,
		psdJump,
		speedMax,
		stanceProp,
		psdSpeed,
		lv2mhp,
		forceCon,
		cooltime,
		indieAsrR,
		indiePad,
		indieMad,
		indiePdd,
		indieMdd,
		indieTerR,
		indieEva,
		indieAcc,
		indieBooster,
		indieSpeed,
		indieJump,
		range,
		time,
		cooltimeMS,
		subTime,
		strX,
		padX,
		bdR,
		damR,
		ignoreMobpdpR,
		mobCountDamR,
		actionSpeed,
		mastery,
		rb2,
		lt2,
		v,
		terR,
		mddX,
		asrR,
		MDF,
		cr,
		prop,
		ballCount,
		t,
		dotInterval,
		dotTime,
		criticaldamageMin,
		criticaldamageMax,
		dot,
		ballDelay,
		ballDelay1,
		pdR,
		dexX,
		mmpR,
		madR,
		lukX,
		intX,
		hcProp,
		hcCooltime,
		hcTime,
		subProp,
		mp,
		hcHp,
		hp,
		indieCr,
		indieDamR,
		mhpX,
		targetPlus,
		indieMaxDamageOverR,
		indieMaxDamageOver,
		damPlus,
		ar,
		madX,
		selfDestruction,
		pddR,
		mddR,
		speed,
		evaX,
		accX,
		onActive,
		jump,
		summonCount,
		acc,
		eva,
		epdd,
		emdd,
		indieMmp,
		indieMhp,
		pdd,
		mdd,
		bulletCount,
		mdd2pdd,
		lv2mmp,
		indiePddR,
		epad,
		attackDelay,
		mdR,
		hcSubTime,
		mad,
		damageToBoss,
		coolTimeR,
		w2,
		u2,
		s2,
		q2,
		v2,
		mesoR,
		dropR,
		expR,
		indieExp,
		indiePadR,
		indieMadR,
		hcSummonHp,
		er,
		indieMhpR,
		indieBDR,
		ppRecovery,
		ballDelay0,
		ballDelay2,
		bulletConsume,
		ignoreMobDamR,
		indieStance,
		dotSuperpos,
		dotTickDamR,
		ppCon,
		ppReq,
		indiePMdR,
		bufftimeR,
		rb3,
		rb4,
		lt3,
		lt4,
		hpCon,
		areaDotCount,
		hcSubProp,
		costmpR,
		MDamageOver,
		variableRect, // null val
		attackPoint, // null val
		property, // null val
		emad,
		ballDelay3,
		emhp,
		mpConReduce,
		indieMmpR,
		indieIgnoreMobpdpR,
		gauge,
		fixdamage,
		hpRCon,
		padR,
		hcReflect,
		reduceForceR,
		timeRemainEffect,
		dex,
		killRecoveryR,
		accR,
		emmp,
		powerCon,
		mmpX,
		epCon,
		kp,
		a,
		ignoreCounter,
		action,
		evaR,
		damageTW3,
		damageTW2,
		damageTW4,
		pad,
		indieAllStat,
		bulletSpeed,
		morph,
		itemConsume,
		nbdR,
		psdIncMaxDam,
		strFX,
		dexFX,
		lukFX,
		intFX,
		pdd2mdd,
		acc2mp,
		eva2hp,
		str2dex,
		dex2str,
		int2luk,
		luk2int,
		luk2dex,
		dex2luk,
		lv2pad,
		lv2mad,
		tdR,
		minionDeathProp,
		abnormalDamR,
		acc2dam,
		pdd2dam,
		mdd2dam,
		pdd2mdx,
		mdd2pdx,
		nocoolProp,
		passivePlus,
		mpConEff,
		lv2damX,
		summonTimeR,
		expLossReduceR,
		onHitHpRecoveryR,
		onHitMpRecoveryR,
		pdr,
		mhp2damX,
		mmp2damX,
		finalAttackDamR,
		guardProp,
		mob, // null val
		extendPrice,
		priceUnit,
		period,
		price,
		reqGuildLevel,
		mileage,
		disCountR,
		pqPointR,
		mesoG,
		itemUpgradeBonusR,
		itemCursedProtectR,
		itemTUCProtectR,
		igpCon,
		gpCon,
		iceGageCon,
		PVPdamage,
		lv2str,
		lv2dex,
		lv2int,
		lv2luk,
		orbCount,
		dotHealHPPerSecondR,
		ballDelay6,
		ballDelay7,
		ballDelay4,
		ballDelay5,
		ballDamage,
		ballAttackCount,
		ballMobCount,
		delay,
		strR,
		dexR,
		intR,
		lukR,
		OnActive,
		PVPdamageX,
		indieMDF,
		soulmpCon,
		prob,
		indieMddR,
		indieDrainHP,
		trembling,
		incMobRateDummy,
		fixCoolTime,
		indieForceSpeed,
		indieForceJump,
		itemCon,
		itemConNo,
	}
	public class SkillInfo
	{
		public Dictionary<SkillStat, string> skillStatInfo = new Dictionary<SkillStat, string> ();
		public Rectangle_short range { get; set; }

		public string damage_Expression { get; set; }
		public string attackCount_Expression { get; set; }
		public string mobCount_Expression { get; set; }
		public string mpCon_Expression { get; set; }
		private int SkillId;
		public int getSkillId ()
		{
			return SkillId;
		}
		public double damage ()
		{
			var e = new org.mariuszgromada.math.mxparser.Expression (damage_Expression);
			return e.calculate ();
		}
		public void addSkillStatInfo (SkillStat sc, string value)
		{
			skillStatInfo.TryAdd (sc, value);
		}

		public int getValue (SkillStat skillStat, int slv)
		{
			int result = 0;
			var value = skillStatInfo.TryGetValue (skillStat);
			if (value == null || slv == 0)
			{
				return 0;
			}
			// Sometimes newlines get taken, just remove those
			value = value.Replace ("\n", "").Replace ("\r", "");
			value = value.Replace ("\\n", "").Replace ("\\r", ""); // unluko
			var original = value;
			if (value.isNumber ())
			{
				result = int.Parse (value);
			}
			else
			{
				try
				{
					value = value.Replace ("u", "Math.ceil");
					value = value.Replace ("d", "Math.floor");
					var toReplace = value.Contains ("y") ? "y"
							: value.Contains ("X") ? "X"
							: "x";

					var res = new Jint.Engine ().Execute (value.Replace (toReplace, slv + "")).GetCompletionValue ().AsNumber ();
					result = (int)res;

				}
				catch (Exception ex)
				{
					AppDebug.LogError (String.Format ("Error when parsing: skill %d, level %d, skill stat %s, tried to eval %s.",
							getSkillId (), slv, skillStat, original));
				}
			}
			return result;
		}

	}

	// Contains information about a skill
	public class SkillData : Cache<SkillData>
	{
		// The stats of one level
		public class Stats
		{
			public float damage;
			/// <summary>
			/// magic damage
			/// </summary>
			public int mad;
			public int fixdamage;
			public int mastery;
			public byte attackcount;
			public byte mobcount;
			public byte bulletcount;
			public short bulletcost;
			public int hpcost;
			public int mpcost;
			public float chance;
			public float critical;
			public float ignoredef;
			public float hrange;
			public Rectangle_short range = new Rectangle_short ();

			public Stats (float damage, int matk, int fixdamage, int mastery, byte attackcount, byte mobcount, byte bulletcount, short bulletcost, int hpcost, int mpcost, float chance, float critical, float ignoredef, float hrange, Rectangle_short range)
			{
				this.damage = damage;
				this.mad = matk;
				this.fixdamage = fixdamage;
				this.mastery = mastery;
				this.attackcount = attackcount;
				this.mobcount = mobcount;
				this.bulletcount = bulletcount;
				this.bulletcost = bulletcost;
				this.hpcost = hpcost;
				this.mpcost = mpcost;
				this.chance = chance;
				this.critical = critical;
				this.ignoredef = ignoredef;
				this.hrange = hrange;
				this.range = new ms.Rectangle_short (range);
			}
		}



		// Skill flags, unfortunately these just have to be hard-coded
		public enum Flags
		{
			NONE = 0x0000,
			ATTACK = 0x0001,
			RANGED = 0x0002
		}

		// Icon types
		public enum Icon
		{
			NORMAL,
			DISABLED,
			MOUSEOVER,
			NUM_ICONS
		}



		// Load a skill from the game files
		public SkillData (int id)
		{
			/// Locate sources
			if (id == 2201008)
			{
				AppDebug.Log ("");
			}
			string strid;
			string jobid;
			if (id > 0 && id <= 9999999)
			{
				strid = string_format.extend_id (id, 7);
				jobid = strid.Substring (0, 3);
			}
			else
			{
				/*	strid = string_format.extend_id (id, 8);
					jobid = strid.Substring (0, 4);*/
				strid = id.ToString ();
				jobid = (id / 10000).ToString ();
			}

			var node_Skillwz_1111img_skill_11111004 = wz.findSkillImage ($"{jobid}.img")["skill"][strid];
			var node_Stringwz_Skillimg_000 = wz.wzFile_string["Skill.img"][strid];

			if (node_Skillwz_1111img_skill_11111004 == null)//sever send skillId 100 ,or others ,which doesn't exist in skillWz
			{
				return;
			}

			/// Load icons
			icons = new[] { new Texture (node_Skillwz_1111img_skill_11111004["icon"]), new Texture (node_Skillwz_1111img_skill_11111004["iconDisabled"]), new Texture (node_Skillwz_1111img_skill_11111004["iconMouseOver"]) };

			/// Load strings
			name = node_Stringwz_Skillimg_000?["name"]?.ToString () ?? "name is null";//todo 2 newly added skill name is null
			desc = node_Stringwz_Skillimg_000?["desc"]?.ToString () ?? "desc is null";

			if (node_Stringwz_Skillimg_000 is WzImageProperty property_Stringwz_Skillimg_000)
			{
				var childNodeCount = property_Stringwz_Skillimg_000.WzProperties.Count;
				for (int level = 1; level <= childNodeCount; level++)
				{
					var node_Stringwz_Skillimg_000_h1 = property_Stringwz_Skillimg_000["h" + level]; //todo 2 可能除了h1、h2、h3等类似节点 还有其他节点 
					if (node_Stringwz_Skillimg_000_h1 != null)
					{
						levels.Add (level, node_Stringwz_Skillimg_000_h1.ToString ());
					}
				}
			}

			/// Load stats
			HashSet<string> unkVals = new HashSet<string> ();

			var node_Skillwz_1111img_skill_0000008_level = node_Skillwz_1111img_skill_11111004["level"];
			if (node_Skillwz_1111img_skill_0000008_level is WzImageProperty property_Skillwz_000img_skill_0000008_level)
			{
				foreach (var property_Skillwz_000img_skill_11111004_level_1 in property_Skillwz_000img_skill_0000008_level.WzProperties)
				{
					float damage = (float)property_Skillwz_000img_skill_11111004_level_1["damage"] / 100;
					int mad = property_Skillwz_000img_skill_11111004_level_1["mad"];
					int fixdamage = property_Skillwz_000img_skill_11111004_level_1["fixdamage"];
					int mastery = property_Skillwz_000img_skill_11111004_level_1["mastery"];
					byte attackcount = (byte)(property_Skillwz_000img_skill_11111004_level_1["attackCount"] ?? 1);
					byte mobcount = (byte)(property_Skillwz_000img_skill_11111004_level_1["mobCount"] ?? 1);
					byte bulletcount = (byte)(property_Skillwz_000img_skill_11111004_level_1["bulletCount"] ?? 1);
					short bulletcost = (short)(property_Skillwz_000img_skill_11111004_level_1["bulletConsume"] ?? bulletcount);
					int hpcost = property_Skillwz_000img_skill_11111004_level_1["hpCon"];
					int mpcost = property_Skillwz_000img_skill_11111004_level_1["mpCon"];
					float chance = (property_Skillwz_000img_skill_11111004_level_1["prop"] ?? 100f) / 100;
					float critical = 0.0f;
					float ignoredef = 0.0f;
					float hrange = (property_Skillwz_000img_skill_11111004_level_1["range"] ?? 100f) / 100;
					Rectangle_short range = new Rectangle_short (property_Skillwz_000img_skill_11111004_level_1);
					int level = string_conversion.or_default (property_Skillwz_000img_skill_11111004_level_1.Name, -1);
					stats.Add (level, new Stats (damage, mad, fixdamage, mastery, attackcount, mobcount, bulletcount, bulletcost, hpcost, mpcost, chance, critical, ignoredef, hrange, range));
				}
				masterlevel = stats.Count;
			}
			else if (node_Skillwz_1111img_skill_11111004["common"] is WzSubProperty node_Skillwz_1111img_skill_11111004_common)
			{
				skillInfo = new SkillInfo ();

				foreach (var commonNode in node_Skillwz_1111img_skill_11111004_common.WzProperties)
				{
					var nodeName = commonNode.Name;
					if (nodeName.Equals ("maxLevel"))
					{
						masterlevel = commonNode;
					}
					else if (nodeName.Contains ("lt") && nodeName.Length <= 3)
					{
						skillInfo.range = new Rectangle_short (node_Skillwz_1111img_skill_11111004_common);//todo 可能存在多个range
					}
					else
					{
						Enum.TryParse (nodeName, out SkillStat skillStat);
						if (skillStat != SkillStat.None)
						{
							skillInfo.addSkillStatInfo (skillStat, commonNode?.ToString ());
						}
						else if (!unkVals.Contains (nodeName))
						{
							//if (LOG_UNKS)
							{
								AppDebug.LogWarning ("Unknown SkillStat " + nodeName);
							}
							unkVals.Add (nodeName);
						}
					}
				}
				/*				skillInfo.range = new Rectangle_short (node_Skillwz_1111img_skill_11111004_common);

								skillInfo.damage_Expression = node_Skillwz_1111img_skill_11111004_common["damage"]?.ToString ();
								skillInfo.mobCount_Expression = node_Skillwz_1111img_skill_11111004_common["mobCount"]?.ToString ();
								skillInfo.attackCount_Expression = node_Skillwz_1111img_skill_11111004_common["attackCount"]?.ToString ();
								skillInfo.mpCon_Expression = node_Skillwz_1111img_skill_11111004_common["mpCon"]?.ToString ();*/

			}

			element = node_Skillwz_1111img_skill_11111004["elemAttr"]?.ToString ();

			if (jobList_canUseAnyWeapon.Any (j => j.ToString () == jobid))
			//if (jobid == "900" || jobid == "910" || jobid == ((int)MapleJob.DAWNWARRIOR1).ToString ())
			{
				reqweapon = Weapon.Type.NONE;
			}
			else
			{
				reqweapon = Weapon.by_value (100 + node_Skillwz_1111img_skill_11111004["weapon"]);
			}

			passive = id % 10000 / 1000 == 0;
			//AppDebug.Log($"skillId:{id}\t isPassive:{passive}");
			flags = flags_of (id);
			invisible = node_Skillwz_1111img_skill_11111004["invisible"];

			/// Load required skills
			var node_Skillwz_1111img_skill_11111004_req = node_Skillwz_1111img_skill_11111004["req"];
			if (node_Skillwz_1111img_skill_11111004_req is WzImageProperty property_Skillwz_1111img_skill_11111003_req)
			{
				foreach (var property_Skillwz_1111img_skill_11111003_req_11111001 in property_Skillwz_1111img_skill_11111003_req.WzProperties)
				{
					int skillid = string_conversion.or_default (property_Skillwz_1111img_skill_11111003_req_11111001.Name, -1);
					int reqlv = property_Skillwz_1111img_skill_11111003_req_11111001;

					reqskills.Add (skillid, reqlv);
				}
			}
		}

		// Return whether the skill is passive
		public bool is_passive ()
		{
			return passive;
		}

		// Return whether the skill is an attack skill
		public bool is_attack ()
		{
			return !passive && (flags & (int)Flags.ATTACK) > 0;
		}

		// Return whether this skill is invisible in the skill book UI
		public bool is_invisible ()
		{
			return invisible;
		}

		// Return the default masterlevel
		public int get_masterlevel ()
		{
			return masterlevel;
		}

		// Return the required weapon
		public Weapon.Type get_required_weapon ()
		{
			return reqweapon;
		}

		// Return the stats of one level
		// If there are no stats for that level, a default object is returned.
		private readonly Stats null_stats = new Stats (0.0f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.0f, 0.0f, 0.0f, 0.0f, new Rectangle_short ());

		public Stats get_stats (int level)
		{
			if (stats.TryGetValue (level, out var stat))
			{
				return stat;
			}

			return null_stats;

		}

		// Return the name of the skill
		public string get_name ()
		{
			return name;
		}

		// Return the description of the skill
		public string get_desc ()
		{
			return desc;
		}

		private readonly string null_level = "Missing level description.";

		// Return the description of a level
		// If there is no description for this level, a warning message is returned.
		public string get_level_desc (int level)
		{
			if (levels.TryGetValue (level, out var level_desc))
			{
				return level_desc;
			}

			return null_level;
		}

		// Return one of the skill icons
		// Cannot fail if type is a valid enumeration
		public Texture get_icon (Icon icon)
		{
			return icons[(int)icon];
		}

		// Return id and level of all required skills
		public Dictionary<int, int> get_reqskills ()
		{
			return reqskills;
		}

		/// <summary>
		/// 新版wz把信息都放在Common节点下
		/// </summary>
		/// <returns></returns>
		public bool hasCommonNode ()
		{
			return skillInfo != null;
		}

		private static List<int> jobList_canUseAnyWeapon = new List<int> () {
			900, 910,
			(int)MapleJob.MH1, (int)MapleJob.MH2,
			(int)MapleJob.MH3, (int)MapleJob.MH4 };

		private Dictionary<int, int> skill_flags = new Dictionary<int, int>
		{
			// Beginner -0
			{(int)SkillId.Id.THREE_SNAILS, (int)Flags.ATTACK},

			// Warrior -100
			{(int)SkillId.Id.POWER_STRIKE, (int)Flags.ATTACK},
			{(int)SkillId.Id.SLASH_BLAST, (int)Flags.ATTACK},

			// Fighter -110
			// Crusader -111
			{(int)SkillId.Id.SWORD_PANIC, (int)Flags.ATTACK},
			{(int)SkillId.Id.AXE_PANIC, (int)Flags.ATTACK},
			{(int)SkillId.Id.SWORD_COMA, (int)Flags.ATTACK},
			//{(int)SkillId.Id.AXE_COMA, (int)Flags.ATTACK},//todo 2 repeaded added
			// Hero -112
			{(int)SkillId.Id.RUSH_HERO, (int)Flags.ATTACK},
			{(int)SkillId.Id.BRANDISH, (int)Flags.ATTACK},

			// Page -120
			{(int)Page.SWORD_ICE_BLOW, (int)Flags.ATTACK  },
			{(int)Page.SWORD_FIRE_BLOW, (int)Flags.ATTACK },
			{(int)Page.SWORD_LIT_BLOW, (int)Flags.ATTACK  },
			{(int)Page.SWORD_HOLY_BLOW, (int)Flags.ATTACK },
			// White Knight -121
			{(int)SkillId.Id.CHARGE, (int)Flags.ATTACK},
			{(int)WhiteKnight.SWORD_ICE_BLOW, (int)Flags.ATTACK  },
			{(int)WhiteKnight.SWORD_FIRE_BLOW, (int)Flags.ATTACK },
			{(int)WhiteKnight.SWORD_LIT_BLOW, (int)Flags.ATTACK  },
			{(int)WhiteKnight.SWORD_HOLY_BLOW, (int)Flags.ATTACK },
			// Paladin -122
			{(int)SkillId.Id.RUSH_PALADIN, (int)Flags.ATTACK},
			{(int)SkillId.Id.BLAST, (int)Flags.ATTACK},
			{(int)SkillId.Id.HEAVENS_HAMMER, (int)Flags.ATTACK},
			{(int)Paladin.SWORD_ICE_BLOW, (int)Flags.ATTACK  },
			{(int)Paladin.SWORD_FIRE_BLOW, (int)Flags.ATTACK },
			{(int)Paladin.SWORD_LIT_BLOW, (int)Flags.ATTACK  },
			{(int)Paladin.SWORD_HOLY_BLOW, (int)Flags.ATTACK },

			// Spearman -130
			// Dragon Knight -131
			{(int)SkillId.Id.DRAGON_BUSTER, (int)Flags.ATTACK},
			{(int)SkillId.Id.DRAGON_FURY, (int)Flags.ATTACK},
			{(int)SkillId.Id.PA_BUSTER, (int)Flags.ATTACK},
			{(int)SkillId.Id.PA_FURY, (int)Flags.ATTACK},
			{(int)SkillId.Id.SACRIFICE, (int)Flags.ATTACK},
			{(int)SkillId.Id.DRAGONS_ROAR, (int)Flags.ATTACK},
			// Dark Knight -132
			{(int)SkillId.Id.RUSH_DK, (int)Flags.ATTACK},

			// MAGICIAN -200 
			{(int)SkillId.Id.ENERGY_BOLT, (int)Flags.ATTACK },
			{(int)SkillId.Id.MAGIC_CLAW, (int)Flags.ATTACK  },

			// FP_WIZARD -210

			// FP_MAGE -211
			{(int)SkillId.Id.SLOW_FP, (int)Flags.ATTACK},
			{(int)SkillId.Id.FIRE_ARROW, (int)Flags.ATTACK    },
			{(int)SkillId.Id.POISON_BREATH, (int)Flags.ATTACK },
			// F/P ArchMage -212
			{(int)SkillId.Id.EXPLOSION, (int)Flags.ATTACK},
			//{(int)SkillId.Id.POISON_BREATH,(int) Flags.ATTACK},//todo 2 repeated added
			{(int)SkillId.Id.SEAL_FP, (int)Flags.ATTACK},
			{(int)SkillId.Id.ELEMENT_COMPOSITION_FP, (int)Flags.ATTACK },

			//冰雷_巫师 = 220
			{(int)ILWizard.COLD_BEAM, (int)Flags.ATTACK },
			{(int)ILWizard.Bolt_Fall, (int)Flags.ATTACK },

			// TODO: Blank?
			{(int)SkillId.Id.FIRE_DEMON, (int)Flags.ATTACK},
			{(int)SkillId.Id.PARALYZE, (int)Flags.ATTACK },
			{(int)SkillId.Id.METEOR_SHOWER, (int)Flags.ATTACK},

			//BOWMAN - 300

			//HUNTER - 310
			//RANGER - 311
			{(int)SkillId.Id.RANGER_Inferno, (int)Flags.ATTACK| (int)Flags.RANGED},
			{(int)SkillId.Id.RANGER_SilverHawk, (int)Flags.ATTACK| (int)Flags.RANGED},
			{(int)SkillId.Id.RANGER_Strafe, (int)Flags.ATTACK},
			{(int)SkillId.Id.RANGER_MortalBlow, (int)Flags.ATTACK},
			//BOWMASTER - 312
			{(int)Bowmaster.HURRICANE, (int)Flags.ATTACK| (int)Flags.RANGED},

			{(int)MH.MHbaseAttack1_0, (int)Flags.ATTACK},
			{(int)MH.MHbaseAttack1_1, (int)Flags.ATTACK},
			{(int)MH.MHbaseAttack1_2, (int)Flags.ATTACK},

			{(int)MH.MHbaseAttack2_0, (int)Flags.ATTACK},
			{(int)MH.MHbaseAttack2_1, (int)Flags.ATTACK},
			{(int)MH.MHbaseAttack2_2, (int)Flags.ATTACK},

			{(int)MH.MHbaseAttack3_0, (int)Flags.ATTACK},
			{(int)MH.MHbaseAttack3_1, (int)Flags.ATTACK},
			{(int)MH.MHbaseAttack3_2, (int)Flags.ATTACK},

			{(int)MH.MHbaseAttack4_0, (int)Flags.ATTACK},
			{(int)MH.MHbaseAttack4_1, (int)Flags.ATTACK},
		};

		// Get some hard-coded information
		private int flags_of (int id)
		{
			if (skill_flags.TryGetValue (id, out var flag))
			{
				return flag;
			}
			else
			{
				return (int)Flags.NONE;
			}
		}

		public SkillInfo skillInfo { get; private set; }
		private Dictionary<int, Stats> stats = new Dictionary<int, Stats> ();
		private string element;
		private Weapon.Type reqweapon;
		private int masterlevel;
		private int flags;
		private bool passive;
		private bool invisible;

		private string name;
		private string desc;
		private Dictionary<int, string> levels = new Dictionary<int, string> ();
		private Dictionary<int, int> reqskills = new Dictionary<int, int> ();

		private Texture[] icons = new Texture[(int)Icon.NUM_ICONS];
	}
}