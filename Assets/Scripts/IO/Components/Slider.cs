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
	public class Slider
	{
		public Slider (int t, Range<short> ver, short xp, short ur, short rm, System.Action<bool> om)
		{
			this.type = (short)t;
			this.vertical = ver;
			this.x = xp;
			this.onmoved = om;
			start = new Point<short> (x, vertical.first ());
			end = new Point<short> (x, vertical.second ());

			WzObject src;
			string base_str = "base";

			if (type == (int)Type.CHATBAR)
			{
				src = nl.nx.wzFile_ui["StatusBar3.img"]["chat"]["common"]["scroll"];
				base_str += "_c";
			}
			else
			{
				string VScr = "VScr";

				if (type != (int)Type.LINE_CYAN)
				{
					VScr += Convert.ToString (type);
				}

				src = nl.nx.wzFile_ui["Basic.img"][VScr];
			}

			WzObject dsrc = src["disabled"];

			dbase = dsrc[base_str];

			dprev = dsrc["prev"];
			dnext = dsrc["next"];

			WzObject esrc = src["enabled"];

			baseTexture = esrc[base_str];

			prev = new TwoSpriteButton (esrc["prev0"], esrc["prev1"], start);
			next = new TwoSpriteButton (esrc["next0"], esrc["next1"], end);
			thumb = new TwoSpriteButton (esrc["thumb0"], esrc["thumb1"]);

			buttonheight = dnext.get_dimensions ().y ();

			setrows (ur, rm);

			enabled = true;
			scrolling = false;
		}

		public Slider () : this (0,  new Range<short> (), 0, 0, 0, null)
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool isenabled() const
		public bool isenabled ()
		{
			return enabled;
		}

		public void setenabled (bool en)
		{
			enabled = en;
		}

		public void setrows (short nr, short ur, short rm)
		{
			rowmax = (short)(rm - ur);

			if (rowmax > 0)
			{
				rowheight = (short)((vertical.length () - buttonheight * 2) / rowmax);
			}
			else
			{
				rowheight = 0;
			}

			row = nr;
		}

		public void setrows (short ur, short rm)
		{
			setrows (0, ur, rm);
		}

		public void setvertical (Range<short> ver)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: vertical = ver;
			vertical= (ver);
			start = new Point<short> (x, vertical.first ());
			end = new Point<short> (x, vertical.second ());
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: prev.set_position(start);
			prev.set_position (start);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: next.set_position(end);
			next.set_position (end);

			if (rowmax > 0)
			{
				rowheight = (short)((vertical.length () - buttonheight * 2) / rowmax);
			}
			else
			{
				rowheight = 0;
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Range<short> getvertical() const
		public Range<short> getvertical ()
		{
			return vertical;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point<short> position) const
		public void draw (Point<short> position)
		{
			Point<short> base_pos = position + start;
			Point<short> fill = new Point<short> (0, (short)(vertical.length () + buttonheight - 2));
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: DrawArgument base_arg = DrawArgument(Point<short>(base_pos.x(), base_pos.y() + 1), fill);
			DrawArgument base_arg = new DrawArgument (new Point<short> (base_pos.x (), (short)(base_pos.y () + 1)), fill);

			short height = dbase.height ();
			short maxheight = (short)(vertical.first () + height);

			while (maxheight < vertical.second ())
			{
				dbase.draw (position + new Point<short> (start.x (), maxheight));

				maxheight += height;
			}

			if (enabled)
			{
				if (rowheight > 0)
				{
					prev.draw (position);
					next.draw (position);
					thumb.draw (position + getthumbpos ());
				}
				else
				{
					dprev.draw (position + start);
					dnext.draw (position + end);
				}
			}
			else
			{
				dprev.draw (position + start);
				dnext.draw (position + end);
			}
		}

		public void remove_cursor ()
		{
			scrolling = false;

			thumb.set_state (Button.State.NORMAL);
			next.set_state (Button.State.NORMAL);
			prev.set_state (Button.State.NORMAL);
		}

		public Cursor.State send_cursor (Point<short> cursor, bool pressed)
		{
			Point<short> relative = cursor - start;

			if (scrolling)
			{
				if (pressed)
				{
					short thumby = (short)(row * rowheight + buttonheight * 2);
					short delta = (short)(relative.y () - thumby);

					if (delta > rowheight / 2 && row < rowmax)
					{
						row++;
						onmoved (false);
					}
					else if (delta < -rowheight / 2 && row > 0)
					{
						row--;
						onmoved (true);
					}

					return Cursor.State.VSCROLLIDLE;
				}
				else
				{
					scrolling = false;
				}
			}
			else if (relative.x () < 0 || relative.y () < 0 || relative.x () > 8 || relative.y () > vertical.second ())
			{
				thumb.set_state (Button.State.NORMAL);
				next.set_state (Button.State.NORMAL);
				prev.set_state (Button.State.NORMAL);

				return Cursor.State.IDLE;
			}

			Point<short> thumbpos = getthumbpos ();

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: if (thumb.bounds(thumbpos).contains(cursor))
			if (thumb.bounds (thumbpos).contains (cursor))
			{
				if (pressed)
				{
					scrolling = true;
					thumb.set_state (Button.State.PRESSED);

					return Cursor.State.VSCROLLIDLE;
				}
				else
				{
					thumb.set_state (Button.State.NORMAL);

					return Cursor.State.VSCROLL;
				}
			}
			else
			{
				thumb.set_state (Button.State.NORMAL);
			}

			if (prev.bounds (new Point<short> ()).contains (cursor))
			{
				if (pressed)
				{
					if (row > 0)
					{
						row--;
						onmoved (true);
					}

					prev.set_state (Button.State.PRESSED);

					return Cursor.State.VSCROLLIDLE;
				}
				else
				{
					prev.set_state (Button.State.MOUSEOVER);

					return Cursor.State.VSCROLL;
				}
			}
			else
			{
				prev.set_state (Button.State.NORMAL);
			}

			if (next.bounds (new Point<short> ()).contains (cursor))
			{
				if (pressed)
				{
					if (row < rowmax)
					{
						row++;
						onmoved (false);
					}

					next.set_state (Button.State.PRESSED);

					return Cursor.State.VSCROLLIDLE;
				}
				else
				{
					next.set_state (Button.State.MOUSEOVER);

					return Cursor.State.VSCROLL;
				}
			}
			else
			{
				next.set_state (Button.State.NORMAL);
			}

			if (cursor.y () < vertical.second ())
			{
				if (pressed)
				{
					var yoffset = (double)(relative.y () - buttonheight * 2);
					var cursorrow = (short)Math.Round (yoffset / rowheight);

					if (cursorrow < 0)
					{
						cursorrow = 0;
					}
					else if (cursorrow > rowmax)
					{
						cursorrow = rowmax;
					}

					short delta = (short)(row - cursorrow);

					for (uint i = 0; i < 2; i++)
					{
						if (delta > 0)
						{
							row--;
							delta--;
							onmoved (true);
						}

						if (delta < 0)
						{
							row++;
							delta++;
							onmoved (false);
						}
					}

					return Cursor.State.VSCROLLIDLE;
				}
			}

			return Cursor.State.VSCROLL;
		}

		public void send_scroll (double yoffset)
		{
			if (yoffset < 0 && row < rowmax)
			{
				row++;
				onmoved (false);
			}

			if (yoffset > 0 && row > 0)
			{
				row--;
				onmoved (true);
			}
		}

		public enum Type
		{
			/// Default
			LINE_CYAN,
			LINE_CONTESSA = 2,
			SMALL_HAVELOCKBLUE,
			NORMAL_CALYPSO,
			NORMAL_ROCKBLUE,
			LINE_PUNGA,
			LINE_YELLOWMETAL,
			NORMAL_JUDGEGRAY,
			DEFAULT_SILVER,
			LINE_MINESHAFT,
			DEFAULT_ALTO,
			DEFAULT_SANDAL,
			DEFAULT_QUICKSAND,
			LINE_HOTCINNAMON,
			THIN_DUSTYGRAY_LIGHT,
			THIN_MINESHAFT,
			THIN_DUSTYGRAY,
			THIN_MINESHAFT_LIGHT,
			THIN_WOODYBROWN,
			BLIZZARDBLUE,
			DEFAULT_ARROWTOWN = 100,
			THIN_ZORBA,
			ARROWS_IRISHCOFFEE,
			THIN_MIKADO,
			ARROWS_TORYBLUE,
			THIN_SLATEGRAY,

			/// Custom
			CHATBAR
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> getthumbpos() const
		private Point<short> getthumbpos ()
		{
			short y = (short)(row < rowmax ? vertical.first () + row * rowheight + buttonheight : vertical.second () - buttonheight * 2 - 2);

			return new Point<short> (x, y);
		}

		private System.Action<bool> onmoved;

		private Range<short> vertical = new Range<short> ();
		private Point<short> start = new Point<short> ();
		private Point<short> end = new Point<short> ();
		private short type;
		private short buttonheight;
		private short rowheight;
		private short x;
		private short row;
		private short rowmax;
		private bool scrolling;
		private bool enabled;

		private Texture dbase = new Texture ();
		private Texture dnext = new Texture ();
		private Texture dprev = new Texture ();
		private Texture baseTexture = new Texture ();
		private TwoSpriteButton next = new TwoSpriteButton ();
		private TwoSpriteButton prev = new TwoSpriteButton ();
		private TwoSpriteButton thumb = new TwoSpriteButton ();
	}
}

#if USE_NX
#endif