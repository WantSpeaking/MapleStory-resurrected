using System;




namespace ms
{
	public class InventoryType
	{
		// Inventory types
		public enum Id : sbyte
		{
			NONE,
			EQUIP,
			USE,
			SETUP,
			ETC,
			CASH,
			EQUIPPED,
		}

		public static readonly Id[] values_by_id = new[]
		{
			Id.NONE,
			Id.EQUIP,
			Id.USE,
			Id.SETUP,
			Id.ETC,
			Id.CASH
		};

		public static Id by_item_id (int item_id)
		{
			int prefix = item_id / 1000000;

			return (prefix > (int)Id.NONE && prefix <= (int)Id.CASH) ? values_by_id[prefix] : Id.NONE;
		}

		public static InventoryType.Id by_value (sbyte value)
		{
			switch (value)
			{
				case -1:
					return Id.EQUIPPED;
				case 1:
					return Id.EQUIP;
				case 2:
					return Id.USE;
				case 3:
					return Id.SETUP;
				case 4:
					return Id.ETC;
				case 5:
					return Id.CASH;
			}

            AppDebug.Log ($"Unknown InventoryType.Id value: {value}");
			//std.cout << "Unknown InventoryType.Id value: [" << value << "]" << std.endl;

			return Id.NONE;
		}
	}

	public class InventoryPosition
	{
		public InventoryType.Id type;
		public short slot;
	}
}