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
	using MapleCharacter = client.MapleCharacter;
	using WzImageProperty = MapleLib.WzLib.WzImageProperty;
	using MapleDataTool = provider.MapleDataTool;
	using System;
    using provider;

    /// 
    /// <summary>
    /// @author Ronan
    /// </summary>
	public class EndTimeRequirement : MapleQuestRequirement
	{
		private string end;

		public EndTimeRequirement (MapleQuest quest, MapleData data) : base (MapleQuestRequirementType.START)
		{
			processData (data);
		}

		public override void processData (MapleData data)
		{
			end = data?.ToString ();
		}

		public override bool check (MapleCharacter chr, int? npcid)
		{
			var nowString = System.DateTime.Now.ToString ("yyyyMMddHH");
			var nowTime = Convert.ToInt32 (nowString);

			if (!string.IsNullOrEmpty (end))
			{
				var endTimeTimeString = end;
				var endTime = Convert.ToInt32 (endTimeTimeString);
				return nowTime <= endTime;
			}

			return false;
		}

		public virtual string get ()
		{
			return end;
		}
	}

}