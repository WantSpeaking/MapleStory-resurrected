using System.Collections.Generic;
using MapleLib.WzLib;

namespace ms
{
	// Contains information about a job
	public class JobData : Cache<JobData>
	{
		// Return the ids of the skills for this job
		public List<int> get_skills()
		{
			return skills;
		}
		// Return the name of the skill book
		public string get_name()
		{
			return name;
		}
		// Return the book cover icon
		public Texture get_icon()
		{
			return icon;
		}

		public JobData(int id)
		{
			if (id == 312)
			{
				AppDebug.Log ("JobData id 312");
			}
			string strid = string_format.extend_id(id, 3);
			var node_SkillWZ_000img = ms.wz.findSkillImage(strid + ".img");
			var strsrc = ms.wz.wzFile_string["Skill.img"][strid];

			if (node_SkillWZ_000img == null)
			{
				AppDebug.LogWarning ($"JobData skillId :{id} doesn't exist");
				return;
			}
			
			
			icon = new Texture (node_SkillWZ_000img["info"]["icon"]);

			name = strsrc["bookName"]?.ToString ();

			if (node_SkillWZ_000img["skill"] is WzImageProperty property_SkillWZ_000img_skill)
			{
				foreach (var property_SkillWZ_000img_skill_0000008 in property_SkillWZ_000img_skill.WzProperties)
				{
					int skill_id = string_conversion.or_zero<int>(property_SkillWZ_000img_skill_0000008.Name);

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
