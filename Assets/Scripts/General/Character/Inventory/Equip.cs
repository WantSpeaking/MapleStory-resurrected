


namespace ms
{
	public class Equip
	{
		public enum Potential
		{
			POT_NONE,
			POT_HIDDEN,
			POT_RARE,
			POT_EPIC,
			POT_UNIQUE,
			POT_LEGENDARY,
		}

		public Equip(int item_id, long expiration, string owner, short flags, byte slots, byte level, EnumMap<EquipStat.Id, ushort> stats, byte itemlevel, short itemexp, int vicious)
		{
			this.item_id = item_id;
			this.expiration = expiration;
			this.owner = owner;
			this.flags = flags;
			this.slots = slots;
			this.level = level;
			this.stats = stats;
			this.itemlevel = itemlevel;
			this.itemexp = itemexp;
			this.vicious = vicious;
			potrank = Equip.Potential.POT_NONE;
			quality = EquipQuality.check_quality(item_id, level > 0, stats);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_item_id() const
		public int get_item_id()
		{
			return item_id;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: long get_expiration() const
		public long get_expiration()
		{
			return expiration;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const string& get_owner() const
		public string get_owner()
		{
			return owner;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short get_flags() const
		public short get_flags()
		{
			return flags;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: byte get_slots() const
		public byte get_slots()
		{
			return slots;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: byte get_level() const
		public byte get_level()
		{
			return level;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: byte get_itemlevel() const
		public byte get_itemlevel()
		{
			return itemlevel;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ushort get_stat(EquipStat::Id type) const
		public ushort get_stat(EquipStat.Id type)
		{
			return stats[type];
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_vicious() const
		public int get_vicious()
		{
			return vicious;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Equip::Potential get_potrank() const
		public Equip.Potential get_potrank()
		{
			return potrank;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: EquipQuality::Id get_quality() const
		public EquipQuality.Id get_quality()
		{
			return quality;
		}

		private EnumMap<EquipStat.Id, ushort> stats = new EnumMap<EquipStat.Id, ushort>();
		private int item_id;
		private long expiration;
		private string owner;
		private short flags;
		private byte slots;
		private byte level;
		private byte itemlevel;
		private short itemexp;
		private int vicious;
		private Potential potrank;
		private EquipQuality.Id quality;
	}
}
