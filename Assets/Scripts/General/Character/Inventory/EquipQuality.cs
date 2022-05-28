


namespace ms
{
	public class EquipQuality
	{
		public enum Id
		{
			GREY,
			WHITE,
			ORANGE,
			BLUE,
			VIOLET,
			GOLD
		}

		public static Id check_quality (int item_id, bool scrolled,   EnumMap<EquipStat.Id, ushort> stats)
		{
			EquipData data = EquipData.get (item_id);

			var delta = 0;
			foreach (var pair in stats)
			{
				EquipStat.Id es = pair.Key;
				ushort stat = pair.Value;
				short defstat = data.get_defstat (es);
				delta += stat - defstat;
			}

			if (delta < -5)
				return scrolled ? Id.ORANGE : Id.GREY;
			if (delta < 7)
				return scrolled ? Id.ORANGE : Id.WHITE;
			if (delta < 14)
				return Id.BLUE;
			if (delta < 21)
				return Id.VIOLET;
			return Id.GOLD;
		}
	}
}