using System.Collections.Generic;
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
	public class NameTag
	{
		public NameTag (WzObject src, Text.Font f, string n)
		{
			name = new OutlinedText (f, Text.Alignment.CENTER, Color.Name.EAGLE, Color.Name.JAMBALAYA);
			name.change_text (n);

			textures[false].Add (src["0"]["0"]);
			textures[false].Add (src["0"]["1"]);
			textures[false].Add (src["0"]["2"]);

			textures[true].Add (src["1"]["0"]);
			textures[true].Add (src["1"]["1"]);
			textures[true].Add (src["1"]["2"]);

			selected = false;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point<short> position) const
		public void draw (Point<short> position)
		{
			position.shift (new Point<short> (1, 2));

			var tag = textures[selected];

			short width = name.width ();

			// If ever changing startpos, confirm with UICharSelect.cpp
			Point<short> startpos = position - new Point<short> ((short)(6 + width / 2), 0);

			tag[0].draw (startpos);
			tag[1].draw (new DrawArgument (startpos + new Point<short> (6, 0), new Point<short> (width, 0)));
			tag[2].draw (new DrawArgument (startpos + new Point<short> ((short)(width + 6), 0)));

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: name.draw(position);
			name.draw (position);
		}

		public void set_selected (bool s)
		{
			selected = s;

			if (s)
			{
				name.change_color (Color.Name.WHITE);
			}
			else
			{
				name.change_color (Color.Name.EAGLE);
			}
		}

		private OutlinedText name = new OutlinedText ();
		private BoolPair<List<Texture>> textures = new BoolPair<List<Texture>> (new List<Texture> (), new List<Texture> ());
		private bool selected;
	}
}