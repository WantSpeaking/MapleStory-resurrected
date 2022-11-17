using System.Collections;
using System.Collections.Generic;
using server.quest;
using server.quest.requirements;
using UnityEngine;

namespace client
{
	public class MapleQuestStatus
	{
		public enum Status
		{
			UNDEFINED = -1,
			NOT_STARTED = 0,
			STARTED = 1,
			COMPLETED = 2
		}

		public MapleQuestStatus.Status status;
		public int Forfeited;

		private Dictionary<int, string> progress = new Dictionary<int, string> ();

		private int npc;
		private long completionTime, expirationTime;
		private int forfeited = 0, completed = 0;
		private string customData;
		short questID;
		short infoNumber;


		public long CompletionTime { get => completionTime; set => completionTime = value; }
		public long ExpirationTime { get => expirationTime; set => expirationTime = value; }
		public short QuestID { get => questID; set => questID = value; }
		public short InfoNumber { get => infoNumber; set => infoNumber = value; }
		public int Forfeited1 { get => forfeited; set => forfeited = value; }
		public int Completed { get => completed; set => completed = value; }
		public int Npc
		{
			get
			{
				if (npc == 0)
				{
					if (getStatus () == Status.NOT_STARTED)
					{
						npc = MapleQuest.getInstance (QuestID)?.getStartReqNpc () ?? 0;
					}
					if (getStatus () == Status.STARTED)
					{
						npc = MapleQuest.getInstance (QuestID)?.getCompleteReqNpc () ?? 0;
					}
				}
				return npc;
			}
			set => npc = value;
		}
		public Dictionary<int, string> Progress { get => progress; }

		public MapleQuestStatus (short questID)
		{
			this.QuestID = questID;
			this.completionTime = System.DateTime.Now.Millisecond;
			this.expirationTime = 0;
		}
		public MapleQuestStatus (short questID, Status s)
		{
			this.QuestID = questID;
			this.setStatus (s);
			this.completionTime = System.DateTime.Now.Millisecond;
			this.expirationTime = 0;
		}

		public MapleQuestStatus (MapleQuest quest, Status s)
		{
			this.QuestID = quest.Id;
			this.setStatus (s);
			this.completionTime = System.DateTime.Now.Millisecond;
			this.expirationTime = 0;
		}
		public MapleQuestStatus (MapleQuest quest, Status s, int npc)
		{
			this.questID = quest.Id;
			this.setStatus (s);
			this.Npc = npc;
			this.completionTime = DateTimeHelper.CurrentUnixTimeMillis ();
			this.expirationTime = 0;
			//this.updated = true;
			/*	if (status == Status.STARTED)
				{
					registerMobs ();
				}*/
		}

		public Status getStatus ()
		{
			return status;
		}
		public void setStatus (Status s)
		{
			status = s;
		}
		public virtual void setProgress (int id, string pr)
		{
			Progress[id] = pr;
			//this.setUpdated();
		}

		public string getProgress (int id)
		{
			string ret = Progress.TryGetValue (id);
			if (ret == null)
			{
				return "";
			}
			else
			{
				return ret;
			}
		}
		public MapleQuest getQuest ()
		{
			return MapleQuest.getInstance (QuestID);
		}
		public IList<string> getInfoEx ()
		{
			MapleQuest q = this.getQuest ();
			Status s = this.getStatus ();

			return q.getInfoEx (s);
		}

		public override string ToString ()
		{
			return $"Quest id:{questID}\t Status:{getStatus()}";
		}
	}
}

