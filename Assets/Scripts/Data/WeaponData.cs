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
	// Contains information about a weapon
	public class WeaponData : Cache<WeaponData>
	{
		// Returns whether the weapon was loaded correctly
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_valid() const
		public bool is_valid()
		{
			return equipdata.is_valid();
		}
		// Returns whether the weapon was loaded correctly
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator bool() const
		public static implicit operator bool(WeaponData ImpliedObject)
		{
			return ImpliedObject.is_valid();
		}

		// Return whether this weapon uses two-handed stances
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_twohanded() const
		public bool is_twohanded()
		{
			return twohanded;
		}
		// Return the attack speed
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: byte get_speed() const
		public byte get_speed()
		{
			return attackspeed;
		}
		// Return the attack type
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: byte get_attack() const
		public byte get_attack()
		{
			return attack;
		}
		// Return the speed as displayed in a Tooltip
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string getspeedstring() const
		public string getspeedstring()
		{
			switch (attackspeed)
			{
			case 1:
				return "FAST (1)";
			case 2:
				return "FAST (2)";
			case 3:
				return "FAST (3)";
			case 4:
				return "FAST (4)";
			case 5:
				return "NORMAL (5)";
			case 6:
				return "NORMAL (6)";
			case 7:
				return "SLOW (7)";
			case 8:
				return "SLOW (8)";
			case 9:
				return "SLOW (9)";
			default:
				return "";
			}
		}
		// Return the attack delay
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: byte get_attackdelay() const
		public byte get_attackdelay()
		{
			if (type == Weapon.Type.NONE)
			{
				return 0;
			}
			else
			{
				return (byte)(50 - 25 / attackspeed);
			}
		}
		// Return the weapon type
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Weapon::Type get_type() const
		public Weapon.Type get_type()
		{
			return type;
		}
		// Return the sound to play when attacking
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Sound get_usesound(bool degenerate) const
		/*public Sound get_usesound(bool degenerate)//todo sound
		{
			return usesounds[degenerate];
		}*/
		// Return the name of the afterimage
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const string& get_afterimage() const
		public string get_afterimage()
		{
			return afterimage;
		}
		// Return the general equip data
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const EquipData& get_equipdata() const
		public EquipData get_equipdata()
		{
			return equipdata;
		}

		// Allow the cache to use the constructor
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C# has no concept of a 'friend' class:
//		friend Cache<WeaponData>;
		// Load a weapon from the game files
		private WeaponData(int equipid)
		{
			this.equipdata = EquipData.get(equipid);
			int prefix = equipid / 10000;
			type = Weapon.by_value(prefix);
			twohanded = prefix == (int)Weapon.Type.STAFF || (prefix >= ((int)Weapon.Type.SWORD_2H) && prefix <= (int)Weapon.Type.POLEARM) || prefix == (int)Weapon.Type.CROSSBOW;

			var src = nl.nx.wzFile_character["Weapon"]["0" + Convert.ToString(equipid) + ".img"]["info"];

			attackspeed = (byte)src["attackSpeed"].GetShort ().ToByte ();
			attack = (byte)src["attack"].GetShort ().ToByte ();

			var soundsrc = nl.nx.wzFile_sound["Weapon.img"][src["sfx"].ToString ()];

			/*bool twosounds = soundsrc["Attack2"].data_type() == nl.node.type.audio;//todo twosounds

			if (twosounds)
			{
				usesounds[false] = soundsrc["Attack"];
				usesounds[true] = soundsrc["Attack2"];
			}
			else
			{
				usesounds[false] = soundsrc["Attack"];
				usesounds[true] = soundsrc["Attack"];
			}*/

			afterimage = src["afterImage"].ToString ();
		}

		private readonly EquipData equipdata;

		private Weapon.Type type;
		private bool twohanded;
		private byte attackspeed;
		private byte attack;
		//private BoolPair<Sound> usesounds = new BoolPair<Sound>();
		private string afterimage;
	}
}

#if USE_NX
#endif
