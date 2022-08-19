#define USE_NX

using System;
using System.Collections.Generic;
using System.Linq;
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
			findQuest ();

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

		// Returns the NPC's function description or title
		public string get_func ()
		{
			return func;
		}
		SortedDictionary<int, QuestInfo> available_Quests = new SortedDictionary<int, QuestInfo>();
		SortedDictionary<int, QuestInfo> inProgress_Quests = new SortedDictionary<int, QuestInfo>();
		QuestLog questLog => Stage.Instance.get_player ().get_questlog ();
		CheckLog checkLog => Stage.Instance.get_player ().get_checklog ();
		Quest quest => Stage.Instance.get_player ().get_quest ();
		private void findQuest()
		{
			foreach (var questId in questLog.In_progress.Keys)
			{
				inProgress_Quests.Add (questId, questLog.GetQuestInfo (questId));
			}

			quest.GetAvailable_Quest ();
			foreach (var pair in quest.AvailableQuests)
			{
				var questId = pair.Key;
				var questInfo = pair.Value;
				var checkInfo = checkLog.GetCheckInfo (questId);
				if (checkInfo.checkStages[0].npc == npcid)
				{
					available_Quests.Add (questId, questInfo);
				}
			}

		}

		public bool hasQuest()
		{
			return available_Quests.Count != 0 || inProgress_Quests.Count != 0;
		}
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