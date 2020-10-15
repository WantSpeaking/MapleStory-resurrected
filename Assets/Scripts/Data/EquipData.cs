#define USE_NX

using System;
using ms.Helper;

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
	// Contains information about an equip
	public class EquipData : Cache<EquipData>
	{
		// Returns whether the equip was loaded correctly
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_valid() const
		public bool is_valid ()
		{
			return itemdata.is_valid ();
		}

		// Returns whether the equip was loaded correctly
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator bool() const
		public static implicit operator bool (EquipData ImpliedObject)
		{
			return ImpliedObject.is_valid ();
		}

		// Returns whether this equip has equipslot WEAPON
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_weapon() const
		public bool is_weapon ()
		{
			return eqslot == EquipSlot.Id.WEAPON;
		}

		// Returns a required base stat
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short get_reqstat(MapleStat::Id stat) const
		public short get_reqstat (MapleStat.Id stat)
		{
			return reqstats[stat];
		}

		// Returns a default stat
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short get_defstat(EquipStat::Id stat) const
		public short get_defstat (EquipStat.Id stat)
		{
			return defstats[stat];
		}

		// Returns the equip slot
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: EquipSlot::Id get_eqslot() const
		public EquipSlot.Id get_eqslot ()
		{
			return eqslot;
		}

		// Returns the category name
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const string& get_type() const
		public string get_type ()
		{
			return type;
		}

		// Returns the general item data (name, price, etc.)
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const ItemData& get_itemdata() const
		public ItemData get_itemdata ()
		{
			return itemdata;
		}

		readonly string[] types = new[] {"HAT", "FACE ACCESSORY", "EYE ACCESSORY", "EARRINGS", "TOP", "OVERALL", "BOTTOM", "SHOES", "GLOVES", "SHIELD", "CAPE", "RING", "PENDANT", "BELT", "MEDAL"};

		readonly EquipSlot.Id[] equipslots = {EquipSlot.Id.HAT, EquipSlot.Id.FACE, EquipSlot.Id.EYEACC, EquipSlot.Id.EARACC, EquipSlot.Id.TOP, EquipSlot.Id.TOP, EquipSlot.Id.BOTTOM, EquipSlot.Id.SHOES, EquipSlot.Id.GLOVES, EquipSlot.Id.SHIELD, EquipSlot.Id.CAPE, EquipSlot.Id.RING1, EquipSlot.Id.PENDANT1, EquipSlot.Id.BELT, EquipSlot.Id.MEDAL};

		readonly string[] weaponTypes = {"ONE-HANDED SWORD", "ONE-HANDED AXE", "ONE-HANDED MACE", "DAGGER", "", "", "", "WAND", "STAFF", "", "TWO-HANDED SWORD", "TWO-HANDED AXE", "TWO-HANDED MACE", "SPEAR", "POLEARM", "BOW", "CROSSBOW", "CLAW", "KNUCKLE", "GUN"};

		// Allow the cache to use the constructor
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C# has no concept of a 'friend' class:
//		friend Cache<EquipData>;
		// Load an equip from the game files
		public EquipData (int id)
		{
			this.itemdata = ItemData.get (id);
			string strid = "0" + Convert.ToString (id);
			string category = itemdata.get_category ();
			var src = nl.nx.wzFile_character[category][strid + ".img"]["info"];

			cash = src["cash"].GetInt ().ToBool ();
			tradeblock = src["tradeBlock"].GetInt ().ToBool ();
			slots = src["tuc"].GetShort ().ToByte ();

			reqstats[MapleStat.Id.LEVEL] = src["reqLevel"].GetShort ();
			reqstats[MapleStat.Id.JOB] = src["reqJob"].GetShort ();
			reqstats[MapleStat.Id.STR] = src["reqSTR"].GetShort ();
			reqstats[MapleStat.Id.DEX] = src["reqDEX"].GetShort ();
			reqstats[MapleStat.Id.INT] = src["reqINT"].GetShort ();
			reqstats[MapleStat.Id.LUK] = src["reqLUK"].GetShort ();
			defstats[EquipStat.Id.STR] = src["incSTR"].GetShort ();
			defstats[EquipStat.Id.DEX] = src["incDEX"].GetShort ();
			defstats[EquipStat.Id.INT] = src["incINT"].GetShort ();
			defstats[EquipStat.Id.LUK] = src["incLUK"].GetShort ();
			defstats[EquipStat.Id.WATK] = src["incPAD"].GetShort ();
			defstats[EquipStat.Id.WDEF] = src["incPDD"].GetShort ();
			defstats[EquipStat.Id.MAGIC] = src["incMAD"].GetShort ();
			defstats[EquipStat.Id.MDEF] = src["incMDD"].GetShort ();
			defstats[EquipStat.Id.HP] = src["incMHP"].GetShort ();
			defstats[EquipStat.Id.MP] = src["incMMP"].GetShort ();
			defstats[EquipStat.Id.ACC] = src["incACC"].GetShort ();
			defstats[EquipStat.Id.AVOID] = src["incEVA"].GetShort ();
			defstats[EquipStat.Id.HANDS] = src["incHANDS"].GetShort ();
			defstats[EquipStat.Id.SPEED] = src["incSPEED"].GetShort ();
			defstats[EquipStat.Id.JUMP] = src["incJUMP"].GetShort ();

			const uint NON_WEAPON_TYPES = 15;
			const uint WEAPON_OFFSET = NON_WEAPON_TYPES + 15;
			const uint WEAPON_TYPES = 20;
			uint index = (uint)((id / 10000) - 100);

			if (index < NON_WEAPON_TYPES)
			{
				type = weaponTypes[index];
				eqslot = equipslots[index];
			}
			else if (index >= WEAPON_OFFSET && index < WEAPON_OFFSET + WEAPON_TYPES)
			{
				uint weaponindex = index - WEAPON_OFFSET;
				type = weaponTypes[weaponindex];
				eqslot = EquipSlot.Id.WEAPON;
			}
			else
			{
				type = "CASH";
				eqslot = EquipSlot.Id.NONE;
			}
		}

		private readonly ItemData itemdata;

		private EnumMap<MapleStat.Id, short> reqstats = new EnumMap<MapleStat.Id, short> ();
		private EnumMap<EquipStat.Id, short> defstats = new EnumMap<EquipStat.Id, short> ();
		private string type;
		private EquipSlot.Id eqslot;
		private byte slots;
		private bool cash;
		private bool tradeblock;
	}
}

#if USE_NX
#endif