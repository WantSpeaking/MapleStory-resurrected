using System.Collections.Generic;




namespace ms
{
	// Saved locations for the 'teleport rock' and 'VIP teleport rock' cash items
	public class TeleportRock
	{
		public void addlocation(int mapid)
		{
			locations.Add(mapid);
		}
		public void addviplocation(int mapid)
		{
			viplocations.Add(mapid);
		}

		private List<int> locations = new List<int>();
		private List<int> viplocations = new List<int>();
	}
}
