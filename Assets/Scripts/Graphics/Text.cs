using System.Collections.Generic;

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
	public class Text
	{
		public enum Font
		{
			A11M,
			A11B,
			A12M,
			A12B,
			A13M,
			A13B,
			A14B,
			A15B,
			A18M,
			NUM_FONTS
		}

		public enum Alignment
		{
			LEFT,
			CENTER,
			RIGHT
		}

		public enum Background
		{
			NONE,
			NAMETAG
		}

		public class Layout
		{
			public class Word
			{
				public uint first;
				public uint last;
				public Font font;
				public Color.Name color;
			}

			public class Line
			{
				public List<Word> words = new List<Word>();
				public Point<short> position = new Point<short>();
			}

			public Layout(List<Layout.Line> l, List<short> a, short w, short h, short ex, short ey)
			{
				this.lines = new List<Line>(l);
				this.advances = new List<short>(a);
				this.dimensions = new ms.Point<short>(w, h);
				this.endoffset = new ms.Point<short>(ex, ey);
			}
			public Layout() : this(new List<Layout.Line>(), new List<short>(), 0, 0, 0, 0)
			{
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short width() const
			public short width()
			{
				return dimensions.x();
			}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short height() const
			public short height()
			{
				return dimensions.y();
			}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short advance(uint index) const
			public short advance(uint index)
			{
				return (short)(index < advances.Count ? advances[(int)index] : 0);
			}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> get_dimensions() const
			public Point<short> get_dimensions()
			{
				return dimensions;
			}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> get_endoffset() const
			public Point<short> get_endoffset()
			{
				return endoffset;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Text::Layout::iterator begin() const
			/*public Text.Layout.iterator begin()
			{
				return lines.GetEnumerator();
			}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Text::Layout::iterator end() const
			public Text.Layout.iterator end()
			{
				return lines.end();
			}*/

			private List<Line> lines = new List<Line>();
			private List<short> advances = new List<short>();
			private Point<short> dimensions = new Point<short>();
			private Point<short> endoffset = new Point<short>();
		}

		public Text(Font f, Alignment a, Color.Name c, Background b, string t = "", ushort mw = 0, bool fm = true, short la = 0)
		{
			this.font = f;
			this.alignment = a;
			this.color = c;
			this.background = b;
			this.maxwidth = mw;
			this.formatted = fm;
			this.line_adj = la;
			change_text(t);
		}
		public Text(Font f, Alignment a, Color.Name c, string t = "", ushort mw = 0, bool fm = true, short la = 0) : this(f, a, c, Background.NONE, t, mw, fm, la)
		{
		}
		public Text() : this(Font.A11M, Alignment.LEFT, Color.Name.BLACK)
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(const DrawArgument& args) const
		public void draw(DrawArgument args)
		{
			draw(args, new Range<short>(0, 0));
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(const DrawArgument& args, const Range<short>& vertical) const
		public void draw(DrawArgument args, Range<short> vertical)
		{
			//todo GraphicsGL.get().drawtext(args, vertical, text, layout, font, color, background);
		}

		public void change_text(string t)
		{
			if (text == t)
			{
				return;
			}

			text = t;

			reset_layout();
		}
		public void change_color(Color.Name c)
		{
			if (color == c)
			{
				return;
			}

			color = c;

			reset_layout();
		}
		public void set_background(Background b)
		{
			background = b;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool empty() const
		public bool empty()
		{
			return string.IsNullOrEmpty(text);
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint length() const
		public uint length()
		{
			return (uint)text.Length;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short width() const
		public short width()
		{
			return layout.width();
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short height() const
		public short height()
		{
			return layout.height();
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ushort advance(uint pos) const
		public ushort advance(uint pos)
		{
			return (ushort)layout.advance(pos);
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> dimensions() const
		public Point<short> dimensions()
		{
			return layout.get_dimensions();
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> endoffset() const
		public Point<short> endoffset()
		{
			return layout.get_endoffset();
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const string& get_text() const
		public string get_text()
		{
			return text;
		}

		private void reset_layout()
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: layout = GraphicsGL::get().createlayout(text, font, alignment, maxwidth, formatted, line_adj);
			//todo layout=(GraphicsGL.get().createlayout(text, font, alignment, maxwidth, formatted, line_adj));
		}

		private Font font;
		private Alignment alignment;
		private Color.Name color;
		private Background background;
		private Layout layout = new Layout();
		private ushort maxwidth;
		private bool formatted;
		private string text;
		private short line_adj;
	}
}

