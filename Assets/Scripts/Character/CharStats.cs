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
	public class CharStats
	{
		public CharStats(StatsEntry s)
		{
			this.name = s.name;
			this.petids = new List<long>(s.petids);
			this.exp = s.exp;
			this.mapid = s.mapid;
			this.portal = s.portal;
			this.rank = s.rank;
			this.jobrank = s.jobrank;
			this.basestats = s.stats;
			this.female = s.female;
			job = new Job(basestats[MapleStat.Id.JOB]); 

			init_totalstats();
		}
		public CharStats()
		{
		}

		public void init_totalstats()
		{
			totalstats.SetValue ((() => 0));
			buffdeltas.SetValue ((() => 0));
			percentages.SetValue ((() => 0));
			/*totalstats.Clear();
			buffdeltas.Clear();
			percentages.Clear();*/

			totalstats[EquipStat.Id.HP] = get_stat(MapleStat.Id.MAXHP);
			totalstats[EquipStat.Id.MP] = get_stat(MapleStat.Id.MAXMP);
			totalstats[EquipStat.Id.STR] = get_stat(MapleStat.Id.STR);
			totalstats[EquipStat.Id.DEX] = get_stat(MapleStat.Id.DEX);
			totalstats[EquipStat.Id.INT] = get_stat(MapleStat.Id.INT);
			totalstats[EquipStat.Id.LUK] = get_stat(MapleStat.Id.LUK);
			totalstats[EquipStat.Id.SPEED] = 100;
			totalstats[EquipStat.Id.JUMP] = 100;
			totalstats[EquipStat.Id.ACC] = 100;//todo remove later totalstats[EquipStat.Id.ACC] = 100;

			maxdamage = 0;
			mindamage = 0;
			honor = 0;
			attackspeed = 0;
			projectilerange = 400;
			mastery = 0.0f;
			critical = 0.05f;
			mincrit = 0.5f;
			maxcrit = 0.75f;
			damagepercent = 0.0f;
			bossdmg = 0.0f;
			ignoredef = 0.0f;
			stance = 0.0f;
			resiststatus = 0.0f;
			reducedamage = 0.0f;
		}
		public void set_stat(MapleStat.Id stat, ushort value)
		{
			basestats[stat] = value;
		}
		public void set_total(EquipStat.Id stat, int value)
		{
			StatCaps.EQSTAT_CAPS.TryGetValue (stat, out var cap_value);
			if (value > cap_value)
			{
				value = cap_value;
			}
			
			/*var iter = StatCaps.EQSTAT_CAPS.find(stat);

			if (iter != StatCaps.EQSTAT_CAPS.end())
			{
				int cap_value = iter.second;

				if (value > cap_value)
				{
					value = cap_value;
				}
			}*/

			totalstats[stat] = value;
		}
		public void add_buff(EquipStat.Id stat, int value)
		{
			int current = get_total(stat);
			set_total(stat, current + value);
			buffdeltas[stat] += value;
		}
		public void add_value(EquipStat.Id stat, int value)
		{
			int current = get_total(stat);
			set_total(stat, current + value);
		}
		public void add_percent(EquipStat.Id stat, float percent)
		{
			percentages[stat] += percent;
		}
		public void close_totalstats()
		{
			totalstats[EquipStat.Id.ACC] += calculateaccuracy();

			foreach (var iter in percentages)
			{
				EquipStat.Id stat = iter.Key;
				int total = totalstats[stat];
				total += (int)(total * iter.Value);
				set_total(stat, total);
			}

			int primary = get_primary_stat();
			int secondary = get_secondary_stat();
			int attack = get_total(EquipStat.Id.WATK);
			float multiplier = damagepercent + (float)attack / 100;
			maxdamage = (int)((primary + secondary) * multiplier);
			mindamage = (int)(((primary * 0.9f * mastery) + secondary) * multiplier);
		}

		public void set_weapontype(Weapon.Type w)
		{
			weapontype = w;
		}
		public void set_exp(long e)
		{
			exp = e;
		}
		public void set_portal(byte p)
		{
			portal = p;
		}
		public void set_mastery(float m)
		{
			mastery = 0.5f + m;
		}
		public void set_damagepercent(float d)
		{
			damagepercent = d;
		}
		public void set_reducedamage(float r)
		{
			reducedamage = r;
		}

		public void change_job(ushort id)
		{
			basestats[MapleStat.Id.JOB] = id;
			job.change_job(id);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int calculate_damage(int mobatk) const
		public int calculate_damage(int mobatk)
		{
			// TODO: Random stuff, need to find the actual formula somewhere.
			var weapon_def = get_total(EquipStat.Id.WDEF);

			if (weapon_def == 0)
			{
				return mobatk;
			}

			int reduceatk = mobatk / 2 + mobatk / weapon_def;

			return reduceatk - (int)(reduceatk * reducedamage);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_damage_buffed() const
		public bool is_damage_buffed()
		{
			return get_buffdelta(EquipStat.Id.WATK) > 0 || get_buffdelta(EquipStat.Id.MAGIC) > 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ushort get_stat(MapleStat::Id stat) const
		public ushort get_stat(MapleStat.Id stat)
		{
			return basestats[stat];
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_total(EquipStat::Id stat) const
		public int get_total(EquipStat.Id stat)
		{
			return totalstats[stat];
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_buffdelta(EquipStat::Id stat) const
		public int get_buffdelta(EquipStat.Id stat)
		{
			return buffdeltas[stat];
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Rectangle<short> get_range() const
		public Rectangle<short> get_range()
		{
			return new Rectangle<short>((short)-projectilerange, -5, -50, 50);
		}

		public void set_mapid(int id)
		{
			mapid = id;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_mapid() const
		public int get_mapid()
		{
			return mapid;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: byte get_portal() const
		public byte get_portal()
		{
			return portal;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: long get_exp() const
		public long get_exp()
		{
			return exp;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const string& get_name() const
		public string get_name()
		{
			return name;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const string& get_jobname() const
		public string get_jobname()
		{
			return job.get_name();
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Weapon::Type get_weapontype() const
		public Weapon.Type get_weapontype()
		{
			return weapontype;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float get_mastery() const
		public float get_mastery()
		{
			return mastery;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float get_critical() const
		public float get_critical()
		{
			return critical;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float get_mincrit() const
		public float get_mincrit()
		{
			return mincrit;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float get_maxcrit() const
		public float get_maxcrit()
		{
			return maxcrit;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float get_reducedamage() const
		public float get_reducedamage()
		{
			return reducedamage;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float get_bossdmg() const
		public float get_bossdmg()
		{
			return bossdmg;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float get_ignoredef() const
		public float get_ignoredef()
		{
			return ignoredef;
		}
		public void set_stance(float s)
		{
			stance = s;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float get_stance() const
		public float get_stance()
		{
			return stance;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float get_resistance() const
		public float get_resistance()
		{
			return resiststatus;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_maxdamage() const
		public int get_maxdamage()
		{
			return maxdamage;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_mindamage() const
		public int get_mindamage()
		{
			return mindamage;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ushort get_honor() const
		public ushort get_honor()
		{
			return honor;
		}
		public void set_attackspeed(sbyte @as)
		{
			attackspeed = @as;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: sbyte get_attackspeed() const
		public sbyte get_attackspeed()
		{
			return attackspeed;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const Job& get_job() const
		public Job get_job()
		{
			return job;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool get_female() const
		public bool get_female()
		{
			return female;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int calculateaccuracy() const
		private int calculateaccuracy()
		{
			int totaldex = get_total(EquipStat.Id.DEX);
			int totalluk = get_total(EquipStat.Id.LUK);

			return (int)(totaldex * 0.8f + totalluk * 0.5f);
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_primary_stat() const
		private int get_primary_stat()
		{
			EquipStat.Id primary = job.get_primary(weapontype);

			return (int)(get_multiplier() * get_total(primary));
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_secondary_stat() const
		private int get_secondary_stat()
		{
			EquipStat.Id secondary = job.get_secondary(weapontype);

			return get_total(secondary);
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float get_multiplier() const
		private float get_multiplier()
		{
			switch (weapontype)
			{
			case Weapon.Type.SWORD_1H:
				return 4.0f;
			case Weapon.Type.AXE_1H:
			case Weapon.Type.MACE_1H:
			case Weapon.Type.WAND:
			case Weapon.Type.STAFF:
				return 4.4f;
			case Weapon.Type.DAGGER:
			case Weapon.Type.CROSSBOW:
			case Weapon.Type.CLAW:
			case Weapon.Type.GUN:
				return 3.6f;
			case Weapon.Type.SWORD_2H:
				return 4.6f;
			case Weapon.Type.AXE_2H:
			case Weapon.Type.MACE_2H:
			case Weapon.Type.KNUCKLE:
				return 4.8f;
			case Weapon.Type.SPEAR:
			case Weapon.Type.POLEARM:
				return 5.0f;
			case Weapon.Type.BOW:
				return 3.4f;
			default:
				return 0.0f;
			}
		}

		private string name;
		private List<long> petids = new List<long>();
		private Job job = new Job();
		private long exp;
		private int mapid;
		private byte portal;
		private System.Tuple<int, sbyte> rank = new System.Tuple<int, sbyte>(0, 0);
		private System.Tuple<int, sbyte> jobrank = new System.Tuple<int, sbyte>(0, 0);
		private EnumMap<MapleStat.Id, ushort> basestats = new EnumMap<MapleStat.Id, ushort>();
		private EnumMap<EquipStat.Id, int> totalstats = new EnumMap<EquipStat.Id, int>();
		private EnumMap<EquipStat.Id, int> buffdeltas = new EnumMap<EquipStat.Id, int>();
		private EnumMap<EquipStat.Id, float> percentages = new EnumMap<EquipStat.Id, float>();
		private int maxdamage;
		private int mindamage;
		private ushort honor;
		private sbyte attackspeed;
		private short projectilerange;
		private Weapon.Type weapontype;
		private float mastery;
		private float critical;
		private float mincrit;
		private float maxcrit;
		private float damagepercent;
		private float bossdmg;
		private float ignoredef;
		private float stance;
		private float resiststatus;
		private float reducedamage;
		private bool female;
	}
}



