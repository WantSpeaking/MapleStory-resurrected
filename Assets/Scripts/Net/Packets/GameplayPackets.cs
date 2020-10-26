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
	// Opcode: CHANGE_MAP(38)
	public class ChangeMapPacket : OutPacket
	{
		// Request the server to warp the player to a different map
		public ChangeMapPacket(bool died, int targetid, string targetp, bool usewheel) : base((short)OutPacket.Opcode.CHANGEMAP)
		{
			write_byte(died.ToSByte ());
			write_int(targetid);
			write_string(targetp);
			skip(1);
			write_short((short)(usewheel ? 1 : 0));
		}

		// Request the server to exit the cash shop
		public ChangeMapPacket() : base((short)OutPacket.Opcode.CHANGEMAP)
		{
		}
	}

	// Opcode: ENTER_CASHSHOP(40)
	public class EnterCashShopPacket : OutPacket
	{
		// Requests the server to warp the player into the cash shop
		public EnterCashShopPacket() : base((short)OutPacket.Opcode.ENTER_CASHSHOP)
		{
		}
	}

	// Opcode: MOVE_PLAYER(41)
	public class MovePlayerPacket : MovementPacket
	{
		// Updates the player's position with the server
		public MovePlayerPacket(Movement movement) : base(OutPacket.Opcode.MOVE_PLAYER)
		{
			skip(9);
			write_byte(1);
			writemovement(movement);
		}
	}

	// Opcode: PARTY_OPERATION(124)
	public class PartyOperationPacket : OutPacket
	{
		public enum Operation : sbyte
		{
			CREATE = 1,
			LEAVE = 2,
			JOIN = 3,
			INVITE = 4,
			EXPEL = 5,
			PASS_LEADER = 6
		}

		protected PartyOperationPacket(Operation op) : base((short)OutPacket.Opcode.PARTY_OPERATION)
		{
			write_byte((sbyte)op);
		}
	}

	// Creates a new party
	public class CreatePartyPacket : PartyOperationPacket
	{
		public CreatePartyPacket() : base(PartyOperationPacket.Operation.CREATE)
		{
		}
	}

	// Leaves a party
	public class LeavePartyPacket : PartyOperationPacket
	{
		public LeavePartyPacket() : base(PartyOperationPacket.Operation.LEAVE)
		{
		}
	}

	// Joins a party
	public class JoinPartyPacket : PartyOperationPacket
	{
		public JoinPartyPacket(int party_id) : base(PartyOperationPacket.Operation.JOIN)
		{
			write_int(party_id);
		}
	}

	// Invites a player to a party
	public class InviteToPartyPacket : PartyOperationPacket
	{
		public InviteToPartyPacket(string name) : base(PartyOperationPacket.Operation.INVITE)
		{
			write_string(name);
		}
	}

	// Expels someone from a party
	public class ExpelFromPartyPacket : PartyOperationPacket
	{
		public ExpelFromPartyPacket(int cid) : base(PartyOperationPacket.Operation.EXPEL)
		{
			write_int(cid);
		}
	}

	// Passes party leadership to another character
	public class ChangePartyLeaderPacket : PartyOperationPacket
	{
		public ChangePartyLeaderPacket(int cid) : base(PartyOperationPacket.Operation.PASS_LEADER)
		{
			write_int(cid);
		}
	}

	// Opcode: ADMIN_COMMAND(128)
	public class AdminCommandPacket : OutPacket
	{
		public enum Mode : sbyte
		{
			ENTER_MAP = 0x11
		}

		protected AdminCommandPacket(Mode mode) : base((short)OutPacket.Opcode.ADMIN_COMMAND)
		{
			write_byte((sbyte)mode);
		}
	}

	// Admin has entered the map
	public class AdminEnterMapPacket : AdminCommandPacket
	{
		public enum Operation : sbyte
		{
			SHOW_USERS,
			ALERT_ADMINS = 12
		}

		public AdminEnterMapPacket(Operation op) : base(AdminCommandPacket.Mode.ENTER_MAP)
		{
			write_byte((sbyte)op);
		}
	}

	// Opcode: MOVE_MONSTER(188)
	public class MoveMobPacket : MovementPacket
	{
		// Updates a mob's position with the server
		public MoveMobPacket(int oid, short type, sbyte skillb, sbyte skill0, sbyte skill1, sbyte skill2, sbyte skill3, sbyte skill4, Point<short> startpos, Movement movement) : base(OutPacket.Opcode.MOVE_MONSTER)
		{
			write_int(oid);
			write_short(type);
			write_byte(skillb);
			write_byte(skill0);
			write_byte(skill1);
			write_byte(skill2);
			write_byte(skill3);
			write_byte(skill4);

			skip(13);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: write_point(startpos);
			write_point(startpos);

			write_byte(1);
			writemovement(movement);
		}
	}

	// Opcode: PICKUP_ITEM(202)
	public class PickupItemPacket : OutPacket
	{
		// Requests picking up an item
		public PickupItemPacket(int oid, Point<short> position) : base((short)OutPacket.Opcode.PICKUP_ITEM)
		{
			write_int(0);
			write_byte(0);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: write_point(position);
			write_point(position);
			write_int(oid);
		}
	}

	// Opcode: DAMAGE_REACTOR(205)
	public class DamageReactorPacket : OutPacket
	{
		// Requests damaging a reactor
		public DamageReactorPacket(int oid, Point<short> position, short stance, int skillid) : base((short)OutPacket.Opcode.DAMAGE_REACTOR)
		{
			write_int(oid);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: write_point(position);
			write_point(position);
			write_short(stance);
			skip(4);
			write_int(skillid);
		}
	}

	// Opcode: PLAYER_MAP_TRANSFER(207)
	public class PlayerMapTransferPacket : OutPacket
	{
		// Requests the server to set map transition complete
		public PlayerMapTransferPacket() : base((short)OutPacket.Opcode.PLAYER_MAP_TRANSFER)
		{
		}
	}

	// Opcode: PLAYER_UPDATE(223)
	public class PlayerUpdatePacket : OutPacket
	{
		// Finished updating player stats
		public PlayerUpdatePacket() : base((short)OutPacket.Opcode.PLAYER_UPDATE)
		{
		}
	}
}