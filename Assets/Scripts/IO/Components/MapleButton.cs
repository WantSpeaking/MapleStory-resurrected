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


using System.Linq;
using MapleLib.WzLib;

namespace ms
{
	// A standard MapleStory button with 4 states and a texture for each state
	public class MapleButton : Button
	{
		public MapleButton(WzObject src, Point<short> pos)
		{
			WzObject normal = src["normal"];

			if (normal.Count() > 1)
			{
				animations[(int)Button.State.NORMAL] = normal;
			}
			else
			{
				textures[(int)Button.State.NORMAL] = normal["0"];
			}

			textures[(int)Button.State.PRESSED] = src["pressed"]["0"];
			textures[(int)Button.State.MOUSEOVER] = src["mouseOver"]["0"];
			textures[(int)Button.State.DISABLED] = src["disabled"]["0"];

			active = true;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: position = pos;
			position=(pos);
			state = Button.State.NORMAL;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this(src, Point<short>(x, y));
		public MapleButton(WzObject src, short x, short y) : this(src, new Point<short>(x, y))
		{
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this(src, Point<short>());
		public MapleButton(WzObject src) : this(src, new Point<short>())
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point<short> parentpos) const
		public override void draw(Point<short> parentpos)
		{
			if (active)
			{
				textures[(int)state].draw(position + parentpos);
				animations[(int)state].draw(position + parentpos, 1.0f);
			}
		}
		public override void update()
		{
			if (active)
			{
				animations[(int)state].update(6);
			}
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Rectangle<short> bounds(Point<short> parentpos) const
		public override Rectangle<short> bounds(Point<short> parentpos)
		{
			Point<short> lt = new Point<short>();
			Point<short> rb = new Point<short>();

			if (textures[(int)state].is_valid())
			{
				lt = parentpos + position - textures[(int)state].get_origin();
				rb = lt + textures[(int)state].get_dimensions();
			}
			else
			{
				lt = parentpos + position - animations[(int)state].get_origin();
				rb = lt + animations[(int)state].get_dimensions();
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: return Rectangle<short>(lt, rb);
			return new Rectangle<short>(lt, rb);
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short width() const
		public override short width()
		{
			return textures[(int)state].width();
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> origin() const
		public override Point<short> origin()
		{
			return textures[(int)state].get_origin();
		}
		public override Cursor.State send_cursor(bool UnnamedParameter1, Point<short> UnnamedParameter2)
		{
			return Cursor.State.IDLE;
		}

		private Texture[] textures =new Texture[(int)Button.State.NUM_STATES]; 
		private Animation[] animations =new Animation[(int)Button.State.NUM_STATES];
	}
}
