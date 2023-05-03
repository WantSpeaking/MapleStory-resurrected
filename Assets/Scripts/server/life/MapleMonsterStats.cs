using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading;

namespace server.life
{
	using BanishInfo = server.life.MapleLifeFactory.BanishInfo;
	using loseItem = server.life.MapleLifeFactory.loseItem;
	using selfDestruction = server.life.MapleLifeFactory.selfDestruction;

	/// <summary>
	/// @author Frz
	/// </summary>
	public class MapleMonsterStats
	{
		public bool changeable;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		public int exp, hp, mp, level, PADamage_Conflict, PDDamage_Conflict, MADamage_Conflict, MDDamage_Conflict, dropPeriod, cp, buffToGive = -1, removeAfter_Conflict;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		public bool boss, undead, ffaLoot, isExplosiveReward, firstAttack, removeOnMiss_Conflict;
		public string name;
		public IDictionary<string, int> animationTimes = new Dictionary<string, int>();
		public IDictionary<Element, ElementalEffectiveness> resistance = new Dictionary<Element, ElementalEffectiveness>();
		public IList<int> revives = new List<int> ();
		public sbyte tagColor, tagBgColor;
		public IList<Tuple<int, int>> skills = new List<Tuple<int, int>>();
		public Tuple<int, int> cool = Tuple.Create(0,0);
		public BanishInfo banish = null;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		public IList<loseItem> loseItem_Conflict = null;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		public selfDestruction selfDestruction_Conflict = null;
		public int fixedStance = 0;
		public bool friendly;

		public virtual bool Change
		{
			set
			{
				this.changeable = value;
			}
		}

		public virtual bool Changeable
		{
			get
			{
				return changeable;
			}
		}

		public virtual int Exp
		{
			get
			{
				return exp;
			}
			set
			{
				this.exp = value;
			}
		}


		public virtual int Hp
		{
			get
			{
				return hp;
			}
			set
			{
				this.hp = value;
			}
		}


		public virtual int Mp
		{
			get
			{
				return mp;
			}
			set
			{
				this.mp = value;
			}
		}


		public virtual int Level
		{
			get
			{
				return level;
			}
			set
			{
				this.level = value;
			}
		}


		public virtual int removeAfter()
		{
			return removeAfter_Conflict;
		}

		public virtual int RemoveAfter
		{
			set
			{
				this.removeAfter_Conflict = value;
			}
		}

		public virtual int DropPeriod
		{
			get
			{
				return dropPeriod;
			}
			set
			{
				this.dropPeriod = value;
			}
		}


		public virtual bool Boss
		{
			set
			{
				this.boss = value;
			}
			get
			{
				return boss;
			}
		}


		public virtual bool FfaLoot
		{
			set
			{
				this.ffaLoot = value;
			}
			get
			{
				return ffaLoot;
			}
		}


		public virtual void setAnimationTime(string name, int delay)
		{
			animationTimes[name] = delay;
		}

		public virtual int getAnimationTime(string name)
		{
			int? ret = animationTimes[name];
			if (ret == null)
			{
				return 500;
			}
			return ret.Value;
		}

		public virtual bool Mobile
		{
			get
			{
				return animationTimes.ContainsKey("move") || animationTimes.ContainsKey("fly");
			}
		}

		public virtual IList<int> Revives
		{
			get
			{
				return revives;
			}
			set
			{
				this.revives = value;
			}
		}


		public virtual bool Undead
		{
			set
			{
				this.undead = value;
			}
			get
			{
				return undead;
			}
		}


		public virtual void setEffectiveness(Element e, ElementalEffectiveness ee)
		{
			resistance[e] = ee;
		}

		public virtual ElementalEffectiveness getEffectiveness(Element e)
		{
			ElementalEffectiveness elementalEffectiveness = resistance[e];
			if (elementalEffectiveness == null)
			{
				return ElementalEffectiveness.NORMAL;
			}
			else
			{
				return elementalEffectiveness;
			}
		}

		public virtual string Name
		{
			get
			{
				return name;
			}
			set
			{
				this.name = value;
			}
		}


		public virtual sbyte getTagColor()
		{
			return tagColor;
		}

		public virtual void setTagColor(int tagColor)
		{
			this.tagColor = (sbyte) tagColor;
		}

		public virtual sbyte getTagBgColor()
		{
			return tagBgColor;
		}

		public virtual void setTagBgColor(int tagBgColor)
		{
			this.tagBgColor = (sbyte) tagBgColor;
		}

		public virtual IList<Tuple<int, int>> Skills
		{
			set
			{
				for (int i = this.skills.Count; i < value.Count; i++)
				{
					this.skills.Add(null);
				}
    
				for (int i = 0; i < value.Count; i++)
				{
					this.skills[i] = value[i];
				}
			}
			get
			{
				return this.skills;
			}
		}


		public virtual int NoSkills
		{
			get
			{
				return this.skills.Count;
			}
		}

		public virtual bool hasSkill(int skillId, int level)
		{
			foreach (Tuple<int, int> skill in skills)
			{
				if (skill.Item1 == skillId && skill.Item2 == level)
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool FirstAttack
		{
			set
			{
				this.firstAttack = value;
			}
			get
			{
				return firstAttack;
			}
		}


		public virtual int BuffToGive
		{
			set
			{
				this.buffToGive = value;
			}
			get
			{
				return buffToGive;
			}
		}


		internal virtual void removeEffectiveness(Element e)
		{
			resistance.Remove(e);
		}

		public virtual BanishInfo BanishInfo
		{
			get
			{
				return banish;
			}
			set
			{
				this.banish = value;
			}
		}


		public virtual int PADamage
		{
			get
			{
				return PADamage_Conflict;
			}
			set
			{
				this.PADamage_Conflict = value;
			}
		}


		public virtual int CP
		{
			get
			{
				return cp;
			}
			set
			{
				this.cp = value;
			}
		}


		public virtual IList<loseItem> loseItem()
		{
			return loseItem_Conflict;
		}

		public virtual void addLoseItem(loseItem li)
		{
			if (loseItem_Conflict == null)
			{
				loseItem_Conflict = new List<loseItem>();
			}
			loseItem_Conflict.Add(li);
		}

		public virtual selfDestruction selfDestruction()
		{
			return selfDestruction_Conflict;
		}

		public virtual selfDestruction SelfDestruction
		{
			set
			{
				this.selfDestruction_Conflict = value;
			}
		}

		public virtual bool ExplosiveReward
		{
			set
			{
				this.isExplosiveReward = value;
			}
			get
			{
				return isExplosiveReward;
			}
		}


		public virtual bool RemoveOnMiss
		{
			set
			{
				this.removeOnMiss_Conflict = value;
			}
		}

		public virtual bool removeOnMiss()
		{
			return removeOnMiss_Conflict;
		}

		public virtual Tuple<int, int> Cool
		{
			set
			{
				this.cool = value;
			}
			get
			{
				return cool;
			}
		}


		public virtual int PDDamage
		{
			get
			{
				return PDDamage_Conflict;
			}
			set
			{
				this.PDDamage_Conflict = value;
			}
		}

		public virtual int MADamage
		{
			get
			{
				return MADamage_Conflict;
			}
			set
			{
				this.MADamage_Conflict = value;
			}
		}

		public virtual int MDDamage
		{
			get
			{
				return MDDamage_Conflict;
			}
			set
			{
				this.MDDamage_Conflict = value;
			}
		}

		public virtual bool Friendly
		{
			get
			{
				return friendly;
			}
			set
			{
				this.friendly = value;
			}
		}





		public virtual int FixedStance
		{
			get
			{
				return this.fixedStance;
			}
			set
			{
				this.fixedStance = value;
			}
		}


		public virtual MapleMonsterStats copy()
		{
			MapleMonsterStats copy = new MapleMonsterStats();
			try
			{
				FieldCopyUtil.setFields(this, copy);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
				try
				{
					Thread.Sleep(10000);
				}
				catch (Exception)
				{

				}

			}

			return copy;
		}

		// FieldCopyUtil src: http://www.codesenior.com/en/tutorial/Java-Copy-Fields-From-One-Object-to-Another-Object-with-Reflection
		private class FieldCopyUtil
		{ // thanks to Codesenior dev team
			internal static void setFields(object from, object to)
			{
				/*System.Reflection.FieldInfo[] fields = from.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
				foreach (System.Reflection.FieldInfo field in fields)
				{
					try
					{
						System.Reflection.FieldInfo fieldFrom = from.GetType().getDeclaredField(field.Name);
						object value = fieldFrom.get(from);
						to.GetType().getDeclaredField(field.Name).set(to, value);

					}
					catch (IllegalAccessException e)
					{
						Console.WriteLine(e.ToString());
						Console.Write(e.StackTrace);
					}
					catch (NoSuchFieldException e)
					{
						Console.WriteLine(e.ToString());
						Console.Write(e.StackTrace);
					}
				}*/
			}
		}
	}

}
