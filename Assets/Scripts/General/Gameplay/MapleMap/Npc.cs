#define USE_NX

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using client;
using MapleLib.WzLib;
using server.quest;
using Sirenix.Serialization;
using tools;

namespace ms
{
	// Represents a NPC on the current map
	// Implements the 'MapObject' interface to be used in a 'MapObjects' template
	public class Npc : MapObject
	{
		// Constructs an NPC by combining data from game files with data sent by the server
		public Npc (int id, int o, bool fl, ushort f, bool cnt, Point_short position) : base (o, position)
		{
			//AppDebug.Log ($"npc {id} on ctor");

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
			//scripted = info["script"]?.Any () ?? false || info["shop"];
			scripted = info["script"]?.Any () ?? false;
			if (scripted)
			{
				ScriptInfos = new List<string> ();
				foreach (var src_script_0 in info["script"])
				{
					var script = src_script_0?["script"]?.ToString ();
					var ScriptInfo = "对话或进入";
					if (script!= null)
					{
						ScriptInfo = ms.wz.wzFile_etc["ScriptInfo.img"]?[script]?.ToString ()??script;
					}
					ScriptInfos.Add (ScriptInfo);
				}
			}

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

			questIcon_CanStart = wz.wzFile_ui["UIWindow.img"]["QuestIcon"]["0"];
			questIcon_InProgressed = wz.wzFile_ui["UIWindow.img"]["QuestIcon"]["1"];
			questIcon_CanComplete = wz.wzFile_ui["UIWindow.img"]["QuestIcon"]["2"];

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
            //Point_short absp = phobj.get_position ();

			if (animations.count (stance) > 0)
			{
				//animations[stance].draw (new DrawArgument (new Point_short (absp), flip, 8, 0), alpha);
				animations[stance].draw (new DrawArgument (new Point_short (absp), flip).SetParent (MapGameObject), alpha);
			}

			if (!hidename)
			{
				// If ever changing code for namelabel confirm placements with map 10000
				namelabel.draw (absp + new Point_short (0, -4));
				funclabel.draw (absp + new Point_short (0, 18));
			}

			lastDraw_Stance = stance;

			questIcon_Current?.draw (new DrawArgument (absp + new Point_short (0, -charHeight), flip).SetParent (MapGameObject), alpha);
		}

		// Updates the current animation and physics
		public override sbyte update (Physics physics)
		{
			if (!active)
			{
				return phobj.fhlayer;
			}

			//physics.move_object (phobj);

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

			return new Rectangle_short ((short)(absp.x () - dim.x () / 2), (short)(absp.x () + dim.x () / 2), (short)(absp.y () - dim.y ()*2f), absp.y ()).contains (cursorpos);
		}

		// Returns the NPC name
		public string get_name ()
		{
			return name;
		}

		public int get_id ()
		{
			return npcid;
		}

		// Returns the NPC's function description or title
		public string get_func ()
		{
			return func;
		}

		private bool check_Has_CanCompleteQuest (ref int canCompleteQuestId)
		{
			if (!check_Has_StartedQuest ())
				return false;

			bool isAvailable = false;
			foreach (var pair in started_Quests)
			{
				var questId = pair.Key;
				var mapleQuest = pair.Value;

				if (mapleQuest == null)
				{
					AppDebug.LogError($"mapleQuest == null,Id:{questId}");
					continue;
				}
                if (MapleCharacter.Player == null)
                {
                    AppDebug.LogError($"MapleCharacter.Player == null");
                }

                isAvailable = mapleQuest.canComplete (MapleCharacter.Player, npcid);
				if (isAvailable)
				{
					canCompleteQuestId = questId;
					break;
				}
			}
			return isAvailable;
		}

        private bool check_Has_StartedQuest ()
		{
			return started_Quests.Count != 0;
		}
		private bool check_Has_CanStartQuest ()
		{
			return canStarted_Quests.Count != 0;
		}
		SortedDictionary<int, MapleQuest> canStarted_Quests = new SortedDictionary<int, MapleQuest> ();
		SortedDictionary<int, MapleQuest> started_Quests = new SortedDictionary<int, MapleQuest> ();
		QuestLog questLog => Stage.Instance.get_player ().get_questlog ();
		CheckLog checkLog => Stage.Instance.get_player ().get_checklog ();
		SayLog sayLog => Stage.Instance.get_player ().get_saylog ();
		Quest quest => Stage.Instance.get_player ().get_quest ();

		public void UpdateQuest (bool noticeUpdate = false)
		{
			canStarted_Quests.Clear ();
			started_Quests.Clear ();

			MapleCharacter.Player.RefreshCanStart_Quest ();
			MapleCharacter.Player.LogAllQuest ();
			
			foreach (var qs in MapleCharacter.Player.CanStartQuests)
			{
				var questId = qs.Key;
				//var sayInfo = sayLog.GetSayInfo (questId);

				//var checkInfo = checkLog.GetCheckInfo (questId);
				if (qs.Value.getStartReqNpc () == npcid)
				{
					canStarted_Quests.Add (questId, MapleQuest.getInstance (qs.Value.Id));
				}
			}

			foreach (var qs in MapleCharacter.Player.getStartedQuests ())
			{
				var questId = qs.QuestID;
				//var checkInfo = checkLog.GetCheckInfo (questId);

				var questNpcId = qs.Npc;
				//var questNpcId = checkInfo.checkStages.TryGet (1).npc != 0 ? checkInfo.checkStages.TryGet (1).npc : checkInfo.checkStages.TryGet (0).npc;
				if (qs.StartReqNpc == npcid || qs.CompleteReqNpc == npcid)
				{
					started_Quests.Add (questId, MapleQuest.getInstance (qs.QuestID));
				}
			}

			int canCompleteQuestId = 0;
			if (check_Has_CanCompleteQuest (ref canCompleteQuestId))
			{
				questIcon_Current = questIcon_CanComplete;
				if (noticeUpdate)
					ms_Unity.FGUI_Notice.ShowNotice ($"可完成任务：{MapleQuest.getInstance (canCompleteQuestId).Name}");
			}
			else if (check_Has_StartedQuest ())
			{
				questIcon_Current = questIcon_InProgressed;
			}
			else if (check_Has_CanStartQuest ())
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
			questChooseList.Clear();

            stringBuilder.Clear ();
			var index = 0;

			if (canStarted_Quests.Count > 0)
			{
				//stringBuilder.Append ($"{}");
				stringBuilder.AppendLine ($"可开始的任务");
				foreach (var pair in canStarted_Quests)
				{
					questChooseList.Add((pair.Value,0));

                    stringBuilder.Append ($"<a href=\"{index++}\" target=\"_blank\">{pair.Value.Name}</a>\r\n");
                }
			}

            //已经开始的任务 由于npc条件的不同，可以是正在进行的状态，也可以是可完成状态
            if (started_Quests.Count > 0)
			{
                var inProgressString = "";
                foreach (var pair in started_Quests)
				{
					if (!pair.Value.canComplete(MapleCharacter.Player,npcid))//如果不能完成，就是正在进行的
					{
                        questChooseList.Add((pair.Value,1));
                        inProgressString += ($"<a href=\"{index++}\" target=\"_blank\">{pair.Value.Name}</a>\r\n");
					}
                }

                if (!string.IsNullOrEmpty (inProgressString))
                {
	                stringBuilder.AppendLine ($"\r\n正在进行的任务");
	                stringBuilder.Append (inProgressString);
                }

                var completeString = "";
                foreach (var pair in started_Quests)
                {
                    if (pair.Value.canComplete(MapleCharacter.Player, npcid))//如果能完成，就是可完成的
                    {
                        questChooseList.Add((pair.Value,1));
                        completeString += ($"<a href=\"{index++}\" target=\"_blank\">{pair.Value.Name}</a>\r\n");
                    }
                }

                if (!string.IsNullOrEmpty (completeString))
                {
	                stringBuilder.AppendLine ($"\r\n可完成的任务:");
	                stringBuilder.Append (completeString);
                }
            }

			if (scripted)
			{
				stringBuilder.AppendLine ($"\r\n其它:");
				foreach (var scriptInfo in ScriptInfos)
				{
                    stringBuilder.Append($"<a href=\"{index++}\" target=\"_blank\">{scriptInfo}</a>\r\n");
				}
			} 
			return stringBuilder.ToString ();
		}

		public SayPage getInitPage ()
		{
			var sayPage = new SayPage ();
			sayPage.npcId = npcid;
			sayPage.pageIndex = -1;
			sayPage.text = getQuestListString ();

			return sayPage;
		}

		static List<(MapleQuest,int)> questChooseList = new();
		public (MapleQuest,int) GetQuestSayInfo (short selectQuestIndex)
		{
			ValueTuple<MapleQuest,int> pair;
			 pair = questChooseList.TryGet(selectQuestIndex);
			 if (pair.Item1 == null)
			 {
				 pair = (null, selectQuestIndex - questChooseList.Count);
			 }
			 
            return pair;
            /*short index = 0;

			if (canStarted_Quests.Count > 0)
			{
				foreach (var pair in canStarted_Quests)
				{
					if (index == selectQuestIndex)
					{
						isQuestStarted = false;
						return pair.Value;
					}
					index++;
				}
			}

			if (started_Quests.Count > 0)
			{
				foreach (var pair in started_Quests)
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
			return null;*/
		}
		public bool hasQuest ()
		{
			UpdateQuest ();
			return canStarted_Quests.Count != 0 || started_Quests.Count != 0;
		}

		public override void Dispose ()
		{
			base.Dispose ();
			foreach (var pair in animations)
			{
				pair.Value.Dispose ();
			}
			animations.Clear ();
			animations = null;
			
			questIcon_CanStart?.Dispose ();
			questIcon_InProgressed?.Dispose ();
			questIcon_CanComplete?.Dispose ();
			
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
		public List<string> ScriptInfos;
		
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