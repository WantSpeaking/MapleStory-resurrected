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
	public class MapleStat
	{
		public enum Id
		{
			SKIN,
			FACE,
			HAIR,
			LEVEL,
			JOB,
			STR,
			DEX,
			INT,
			LUK,
			HP,
			MAXHP,
			MP,
			MAXMP,
			AP,
			SP,
			EXP,
			FAME,
			MESO,
			PET,
			GACHAEXP,
		}

		public static readonly EnumMap<Id, int> codes = new EnumMap<Id, int> ()
		{
			[Id.SKIN] = 0x1,
			[Id.SKIN] = 0x2,
			[Id.SKIN] = 0x4,
			[Id.SKIN] = 0x10,
			[Id.SKIN] = 0x20,
			[Id.SKIN] = 0x40,
			[Id.SKIN] = 0x80,
			[Id.SKIN] = 0x100,
			[Id.SKIN] = 0x200,
			[Id.SKIN] = 0x400,
			[Id.SKIN] = 0x800,
			[Id.SKIN] = 0x1000,
			[Id.SKIN] = 0x2000,
			[Id.SKIN] = 0x4000,
			[Id.SKIN] = 0x8000,
			[Id.SKIN] = 0x10000,
			[Id.SKIN] = 0x20000,
			[Id.SKIN] = 0x40000,
			[Id.SKIN] = 0x180008,
			[Id.SKIN] = 0x200000,
		};
	}
}