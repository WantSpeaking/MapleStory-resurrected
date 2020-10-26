#define USE_NX

using System;
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

		public MapleComboBox(Type type, List<string> o, ushort default_option, Point<short> ppos, Point<short> pos, long w)
		{
			this.options = new List<string>(o);
			this.selected_index = default_option;
			this.parentpos = ppos;
			this.rwidth = (ushort)w;
			string combobox = "ComboBox";

			if (type != Type.DEFAULT)
			{
				combobox += Convert.ToString(type);
			}

			WzObject src = nl.nx.wzFile_ui["Basic.img"][combobox];

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
				option_text.Add(new Text(Text.Font.A12M, Text.Alignment.LEFT, Color.Name.BLACK, option));
			}

			Text.Font selected_font = Text.Font.A12M;
			Color.Name selected_color = Color.Name.BLACK;
			selected_adj = new Point<short>(2, -3);

			if (type == Type.BLACKL)
			{
				selected_font = Text.Font.A11M;
				selected_color = Color.Name.WHITE;
				selected_adj = new Point<short>(11, 2);
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: selected = Text(selected_font, Text::Alignment::LEFT, selected_color, options[selected_index]);
			selected = new Text(selected_font, Text.Alignment.LEFT, selected_color, (options[selected_index]));

			state = Button.State.NORMAL;
			background = new ColorBox(width(), (short)(options.Count * HEIGHT), Color.Name.DUSTYGRAY, 1.0f);
			rect = new ColorBox((short)(width() - 2), (short)(options.Count * HEIGHT - 2), Color.Name.GALLERY, 1.0f);
			current_rect = new ColorBox((short)(width() - 2), HEIGHT - 2, Color.Name.GRAYOLIVE, 1.0f);

			Point<short> option_pos = new Point<short>(position.x(), (short)(position.y() + textures[(int)state, 0].get_dimensions().y())) + parentpos;

			for (ushort i = 0; i < option_text.Count; i++)
			{
				buttons[i] = new  AreaButton(new Point<short>((short)(option_pos.x() + 1), (short)(option_pos.y() + (i * HEIGHT) + 1)), new Point<short>((short)(width() - 2), HEIGHT - 2));
			}

			current_pos = 0;
			current_shown = false;
			last_shown = 0;

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: position = pos;
			position=(pos);
			active = true;
			pressed = false;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point<short> parentpos) const override
		public override void draw(Point<short> parentpos)
		{
			if (active)
			{
				Point<short> lpos = position + parentpos;

				textures[(int)state, 0].draw(lpos);
				lpos.shift_x(textures[(int)state, 0].width());

				short middle_width = textures[(int)state, 1].width();
				short current_width = middle_width;

				while (current_width < rwidth)
				{
					textures[(int)state, 1].draw(lpos);
					lpos.shift_x(middle_width);
					current_width += middle_width;
				}

				textures[(int)state, 2].draw(lpos);

				selected.draw(position + parentpos + selected_adj);

				if (pressed)
				{
					Point<short> pos = new Point<short>(position.x(), (short)(position.y() + textures[(int)state, 0].get_dimensions().y())) + parentpos;

					background.draw(pos + new Point<short>(0, 2));
					rect.draw(pos + new Point<short>(1, 3));

					if (current_shown)
					{
						current_rect.draw(new DrawArgument(pos.x() + 1, pos.y() + current_pos + 3));
					}

					for (int i = 0; i < option_text.Count; i++)
					{
						option_text[i].draw(new DrawArgument((short)(pos.x () + 6), (short)(pos.y() + (i * HEIGHT) - 4)));
					}
				}
			}
		}
		public override void update()
		{
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Rectangle<short> bounds(Point<short> parentpos) const override
		public override Rectangle<short> bounds(Point<short> parentpos)
		{
			var lt = parentpos + position - origin();
			var rb = lt + textures[(int)state, 0].get_dimensions();

			var end = textures[(int)state, 2].get_dimensions();

			rb = new Point<short>((short)(rb.x() + end.x() + rwidth), rb.y());

			return new Rectangle<short>(lt, rb);
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short width() const override
		public override short width()
		{
			return (short)(textures[(int)state, 0].width() + textures[(int)state, 2].width() + rwidth);
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> origin() const override
		public override Point<short> origin()
		{
			return textures[(int)state, 0].get_origin();
		}
		public override Cursor.State send_cursor(bool clicked, Point<short> cursorpos)
		{
			Cursor.State ret = clicked ? Cursor.State.CLICKING : Cursor.State.IDLE;

			current_shown = false;
			option_text[last_shown].change_color(Color.Name.BLACK);

			foreach (var btit in buttons)
			{
				if (btit.Value.is_active() && btit.Value.bounds(position).contains(cursorpos))
				{
					if (btit.Value.get_state() == Button.State.NORMAL)
					{
						new Sound(Sound.Name.BUTTONOVER).play();

						btit.Value.set_state(Button.State.MOUSEOVER);
						ret = Cursor.State.CANCLICK;
					}
					else if (btit.Value.get_state() == Button.State.MOUSEOVER)
					{
						if (clicked)
						{
							new Sound(Sound.Name.BUTTONCLICK).play();

							btit.Value.set_state(button_pressed(btit.Key));

							ret = Cursor.State.IDLE;
						}
						else
						{
							ret = Cursor.State.CANCLICK;
							current_pos = (ushort)(btit.Key * HEIGHT);
							current_shown = true;
							last_shown = btit.Key;
							option_text[btit.Key].change_color(Color.Name.WHITE);
						}
					}
				}
				else if (btit.Value.get_state() == Button.State.MOUSEOVER)
				{
					btit.Value.set_state(Button.State.NORMAL);
				}
			}

			return ret;
		}
		public override bool in_combobox(Point<short> cursorpos)
		{
			Point<short> lt = new Point<short>((short)(position.x() + 1), (short)(position.y() + textures[(int)state, 0].get_dimensions().y() + 1)) + parentpos;
			Point<short> rb = lt + new Point<short>((short)(width() - 2), (short)(options.Count * HEIGHT - 2));

			return new Rectangle<short>(lt, rb).contains(cursorpos);
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ushort get_selected() const override
		public override ushort get_selected()
		{
			return selected_index;
		}

		protected Button.State button_pressed(ushort buttonid)
		{
			selected_index = buttonid;

			selected.change_text(options[selected_index]);

			toggle_pressed();

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

		private Texture[,] textures =new Texture[(int)Button.State.NUM_STATES, 3];
		private List<string> options = new List<string>();
		private List<Text> option_text = new List<Text>();
		private Text selected = new Text();
		private ColorBox background = new ColorBox();
		private ColorBox rect = new ColorBox();
		private ColorBox current_rect = new ColorBox();
		private ushort rwidth;
		private const ushort HEIGHT = 16;
		private SortedDictionary<ushort, Button> buttons = new SortedDictionary<ushort, Button>();
		private ushort current_pos;
		private bool current_shown;
		private ushort last_shown;
		private ushort selected_index;
		private Point<short> selected_adj = new Point<short>();
		private Point<short> parentpos = new Point<short>();
	}
}



#if USE_NX
#endif
