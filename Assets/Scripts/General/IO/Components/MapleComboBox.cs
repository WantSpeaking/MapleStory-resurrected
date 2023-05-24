#define USE_NX

using System;
using System.Collections.Generic;
using MapleLib.WzLib;




namespace ms
{
	// A standard MapleStory combo box with four states and three textures for each state
	public class MapleComboBox : Button
	{
		public enum Type : byte
		{
			DEFAULT = 1,
			BROWN = 3,
			BLUENEG,
			DEFAULT2,
			BLACKM,
			BLACKL,
			BLACKS,
			BROWNNEG,
			BLACKL2,
			GREENNEG
		}

		public MapleComboBox (Type type, List<string> o, ushort default_option, Point_short ppos, Point_short pos, long w)
		{
			this.options = new List<string> (o);
			this.selected_index = default_option;
			this.parentpos = new Point_short (ppos);
			this.rwidth = (ushort)w;
			string combobox = "ComboBox";

			if (type != Type.DEFAULT)
			{
				combobox += Convert.ToString ((byte)type);
			}

			WzObject src = ms.wz.wzFile_ui["Basic.img"][combobox];

			textures[(int)Button.State.PRESSED, 0] = src["pressed"][0.ToString ()];
			textures[(int)Button.State.PRESSED, 1] = src["pressed"][1.ToString ()];
			textures[(int)Button.State.PRESSED, 2] = src["pressed"][2.ToString ()];

			textures[(int)Button.State.MOUSEOVER, 0] = src["mouseOver"][0.ToString ()];
			textures[(int)Button.State.MOUSEOVER, 1] = src["mouseOver"][1.ToString ()];
			textures[(int)Button.State.MOUSEOVER, 2] = src["mouseOver"][2.ToString ()];

			textures[(int)Button.State.NORMAL, 0] = src["normal"][0.ToString ()];
			textures[(int)Button.State.NORMAL, 1] = src["normal"][1.ToString ()];
			textures[(int)Button.State.NORMAL, 2] = src["normal"][2.ToString ()];

			textures[(int)Button.State.DISABLED, 0] = src["disabled"][0.ToString ()];
			textures[(int)Button.State.DISABLED, 1] = src["disabled"][1.ToString ()];
			textures[(int)Button.State.DISABLED, 2] = src["disabled"][2.ToString ()];

			foreach (var option in options)
			{
				option_text.Add (new Text (Text.Font.A12M, Text.Alignment.LEFT, Color.Name.BLACK, option));
			}

			Text.Font selected_font = Text.Font.A12M;
			Color.Name selected_color = Color.Name.BLACK;
			selected_adj = new Point_short (2, -3);

			if (type == Type.BLACKL)
			{
				selected_font = Text.Font.A11M;
				selected_color = Color.Name.WHITE;
				selected_adj = new Point_short (11, 2);
			}

			selected = new Text (selected_font, Text.Alignment.LEFT, selected_color, options[selected_index]);

			state = Button.State.NORMAL;
			background = new ColorBox (width (), (short)(options.Count * HEIGHT), Color.Name.DUSTYGRAY, 1.0f);
			rect = new ColorBox ((short)(width () - 2), (short)(options.Count * HEIGHT - 2), Color.Name.GALLERY, 1.0f);
			current_rect = new ColorBox ((short)(width () - 2), HEIGHT - 2, Color.Name.GRAYOLIVE, 1.0f);

			Point_short option_pos = new Point_short (position.x (), (short)(position.y () + textures[(int)state, 0].get_dimensions ().y ())) + parentpos;

			for (ushort i = 0; i < option_text.Count; i++)
			{
				buttons[i] = new AreaButton (new Point_short ((short)(option_pos.x () + 1), (short)(option_pos.y () + (i * HEIGHT) + 1)), new Point_short ((short)(width () - 2), HEIGHT - 2));
			}

			current_pos = 0;
			current_shown = false;
			last_shown = 0;

			position = new Point_short (pos);
			active = true;
			pressed = false;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point_short parentpos) const override
		public override void draw (Point_short parentpos)
		{
			if (active)
			{
				Point_short lpos = position + parentpos;

				textures[(int)state, 0].draw (lpos);
				lpos.shift_x (textures[(int)state, 0].width ());

				short middle_width = textures[(int)state, 1].width ();
				short current_width = middle_width;

				while (current_width < rwidth)
				{
					textures[(int)state, 1].draw (lpos);
					lpos.shift_x (middle_width);
					current_width += middle_width;
				}

				textures[(int)state, 2].draw (lpos);

				selected.draw (position + parentpos + selected_adj);

				if (pressed)
				{
					Point_short pos = new Point_short (position.x (), (short)(position.y () + textures[(int)state, 0].get_dimensions ().y ())) + parentpos;

					background.draw (pos + new Point_short (0, 2));
					rect.draw (pos + new Point_short (1, 3));

					if (current_shown)
					{
						current_rect.draw (new DrawArgument ((short)(pos.x () + 1), (short)(pos.y () + current_pos + 3)));
					}

					for (int i = 0; i < option_text.Count; i++)
					{
						option_text[i].draw (new DrawArgument ((short)(pos.x () + 6), (short)(pos.y () + (i * HEIGHT) - 4)));
					}
				}
			}
		}

		public override void update ()
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Rectangle_short bounds(Point_short parentpos) const override
		public override Rectangle_short bounds (Point_short parentpos)
		{
			var lt = parentpos + position - origin ();
			var rb = lt + textures[(int)state, 0].get_dimensions ();

			var end = textures[(int)state, 2].get_dimensions ();

			rb = new Point_short ((short)(rb.x () + end.x () + rwidth), rb.y ());

			return new Rectangle_short (lt, rb);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short Width() const override
		public override short width ()
		{
			return (short)(textures[(int)state, 0].width () + textures[(int)state, 2].width () + rwidth);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point_short origin() const override
		public override Point_short origin ()
		{
			return textures[(int)state, 0].get_origin ();
		}

		public override Cursor.State send_cursor (bool clicked, Point_short cursorpos)
		{
			Cursor.State ret = clicked ? Cursor.State.CLICKING : Cursor.State.IDLE;

			current_shown = false;
			option_text[last_shown].change_color (Color.Name.BLACK);

			foreach (var btit in buttons)
			{
				if (btit.Value.is_active () && btit.Value.bounds (position).contains (cursorpos))
				{
					if (btit.Value.get_state () == Button.State.NORMAL)
					{
						new Sound (Sound.Name.BUTTONOVER).play ();

						btit.Value.set_state (Button.State.MOUSEOVER);
						ret = Cursor.State.CANCLICK;
					}
					else if (btit.Value.get_state () == Button.State.MOUSEOVER)
					{
						if (clicked)
						{
							new Sound (Sound.Name.BUTTONCLICK).play ();

							btit.Value.set_state (button_pressed (btit.Key));

							ret = Cursor.State.IDLE;
						}
						else
						{
							ret = Cursor.State.CANCLICK;
							current_pos = (ushort)(btit.Key * HEIGHT);
							current_shown = true;
							last_shown = btit.Key;
							option_text[btit.Key].change_color (Color.Name.WHITE);
						}
					}
				}
				else if (btit.Value.get_state () == Button.State.MOUSEOVER)
				{
					btit.Value.set_state (Button.State.NORMAL);
				}
			}

			return ret;
		}

		public override bool in_combobox (Point_short cursorpos)
		{
			Point_short lt = new Point_short ((short)(position.x () + 1), (short)(position.y () + textures[(int)state, 0].get_dimensions ().y () + 1)) + parentpos;
			Point_short rb = lt + new Point_short ((short)(width () - 2), (short)(options.Count * HEIGHT - 2));

			return new Rectangle_short (lt, rb).contains (cursorpos);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ushort get_selected() const override
		public override ushort get_selected ()
		{
			return selected_index;
		}

		protected Button.State button_pressed (ushort buttonid)
		{
			selected_index = buttonid;

			selected.change_text (options[selected_index]);

			toggle_pressed ();

			return Button.State.NORMAL;
		}

		private enum Buttons : ushort
		{
			OPTION1,
			OPTION2,
			OPTION3,
			OPTION4,
			OPTION5,
			OPTION6,
			OPTION7,
			OPTION8,
			OPTION9,
			OPTION10
		}

		private Texture[,] textures = new Texture[(int)Button.State.NUM_STATES, 3];
		private List<string> options = new List<string> ();
		private List<Text> option_text = new List<Text> ();
		private Text selected = new Text ();
		private ColorBox background = new ColorBox ();
		private ColorBox rect = new ColorBox ();
		private ColorBox current_rect = new ColorBox ();
		private ushort rwidth;
		private const ushort HEIGHT = 16;
		private SortedDictionary<ushort, Button> buttons = new SortedDictionary<ushort, Button> ();
		private ushort current_pos;
		private bool current_shown;
		private ushort last_shown;
		private ushort selected_index;
		private Point_short selected_adj = new Point_short ();
		private Point_short parentpos = new Point_short ();

		public override void Dispose ()
		{
			base.Dispose ();
			foreach (var p in textures)
			{
				p?.Dispose ();
			}
			foreach (var p in buttons)
			{
				p.Value?.Dispose ();
			}
			
		}
	}
}


#if USE_NX
#endif