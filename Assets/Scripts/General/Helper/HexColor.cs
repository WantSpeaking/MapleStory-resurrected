namespace ms.Util
{
	public static class HexColor
	{
		
		/// <summary>
		/// b 0000FF
		/// </summary>
		public static string Blue = "#0000FF";
		
		/// <summary>
		/// d
		/// </summary>
		public static string Purple  = "#800080";
		
		/// <summary>
		/// g
		/// </summary>
		public static string Green = "#008000";
		
		/// <summary>
		/// k
		/// </summary>
		public static string Black = "#000000";
		
		/// <summary>
		/// r
		/// </summary>
		public static string Red = "#FF0000";

		public static string MSColorTagToHexColor (string tag)
		{
			switch (tag)
			{
				case "b":
					return Blue;
				case "d":
					return Purple;
				case "g":
					return Green;
				case "k":
					return Black;
				case "r":
					return Red;
			}

			return Black;
		}
	}
}