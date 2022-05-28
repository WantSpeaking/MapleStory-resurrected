#define USE_NX

using System;
using MapleLib.WzLib;

namespace ms
{
	public class Slider
	{
		public Slider (int t, Range_short ver, short xp, short ur, short rm, System.Action<bool> om)
		{
			this.type = (short)t;
			this.vertical = ver;
			this.x = xp;
			this.onmoved = om;
			start = new Point_short (x, vertical.first ());
			end = new Point_short (x, vertical.second ());

			WzObject src;
			string base_str = "base";

			if (type == (int)Type.CHATBAR)
			{
				src = ms.wz.wzFile_ui["StatusBar3.img"]["chat"]["common"]["scroll"];
				base_str += "_c";
			}
			else
			{
				string VScr = "VScr";

				if (type != (int)Type.LINE_CYAN)
				{
					VScr += Convert.ToString (type);
				}

				src = ms.wz.wzFile_ui["Basic.img"][VScr];
			}

			WzObject dsrc = src["disabled"];

			dbase = dsrc[base_str];

			dprev = dsrc["prev"];
			dnext = dsrc["next"];

			WzObject esrc = src["enabled"];

			baseTexture = esrc[base_str];

			prev = new TwoSpriteButton (esrc["prev0"], esrc["prev1"], new Point_short (start));
			next = new TwoSpriteButton (esrc["next0"], esrc["next1"], new Point_short(end));
			thumb = new TwoSpriteButton (esrc["thumb0"], esrc["thumb1"]);

			buttonheight = dnext.get_dimensions ().y ();

			setrows (ur, rm);

			enabled = true;
			scrolling = false;
		}

		public Slider () : this (0,  new Range_short (), 0, 0, 0, null)
		{
		}

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

		public void setvertical (Range_short ver)
		{
			vertical= new Range_short (ver);
			start = new Point_short (x, vertical.first ());
			end = new Point_short (x, vertical.second ());
			prev.set_position (new Point_short (start));
			next.set_position (new Point_short (end));

			if (rowmax > 0)
			{
				rowheight = (short)((vertical.length () - buttonheight * 2) / rowmax);
			}
			else
			{
				rowheight = 0;
			}
		}

		public Range_short getvertical ()
		{
			return vertical;
		}

		public void draw (Point_short position)
		{
			Point_short base_pos = position + start;
			Point_short fill = new Point_short (0, (short)(vertical.length () + buttonheight - 2));
			DrawArgument base_arg = new DrawArgument (new Point_short (base_pos.x (), (short)(base_pos.y () + 1)), new Point_short (fill));

			short height = dbase.height ();
			short maxheight = (short)(vertical.first () + height);

			while (maxheight < vertical.second ())
			{
				dbase.draw (position + new Point_short (start.x (), maxheight));

				maxheight += height;
			}

			if (enabled)
			{
				if (rowheight > 0)
				{
					prev.draw (new Point_short (position));
					next.draw (new Point_short (position));
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

		public Cursor.State send_cursor (Point_short cursor, bool pressed)
		{
			Point_short relative = cursor - start;

			if (scrolling)
			{
				if (pressed)
				{
					short thumby = (short)(row * rowheight + buttonheight * 2);
					short delta = (short)(relative.y () - thumby);

					if (delta > rowheight / 2 && row < rowmax)
					{
						row++;
						onmoved?.Invoke (false);
					}
					else if (delta < -rowheight / 2 && row > 0)
					{
						row--;
						onmoved?.Invoke  (true);
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

			Point_short thumbpos = getthumbpos ();

			if (thumb.bounds (new Point_short (thumbpos)).contains (cursor))
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

			if (prev.bounds (new Point_short ()).contains (cursor))
			{
				if (pressed)
				{
					if (row > 0)
					{
						row--;
						onmoved?.Invoke  (true);
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

			if (next.bounds (new Point_short ()).contains (cursor))
			{
				if (pressed)
				{
					if (row < rowmax)
					{
						row++;
						onmoved?.Invoke  (false);
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
							onmoved?.Invoke  (true);
						}

						if (delta < 0)
						{
							row++;
							delta++;
							onmoved?.Invoke  (false);
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
				onmoved?.Invoke  (false);
			}

			if (yoffset > 0 && row > 0)
			{
				row--;
				onmoved?.Invoke  (true);
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

		private Point_short getthumbpos ()
		{
			short y = (short)(row < rowmax ? vertical.first () + row * rowheight + buttonheight : vertical.second () - buttonheight * 2 - 2);

			return new Point_short (x, y);
		}

		private System.Action<bool> onmoved;

		private Range_short vertical = new Range_short ();
		private Point_short start = new Point_short ();
		private Point_short end = new Point_short ();
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