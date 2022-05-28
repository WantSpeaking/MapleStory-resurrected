#define USE_NX

using System;
using ms.Helper;





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

		private readonly string[] types = new[] {"HAT", "FACE ACCESSORY", "EYE ACCESSORY", "EARRINGS", "TOP", "OVERALL", "BOTTOM", "SHOES", "GLOVES", "SHIELD", "CAPE", "RING", "PENDANT", "BELT", "MEDAL"};

		private readonly EquipSlot.Id[] equipslots = {EquipSlot.Id.HAT, EquipSlot.Id.FACE, EquipSlot.Id.EYEACC, EquipSlot.Id.EARACC, EquipSlot.Id.TOP, EquipSlot.Id.TOP, EquipSlot.Id.BOTTOM, EquipSlot.Id.SHOES, EquipSlot.Id.GLOVES, EquipSlot.Id.SHIELD, EquipSlot.Id.CAPE, EquipSlot.Id.RING1, EquipSlot.Id.PENDANT1, EquipSlot.Id.BELT, EquipSlot.Id.MEDAL};

		private readonly string[] weaponTypes = {"ONE-HANDED SWORD", "ONE-HANDED AXE", "ONE-HANDED MACE", "DAGGER", "", "", "", "WAND", "STAFF", "", "TWO-HANDED SWORD", "TWO-HANDED AXE", "TWO-HANDED MACE", "SPEAR", "POLEARM", "BOW", "CROSSBOW", "CLAW", "KNUCKLE", "GUN"};

		// Allow the cache to use the constructor
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C# has no concept of a 'friend' class:
//		friend Cache<EquipData>;
		// Load an equip from the game files
		public EquipData (int id)
		{
			this.itemdata = ItemData.get (id);
			string strid = "0" + Convert.ToString (id);
			string category = itemdata.get_category ();
			//AppDebug.Log ($"id:{id}\tstrid:{strid}\tcategory:{category}");
			//if(category == null) return;//todo 2 why id == 3 maybe chalist parsed wrong
			var src = ms.wz.wzFile_character[category]?[strid + ".img"]?["info"];

			if (src != null)
			{
				cash = src["cash"];
				tradeblock = src["tradeBlock"];
				slots = src["tuc"];

				reqstats[MapleStat.Id.LEVEL] = src["reqLevel"];
				reqstats[MapleStat.Id.JOB] = (short)(src["reqJob"]*100);//todo 2 currently all the src["reqJob"] is 0-4, however actual job is 0,100,200,300,400
				reqstats[MapleStat.Id.STR] = src["reqSTR"];
				reqstats[MapleStat.Id.DEX] = src["reqDEX"];
				reqstats[MapleStat.Id.INT] = src["reqINT"];
				reqstats[MapleStat.Id.LUK] = src["reqLUK"];
				defstats[EquipStat.Id.STR] = src["incSTR"];
				defstats[EquipStat.Id.DEX] = src["incDEX"];
				defstats[EquipStat.Id.INT] = src["incINT"];
				defstats[EquipStat.Id.LUK] = src["incLUK"];
				defstats[EquipStat.Id.WATK] = src["incPAD"];
				defstats[EquipStat.Id.WDEF] = src["incPDD"];
				defstats[EquipStat.Id.MAGIC] = src["incMAD"];
				defstats[EquipStat.Id.MDEF] = src["incMDD"];
				defstats[EquipStat.Id.HP] = src["incMHP"];
				defstats[EquipStat.Id.MP] = src["incMMP"];
				defstats[EquipStat.Id.ACC] = src["incACC"];
				defstats[EquipStat.Id.AVOID] = src["incEVA"];
				defstats[EquipStat.Id.HANDS] = src["incHANDS"];
				defstats[EquipStat.Id.SPEED] = src["incSPEED"];
				defstats[EquipStat.Id.JUMP] = src["incJUMP"];
			}

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