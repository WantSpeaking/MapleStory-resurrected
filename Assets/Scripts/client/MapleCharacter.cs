using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ms;
using server.quest;
using UnityEngine;

namespace client
{
	public class MapleCharacter : Singleton<MapleCharacter>
	{
		public static MapleCharacter Player => Instance;
		public int MapId;
		private Dictionary<short, MapleQuestStatus> quests = new Dictionary<short, MapleQuestStatus> ();
		private Dictionary<MapleQuest, long> questExpirations = new Dictionary<MapleQuest, long> ();

		public MapleCharacter ()
		{
			CanStartQuests = new ReadOnlyDictionary<short, MapleQuest> (canStart_Quest);
		}
		/*		ms.Quest quest => ms.Stage.get ().get_player ().get_quest ();
				ms.QuestLog questLog => quest.questLog;*/
		public bool hasBuffFromSourceid (int buffId)
		{
			return false;//todo hasBuffFromSourceid
		}
		public virtual void updateQuestStatus (MapleQuestStatus qs)
		{
			if (quests.ContainsKey (qs.QuestID))
			{
				quests[qs.QuestID] = qs;
			}
			else
			{
				quests.Add (qs.QuestID, qs);
			}
		}

		public MapleQuestStatus getQuest (int quest)
		{
			return getQuest (MapleQuest.getInstance (quest));
		}

		public MapleQuestStatus getQuest (MapleQuest quest)
		{
			short questid = quest.Id;
			MapleQuestStatus qs = quests.TryGetValue (questid);
			if (qs == null)
			{
				qs = new MapleQuestStatus (quest, MapleQuestStatus.Status.NOT_STARTED);
				quests.Add (questid, qs);
			}
			return qs;
		}

		public virtual void setQuestProgress (int id, int infoNumber, string progress)
		{
			MapleQuest q = MapleQuest.getInstance (id);
			MapleQuestStatus qs = getQuest (q);

			if (qs.InfoNumber == infoNumber && infoNumber > 0)
			{
				MapleQuest iq = MapleQuest.getInstance (infoNumber);
				MapleQuestStatus iqs = getQuest (iq);
				iqs.setProgress (0, progress);
			}
			else
			{
				qs.setProgress (infoNumber, progress); // quest progress is thoroughly a string match, infoNumber is actually another questid
			}
		}

		public virtual void questTimeLimit (MapleQuest quest, int seconds)
		{
			registerQuestExpire (quest, seconds * 1000);
		}
		public virtual void questTimeLimit2 (MapleQuest quest, long expires)
		{
			long timeLeft = expires - DateTimeHelper.CurrentUnixTimeMillis ();

			if (timeLeft <= 0)
			{
				expireQuest (quest);
			}
			else
			{
				registerQuestExpire (quest, timeLeft);
			}
		}

		public List<MapleQuest> getCanStartedQuests ()
		{
			var ret = new List<MapleQuest> ();
			foreach (var qs in canStart_Quest.Values)
			{
				ret.Add (qs);
			}
			return ret;
		}
		public List<MapleQuestStatus> getNotStartedQuests ()
		{
			var ret = new List<MapleQuestStatus> ();
			foreach (MapleQuestStatus qs in quests.Values)
			{
				if (qs.getStatus ().Equals (MapleQuestStatus.Status.NOT_STARTED))
				{
					ret.Add (qs);
				}
			}
			return ret;
		}
		public List<MapleQuestStatus> getStartedQuests ()
		{
			var ret = new List<MapleQuestStatus> ();
			foreach (MapleQuestStatus qs in quests.Values)
			{
				if (qs.getStatus ().Equals (MapleQuestStatus.Status.STARTED) && MapleQuest.getInstance(qs.QuestID)!= null)
				{
					ret.Add (qs);
				}
			}
			return ret;
		}
		public List<MapleQuestStatus> getCompletedQuests ()
		{
			var ret = new List<MapleQuestStatus> ();
			foreach (MapleQuestStatus qs in quests.Values)
			{
				if (qs.getStatus ().Equals (MapleQuestStatus.Status.COMPLETED))
				{
					ret.Add (qs);
				}
			}
			return ret;
		}

		public string getQuestProgress (int id, int infoNumber)
		{
			MapleQuestStatus qs = getQuest (MapleQuest.getInstance (id));

			if (qs.InfoNumber == infoNumber && infoNumber > 0)
			{
				qs = getQuest (MapleQuest.getInstance (infoNumber));
				infoNumber = 0;
			}

			if (qs != null)
			{
				return qs.getProgress (infoNumber);
			}
			else
			{
				return "";
			}
		}

		private void expireQuest (MapleQuest quest)
		{
			if (quest.forfeit (this))
			{
			}
		}

		TimedQueue.Timed questExpireTask;

		private void registerQuestExpire (MapleQuest quest, long time)
		{
			try
			{
				questExpireTask = TimerManager.Instance.register (() =>
				{
					runQuestExpireTask ();
				}, 10 * 1000);

				questExpirations[quest] = System.DateTime.Now.Millisecond + time;
			}
			finally
			{
			}
		}

		private void runQuestExpireTask ()
		{
			try
			{
				long timeNow = System.DateTime.Now.Millisecond;
				var expireList = new LinkedList<MapleQuest> ();

				foreach (KeyValuePair<MapleQuest, long> qe in questExpirations)
				{
					if (qe.Value <= timeNow)
					{
						expireList.AddLast (qe.Key);
					}
				}

				if (expireList.Count > 0)
				{
					foreach (MapleQuest quest in expireList)
					{
						expireQuest (quest);
						questExpirations.Remove (quest);
					}

					if (questExpirations.Count == 0)
					{
						questExpireTask.cancel (false);
						questExpireTask = null;
					}
				}
			}
			finally
			{
			}
		}

		public void LogAllQuest ()
		{
			/*AppDebug.Log("NotStartedQuests");
			AppDebug.Log(getNotStartedQuests().ToDebugLog());
			AppDebug.Log("StartedQuests");
            AppDebug.Log(getStartedQuests().ToDebugLog());
            AppDebug.Log("CompletedQuests");
            AppDebug.Log(getCompletedQuests().ToDebugLog());*/

        }

        private readonly SortedDictionary<short, MapleQuest> canStart_Quest = new SortedDictionary<short, MapleQuest> ();
		public ReadOnlyDictionary<short, MapleQuest> CanStartQuests;

		public IEnumerator RefreshCanStart_Quest (bool forceGet = false)
		{
			canStart_Quest.Clear ();
			foreach (var id_quest_pair in MapleQuest.getAllQuest())
			{
				var questId = id_quest_pair.Key;
				var quest = id_quest_pair.Value;
				if (id_quest_pair.Value.canStart (Player, quest.getStartReqNpc ()))
				{
					canStart_Quest.Add ((short)questId, quest);
					yield return null;
				}
			}
		}
	}
}

