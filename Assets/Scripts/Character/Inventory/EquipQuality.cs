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