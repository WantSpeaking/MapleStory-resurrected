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
	public class NpcSpawn
	{
		public NpcSpawn(int o, int i, Point<short> p, bool fl, ushort f)
		{
			this.oid = o;
			this.id = i;
			this.position = new Point<short> (p);
			this.flip = fl;
			this.fh = f;
		}

		public int get_oid()
		{
			return oid;
		}
		public MapObject instantiate(Physics physics)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: var spawnposition = physics.get_y_below(position);
			var spawnposition = physics.get_y_below(position);
			return new Npc(id, oid, flip, fh, false, spawnposition) ;
		}

		private int oid;
		private int id;
		private Point<short> position = new Point<short>();
		private bool flip;
		private ushort fh;
	}

	public class MobSpawn
	{
		public MobSpawn(int o, int i, sbyte m, sbyte st, ushort f, bool ns, sbyte t, Point<short> p)
		{
			this.oid = o;
			this.id = i;
			this.mode = m;
			this.stance = st;
			this.fh = f;
			this.newspawn = ns;
			this.team = t;
			this.position = new ms.Point<short>(p);
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
		private Point<short> position;
	}

	public class ReactorSpawn
	{
		public ReactorSpawn(int o, int r, sbyte s, Point<short> p)
		{
			this.oid = o;
			this.rid = r;
			this.state = s;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this.position = new ms.Point<short>(p);
			this.position = p;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_oid() const
		public int get_oid()
		{
			return oid;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: unique_ptr<MapObject> instantiate(const Physics& physics) const
		public MapObject instantiate(Physics physics)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: var spawnposition = physics.get_y_below(position);
			var spawnposition = physics.get_y_below(position);
			return new Reactor(oid, rid, state, spawnposition);
		}

		private int oid;
		private int rid;
		private sbyte state;
		private Point<short> position = new Point<short>();
	}

	public class DropSpawn
	{
		public DropSpawn(int o, int i, bool ms, int ow, Point<short> p, Point<short> d, sbyte t, sbyte m, bool pd)
		{
			this.oid = o;
			this.id = i;
			this.meso = ms;
			this.owner = ow;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this.start = new ms.Point<short>(p);
			this.start = p;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this.dest = new ms.Point<short>(d);
			this.dest = d;
			this.droptype = t;
			this.mode = m;
			this.playerdrop = pd;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_meso() const
		public bool is_meso()
		{
			return meso;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_itemid() const
		public int get_itemid()
		{
			return id;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_oid() const
		public int get_oid()
		{
			return oid;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: unique_ptr<MapObject> instantiate(const Animation& icon) const
		public MapObject instantiate(Animation icon)
		{
			return new MesoDrop(oid, owner, start, dest, droptype, mode, playerdrop, icon);
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: unique_ptr<MapObject> instantiate(const Texture& icon) const
		public MapObject instantiate(Texture icon)
		{
			return new ItemDrop(oid, owner, start, dest, droptype, mode, id, playerdrop, icon);
		}

		private int oid;
		private int id;
		private bool meso;
		private int owner;
		private Point<short> start = new Point<short>();
		private Point<short> dest = new Point<short>();
		private sbyte droptype;
		private sbyte mode;
		private bool playerdrop;
	}

	public class CharSpawn
	{
		public CharSpawn(int c, LookEntry lk, byte l, short j, string nm, sbyte st, Point<short> p)
		{
			this.cid = c;
			this.look = lk;
			this.level = l;
			this.job = j;
			this.name = nm;
			this.stance = st;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this.position = new ms.Point<short>(p);
			this.position = p;
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
		private Point<short> position = new Point<short>();
		private LookEntry look = new LookEntry();
	}
}


