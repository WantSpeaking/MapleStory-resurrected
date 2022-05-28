using System;




namespace ms
{
	// The scrolling server notice at the top of the screen
	public class ScrollingNotice
	{
		public ScrollingNotice()
		{
			width = 800;
			background = new ColorBox(width, 23, Color.Name.BLACK, 0.535f);
			notice = new Text(Text.Font.A12M, Text.Alignment.LEFT, Color.Name.YELLOW);

			xpos.set(0.0);
			active = false;
		}

		public void setnotice(string n)
		{
			notice.change_text(n);
			xpos.set((double)width);
			active = n.Length > 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float alpha) const
		public void draw(float alpha)
		{
			if (active)
			{
				short interx = (short)Math.Round(xpos.get(alpha));
				var position = new Point_short(interx, -1);

				background.draw(new Point_short(0, 0));
				notice.draw(position);
			}
		}
		public void update()
		{
			if (active)
			{
				short new_width = Constants.get().get_viewwidth();

				if (new_width != width)
				{
					width = new_width;
					background.setwidth(width);
					xpos.set((double)width);
				}

				xpos -= 0.5;

				var xmin = (double)(-notice.width());

				if (xpos.last() < xmin)
				{
					xpos.set((double)width);
				}
			}
		}

		private ColorBox background = new ColorBox();
		private Text notice = new Text();
		private Linear_double xpos = new Linear_double();
		private bool active;
		private short width;
	}
}
