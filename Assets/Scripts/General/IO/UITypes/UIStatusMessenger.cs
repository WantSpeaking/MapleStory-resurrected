using System.Collections.Generic;
using Beebyte.Obfuscator;

namespace ms
{
	public class StatusInfo
	{
		public StatusInfo(string str, Color.Name color)
		{
			text = new Text(Text.Font.A12M, Text.Alignment.RIGHT, color, str);
			shadow = new Text(Text.Font.A12M, Text.Alignment.RIGHT, Color.Name.BLACK, str);

			opacity.set(1.0f);
		}

		public void draw(Point_short position, float alpha)
		{
			float interopc = opacity.get(alpha);

			shadow.draw(new DrawArgument(position + new Point_short(1, 1), interopc));
			text.draw(new DrawArgument(new Point_short (position), interopc));
		}
		public bool update()
		{
			const float FADE_STEP = Constants.TIMESTEP * 1.0f / FADE_DURATION;
			opacity -= FADE_STEP;

			return opacity.last() < FADE_STEP;
		}

		private Text text = new Text();
		private Text shadow = new Text();
		private Linear_float opacity = new Linear_float();

		// 8 seconds
		private const long FADE_DURATION = 8_000;
	}

	[Skip]
	public class UIStatusMessenger : UIElement
	{
		public const Type TYPE = UIElement.Type.STATUSMESSENGER;
		public const bool FOCUSED = false;
		public const bool TOGGLED = false;

		public UIStatusMessenger()
		{
			short height = Constants.get().get_viewheight();
			short width = Constants.get().get_viewwidth();

			update_screen(width, height);
		}

		public override void draw(float inter)
		{
			Point_short infopos = new Point_short(position.x(), position.y());

			foreach (StatusInfo info in statusinfos)
			{
				info.draw(new Point_short (infopos), inter);
				infopos.shift_y(-14);
			}
		}
		public override void update()
		{
			foreach (StatusInfo info in statusinfos)
			{
				info.update();
			}
		}
		public override void update_screen(short new_width, short new_height)
		{
			short y_adj = (short)((new_width > 800) ? 37 : 0);

			position = new Point_short((short)(new_width - 400), (short)(new_height - 145 + y_adj));
		}

		public override UIElement.Type get_type()
		{
			return TYPE;
		}

		public void show_status(Color.Name color, string message)
		{
			statusinfos.AddFirst(new StatusInfo(message, color));

			if (statusinfos.Count > MAX_MESSAGES)
			{
				statusinfos.RemoveLast();
			}
		}

		private const uint MAX_MESSAGES = 6;

		private LinkedList<StatusInfo> statusinfos = new LinkedList<StatusInfo>();
	}
}

