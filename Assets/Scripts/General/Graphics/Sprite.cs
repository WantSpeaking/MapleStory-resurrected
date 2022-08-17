


using System;
using MapleLib.WzLib;

namespace ms
{
	// Combines an Animation with additional state
	public class Sprite : IDisposable
	{
		public Sprite (Animation a, DrawArgument args)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this.animation = new ms.Animation(a);
			this.animation = new ms.Animation(a);
			this.stateargs = new DrawArgument(args);
		}

		public Sprite (WzObject src, DrawArgument args)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this.animation = new ms.Animation(src);
			this.animation = new ms.Animation (src);
			this.stateargs = new DrawArgument(args);
		}

		public Sprite (WzObject src) : this (src, new DrawArgument ())
		{
		}

		public Sprite ()
		{
		}

		public void Dispose ()
		{
			animation.Dispose ();
		}

		//private static DrawArgument uiRenderOrderArgs = new DrawArgument(Constants.get ().sortingLayer_UI,0);

		public void draw (Point_short parentpos, float alpha)
		{
			//var absargs = stateargs + parentpos + uiRenderOrderArgs;//todo 2 uiRenderOrderArgs
			var absargs = stateargs + parentpos;
			animation.draw (absargs, alpha);
		}
		
		public void draw (Point_short parentpos, float alpha, DrawArgument renderOrderArgs)
		{
			var absargs = renderOrderArgs != null ? stateargs + parentpos + renderOrderArgs : stateargs + parentpos;
			animation.draw (absargs, alpha);
		}

		public bool update (ushort timestep)
		{
			return animation.update (timestep);
		}

		public bool update ()
		{
			return animation.update ();
		}

		public short width ()
		{
			return get_dimensions ().x ();
		}

		public short height ()
		{
			return get_dimensions ().y ();
		}

		public Point_short get_origin ()
		{
			return animation.get_origin ();
		}

		public Point_short get_dimensions ()
		{
			return animation.get_dimensions ();
		}

		private Animation animation = new Animation ();
		private DrawArgument stateargs = new DrawArgument ();
	}
}