


namespace ms
{
	public class Item
	{
		public Item(int item_id, long expiration, string owner, short flags)
		{
			this.item_id = item_id;
			this.expiration = expiration;
			this.owner = owner;
			this.flags = flags;
		}

		private int item_id;
		private long expiration;
		private string owner;
		private short flags;
	}
}
