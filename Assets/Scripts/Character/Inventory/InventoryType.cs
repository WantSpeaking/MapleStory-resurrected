using System;

//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////


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
			LENGTH
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

			Console.WriteLine ($"Unknown InventoryType.Id value: {value}");
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