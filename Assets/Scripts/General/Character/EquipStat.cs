namespace ms
{
	public class EquipStat
	{
		public enum Id
		{
			STR,
			DEX,
			INT,
			LUK,
			HP,
			MP,
			WATK,
			MAGIC,
			WDEF,
			MDEF,
			ACC,
			AVOID,
			HANDS,
			SPEED,
			JUMP,
		}
		
		public static readonly string[] names={
			"STR",
			"DEX",
			"INT",
			"LUK",
			"MaxHP",
			"MaxMP",
			"Attack Power",
			"Magic Attack",
			"Defense",

			// TODO: Does current GMS use these anymore?
			"MAGIC DEFENSE",
			"ACCURACY",
			"AVOID",
			"HANDS",

			"Speed",
			"Jump"
		};
	}
}

