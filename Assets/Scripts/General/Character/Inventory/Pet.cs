


namespace ms
{
	public class Pet
	{
		public Pet(int item_id, long expiration, string petname, byte level, ushort closeness, byte fullness)
		{
			this.item_id = item_id;
			this.expiration = expiration;
			this.petname = petname;
			this.petlevel = level;
			this.closeness = closeness;
			this.fullness = fullness;
		}

		private int item_id;
		private long expiration;
		private string petname;
		private byte petlevel;
		private ushort closeness;
		private byte fullness;
	}
}
