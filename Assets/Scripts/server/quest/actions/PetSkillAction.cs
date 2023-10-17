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
	using ItemConstants = constants.inventory.ItemConstants;
	using WzImageProperty = MapleLib.WzLib.WzImageProperty;
	using MapleDataTool = provider.MapleDataTool;
    using provider;

    /// 
    /// <summary>
    /// @author Tyler (Twdtwd)
    /// </summary>
	public class PetSkillAction : MapleQuestAction
	{
		internal int flag;

		public PetSkillAction(MapleQuest quest, MapleData data) : base(MapleQuestActionType.PETSKILL, quest)
		{
			questID = quest.Id;
			processData(data);
		}


		public override void processData(MapleData data)
		{
			flag = MapleDataTool.getInt("petskill", data);
		}

		public override bool check(MapleCharacter chr, int? extSelection)
		{
			MapleQuestStatus status = chr.getQuest(MapleQuest.getInstance(questID));
			if (!(status.getStatus() == MapleQuestStatus.Status.NOT_STARTED && status.Forfeited > 0))
			{
				return false;
			}

			return player.pets.TryGet(0) != null;
		}

		public override void run(MapleCharacter chr, int? extSelection)
		{
			//chr.getPet(0).Flag = (sbyte) ItemConstants.getFlagByInt(flag);
		}
	}

}