using System;
using System.Collections.Generic;
using System.Linq;
using MapleLib.WzLib;
using Utility;

namespace server.life
{
	using YamlConfig = config.YamlConfig;
	using ItemConstants = constants.inventory.ItemConstants;
	using MapleData = WzObject;
	using MapleDataProvider = provider.MapleDataProvider;
	using MapleDataProviderFactory = provider.MapleDataProviderFactory;
	using MapleDataTool = provider.MapleDataTool;
	//using DatabaseConnection = tools.DatabaseConnection;
	using Randomizer = tools.Randomizer;

	public class MapleMonsterInformationProvider
	{
		// Author : LightPepsi

		private static readonly MapleMonsterInformationProvider instance = new MapleMonsterInformationProvider();

		public static MapleMonsterInformationProvider Instance
		{
			get
			{
				return instance;
			}
		}

		/*private readonly IDictionary<int, IList<MonsterDropEntry>> drops = new Dictionary<int, IList<MonsterDropEntry>>();
		private readonly IList<MonsterGlobalDropEntry> globaldrops = new List<MonsterGlobalDropEntry>();
		private readonly IDictionary<int, IList<MonsterGlobalDropEntry>> continentdrops = new Dictionary<int, IList<MonsterGlobalDropEntry>>();

		private readonly IDictionary<int, IList<int>> dropsChancePool = new Dictionary<int, IList<int>>(); // thanks to ronan
		private readonly ISet<int> hasNoMultiEquipDrops = new HashSet<int>();
		private readonly IDictionary<int, IList<MonsterDropEntry>> extraMultiEquipDrops = new Dictionary<int, IList<MonsterDropEntry>>();*/

		private readonly IDictionary<Tuple<int, int>, int> mobAttackAnimationTime = new Dictionary<Tuple<int, int>, int>();
		private readonly IDictionary<MobSkill, int> mobSkillAnimationTime = new Dictionary<MobSkill, int>();

		private readonly IDictionary<int, Tuple<int, int>> mobAttackInfo = new Dictionary<int, Tuple<int, int>>();

		private readonly IDictionary<int, bool> mobBossCache = new Dictionary<int, bool>();
		private readonly IDictionary<int, string> mobNameCache = new Dictionary<int, string>();

		protected internal MapleMonsterInformationProvider()
		{
			//retrieveGlobal();
		}

		/*public IList<MonsterGlobalDropEntry> getRelevantGlobalDrops(int mapid)
		{
			int continentid = mapid / 100000000;

			IList<MonsterGlobalDropEntry> contiItems = continentdrops[continentid];
			if (contiItems == null)
			{ // continent separated global drops found thanks to marcuswoon
				contiItems = new List<>();

				foreach (MonsterGlobalDropEntry e in globaldrops)
				{
					if (e.continentid < 0 || e.continentid == continentid)
					{
						contiItems.Add(e);
					}
				}

				continentdrops[continentid] = contiItems;
			}

			return contiItems;
		}

		private void retrieveGlobal()
		{
			PreparedStatement ps = null;
			ResultSet rs = null;
			Connection con = null;

			try
			{
				con = DatabaseConnection.Connection;
				ps = con.prepareStatement("SELECT * FROM drop_data_global WHERE chance > 0");
				rs = ps.executeQuery();

				while (rs.next())
				{
					globaldrops.Add(new MonsterGlobalDropEntry(rs.getInt("itemid"), rs.getInt("chance"), rs.getByte("continent"), rs.getInt("minimum_quantity"), rs.getInt("maximum_quantity"), rs.getShort("questid")));
				}

				rs.close();
				ps.close();
				con.close();
			}
			catch (SQLException e)
			{
				Console.Error.WriteLine("Error retrieving drop" + e);
			}
			finally
			{
				try
				{
					if (ps != null && !ps.Closed)
					{
						ps.close();
					}
					if (rs != null && !rs.Closed)
					{
						rs.close();
					}
					if (con != null && !con.Closed)
					{
						con.close();
					}
				}
				catch (SQLException ignore)
				{
					Console.WriteLine(ignore.ToString());
					Console.Write(ignore.StackTrace);
				}
			}
		}

//JAVA TO C# CONVERTER WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public java.util.List<MonsterDropEntry> retrieveEffectiveDrop(final int monsterId)
		public virtual IList<MonsterDropEntry> retrieveEffectiveDrop(int monsterId)
		{
			// this reads the drop entries searching for multi-equip, properly processing them

			IList<MonsterDropEntry> list = retrieveDrop(monsterId);
			if (hasNoMultiEquipDrops.Contains(monsterId) || !YamlConfig.config.server.USE_MULTIPLE_SAME_EQUIP_DROP)
			{
				return list;
			}

			IList<MonsterDropEntry> multiDrops = extraMultiEquipDrops[monsterId], extra = new LinkedList<MonsterDropEntry>();
			if (multiDrops == null)
			{
				multiDrops = new LinkedList<>();

				foreach (MonsterDropEntry mde in list)
				{
					if (ItemConstants.isEquipment(mde.itemId) && mde.Maximum > 1)
					{
						multiDrops.Add(mde);

						int rnd = Randomizer.rand(mde.Minimum, mde.Maximum);
						for (int i = 0; i < rnd - 1; i++)
						{
							extra.Add(mde); // this passes copies of the equips' MDE with min/max quantity > 1, but idc on equips they are unused anyways
						}
					}
				}

				if (multiDrops.Count > 0)
				{
					extraMultiEquipDrops[monsterId] = multiDrops;
				}
				else
				{
					hasNoMultiEquipDrops.Add(monsterId);
				}
			}
			else
			{
				foreach (MonsterDropEntry mde in multiDrops)
				{
					int rnd = Randomizer.rand(mde.Minimum, mde.Maximum);
					for (int i = 0; i < rnd - 1; i++)
					{
						extra.Add(mde);
					}
				}
			}

			IList<MonsterDropEntry> ret = new LinkedList<MonsterDropEntry>(list);
			((IList<MonsterDropEntry>)ret).AddRange(extra);

			return ret;
		}

//JAVA TO C# CONVERTER WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public final java.util.List<MonsterDropEntry> retrieveDrop(final int monsterId)
		public IList<MonsterDropEntry> retrieveDrop(int monsterId)
		{
			if (drops.ContainsKey(monsterId))
			{
				return drops[monsterId];
			}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.List<MonsterDropEntry> ret = new java.util.LinkedList<>();
			IList<MonsterDropEntry> ret = new LinkedList<MonsterDropEntry>();

			PreparedStatement ps = null;
			ResultSet rs = null;
			Connection con = null;
			try
			{
				con = DatabaseConnection.Connection;
				ps = con.prepareStatement("SELECT itemid, chance, minimum_quantity, maximum_quantity, questid FROM drop_data WHERE dropperid = ?");
				ps.setInt(1, monsterId);
				rs = ps.executeQuery();

				while (rs.next())
				{
					ret.Add(new MonsterDropEntry(rs.getInt("itemid"), rs.getInt("chance"), rs.getInt("minimum_quantity"), rs.getInt("maximum_quantity"), rs.getShort("questid")));
				}

				con.close();
			}
			catch (SQLException e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
				return ret;
			}
			finally
			{
				try
				{
					if (ps != null && !ps.Closed)
					{
						ps.close();
					}
					if (rs != null && !rs.Closed)
					{
						rs.close();
					}
					if (con != null && !con.Closed)
					{
						con.close();
					}
				}
				catch (SQLException ignore)
				{
					Console.WriteLine(ignore.ToString());
					Console.Write(ignore.StackTrace);
					return ret;
				}
			}
			drops[monsterId] = ret;
			return ret;
		}
		

//JAVA TO C# CONVERTER WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public final java.util.List<int> retrieveDropPool(final int monsterId)
		public IList<int> retrieveDropPool(int monsterId)
		{ // ignores Quest and Party Quest items
			if (dropsChancePool.ContainsKey(monsterId))
			{
				return dropsChancePool[monsterId];
			}

			MapleItemInformationProvider ii = MapleItemInformationProvider.Instance;

			IList<MonsterDropEntry> dropList = retrieveDrop(monsterId);
			IList<int> ret = new List<int>();

			int accProp = 0;
			foreach (MonsterDropEntry mde in dropList)
			{
				if (!ii.isQuestItem(mde.itemId) && !ii.isPartyQuestItem(mde.itemId))
				{
					accProp += mde.chance;
				}

				ret.Add(accProp);
			}

			if (accProp == 0)
			{
				ret.Clear(); // don't accept mobs dropping no relevant items
			}
			dropsChancePool[monsterId] = ret;
			return ret;
		}
*/
		public void setMobAttackAnimationTime(int monsterId, int attackPos, int animationTime)
		{
			mobAttackAnimationTime[Tuple.Create<int,int>(monsterId, attackPos)] = animationTime;
		}

		public int? getMobAttackAnimationTime(int monsterId, int attackPos)
		{
			mobAttackAnimationTime.TryGetValue (Tuple.Create<int,int>(monsterId, attackPos), out var time);
			return time;
		}

		public void setMobSkillAnimationTime(MobSkill skill, int animationTime)
		{
			mobSkillAnimationTime.TryAdd (skill, animationTime);
		}

		public int? getMobSkillAnimationTime(MobSkill skill)
		{
			mobSkillAnimationTime.TryGetValue (skill, out var time);
			return time;
		}

		public void setMobAttackInfo(int monsterId, int attackPos, int mpCon, int coolTime)
		{
			mobAttackInfo[(monsterId << 3) + attackPos] = new Tuple<int, int>(mpCon, coolTime);
		}

		public Tuple<int, int> getMobAttackInfo(int monsterId, int attackPos)
		{
			if (attackPos < 0 || attackPos > 7)
			{
				return null;
			}
			return mobAttackInfo[(monsterId << 3) + attackPos];
		}

		/*public static List<Tuple<int, string>> getMobsIDsFromName(string search)
		{
			MapleDataProvider dataProvider = MapleDataProviderFactory.getDataProvider("wz/String.wz");
			List<Tuple<int, string>> retMobs = new List<Tuple<int, string>>();
			MapleData data = dataProvider.getData("Mob.img");
			var mobTupleList = new LinkedList<Tuple<int, string>>();
			foreach (var mobIdData in data.Children)
			{
				int mobIdFromData = int.Parse(mobIdData.Name);
				string mobNameFromData = MapleDataTool.getString(mobIdData.getChildByPath("name"), "NO-NAME");
				mobTupleList.Add(new Tuple<int, string>(mobIdFromData, mobNameFromData));
			}
			foreach (Tuple<int, string> mobTuple in mobTupleList)
			{
				if (mobTuple.Item2.ToLower().Contains(search.ToLower()))
				{
					retMobs.Add(mobTuple);
				}
			}
			return retMobs;
		}

		public virtual bool isBoss(int id)
		{
			bool? boss = mobBossCache[id];
			if (boss == null)
			{
				try
				{
					boss = MapleLifeFactory.getMonster(id).Boss;
				}
				catch (System.NullReferenceException)
				{
					boss = false;
				}
				catch (Exception e)
				{ //nonexistant mob
					boss = false;

					Console.WriteLine(e.ToString());
					Console.Write(e.StackTrace);
					Console.Error.WriteLine("Nonexistant mob id " + id);
				}

				mobBossCache[id] = boss.Value;
			}

			return boss.Value;
		}

		public virtual string getMobNameFromId(int id)
		{
			string mobName = mobNameCache[id];
			if (string.ReferenceEquals(mobName, null))
			{
				MapleDataProvider dataProvider = MapleDataProviderFactory.getDataProvider(new File("wz/String.wz"));
				MapleData mobData = dataProvider.getData("Mob.img");

				mobName = MapleDataTool.getString(mobData.getChildByPath(id + "/name"), "");
				mobNameCache[id] = mobName;
			}

			return mobName;
		}

		public void clearDrops()
		{
			drops.Clear();
			hasNoMultiEquipDrops.Clear();
			extraMultiEquipDrops.Clear();
			dropsChancePool.Clear();
			globaldrops.Clear();
			continentdrops.Clear();
			retrieveGlobal();
		}*/
	}

}
