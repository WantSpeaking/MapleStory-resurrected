#define USE_NX

using System;
using System.Collections.Generic;
using constants.skills;
using MapleLib.WzLib;




namespace ms
{
	// Interface for passive buffs
	public abstract class PassiveBuff : System.IDisposable
	{
		public virtual void Dispose ()
		{
		}

		public abstract bool is_applicable (CharStats stats, WzObject level);
		public abstract void apply_to (CharStats stats, WzObject level);

		protected bool f_is_applicable (CharStats stats, WzObject level, Weapon.Type W1, Weapon.Type W2)
		{
			return f_is_applicable (stats, level, W1) || f_is_applicable (stats, level, W2);
		}

		protected bool f_is_applicable (CharStats stats, WzObject level, Weapon.Type W1)
		{
			return stats.get_weapontype () == W1;
		}
	}

	// Abstract base for passives without conditions
	public abstract class ConditionlessBuff : PassiveBuff
	{
		public override bool is_applicable (CharStats stats, WzObject level)
		{
			return true;
		}
	}

	// Buff for angel blessing/blessing of the spirit
	public class AngelBlessingBuff : ConditionlessBuff
	{
		public override void apply_to (CharStats stats, WzObject level)
		{
			stats.add_value (EquipStat.Id.WATK, level["x"]);
			stats.add_value (EquipStat.Id.MAGIC, level["y"]);
			stats.add_value (EquipStat.Id.ACC, level["z"]);
			stats.add_value (EquipStat.Id.AVOID, level["z"]);
		}
	}

	// Buff for Mastery skills
	public class WeaponMasteryBuff : PassiveBuff
	{
		Weapon.Type W1;
		Weapon.Type W2;

		public WeaponMasteryBuff (Weapon.Type w1)
		{
			W1 = w1;
		}

		public WeaponMasteryBuff (Weapon.Type w1, Weapon.Type w2)
		{
			W1 = w1;
			W2 = w2;
		}

		public override bool is_applicable (CharStats stats, WzObject level)
		{
			return f_is_applicable (stats, level, W1);
		}

		public override void apply_to (CharStats stats, WzObject level)
		{
			float mastery = (float)(level["mastery"]) / 100f;
			stats.set_mastery (mastery);
			stats.add_value (EquipStat.Id.ACC, level["x"]);
		}
	}

	// Buff for Achilles
	public class AchillesBuff : ConditionlessBuff
	{
		public override void apply_to (CharStats stats, WzObject level)
		{
			float reducedamage = (float)level["x"] / 1000;
			stats.set_reducedamage (reducedamage);
		}
	}

	// Buff for Berserk
	public class BerserkBuff : PassiveBuff
	{
		public override bool is_applicable (CharStats stats, WzObject level)
		{
			float hp_percent = (float)level["x"] / 100;
			int hp_threshold = (int)(stats.get_total (EquipStat.Id.HP) * hp_percent);
			int hp_current = stats.get_stat (MapleStat.Id.HP);

			return hp_current <= hp_threshold;
		}

		public override void apply_to (CharStats stats, WzObject level)
		{
			float damagepercent = (float)level["damage"] / 100;
			stats.set_damagepercent (damagepercent);
		}
	}

	// Collection of passive buffs
	public class PassiveBuffs
	{
		// Register all effects
		public PassiveBuffs ()
		{
			/*foreach (SkillId.Id skillId in Enum.GetValues (typeof (SkillId.Id)))
			{
				if (skillId == constants.skills.Beginner.BLESSING_OF_THE_FAIRY ||
				    skillId == SkillId.Id.SWORD_MASTERY_FIGHTER ||
				    skillId == SkillId.Id.AXE_MASTERY ||
				    skillId == SkillId.Id.ACHILLES_HERO ||
				    skillId == SkillId.Id.BW_MASTERY ||
				    skillId == SkillId.Id.ACHILLES_PALADIN ||
				    skillId == SkillId.Id.SPEAR_MASTERY ||
				    skillId == SkillId.Id.PA_MASTERY ||
				    skillId == SkillId.Id.ACHILLES_DK ||
				    skillId == SkillId.Id.BERSERK)
				{
					buffs.TryAdd ((int)skillId, null);
				}
			}*/

			// Beginner
			buffs[(int)Beginner.BLESSING_OF_THE_FAIRY] = new AngelBlessingBuff ();

			#region 战士
			// Fighter
			buffs[(int)Fighter.SWORD_MASTERY] = new WeaponMasteryBuff(Weapon.Type.SWORD_1H, Weapon.Type.SWORD_2H);
			buffs[(int)Fighter.AXE_MASTERY] = new WeaponMasteryBuff(Weapon.Type.AXE_1H, Weapon.Type.AXE_2H);

			// Crusader

			// Hero
			buffs[(int)Hero.ACHILLES] = new AchillesBuff();

			// Page
			buffs[(int)Page.SWORD_MASTERY] = new WeaponMasteryBuff(Weapon.Type.SWORD_1H, Weapon.Type.SWORD_2H);//repeated added
			buffs[(int)Page.BW_MASTERY] = new WeaponMasteryBuff(Weapon.Type.MACE_1H, Weapon.Type.MACE_2H);

			// White Knight

			// Paladin
			buffs[(int)Paladin.ACHILLES] = new AchillesBuff();

			// Spearman
			buffs[(int)Spearman.SPEAR_MASTERY] = new WeaponMasteryBuff(Weapon.Type.SPEAR);
			buffs[(int)Spearman.POLEARM_MASTERY] = new WeaponMasteryBuff(Weapon.Type.POLEARM);

			// Dragon Knight

			// Dark Knight
			buffs[(int)DarkKnight.ACHILLES] = new AchillesBuff();
			buffs[(int)DarkKnight.BERSERK] = new BerserkBuff();
            #endregion

            //精准弓
            //神箭手
            buffs[(int)Hunter.BOW_MASTERY] = new WeaponMasteryBuff(Weapon.Type.BOW);
            buffs[(int)Bowmaster.BOW_EXPERT] = new WeaponMasteryBuff(Weapon.Type.BOW);

            //精准弩
            //神弩手
            buffs[(int)Crossbowman.CROSSBOW_MASTERY] = new WeaponMasteryBuff(Weapon.Type.CROSSBOW);
            buffs[(int)Marksman.MARKSMAN_BOOST] = new WeaponMasteryBuff(Weapon.Type.CROSSBOW);

            //精准暗器
            //精准短刀
            buffs[(int)Assassin.CLAW_MASTERY] = new WeaponMasteryBuff(Weapon.Type.CLAW);
            buffs[(int)Bandit.DAGGER_MASTERY] = new WeaponMasteryBuff(Weapon.Type.DAGGER);

            //精准拳
            //精准枪
            buffs[(int)Brawler.KNUCKLER_MASTERY] = new WeaponMasteryBuff(Weapon.Type.KNUCKLE);
            buffs[(int)Gunslinger.GUN_MASTERY] = new WeaponMasteryBuff(Weapon.Type.GUN);

            //魂骑士
            buffs[(int)DawnWarrior.SWORD_MASTERY] = new WeaponMasteryBuff(Weapon.Type.SWORD_1H, Weapon.Type.SWORD_2H);

            //风灵使者
            buffs[(int)WindArcher.BOW_MASTERY] = new WeaponMasteryBuff(Weapon.Type.BOW);

            //夜行者
            buffs[(int)NightWalker.CLAW_MASTERY] = new WeaponMasteryBuff(Weapon.Type.CLAW);

            //奇袭者
            buffs[(int)ThunderBreaker.KNUCKLER_MASTERY] = new WeaponMasteryBuff(Weapon.Type.KNUCKLE);

            //战神
            buffs[(int)Aran.POLEARM_MASTER] = new WeaponMasteryBuff(Weapon.Type.SPEAR);
            buffs[(int)Aran.HIGH_MASTERY] = new WeaponMasteryBuff(Weapon.Type.SPEAR);

        }

        // Apply a passive skill effect to the character stats
        public void apply_buff (CharStats stats, int skill_id, int skill_level)
		{
			if (buffs.TryGetValue (skill_id, out var buff))
			{
				bool wrong_job = !stats.get_job().can_use(skill_id);

				if (wrong_job)
				{
					return;
				}
				if (skill_level<=0)
                {
                    return;
                }

                string strid;

				if (skill_id < 10000000)
				{
					strid = string_format.extend_id (skill_id, 7);
				}
				else
				{
					strid = Convert.ToString (skill_id);
				}

				WzObject src = ms.wz.findSkillImage($"{strid.Substring (0, 3)}.img")["skill"]?[strid]?["level"]?[skill_level.ToString ()];//why has skillId 100;why skillId 12 have skillLevel 0

				if (src != null)
				{
					if (buff != null && buff.is_applicable (stats, src))
					{
						buff.apply_to (stats, src);
					}
				}
				else
				{
					AppDebug.LogWarning ($"passivebuff doesn't exist, skill_id:{skill_id}\t skill_level:{skill_level}");
				}
			}
		}

		private Dictionary<int, PassiveBuff> buffs = new Dictionary<int, PassiveBuff> ();
	}
}


