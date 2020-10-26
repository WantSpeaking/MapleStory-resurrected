#define USE_NX

using System;
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
	public class ChatBalloon
	{
		public ChatBalloon (sbyte type)
		{
			string typestr = String.Empty;

			if (type < 0)
			{
				switch (type)
				{
					case -1:
						typestr = "dead";
						break;
				}
			}
			else
			{
				typestr = Convert.ToString (type);
			}

			var src = nl.nx.wzFile_ui["ChatBalloon.img"][typestr];

			arrow = src["arrow"];
			frame = new MapleFrame (src);

			textlabel = new Text (Text.Font.A11M, Text.Alignment.CENTER, Color.Name.BLACK, "", 80);

			duration = 0;
		}

		public ChatBalloon () : this (0)
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point<short> position) const
		public void draw (Point<short> position)
		{
			if (duration == 0)
			{
				return;
			}

			short width = textlabel.width ();
			short height = textlabel.height ();

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: frame.draw(position, width, height);
			frame.draw (position, width, height);
			arrow.draw (position);
			textlabel.draw (position - new Point<short> (0, (short)(height + 4)));
		}

		public void update ()
		{
			duration -= (short)Constants.TIMESTEP;

			if (duration < 0)
			{
				duration = 0;
			}
		}

		public void change_text (string text)
		{
			textlabel.change_text (text);

			duration = DURATION;
		}

		public void expire ()
		{
			duration = 0;
		}

		// How long a line stays on screen
		private const short DURATION = 4000; // 4 seconds

		private MapleFrame frame = new MapleFrame ();
		private Text textlabel = new Text ();
		private Texture arrow = new Texture ();
		private short duration;
	}

	public class ChatBalloonHorizontal
	{
		public ChatBalloonHorizontal ()
		{
			WzObject Balloon = nl.nx.wzFile_ui["Login.img"]["WorldNotice"]["Balloon"];

			arrow = Balloon["arrow"];
			center = Balloon["c"];
			east = Balloon["e"];
			northeast = Balloon["ne"];
			north = Balloon["n"];
			northwest = Balloon["nw"];
			west = Balloon["w"];
			southwest = Balloon["sw"];
			south = Balloon["s"];
			southeast = Balloon["se"];

			xtile = Math.Max (north.width (), (short)1);
			ytile = Math.Max (west.height (), (short)1);

			textlabel = new Text (Text.Font.A13B, Text.Alignment.LEFT, Color.Name.BLACK);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point<short> position) const
		public void draw (Point<short> position)
		{
			short width = (short)(textlabel.width () + 9);
			short height = (short)(textlabel.height () - 2);

			short left = (short)(position.x () - width / 2);
			short top = (short)(position.y () - height);
			short right = (short)(left + width);
			short bottom = (short)(top + height);

			northwest.draw (new DrawArgument (left, top));
			southwest.draw (new DrawArgument (left, bottom));

			for (short y = top; y < bottom; y += ytile)
			{
				west.draw (new DrawArgument (left, y));
				east.draw (new DrawArgument (right, y));
			}

			center.draw (new DrawArgument (new Point<short> ((short)(left - 8), top), new Point<short> ((short)(width + 8), height)));

			for (short x = left; x < right; x += xtile)
			{
				north.draw (new DrawArgument (x, top));
				south.draw (new DrawArgument (x, bottom));
			}

			northeast.draw (new DrawArgument (right, top));
			southeast.draw (new DrawArgument (right, bottom));

			arrow.draw (new DrawArgument (right + 1, top));
			textlabel.draw (new DrawArgument (left + 6, top - 5));
		}

		public void change_text (string text)
		{
			textlabel.change_text (text);
		}

		private Text textlabel = new Text ();
		private Texture arrow = new Texture ();
		private Texture center = new Texture ();
		private Texture east = new Texture ();
		private Texture northeast = new Texture ();
		private Texture north = new Texture ();
		private Texture northwest = new Texture ();
		private Texture west = new Texture ();
		private Texture southwest = new Texture ();
		private Texture south = new Texture ();
		private Texture southeast = new Texture ();
		private short xtile;
		private short ytile;
	}
}


#if USE_NX
#endif