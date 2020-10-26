#define USE_NX

using System;
using System.Collections.Generic;
using System.Linq;
using MapleLib.WzLib;

//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////



namespace ms
{
	// Represents a NPC on the current map
	// Implements the 'MapObject' interface to be used in a 'MapObjects' template
	public class Npc : MapObject
	{
		// Constructs an NPC by combining data from game files with data sent by the server
		public Npc(int id, int o, bool fl, ushort f, bool cnt, Point<short> position) : base(o,position)
		{
			string strid = Convert.ToString(id);
			strid = strid.insert(0, 7 - strid.Length, '0');
			strid.append(".img");

			WzObject src = nl.nx.wzFile_npc[strid];
			WzObject strsrc = nl.nx.wzFile_string["Npc.img"][Convert.ToString(id)];

			string link = src["info"]["link"].ToString ();

			if (link.Length > 0)
			{
				link.append(".img");
				src = nl.nx.wzFile_npc[link];
			}

			WzObject info = src["info"];

			hidename = info["hideName"];
			mouseonly = info["talkMouseOnly"];
			scripted = info["script"].Any() || info["shop"];

			foreach (var npcnode in src)
			{
				string state = npcnode.Name;

				if (state != "info")
				{
					animations[state] = npcnode;
					states.Add(state);
				}

				foreach (var speaknode in npcnode["speak"])
				{
					lines[state].Add(strsrc[speaknode.ToString()].ToString ());
				}
			}

			name = strsrc["name"].ToString ();
			func = strsrc["func"].ToString ();

			namelabel = new Text(Text.Font.A13B, Text.Alignment.CENTER, Color.Name.YELLOW, Text.Background.NAMETAG, name);
			funclabel = new Text(Text.Font.A13B, Text.Alignment.CENTER, Color.Name.YELLOW, Text.Background.NAMETAG, func);

			npcid = id;
			flip = !fl;
			control = cnt;
			stance = "stand";

			phobj.fhid = f;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: set_position(position);
			set_position(position);
		}

		// Draws the current animation and name/function tags
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(double viewx, double viewy, float alpha) const override
		public override void draw(double viewx, double viewy, float alpha)
		{
			Point<short> absp = phobj.get_absolute(viewx, viewy, alpha);

			if (animations.count(stance)>0)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: animations.at(stance).draw(DrawArgument(absp, flip), alpha);
				animations[stance].draw(new DrawArgument(absp, flip), alpha);
			}

			if (!hidename)
			{
				// If ever changing code for namelabel confirm placements with map 10000
				namelabel.draw(absp + new Point<short>(0, -4));
				funclabel.draw(absp + new Point<short>(0, 18));
			}
		}
		// Updates the current animation and physics
		public override sbyte update(Physics physics)
		{
			if (!active)
			{
				return phobj.fhlayer;
			}

			physics.move_object(phobj);

			if (animations.count(stance)>0)
			{
				bool aniend = animations[stance].update();

				if (aniend && states.Count > 0)
				{
					int next_stance = random.next_int(states.Count);
					string new_stance = states[next_stance];
					set_stance(new_stance);
				}
			}

			return phobj.fhlayer;
		}

		// Changes stance and resets animation
		public void set_stance(string st)
		{
			if (stance != st)
			{
				stance = st;
				if (!animations.ContainsKey (stance))
				{
					return;
				}

				animations[st].reset();
			}
		}

		// Check whether this is a server-sided NPC
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool isscripted() const
		public bool isscripted()
		{
			return scripted;
		}
		// Check if the NPC is in range of the cursor
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool inrange(Point<short> cursorpos, Point<short> viewpos) const
		public bool inrange(Point<short> cursorpos, Point<short> viewpos)
		{
			if (!active)
			{
				return false;
			}

			Point<short> absp = get_position() + viewpos;

			Point<short> dim = animations.count(stance) >0? animations[stance].get_dimensions() : new Point<short>();

			return new Rectangle<short>((short)(absp.x() - dim.x() / 2), (short)(absp.x() + dim.x() / 2), (short)(absp.y() - dim.y()), absp.y()).contains(cursorpos);
		}

		// Returns the NPC name
		public string get_name()
		{
			return name;
		}
		// Returns the NPC's function description or title
		public string get_func()
		{
			return func;
		}

		private SortedDictionary<string, Animation> animations = new SortedDictionary<string, Animation>();
		private SortedDictionary<string, List<string>> lines = new SortedDictionary<string, List<string>>();
		private List<string> states = new List<string>();
		private string name;
		private string func;
		private bool hidename;
		private bool scripted;
		private bool mouseonly;

		private int npcid;
		private bool flip;
		private string stance;
		private bool control;

		private Randomizer random = new Randomizer();
		private Text namelabel = new Text();
		private Text funclabel = new Text();
	}
}

#if USE_NX
#endif
