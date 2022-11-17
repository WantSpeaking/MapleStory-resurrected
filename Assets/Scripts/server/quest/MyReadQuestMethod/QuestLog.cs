using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using client;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using provider;
using server.quest;
using server.quest.actions;
using server.quest.requirements;

namespace ms
{
	// Class that stores information on the quest log of an individual character
	public class QuestLog
	{
		/*
		public QuestLog ()
		{
			NotStarted = new ReadOnlyDictionary<short, short> (notStarted);
			Started = new ReadOnlyDictionary<short, string> (started);
			Completed = new ReadOnlyDictionary<short, long> (completed);
		}
				public void add_notStarted (short qid)
				{
					notStarted[qid] = qid;
				}
				public void add_started (short qid, string qdata, bool isOverlay = false)
				{
					if (started.TryGetValue (qid, out var progressData))
					{
						if (isOverlay)
						{
							started[qid] = qdata;
						}
						else
						{
							started[qid] += qdata;
						}
					}
					else
					{
						started[qid] = qdata;
					}

				}
				public void add_completed (short qid, long time)
				{
					completed[qid] = time;
				}
				public bool is_notStarted (short qid)
				{
					return completed.Any (pair => pair.Key == qid);
				}
				public bool is_started (short qid)
				{
					return started.Any (pair => pair.Key == qid);
				}
				public bool is_completed (short qid)
				{
					return completed.Any (pair => pair.Key == qid);
				}
				public string get_started (short qid)
				{
					return started.TryGetValue (qid);
				}
				public void remove_started (short qid)
				{
					if (started.ContainsKey (qid))
					{
						started.Remove (qid);
					}
				}
				public void remove_notStarted (short qid)
				{
					if (notStarted.ContainsKey (qid))
					{
						notStarted.Remove (qid);
					}
				}
				public MapleQuest GetQuestInfo (short questId)
				{
					if (!questId_Info_Dict.TryGetValue (questId, out var questInfo))
					{
						questInfo = new MapleQuest (questId);

						questId_Info_Dict.Add (questId, questInfo);
					}

					return questInfo;
				}
		*/
		private SortedDictionary<short, short> notStarted = new SortedDictionary<short, short> ();

		private SortedDictionary<short, string> started = new SortedDictionary<short, string> ();
		private SortedDictionary<short, long> completed = new SortedDictionary<short, long> ();

		public ReadOnlyDictionary<short, short> NotStarted;
		public ReadOnlyDictionary<short, string> Started;
		public ReadOnlyDictionary<short, long> Completed;


		private SortedDictionary<short, MapleQuest> questId_Info_Dict = new SortedDictionary<short, MapleQuest> ();

		private SortedDictionary<short, MapleQuestStatus> quests = new SortedDictionary<short, MapleQuestStatus> ();

	}


}


