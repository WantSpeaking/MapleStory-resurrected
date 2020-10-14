using System.Drawing;
using UnityEngine;

namespace ms
{
	public class DrawArgument
	{
		/*public DrawArgument () : this (Point<short>.zero)
		{
		}

		public DrawArgument (short x, short y) : this (new Point<short> (x, y))
		{
		}

		public DrawArgument (Point<short> position)
		{
			pos = position;
			center = position;
			xscale = 1;
			yscale = 1;
		}

		public DrawArgument (bool flip)
		{
			this.flip = flip;
		}

		public DrawArgument (Point<short> position, Point<short> center, Point<short> stretch, float xscale, float yscale, float opacity)
		{
			pos = position;
			this.center = center;
			this.stretch = stretch;
			this.xscale = xscale;
			this.yscale = yscale;
			this.opacity = opacity;
		}

		public DrawArgument (Point<short> position, bool flip, Point<short> center)
		{
			pos = position;
			this.center = center;
			xscale = 1;
			yscale = 1;
			this.sortingLayer = sortingLayer;
			this.orderInLayer = orderInLayer;
		}

		public DrawArgument (Point<short> position, int sortingLayer, int orderInLayer)
		{
			pos = position;
			center = position;
			xscale = 1;
			yscale = 1;
			this.sortingLayer = sortingLayer;
			this.orderInLayer = orderInLayer;
		}

		public DrawArgument (Point<short> position, float inter)
		{
			pos = position;
			center = position;
			xscale = 1;
			yscale = 1;
		}

		public DrawArgument (Point<short> position, bool flip)
		{
			pos = position;
			center = position;
			xscale = 1;
			yscale = 1;
		}

		public DrawArgument (Point<short> position, bool flip, float opacity, string fullPath)
		{
			pos = position;
			center = position;
			xscale = 1;
			yscale = 1;
		}

		public DrawArgument (Point<short> position, bool flip, float opacity, short cx, short cy, int sortingLayer, int orderInLayer)
		{
			//Debug.LogFormat("old cx:{0}\t cy:{1}", cx, cy);

			pos = position;
			center = position;
			xscale = 1;
			yscale = 1;
			this.cx = cx;
			this.cy = cy;
			this.sortingLayer = sortingLayer;
			this.orderInLayer = orderInLayer;
			//Debug.LogFormat("new cx:{0}\t cy:{1}", this.cx, this.cy);
		}*/


		public DrawArgument () : this (0, 0)
		{
		}

		public DrawArgument (short x, short y) : this (new Point<short> (x, y))
		{
		}

		public DrawArgument (Point<short> position) : this (position, 1.0f)
		{
		}

		public DrawArgument (Point<short> position, float xscale, float yscale) : this (position, position, xscale, yscale, 1.0f)
		{
		}

		public DrawArgument (Point<short> position, Point<short> stretch) : this (position, position, stretch, 1.0f, 1.0f, 1.0f, 0.0f)
		{
		}

		public DrawArgument (Point<short> position, bool flip) : this (position, flip, 1.0f)
		{
		}

		public DrawArgument (float angle, Point<short> position, float opacity) : this (angle, position, false, opacity)
		{
		}

		public DrawArgument (Point<short> position, float opacity) : this (position, false, opacity)
		{
		}

		public DrawArgument (Point<short> position, Color color) : this (position, position, new Point<short> (0, 0), 1.0f, 1.0f, color, 0.0f)
		{
		}

		public DrawArgument (Point<short> position, bool flip, Point<short> center) : this (position, center, flip ? -1.0f : 1.0f, 1.0f, 1.0f)
		{
		}

		public DrawArgument (Point<short> position, Point<short> center, float xscale, float yscale, float opacity) : this (position, center, new Point<short> (0, 0), xscale, yscale, opacity, 0.0f)
		{
		}

		public DrawArgument (bool flip) : this (flip ? -1.0f : 1.0f, 1.0f, 1.0f)
		{
		}

		public DrawArgument (float xscale, float yscale, float opacity) : this (new Point<short> (0, 0), xscale, yscale, opacity)
		{
		}

		public DrawArgument (Point<short> position, float xscale, float yscale, float opacity) : this (position, position, xscale, yscale, opacity)
		{
		}

		public DrawArgument (Point<short> position, bool flip, float opacity) : this (position, position, flip ? -1.0f : 1.0f, 1.0f, opacity)
		{
		}

		public DrawArgument (float angle, Point<short> position, bool flip, float opacity) : this (position, position, new Point<short> (0, 0), flip ? -1.0f : 1.0f, 1.0f, opacity, angle)
		{
		}

		public DrawArgument (Point<short> position, Point<short> center, Point<short> stretch, float xscale, float yscale, float opacity, float angle)
		{
			pos = position;
			this.center = center;
			this.stretch = stretch;
			this.xscale = xscale;
			this.yscale = yscale;
			this.color = new Color (1.0f, 1.0f, 1.0f, opacity);
			this.angle = angle;
		}

		public DrawArgument (Point<short> position, Point<short> center, Point<short> stretch, float xscale, float yscale, Color color, float angle)
		{
			pos = position;
			this.center = center;
			this.stretch = stretch;
			this.xscale = xscale;
			this.yscale = yscale;
			this.color = color;
			this.angle = angle;
		}

		public DrawArgument (Point<short> position, bool flip, float opacity, int sortingLayer, int orderInLayer) : this (position, position, flip ? -1.0f : 1.0f, 1.0f, opacity, sortingLayer, orderInLayer)
		{
		}

		public DrawArgument (Point<short> position, Point<short> center, float xscale, float yscale, float opacity, int sortingLayer, int orderInLayer) : this (position, center, new Point<short> (0, 0), xscale, yscale, opacity, 0.0f, sortingLayer, orderInLayer)
		{
		}

		public DrawArgument (Point<short> position, Point<short> center, Point<short> stretch, float xscale, float yscale, float opacity, float angle, int sortingLayer, int orderInLayer)
		{
			pos = position;
			this.center = center;
			this.stretch = stretch;
			this.xscale = xscale;
			this.yscale = yscale;
			this.color = new Color (1.0f, 1.0f, 1.0f, opacity);
			this.angle = angle;
			this.sortingLayer = sortingLayer;
			this.orderInLayer = orderInLayer;
		}

		#region operator

		public static DrawArgument operator + (DrawArgument a, DrawArgument b)
		{
			return new DrawArgument ()
			{
				pos = a.pos + b.pos,
				center = a.center + b.center,
				stretch = a.stretch + b.stretch,
				xscale = a.xscale * b.xscale,
				yscale = a.yscale * b.yscale,
				color = a.color * b.color,
				angle = a.angle + b.angle,
				sortingLayer = a.sortingLayer + b.sortingLayer,
				orderInLayer = a.orderInLayer + b.orderInLayer
			};
		}

		public static DrawArgument operator + (DrawArgument a, Point<short> argpos)
		{
			return new DrawArgument (a.pos + argpos, a.center + argpos, a.stretch, a.xscale, a.yscale, a.color, a.angle);
			/*return {
				pos + argpos,
				center + argpos,
				stretch, xscale, yscale, color, angle
			};*/
		}

		#endregion

		public Point<short> get_Pos ()
		{
			return pos;
		}

		public Rectangle get_rectangle (Point<short> origin, Point<short> dimensions)
		{
			short w = stretch.x ();

			if (w == 0)
			{
				w = dimensions.x ();
			}

			short h = stretch.y ();

			if (h == 0)
			{
				h = dimensions.y ();
			}

			Point<short> rlt = new Point<short> ((short)(pos.x () - center.x () - origin.x ()), (short)(pos.y () - center.y () - origin.y ()));
			short rl = rlt.x ();
			short rr = (short)(rlt.x () + w);
			short rt = rlt.y ();
			short rb = (short)(rlt.y () + h);
			short cx = center.x ();
			short cy = center.y ();

			return new Rectangle (cx + (short)(xscale * rl), cx + (short)(xscale * rr), cy + (short)(yscale * rt), cy + (short)(yscale * rb));
		}

		private Point<short> pos = new Point<short> ();
		private Point<short> center = new Point<short> ();
		private Point<short> stretch = new Point<short> ();
		private float xscale;
		private float yscale;
		private float opacity;
		private float angle;
		private Color color = new Color (Color.Code.CWHITE);
		public short cx;
		public short cy;
		public bool isBack;
		public int sortingLayer;
		public int orderInLayer;
		public string fullPath;
		public bool flip;
	}
}