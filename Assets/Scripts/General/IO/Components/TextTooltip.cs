#define USE_NX

using MapleLib.WzLib;





namespace ms
{
	public class TextTooltip : Tooltip
	{
		public TextTooltip()
		{
			/*WzObject Frame = ms.wz.wzProvider_ui["UIToolTip.img"]["Item"]["Frame2"];

			frame = new MapleFrame (Frame);
			cover = Frame["cover"];*/

			text = "";
		}

		public override void draw(Point_short pos)
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
				frame?.draw(pos + new Point_short((short)(fillwidth / 2), (short)(fillheight - 6)), (short)(fillwidth - 19), (short)(fillheight - 17));

				if (fillheight > cover.height())
				{
					cover?.draw(pos + new Point_short(-5, -2));
				}
				else
				{
					cover?.draw(pos + new Point_short(-5, -2), new Range_short(0, (short)(fillheight / 2 - 14 + 2)));
				}

				text_label.draw(pos + new Point_short(0, 1));
			}
			else
			{
				frame?.draw(pos + new Point_short((short)(fillwidth / 2), (short)(fillheight - 7)), (short)(fillwidth - 19), (short)(fillheight - 18));
				cover?.draw(pos + new Point_short(-5, -2), new Range_short(0, (short)(fillheight + 2)));
				text_label.draw(pos + new Point_short(-1, -2));
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

		private MapleFrame frame;
		private Texture cover;
		private string text;
		private Text text_label = new Text();
	}
}