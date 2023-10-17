using provider;
using System.Text;

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
	using MapleCharacter = client.MapleCharacter;
	using MapleQuestStatus = client.MapleQuestStatus;
	using WzImageProperty = MapleLib.WzLib.WzImageProperty;
	using MapleDataTool = provider.MapleDataTool;

	/// 
	/// <summary>
	/// @author Tyler (Twdtwd)
	/// </summary>
	public class IntervalRequirement : MapleQuestRequirement
	{
		private int interval = -1;
		private int questID;

		public IntervalRequirement (MapleQuest quest, MapleData data) : base (MapleQuestRequirementType.INTERVAL)
		{
			questID = quest.Id;
			processData (data);
		}

		public virtual int Interval
		{
			get
			{
				return interval;
			}
		}

		public override void processData (MapleData data)
		{
			interval = MapleDataTool.getInt (data) * 60 * 1000;
		}

		private static string getIntervalTimeLeft (MapleCharacter chr, IntervalRequirement r)
		{
			StringBuilder str = new StringBuilder ();

			long futureTime = chr.getQuest (MapleQuest.getInstance (r.questID)).CompletionTime + r.Interval;
			long leftTime = futureTime - DateTimeHelper.CurrentUnixTimeMillis ();

			sbyte mode = 0;
			if (leftTime / (60 * 1000) > 0)
			{
				mode++; //counts minutes

				if (leftTime / (60 * 60 * 1000) > 0)
				{
					mode++; //counts hours
				}
			}

			switch (mode)
			{
				case 2:
					int hours = (int)((leftTime / (1000 * 60 * 60)));
					str.Append (hours + " hours, ");

					goto case 1;
				case 1:
					int minutes = (int)((leftTime / (1000 * 60)) % 60);
					str.Append (minutes + " minutes, ");

					goto default;
				default:
					int seconds = (int)(leftTime / 1000) % 60;
					str.Append (seconds + " seconds");
					break;
			}

			return str.ToString ();
		}

		public override bool check (MapleCharacter chr, int? npcid)
		{
			bool check = !chr.getQuest (MapleQuest.getInstance (questID)).getStatus().Equals (MapleQuestStatus.Status.COMPLETED);
			bool check2 = chr.getQuest (MapleQuest.getInstance (questID)).CompletionTime <= DateTimeHelper.CurrentUnixTimeMillis () - interval;

			if (check || check2)
			{
				return true;
			}
			else
			{
				//chr.message ("This quest will become available again in approximately " + getIntervalTimeLeft (chr, this) + ".");
				return false;
			}
		}
	}

}