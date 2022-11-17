/*using System;

*//*
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
 *//*
namespace server.quest.requirements
{
	using MapleCharacter = client.MapleCharacter;
	using WzImageProperty = MapleLib.WzLib.WzImageProperty;
	using MapleDataTool = provider.MapleDataTool;

	/// 
	/// <summary>
	/// @author Tyler (Twdtwd)
	/// </summary>
	public class EndDateRequirement : MapleQuestRequirement
	{
		private string timeStr;


		public EndDateRequirement(MapleQuest quest, WzImageProperty data) : base(MapleQuestRequirementType.END_DATE)
		{
			processData(data);
		}

		/// 
		/// <param name="data">  </param>
		public override void processData(WzImageProperty data)
		{
			timeStr = MapleDataTool.getString(data);
		}


		public override bool check(MapleCharacter chr, int? npcid)
		{
			DateTime cal = new DateTime();
			cal = new DateTime(int.Parse(timeStr.Substring(0, 4)), int.Parse(timeStr.Substring(4, 2)), int.Parse(timeStr.Substring(6, 2)), int.Parse(timeStr.Substring(8, 2)), 0, 0);
			return cal.Ticks >= DateTimeHelper.CurrentUnixTimeMillis();
		}
	}

}*/