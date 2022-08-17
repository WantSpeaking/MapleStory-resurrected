


namespace ms
{
	// An invisible button which is only defined by it's area.
	public class AreaButton : Button
	{
		public AreaButton (Point_short pos, Point_short dim)
		{
			position = new Point_short (pos);
			dimension = new Point_short (dim);
			state = Button.State.NORMAL;
			active = true;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point_short) const
		public override void draw (Point_short UnnamedParameter1)
		{
		}

		public override void update ()
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Rectangle_short bounds(Point_short parentpos) const
		public override Rectangle_short bounds (Point_short parentpos)
		{
			Point_short absp = position + parentpos;

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: return Rectangle_short(absp, absp + dimension);
			return new Rectangle_short (new Point_short (absp), absp + dimension);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short Width() const
		public override short width ()
		{
			return dimension.x ();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point_short origin() const
		public override Point_short origin ()
		{
			return new Point_short ();
		}

		public override Cursor.State send_cursor (bool UnnamedParameter1, Point_short UnnamedParameter2)
		{
			return Cursor.State.IDLE;
		}

		private Point_short dimension = new Point_short ();
	}
}