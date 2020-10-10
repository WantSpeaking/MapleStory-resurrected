namespace ms
{
	public static class GlobalMembers
	{
		public const double GRAVFORCE = 0.14;
		public	const double SWIMGRAVFORCE = 0.03;
		public	const double FRICTION = 0.3;
		public	const double SLOPEFACTOR = 0.1;
		public	const double GROUNDSLIP = 3.0;
		public	const double FLYFRICTION = 0.05;
		public	const double SWIMFRICTION = 0.08;
		
		// Timestep, e.g. the granularity in which the game advances.
		public const ushort TIMESTEP = 8;
		public const string SortingLayer_Back = "Back";
		public const string SortingLayer_Front = "Front";
		public const string SortingLayer_Obj = "Obj";
		public const string SortingLayer_Tile = "Tile";
	}
}