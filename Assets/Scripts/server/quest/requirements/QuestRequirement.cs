using System.Collections.Generic;

/*
	This file is part of the MapleSolaxia Maple Story Server

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
	using MapleCharacter = client.MapleCharacter;
	using MapleQuestStatus = client.MapleQuestStatus;

	/// 
	/// <summary>
	/// @author Tyler (Twdtwd)
	/// </summary>
	public class QuestRequirement : MapleQuestRequirement
	{
		internal IDictionary<int, int> quests = new Dictionary<int, int> ();

		public QuestRequirement (MapleQuest quest, WzImageProperty data) : base (MapleQuestRequirementType.QUEST)
		{
			processData (data);
		}

		/// 
		/// <param name="data">  </param>
		public override void processData (WzImageProperty data)
		{
			foreach (WzImageProperty questEntry in data)
			{
				int questID = MapleDataTool.getInt (questEntry.GetFromPath ("id"));
				int stateReq = MapleDataTool.getInt (questEntry.GetFromPath ("state"));
				quests[questID] = stateReq;
			}
		}


		public override bool check (MapleCharacter chr, int? npcid)
		{
			foreach (int questID in quests.Keys)
			{
				int stateReq = quests[questID];
				MapleQuestStatus qs = chr.getQuest (MapleQuest.getInstance (questID));

				if (qs == null && (MapleQuestStatus.Status)(stateReq) == (MapleQuestStatus.Status.NOT_STARTED))
				{
					continue;
				}

				if (qs == null || !qs.getStatus ().Equals ((MapleQuestStatus.Status)(stateReq)))
				{
					return false;
				}

			}
			return true;
		}
	}

}