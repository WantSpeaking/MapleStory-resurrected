using System;
using System.Collections.Generic;
using UnityEngine;

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
	public class EquipSlot
	{
		public enum Id : short
		{
			NONE = 0,
			HAT = 1,
			FACE = 2,
			EYEACC = 3,
			EARACC = 4,
			TOP = 5,
			BOTTOM = 6,
			SHOES = 7,
			GLOVES = 8,
			CAPE = 9,
			SHIELD = 10, // TODO: Where is this now?
			WEAPON = 11,
			RING1 = 12,
			RING2 = 13,
			RING3 = 15,
			RING4 = 16,
			PENDANT1 = 17,
			TAMEDMOB = 18, // TODO: Where is this now?
			SADDLE = 19, // TODO: Where is this now?
			MEDAL = 49,
			BELT = 50,
			POCKET, // TODO: What is the proper value for this?
			BOOK, // TODO: What is the proper value for this?
			PENDANT2, // TODO: What is the proper value for this?
			SHOULDER, // TODO: What is the proper value for this?
			ANDROID, // TODO: What is the proper value for this?
			EMBLEM, // TODO: What is the proper value for this?
			BADGE, // TODO: What is the proper value for this?
			SUBWEAPON, // TODO: What is the proper value for this?
			HEART, // TODO: What is the proper value for this?
			LENGTH
		}

		public static List<Id> values = new List<Id> ()
		{
			Id.NONE,
			Id.HAT,
			Id.FACE,
			Id.EYEACC,
			Id.EARACC,
			Id.TOP,
			Id.BOTTOM,
			Id.SHOES,
			Id.GLOVES,
			Id.CAPE,
			Id.SHIELD, // TODO: Where is this now?
			Id.WEAPON,
			Id.RING1,
			Id.RING2,
			Id.RING3,
			Id.RING4,
			Id.PENDANT1,
			Id.TAMEDMOB, // TODO: Where is this now?
			Id.SADDLE, // TODO: Where is this now?
			Id.MEDAL,
			Id.BELT,
			Id.POCKET, // TODO: What is the proper value for this?
			Id.BOOK, // TODO: What is the proper value for this?
			Id.PENDANT2, // TODO: What is the proper value for this?
			Id.SHOULDER, // TODO: What is the proper value for this?
			Id.ANDROID, // TODO: What is the proper value for this?
			Id.EMBLEM, // TODO: What is the proper value for this?
			Id.BADGE, // TODO: What is the proper value for this?
			Id.SUBWEAPON, // TODO: What is the proper value for this?
			Id.HEART, // TODO: What is the proper value for this?
		};

		public static Id by_id (short id)
		{
			if (id >= (short)Id.LENGTH)
			{
				Debug.Log ($"Unknown EquipSlot::Id id: [{id}]");
				return Id.NONE;
			}

			return (Id)(id);
		}
	}
}