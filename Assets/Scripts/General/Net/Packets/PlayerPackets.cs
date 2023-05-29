using System.Collections.Generic;

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
	// Requests a stat increase by spending AP
	// Opcode: SPEND_AP(87)
	public class SpendApPacket : OutPacket
	{
		public SpendApPacket(MapleStat.Id stat, int assignCount = 1) : base((short)OutPacket.Opcode.SPEND_AP)
		{
			write_time();
			write_int(MapleStat.codes[stat]);
			write_int(assignCount);
		}
	}
	
	public class AUTO_DISTRIBUTE_AP_Packet : OutPacket
	{
		public AUTO_DISTRIBUTE_AP_Packet() : base((short)OutPacket.Opcode.AUTO_DISTRIBUTE_AP)
		{
			skip (8);
			skip (1);//opt  useful for pirate autoassigning
		}
	}
	// Requests a skill level increase by spending SP
	// Opcode: SPEND_SP(90)
	public class SpendSpPacket : OutPacket
	{
		public SpendSpPacket(int skill_id,int count = 1) : base((short)OutPacket.Opcode.SPEND_SP)
		{
			write_time();
			write_int(skill_id);
            write_int(count);
        }
	}

	// Requests the server to change key mappings
	// Opcode: CHANGE_KEYMAP(135)
	public class ChangeKeyMapPacket : OutPacket
	{
		public ChangeKeyMapPacket(List<System.Tuple<KeyConfig.Key, KeyType.Id, int>> updated_actions) : base((short)OutPacket.Opcode.CHANGE_KEYMAP)
		{
			write_int(0); // mode
			write_int(updated_actions.Count); // Number of key changes

			for (uint i = 0; i < updated_actions.Count; i++)
			{
				var keymap = updated_actions[(int)i];

				write_int((int)keymap.Item1); // key
				write_byte((sbyte)keymap.Item2); // type
				write_int(keymap.Item3); // action
			}
		}
	}
}