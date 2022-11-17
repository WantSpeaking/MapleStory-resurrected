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
	using MapleCharacter = client.MapleCharacter;
	using Item = client.inventory.Item;
	using MapleInventoryType = client.inventory.MapleInventoryType;
	using ItemConstants = constants.inventory.ItemConstants;

	/// 
	/// <summary>
	/// @author Tyler (Twdtwd)
	/// </summary>
	public class ItemRequirement : MapleQuestRequirement
	{
		internal IDictionary<int, int> items = new Dictionary<int, int> ();


		public ItemRequirement (MapleQuest quest, WzImageProperty data) : base (MapleQuestRequirementType.ITEM)
		{
			processData (data);
		}

		public override void processData (WzImageProperty data)
		{
			foreach (WzImageProperty itemEntry in data)
			{
				int itemId = MapleDataTool.getInt (itemEntry.GetFromPath ("id"));
				int count = MapleDataTool.getInt (itemEntry.GetFromPath ("count"), 0);

				items[itemId] = count;
			}
		}


		public override bool check (MapleCharacter chr, int? npcid)
		{
			//MapleItemInformationProvider ii = MapleItemInformationProvider.Instance;
			foreach (int? itemIdNullable in items.Keys)
			{
				var itemId = itemIdNullable.Value;
				if (!itemIdNullable.HasValue)
					continue;

				int countNeeded = items[itemId];
				int count = 0;

				MapleInventoryType iType = ItemConstants.getInventoryType (itemId);

				if (iType.Equals (MapleInventoryType.UNDEFINED))
				{
					return false;
				}

				count = player.get_inventory ().get_total_item_count (itemId);

				//Weird stuff, nexon made some quests only available when wearing gm clothes. This enables us to accept it ><
				if (iType.Equals (MapleInventoryType.EQUIP) && !ItemConstants.isMedal (itemId))
				{
					if (count < countNeeded)
					{
						
						if (player.get_inventory ().get_total_item_count(ms.InventoryType.Id.EQUIPPED,itemId) + count >= countNeeded)
						{
							//chr.dropMessage (5, "Unequip the required " + ii.getName (itemId) + " before trying this quest operation.");
							return false;
						}
					}
				}

				if (count < countNeeded || countNeeded <= 0 && count > 0)
				{
					return false;
				}
			}
			return true;
		}

		public virtual int getItemAmountNeeded (int itemid, bool complete)
		{
			int? amount = items[itemid];
			if (amount != null)
			{
				return amount.Value;
			}
			else
			{
				return complete ? int.MaxValue : int.MinValue;
			}
		}

		public virtual IDictionary<int, int> getCompleteItems ()
		{
			return items;
		}
	}

}