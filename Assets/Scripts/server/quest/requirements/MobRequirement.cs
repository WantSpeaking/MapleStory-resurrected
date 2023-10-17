using provider;
using System.Collections.Generic;

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
namespace server.quest.requirements
{

	using WzImageProperty = MapleLib.WzLib.WzImageProperty;
	using MapleDataTool = provider.MapleDataTool;
	using FilePrinter = tools.FilePrinter;
	using MapleCharacter = client.MapleCharacter;
	using MapleQuestStatus = client.MapleQuestStatus;

	/// 
	/// <summary>
	/// @author Tyler (Twdtwd)
	/// </summary>
	public class MobRequirement : MapleQuestRequirement
	{
		internal IDictionary<int, int> mobs = new Dictionary<int, int>();
		private int questID;

		public MobRequirement(MapleQuest quest, MapleData data) : base(MapleQuestRequirementType.MOB)
		{
			questID = quest.Id;
					processData(data);
		}

		/// 
		/// <param name="data">  </param>
		public override void processData(MapleData data)
		{
			foreach (var questEntry in data)
			{
				int mobID = MapleDataTool.getInt(questEntry.getChildByPath("id"));
				int countReq = MapleDataTool.getInt(questEntry.getChildByPath("count"));
				mobs[mobID] = countReq;
			}
		}

		public override bool check(MapleCharacter chr, int? npcid)
		{
			MapleQuestStatus status = chr.getQuest(MapleQuest.getInstance(questID));
			foreach (int? mobIDNullable in mobs.Keys)
			{
				if (!mobIDNullable.HasValue)
					continue;
				var mobID = mobIDNullable.Value;
				int countReq = mobs[mobID];
				int progress;

				try
				{
					progress = int.Parse(status.getProgress(mobID));
				}
				catch (System.FormatException ex)
				{
					//FilePrinter.printError(FilePrinter.EXCEPTION_CAUGHT, ex, "Mob: " + mobID + " Quest: " + questID + "CID: " + chr.Id + " Progress: " + status.getProgress(mobID));
					return false;
				}

				if (progress < countReq)
				{
					return false;
				}
			}
			return true;
		}

		public virtual int getRequiredMobCount(int mobid)
		{
			if (mobs.ContainsKey(mobid))
			{
				return mobs[mobid];
			}
			return 0;
		}

		public virtual IDictionary<int, int> getRequiredMobs ()
		{
			return mobs;
		}
	}

}