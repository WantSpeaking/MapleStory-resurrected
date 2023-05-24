using System;
using System.Collections.Generic;





namespace ms
{
	// A characters equipment (The visual part)
	public class CharEquips:IDisposable
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
		public void draw (EquipSlot.Id slot, Stance.Id stance, Clothing.Layer layer, byte frame, DrawArgument args, bool drawOrErase = true)
		{
			var cloth = clothes[slot];
			//if (stance.ToString ().Contains ("WALK"))
			//AppDebug.Log ($"draw CharEquips: slot:{slot}\t stance:{stance}\t layer:{layer}\t frame:{frame}\t  cloth:{cloth}");
			{
				cloth?.draw (stance, layer, frame, args, drawOrErase);
			}
		}

		// Add an equip, if not in cache, the equip is created from the files.
		public void add_equip (int itemid, BodyDrawInfo drawinfo)
		{
			if (itemid <= 0)
			{
				return;
			}

            /*if (!cloth_cache.TryGetValue (itemid, out var cloth))
			{
				cloth = new Clothing (itemid, drawinfo);
				cloth_cache.Add (itemid, cloth);
			}*/
            var cloth = new Clothing(itemid, drawinfo);
            EquipSlot.Id slot = cloth.get_eqslot ();
			clothes[slot] = cloth;

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
		public Stance.Id adjust_stance (Stance.Id stance)
		{
			if (clothes.TryGetValue (EquipSlot.Id.WEAPON, out var weapon))
			{
				switch (stance)
				{
					case Stance.Id.STAND1:
					case Stance.Id.STAND2:
						return weapon?.get_stand () ?? Stance.Id.STAND1;
					case Stance.Id.WALK1:
					case Stance.Id.WALK2:
						return weapon?.get_walk () ?? Stance.Id.WALK1;
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
		public int get_equip (EquipSlot.Id slot)
		{
			if (clothes.TryGetValue (slot, out var cloth) && cloth != null)
			{
				return cloth.get_id ();
			}
			else
			{
				return 0;
			}
		}

		// Return the item id of the equipped weapon
		public int get_weapon ()
		{
			return get_equip (EquipSlot.Id.WEAPON);
		}

		private readonly EnumMap<EquipSlot.Id, Clothing> clothes = new EnumMap<EquipSlot.Id, Clothing> ();

		private static Dictionary<int, Clothing> cloth_cache = new Dictionary<int, Clothing> ();
		public void Dispose ()
		{
			foreach (var pair in clothes)
			{
				pair.Value?.Dispose ();
			}
			//clothes.Clear ();
		}
	}
}