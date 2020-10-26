using System;
using System.Collections.Generic;
using System.Linq;

//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////


namespace ms
{
	// The player's inventory
	public class Inventory
	{
		public enum Movement : sbyte
		{
			MOVE_NONE = -1,
			MOVE_INTERNAL = 0,
			MOVE_UNEQUIP = 1,
			MOVE_EQUIP = 2
		}

		public enum Modification : sbyte
		{
			ADD,
			CHANGECOUNT,
			SWAP,
			REMOVE,
			ADDCOUNT
		}

		// Return the move type by value
		public static Inventory.Movement movementbyvalue (sbyte value)
		{
			if (value >= ((int)Inventory.Movement.MOVE_INTERNAL) && value <= (int)Inventory.Movement.MOVE_EQUIP)
			{
				return (Movement)value;
			}

			Console.Write ("Unknown Inventory::Movement value: [");
			Console.Write (value);
			Console.Write ("]");
			Console.Write ("\n");

			return Inventory.Movement.MOVE_NONE;
		}

		public Inventory ()
		{
			bulletslot = 0;
			meso = 0;
			running_uid = 0;
			slotmaxima[InventoryType.Id.EQUIPPED] = (byte)EquipSlot.Id.LENGTH;
		}

		// Recalculate sums of equip stats
		public void recalc_stats (Weapon.Type type)
		{
			totalstats.Clear ();

			foreach (var iter in inventories[InventoryType.Id.EQUIPPED])
			{
				if (equips.TryGetValue (iter.Value.unique_id, out var equip))
				{
					foreach (var stat_iter in totalstats)
					{
						//var old_State = stat_iter.Value;
						//var new_State = old_State + equip.get_stat (stat_iter.Key);
						//totalstats[(Id)iter.Value.unique_id] = (ushort)new_State;
						totalstats[(EquipStat.Id)iter.Value.unique_id] += equip.get_stat (stat_iter.Key);
						//stat_iter.Value = new_State;
						//stat_iter.Value += equip.get_stat(stat_iter.Key);
					}
				}

				/*var equip_iter = equips.find (iter.Value.unique_id);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
				if (equip_iter != equips.end ())
				{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
					Equip equip = equip_iter.second;

					foreach (var stat_iter in totalstats)
					{
						stat_iter.Value += equip.get_stat (stat_iter.Key);
					}
				}*/
			}

			int prefix;

			switch (type)
			{
				case Weapon.Type.BOW:
					prefix = 2060;
					break;
				case Weapon.Type.CROSSBOW:
					prefix = 2061;
					break;
				case Weapon.Type.CLAW:
					prefix = 2070;
					break;
				case Weapon.Type.GUN:
					prefix = 2330;
					break;
				default:
					prefix = 0;
					break;
			}

			bulletslot = 0;

			if (prefix != 0)
			{
				foreach (var iter in inventories[InventoryType.Id.USE])
				{
					Slot slot = iter.Value;

					if (slot.count != 0 && slot.item_id / 1000 == prefix)
					{
						bulletslot = iter.Key;
						break;
					}
				}
			}

			int bulletid = get_bulletid ();
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
			if (bulletid != 0)
			{
				totalstats[EquipStat.Id.WATK] += (ushort)BulletData.get (bulletid).get_watk ();
			}
		}

		// Set the meso amount
		public void set_meso (long m)
		{
			meso = m;
		}

		// Set the number of slots for a given inventory
		public void set_slotmax (InventoryType.Id type, byte slotmax)
		{
			slotmaxima[type] = slotmax;
		}

		// Modify the inventory with info from a packet
		public void modify (InventoryType.Id type, short slot, sbyte mode, short arg, Movement move)
		{
			if (slot < 0)
			{
				slot = (short)-slot;
				type = InventoryType.Id.EQUIPPED;
			}

			arg = (short)((arg < 0) ? -arg : arg);

			switch ((Modification)mode)
			{
				case Modification.CHANGECOUNT:
					change_count (type, slot, arg);
					break;
				case Modification.SWAP:
					switch (move)
					{
						case Movement.MOVE_INTERNAL:
							swap (type, slot, type, arg);
							break;
						case Movement.MOVE_UNEQUIP:
							swap (InventoryType.Id.EQUIPPED, slot, InventoryType.Id.EQUIP, arg);
							break;
						case Movement.MOVE_EQUIP:
							swap (InventoryType.Id.EQUIP, slot, InventoryType.Id.EQUIPPED, arg);
							break;
					}

					break;
				case Modification.REMOVE:
					remove (type, slot);
					break;
			}
		}

		// Add a general item
		public void add_item (InventoryType.Id invtype, short slot, int item_id, bool cash, long expire, ushort count, string owner, short flags)
		{
			items.Add (add_slot (invtype, slot, item_id, (short)count, cash), new Item (item_id, expire, owner, flags));
		}

		// Add a pet item
		public void add_pet (InventoryType.Id invtype, short slot, int item_id, bool cash, long expire, string name, sbyte level, short closeness, sbyte fullness)
		{
			pets.Add (add_slot (invtype, slot, item_id, 1, cash), new Pet (item_id, expire, name, (byte)level, (ushort)closeness, (byte)fullness));
		}

		// Add an equip item
		public void add_equip (InventoryType.Id invtype, short slot, int item_id, bool cash, long expire, byte slots, byte level, EnumMap<EquipStat.Id, ushort> stats, string owner, short flag, byte ilevel, ushort iexp, int vicious)
		{
			equips.Add (add_slot (invtype, slot, item_id, 1, cash), new Equip (item_id, expire, owner, flag, slots, level, stats, ilevel, (short)iexp, vicious));
		}

		// Check if the use inventory contains at least one projectile
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool has_projectile() const
		public bool has_projectile ()
		{
			return bulletslot > 0;
		}

		// Return if an equip is equipped in the specified slot
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool has_equipped(EquipSlot::Id slot) const
		public bool has_equipped (EquipSlot.Id slot)
		{
			return inventories[InventoryType.Id.EQUIPPED].Any (pair => pair.Key == (short)slot);
		}

		// Return the currently active projectile slot
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short get_bulletslot() const
		public short get_bulletslot ()
		{
			return bulletslot;
		}

		// Return the count of the currently active projectile
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ushort get_bulletcount() const
		public ushort get_bulletcount ()
		{
			return (ushort)get_item_count (InventoryType.Id.USE, bulletslot);
		}

		// Return the itemid of the currently active projectile
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_bulletid() const
		public int get_bulletid ()
		{
			return get_item_id (InventoryType.Id.USE, bulletslot);
		}

		// Return the number of slots for the specified inventory
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: byte get_slotmax(InventoryType::Id type) const
		public byte get_slotmax (InventoryType.Id type)
		{
			return slotmaxima[type];
		}

		// Return a total stat
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ushort get_stat(EquipStat::Id type) const
		public ushort get_stat (EquipStat.Id type)
		{
			return totalstats[type];
		}

		// Return the amount of meso
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: long get_meso() const
		public long get_meso ()
		{
			return meso;
		}

		// Find a free slot for the specified equip
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: EquipSlot::Id find_equipslot(int itemid) const
		public EquipSlot.Id find_equipslot (int itemid)
		{
			EquipData cloth = EquipData.get (itemid);

			if (!cloth.is_valid ())
			{
				return EquipSlot.Id.NONE;
			}

			EquipSlot.Id eqslot = cloth.get_eqslot ();

			if (eqslot == EquipSlot.Id.RING1)
			{
				if (!has_equipped (EquipSlot.Id.RING2))
				{
					return EquipSlot.Id.RING2;
				}

				if (!has_equipped (EquipSlot.Id.RING3))
				{
					return EquipSlot.Id.RING3;
				}

				if (!has_equipped (EquipSlot.Id.RING4))
				{
					return EquipSlot.Id.RING4;
				}

				return EquipSlot.Id.RING1;
			}
			else
			{
				return eqslot;
			}
		}

		// Find a free slot in the specified inventory
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short find_free_slot(InventoryType::Id type) const
		public short find_free_slot (InventoryType.Id type)
		{
			short counter = 1;

			foreach (var iter in inventories[type])
			{
				if (iter.Key != counter)
				{
					return counter;
				}

				counter++;
			}

			return (short)(counter <= slotmaxima[type] ? counter : 0);
		}

		// Return the first slot which contains the specified item
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short find_item(InventoryType::Id type, int itemid) const
		public short find_item (InventoryType.Id type, int itemid)
		{
			foreach (var iter in inventories[type])
			{
				if (iter.Value.item_id == itemid)
				{
					return iter.Key;
				}
			}

			return 0;
		}

		// Return the count of an item
		// Returns zero if the slot is empty
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short get_item_count(InventoryType::Id type, short slot) const
		public short get_item_count (InventoryType.Id type, short slot)
		{
			if (inventories[type].TryGetValue (slot, out var slotItem))
			{
				return slotItem.count;
			}
			else
			{
				return 0;
			}
		}

		// Return the total count of an item
		// Returns zero if no instances of the item was found
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short get_total_item_count(int itemid) const
		public short get_total_item_count (int itemid)
		{
			InventoryType.Id type = InventoryType.by_item_id (itemid);

			short total_count = 0;

			foreach (var iter in inventories[type])
			{
				if (iter.Value.item_id == itemid)
				{
					total_count += iter.Value.count;
				}
			}

			return total_count;
		}

		// Return the id of an item
		// Returns zero if the slot is empty
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_item_id(InventoryType::Id type, short slot) const
		public int get_item_id (InventoryType.Id type, short slot)
		{
			if (inventories[type].TryGetValue (slot, out var slotItem))
			{
				return slotItem.item_id;
			}
			else
			{
				return 0;
			}
		}

		// Return a pointer to an equip
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Optional<const Equip> get_equip(InventoryType::Id type, short slot) const
		public Optional<Equip> get_equip (InventoryType.Id type, short slot)
		{
			if (type != InventoryType.Id.EQUIPPED && type != InventoryType.Id.EQUIP)
			{
				return new Optional<Equip> ();
			}

			if (!inventories[type].TryGetValue (slot, out var slotItem))
			{
				return new Optional<Equip> ();
			}
			else
			{
				if (!equips.TryGetValue (slotItem.unique_id, out var equipItem))
				{
					return new Optional<Equip> ();
				}
				else
				{
					return equipItem;
				}
			}
		}

		// Add an inventory slot and return the unique_id
		private int add_slot (InventoryType.Id type, short slot, int item_id, short count, bool cash)
		{
			running_uid++;
			inventories[type][slot] = new Slot (running_uid, item_id, count, cash);
			return running_uid;
		}

		// Change the quantity of an item
		private void change_count (InventoryType.Id type, short slot, short count)
		{
			if (inventories[type].TryGetValue (slot, out var slotItem))
			{
				var temp_Slot = inventories[type][slot];
				temp_Slot.count = count;
				inventories[type][slot] = temp_Slot;
			}
		}

		// Swap two items
		private void swap (InventoryType.Id firsttype, short firstslot, InventoryType.Id secondtype, short secondslot)
		{
			Slot first = inventories[firsttype][firstslot];
			inventories[firsttype][firstslot] = inventories[secondtype][secondslot];
			inventories[secondtype][secondslot] = first;

			if (inventories[firsttype][firstslot].item_id == 0)
			{
				remove (firsttype, firstslot);
			}

			if (inventories[secondtype][secondslot].item_id == 0)
			{
				remove (secondtype, secondslot);
			}
		}

		// Remove an item
		private void remove (InventoryType.Id type, short slot)
		{
			if (!inventories[type].TryGetValue (slot,out var slotItem))
			{
				return;
			}

			int unique_id = slotItem.unique_id;
			inventories[type].Remove (slot);

			switch (type)
			{
				case InventoryType.Id.EQUIPPED:
				case InventoryType.Id.EQUIP:
					equips.Remove (unique_id);
					break;
				case InventoryType.Id.CASH:
					items.Remove (unique_id);
					pets.Remove (unique_id);
					break;
				default:
					items.Remove (unique_id);
					break;
			}
		}

		private struct Slot
		{
			public Slot (int uniqueID, int itemID, short count, bool cash)
			{
				unique_id = uniqueID;
				item_id = itemID;
				this.count = count;
				this.cash = cash;
			}

			public int unique_id;
			public int item_id;
			public short count;
			public bool cash;
		}

		private EnumMapNew<InventoryType.Id, SortedDictionary<short, Slot>> inventories = new EnumMapNew<InventoryType.Id, SortedDictionary<short, Slot>> ();
		private Dictionary<int, Item> items = new Dictionary<int, Item> ();
		private Dictionary<int, Equip> equips = new Dictionary<int, Equip> ();
		private Dictionary<int, Pet> pets = new Dictionary<int, Pet> ();
		private int running_uid;

		private EnumMap<EquipStat.Id, ushort> totalstats = new EnumMap<EquipStat.Id, ushort> ();
		private EnumMap<InventoryType.Id, byte> slotmaxima = new EnumMap<InventoryType.Id, byte> ();
		private long meso;
		private short bulletslot;
	}
}