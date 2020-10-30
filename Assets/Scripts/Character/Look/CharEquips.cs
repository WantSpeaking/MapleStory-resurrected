using System.Collections.Generic;
using UnityEngine;

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
	// A characters equipment (The visual part)
	public class CharEquips
	{
		// Cap types (vslot)
		public enum CapType
		{
			NONE,
			HEADBAND,
			HAIRPIN,
			HALFCOVER,
			FULLCOVER
		}

		// Initialize pointers with zero
		public CharEquips ()
		{
			/*foreach (var iter in clothes)
			{
				iter.Value = null;
			}*/
		}

		// Draw an equip
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(EquipSlot::Id slot, Stance::Id stance, Clothing::Layer layer, byte frame, const DrawArgument& args) const
		public void draw (EquipSlot.Id slot, Stance.Id stance, Clothing.Layer layer, byte frame, DrawArgument args)
		{
			
			var cloth = clothes[slot];
			if (stance.ToString ().Contains ("STAND"))
				Debug.Log ($"draw CharEquips: slot:{slot}\t stance:{stance}\t layer:{layer}\t frame:{frame}\t  cloth:{cloth}");
			//if (const Clothing * cloth = clothes[(int)slot])
			{
				cloth?.draw (stance, layer, frame, args);
			}
		}

		// Add an equip, if not in cache, the equip is created from the files.
		public void add_equip (int itemid, BodyDrawInfo drawinfo)
		{
			if (itemid <= 0)
			{
				return;
			}

			if (!cloth_cache.TryGetValue (itemid, out var cloth))
			{
				cloth = new Clothing (itemid, drawinfo);
				cloth_cache.Add (itemid, cloth);
			}

			EquipSlot.Id slot = cloth.get_eqslot ();
			clothes[slot] = cloth;

			/*var iter = cloth_cache.find(itemid);

			if (iter == cloth_cache.end())
			{
				iter = cloth_cache.emplace(piecewise_construct, forward_as_tuple(itemid), forward_as_tuple(itemid, drawinfo)).first;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			Clothing cloth = iter.second;

			EquipSlot.Id slot = cloth.get_eqslot();
			clothes[(int)slot] = cloth;*/
		}

		// Remove an equip
		public void remove_equip (EquipSlot.Id slot)
		{
			clothes[slot] = null;
		}

		// Check if an equip is visible
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_visible(EquipSlot::Id slot) const
		public bool is_visible (EquipSlot.Id slot)
		{
			if (cloth_cache.TryGetValue ((int)slot, out var cloth))
			{
				return cloth.is_transparent () == false;
			}
			else
			{
				return false;
			}
		}

		// Check if the equip at the specified slot in the specified stance contains a part on the specified layer
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool comparelayer(EquipSlot::Id slot, Stance::Id stance, Clothing::Layer layer) const
		public bool comparelayer (EquipSlot.Id slot, Stance.Id stance, Clothing.Layer layer)
		{
			if (cloth_cache.TryGetValue ((int)slot, out var cloth))
			{
				return cloth.contains_layer (stance, layer);
			}
			else
			{
				return false;
			}
		}

		// Return if there is an overall equipped
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool has_overall() const
		public bool has_overall ()
		{
			return get_equip (EquipSlot.Id.TOP) / 10000 == 105;
		}

		// Return if there is a weapon equipped
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool has_weapon() const
		public bool has_weapon ()
		{
			return get_weapon () != 0;
		}

		// Return whether the equipped weapon is twohanded
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_twohanded() const
		public bool is_twohanded ()
		{
			if (cloth_cache.TryGetValue ((int)EquipSlot.Id.WEAPON, out var weapon))
			{
				return weapon.is_twohanded ();
			}
			else
			{
				return false;
			}

			/*if (const Clothing * weapon = clothes[(int)EquipSlot.Id.WEAPON])
			{
				return weapon.is_twohanded();
			}
			else
			{
				return false;
			}*/
		}

		// Return the cap type (vslot)
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: CharEquips::CapType getcaptype() const
		public CharEquips.CapType getcaptype ()
		{
			if (cloth_cache.TryGetValue ((int)EquipSlot.Id.HAT, out var cap))
			{
				string vslot = cap.get_vslot ();
				if (vslot == "CpH1H5")
				{
					return CharEquips.CapType.HALFCOVER;
				}
				else if (vslot == "CpH1H5AyAs")
				{
					return CharEquips.CapType.FULLCOVER;
				}
				else if (vslot == "CpH5")
				{
					return CharEquips.CapType.HEADBAND;
				}
				else
				{
					return CharEquips.CapType.NONE;
				}
			}
			else
			{
				return CharEquips.CapType.NONE;
			}
		}

		// Return a stance which has been adjusted to the equipped weapon type
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Stance::Id adjust_stance(Stance::Id stance) const
		public Stance.Id adjust_stance (Stance.Id stance)
		{
			if (cloth_cache.TryGetValue ((int)EquipSlot.Id.WEAPON, out var weapon))
			{
				switch (stance)
				{
					case Stance.Id.STAND1:
					case Stance.Id.STAND2:
						return weapon.get_stand ();
					case Stance.Id.WALK1:
					case Stance.Id.WALK2:
						return weapon.get_walk ();
					default:
						return stance;
				}
			}
			else
			{
				return stance;
			}
		}

		// Return the item id of the equip at the specified slot
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_equip(EquipSlot::Id slot) const
		public int get_equip (EquipSlot.Id slot)
		{
			if (cloth_cache.TryGetValue ((int)slot, out var cloth))
			{
				return cloth.get_id ();
			}
			else
			{
				return 0;
			}
		}

		// Return the item id of the equipped weapon
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_weapon() const
		public int get_weapon ()
		{
			return get_equip (EquipSlot.Id.WEAPON);
		}

		private readonly EnumMap<EquipSlot.Id, Clothing> clothes = new EnumMap<EquipSlot.Id, Clothing> ();

		private static Dictionary<int, Clothing> cloth_cache = new Dictionary<int, Clothing> ();
	}
}