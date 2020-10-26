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


using MapleLib.WzLib;

namespace ms
{
	public class TwoSpriteButton : Button
	{
		public TwoSpriteButton (WzObject nsrc, WzObject ssrc, Point<short> np, Point<short> sp)
		{
			this.textures = new ms.BoolPair<Texture> (ssrc, nsrc);
			this.npos = np;
			this.spos = sp;
			state = Button.State.NORMAL;
			active = true;
		}

		public TwoSpriteButton (WzObject nsrc, WzObject ssrc, Point<short> pos) : this (nsrc, ssrc, pos, pos)
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this(nsrc, ssrc, Point<short>());
		public TwoSpriteButton (WzObject nsrc, WzObject ssrc) : this (nsrc, ssrc, new Point<short> ())
		{
		}

		public TwoSpriteButton ()
		{
			this.textures = new ms.BoolPair<Texture> (new Texture (), new Texture ());
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point<short> parentpos) const
		public override void draw (Point<short> parentpos)
		{
			if (active)
			{
				bool selected = state == Button.State.MOUSEOVER || state == Button.State.PRESSED;

				if (selected)
				{
					textures[selected].draw (spos + parentpos);
				}
				else
				{
					textures[selected].draw (npos + parentpos);
				}
			}
		}

		public override void update ()
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Rectangle<short> bounds(Point<short> parentpos) const
		public override Rectangle<short> bounds (Point<short> parentpos)
		{
			bool selected = state == Button.State.MOUSEOVER || state == Button.State.PRESSED;
			Point<short> absp = new Point<short> ();
			Point<short> dim = new Point<short> ();

			if (selected)
			{
				absp = parentpos + spos - textures[selected].get_origin ();
				dim = textures[selected].get_dimensions ();
			}
			else
			{
				absp = parentpos + npos - textures[selected].get_origin ();
				dim = textures[selected].get_dimensions ();
			}

			return new Rectangle<short> (absp, absp + dim);
		}

		public override short width ()
		{
			bool selected = state == Button.State.MOUSEOVER || state == Button.State.PRESSED;

			return textures[selected].width ();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> origin() const
		public override Point<short> origin ()
		{
			bool selected = state == Button.State.MOUSEOVER || state == Button.State.PRESSED;

			return textures[selected].get_origin ();
		}

		public override Cursor.State send_cursor (bool UnnamedParameter1, Point<short> UnnamedParameter2)
		{
			return Cursor.State.IDLE;
		}

		private BoolPair<Texture> textures = new BoolPair<Texture> ();
		private Point<short> npos = new Point<short> ();
		private Point<short> spos = new Point<short> ();
	}
}