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


using ms.Helper;

namespace ms
{
	// Opcode: LOGIN(1)
	public class LoginPacket : OutPacket
	{
		// Request to be logged-in to an account
		public LoginPacket(string acc, string pass) : base((short)OutPacket.Opcode.LOGIN)
		{
			string volumeSerialNumber = Configuration.get().get_vol_serial_num();

			string part1 = volumeSerialNumber.Substring(0, 2);
			string part2 = volumeSerialNumber.Substring(2, 2);
			string part3 = volumeSerialNumber.Substring(4, 2);
			string part4 = volumeSerialNumber.Substring(6, 2);

			int h = hex_to_dec(part4);
			int w = hex_to_dec(part3);
			int i = hex_to_dec(part2);
			int d = hex_to_dec(part1);

			write_string(acc);
			write_string(pass);

			skip(6);

			write_byte((sbyte)h);
			write_byte((sbyte)w);
			write_byte((sbyte)i);
			write_byte((sbyte)d);
		}
	}

	// Opcode: CHARLIST_REQUEST(5)
	public class CharlistRequestPacket : OutPacket
	{
		// Requests the list of characters on a world
		public CharlistRequestPacket(byte world, byte channel) : base((short)OutPacket.Opcode.CHARLIST_REQUEST)
		{
			write_byte(0);
			write_byte((sbyte)world);
			write_byte((sbyte)channel);
		}
	}

	// Opcode: SERVERSTATUS_REQUEST(6)
	public class ServerStatusRequestPacket : OutPacket
	{
		// Requests the status of the server
		public ServerStatusRequestPacket(short world) : base((short)OutPacket.Opcode.SERVERSTATUS_REQUEST)
		{
			write_short(world);
		}
	}

	// Opcode: ACCEPT_TOS(7)
	public class TOSPacket : OutPacket
	{
		// Accept the Terms of Service
		public TOSPacket() : base((short)OutPacket.Opcode.ACCEPT_TOS)
		{
			write_byte(1);
		}
	}

	// Opcode: SET_GENDER(8)
	public class GenderPacket : OutPacket
	{
		// Send user selected Gender
		public GenderPacket(bool female) : base((short)OutPacket.Opcode.SET_GENDER)
		{
			write_byte(1);
			write_byte(female.ToSByte ());
		}
	}

	// Opcode: SERVERLIST_REQUEST(11)
	public class ServerRequestPacket : OutPacket
	{
		// Requests the list of worlds and channels
		public ServerRequestPacket() : base((short)OutPacket.Opcode.SERVERLIST_REQUEST)
		{
		}
	}

	// Opcode: PLAYER_LOGIN(20)
	public class PlayerLoginPacket : OutPacket
	{
		// Requests being logged-in to a channel server with the specified character
		public PlayerLoginPacket(int cid) : base((short)OutPacket.Opcode.PLAYER_LOGIN)
		{
			write_int(cid);
		}
	}

	// Opcode: LOGIN_START(35)
	public class LoginStartPacket : OutPacket
	{
		// Sends whenever we hit the start of the Login screen
		public LoginStartPacket() : base((short)OutPacket.Opcode.LOGIN_START)
		{
		}
	}
}