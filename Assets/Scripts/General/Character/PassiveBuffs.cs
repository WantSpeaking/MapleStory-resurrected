#define USE_NX

using System;
using System.Collections.Generic;
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
			float mastery = (float)(level["mastery"]) / 100;
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
			foreach (SkillId.Id skillId in Enum.GetValues (typeof (SkillId.Id)))
			{
				if (skillId == SkillId.Id.ANGEL_BLESSING ||
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
			}

			// Beginner
			buffs[(int)SkillId.Id.ANGEL_BLESSING] = new AngelBlessingBuff ();

			// Fighter
			buffs[(int)SkillId.Id.SWORD_MASTERY_FIGHTER] = new WeaponMasteryBuff (Weapon.Type.SWORD_1H, Weapon.Type.SWORD_2H);
			buffs[(int)SkillId.Id.AXE_MASTERY] = new WeaponMasteryBuff (Weapon.Type.AXE_1H, Weapon.Type.AXE_2H);

			// Crusader

			// Hero
			buffs[(int)SkillId.Id.ACHILLES_HERO] = new AchillesBuff ();

			// Page
			//buffs[(int)SkillId.Id.SWORD_MASTERY_FIGHTER] = new WeaponMasteryBuff (Weapon.Type.SWORD_1H, Weapon.Type.SWORD_2H);//repeated added
			buffs[(int)SkillId.Id.BW_MASTERY] = new WeaponMasteryBuff (Weapon.Type.MACE_1H, Weapon.Type.MACE_2H);

			// White Knight

			// Paladin
			buffs[(int)SkillId.Id.ACHILLES_PALADIN] = new AchillesBuff ();

			// Spearman
			buffs[(int)SkillId.Id.SPEAR_MASTERY] = new WeaponMasteryBuff (Weapon.Type.SPEAR);
			buffs[(int)SkillId.Id.PA_MASTERY] = new WeaponMasteryBuff (Weapon.Type.POLEARM);

			// Dragon Knight

			// Dark Knight
			buffs[(int)SkillId.Id.ACHILLES_DK] = new AchillesBuff ();
			buffs[(int)SkillId.Id.BERSERK] = new BerserkBuff ();
		}

		// Apply a passive skill effect to the character stats
		public void apply_buff (CharStats stats, int skill_id, int skill_level)
		{
			if (buffs.TryGetValue (skill_id, out var buff))
			{
				bool wrong_job = !stats.get_job ().can_use (skill_id);

				if (wrong_job)
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

				WzObject src = ms.wz.findSkillImage($"{strid.Substring (0, 3)}.img")["skill"][strid]["level"][skill_level.ToString ()];//why has skillId 100;why skillId 12 have skillLevel 0

				if (src != null)
				{
					if (buff != null && buff.is_applicable (stats, src))
					{
						buff.apply_to (stats, src);
					}
				}
				else
				{
					//AppDebug.LogWarning ($"buff doesn't exist, skill_id:{skill_id}\t skill_level:{skill_level}");
				}
			}
		}

		private Dictionary<int, PassiveBuff> buffs = new Dictionary<int, PassiveBuff> ();
	}
}


