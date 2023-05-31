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
	// Packet which requests that the inventory is sorted
	// Opcode: GATHER_ITEMS(69)
	public class GatherItemsPacket : OutPacket
	{
		public GatherItemsPacket (InventoryType.Id type) : base ((short)OutPacket.Opcode.GATHER_ITEMS)
		{
			write_time ();
			write_byte ((sbyte)type);
		}
	}

	// Packet which requests that the inventory is sorted
	// Opcode: SORT_ITEMS(70)
	public class SortItemsPacket : OutPacket
	{
		public SortItemsPacket (InventoryType.Id type) : base ((short)OutPacket.Opcode.SORT_ITEMS)
		{
			write_time ();
			write_byte ((sbyte)type);
		}
	}

	// Packet which requests that an item is moved
	// Opcode: MOVE_ITEM(71)
	public class MoveItemPacket : OutPacket
	{
		public MoveItemPacket (InventoryType.Id type, short slot, short action, short qty) : base ((short)OutPacket.Opcode.MOVE_ITEM)
		{
			write_time ();
			write_byte ((sbyte)type);
			write_short (slot);
			write_short (action);
			write_short (qty);
		}
	}

	// Packet which requests that an item is equipped
	// Opcode: MOVE_ITEM(71)
	public class EquipItemPacket : MoveItemPacket
	{
		public EquipItemPacket (short src, EquipSlot.Id dest) : base (InventoryType.Id.EQUIP, src, (short)(-(short)dest), 1)
		{
		}
	}

	// Packet which requests that an item is unequipped
	// Opcode: MOVE_ITEM(71)
	public class UnequipItemPacket : MoveItemPacket
	{
		public UnequipItemPacket (short src, short dest) : base (InventoryType.Id.EQUIPPED, (short)-src, dest, 1)
		{
		}
	}

	// A packet which requests that an 'USE' item is used
	// Opcode: USE_ITEM(72)
	public class UseItemPacket : OutPacket
	{
		public UseItemPacket (short slot, int itemid) : base ((short)OutPacket.Opcode.USE_ITEM)
		{
			new Sound (itemid).play ();

			write_time ();
			write_short (slot);
			write_int (itemid);
		}
	}

	// Requests using a scroll on an equip 
	// Opcode: SCROLL_EQUIP(86)
	public class ScrollEquipPacket : OutPacket
	{
		public enum Flag : byte
		{
			NONE = 0x00,
			UNKNOWN = 0x01,
			WHITESCROLL = 0x02
		}

		public ScrollEquipPacket (short scrollSlot, short equipSlot, bool isEquipped, byte flags) : base ((short)OutPacket.Opcode.USE_UPGRADE_SCROLL)
		{
			write_time ();
			write_short (scrollSlot);
			write_short ((short)(equipSlot*(isEquipped?-1:1)));
			write_short (flags);
		}

		public ScrollEquipPacket (short scrollSlot, short equipSlot, bool isEquipped) : this (scrollSlot, equipSlot, isEquipped,0)
		{
		}
	}
}