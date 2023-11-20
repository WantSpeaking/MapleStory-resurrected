using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MapleLib.WzLib.WzProperties;
using provider;

namespace ms
{
	// Class that stores information on the quest log of an individual character
	public class CheckLog
	{
		public CheckLog ()
		{

		}

		public CheckInfo GetCheckInfo (short questId)
		{
			if (!questId_Info_Dict.TryGetValue(questId, out var checkInfo))
			{
				checkInfo = new CheckInfo();
				checkInfo.checkStages = new List<CheckStage>();
				if (wz.wzProvider_quest["Check.img"][$"{questId}"] is MapleData wzObj_Checkimg_1000)
				{
					var count = wzObj_Checkimg_1000.Count ();
					for (int i = 0; i < count; i++)
					/*{
						wzObj_Checkimg_1000[i]
					}
					foreach (var checkimg_1000_0 in wzObj_Checkimg_1000)*/
					{
						var checkimg_1000_0 = wzObj_Checkimg_1000[i.ToString()];
						if (checkimg_1000_0 == null)
						{
							//4940
							AppDebug.LogWarning ($"Quest.wz/Check.img/{questId}/{i} is null");
							continue;
						}
						var checkStage = new CheckStage ();
						checkStage.npc = checkimg_1000_0["npc"];
						checkStage.lvmin = checkimg_1000_0["lvmin"];
						checkStage.lvmax = checkimg_1000_0["lvmax"];
						checkStage.dayByDay = checkimg_1000_0["dayByDay"];
						checkStage.infoNumber = checkimg_1000_0["infoNumber"];
						checkStage.start = checkimg_1000_0["start"]?.ToString ();
						checkStage.end = checkimg_1000_0["end"]?.ToString ();
						checkStage.normalAutoStart = checkimg_1000_0["normalAutoStart"];
						checkStage.startscript = checkimg_1000_0["startscript"]?.ToString ();
						checkStage.endscript = checkimg_1000_0["endscript"]?.ToString ();
						checkStage.interval = checkimg_1000_0["interval"];
						checkStage.worldmin = checkimg_1000_0["worldmin"]?.ToString ();
						checkStage.worldmax = checkimg_1000_0["worldmax"]?.ToString ();
						checkStage.morph = checkimg_1000_0["morph"];
						checkStage.pop = checkimg_1000_0["pop"];
						checkStage.endmeso = checkimg_1000_0["endmeso"];
						checkStage.acquire = checkimg_1000_0["acquire"];
						checkStage.level = checkimg_1000_0["level"];
						checkStage.buff = checkimg_1000_0["buff"]?.ToString ();
						checkStage.exceptbuff = checkimg_1000_0["exceptbuff"]?.ToString ();
						checkStage.partyQuest_S = checkimg_1000_0["partyQuest_S"];
						checkStage.questComplete = checkimg_1000_0["questComplete"];
						checkStage.pettamenessmin = checkimg_1000_0["pettamenessmin"];
						checkStage.mbmin = checkimg_1000_0["mbmin"];
						checkStage.userInteract = checkimg_1000_0["userInteract"];
						checkStage.petRecallLimit = checkimg_1000_0["petRecallLimit"];
						checkStage.petAutoSpeakingLimit = checkimg_1000_0["petAutoSpeakingLimit"];
						checkStage.tamingmoblevelmin = checkimg_1000_0["tamingmoblevelmin"];

						checkStage.items = new List<CheckRequireItem> ();
						if (checkimg_1000_0["item"] is MapleData checkimg_1000_0_item)
						{
							foreach (var checkimg_1000_0_item_0 in checkimg_1000_0_item)
							{
								var checkitem = new CheckRequireItem ();
								checkitem.id = checkimg_1000_0_item_0["id"];
								checkitem.count = checkimg_1000_0_item_0["count"];
								checkStage.items.Add (checkitem);
							}
						}

						checkStage.quests = new List<CheckRequireQuest> ();
						if (checkimg_1000_0["quest"] is MapleData checkimg_1000_0_quest)
						{
							foreach (var checkimg_1000_0_quest_0 in checkimg_1000_0_quest)
							{
								var checkQuest = new CheckRequireQuest ();
								checkQuest.id = checkimg_1000_0_quest_0["id"];
								checkQuest.state = checkimg_1000_0_quest_0["state"];
								checkStage.quests.Add (checkQuest);
							}
						}

						checkStage.jobs = new List<int> ();
						if (checkimg_1000_0["job"] is MapleData checkimg_1000_0_job)
						{
							foreach (var checkimg_1000_0_job_0 in checkimg_1000_0_job)
							{
								int jobId = checkimg_1000_0_job_0;
								checkStage.jobs.Add (jobId);
							}
						}

						checkStage.fieldEnters = new List<int> ();
						if (checkimg_1000_0["fieldEnter"] is MapleData checkimg_1000_0_fieldEnter)
						{
							foreach (var checkimg_1000_0_fieldEnter_0 in checkimg_1000_0_fieldEnter)
							{
								int mapId = checkimg_1000_0_fieldEnter_0;
								checkStage.fieldEnters.Add (mapId);
							}
						}

						checkStage.pets = new List<int> ();
						if (checkimg_1000_0["pet"] is MapleData checkimg_1000_0_pet)
						{
							foreach (var checkimg_1000_0_pet_0 in checkimg_1000_0_pet)
							{
								int petId = checkimg_1000_0_pet_0["id"];
								checkStage.pets.Add (petId);
							}
						}

						checkStage.infoexs = new List<CheckRequireInfoex> ();
						if (checkimg_1000_0["infoex"] is MapleData checkimg_1000_0_infoex)
						{
							foreach (var checkimg_1000_0_infoex_0 in checkimg_1000_0_infoex)
							{
								var checkInfoex = new CheckRequireInfoex ();
								checkInfoex.value = checkimg_1000_0_infoex_0["value"]?.ToString();
								checkInfoex.cond = checkimg_1000_0_infoex_0["cond"];
								checkStage.infoexs.Add (checkInfoex);
							}
						}

						checkStage.mbcards = new List<CheckRequireMbcard> ();
						if (checkimg_1000_0["mbcard"] is MapleData checkimg_1000_0_mbcard)
						{
							foreach (var checkimg_1000_0_mbcard_0 in checkimg_1000_0_mbcard)
							{
								var checkMbcard = new CheckRequireMbcard ();
								checkMbcard.id = checkimg_1000_0_mbcard_0["id"];
								checkMbcard.min = checkimg_1000_0_mbcard_0["min"];
								checkStage.mbcards.Add (checkMbcard);
							}
						}

						checkStage.mobs = new List<CheckRequireMob> ();
						if (checkimg_1000_0["mob"] is MapleData checkimg_1000_0_mob)
						{
							foreach (var checkimg_1000_0_mob_0 in checkimg_1000_0_mob)
							{
								var checkMob = new CheckRequireMob ();
								checkMob.id = checkimg_1000_0_mob_0["id"];
								checkMob.count = checkimg_1000_0_mob_0["count"];
								checkStage.mobs.Add (checkMob);
							}
						}
						checkInfo.checkStages.Add (checkStage);
					}

				}
				else
				{
					var tempName = $"Quest check doesn't exist,id:{questId}";
					//checkInfo.Name = tempName;
					AppDebug.LogWarning (tempName);
				}
				questId_Info_Dict.Add (questId, checkInfo);
			}

			return checkInfo;
		}

		private SortedDictionary<short, CheckInfo> questId_Info_Dict = new SortedDictionary<short, CheckInfo> ();

	}

	public struct CheckInfo
	{
		public List<CheckStage> checkStages { get; set; }
	}

	public struct CheckStage
	{
		public List<CheckRequireItem> items { get; set; }
		public List<CheckRequireQuest> quests { get; set; }
		public List<int> jobs { get; set; }
		public List<int> fieldEnters { get; set; }
		public List<int> pets { get; set; }
		public List<CheckRequireInfoex> infoexs { get; set; }
		public List<CheckRequireMbcard> mbcards { get; set; }
		public List<CheckRequireMob> mobs { get; set; }

		public int npc { get; set; }
		public int lvmin { get; set; }
		public int lvmax { get; set; }
		public int dayByDay { get; set; }
		public int infoNumber { get; set; }

		/// <summary>
		/// 开始日期
		/// </summary>
		public string start { get; set; }
		/// <summary>
		/// 结束日期
		/// </summary>
		public string end { get; set; }
		public bool normalAutoStart { get; set; }
		public string startscript { get; set; }
		public string endscript { get; set; }
		public int interval { get; set; }
		public string worldmin { get; set; }
		public string worldmax { get; set; }
		public int morph { get; set; }
		public int pop { get; set; }
		public int endmeso { get; set; }
		public int acquire { get; set; }
		public int level { get; set; }
		public string buff { get; set; }
		public string exceptbuff { get; set; }
		public int partyQuest_S { get; set; }
		public int questComplete { get; set; }
		public int pettamenessmin { get; set; }
		public int mbmin { get; set; }
		public int userInteract { get; set; }
		public int petRecallLimit { get; set; }
		public int petAutoSpeakingLimit { get; set; }
		public int tamingmoblevelmin { get; set; }


	}


	public struct CheckRequireItem
	{
		public int id { get; set; }
		public int count { get; set; }

	}
	public struct CheckRequireMob
	{
		public int id { get; set; }
		public int count { get; set; }

	}
	public struct CheckRequireQuest
	{
		public int id { get; set; }
		/// <summary>
		/// 0:unstarted 1:started 2:completed
		/// </summary>
		public int state { get; set; }

	}
	public struct CheckRequireMbcard
	{
		public int id { get; set; }
		public int min { get; set; }

	}

	public struct CheckRequireInfoex
	{
		public string value { get; set; }
		public int cond { get; set; }

	}

}


