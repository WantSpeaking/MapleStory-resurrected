




using System;

namespace ms
{
	public class Portal:IDisposable
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
			public int targetMapid;
			public string targetName;
			public string name;
			public bool intramap;
			public bool valid;
            public string script;

             public WarpInfo (int m, bool i, string tn, string n,string script = null)
			{
				this.targetMapid = m;
				this.intramap = i;
				this.targetName = tn;
				this.name = n;
				valid = targetMapid < 999999999 || !string.IsNullOrEmpty(script);
                this.script = script;

            }

            /*	public WarpInfo()
                {
                    this.mapid = 999999999;
                    this.intramap = false;
                    this.toname = string.Empty;
                    this.name = string.Empty;
                    valid = mapid < 999999999;
                }*/
            public override string ToString ()
            {
	            return $"WarpInfo mapid:{targetMapid}\t intramap:{intramap}\t toname:{targetName}\t name:{name}\t script:{script}";
            }
		}

		public Portal (Animation a, Type t, string nm, bool intramap, Point_short p, int tid, string tnm, string script = null)
		{
			this.animation = a;// class Portal（） 原c++传递的是Animation指针
			this.type = t;
			this.name = nm;
			this.position = new ms.Point_short (p);
			this.warpinfo = new ms.Portal.WarpInfo (tid, intramap, tnm, nm, script);
			touched = false;
			this.script = script;
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

		public Portal.Type get_type ()
		{
			return type;
		}

		public Point_short get_position ()
		{
			return position;
		}

		public Rectangle_short bounds ()
		{
			var lt = position + new Point_short (-25, -100);
			var rb = position + new Point_short (25, 25);

			return new Rectangle_short (lt, rb);
		}

		public Portal.WarpInfo getwarpinfo ()
		{
			return warpinfo;
		}

        public void Dispose()
        {
			animation?.Dispose();
        }

        private readonly Animation animation;
		private Type type;
		private string name;
		private Point_short position = new Point_short ();
		private WarpInfo warpinfo = new WarpInfo ();
		private bool touched;
        private string script;
    }
}