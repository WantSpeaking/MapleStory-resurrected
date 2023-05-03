using System;
using System.Collections.Generic;
using MapleLib.WzLib;

namespace server.life
{
	using MapleData = WzObject;
	using MapleDataProvider = provider.MapleDataProvider;
	using MapleDataProviderFactory = provider.MapleDataProviderFactory;
	using MapleDataTool = provider.MapleDataTool;
	using MapleDataType = WzObjectType;

	public class MapleLifeFactory
	{

		private static MapleDataProvider data = MapleDataProviderFactory.getDataProvider("/Mob.wz");
		private static readonly MapleDataProvider stringDataWZ = MapleDataProviderFactory.getDataProvider("/String.wz");
		private static MapleData mobStringData = stringDataWZ.getData("Mob.img");
		private static MapleData npcStringData = stringDataWZ.getData("Npc.img");
		private static IDictionary<int, MapleMonsterStats> monsterStats = new Dictionary<int, MapleMonsterStats>();
		private static ISet<int> hpbarBosses = HpBarBosses;

		private static ISet<int> HpBarBosses
		{
			get
			{
				ISet<int> ret = new HashSet<int>();
    
				MapleDataProvider uiDataWZ = MapleDataProviderFactory.getDataProvider("UI.wz");
				foreach (var bossData in uiDataWZ.getData("UIWindow.img").getChildByPath("MobGage/Mob").Children)
				{
					ret.Add(Convert.ToInt32(bossData.Name));
				}
    
				return ret;
			}
		}

		/*public static AbstractLoadedMapleLife getLife(int id, string type)
		{
			if (type.Equals("n", StringComparison.OrdinalIgnoreCase))
			{
				return getNPC(id);
			}
			else if (type.Equals("m", StringComparison.OrdinalIgnoreCase))
			{
				return getMonster(id);
			}
			else
			{
				Console.WriteLine("Unknown Life type: " + type);
				return null;
			}
		}*/

		private class MobAttackInfoHolder
		{
			protected internal int attackPos;
			protected internal int mpCon;
			protected internal int coolTime;
			protected internal int animationTime;

			protected internal MobAttackInfoHolder(int attackPos, int mpCon, int coolTime, int animationTime)
			{
				this.attackPos = attackPos;
				this.mpCon = mpCon;
				this.coolTime = coolTime;
				this.animationTime = animationTime;
			}
		}

		private static void setMonsterAttackInfo(int mid, IList<MobAttackInfoHolder> attackInfos)
		{
			if (attackInfos.Count > 0)
			{
				MapleMonsterInformationProvider mi = MapleMonsterInformationProvider.Instance;

				foreach (MobAttackInfoHolder attackInfo in attackInfos)
				{
					mi.setMobAttackInfo(mid, attackInfo.attackPos, attackInfo.mpCon, attackInfo.coolTime);
					mi.setMobAttackAnimationTime(mid, attackInfo.attackPos, attackInfo.animationTime);
				}
			}
		}

		private static Tuple<MapleMonsterStats, IList<MobAttackInfoHolder>> getMonsterStats(int mid)
		{
			MapleData monsterData = data.getData(StringUtil.getLeftPaddedStr(Convert.ToString(mid) + ".img", '0', 11));
			if (monsterData == null)
			{
				return null;
			}
			MapleData monsterInfoData = monsterData.getChildByPath("info");

			var attackInfos = new List<MobAttackInfoHolder>();
			MapleMonsterStats stats = new MapleMonsterStats();

			int linkMid = MapleDataTool.getIntConvert("link", monsterInfoData, 0);
			if (linkMid != 0)
			{
				Tuple<MapleMonsterStats, IList<MobAttackInfoHolder>> linkStats = getMonsterStats(linkMid);
				if (linkStats == null)
				{
					return null;
				}

				// thanks resinate for noticing non-propagable infos such as revives getting retrieved
				(attackInfos).AddRange(linkStats.Item2);
			}

			stats.Hp = MapleDataTool.getIntConvert("maxHP", monsterInfoData);
			stats.Friendly = MapleDataTool.getIntConvert("damagedByMob", monsterInfoData, stats.Friendly ? 1 : 0) == 1;
			stats.PADamage = MapleDataTool.getIntConvert("PADamage", monsterInfoData);
			stats.PDDamage = MapleDataTool.getIntConvert("PDDamage", monsterInfoData);
			stats.MADamage = MapleDataTool.getIntConvert("MADamage", monsterInfoData);
			stats.MDDamage = MapleDataTool.getIntConvert("MDDamage", monsterInfoData);
			stats.Mp = MapleDataTool.getIntConvert("maxMP", monsterInfoData, stats.Mp);
			stats.Exp = MapleDataTool.getIntConvert("exp", monsterInfoData, stats.Exp);
			stats.Level = MapleDataTool.getIntConvert("level", monsterInfoData);
			stats.RemoveAfter = MapleDataTool.getIntConvert("removeAfter", monsterInfoData, stats.removeAfter());
			stats.Boss = MapleDataTool.getIntConvert("boss", monsterInfoData, stats.Boss ? 1 : 0) > 0;
			stats.ExplosiveReward = MapleDataTool.getIntConvert("explosiveReward", monsterInfoData, stats.ExplosiveReward ? 1 : 0) > 0;
			stats.FfaLoot = MapleDataTool.getIntConvert("publicReward", monsterInfoData, stats.FfaLoot ? 1 : 0) > 0;
			stats.Undead = MapleDataTool.getIntConvert("undead", monsterInfoData, stats.Undead ? 1 : 0) > 0;
			stats.Name = MapleDataTool.getString(mid + "/name", mobStringData, "MISSINGNO");
			stats.BuffToGive = MapleDataTool.getIntConvert("buff", monsterInfoData, stats.BuffToGive);
			stats.CP = MapleDataTool.getIntConvert("getCP", monsterInfoData, stats.CP);
			stats.RemoveOnMiss = MapleDataTool.getIntConvert("removeOnMiss", monsterInfoData, stats.removeOnMiss() ? 1 : 0) > 0;

			MapleData special = monsterInfoData.getChildByPath("coolDamage");
			if (special != null)
			{
				int coolDmg = MapleDataTool.getIntConvert("coolDamage", monsterInfoData);
				int coolProb = MapleDataTool.getIntConvert("coolDamageProb", monsterInfoData, 0);
				stats.Cool = Tuple.Create<int,int>(coolDmg, coolProb);
			}
			special = monsterInfoData.getChildByPath("loseItem");
			if (special != null)
			{
				foreach (MapleData liData in special.Children)
				{
					stats.addLoseItem(new loseItem(MapleDataTool.getInt(liData.getChildByPath("id")), (sbyte) MapleDataTool.getInt(liData.getChildByPath("prop")), (sbyte) MapleDataTool.getInt(liData.getChildByPath("x"))));
				}
			}
			special = monsterInfoData.getChildByPath("selfDestruction");
			if (special != null)
			{
				stats.SelfDestruction = new selfDestruction((sbyte) MapleDataTool.getInt(special.getChildByPath("action")), MapleDataTool.getIntConvert("removeAfter", special, -1), MapleDataTool.getIntConvert("hp", special, -1));
			}
			MapleData firstAttackData = monsterInfoData.getChildByPath("firstAttack");
			int firstAttack = 0;
			if (firstAttackData != null)
			{
				if (firstAttackData.Type == WzPropertyType.Float)
				{
					firstAttack = (int)Math.Round(MapleDataTool.getFloat(firstAttackData), MidpointRounding.AwayFromZero);
				}
				else
				{
					firstAttack = MapleDataTool.getInt(firstAttackData);
				}
			}
			stats.FirstAttack = firstAttack > 0;
			stats.DropPeriod = MapleDataTool.getIntConvert("dropItemPeriod", monsterInfoData, stats.DropPeriod / 10000) * 10000;

			// thanks yuxaij, Riizade, Z1peR, Anesthetic for noticing some bosses crashing players due to missing requirements
			bool hpbarBoss = stats.Boss && hpbarBosses.Contains(mid);
			stats.setTagColor (hpbarBoss ? MapleDataTool.getIntConvert("hpTagColor", monsterInfoData, 0) : 0);
			stats.setTagBgColor ( hpbarBoss ? MapleDataTool.getIntConvert("hpTagBgcolor", monsterInfoData, 0) : 0);

			foreach (MapleData idata in monsterData)
			{
				if (!idata.Name.Equals("info"))
				{
					int delay = 0;
					foreach (MapleData pic in idata.Children)
					{
						delay += MapleDataTool.getIntConvert("delay", pic, 0);
					}
					stats.setAnimationTime(idata.Name, delay);
				}
			}
			MapleData reviveInfo = monsterInfoData.getChildByPath("revive");
			if (reviveInfo != null)
			{
				IList<int> revives = new List<int>();
				foreach (MapleData data_ in reviveInfo)
				{
					revives.Add(MapleDataTool.getInt(data_));
				}
				stats.Revives = revives;
			}
			decodeElementalString(stats, MapleDataTool.getString("elemAttr", monsterInfoData, ""));

			MapleMonsterInformationProvider mi = MapleMonsterInformationProvider.Instance;
			MapleData monsterSkillInfoData = monsterInfoData.getChildByPath("skill");
			if (monsterSkillInfoData != null)
			{
				int i = 0;
				IList<Tuple<int, int>> skills = new List<Tuple<int, int>>();
				while (monsterSkillInfoData.getChildByPath(Convert.ToString(i)) != null)
				{
					int skillId = MapleDataTool.getInt(i + "/skill", monsterSkillInfoData, 0);
					int skillLv = MapleDataTool.getInt(i + "/level", monsterSkillInfoData, 0);
					skills.Add(Tuple.Create<int,int>(skillId, skillLv));

					MapleData monsterSkillData = monsterData.getChildByPath("skill" + (i + 1));
					if (monsterSkillData != null)
					{
						int animationTime = 0;
						foreach (MapleData effectEntry in monsterSkillData.Children)
						{
							animationTime += MapleDataTool.getIntConvert("delay", effectEntry, 0);
						}

						MobSkill skill = MobSkillFactory.getMobSkill(skillId, skillLv);
						mi.setMobSkillAnimationTime(skill, animationTime);
					}

					i++;
				}
				stats.Skills = skills;
			}

			int j = 0;
			MapleData monsterAttackData;
			while ((monsterAttackData = monsterData.getChildByPath("attack" + (j + 1))) != null)
			{
				int animationTime = 0;
				foreach (MapleData effectEntry in monsterAttackData.Children)
				{
					animationTime += MapleDataTool.getIntConvert("delay", effectEntry, 0);
				}

				int mpCon = MapleDataTool.getIntConvert("info/conMP", monsterAttackData, 0);
				int coolTime = MapleDataTool.getIntConvert("info/attackAfter", monsterAttackData, 0);
				attackInfos.Add(new MobAttackInfoHolder(j, mpCon, coolTime, animationTime));
				j++;
			}

			MapleData banishData = monsterInfoData.getChildByPath("ban");
			if (banishData != null)
			{
				stats.BanishInfo = new BanishInfo(MapleDataTool.getString("banMsg", banishData), MapleDataTool.getInt("banMap/0/field", banishData, -1), MapleDataTool.getString("banMap/0/portal", banishData, "sp"));
			}

			int noFlip = MapleDataTool.getInt("noFlip", monsterInfoData, 0);
			if (noFlip > 0)
			{
				Point origin = MapleDataTool.getPoint("stand/0/origin", monsterData, default (Point));
				if (origin.X != 0)
				{
					stats.FixedStance = origin.X < 1 ? 5 : 4; // fixed left/right
				}
			}

			return Tuple.Create<MapleMonsterStats, IList<MobAttackInfoHolder>>(stats, attackInfos);
		}

		public static MapleMonster getMonster(int mid)
		{
			try
			{
				monsterStats.TryGetValue (mid, out var stats);
				if (stats == null)
				{
					Tuple<MapleMonsterStats, IList<MobAttackInfoHolder>> mobStats = getMonsterStats(mid);
					stats = mobStats.Item1;
					setMonsterAttackInfo(mid, mobStats.Item2);

					monsterStats[Convert.ToInt32(mid)] = stats;
				}
				MapleMonster ret = new MapleMonster(mid, stats);
				return ret;
			}
			catch (System.NullReferenceException npe)
			{
				Console.WriteLine("[SEVERE] MOB " + mid + " failed to load. Issue: " + npe.Message + "\n\n");
				Console.WriteLine(npe.ToString());
				Console.Write(npe.StackTrace);

				return null;
			}
		}

		public static int getMonsterLevel(int mid)
		{
			try
			{
				monsterStats.TryGetValue (mid, out var stats);
				if (stats == null)
				{
					MapleData monsterData = data.getData(StringUtil.getLeftPaddedStr(Convert.ToString(mid) + ".img", '0', 11));
					if (monsterData == null)
					{
						return -1;
					}
					MapleData monsterInfoData = monsterData.getChildByPath("info");
					return MapleDataTool.getIntConvert("level", monsterInfoData);
				}
				else
				{
					return stats.Level;
				}
			}
			catch (System.NullReferenceException npe)
			{
				Console.WriteLine("[SEVERE] MOB " + mid + " failed to load. Issue: " + npe.Message + "\n\n");
				Console.WriteLine(npe.ToString());
				Console.Write(npe.StackTrace);
			}

			return -1;
		}

		private static void decodeElementalString(MapleMonsterStats stats, string elemAttr)
		{
			for (int i = 0; i < elemAttr.Length; i += 2)
			{
				stats.setEffectiveness(Element.getFromChar(elemAttr[i]), ElementalEffectiveness.getByNumber(Convert.ToInt32(elemAttr[i + 1].ToString())));
			}
		}

		/*public static MapleNPC getNPC(int nid)
		{
			return new MapleNPC(nid, new MapleNPCStats(MapleDataTool.getString(nid + "/name", npcStringData, "MISSINGNO")));
		}*/

		public static string getNPCDefaultTalk(int nid)
		{
			return MapleDataTool.getString(nid + "/d0", npcStringData, "(...)");
		}

		public class BanishInfo
		{

			internal int map;
			internal string portal, msg;

			public BanishInfo(string msg, int map, string portal)
			{
				this.msg = msg;
				this.map = map;
				this.portal = portal;
			}

			public virtual int Map
			{
				get
				{
					return map;
				}
			}

			public virtual string Portal
			{
				get
				{
					return portal;
				}
			}

			public virtual string Msg
			{
				get
				{
					return msg;
				}
			}
		}

		public class loseItem
		{

			internal int id;
			internal sbyte chance, x;

			internal loseItem(int id, sbyte chance, sbyte x)
			{
				this.id = id;
				this.chance = chance;
				this.x = x;
			}

			public virtual int Id
			{
				get
				{
					return id;
				}
			}

			public virtual sbyte Chance
			{
				get
				{
					return chance;
				}
			}

			public virtual sbyte X
			{
				get
				{
					return x;
				}
			}
		}

		public class selfDestruction
		{

			internal sbyte action;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
			internal int removeAfter_Conflict;
			internal int hp;

			internal selfDestruction(sbyte action, int removeAfter, int hp)
			{
				this.action = action;
				this.removeAfter_Conflict = removeAfter;
				this.hp = hp;
			}

			public virtual int Hp
			{
				get
				{
					return hp;
				}
			}

			public virtual sbyte Action
			{
				get
				{
					return action;
				}
			}

			public virtual int removeAfter()
			{
				return removeAfter_Conflict;
			}
		}
	}

}
