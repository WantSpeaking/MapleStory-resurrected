#define USE_NX

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
	public class TextTooltip : Tooltip
	{
		public TextTooltip()
		{
			/*todo WzObject Frame = nl.nx.wzFile_ui["UIToolTip.img"]["Item"]["Frame2"];

			frame = new MapleFrame (Frame);
			cover = Frame["cover"];

			text = "";*/
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point<short> pos) const override
		public override void draw(Point<short> pos)
		{
			if (text_label.empty())
			{
				return;
			}

			short fillwidth = text_label.width();
			short fillheight = text_label.height();

			if (fillheight < 18)
			{
				fillheight = 18;
			}

			short max_width = Constants.get().get_viewwidth();
			short max_height = Constants.get().get_viewheight();
			short cur_width = (short)(pos.x() + fillwidth + 21);
			int cur_height = pos.y() + fillheight + 40;

			int adj_x = cur_width - max_width;
			int adj_y = cur_height - max_height;

			if (adj_x > 0)
			{
				pos.shift_x((short)(adj_x * -1));
			}

			if (adj_y > 0)
			{
				pos.shift_y((short)(adj_y * -1));
			}

			if (fillheight > 18)
			{
				frame.draw(pos + new Point<short>((short)(fillwidth / 2), (short)(fillheight - 6)), (short)(fillwidth - 19), (short)(fillheight - 17));

				if (fillheight > cover.height())
				{
					cover.draw(pos + new Point<short>(-5, -2));
				}
				else
				{
					cover.draw(pos + new Point<short>(-5, -2), new Range<short>(0, (short)(fillheight / 2 - 14 + 2)));
				}

				text_label.draw(pos + new Point<short>(0, 1));
			}
			else
			{
				frame.draw(pos + new Point<short>((short)(fillwidth / 2), (short)(fillheight - 7)), (short)(fillwidth - 19), (short)(fillheight - 18));
				cover.draw(pos + new Point<short>(-5, -2), new Range<short>(0, (short)(fillheight + 2)));
				text_label.draw(pos + new Point<short>(-1, -2));
			}
		}

		public bool set_text(string t, ushort maxwidth = 340, bool formatted = true, short line_adj = 2)
		{
			if (text == t)
			{
				return false;
			}

			text = t;

			if (string.IsNullOrEmpty(text))
			{
				return false;
			}

			text_label = new Text(Text.Font.A12M, Text.Alignment.LEFT, Color.Name.WHITE, text, maxwidth, formatted, line_adj);

			return true;
		}

		private MapleFrame frame = new MapleFrame();
		private Texture cover = new Texture();
		private string text;
		private Text text_label = new Text();
	}
}


#if USE_NX
#endif
