using System;
using System.Collections.Generic;

/*
 This file is part of the OdinMS Maple Story Server
 Copyright (C) 2008 Patrick Huy <patrick.huy@frz.cc>
 Matthias Butz <matze@odinms.de>
 Jan Christian Meyer <vimes@odinms.de>

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU Affero General Public License as
 published by the Free Software Foundation version 3 as published by
 the Free Software Foundation. You may not use, modify or distribute
 this program under any other version of the GNU Affero General Public
 License.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU Affero General Public License for more details.

 You should have received a copy of the GNU Affero General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
namespace server.quest.actions
{
	using MapleCharacter = client.MapleCharacter;
	using MapleJob = client.MapleJob;
	using Skill = client.Skill;
	using SkillFactory = client.SkillFactory;
	using WzImageProperty = MapleLib.WzLib.WzImageProperty;
	using MapleDataTool = provider.MapleDataTool;

	/// 
	/// <summary>
	/// @author Tyler (Twdtwd)
	/// </summary>
	public class SkillAction : MapleQuestAction
	{
		private int itemEffect;
		private IDictionary<int, SkillData> skillData = new Dictionary<int, SkillData>();

		public SkillAction(MapleQuest quest, WzImageProperty data) : base(MapleQuestActionType.SKILL, quest)
		{
			processData(data);
		}


		public override void processData(WzImageProperty data)
		{
			foreach (WzImageProperty sEntry in data)
			{
				sbyte skillLevel = 0;
				int skillid = MapleDataTool.getInt(sEntry.GetFromPath("id"));
				WzImageProperty skillLevelData = sEntry.GetFromPath("skillLevel");
				if (skillLevelData != null)
				{
					skillLevel = (sbyte) MapleDataTool.getInt(skillLevelData);
				}
				int masterLevel = MapleDataTool.getInt(sEntry.GetFromPath("masterLevel"));
				IList<int> jobs = new List<int>();

				WzImageProperty applicableJobs = sEntry.GetFromPath("job");
				if (applicableJobs != null)
				{
					foreach (WzImageProperty applicableJob in applicableJobs)
					{
						jobs.Add(MapleDataTool.getInt(applicableJob));
					}
				}

				skillData[skillid] = new SkillData(this, skillid, skillLevel, masterLevel, jobs);
			}
		}

		public override void run(MapleCharacter chr, int? extSelection)
		{
			/*foreach (SkillData skill in skillData.Values)
			{
				Skill skillObject = SkillFactory.getSkill(skill.Id);
							if (skillObject == null)
							{
								continue;
							}

							bool shouldLearn = false;

				if (skill.jobsContains(chr.Job) || skillObject.BeginnerSkill)
				{
					shouldLearn = true;
				}

				sbyte skillLevel = (sbyte) Math.Max(skill.Level, chr.getSkillLevel(skillObject));
				int masterLevel = Math.Max(skill.MasterLevel, chr.getMasterLevel(skillObject));
				if (shouldLearn)
				{
					chr.changeSkillLevel(skillObject, skillLevel, masterLevel, -1);
				}

			}*/
		}

		private class SkillData
		{
			private readonly SkillAction outerInstance;

			protected internal int id, level, masterLevel;
			internal IList<int> jobs = new List<int>();

			public SkillData(SkillAction outerInstance, int id, int level, int masterLevel, IList<int> jobs)
			{
				this.outerInstance = outerInstance;
				this.id = id;
				this.level = level;
				this.masterLevel = masterLevel;
				this.jobs = jobs;
			}

			public virtual int Id
			{
				get
				{
					return id;
				}
			}

			public virtual int Level
			{
				get
				{
					return level;
				}
			}

			public virtual int MasterLevel
			{
				get
				{
					return masterLevel;
				}
			}

			public virtual bool jobsContains(ms.Job job)
			{
				return jobs.Contains(job.get_id());
			}
		}
	}
}