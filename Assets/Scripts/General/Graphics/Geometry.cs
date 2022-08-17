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
	public class Geometry : System.IDisposable
	{
		public virtual void Dispose()
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(short x, short y, short w, short h, Color::Name cid, float opacity) const
		protected void draw(short x, short y, short w, short h, Color.Name cid, float opacity)
		{
			if (w == 0 || h == 0 || opacity <= 0.0f)
			{
				return;
			}

			float[] color = Color.colors[(int)cid];

			//todo GraphicsGL.get().drawrectangle(x, y, w, h, color[0], color[1], color[2], opacity);
		}
	}

	public class ColorBox : Geometry
	{
		public ColorBox(short w, short h, Color.Name c, float o)
		{
			this.width = w;
			this.height = h;
			this.color = c;
			this.opacity = o;
		}
		public ColorBox() : this(0, 0, Color.Name.BLACK, 0.0f)
		{
		}

		public void setwidth(short w)
		{
			width = w;
		}
		public void setheight(short h)
		{
			height = h;
		}
		public void set_color(Color.Name c)
		{
			color = c;
		}
		public void setopacity(float o)
		{
			opacity = o;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(const DrawArgument& args) const
		public void draw(DrawArgument args)
		{
			Point_short absp = args.getpos();
			short absw = args.getstretch().x();

			if (absw == 0)
			{
				absw = width;
			}

			short absh = args.getstretch().y();

			if (absh == 0)
			{
				absh = height;
			}

			absw = (short)(absw * args.get_xscale());
			absh = (short)(absh * args.get_yscale());

			float absopc = opacity * args.get_color().a();

			base.draw(absp.x(), absp.y(), absw, absh, color, absopc);
		}

		private short width;
		private short height;
		private Color.Name color;
		private float opacity;
	}

	public class ColorLine : Geometry
	{
		public ColorLine(short size, Color.Name color, float opacity, bool vertical)
		{
			this.size = size;
			this.color = color;
			this.opacity = opacity;
			this.vertical = vertical;
		}
		public ColorLine() : this(0, Color.Name.BLACK, 0.0f, false)
		{
		}

		public void setsize(short s)
		{
			size = s;
		}
		public void setcolor(Color.Name c)
		{
			color = c;
		}
		public void setopacity(float o)
		{
			opacity = o;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(const DrawArgument& args) const
		public void draw(DrawArgument args)
		{
			Point_short absp = args.getpos();

			short absw = args.getstretch().x();
			short absh = args.getstretch().y();

			if (absw == 0)
			{
				absw = (short)(vertical ? 1 : size);
			}

			if (absh == 0)
			{
				absh = (short)(vertical ? size : 1);
			}

			absw = (short)(absw * args.get_xscale());
			absh = (short)(absh * args.get_yscale());

			float absopc = opacity * args.get_color().a();

			base.draw(absp.x(), absp.y(), absw, absh, color, absopc);
		}

		private short size;
		private Color.Name color;
		private float opacity;
		private bool vertical;
	}

	public class MobHpBar : Geometry
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point_short position, short hppercent) const
		public void draw(Point_short position, short hppercent)
		{
			short fillw = (short)((WIDTH - 6) * (float)hppercent / 100);
			short x = (short)(position.x() - WIDTH / 2);
			short y = (short)(position.y() - HEIGHT * 3);

			base.draw(x, y, WIDTH, HEIGHT, Color.Name.BLACK, 1.0f);
			base.draw((short)(x + 1), (short)(y + 1), WIDTH - 2, 1, Color.Name.WHITE, 1.0f);
			base.draw((short)(x + 1), (short)(y + HEIGHT - 2), WIDTH - 2, 1, Color.Name.WHITE, 1.0f);
			base.draw((short)(x + 1), (short)(y + 2), 1, HEIGHT - 4, Color.Name.WHITE, 1.0f);
			base.draw((short)(x + WIDTH - 2), (short)(y + 2), 1, HEIGHT - 4, Color.Name.WHITE, 1.0f);
			base.draw((short)(x + 3), (short)(y + 3), fillw, 3, Color.Name.LIGHTGREEN, 1.0f);
			base.draw((short)(x + 3), (short)(y + 6), fillw, 1, Color.Name.JAPANESELAUREL, 1.0f);
		}

		private const short WIDTH = 50;
		private const short HEIGHT = 10;
	}
}
