#define USE_NX

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapleLib.WzLib;





namespace ms
{
	// Represents a NPC on the current map
	// Implements the 'MapObject' interface to be used in a 'MapObjects' template
	public class Npc : MapObject
	{
		// Constructs an NPC by combining data from game files with data sent by the server
		public Npc (int id, int o, bool fl, ushort f, bool cnt, Point_short position) : base (o, position)
		{
			string strid = Convert.ToString (id);
			strid = strid.insert (0, 7 - strid.Length, '0');
			strid = strid.append (".img");

			WzObject src = ms.wz.wzFile_npc[strid];
			WzObject strsrc = ms.wz.wzFile_string["Npc.img"][Convert.ToString (id)];

			string link = src?["info"]?["link"]?.ToString ();

			if (!string.IsNullOrEmpty (link) && link.Length > 0)
			{
				link.append (".img");
				src = ms.wz.wzFile_npc[link];
			}

			if (src == null)
			{
				AppDebug.Log ($"cant't find Npc strid:{strid}\t id:{id}");
				return;
			}

			WzObject info = src["info"];

			hidename = info["hideName"];
			mouseonly = info["talkMouseOnly"];
			scripted = info["script"]?.Any () ?? false || info["shop"];

			foreach (var npcnode in src)
			{
				string state = npcnode.Name;

				if (state != "info")
				{
					animations[state] = npcnode;
					states.Add (state);
				}

				if (npcnode["speak"] is WzImageProperty property_speak)
				{
					foreach (var speaknode in property_speak)
					{
						var a = speaknode;
						var b = state;
						var c = speaknode.ToString ();
						var d = strsrc?[speaknode.ToString ()];
						var e = strsrc?[speaknode?.ToString ()]?.ToString ();

						//AppDebug.Log ($"a:{a}\t b:{b}\t c:{c}\t d:{d}\t e:{e}");
						lines.TryAdd (state).TryGetValue (state)?.Add (strsrc?[speaknode.ToString ()]?.ToString ());
						//lines[state].Add (strsrc[speaknode.ToString ()].ToString ());
					}
				}
			}

			name = strsrc?["name"]?.ToString ();
			func = strsrc?["func"]?.ToString ();

			namelabel = new Text (Text.Font.A13B, Text.Alignment.CENTER, Color.Name.YELLOW, Text.Background.NAMETAG, name);
			funclabel = new Text (Text.Font.A13B, Text.Alignment.CENTER, Color.Name.YELLOW, Text.Background.NAMETAG, func);

			npcid = id;
			flip = !fl;
			control = cnt;
			stance = "stand";

			phobj.fhid = f;
			set_position (new Point_short (position));

			questIcon_CanStart = wz.wzFile_ui["UIWindow2.img"]["QuestIcon"]["0"];
			questIcon_InProgressed = wz.wzFile_ui["UIWindow2.img"]["QuestIcon"]["1"];
			questIcon_CanComplete = wz.wzFile_ui["UIWindow2.img"]["QuestIcon"]["2"];

			UpdateQuest ();
		}

		private string lastDraw_Stance = string.Empty;

		// Draws the current animation and name/function tags
		public override void draw (double viewx, double viewy, float alpha)
		{
			if (!string.IsNullOrEmpty (lastDraw_Stance))
			{
				animations[lastDraw_Stance].eraseAllFrame ();
			}

			Point_short absp = phobj.get_absolute (viewx, viewy, alpha);

			if (animations.count (stance) > 0)
			{
				//animations[stance].draw (new DrawArgument (new Point_short (absp), flip, 8, 0), alpha);
				animations[stance].draw (new DrawArgument (new Point_short (absp), flip), alpha);
			}

			if (!hidename)
			{
				// If ever changing code for namelabel confirm placements with map 10000
				namelabel.draw (absp + new Point_short (0, -4));
				funclabel.draw (absp + new Point_short (0, 18));
			}

			lastDraw_Stance = stance;

			questIcon_Current?.draw (new DrawArgument (absp + new Point_short (0, -charHeight), flip), alpha);
		}

		// Updates the current animation and physics
		public override sbyte update (Physics physics)
		{
			if (!active)
			{
				return phobj.fhlayer;
			}

			physics.move_object (phobj);

			if (animations.count (stance) > 0)
			{
				bool aniend = animations[stance].update ();

				if (aniend && states.Count > 0)
				{
					int next_stance = random.next_int (states.Count);
					string new_stance = states[next_stance];
					set_stance (new_stance);
				}
			}


			questIcon_Current?.update ();

			return phobj.fhlayer;
		}

		// Changes stance and resets animation
		public void set_stance (string st)
		{
			if (stance != st)
			{
				stance = st;
				if (!animations.ContainsKey (stance))
				{
					return;
				}

				animations[st].reset ();
			}
		}

		// Check whether this is a server-sided NPC
		public bool isscripted ()
		{
			return scripted;
		}

		// Check if the NPC is in range of the cursor
		public bool inrange (Point_short cursorpos, Point_short viewpos)
		{
			if (!active)
			{
				return false;
			}

			Point_short absp = get_position () + viewpos;

			Point_short dim = animations.count (stance) > 0 ? animations[stance].get_dimensions () : new Point_short ();

			return new Rectangle_short ((short)(absp.x () - dim.x () / 2), (short)(absp.x () + dim.x () / 2), (short)(absp.y () - dim.y ()), absp.y ()).contains (cursorpos);
		}

		// Returns the NPC name
		public string get_name ()
		{
			return name;
		}

		public int get_id()
		{
			return npcid;
		}

		// Returns the NPC's function description or title
		public string get_func ()
		{
			return func;
		}

		private bool check_Has_CanCompleteQuest(ref short canCompleteQuestId)
		{
			if(!check_Has_InProgressQuest()) return false;

			bool isAvailable = true;

			foreach (var pair in inProgress_Quests)
			{
				var questId = pair.Key;
				var checkInfo = checkLog.GetCheckInfo (questId);
				var checkStage0 = checkInfo.checkStages[1];
				var questInfo = questLog.GetQuestInfo (questId);
				var player = ms.Stage.Instance.get_player ();

				foreach (var item in checkStage0.items)
				{
					isAvailable &= player.get_inventory ().hasEnoughItem (item.id, item.count);
				}

				foreach (var mob in checkStage0.mobs)
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
					canCompleteQuestId = questId;
					break;
				}
			}
			return isAvailable;

		}
		private bool check_Has_InProgressQuest ()
		{
			return inProgress_Quests.Count != 0;
		}
		private bool check_Has_AvailableQuest ()
		{
			return available_Quests.Count != 0;
		}
		SortedDictionary<short, SayInfo> available_Quests = new SortedDictionary<short, SayInfo> ();
		SortedDictionary<short, SayInfo> inProgress_Quests = new SortedDictionary<short, SayInfo> ();
		QuestLog questLog => Stage.Instance.get_player ().get_questlog ();
		CheckLog checkLog => Stage.Instance.get_player ().get_checklog ();
		SayLog sayLog => Stage.Instance.get_player ().get_saylog ();
		Quest quest => Stage.Instance.get_player ().get_quest ();
		public void UpdateQuest ()
		{
			inProgress_Quests.Clear ();
			available_Quests.Clear ();

			foreach (var pair in questLog.In_progress)
			{
				var questId = pair.Key;
				var sayInfo = sayLog.GetSayInfo (questId);
				var checkInfo = checkLog.GetCheckInfo (questId);
				if (checkInfo.checkStages[1].npc == npcid)
				{
					inProgress_Quests.Add (questId, sayInfo);
				}
			}

			foreach (var pair in quest.AvailableQuests)
			{
				var questId = pair.Key;
				var questInfo = pair.Value;
				var sayInfo = sayLog.GetSayInfo (questId);
				var checkInfo = checkLog.GetCheckInfo (questId);
				if (checkInfo.checkStages[0].npc == npcid)
				{
					available_Quests.Add (questId, sayInfo);
				}
			}
			short canCompleteQuestId = 0;
			if (check_Has_CanCompleteQuest(ref canCompleteQuestId))
			{
				questIcon_Current = questIcon_CanComplete;
				ms_Unity.FGUI_Notice.ShowNotice ($"可完成任务：{questLog.GetQuestInfo(canCompleteQuestId).Name}");
			}
			else if(check_Has_InProgressQuest())
			{
				questIcon_Current = questIcon_InProgressed;
			}
			else if (check_Has_AvailableQuest ())
			{
				questIcon_Current = questIcon_CanStart;
			}
			else
			{
				questIcon_Current = null;
			}
		}

		StringBuilder stringBuilder = new StringBuilder ();
		/// <summary>
		/// 拼装 npc对话框内的string 包含 available_Quests 、inProgress_Quests 列表 以供选择
		/// </summary>
		/// <returns></returns>
		public string getQuestListString ()
		{
			stringBuilder.Clear ();
			var index = 0;

			if (available_Quests.Count > 0)
			{
				//stringBuilder.Append ($"{}");
				stringBuilder.AppendLine ($"可开始的任务");
				foreach (var pair in available_Quests)
				{

					stringBuilder.Append ($"#L{index++}# {pair.Value.questName} #l \r\n");
				}
			}

			if (inProgress_Quests.Count > 0)
			{
				//stringBuilder.Append ($"{}");
				stringBuilder.AppendLine ($"正在进行的任务");
				foreach (var pair in inProgress_Quests)
				{
					stringBuilder.Append ($"#L{index++}# {pair.Value.questName} #l \r\n");
				}
			}
			return stringBuilder.ToString ();
		}

		public SayPage getInitPage()
		{
			var sayPage = new SayPage ();
			sayPage.npcId = npcid;
			sayPage.pageIndex = -1;
			sayPage.text = getQuestListString ();

			return sayPage;
		}

		public SayInfo GetQuestSayInfo(short selectQuestIndex, out bool isQuestStarted)
		{
			short index = 0;

			if (available_Quests.Count > 0)
			{
				foreach (var pair in available_Quests)
				{
					if (index == selectQuestIndex)
					{
						isQuestStarted = false;
						return pair.Value;
					}
					index++;
				}
			}

			if (inProgress_Quests.Count > 0)
			{
				foreach (var pair in inProgress_Quests)
				{
					if (index == selectQuestIndex)
					{
						isQuestStarted = true;

						return pair.Value;
					}
					index++;
				}
			}
			isQuestStarted = false;
			return null;
		}
		public bool hasQuest ()
		{
			return available_Quests.Count != 0 || inProgress_Quests.Count != 0;
		}

		Animation questIcon_CanStart;
		Animation questIcon_InProgressed;
		Animation questIcon_CanComplete;

		Animation questIcon_Current;

		public const short charWidth = 40;
		public const short charHeight = 90;
		public Point_short charHalf = new Point_short (charWidth / 2, charHeight / 2);

		private SortedDictionary<string, Animation> animations = new SortedDictionary<string, Animation> ();
		private SortedDictionary<string, List<string>> lines = new SortedDictionary<string, List<string>> ();
		private List<string> states = new List<string> ();
		private string name;
		private string func;
		private bool hidename;
		private bool scripted;
		private bool mouseonly;

		private int npcid;
		private bool flip;
		private string stance;
		private bool control;

		private Randomizer random = new Randomizer ();
		private Text namelabel = new Text ();
		private Text funclabel = new Text ();
	}
}

#if USE_NX
#endif