using System.Collections.Generic;
using System.Linq;

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
	// Class that stores all information about the skills of an individual character
	public class SkillBook
	{
		public void set_skill(int id, int level, int mlevel, long expire)
		{
			skillentries[id] =new SkillEntry (level, mlevel, expire); {};
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool has_skill(int id) const
		public bool has_skill(int id)
		{
			return skillentries.Any(pair=>pair.Key == id);
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_level(int id) const
		public int get_level(int id)
		{
			if (skillentries.TryGetValue (id, out var skillEntry))
			{
				return skillEntry.level;
			}
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_masterlevel(int id) const
		public int get_masterlevel(int id)
		{
			if (skillentries.TryGetValue (id, out var skillEntry))
			{
				return skillEntry.masterlevel;
			}
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: long get_expiration(int id) const
		public long get_expiration(int id)
		{
			if (skillentries.TryGetValue (id, out var skillEntry))
			{
				return skillEntry.expiration;
			}
			return 0;
		}

		// Return id and level of all passive skills
		// An ordered map is used so that lower passive skills don't override higher ones
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ClassicMap<int, int> collect_passives() const
		SortedDictionary<int, int> passives = new SortedDictionary<int, int>();
		public SortedDictionary<int, int> collect_passives()
		{
			passives.Clear ();

			foreach (var iter in skillentries)
			{
				if (SkillData.get(iter.Key).is_passive())
				{
					passives.Add(iter.Key, iter.Value.level);
				}
			}

			return passives;
		}

		// Return id and level of all required skills
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ClassicUnorderedMap<int, int> collect_required(int id) const
		Dictionary<int, int> cache_reqSkills = new Dictionary<int, int> ();
		public Dictionary<int, int> collect_required(int id)
		{
			if (skillentries.TryGetValue (id, out var skillEntry))
			{
				return SkillData.get(id).get_reqskills();
			}
			return cache_reqSkills;
			/*var iter = skillentries.find(id);

			if (iter == skillentries.end())
			{
				return
				{
				};
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			return SkillData.get(iter.first).get_reqskills();*/
		}

		private struct SkillEntry
		{
			public SkillEntry (int level, int masterlevel, long expiration)
			{
				this.level = level;
				this.masterlevel = masterlevel;
				this.expiration = expiration;
			}

			public int level;
			public int masterlevel;
			public long expiration;
		}

		private Dictionary<int, SkillEntry> skillentries = new Dictionary<int, SkillEntry>();
	}
}



