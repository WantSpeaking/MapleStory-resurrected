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
			this.checkLog = ms.Stage.Instance.get_player ().get_checkLog ();

			AvailableQuests = new ReadOnlyDictionary<short, QuestInfo> (available_QuestId_Info_Dict);

		}


		QuestLog questLog;
		CheckLog checkLog;

		public ReadOnlyDictionary<short, QuestInfo> AvailableQuests;

		private SortedDictionary<short, QuestInfo> available_QuestId_Info_Dict = new SortedDictionary<short, QuestInfo> ();

		public void GetAvailable_Quest ()
		{
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
				if (questId == 3242)
				{
					isAvailable = true;
				}
				var checkStage0 = checkInfo.checkStages[0];
				isAvailable &= checkStage0.lvmin == 0 ? true : player.get_level () >= checkStage0.lvmin;
				isAvailable &= checkStage0.lvmax == 0 ? true : player.get_level () <= checkStage0.lvmax;
				isAvailable &= checkStage0.level == 0 ? true : player.get_level () == checkStage0.level;

				foreach (var item in checkStage0.items)
				{
					isAvailable &= item.count <= player.get_inventory ().get_total_item_count (item.id);
				}

				foreach (var job in checkStage0.jobs)
				{
					isAvailable |= (job == player.get_stats ().get_job ().get_id ());
				}

				if (isAvailable)
				{
					available_QuestId_Info_Dict.Add (questId, questInfo);
				}
			}
		}
	}


}


