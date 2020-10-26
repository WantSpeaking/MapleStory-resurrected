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
	public class Gauge
	{
		public enum Type : byte
		{
			GAME,
			CASHSHOP
		}

		public Gauge(Type type, Texture front, short max, float percent)
		{
			this.type = type;
			this.barfront = front;
			this.maximum = max;
			this.percentage = percent;
			target = percentage;
		}
		public Gauge(Type type, Texture front, Texture mid, short max, float percent)
		{
			this.type = type;
			this.barfront = front;
			this.barmid = mid;
			this.maximum = max;
			this.percentage = percent;
			target = percentage;
		}
		public Gauge(Type type, Texture front, Texture mid, Texture end, short max, float percent)
		{
			this.type = type;
			this.barfront = front;
			this.barmid = mid;
			this.barend =end;
			this.maximum = max;
			this.percentage = percent;
			target = percentage;
		}
		public Gauge()
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(const DrawArgument& args) const
		public void draw(DrawArgument args)
		{
			short length = (short)(percentage * maximum);

			if (length > 0)
			{
				if (type == Type.GAME)
				{
					barfront.draw(args + new DrawArgument(new Point<short>(0, 0), new Point<short>(length, 0)));
					barmid.draw(args);
					barend.draw(args + new Point<short>((short)(length + 8), 20));
				}
				else if (type == Type.CASHSHOP)
				{
					Point<short> pos_adj = new Point<short>(45, 1);

					barfront.draw(args - pos_adj);
					barmid.draw(args + new DrawArgument(new Point<short>(0, 0), new Point<short>(length, 0)));
					barend.draw(args - pos_adj + new Point<short>((short)(length + barfront.width()), 0));
				}
			}
		}
		public void update(float t)
		{
			if (target != t)
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
			}
		}

		private Texture barfront = new Texture();
		private Texture barmid = new Texture();
		private Texture barend = new Texture();
		private short maximum;

		private float percentage;
		private float target;
		private float step;

		private Type type;
	}
}
