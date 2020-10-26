

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
	public class Stance
	{
		public enum Id : byte
		{
			NONE,
			ALERT,
			DEAD,
			FLY,
			HEAL,
			JUMP,
			LADDER,
			PRONE,
			PRONESTAB,
			ROPE,
			SHOT,
			SHOOT1,
			SHOOT2,
			SHOOTF,
			SIT,
			STABO1,
			STABO2,
			STABOF,
			STABT1,
			STABT2,
			STABTF,
			STAND1,
			STAND2,
			SWINGO1,
			SWINGO2,
			SWINGO3,
			SWINGOF,
			SWINGP1,
			SWINGP2,
			SWINGPF,
			SWINGT1,
			SWINGT2,
			SWINGT3,
			SWINGTF,
			WALK1,
			WALK2,
			LENGTH
		}

		public static EnumMap<Id, string> names = new EnumMap<Id, string>
		{
			[Id.NONE] = "",
			[Id.ALERT] = "alert",
			[Id.DEAD] = "dead",
			[Id.FLY] = "fly",
			[Id.HEAL] = "heal",
			[Id.JUMP] = "jump",
			[Id.LADDER] = "ladder",
			[Id.PRONE] = "prone",
			[Id.PRONESTAB] = "proneStab",
			[Id.ROPE] = "rope",
			[Id.SHOT] = "shot",
			[Id.SHOOT1] = "shoot1",
			[Id.SHOOT2] = "shoot2",
			[Id.SHOOTF] = "shootF",
			[Id.SIT] = "sit",
			[Id.STABO1] = "stabO1",
			[Id.STABO2] = "stabO2",
			[Id.STABOF] = "stabOF",
			[Id.STAND1] = "stabT1",
			[Id.STAND2] = "stabT2",
			[Id.STABTF] = "stabTF",
			[Id.STAND1] = "stand1",
			[Id.STAND2] = "stand2",
			[Id.SWINGO1] = "swingO1",
			[Id.SWINGO2] = "swingO2",
			[Id.SWINGO3] = "swingO3",
			[Id.SWINGOF] = "swingOF",
			[Id.SWINGP1] = "swingP1",
			[Id.SWINGP2] = "swingP2",
			[Id.SWINGPF] = "swingPF",
			[Id.SWINGT1] = "swingT1",
			[Id.SWINGT2] = "swingT2",
			[Id.SWINGT3] = "swingT3",
			[Id.SWINGTF] = "swingTF",
			[Id.WALK1] = "walk1",
			[Id.WALK2] = "walk2",
		};

		public static Id[] statevalues =
		{
			Id.WALK1,
			Id.STAND1,
			Id.JUMP,
			Id.ALERT,
			Id.PRONE,
			Id.FLY,
			Id.LADDER,
			Id.ROPE,
			Id.DEAD,
			Id.SIT
		};

		public static Id by_state (sbyte state)
		{
			sbyte index = (sbyte)((state / 2) - 1);

			if (index < 0 || index > 10)
				return Id.WALK1;


			return statevalues[index];
		}

		public static Id by_id (byte id)
		{
			if (id <= (int)Id.NONE || id >= (int)Id.LENGTH)
				return Id.NONE;

			return (Id) (id);
		}

		public static Id by_string (string name)
		{
			foreach (var pair in names)
			{
				if (pair.Value == name)
				{
					return pair.Key;
				}
			}
			/*for (var iter : names)
			if (iter.second == name)
				return iter.first;

			cout << "Unknown Stance::Id name: [" << name << "]" << endl;*/

			return Id.NONE;
		}

		public static bool is_climbing (Id value)
		{
			return value == Id.LADDER || value == Id.ROPE;
		}

		public static Id baseof (Id value)
		{
			switch (value)
			{
				case Id.STAND2:
					return Id.STAND1;
				case Id.WALK2:
					return Id.WALK1;
				default:
					return value;
			}
		}
	}
}