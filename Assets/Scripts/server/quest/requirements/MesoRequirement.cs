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
namespace server.quest.requirements
{
	using WzImageProperty = MapleLib.WzLib.WzImageProperty;
	using MapleDataTool = provider.MapleDataTool;
	using MapleCharacter = client.MapleCharacter;
    using provider;

    /// 
    /// <summary>
    /// @author Ronan
    /// </summary>
	public class MesoRequirement : MapleQuestRequirement
	{
			private int meso = 0;

		public MesoRequirement(MapleQuest quest, MapleData data) : base(MapleQuestRequirementType.MESO)
		{
			processData(data);
		}

		public override void processData(MapleData data)
		{
			meso = MapleDataTool.getInt(data);
		}


		public override bool check(MapleCharacter chr, int? npcid)
		{
					if (player.Meso >= meso)
					{
							return true;
					}
					else
					{
							//chr.dropMessage(5, "You don't have enough mesos to complete this quest.");
							return false;
					}
		}
	}

}