﻿/*
    This file is part of the HeavenMS MapleStory Server
    Copyleft (L) 2016 - 2019 RonanLana

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
	/// @author Ronan
	/// </summary>
	public class InfoAction : MapleQuestAction
	{

			private string info;
			private new int questID;

			public InfoAction(MapleQuest quest, WzImageProperty data) : base(MapleQuestActionType.INFO, quest)
			{
					questID = quest.Id;
					processData(data);
			}

			public override void processData(WzImageProperty data)
			{
					info = MapleDataTool.getString(data, "");
			}


			public override void run(MapleCharacter chr, int? extSelection)
			{
					//chr.AbstractPlayerInteraction.setQuestProgress(questID, info);
			}

	}

}