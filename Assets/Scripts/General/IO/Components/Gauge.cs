


namespace ms
{
	public class Gauge
	{
		public enum Type : byte
		{
			GAME,
			CASHSHOP
		}

		public Gauge (Type type, Texture front, short max, float percent)
		{
			this.type = type;
			this.barfront = new Texture (front);
			this.maximum = max;
			this.percentage = percent;
			target = percentage;
		}

		public Gauge (Type type, Texture front, Texture mid, short max, float percent)
		{
			this.type = type;
			this.barfront = new Texture (front);
			this.barmid = new Texture (mid);
			this.maximum = max;
			this.percentage = percent;
			target = percentage;
		}

		public Gauge (Type type, Texture front, Texture mid, Texture end, short max, float percent)
		{
			this.type = type;
			this.barfront = new Texture (front);
			this.barmid = new Texture (mid);
			this.barend = new Texture (end);
			this.maximum = max;
			this.percentage = percent;
			target = percentage;
		}

		public void Setbarfront_PosOffset (Point_short offset)
		{
			barfront_PosOffset = offset;
		}
		public void Setbarmid_PosOffset (Point_short offset)
		{
			barmid_PosOffset = offset;
		}
		private Point_short barfront_PosOffset = Point_short.zero;
		private Point_short barmid_PosOffset = Point_short.zero;
		public Gauge ()
		{
		}

		public void draw (DrawArgument args)
		{
			short length = (short)(percentage * maximum);

			if (length > 0)
			{
				if (type == Type.GAME)
				{
					/*barfront.draw(args + new DrawArgument(new Point_short(0, 0), new Point_short(length, 0)));
					barmid.draw(args);
					barend.draw(args + new Point_short((short)(length + 8), 20));*/

					barend.draw (args);
					barfront.draw (args + new DrawArgument (barfront_PosOffset, new Point_short (length, 0)));
					barmid.draw (args + barmid_PosOffset);
				}
				else if (type == Type.CASHSHOP)
				{
					Point_short pos_adj = new Point_short (45, 1);

					barfront.draw (args - pos_adj);
					barmid.draw (args + new DrawArgument (new Point_short (0, 0), new Point_short (length, 0)));
					barend.draw (args - pos_adj + new Point_short ((short)(length + barfront.width ()), 0));
				}
			}
		}

		public void update (float t)
		{
			/*if (target != t)
			{
				target = t;
				step = (target - percentage) / 24;
			}

			if (percentage != target)
			{
				percentage += step;

				if (step < 0.0f)
				{
					if (target - percentage >= step)
					{
						percentage = target;
					}
				}
				else if (step > 0.0f)
				{
					if (target - percentage <= step)
					{
						percentage = target;
					}
				}
			}*/
			percentage = t;
		}

		public bool active { get; set; }
		private Texture barfront = new Texture ();
		private Texture barmid = new Texture ();
		private Texture barend = new Texture ();
		private short maximum;

		private float percentage;
		private float target;
		private float step;

		private Type type;
	}
}