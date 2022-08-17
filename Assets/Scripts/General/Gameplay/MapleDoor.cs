namespace ms
{
	public class MapleDoor
	{
		private int ownerId;

		/*private MapleMap town;
		private MaplePortal townPortal;
		private MapleMap target;
		private Pair<String, Integer> posStatus = null;*/
		private long deployTime;
		private bool active;

		/*private MapleDoorObject townDoor;
		private MapleDoorObject areaDoor;*/

		public int townId { get; set; }
		public int areaId { get; set; }
		
		public int fromMapId { get; set; }
		public int toMapId { get; set; }
		
		public int positionX { get; set; }
		public int positionY { get; set; }

		public MapleDoor ()
		{
			
		}
		public MapleDoor (int townId, int areaId, int positionX, int positionY)
		{
			this.townId = townId;
			this.areaId = areaId;
			this.positionX = positionX;
			this.positionY = positionY;
		}

		public void Reset ()
		{
			townId = 0;
			areaId=0;
			fromMapId = 0;
			toMapId = 0;
			positionX = 0;
			positionX = 0;
			
		}
	}
}