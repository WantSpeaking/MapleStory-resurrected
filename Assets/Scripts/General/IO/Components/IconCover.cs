


namespace ms
{
	// A transparent rectangle with icon size (30x30)
	public class IconCover
	{
		public enum Type
		{
			BUFF,
			COOLDOWN
		}

		public IconCover(Type t, int duration)
		{
			cover = new ColorBox(30, 30, Color.Name.BLACK, 0.6f);

			if (duration <= Constants.TIMESTEP)
			{
				scalestep = 1.0f;
			}
			else
			{
				scalestep = Constants.TIMESTEP * 1.0f / duration;
			}

			type = t;

			switch (type)
			{
			case Type.BUFF:
				yscale.set(0.0f);
				break;
			case Type.COOLDOWN:
				yscale.set(1.0f);
				break;
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point_short position, float alpha) const
		public void draw(Point_short position, float alpha)
		{
			float interyscale = yscale.get(alpha);
			var interheight = (short)(30 * interyscale);

			if (interheight == 0)
			{
				return;
			}

			cover.draw(new DrawArgument(position + new Point_short(0, (short)(30 - interheight)), new Point_short(30, interheight)));
		}
		public void update()
		{
			switch (type)
			{
			case Type.BUFF:
				yscale += scalestep;

				if (yscale.last() >= 1.0f)
				{
					yscale.set(1.0f);
					scalestep = 0.0f;
				}

				break;
			case Type.COOLDOWN:
				yscale -= scalestep;

				if (yscale.last() <= 0.0f)
				{
					yscale.set(0.0f);
					scalestep = 0.0f;
				}

				break;
			}
		}

		private ColorBox cover = new ColorBox();
		private Linear_float yscale = new Linear_float();
		private float scalestep;
		private Type type;
	}
}
