using System;




namespace ms
{
	public class Weapon
	{
		public enum Type
		{
			NONE = 0,
			SWORD_1H = 130,
			AXE_1H = 131,
			MACE_1H = 132,
			DAGGER = 133,
			WAND = 137,
			STAFF = 138,
			SWORD_2H = 140,
			AXE_2H = 141,
			MACE_2H = 142,
			SPEAR = 143,
			POLEARM = 144,
			BOW = 145,
			CROSSBOW = 146,
			CLAW = 147,
			KNUCKLE = 148,
			GUN = 149,
			CASH = 170
		}

		public static Type by_value (int value)
		{
			if (value < 130 || (value > 133 && value < 137) || value == 139 || (value > 149 && value < 170) || value > 170)
			{
				if (value != 100)
					AppDebug.Log($"Unknown Weapon::Type [{value}:]");
				//cout << "Unknown Weapon::Type value: [" << value << "]" << endl;

				return Type.NONE;
			}

			return (Type) (value);
		}
	}
}