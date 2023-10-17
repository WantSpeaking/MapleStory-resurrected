using System;
using System.Collections.Generic;
using MapleLib.WzLib;
using provider;

/*
This file is part of the OdinMS Maple Story Server
Copyright (C) 2008 Patrick Huy <patrick.huy@frz.cc>
Matthias Butz <matze@odinms.de>
Jan Christian Meyer <vimes@odinms.de>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as
published by the Free Software Foundation version 3 as published by
the Free Software Foundation. You may not use, modify or distribute
this program under any other version of the GNU Affero General Public
License.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
namespace server.life
{
	/*using MonitoredLockType = net.server.audit.locks.MonitoredLockType;
	using MonitoredReadLock = net.server.audit.locks.MonitoredReadLock;
	using MonitoredReentrantReadWriteLock = net.server.audit.locks.MonitoredReentrantReadWriteLock;
	using MonitoredWriteLock = net.server.audit.locks.MonitoredWriteLock;
	using MonitoredReadLockFactory = net.server.audit.locks.factory.MonitoredReadLockFactory;
	using MonitoredWriteLockFactory = net.server.audit.locks.factory.MonitoredWriteLockFactory;*/
	using MapleDataProvider = provider.MapleDataProvider;
	using MapleDataProviderFactory = provider.MapleDataProviderFactory;
	using MapleDataTool = provider.MapleDataTool;

	/// 
	/// <summary>
	/// @author Danny (Leifde)
	/// </summary>
	public class MobSkillFactory
	{

		private static IDictionary<string, MobSkill> mobSkills = new Dictionary<string, MobSkill>();
		private static readonly MapleDataProvider dataSource = MapleDataProviderFactory.getDataProvider("/Skill.wz");
		private static MapleData skillRoot = dataSource.getData("MobSkill.img");


//JAVA TO C# CONVERTER WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static MobSkill getMobSkill(final int skillId, final int level)
		public static MobSkill getMobSkill(int skillId, int level)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String key = skillId + "" + level;
			string key = skillId + "" + level;
			try
			{
				MobSkill ret = mobSkills[key];
				if (ret != null)
				{
					return ret;
				}
			}
			finally
			{
				
			}
			try
			{
				MobSkill ret;
				ret = mobSkills[key];
				if (ret == null)
				{
					MapleData skillData = skillRoot.getChildByPath(skillId + "/level/" + level);
					if (skillData != null)
					{
						int mpCon = MapleDataTool.getInt(skillData.getChildByPath("mpCon"), 0);
						IList<int> toSummon = new List<int>();
						for (int i = 0; i > -1; i++)
						{
							if (skillData.getChildByPath(i.ToString()) == null)
							{
								break;
							}
							toSummon.Add(Convert.ToInt32(MapleDataTool.getInt(skillData.getChildByPath(i.ToString()), 0)));
						}
						int effect = MapleDataTool.getInt("summonEffect", skillData, 0);
						int hp = MapleDataTool.getInt("hp", skillData, 100);
						int x = MapleDataTool.getInt("x", skillData, 1);
						int y = MapleDataTool.getInt("y", skillData, 1);
						long duration = MapleDataTool.getInt("time", skillData, 0) * 1000;
						long cooltime = MapleDataTool.getInt("interval", skillData, 0) * 1000;
						int iprop = MapleDataTool.getInt("prop", skillData, 100);
						float prop = iprop / 100;
						int limit = MapleDataTool.getInt("limit", skillData, 0);
						MapleData ltd = skillData.getChildByPath("lt");
						Point lt = default(Point);
						Point rb = default(Point);
						if (ltd != null)
						{
							lt = (Point) ltd.Data;
							rb = (Point) skillData.getChildByPath("rb").Data;
						}
						ret = new MobSkill(skillId, level);
						ret.addSummons(toSummon);
						ret.CoolTime = cooltime;
						ret.Duration = duration;
						ret.Hp = hp;
						ret.MpCon = mpCon;
						ret.SpawnEffect = effect;
						ret.X = x;
						ret.Y = y;
						ret.Prop = prop;
						ret.Limit = limit;
						ret.setLtRb(lt, rb);
					}
					mobSkills[skillId + "" + level] = ret;
				}
				return ret;
			}
			finally
			{
				
			}
		}
	}

}
