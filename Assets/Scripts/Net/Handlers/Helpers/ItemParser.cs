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
	public class ItemParser
	{
		// Parse a normal item from a packet
		public static void add_item (InPacket recv, InventoryType.Id invtype, short slot, int id, Inventory inventory)
		{
			// Read all item stats
			bool cash = recv.read_bool ();

			if (cash)
				recv.skip (8); // unique id

			long expire = recv.read_long ();
			short count = recv.read_short ();
			string owner = recv.read_string ();
			short flag = recv.read_short ();

			// If the item is a rechargeable projectile, some additional bytes are sent.
			if ((id / 10000 == 233) || (id / 10000 == 207))
				recv.skip (8);

			inventory.add_item (invtype, slot, id, cash, expire, (ushort)count, owner, flag);
		}

		// Parse a pet from a packet
		public static void add_pet (InPacket recv, InventoryType.Id invtype, short slot, int id, Inventory inventory)
		{
			// Read all pet stats
			bool cash = recv.read_bool ();

			if (cash)
				recv.skip (8); // unique id

			long expire = recv.read_long ();
			string petname = recv.read_padded_string (13);
			sbyte petlevel = recv.read_byte ();
			short closeness = recv.read_short ();
			sbyte fullness = recv.read_byte ();

			// Some unused bytes
			recv.skip (18);

			inventory.add_pet (invtype, slot, id, cash, expire, petname, petlevel, closeness, fullness);
		}

		// Parse an equip from a packet
		public static void add_equip (InPacket recv, InventoryType.Id invtype, short slot, int id, Inventory inventory)
		{
			// Read equip information
			bool cash = recv.read_bool ();

			if (cash)
				recv.skip (8); // unique id

			long expire = recv.read_long ();
			var slots = recv.read_byte ();
			var level = recv.read_byte ();

			// Read equip stats
			EnumMapNew<EquipStat.Id, ushort> stats = new EnumMapNew<EquipStat.Id, ushort> ();

			foreach (var iter in stats)
				stats[iter.Key] = (ushort)recv.read_short ();

			// Some more information
			string owner = recv.read_string ();
			short flag = recv.read_short ();
			var itemlevel = 0;
			ushort itemexp = 0;
			int vicious = 0;

			if (cash)
			{
				// Some unused bytes
				recv.skip (10);
			}
			else
			{
				recv.read_byte ();
				itemlevel = recv.read_byte ();
				recv.read_short ();
				itemexp = (ushort)recv.read_short ();
				vicious = recv.read_int ();
				recv.read_long ();
			}

			recv.skip (12);

			if (slot < 0)
			{
				invtype = InventoryType.Id.EQUIPPED;
				slot = (short)-slot;
			}

			inventory.add_equip (invtype, slot, id, cash, expire, (byte)slots, (byte)level, stats, owner, flag, (byte)itemlevel, itemexp, vicious);
		}

		public static void parse_item (InPacket recv, InventoryType.Id invtype, short slot, Inventory inventory)
		{
			// Read type and item id
			recv.read_byte (); // 'type' byte
			int iid = recv.read_int ();

			if (invtype == InventoryType.Id.EQUIP || invtype == InventoryType.Id.EQUIPPED)
			{
				// Parse an equip
				add_equip (recv, invtype, slot, iid, inventory);
			}
			else if (iid >= 5000000 && iid <= 5000102)
			{
				// Parse a pet
				add_pet (recv, invtype, slot, iid, inventory);
			}
			else
			{
				// Parse a normal item
				add_item (recv, invtype, slot, iid, inventory);
			}
		}
	}
}