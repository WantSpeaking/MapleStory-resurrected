


using MapleLib.WzLib;

namespace ms
{
	public class TwoSpriteButton : Button
	{
		public TwoSpriteButton (WzObject nsrc, WzObject ssrc, Point_short np, Point_short sp)
		{
			this.textures = new ms.BoolPairNew<Texture> (ssrc, nsrc);
			this.npos = new Point_short (np);
			this.spos = new Point_short (sp);
			state = Button.State.NORMAL;
			active = true;
		}

		public TwoSpriteButton (WzObject nsrc, WzObject ssrc, Point_short pos) : this (nsrc, ssrc, new Point_short (pos), new Point_short (pos))
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this(nsrc, ssrc, Point_short());
		public TwoSpriteButton (WzObject nsrc, WzObject ssrc) : this (nsrc, ssrc, new Point_short ())
		{
		}

		public TwoSpriteButton ()
		{
			this.textures = new ms.BoolPairNew<Texture> (new Texture (), new Texture ());
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point_short parentpos) const
		public override void draw (Point_short parentpos)
		{
			if (active)
			{
				bool selected = state == Button.State.MOUSEOVER || state == Button.State.PRESSED;

				if (selected)
				{
					textures[selected].draw (spos + parentpos);
				}
				else
				{
					textures[selected].draw (npos + parentpos);
				}
			}
		}

		public override void update ()
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Rectangle_short bounds(Point_short parentpos) const
		public override Rectangle_short bounds (Point_short parentpos)
		{
			bool selected = state == Button.State.MOUSEOVER || state == Button.State.PRESSED;
			Point_short absp = new Point_short ();
			Point_short dim = new Point_short ();

			if (selected)
			{
				absp = parentpos + spos - textures[selected].get_origin ();
				dim = textures[selected].get_dimensions ();
			}
			else
			{
				absp = parentpos + npos - textures?[selected]?.get_origin ()??Point_short.zero;
				dim = textures?[selected]?.get_dimensions () ?? Point_short.zero;
			}

			return new Rectangle_short (new Point_short (absp), absp + dim);
		}

		public override short width ()
		{
			bool selected = state == Button.State.MOUSEOVER || state == Button.State.PRESSED;

			return textures[selected].width ();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point_short origin() const
		public override Point_short origin ()
		{
			bool selected = state == Button.State.MOUSEOVER || state == Button.State.PRESSED;

			return textures[selected].get_origin ();
		}

		public override Cursor.State send_cursor (bool UnnamedParameter1, Point_short UnnamedParameter2)
		{
			return Cursor.State.IDLE;
		}

		public override void Dispose ()
		{
			base.Dispose ();
			textures[true]?.Dispose ();
			textures[false]?.Dispose ();
			//textures = null;
		}

		private BoolPairNew<Texture> textures = new BoolPairNew<Texture> ();
		private Point_short npos = new Point_short ();
		private Point_short spos = new Point_short ();
		
		
	}
}