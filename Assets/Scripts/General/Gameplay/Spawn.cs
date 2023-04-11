




namespace ms
{
	public class NpcSpawn
	{
		public NpcSpawn(int o, int i, Point_short p, bool fl, ushort f)
		{
			this.oid = o;
			this.id = i;
			this.position = new Point_short (p);
			this.flip = fl;
			this.fh = f;
		}

		public int get_oid()
		{
			return oid;
		}
		public int get_id ()
		{
			return id;
		}
		public MapObject instantiate(Physics physics)
		{
			var spawnposition = physics.get_y_below(new Point_short (position));
			//return new Npc(id, oid, flip, fh, false, spawnposition) ;
            return new Npc(id, oid, flip, fh, false, position);

        }

        private int oid;
		private int id;
		private Point_short position = new Point_short();
		private bool flip;
		private ushort fh;
	}

	public class MobSpawn
	{
		public MobSpawn(int o, int i, sbyte m, sbyte st, ushort f, bool ns, sbyte t, Point_short p)
		{
			this.oid = o;
			this.id = i;
			this.mode = m;
			this.stance = st;
			this.fh = f;
			this.newspawn = ns;
			this.team = t;
			this.position = new ms.Point_short(p);
		}

		public sbyte get_mode()
		{
			return mode;
		}
		public int get_oid()
		{
			return oid;
		}
		public MapObject instantiate()
		{
			return new Mob(oid, id, mode, stance, fh, newspawn, team, position);
		}

		private int oid;
		private int id;
		private sbyte mode;
		private sbyte stance;
		private ushort fh;
		private bool newspawn;
		private sbyte team;
		private Point_short position;
	}

	public class ReactorSpawn
	{
		public ReactorSpawn(int o, int r, sbyte s, Point_short p)
		{
			this.oid = o;
			this.rid = r;
			this.state = s;
			this.position = new ms.Point_short(p);
		}

		public int get_oid()
		{
			return oid;
		}
		public MapObject instantiate(Physics physics)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: var spawnposition = physics.get_y_below(position);
			var spawnposition = physics.get_y_below(new ms.Point_short(position));
			return new Reactor(oid, rid, state, spawnposition);
		}

		private int oid;
		private int rid;
		private sbyte state;
		private Point_short position = new Point_short();
	}

	public class DropSpawn
	{
		public DropSpawn(int o, int i, bool ms, int ow, Point_short p, Point_short d, sbyte t, sbyte m, bool pd)
		{
			this.oid = o;
			this.id = i;
			this.meso = ms;
			this.owner = ow;
			this.start = new ms.Point_short(p);
			this.dest = new ms.Point_short(d);
			this.droptype = t;
			this.mode = m;
			this.playerdrop = pd;
		}

		public bool is_meso()
		{
			return meso;
		}
		public int get_itemid()
		{
			return id;
		}
		public int get_oid()
		{
			return oid;
		}
		public MapObject instantiate(Animation icon)
		{
			return new MesoDrop(oid, owner, start, dest, droptype, mode, playerdrop, icon);
		}
		public MapObject instantiate(Texture icon)
		{
			return new ItemDrop(oid, owner, start, dest, droptype, mode, id, playerdrop, icon);
		}

		private int oid;
		private int id;
		private bool meso;
		private int owner;
		private Point_short start = new Point_short();
		private Point_short dest = new Point_short();
		private sbyte droptype;
		private sbyte mode;
		private bool playerdrop;
	}

	public class CharSpawn
	{
		public CharSpawn(int c, LookEntry lk, byte l, short j, string nm, sbyte st, Point_short p)
		{
			this.cid = c;
			this.look = lk;
			this.level = l;
			this.job = j;
			this.name = nm;
			this.stance = st;
			this.position = new Point_short(p);
		}

		public int get_cid()
		{
			return cid;
		}
		public MapObject instantiate()
		{
			return new OtherChar(cid,new CharLook (look), level, job, name, stance, position);
		}

		private int cid;
		private byte level;
		private short job;
		private string name;
		private sbyte stance;
		private Point_short position = new Point_short();
		private LookEntry look = new LookEntry();
	}
}


