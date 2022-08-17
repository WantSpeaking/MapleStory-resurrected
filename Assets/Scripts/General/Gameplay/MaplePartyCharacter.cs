namespace ms
{
	public class MaplePartyCharacter
	{
		public string Name { get; set; }
		public int id { get; set; }
		public int level { get; set; }
		public int channel { get; set; }
		public int world { get; set; }
		public int jobid { get; set; }
		public int mapid { get; set; }
		public bool online { get; set; }
		public Job job;
		public Char character;

		public bool isValid => id != 0 && channel != -2;
		/*public MaplePartyCharacter (Char maplechar)
		{
			this.character = maplechar;
			this.name = maplechar.getName ();
			this.level = maplechar.getLevel ();
			this.channel = maplechar.getClient ().getChannel ();
			this.world = maplechar.getWorld ();
			this.id = maplechar.getId ();
			this.jobid = maplechar.getJob ().getId ();
			this.mapid = maplechar.getMapId ();
			this.online = true;
			this.job = maplechar.getJob ();
		}*/

		public MaplePartyCharacter ()
		{
		}

		public MaplePartyCharacter (int id, string name, int jobId, int level, int channel, int mapid)
		{
			this.id = id;
			this.Name = name;
			this.jobid = jobId;
			this.level = level;
			this.channel = channel;
			this.mapid = mapid;
		}

		public void Reset ()
		{
			Name = null;
			id = 0;
			level = 0;
			channel = 0;
			world = 0;
			jobid = 0;
			mapid = 0;
			online = false;
		}
		/*public MapleCharacter getPlayer ()
		{
			return character;
		}

		public MapleJob getJob ()
		{
			return job;
		}*/

		public int getLevel ()
		{
			return level;
		}

		public int getChannel ()
		{
			return channel;
		}

		public void setChannel (int channel)
		{
			this.channel = channel;
		}

		/*public bool isLeader ()
		{
			return getPlayer ().isPartyLeader ();
		}*/

		public bool isOnline ()
		{
			return online;
		}

		public void setOnline (bool online)
		{
			this.online = online;
			if (!online)
			{
				this.character = null; // thanks Feras for noticing offline party members retaining whole character object unnecessarily
			}
		}

		public int getMapId ()
		{
			return mapid;
		}

		public void setMapId (int mapid)
		{
			this.mapid = mapid;
		}

		public string getName ()
		{
			return Name;
		}

		public int getId ()
		{
			return id;
		}

		public int getJobId ()
		{
			return jobid;
		}

		/*public int getGuildId ()
		{
			return character.getGuildId ();
		}*/


		/*public int hashCode ()
		{
			final int prime = 31;
			int result = 1;
			result = prime * result + ((name == null) ? 0 : name.hashCode ());
			return result;
		}*/


		/*public bool equals (object obj)
		{
			if (this == obj)
			{
				return true;
			}

			if (obj == null)
			{
				return false;
			}

			if (getClass () != obj.getClass ())
			{
				return false;
			}

			final MaplePartyCharacter other = (MaplePartyCharacter)obj;
			if (name == null)
			{
				if (other.name != null)
				{
					return false;
				}
			}
			else if (!name.equals (other.name))
			{
				return false;
			}

			return true;
		}*/

		public int getWorld ()
		{
			return world;
		}
	}
}