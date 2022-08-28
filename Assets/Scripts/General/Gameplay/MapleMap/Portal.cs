




namespace ms
{
	public class Portal
	{
		public enum Type
		{
			SPAWN,
			INVISIBLE,
			REGULAR,
			TOUCH,
			TYPE4,
			TYPE5,
			WARP,
			SCRIPTED,
			SCRIPTED_INVISIBLE,
			SCRIPTED_TOUCH,
			HIDDEN,
			SCRIPTED_HIDDEN,
			SPRING1,
			SPRING2,
			TYPE14
		}

		public static Type typebyid (int id)
		{
			return (Type)id;
		}

		public struct WarpInfo
		{
			public int mapid;
			public string toname;
			public string name;
			public bool intramap;
			public bool valid;

			public WarpInfo (int m, bool i, string tn, string n)
			{
				this.mapid = m;
				this.intramap = i;
				this.toname = tn;
				this.name = n;
				valid = mapid < 999999999;
			}

			/*public WarpInfo() : this(999999999, false, string.Empty, string.Empty)
			{
			}*/
		}

		public Portal (Animation a, Type t, string nm, bool intramap, Point_short p, int tid, string tnm)
		{
			//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
			//ORIGINAL LINE: this.animation = a;
			this.animation = new Animation (a);// class Portal（） 原c++传递的是Animation指针
			this.type = t;
			this.name = nm;
			this.position = new ms.Point_short (p);
			this.warpinfo = new ms.Portal.WarpInfo (tid, intramap, tnm, nm);
			touched = false;
		}

		public Portal () : this (null, Type.SPAWN, "", false, new Point_short (), 0, "")
		{
		}

		public void update (Point_short playerpos)
		{
			touched = bounds ().contains (playerpos);
		}

		public void draw (Point_short viewpos, float inter)
		{
			if (animation == null || (type == Type.HIDDEN && !touched))
			{
				animation?.eraseAllFrame ();
				return;
			}

			//AppDebug.Log ($"protal type:{type,-10} name:{name,-10}, draw postion: {position}");
			animation.update ();
			animation.draw (new DrawArgument (position + viewpos), inter);
		}

		public string get_name ()
		{
			return name;
		}

		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: Portal::Type get_type() const
		public Portal.Type get_type ()
		{
			return type;
		}

		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: Point_short get_position() const
		public Point_short get_position ()
		{
			return position;
		}

		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: Rectangle_short bounds() const
		public Rectangle_short bounds ()
		{
			var lt = position + new Point_short (-25, -100);
			var rb = position + new Point_short (25, 25);

			return new Rectangle_short (lt, rb);
		}

		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: Portal::WarpInfo getwarpinfo() const
		public Portal.WarpInfo getwarpinfo ()
		{
			return warpinfo;
		}

		private readonly Animation animation;
		private Type type;
		private string name;
		private Point_short position = new Point_short ();
		private WarpInfo warpinfo = new WarpInfo ();
		private bool touched;
	}
}