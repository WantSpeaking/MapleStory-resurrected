using MapleLib.WzLib;
using UnityEngine;

namespace ms
{
	public class DrawArgument
	{
		#region constructor

		public DrawArgument () : this (0, 0)
		{
		}

		public DrawArgument (short x, short y) : this (new Point_short (x, y))
		{
		}

		public DrawArgument (Point_short position) : this (new Point_short (position), 1.0f)
		{
		}

		public DrawArgument (Point_short position, float xscale, float yscale) : this (new Point_short (position), new Point_short (position), xscale, yscale, 1.0f)
		{
		}

		public DrawArgument (Point_short position, Point_short stretch) : this (new Point_short (position), new Point_short (position), new Point_short (stretch), 1.0f, 1.0f, 1.0f, 0.0f)
		{
		}

		public DrawArgument (Point_short position, bool flip) : this (new Point_short (position), flip, 1.0f)
		{
		}

		public DrawArgument (float angle, Point_short position, float opacity) : this (angle, new Point_short (position), false, opacity)
		{
		}

		public DrawArgument (Point_short position, float opacity) : this (new Point_short (position), false, opacity)
		{
		}

		public DrawArgument (Point_short position, Color color) : this (new Point_short (position), new Point_short (position), new Point_short (0, 0), 1.0f, 1.0f, new Color (color), 0.0f)
		{
		}

		public DrawArgument (Point_short position, bool flip, Point_short center) : this (new Point_short (position), new Point_short (center), flip ? -1.0f : 1.0f, 1.0f, 1.0f)
		{
		}

		public DrawArgument (Point_short position, Point_short center, float xscale, float yscale, float opacity) : this (new Point_short (position), new Point_short (center), new Point_short (0, 0), xscale, yscale, opacity, 0.0f)
		{
		}

		public DrawArgument (bool flip) : this (flip ? -1.0f : 1.0f, 1.0f, 1.0f)
		{
		}

		public DrawArgument (float xscale, float yscale, float opacity) : this (new Point_short (0, 0), xscale, yscale, opacity)
		{
		}

		public DrawArgument (Point_short position, float xscale, float yscale, float opacity) : this (new Point_short (position), new Point_short (position), xscale, yscale, opacity)
		{
		}

		public DrawArgument (Point_short position, bool flip, float opacity) : this (new Point_short (position), new Point_short (position), flip ? -1.0f : 1.0f, 1.0f, opacity)
		{
		}

		public DrawArgument (float angle, Point_short position, bool flip, float opacity) : this (new Point_short (position), new Point_short (position), new Point_short (0, 0), flip ? -1.0f : 1.0f, 1.0f, opacity, angle)
		{
		}

		public DrawArgument (Point_short position, Point_short center, Point_short stretch, float xscale, float yscale, float opacity, float angle)
		{
			pos = new Point_short (position);
			this.center = new Point_short (center);
			this.stretch = new Point_short (stretch);
			this.xscale = xscale;
			this.yscale = yscale;
			this.color = new Color (1.0f, 1.0f, 1.0f, opacity);
			this.angle = angle;
		}

		public DrawArgument (Point_short position, Point_short center, Point_short stretch, float xscale, float yscale, Color color, float angle)
		{
			pos = new Point_short (position);
			this.center = new Point_short (center);
			this.stretch = new Point_short (stretch);
			this.xscale = xscale;
			this.yscale = yscale;
			this.color = color;
			this.angle = angle;
		}

		public DrawArgument (short x, short y, float rot, float opacity) : this (new Point_short (x, y))
		{
			this.rotation = rot;
			this.color = new Color (1.0f, 1.0f, 1.0f, opacity);
		}
		/*public DrawArgument (Point_short position, bool flip, float opacity, int sortingLayer, int orderInLayer) : this (position, position, flip ? -1.0f : 1.0f, 1.0f, opacity, sortingLayer, orderInLayer)
		{
		}

		public DrawArgument (Point_short position, Point_short center, float xscale, float yscale, float opacity, int sortingLayer, int orderInLayer) : this (position, center, new Point_short (0, 0), xscale, yscale, opacity, 0.0f, sortingLayer, orderInLayer)
		{
		}

		public DrawArgument (Point_short position, Point_short center, Point_short stretch, float xscale, float yscale, float opacity, float angle, int sortingLayer, int orderInLayer)
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

		public DrawArgument (Point_short position, bool flip, Point_short center, int sortingLayer, int orderInLayer) : this (position, center, flip ? -1.0f : 1.0f, 1.0f, 1.0f, sortingLayer: sortingLayer, orderInLayer: orderInLayer)
		{
		}

		public DrawArgument (int sortingLayer, int orderInLayer)
		{
			this.sortingLayer = sortingLayer;
			this.orderInLayer = orderInLayer;
		}

		public DrawArgument (Point_short position, int sortingLayer, int orderInLayer)
		{
			pos = position;
			this.sortingLayer = sortingLayer;
			this.orderInLayer = orderInLayer;
		}

		public DrawArgument (Point_short position, Color color, int sortingLayer, int orderInLayer)
		{
			pos = position;
			this.color = color;
			this.sortingLayer = sortingLayer;
			this.orderInLayer = orderInLayer;
		}

		public DrawArgument (Point_short position, bool flip, int sortingLayer, int orderInLayer)
		{
			pos = position;
			xscale = flip ? -1.0f : 1.0f;
			this.sortingLayer = sortingLayer;
			this.orderInLayer = orderInLayer;
		}

		public DrawArgument (float angle, Point_short position, float opacity, int sortingLayer, int orderInLayer) : this (angle, position, false, opacity)
		{
			this.sortingLayer = sortingLayer;
			this.orderInLayer = orderInLayer;
		}*/

		public DrawArgument (DrawArgument src)
		{
			this.pos = new Point_short (src.pos);
			this.center = new Point_short (src.center);
			this.stretch = new Point_short (src.stretch);
			this.xscale = src.xscale;
			this.yscale = src.yscale;
			this.color = new Color (src.color);
			this.angle = src.angle;
		}

		#endregion

		#region operator

		public static DrawArgument operator + (DrawArgument a, DrawArgument b)
		{
			var pos1 = a.pos + b.pos;
			var center1 = a.center + b.center;
			var stretch1 = a.stretch + b.stretch;
			//AppDebug.Log ($"a.xscale: {a.xscale}\t b.xscale:{b.xscale}\t *:{a.xscale * b.xscale}");
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
				orderInLayer = a.orderInLayer + b.orderInLayer,
				DrawParent = a.DrawParent != null ? a.DrawParent : b.DrawParent != null ? b.DrawParent : null,
				drawOnce = a.drawOnce || b.drawOnce,
                isDontDestoryOnLoad = a.isDontDestoryOnLoad || b.isDontDestoryOnLoad
            };
		}

		public static DrawArgument operator + (DrawArgument a, Point_short argpos)
		{
			return new DrawArgument (
				a.pos + argpos,
				a.center + argpos,
				a.stretch, a.xscale, a.yscale, a.color, a.angle).SetParent(a.DrawParent).SetDontDestoryOnLoad(a.isDontDestoryOnLoad);
		}

		public static DrawArgument operator - (DrawArgument a, Point_short argpos)
		{
			return new DrawArgument (a.pos - argpos, a.center - argpos, a.stretch, a.xscale, a.yscale, a.color, a.angle).SetParent (a.DrawParent).SetDontDestoryOnLoad(a.isDontDestoryOnLoad);
			/*return {
				pos + argpos,
				center + argpos,
				stretch, xscale, yscale, color, angle
			};*/
		}

		public static implicit operator DrawArgument (ms.Point_short point)
		{
			return new DrawArgument (point);
		}

		#endregion

		public Point_short getpos ()
		{
			return pos;
		}

		public Point_short getstretch ()
		{
			return stretch;
		}

		public float get_xscale ()
		{
			return xscale;
		}

		public float get_yscale ()
		{
			return yscale;
		}
		public DrawArgument set_xscale (float xscale)
		{
			this.xscale = xscale;
			return this;
		}

		public DrawArgument set_yscale (float yscale)
		{
			this.yscale = yscale;
			return this;
		}
		public Color get_color ()
		{
			return color;
		}

		public float get_angle ()
		{
			return angle;
		}

		public float get_rotation ()
		{
			return rotation;
		}
		public Rectangle get_rectangle (Point_short origin, Point_short dimensions)
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

			Point_short rlt = new Point_short ((short)(pos.x () - center.x () - origin.x ()), (short)(pos.y () - center.y () - origin.y ()));
			short rl = rlt.x ();
			short rr = (short)(rlt.x () + w);
			short rt = rlt.y ();
			short rb = (short)(rlt.y () + h);
			short cx = center.x ();
			short cy = center.y ();

			return new Rectangle (cx + (short)(xscale * rl), cx + (short)(xscale * rr), cy + (short)(yscale * rt), cy + (short)(yscale * rb));
		}

		public DrawArgument IncreaseOrderInLayer (int orderInLayer = 0)
		{
			this.orderInLayer += orderInLayer;
			return this;
		}

		public DrawArgument SetOrderInLayer (int orderInLayer = 0)
		{
			this.orderInLayer = orderInLayer;
			return this;
		}
		public DrawArgument SetDrawOnce (bool once = false)
		{
			this.drawOnce = once;
			return this;
		}
		private Point_short pos;
		private Point_short center;
		private Point_short stretch;
		private float xscale = 1;
		private float yscale = 1;
		private float angle;
		private Color color;
		private float rotation;
		public bool drawOnce = false;

		public DrawArgument SetParent (GameObject DrawParent)
		{
			this.DrawParent = DrawParent;
			return this;
		}
		public GameObject DrawParent;
        public DrawArgument SetDontDestoryOnLoad(bool b)
        {
			isDontDestoryOnLoad = b;
            return this;
        }
        public bool isDontDestoryOnLoad = false;
        #region to be removed later

        /*public short cx;
		public short cy;
		public bool isBack;*/
        public int sortingLayer;

		public int orderInLayer;

		//public string fullPath;
		public bool FlipX => xscale < 0;
		//private float opacity;

		#endregion
	}
}