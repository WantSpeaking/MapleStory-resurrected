using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MapleLib.WzLib.WzProperties;

namespace ms
{
	// Class that stores information on the quest log of an individual character
	public class Quest
	{
		public Quest ()
		{
			this.questLog = ms.Stage.Instance.get_player ().get_questlog ();
			this.checkLog = ms.Stage.Instance.get_player ().get_checklog ();

			AvailableQuests = new ReadOnlyDictionary<short, QuestInfo> (available_QuestId_Info_Dict);

		}


		public QuestLog questLog;
		public CheckLog checkLog;

		public ReadOnlyDictionary<short, QuestInfo> AvailableQuests;

		private SortedDictionary<short, QuestInfo> available_QuestId_Info_Dict = new SortedDictionary<short, QuestInfo> ();

		public bool isAvailable (short questId)
		{
			return available_QuestId_Info_Dict.ContainsKey (questId);
		}
		public void GetAvailable_Quest (bool forceGet = false)
		{
			if (!forceGet)
			{
				if (available_QuestId_Info_Dict.Count > 0)
					return;
			}

			available_QuestId_Info_Dict.Clear ();
			var questwz_QuestInfoimg = wz.wzFile_quest.GetObjectFromPath ($"Quest.wz/QuestInfo.img");
			foreach (var questwz_QuestInfoimg_1000 in questwz_QuestInfoimg)
			{
				var questId = Convert.ToInt16 (questwz_QuestInfoimg_1000.Name);
				var questInfo = questLog.GetQuestInfo (questId);
				var checkInfo = checkLog.GetCheckInfo (questId);
				var player = ms.Stage.Instance.get_player ();
				if (checkInfo.checkStages.Count == 0)
					return;

				bool isAvailable = true;
				if (questId == 6250)
				{
					isAvailable = true;
				}
				var checkStage0 = checkInfo.checkStages[0];
				isAvailable &= checkStage0.lvmin == 0 ? true : player.get_level () >= checkStage0.lvmin;
				isAvailable &= checkStage0.lvmax == 0 ? true : player.get_level () <= checkStage0.lvmax;
				isAvailable &= checkStage0.level == 0 ? true : player.get_level () == checkStage0.level;


				var nowString = System.DateTime.Now.ToString ("yyyyMMddHH");
				var nowTime = Convert.ToInt32 (nowString);

				if (!string.IsNullOrEmpty (checkStage0.start))
				{
					var startTimeString = checkStage0.start;
					var startTime = Convert.ToInt32 (startTimeString);
					isAvailable &= nowTime >= startTime;
				}
				if (!string.IsNullOrEmpty (checkStage0.end))
				{
					var endTimeString = checkStage0.end;
					var endTime = Convert.ToInt32 (endTimeString);
					isAvailable &= nowTime <= endTime;
				}

				foreach (var item in checkStage0.items)
				{
					isAvailable &= player.get_inventory ().hasEnoughItem (item.id, item.count);
				}

				if (checkStage0.jobs.Count > 0)
				{
					bool isJobFullfill = false;
					foreach (var job in checkStage0.jobs)
					{
						isJobFullfill |= (job == player.get_stats ().get_job ().get_id ());
					}
					isAvailable &= isJobFullfill;
				}

				if (checkStage0.fieldEnters.Count > 0)
				{
					bool isFieldEnterFullfill = false;
					foreach (var fieldEnter in checkStage0.fieldEnters)
					{
						isFieldEnterFullfill |= (fieldEnter == Stage.get ().get_mapid ());
					}
					isAvailable &= isFieldEnterFullfill;
				}

				if (checkStage0.pets.Count > 0)
				{
					bool isFieldEnterFullfill = false;
					foreach (var pet in checkStage0.pets)
					{
						isFieldEnterFullfill |= player.has_pet (pet);
					}
					isAvailable &= isFieldEnterFullfill;
				}

				//如果这个任务本身已经开始或完成，那么也是不可开始的任务
				if (questLog.is_inprogressed (questId) || questLog.is_completed (questId))
				{
					isAvailable &= false;
				}

				//这个任务的前置任务 符合已经开始或完成的条件，该任务才是可开始的任务
				foreach (var checkQuest in checkStage0.quests)
				{
					if (checkQuest.state == 1)
					{
						isAvailable &= questLog.is_inprogressed ((short)checkQuest.id);
					}
					else if (checkQuest.state == 2)
					{
						isAvailable &= questLog.is_completed ((short)checkQuest.id);
					}
				}

				if (isAvailable)
				{
					//AppDebug.Log ($"questId:{questId}\t checkStage0: lvmin:{checkStage0.lvmin}\t lvmax:{checkStage0.lvmax}\t level{checkStage0.level}|player level:{player.get_level ()}");

					available_QuestId_Info_Dict.Add (questId, questInfo);
				}
			}
		}

		public void updateQuest (short questId, byte status, string progressData)
		{
			if (status == 0)//available
			{
				if (questLog.is_inprogressed (questId))//remove from inprogress
				{
					questLog.remove_inprogressed (questId);
				}

				available_QuestId_Info_Dict.TryAdd(questId,questLog.GetQuestInfo (questId),true);//update available
			}
			else if (status == 1)//in progress
			{
				if (isAvailable (questId))//remove from available
				{
					available_QuestId_Info_Dict.Remove (questId);
				}

				questLog.add_in_progress (questId, progressData, true);//update in_progress
			}
			else if (status == 2)//complete
			{
				if (questLog.is_inprogressed (questId))//remove from in_progress
				{
					questLog.remove_inprogressed (questId);
				}

				questLog.add_completed (questId, long.Parse (progressData));//update complete
			}

			ms.Stage.get ().UpdateQuest ();
		}
	}


}


