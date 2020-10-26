using System.Collections.Generic;

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
	// Handler for a packet which signifies that inventory items were gathered
	public class GatherResultHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle (InPacket recv)
		{
			var iteminventory = UI.get ().get_element<UIItemInventory> ();
			if (iteminventory != null)
			{
				iteminventory.get ().set_sort (true);
			}
		}
	}

	// Handler for a packet which signifies that inventory items were sorted
	public class SortResultHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle (InPacket recv)
		{
			var iteminventory = UI.get ().get_element<UIItemInventory> ();
			if (iteminventory != null)
			{
				iteminventory.get ().set_sort (false);
				iteminventory.get ().clear_new ();
			}
		}
	}

	struct Mod
	{
		public sbyte mode;
		public InventoryType.Id type;
		public short pos;
		public short arg;
	}


	// Handler for a packet which modifies the player's inventory
	public class ModifyInventoryHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle (InPacket recv)
		{
			recv.read_bool (); // 'updatetick'

			Inventory inventory = Stage.get ().get_player ().get_inventory ();

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C# does not allow declaring types within methods:


			List<Mod> mods = new List<Mod> ();

			sbyte size = recv.read_byte ();

			for (sbyte i = 0; i < size; i++)
			{
				Mod mod = new Mod ();
				mod.mode = recv.read_byte ();
				mod.type = InventoryType.by_value (recv.read_byte ());
				mod.pos = recv.read_short ();

				mod.arg = 0;
				var keyconfig = UI.get ().get_element<UIKeyConfig> ();

				switch ((Inventory.Modification)mod.mode)
				{
					case Inventory.Modification.ADD:
						ItemParser.parse_item (recv, mod.type, mod.pos, inventory);
						if (keyconfig != null)
						{
							short count_now = inventory.get_item_count (mod.type, mod.pos);
							keyconfig.get ().update_item_count (mod.type, mod.pos, count_now);
						}

						break;
					case Inventory.Modification.CHANGECOUNT:
					{
						mod.arg = recv.read_short ();

						short count_before = inventory.get_item_count (mod.type, mod.pos);
						short count_now = mod.arg;

						inventory.modify (mod.type, mod.pos, mod.mode, mod.arg, Inventory.Movement.MOVE_NONE);

						if (keyconfig != null)
						{
							keyconfig.get ().update_item_count (mod.type, mod.pos, (short)(count_now - count_before));
						}

						if (count_before < count_now)
						{
							mod.mode = (sbyte)Inventory.Modification.ADDCOUNT;
						}
					}
						break;
					case Inventory.Modification.SWAP:
						mod.arg = recv.read_short ();
						break;
					case Inventory.Modification.REMOVE:
						if (keyconfig != null)
						{
							short count_before = inventory.get_item_count (mod.type, mod.pos);
							keyconfig.get ().update_item_count (mod.type, mod.pos, (short)(-1 * count_before));
						}

						inventory.modify (mod.type, mod.pos, mod.mode, mod.arg, Inventory.Movement.MOVE_INTERNAL);
						break;
				}

				mods.Add (mod);
			}

			Inventory.Movement move = (recv.length () > 0) ? Inventory.movementbyvalue (recv.read_byte ()) : Inventory.Movement.MOVE_INTERNAL;

			foreach (Mod mod in mods)
			{
				if (mod.mode == 2)
				{
					inventory.modify (mod.type, mod.pos, mod.mode, mod.arg, move);
				}

				var shop = UI.get ().get_element<UIShop> ();
				if (shop!=null)
				{
					shop.get ().modify (mod.type);
				}

				var eqinvent = UI.get ().get_element<UIEquipInventory> ();
				var itinvent = UI.get ().get_element<UIItemInventory> ();

				switch (move)
				{
					case Inventory.Movement.MOVE_INTERNAL:
						switch (mod.type)
						{
							case InventoryType.Id.EQUIPPED:
								if (eqinvent != null)
								{
									eqinvent.Dereference ().modify (mod.pos, mod.mode, mod.arg);
								}

								Stage.get ().get_player ().change_equip ((short)-mod.pos);
								Stage.get ().get_player ().change_equip ((short)-mod.arg);
								break;
							case InventoryType.Id.EQUIP:
							case InventoryType.Id.USE:
							case InventoryType.Id.SETUP:
							case InventoryType.Id.ETC:
							case InventoryType.Id.CASH:
								if (itinvent != null)
								{
									itinvent.Dereference ().modify (mod.type, mod.pos, mod.mode, mod.arg);
								}

								break;
						}

						break;
					case Inventory.Movement.MOVE_EQUIP:
					case Inventory.Movement.MOVE_UNEQUIP:
						if (mod.pos < 0)
						{
							if (eqinvent != null)
							{
								eqinvent.Dereference ().modify ((short)-mod.pos, 3, 0);
							}

							if (itinvent != null)
							{
								itinvent.Dereference ().modify (InventoryType.Id.EQUIP, mod.arg, mod.mode, 0);
							}

							Stage.get ().get_player ().change_equip ((short)-mod.pos);
						}
						else if (mod.arg < 0)
						{
							if (eqinvent != null)
							{
								eqinvent.Dereference ().modify ((short)-mod.arg, 0, 0);
							}

							if (itinvent != null)
							{
								itinvent.Dereference ().modify (InventoryType.Id.EQUIP, mod.pos, (sbyte)Inventory.Modification.REMOVE, 0);
							}

							Stage.get ().get_player ().change_equip ((short)-mod.arg);
						}

						break;
				}
			}

			Stage.get ().get_player ().recalc_stats (true);
			UI.get ().enable ();
		}
	}
}