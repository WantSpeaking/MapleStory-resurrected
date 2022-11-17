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
namespace server.quest.actions
{
	using MapleCharacter = client.MapleCharacter;
	using MapleQuestStatus = client.MapleQuestStatus;
	using WzImageProperty = MapleLib.WzLib.WzImageProperty;
	using MapleDataTool = provider.MapleDataTool;

	/// 
	/// <summary>
	/// @author Tyler (Twdtwd)
	/// </summary>
	public class QuestAction : MapleQuestAction
	{
		internal int mesos;
		internal IDictionary<int, int> quests = new Dictionary<int, int>();

		public QuestAction(MapleQuest quest, WzImageProperty data) : base(MapleQuestActionType.QUEST, quest)
		{
			questID = quest.Id;
			processData(data);
		}


		public override void processData(WzImageProperty data)
		{
			foreach (WzImageProperty qEntry in data)
			{
				int questid = MapleDataTool.getInt(qEntry.GetFromPath("id"));
				int stat = MapleDataTool.getInt(qEntry.GetFromPath("state"));
				quests[questid] = stat;
			}
		}

		public override void run(MapleCharacter chr, int? extSelection)
		{
			/*foreach (int? questID in quests.Keys)
			{
				int stat = quests[questID];
				chr.updateQuestStatus(new MapleQuestStatus(MapleQuest.getInstance(questID.Value), MapleQuestStatus.Status.getById(stat)));
			}*/
		}
	}

}