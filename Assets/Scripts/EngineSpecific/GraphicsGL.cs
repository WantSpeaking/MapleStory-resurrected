using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ms.Util;

namespace ms
{
	public class GraphicsGL : Singleton<GraphicsGL>
	{
		public SpriteBatch Batch { get; set; }
		public SpriteFont Font_Arial { get; set; }
		public GraphicsGL ()
		{
			VWIDTH = Constants.get ().get_viewwidth ();
			VHEIGHT = Constants.get ().get_viewheight ();
			SCREEN = new Rectangle_short (0, VWIDTH, 0, VHEIGHT);
		}

		/*public void drawtext (DrawArgument args, Range_short vertical, string text, Text.Layout layout, Text.Font id, Color.Name colorid, Text.Background background)
		{
			/*if (locked)
				return;
	
			 Color color = args.get_color();
	
			if (text.empty() || color.invisible())
				return;
	
			 Font font = fonts[id];
	
			var x = args.getpos().x();
			var y = args.getpos().y();
			var w = layout.Width();
			var h = layout.height();
			var minheight = vertical.first() > 0 ? vertical.first() : SCREEN.top();
			var maxheight = vertical.second() > 0 ? vertical.second() : SCREEN.bottom();
	
			switch (background)
			{
				case TextBackgroundNAMETAG:
				{
					// If ever changing code in here confirm placements with map 10000
					for ( TextLayoutLine line : layout)
					{
						var left = x + line.position.x() - 1;
						var right = left + w + 3;
						var top = y + line.position.y() - font.linespace() + 6;
						var bottom = top + h - 2;
						Color ntcolor = Color(0.0f, 0.0f, 0.0f, 0.6f);
	
						quads.emplace_back(left, right, top, bottom, nulloffset, ntcolor, 0.0f);
						quads.emplace_back(left - 1, left, top + 1, bottom - 1, nulloffset, ntcolor, 0.0f);
						quads.emplace_back(right, right + 1, top + 1, bottom - 1, nulloffset, ntcolor, 0.0f);
					}
	
					break;
				}
			}
	
			for ( TextLayoutLine line : layout)
			{
				Point_short position = line.position;
	
				for ( TextLayoutWord word : line.words)
				{
					var ax = position.x() + layout.advance(word.first);
					var ay = position.y();
	
					 GLfloat* wordcolor;
	
					if (word.color < ColorNameNUM_COLORS)
						wordcolor = Colorcolors[word.color];
					else
						wordcolor = Colorcolors[colorid];
	
					Color abscolor = color * Color(wordcolor[0], wordcolor[1], wordcolor[2], 1.0f);
	
					for (int pos = word.first; pos < word.last; ++pos)
					{
						 char c = text[pos];
						 FontChar ch = font.chars[c];
	
						var char_x = x + ax + ch.bl;
						var char_y = y + ay - ch.bt;
						var char_width = ch.bw;
						var char_height = ch.bh;
						var char_bottom = char_y + char_height;
	
						Offset offset = ch.offset;
	
						if (char_bottom > maxheight)
						{
							var bottom_adjust = char_bottom - maxheight;
	
							if (bottom_adjust < 10)
							{
								offset.bottom -= bottom_adjust;
								char_bottom -= bottom_adjust;
							}
							else
							{
								continue;
							}
						}
	
						if (char_y < minheight)
							continue;
	
						if (ax == 0  c == ' ')
							continue;
	
						ax += ch.ax;
	
						if (char_width <= 0 || char_height <= 0)
							continue;
	
						quads.emplace_back(char_x, char_x + char_width, char_y, char_bottom, offset, abscolor, 0.0f);
					}
				}
			}
		}#1#
			Color color = args.get_color ();

			if (string.IsNullOrEmpty (text) || color.invisible ())
				return;

			Font font = fonts[(int)id];

			var x = args.getpos ().x ();
			var y = args.getpos ().y ();
			var w = layout.width ();
			var h = layout.height ();
			var minheight = vertical.first () > 0 ? vertical.first () : SCREEN.top ();
			var maxheight = vertical.second () > 0 ? vertical.second () : SCREEN.bottom ();

			foreach (var line in layout)
			{
				Point_short position = line.position;

				foreach (var word in line.words)
				{
					var ax = position.x () + layout.advance (word.first);
					var ay = position.y ();

					float[] wordcolor;

					if (word.color < Color.Name.NUM_COLORS)
						wordcolor = Color.colors[(int)word.color];
					else
						wordcolor = Color.colors[(int)colorid];

					Color abscolor = color * new Color (wordcolor[0], wordcolor[1], wordcolor[2], 1.0f);

					for (var pos = word.first; pos < word.last; ++pos)
					{
						char c = text[(int)pos];
						Font.Char ch = font.chars[c];

						var char_x = x + ax + ch.bl;
						var char_y = y + ay - ch.bt;
						var char_width = ch.bw;
						var char_height = ch.bh;
						var char_bottom = char_y + char_height;

						Offset offset = ch.offset;

						if (char_bottom > maxheight)
						{
							var bottom_adjust = (short)(char_bottom - maxheight);

							if (bottom_adjust < 10)
							{
								offset.bottom -= bottom_adjust;
								char_bottom -= bottom_adjust;
							}
							else
							{
								continue;
							}
						}

						if (char_y < minheight)
							continue;

						if (ax == 0 && c == ' ')
							continue;

						ax += ch.ax;

						if (char_width <= 0 || char_height <= 0)
							continue;
						var monoColor = new Microsoft.Xna.Framework.Color (abscolor.r () * abscolor.a (), abscolor.g () * abscolor.a (), abscolor.b () * abscolor.a (), abscolor.a ());
						//GraphicsGL.get ().Batch.DrawString (GraphicsGL.get().FontSprite, c.ToString (), new Vector2 (char_x, char_y), monoColor);

						//quads.emplace_back (char_x, char_x + char_width, char_y, char_bottom, offset, abscolor, 0.0f);
					}
				}
			}
		}

		public void drawrectangle (short x, short y, short width, short height, float red, float green, float blue, float alpha)
		{
			Batch.Draw (Batch.BlankTexture (), new Rectangle (x, y, width, height), new Microsoft.Xna.Framework.Color (red, green, blue, alpha));
		}

		public void drawrectangle (int x, int y, int width, int height, float red, float green, float blue, float alpha)
		{
			Batch.Draw (Batch.BlankTexture (), new Rectangle (x, y, width, height), new Microsoft.Xna.Framework.Color (red, green, blue, alpha));
		}
		public void drawrectangle (int x, int y, int width, int height, Microsoft.Xna.Framework.Color color)
		{
			Batch.Draw (Batch.BlankTexture (), new Rectangle (x, y, width, height), color);
		}
		public void drawrectangle (Rectangle rectangle, Microsoft.Xna.Framework.Color color)
		{
			Batch.Draw (Batch.BlankTexture (), rectangle, color);
		}*/

		/*		public void DrawWireRectangle(int x, int y, int Width, int height, Microsoft.Xna.Framework.Color color, int lineWidth = 4)
				{
					DrawWireRectangle(x, y, Width, height, color, lineWidth);
				}*/
		public void DrawWireRectangle (int x, int y, int width, int height, Microsoft.Xna.Framework.Color color, int lineWidth = 4, SpriteBatch spriteBatch = null)
		{
			var rectTop = new Rectangle (x, y, width, lineWidth);
			var rectBot = new Rectangle (x, y + height - lineWidth, width, lineWidth);
			var rectLeft = new Rectangle (x, y, lineWidth, height);
			var rectRight = new Rectangle (x + width - lineWidth, y, lineWidth, height);


			var drawBatch = spriteBatch ?? Batch;
			/*drawBatch.Draw (Batch.BlankTexture (), rectTop, color);
			drawBatch.Draw (Batch.BlankTexture (), rectBot, color);
			drawBatch.Draw (Batch.BlankTexture (), rectLeft, color);
			drawBatch.Draw (Batch.BlankTexture (), rectRight, color);*/
		}
		/*		public void DrawWireRectangle(FairyGUI.Drawing.RectangleF rect, Microsoft.Xna.Framework.Color color, int lineWidth = 4)
				{
					DrawWireRectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height, color, lineWidth);
				}*/
		/*public void DrawWireRectangle (FairyGUI.Drawing.RectangleF rect, Microsoft.Xna.Framework.Color color, int lineWidth = 4, SpriteBatch spriteBatch = null)
		{
			DrawWireRectangle ((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height, color, lineWidth, spriteBatch);
		}*/
		public void DrawWireRectangle (Microsoft.Xna.Framework.Rectangle rect, Microsoft.Xna.Framework.Color color, int lineWidth = 4, SpriteBatch spriteBatch = null)
		{
			DrawWireRectangle ((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height, color, lineWidth, spriteBatch);
		}

		public void clear ()
		{

		}
		public void unlock ()
		{

		}
		public void enlock ()
		{

		}
		struct Offset
		{
			public short left;
			public short right;
			public short top;
			public short bottom;

			public Offset (short x, short y, short width, short height)
			{
				left = x;
				right = (short)(x + width);
				top = y;
				bottom = (short)(y + height);
			}

			/*Offset()
			{
				left = 0;
				right = 0;
				top = 0;
				bottom = 0;
			}*/
		};

		struct Font
		{
			public struct Char
			{
				public short ax;
				public short ay;
				public short bw;
				public short bh;
				public short bl;
				public short bt;
				public Offset offset;
			};

			public short width;
			public short height;
			public Char[] chars;

			public Font (short w, short h)
			{
				width = w;
				height = h;
				chars = new Char[128];
			}

			/*Font()
			{
				Width = 0;
				height = 0;
			}*/

			public short linespace ()
			{
				return (short)(height * 1.35 + 1);
			}
		};

		class LayoutBuilder
		{
			public LayoutBuilder (Font font, Text.Alignment alignment, short maxwidth, bool formatted, short line_adj)
			{
				fontid = Text.Font.NUM_FONTS;
				color = Color.Name.NUM_COLORS;
				ax = 0;
				ay = font.linespace ();
				width = 0;
				endy = 0;

				if (maxwidth == 0)
					maxwidth = 800;
			}

			/*public int add (string text, int prev, int first, int last)
			{
				if (first == last)
					return prev;

				Text.Font last_font = fontid;
				Color.Name last_color = color;
				int skip = 0;
				bool linebreak = false;

				if (formatted)
				{
					switch (text[first])
					{
						case '\\':
							{
								if (first + 1 < last)
								{
									switch (text[first + 1])
									{
										case 'n':
											linebreak = true;
											break;
										case 'r':
											linebreak = ax > 0;
											break;
									}

									skip++;
								}

								skip++;
								break;
							}
						case '#':
							{
								if (first + 1 < last)
								{
									switch (text[first + 1])
									{
										case 'k':
											color = Color.Name.DARKGREY;
											break;
										case 'b':
											color = Color.Name.BLUE;
											break;
										case 'r':
											color = Color.Name.RED;
											break;
										case 'c':
											color = Color.Name.ORANGE;
											break;
									}

									skip++;
								}

								skip++;
								break;
							}
					}
				}

				short wordwidth = 0;

				if (!linebreak)
				{
					for (int i = first; i < last; i++)
					{
						char c = text[i];
						wordwidth += font.chars[c].ax;

						if (wordwidth > maxwidth)
						{
							if (last - first == 1)
							{
								return last;
							}
							else
							{
								prev = add (text, prev, first, i);
								return add (text, prev, i, last);
							}
						}
					}
				}

				bool newword = skip > 0;
				bool newline = linebreak || ax + wordwidth > maxwidth;

				if (newword || newline)
					add_word (prev, first, last_font, last_color);

				if (newline)
				{
					add_line ();

					endy = ay;
					ax = 0;
					ay += font.linespace ();

					if (lines.Count > 0)
						ay -= line_adj;
				}

				for (int pos = first; pos < last; pos++)
				{
					char c = text[pos];
					Font.Char ch = font.chars[c];

					advances.Add (ax);

					if (pos < first + skip || newline && c == ' ')
						continue;

					ax += ch.ax;

					if (width < ax)
						width = ax;
				}

				if (newword || newline)
					return first + skip;
				else
					return prev;
			}*/

			public Text.Layout finish (int first, int last)
			{
				add_word (first, last, fontid, color);
				add_line ();

				advances.Add (ax);

				return new Text.Layout (lines, advances, width, ay, ax, endy);
			}

			public void add_word (int word_first, int word_last, Text.Font word_font, Color.Name word_color)
			{
				//words.Add (new Text.Layout.Word ((uint)word_first, (uint)word_last, word_font, word_color));
			}

			public void add_line ()
			{
				short line_x = 0;
				short line_y = ay;

				switch (alignment)
				{
					case Text.Alignment.CENTER:
						line_x -= (short)(ax / 2);
						break;
					case Text.Alignment.RIGHT:
						line_x -= ax;
						break;
				}

				//lines.Add (new Text.Layout.Line (words, new Point_short (line_x, line_y)));
				words.Clear ();
			}

			Font font;

			Text.Alignment alignment;
			Text.Font fontid;
			Color.Name color;
			short maxwidth;
			bool formatted;

			short ax;
			short ay;

			List<Text.Layout.Line> lines;
			List<Text.Layout.Word> words;
			List<short> advances;
			short width;
			short endy;
			short line_adj;
		};

		short VWIDTH;
		short VHEIGHT;
		Rectangle_short SCREEN;

		short ATLASW = 8192;
		short ATLASH = 8192;
		short MINLOSIZE = 32;

		bool locked;

		//List<Quad> quads;
		uint VBO;
		uint atlas;

		int shaderProgram;
		int attribute_coord;
		int attribute_color;
		int uniform_texture;
		int uniform_atlassize;
		int uniform_screensize;
		int uniform_yoffset;
		int uniform_fontregion;

		Dictionary<int, Offset> offsets;
		Offset nulloffset;

		//QuadTree<int, Leftover> leftovers;
		int rlid;
		int wasted;
		Point_short border;
		Range_short yrange;

		//FT_Library ftlibrary;
		Font[] fonts;
		Point_short fontborder;
		short fontymax;
	}
}