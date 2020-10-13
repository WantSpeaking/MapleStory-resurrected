#define USE_NX

using System.Collections.Generic;
using Assets.ms.Helper;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using nl;

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
	// Contains information about a skill
	public class SkillData : Cache<SkillData>
	{
		// The stats of one level
		public class Stats
		{
			public float damage;
			public int matk;
			public int fixdamage;
			public int mastery;
			public byte attackcount;
			public byte mobcount;
			public byte bulletcount;
			public short bulletcost;
			public int hpcost;
			public int mpcost;
			public float chance;
			public float critical;
			public float ignoredef;
			public float hrange;
			public Rectangle<short> range = new Rectangle<short> ();

			public Stats (float damage, int matk, int fixdamage, int mastery, byte attackcount, byte mobcount, byte bulletcount, short bulletcost, int hpcost, int mpcost, float chance, float critical, float ignoredef, float hrange, Rectangle<short> range)
			{
				this.damage = damage;
				this.matk = matk;
				this.fixdamage = fixdamage;
				this.mastery = mastery;
				this.attackcount = attackcount;
				this.mobcount = mobcount;
				this.bulletcount = bulletcount;
				this.bulletcost = bulletcost;
				this.hpcost = hpcost;
				this.mpcost = mpcost;
				this.chance = chance;
				this.critical = critical;
				this.ignoredef = ignoredef;
				this.hrange = hrange;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this.range = new ms.Rectangle<short>(range);
				this.range = range;
			}
		}

		// Skill flags, unfortunately these just have to be hard-coded
		public enum Flags
		{
			NONE = 0x0000,
			ATTACK = 0x0001,
			RANGED = 0x0002
		}

		// Icon types
		public enum Icon
		{
			NORMAL,
			DISABLED,
			MOUSEOVER,
			NUM_ICONS
		}

		// Return whether the skill is passive
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_passive() const
		public bool is_passive ()
		{
			return passive;
		}

		// Return whether the skill is an attack skill
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_attack() const
		public bool is_attack ()
		{
			return !passive && (flags & (int)Flags.ATTACK) > 0;
		}

		// Return whether this skill is invisible in the skill book UI
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_invisible() const
		public bool is_invisible ()
		{
			return invisible;
		}

		// Return the default masterlevel
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_masterlevel() const
		public int get_masterlevel ()
		{
			return masterlevel;
		}

		// Return the required weapon
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Weapon::Type get_required_weapon() const
		public Weapon.Type get_required_weapon ()
		{
			return reqweapon;
		}

		// Return the stats of one level
		// If there are no stats for that level, a default object is returned.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This was formerly a static local variable declaration (not allowed in C#):
		private readonly Stats null_stats = new Stats (0.0f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.0f, 0.0f, 0.0f, 0.0f, new Rectangle<short> ());

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const SkillData::Stats& get_stats(int level) const
		public Stats get_stats (int level)
		{
			if (stats.TryGetValue (level, out var stat))
			{
				return stat;
			}

			return null_stats;

			/*var iter = stats.find (level);

			if (iter == stats.end ())
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
//				static constexpr Stats null_stats = Stats(0.0f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.0f, 0.0f, 0.0f, 0.0f, Rectangle<short>());

				return null_stats;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			return iter.second;*/
		}

		// Return the name of the skill
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const string& get_name() const
		public string get_name ()
		{
			return name;
		}

		// Return the description of the skill
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const string& get_desc() const
		public string get_desc ()
		{
			return desc;
		}

		readonly string null_level = "Missing level description.";

		// Return the description of a level
		// If there is no description for this level, a warning message is returned.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const string& get_level_desc(int level) const
		public string get_level_desc (int level)
		{
			if (levels.TryGetValue (level, out var level_desc))
			{
				return level_desc;
			}

			return null_level;

			/*var iter = levels.find (level);

			if (iter == levels.end ())
			{
				const GlobalMembers.string null_level = "Missing level description.";

				return null_level;
			}
			else
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
				return iter.second;
			}*/
		}

		// Return one of the skill icons
		// Cannot fail if type is a valid enumeration
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const Texture& get_icon(Icon icon) const
		public Texture get_icon (Icon icon)
		{
			return icons[(int)icon];
		}

		// Return id and level of all required skills
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const ClassicUnorderedMap<int, int>& get_reqskills() const
		public Dictionary<int, int> get_reqskills ()
		{
			return reqskills;
		}

		// Allow the cache to use the constructor
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C# has no concept of a 'friend' class:
//		friend Cache<SkillData>;
		// Load a skill from the game files
		private SkillData (int id)
		{
			/// Locate sources
			string strid = string_format.extend_id (id, 7);
			string jobid = strid.Substring (0, 3);
			var node_Skillwz_1111img_skill_11111004 = nx.wzFile_skill[jobid + ".img"]["skill"][strid];
			var node_Stringwz_Skillimg_000 = nx.wzFile_string["Skill.img"][strid];

			/// Load icons
			icons = new[] {new Texture (node_Skillwz_1111img_skill_11111004["icon"]), new Texture (node_Skillwz_1111img_skill_11111004["iconDisabled"]), new Texture (node_Skillwz_1111img_skill_11111004["iconMouseOver"])};

			/// Load strings
			name = node_Stringwz_Skillimg_000["name"].ToString ();
			desc = node_Stringwz_Skillimg_000["desc"].ToString ();

			if (node_Stringwz_Skillimg_000 is WzImageProperty property_Stringwz_Skillimg_000)
			{
				var childNodeCount = property_Stringwz_Skillimg_000.WzProperties.Count;
				for (int level = 1; level <= childNodeCount; level++)
				{
					var node_Stringwz_Skillimg_000_h1 = property_Stringwz_Skillimg_000["h" + level]; //todo 可能除了h1、h2、h3等类似节点 还有其他节点 
					if (node_Stringwz_Skillimg_000_h1 != null)
					{
						levels.Add (level, node_Stringwz_Skillimg_000_h1.ToString ());
					}
				}
			}

			/*for (int level = 1; nl.node sub = node_Stringwz_Skillimg_000["h" + Convert.ToString (level)];
			level++)
			{
				levels.Add (level, sub);
			}*/

			/// Load stats
			var node_Skillwz_1111img_skill_0000008_level = node_Skillwz_1111img_skill_11111004["level"];
			if (node_Skillwz_1111img_skill_0000008_level is WzImageProperty property_Skillwz_000img_skill_0000008_level)
			{
				foreach (var property_Skillwz_000img_skill_11111004_level_1 in property_Skillwz_000img_skill_0000008_level.WzProperties)
				{
					float damage = (float)(property_Skillwz_000img_skill_11111004_level_1["damage"]?.GetFloat () ?? 100f) / 100;
					int matk = property_Skillwz_000img_skill_11111004_level_1["mad"].GetInt ();
					int fixdamage = property_Skillwz_000img_skill_11111004_level_1["fixdamage"].GetInt ();
					int mastery = property_Skillwz_000img_skill_11111004_level_1["mastery"].GetInt ();
					byte attackcount = (byte)(property_Skillwz_000img_skill_11111004_level_1["attackCount"]?.GetInt () ?? 1);
					byte mobcount = (byte)(property_Skillwz_000img_skill_11111004_level_1["mobCount"]?.GetInt () ?? 1);
					byte bulletcount = (byte)(property_Skillwz_000img_skill_11111004_level_1["bulletCount"]?.GetInt () ?? 1);
					short bulletcost = (short)(property_Skillwz_000img_skill_11111004_level_1["bulletConsume"]?.GetInt () ?? bulletcount);
					int hpcost = property_Skillwz_000img_skill_11111004_level_1["hpCon"]?.GetInt () ?? 0;
					int mpcost = property_Skillwz_000img_skill_11111004_level_1["mpCon"]?.GetInt () ?? 0;
					float chance = (property_Skillwz_000img_skill_11111004_level_1["prop"]?.GetFloat () ?? 100f) / 100;
					float critical = 0.0f;
					float ignoredef = 0.0f;
					float hrange = (float)(property_Skillwz_000img_skill_11111004_level_1["range"]?.GetFloat () ?? 100f) / 100;
					Rectangle<short> range = new Rectangle<short> (property_Skillwz_000img_skill_11111004_level_1);
					int level = string_conversion<int>.or_default (property_Skillwz_000img_skill_11111004_level_1.Name, -1);
					stats.Add (level, new Stats (damage, matk, fixdamage, mastery, attackcount, mobcount, bulletcount, bulletcost, hpcost, mpcost, chance, critical, ignoredef, hrange, range));
				}
			}
			/*foreach (var property_Skillwz_000img_skill_0000008_level_1 in node_Skillwz_000img_skill_0000008_level)
			{
				float damage = (float)property_Skillwz_000img_skill_0000008_level_1["damage"] / 100;
				int matk = property_Skillwz_000img_skill_0000008_level_1["mad"];
				int fixdamage = property_Skillwz_000img_skill_0000008_level_1["fixdamage"];
				int mastery = property_Skillwz_000img_skill_0000008_level_1["mastery"];
				byte attackcount = (byte)property_Skillwz_000img_skill_0000008_level_1["attackCount"].get_integer (1);
				byte mobcount = (byte)property_Skillwz_000img_skill_0000008_level_1["mobCount"].get_integer (1);
				byte bulletcount = (byte)property_Skillwz_000img_skill_0000008_level_1["bulletCount"].get_integer (1);
				short bulletcost = (short)property_Skillwz_000img_skill_0000008_level_1["bulletConsume"].get_integer (bulletcount);
				int hpcost = property_Skillwz_000img_skill_0000008_level_1["hpCon"];
				int mpcost = property_Skillwz_000img_skill_0000008_level_1["mpCon"];
				float chance = (float)property_Skillwz_000img_skill_0000008_level_1["prop"].get_real (100.0) / 100;
				float critical = 0.0f;
				float ignoredef = 0.0f;
				float hrange = (float)property_Skillwz_000img_skill_0000008_level_1["range"].get_real (100.0) / 100;
				Rectangle<short> range = property_Skillwz_000img_skill_0000008_level_1;
				int level = string_conversion.GlobalMembers.or_default<int> (property_Skillwz_000img_skill_0000008_level_1.name (), -1);

				stats.emplace (std::piecewise_construct, std::forward_as_tuple (level), std::forward_as_tuple (damage, matk, fixdamage, mastery, attackcount, mobcount, bulletcount, bulletcost, hpcost, mpcost, chance, critical, ignoredef, hrange, range));
			}*/

			element = node_Skillwz_1111img_skill_11111004["elemAttr"]?.ToString ();

			if (jobid == "900" || jobid == "910")
			{
				reqweapon = Weapon.Type.NONE;
			}
			else
			{
				reqweapon = Weapon.by_value (100 + node_Skillwz_1111img_skill_11111004["weapon"]?.GetInt () ?? 0);
			}

			masterlevel = stats.Count;
			passive = (id % 10000) / 1000 == 0;
			flags = flags_of (id);
			invisible = node_Skillwz_1111img_skill_11111004["invisible"].GetInt ().ToBool ();

			/// Load required skills
			var node_Skillwz_1111img_skill_11111004_req = node_Skillwz_1111img_skill_11111004["req"];
			if (node_Skillwz_1111img_skill_11111004_req is WzImageProperty property_Skillwz_1111img_skill_11111003_req)
			{
				foreach (var property_Skillwz_1111img_skill_11111003_req_11111001 in property_Skillwz_1111img_skill_11111003_req.WzProperties)
				{
					int skillid = string_conversion<int>.or_default (property_Skillwz_1111img_skill_11111003_req_11111001.Name, -1);
					int reqlv = property_Skillwz_1111img_skill_11111003_req_11111001.GetInt ();

					reqskills.Add (skillid, reqlv);
				}
			}
			
		}

		Dictionary<int, int> skill_flags = new Dictionary<int, int>
		{
			{(int)SkillId.Id.THREE_SNAILS, (int)Flags.ATTACK},
			{(int)SkillId.Id.POWER_STRIKE, (int)Flags.ATTACK},
			{(int)SkillId.Id.SLASH_BLAST, (int)Flags.ATTACK},
			{(int)SkillId.Id.SWORD_PANIC, (int)Flags.ATTACK},
			{(int)SkillId.Id.AXE_PANIC, (int)Flags.ATTACK},
			{(int)SkillId.Id.SWORD_COMA, (int)Flags.ATTACK},
			{(int)SkillId.Id.AXE_COMA, (int)Flags.ATTACK},
			{(int)SkillId.Id.RUSH_HERO, (int)Flags.ATTACK},
			{(int)SkillId.Id.BRANDISH, (int)Flags.ATTACK},
			{(int)SkillId.Id.CHARGE, (int)Flags.ATTACK},
			{(int)SkillId.Id.RUSH_PALADIN, (int)Flags.ATTACK},
			{(int)SkillId.Id.BLAST, (int)Flags.ATTACK},
			{(int)SkillId.Id.HEAVENS_HAMMER, (int)Flags.ATTACK},
			{(int)SkillId.Id.DRAGON_BUSTER, (int)Flags.ATTACK},
			{(int)SkillId.Id.DRAGON_FURY, (int)Flags.ATTACK},
			{(int)SkillId.Id.PA_BUSTER, (int)Flags.ATTACK},
			{(int)SkillId.Id.PA_FURY, (int)Flags.ATTACK},
			{(int)SkillId.Id.SACRIFICE, (int)Flags.ATTACK},
			{(int)SkillId.Id.DRAGONS_ROAR, (int)Flags.ATTACK},
			{(int)SkillId.Id.RUSH_DK, (int)Flags.ATTACK},
			{(int)SkillId.Id.ENERGY_BOLT, (int)Flags.ATTACK | (int)Flags.RANGED},
			{(int)SkillId.Id.MAGIC_CLAW, (int)Flags.ATTACK | (int)Flags.RANGED},
			{(int)SkillId.Id.SLOW_FP, (int)Flags.ATTACK},
			{(int)SkillId.Id.FIRE_ARROW, (int)Flags.ATTACK | (int)Flags.RANGED},
			{(int)SkillId.Id.POISON_BREATH, (int)Flags.ATTACK | (int)Flags.RANGED},
			{(int)SkillId.Id.EXPLOSION, (int)Flags.ATTACK},
			{(int)SkillId.Id.POISON_BREATH,(int) Flags.ATTACK},
			{(int)SkillId.Id.SEAL_FP,(int) Flags.ATTACK},
			{(int)SkillId.Id.ELEMENT_COMPOSITION_FP, (int)Flags.ATTACK | (int)Flags.RANGED},
			{(int)SkillId.Id.FIRE_DEMON, (int)Flags.ATTACK},
			{(int)SkillId.Id.PARALYZE, (int)Flags.ATTACK | (int)Flags.RANGED},
			{(int)SkillId.Id.METEOR_SHOWER, (int)Flags.ATTACK}
		};
		// Get some hard-coded information
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int flags_of(int id) const
		private int flags_of (int id)
		{
			skill_flags.TryGetValue (id, out var flag);			
			return flag;
			/*var iter = skill_flags.find (id);

			if (iter == skill_flags.end ())
			{
				return (int)Flags.NONE;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			return iter.second;*/
		}

		private Dictionary<int, Stats> stats = new Dictionary<int, Stats> ();
		private string element;
		private Weapon.Type reqweapon;
		private int masterlevel;
		private int flags;
		private bool passive;
		private bool invisible;

		private string name;
		private string desc;
		private Dictionary<int, string> levels = new Dictionary<int, string> ();
		private Dictionary<int, int> reqskills = new Dictionary<int, int> ();

		private Texture[] icons = new Texture[(int)Icon.NUM_ICONS];
	}
}


#if USE_NX
#endif