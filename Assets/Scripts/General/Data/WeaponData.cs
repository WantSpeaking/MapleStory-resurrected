#define USE_NX

using System;
using MapleLib.WzLib;






namespace ms
{
	// Contains information about a weapon
	public class WeaponData : Cache<WeaponData>
	{
		// Returns whether the weapon was loaded correctly
		public bool is_valid()
		{
			return equipdata.is_valid();
		}
		// Returns whether the weapon was loaded correctly
		public static implicit operator bool(WeaponData ImpliedObject)
		{
			return ImpliedObject.is_valid();
		}

		// Return whether this weapon uses two-handed stances
		public bool is_twohanded()
		{
			return twohanded;
		}
		// Return the attack speed
		public byte get_speed()
		{
			return attackspeed;
		}
		// Return the attack type
		public byte get_attack()
		{
			return attack;
		}
		// Return the speed as displayed in a Tooltip
		public string getspeedstring()
		{
			switch (attackspeed)
			{
			case 1:
				return $"{TestLuban.Get().GetL10nText("FAST")} (1)";
			case 2:
				return $"{TestLuban.Get().GetL10nText("FAST")} (2)";
			case 3:
				return $"{TestLuban.Get().GetL10nText("FAST")} (3)";
			case 4:
				return $"{TestLuban.Get().GetL10nText("FAST")} (4)";
			case 5:
				return $"{TestLuban.Get().GetL10nText("NORMAL")} (5)";
			case 6:
				return $"{TestLuban.Get().GetL10nText("NORMAL")} (6)";
			case 7:
				return $"{TestLuban.Get().GetL10nText("SLOW")} (7)";
			case 8:
				return $"{TestLuban.Get().GetL10nText("SLOW")} (8)";
			case 9:
				return $"{TestLuban.Get().GetL10nText("SLOW")} (9)";
			default:
				return "";
			}
		}
		// Return the attack delay
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
		public Weapon.Type get_type()
		{
			return type;
		}
		// Return the sound to play when attacking
		public Sound get_usesound(bool degenerate)
		{
			return usesounds[degenerate];
		}
		
		// Return the name of the afterimage
		public string get_afterimage()
		{
			return afterimage;
		}
		// Return the general equip data
		public EquipData get_equipdata()
		{
			return equipdata;
		}

		// Allow the cache to use the constructor
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C# has no concept of a 'friend' class:
//		friend Cache<WeaponData>;
		// Load a weapon from the game files
		public WeaponData(int equipid)
		{
			this.equipdata = EquipData.get(equipid);
			int prefix = equipid / 10000;
			type = Weapon.by_value(prefix);
			twohanded = prefix == (int)Weapon.Type.STAFF || (prefix >= ((int)Weapon.Type.SWORD_2H) && prefix <= (int)Weapon.Type.POLEARM) || prefix == (int)Weapon.Type.CROSSBOW;

			var src = ms.wz.wzProvider_character[$"Weapon/0{Convert.ToString (equipid)}.img"]?["info"];

			attackspeed = src["attackSpeed"];
			attack = src["attack"];

			var soundsrc = ms.wz.wzProvider_sound["Weapon.img"][src["sfx"].ToString ()];

			bool twosounds = (soundsrc["Attack2"] != null);

			if (twosounds)
			{
				usesounds[false] = soundsrc["Attack"];
				usesounds[true] = soundsrc["Attack2"];
			}
			else
			{
				usesounds[false] = soundsrc["Attack"];
				usesounds[true] = soundsrc["Attack"];
			}

			afterimage = src["afterImage"].ToString ();
		}

		private readonly EquipData equipdata;

		private Weapon.Type type;
		private bool twohanded;
		private byte attackspeed;
		private byte attack;
		private BoolPair<Sound> usesounds = new BoolPair<Sound>();
		private string afterimage;
	}
}

#if USE_NX
#endif
