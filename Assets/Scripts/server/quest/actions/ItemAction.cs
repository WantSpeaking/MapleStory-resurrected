using provider;
using System;
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
	using MapleClient = client.MapleClient;
	using Item = ms.Item;
	using MapleInventoryType = client.inventory.MapleInventoryType;
	using ItemConstants = constants.inventory.ItemConstants;
	using WzImageProperty = MapleLib.WzLib.WzImageProperty;
	using MapleDataTool = provider.MapleDataTool;
	using MapleInventoryManipulator = client.inventory.manipulator.MapleInventoryManipulator;
	using FilePrinter = tools.FilePrinter;
	using MaplePacketCreator = tools.MaplePacketCreator;
	using Pair = tools.Pair;
	using Randomizer = tools.Randomizer;

	/// 
	/// <summary>
	/// @author Tyler (Twdtwd)
	/// @author Ronan
	/// </summary>
	public class ItemAction : MapleQuestAction
	{
		private SortedList<int, ItemData> items = new SortedList<int, ItemData> ();

		public ItemAction (MapleQuest quest, MapleData data) : base (MapleQuestActionType.ITEM, quest)
		{
			processData (data);
		}

		public override void processData (MapleData data)
		{
			foreach (var iEntry in data)
			{
				int id = MapleDataTool.getInt (iEntry.getChildByPath ("id"));
				int count = MapleDataTool.getInt (iEntry.getChildByPath ("count"), 1);
				int period = MapleDataTool.getInt (iEntry.getChildByPath ("period"), 0);

				int? prop = null;
				var propData = iEntry.getChildByPath("prop");
				if (propData != null)
				{
					prop = MapleDataTool.getInt (propData);
				}

				int gender = 2;
				if (iEntry.getChildByPath("gender") != null)
				{
					gender = MapleDataTool.getInt (iEntry.getChildByPath("gender"));
				}

				int job = -1;
				if (iEntry.getChildByPath("job") != null)
				{
					job = MapleDataTool.getInt (iEntry.getChildByPath("job"));
				}

				var map = int.Parse (iEntry.Name);
				items.Add (map, new ItemData (this, map, id, count, prop, job, gender, period));
			}
			//items.Sort (new ComparatorAnonymousInnerClass (this));
		}

		private class ComparatorAnonymousInnerClass : IComparer<ItemData>
		{
			private readonly ItemAction outerInstance;

			public ComparatorAnonymousInnerClass (ItemAction outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public int compare (ItemData o1, ItemData o2)
			{
				return o1.map - o2.map;
			}

			public int Compare (ItemData x, ItemData y)
			{
				throw new NotImplementedException ();
			}
		}

		public override void run (MapleCharacter chr, int? extSelection)
		{
			/*LinkedList<ItemData> takeItem = new LinkedList<ItemData> ();
			LinkedList<ItemData> giveItem = new LinkedList<ItemData> ();

			int props = 0, rndProps = 0, accProps = 0;
			foreach (ItemData item in items)
			{
				if (item.Prop != null && item.Prop != -1 && canGetItem (item, chr))
				{
					props += item.Prop.Value;
				}
			}

			int extNum = 0;
			if (props > 0)
			{
				rndProps = Randomizer.Next (props);
			}
			foreach (ItemData iEntry in items)
			{
				if (!canGetItem (iEntry, chr))
				{
					continue;
				}

				if (iEntry.Prop != null)
				{
					if (iEntry.Prop == -1)
					{
						if (extSelection.Value != extNum++)
						{
							continue;
						}
					}
					else
					{
						accProps += iEntry.Prop.Value;

						if (accProps <= rndProps)
						{
							continue;
						}
						else
						{
							accProps = int.MinValue;
						}
					}
				}

				if (iEntry.Count < 0)
				{ // Remove Item
					takeItem.Add (iEntry);
				}
				else
				{ // Give Item
					giveItem.Add (iEntry);
				}
			}

			// must take all needed items before giving others

			foreach (ItemData iEntry in takeItem)
			{
				int itemid = iEntry.Id, count = iEntry.Count;

				MapleInventoryType type = ItemConstants.getInventoryType (itemid);
				int quantity = count * -1; // Invert
				if (type.Equals (MapleInventoryType.EQUIP))
				{
					if (chr.getInventory (type).countById (itemid) < quantity)
					{
						// Not enough in the equip inventoty, so check Equipped...
						if (chr.getInventory (MapleInventoryType.EQUIPPED).countById (itemid) > quantity)
						{
							// Found it equipped, so change the type to equipped.
							type = MapleInventoryType.EQUIPPED;
						}
					}
				}

				MapleInventoryManipulator.removeById (chr.Client, type, itemid, quantity, true, false);
				chr.announce (MaplePacketCreator.getShowItemGain (itemid, (short)count, true));
			}

			foreach (ItemData iEntry in giveItem)
			{
				int itemid = iEntry.Id, count = iEntry.Count, period = iEntry.Period; // thanks Vcoc for noticing quest milestone item not getting removed from inventory after a while

				MapleInventoryManipulator.addById (chr.Client, itemid, (short)count, "", -1, period > 0 ? (DateTimeHelper.CurrentUnixTimeMillis () + period * 60 * 1000) : -1);
				chr.announce (MaplePacketCreator.getShowItemGain (itemid, (short)count, true));
			}*/
		}

		public override bool check (MapleCharacter chr, int? extSelection)
		{
			/*var gainList = new LinkedList<(Item, MapleInventoryType)> ();
			var selectList = new LinkedList<(Item, MapleInventoryType)> ();
			var randomList = new LinkedList<(Item, MapleInventoryType)> ();

			var allSlotUsed = new List<int> (5);
			for (sbyte i = 0; i < 5; i++)
			{
				allSlotUsed.Add (0);
			}

			foreach (var pair in items)
			{
				var item = pair.Value;
				if (!canGetItem (item, chr))
				{
					continue;
				}

				MapleInventoryType type = ItemConstants.getInventoryType (item.Id);
				if (item.Prop != null)
				{
					Item toItem = new Item (item.Id, (short)0, "", 0);

					if (item.Prop < 0)
					{
						selectList.AddLast (new ValueTuple<Item, MapleInventoryType> (toItem, type));
					}
					else
					{
						randomList.AddLast (new ValueTuple<Item, MapleInventoryType> (toItem, type));
					}

				}
				else
				{
					// Make sure they can hold the item.
					Item toItem =  new Item (item.Id, (short)0, "", 0);
					gainList.AddLast (new ValueTuple<Item, MapleInventoryType> (toItem, type));

					if (item.Count < 0)
					{
						// Make sure they actually have the item.
						int quantity = item.Count * -1;

						int freeSlotCount = chr.getInventory (type).freeSlotCountById (item.Id, quantity);
						if (freeSlotCount == -1)
						{
							if (type.Equals (MapleInventoryType.EQUIP) && chr.getInventory (MapleInventoryType.EQUIPPED).countById (item.Id) > quantity)
							{
								continue;
							}

							announceInventoryLimit (Collections.singletonList (item.Id), chr);
							return false;
						}
						else
						{
							int idx = type.Type - 1; // more slots available from the given items!
							allSlotUsed[idx] = allSlotUsed[idx] - freeSlotCount;
						}
					}
				}
			}

			if (randomList.Count > 0)
			{
				int result;
				MapleClient c = chr.Client;

				LinkedList<int> rndUsed = new List<object> (5);
				for (sbyte i = 0; i < 5; i++)
				{
					rndUsed.Add (allSlotUsed[i]);
				}

				foreach (Pair<Item, MapleInventoryType> it in randomList)
				{
					int idx = it.Right.Type - 1;

					result = MapleInventoryManipulator.checkSpaceProgressively (c, it.Left.ItemId, it.Left.Quantity, "", rndUsed[idx], false);
					if (result % 2 == 0)
					{
						announceInventoryLimit (Collections.singletonList (it.Left.ItemId), chr);
						return false;
					}

					allSlotUsed[idx] = Math.Max (allSlotUsed[idx], result >> 1);
				}
			}

			if (selectList.Count > 0)
			{
				Pair<Item, MapleInventoryType> selected = selectList[extSelection];
				gainList.Add (selected);
			}

			if (!canHold (chr, gainList))
			{
				LinkedList<int> gainItemids = new LinkedList<int> ();
				foreach (Pair<Item, MapleInventoryType> it in gainList)
				{
					gainItemids.Add (it.Left.ItemId);
				}

				announceInventoryLimit (gainItemids, chr);
				return false;
			}*/
			return true;
		}

		/*private void announceInventoryLimit (LinkedList<int> itemids, MapleCharacter chr)
		{
			foreach (int? id in itemids)
			{
				if (MapleItemInformationProvider.Instance.isPickupRestricted (id) && chr.haveItemWithId (id, true))
				{
					chr.dropMessage (1, "Please check if you already have a similar one-of-a-kind item in your inventory.");
					return;
				}
			}

			chr.dropMessage (1, "Please check if you have enough space in your inventory.");
		}

		private bool canHold (MapleCharacter chr, LinkedList<(Item, MapleInventoryType)> gainList)
		{
			LinkedList<int> toAddItemids = new LinkedList<int> ();
			LinkedList<int> toAddQuantity = new LinkedList<int> ();
			LinkedList<int> toRemoveItemids = new LinkedList<int> ();
			LinkedList<int> toRemoveQuantity = new LinkedList<int> ();

			foreach (var item in gainList)
			{
				Item it = item.Item1;

				if (it.Quantity > 0)
				{
					toAddItemids.AddLast (it.ItemId);
					toAddQuantity.AddLast ((int)it.Quantity);
				}
				else
				{
					toRemoveItemids.AddLast (it.ItemId);
					toRemoveQuantity.AddLast (-1 * ((int)it.Quantity));
				}
			}

			// thanks onechord for noticing quests unnecessarily giving out "full inventory" from quests that also takes items from players
			return chr.AbstractPlayerInteraction.canHoldAllAfterRemoving (toAddItemids, toAddQuantity, toRemoveItemids, toRemoveQuantity);
		}

		private bool canGetItem (ItemData item, MapleCharacter chr)
		{
			if (item.Gender != 2 && item.Gender != chr.Gender)
			{
				return false;
			}

			if (item.job > 0)
			{
				//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
				//ORIGINAL LINE: final java.util.List<int> code = getJobBy5ByteEncoding(item.getJob());
				LinkedList<int> code = getJobBy5ByteEncoding (item.Job);
				bool jobFound = false;
				foreach (int codec in code)
				{
					if (codec / 100 == chr.Job.Id / 100)
					{
						jobFound = true;
						break;
					}
				}
				return jobFound;
			}

			return true;
		}

		public virtual bool restoreLostItem (MapleCharacter chr, int itemid)
		{
			if (!MapleItemInformationProvider.Instance.isQuestItem (itemid))
			{
				return false;
			}

			// thanks danielktran (MapleHeroesD)
			foreach (ItemData item in items)
			{
				if (item.Id == itemid)
				{
					int missingQty = item.Count - chr.countItem (itemid);
					if (missingQty > 0)
					{
						if (!chr.canHold (itemid, missingQty))
						{
							chr.dropMessage (1, "Please check if you have enough space in your inventory.");
							return false;
						}

						MapleInventoryManipulator.addById (chr.Client, item.Id, (short)missingQty);
						FilePrinter.print (FilePrinter.QUEST_RESTORE_ITEM, chr + " obtained " + itemid + " qty. " + missingQty + " from quest " + questID);
					}
					return true;
				}
			}

			return false;
		}*/

		private class ItemData
		{
			private readonly ItemAction outerInstance;

			internal readonly int map, id, count, job, gender, period;
			internal readonly int? prop;

			public ItemData (ItemAction outerInstance, int map, int id, int count, int? prop, int job, int gender, int period)
			{
				this.outerInstance = outerInstance;
				this.map = map;
				this.id = id;
				this.count = count;
				this.prop = prop;
				this.job = job;
				this.gender = gender;
				this.period = period;
			}

			public virtual int Id
			{
				get
				{
					return id;
				}
			}

			public virtual int Count
			{
				get
				{
					return count;
				}
			}

			public virtual int? Prop
			{
				get
				{
					return prop;
				}
			}

			public virtual int Job
			{
				get
				{
					return job;
				}
			}

			public virtual int Gender
			{
				get
				{
					return gender;
				}
			}

			public virtual int Period
			{
				get
				{
					return period;
				}
			}
		}
	}

}