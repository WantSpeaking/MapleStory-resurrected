#define USE_NX

using System;
using MapleLib.WzLib;




namespace ms
{
	public class MapleFrame
	{
		public MapleFrame()
		{
		}
		public MapleFrame(WzObject src)
		{
			center = src["c"];
			east = src["e"];
			northeast = src["ne"];
			north = src["n"];
			northwest = src["nw"];
			west = src["w"];
			southwest = src["sw"];
			south = src["s"];
			southeast = src["se"];

			xtile = Math.Max(north.width(), (short)1);
			ytile = Math.Max(west.height(), (short)1);
		}

		public void draw(Point_short position, short rwidth, short rheight)
		{
			short numhor = (short)(rwidth / xtile + 2);
			short numver = (short)(rheight / ytile);
			short width = (short)(numhor * xtile);
			short height = (short)(numver * ytile);
			short left = (short)(position.x() - width / 2);
			short top = (short)(position.y() - height);
			short right = (short)(left + width);
			short bottom = (short)(top + height);

			northwest.draw(new DrawArgument(left, top));
			southwest.draw(new DrawArgument(left, bottom));

			for (short y = top; y < bottom; y += ytile)
			{
				west.draw(new DrawArgument(left, y));
				east.draw(new DrawArgument(right, y));
			}

			center.draw(new DrawArgument(new Point_short(left, top), new Point_short((short)width, (short)height)));

			for (short x = left; x < right; x += xtile)
			{
				north.draw(new DrawArgument(x, top));
				south.draw(new DrawArgument(x, bottom));
			}

			northeast.draw(new DrawArgument(right, top));
			southeast.draw(new DrawArgument(right, bottom));
		}

		private Texture center = new Texture();
		private Texture east = new Texture();
		private Texture northeast = new Texture();
		private Texture north = new Texture();
		private Texture northwest = new Texture();
		private Texture west = new Texture();
		private Texture southwest = new Texture();
		private Texture south = new Texture();
		private Texture southeast = new Texture();
		private short xtile;
		private short ytile;
	}
}

#if USE_NX
#endif
