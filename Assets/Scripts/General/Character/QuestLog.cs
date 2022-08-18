using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MapleLib.WzLib.WzProperties;

namespace ms
{
	// Class that stores information on the quest log of an individual character
	public class QuestLog
	{
		public QuestLog ()
		{
			Started = new ReadOnlyDictionary<short, string> (started);
			In_progress = new ReadOnlyDictionary<short, System.Tuple<short, string>> (in_progress);
			Completed = new ReadOnlyDictionary<short, long> (completed);
		}
		public void add_started (short qid, string qdata)
		{
			started[qid] = qdata;
		}
		public void add_in_progress (short qid, short qidl, string qdata)
		{
			in_progress[qid] = new Tuple<short, string> (qidl, qdata);
		}
		public void add_completed (short qid, long time)
		{
			completed[qid] = time;
		}
		public bool is_started (short qid)
		{
			return started.Any (pair => pair.Key == qid);
		}
		public short get_last_started ()
		{
			return started.Last ().Key;
		}

		public bool is_completed (short qid)
		{
			return completed.Any (pair => pair.Key == qid);
		}
		public QuestInfo GetQuestInfo (short questId)
		{
			if (!questId_Info_Dict.TryGetValue (questId, out var questInfo))
			{
				questInfo = new QuestInfo ();
				var wzObj_Quest = wz.wzFile_quest.GetObjectFromPath ($"Quest.wz/QuestInfo.img/{questId}");
				if (wzObj_Quest != null)
				{
					var tempName = wzObj_Quest["name"]?.ToString ();
					questInfo.Id = questId;
					questInfo.Name = string.IsNullOrEmpty (tempName) ? $"name none,id:{questId}" : tempName;
					questInfo.Parent = wzObj_Quest["parent"] is WzNullProperty ? "" : wzObj_Quest["parent"]?.ToString ();
					questInfo.Info_started = wzObj_Quest["0"]?.ToString ();
					questInfo.Info_in_progress = wzObj_Quest["1"]?.ToString ();
					questInfo.Info_completed = wzObj_Quest["2"]?.ToString ();
					questInfo.Area = wzObj_Quest["area"];
					questInfo.Order = wzObj_Quest["order"];
					questInfo.AutoStart = wzObj_Quest["autoStart"];
					questInfo.AutoPreComplete = wzObj_Quest["autoPreComplete"];
					questInfo.AutoComplete = wzObj_Quest["autoComplete"];
					questInfo.oneShot = wzObj_Quest["oneShot"];
					questInfo.Summary = wzObj_Quest["summary"]?.ToString ();
					questInfo.DemandSummary = wzObj_Quest["demandSummary"]?.ToString ();
					questInfo.RewardSummary = wzObj_Quest["rewardSummary"]?.ToString ();
					questInfo.medalCategory = wzObj_Quest["medalCategory"];
					questInfo.viewMedalItem = wzObj_Quest["viewMedalItem"];
					questInfo.timeLimit = wzObj_Quest["timeLimit"];
					questInfo.timerUI = wzObj_Quest["timerUI"]?.ToString ();
					questInfo.selectedMob = wzObj_Quest["selectedMob"];
					questInfo.sortkey = wzObj_Quest["sortkey"]?.ToString ();
					questInfo.autoAccept = wzObj_Quest["autoAccept"];
					questInfo.type = wzObj_Quest["type"]?.ToString ();
					questInfo.showLayerTag = wzObj_Quest["showLayerTag"]?.ToString ();
					questInfo.selectedSkillID = wzObj_Quest["selectedSkillID"];
					questInfo.timeLimit2 = wzObj_Quest["timeLimit2"];
					questInfo.dailyPlayTime = wzObj_Quest["dailyPlayTime"];
				}
				else
				{
					var tempName = $"Quest doesn't exist,id:{questId}";
					questInfo.Name = tempName;
					AppDebug.LogWarning (tempName);
				}
				questId_Info_Dict.Add (questId, questInfo);
			}

			return questInfo;
		}

		private SortedDictionary<short, string> started = new SortedDictionary<short, string> ();
		private SortedDictionary<short, System.Tuple<short, string>> in_progress = new SortedDictionary<short, System.Tuple<short, string>> ();
		private SortedDictionary<short, long> completed = new SortedDictionary<short, long> ();

		public ReadOnlyDictionary<short, string> Started;
		public ReadOnlyDictionary<short, System.Tuple<short, string>> In_progress;
		public ReadOnlyDictionary<short, long> Completed;


		private SortedDictionary<short, QuestInfo> questId_Info_Dict = new SortedDictionary<short, QuestInfo> ();


	}

	public struct QuestInfo
	{
		public short Id { get; set; }
		public string Name { get; set; }
		public string Parent { get; set; }
		public string Info_started { get; set; }
		public string Info_in_progress { get; set; }
		public string Info_completed { get; set; }
		public int Area { get; set; }
		public int Order { get; set; }
		public bool AutoStart { get; set; }
		public bool AutoPreComplete { get; set; }
		public bool AutoComplete { get; set; }
		public int oneShot { get; set; }

		public string Summary { get; set; }
		public string DemandSummary { get; set; }
		public string RewardSummary { get; set; }
		public int medalCategory { get; set; }
		public int viewMedalItem { get; set; }
		public int timeLimit { get; set; }
		public string timerUI { get; set; }
		public int selectedMob { get; set; }
		public string sortkey { get; set; }
		public bool autoAccept { get; set; }
		/// <summary>
		/// 技能 必须 转职
		/// </summary>
		public string type { get; set; }
		/// <summary>
		/// 21705 james1 james2 james3 sordQuest magicQuest thiefQuest bowQuest pirateQuest
		/// </summary>
		public string showLayerTag { get; set; }
		public int selectedSkillID { get; set; }
		public int timeLimit2 { get; set; }
		public int dailyPlayTime { get; set; }

		public bool hasParent => !string.IsNullOrEmpty (Parent);
		public string DisplayName => hasParent ? Parent : Name;

	}
}


