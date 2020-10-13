#define USE_NX

using System.Collections.Generic;
using MapleLib.WzLib;

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
	// Contains information about a job
	public class JobData : Cache<JobData>
	{
		// Return the ids of the skills for this job
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const ClassicVector<int>& get_skills() const
		public List<int> get_skills()
		{
			return skills;
		}
		// Return the name of the skill book
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const string& get_name() const
		public string get_name()
		{
			return name;
		}
		// Return the book cover icon
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const Texture& get_icon() const
		public Texture get_icon()
		{
			return icon;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C# has no concept of a 'friend' class:
//		friend Cache<JobData>;
		private JobData(int id)
		{
			string strid = string_format.extend_id(id, 3);
			var node_SkillWZ_000img = nl.nx.wzFile_skill[strid + ".img"];
			var strsrc = nl.nx.wzFile_string["Skill.img"][strid];

			icon =new Texture (node_SkillWZ_000img["info"]["icon"]);

			name = strsrc["bookName"].ToString ();

			if (node_SkillWZ_000img["skill"] is WzImageProperty property_SkillWZ_000img_skill)
			{
				foreach (var property_SkillWZ_000img_skill_0000008 in property_SkillWZ_000img_skill.WzProperties)
				{
					int skill_id = string_conversion<int>.or_zero(property_SkillWZ_000img_skill_0000008.Name);

					if (skill_id == 0)
					{
						continue;
					}

					skills.Add(skill_id);
				}
			}
			
		}

		private Texture icon = new Texture();
		private List<int> skills = new List<int>();
		private string name;
	}
}


#if USE_NX
#endif
