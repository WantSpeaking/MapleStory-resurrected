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
	// Packet which requests a dialog with a server-sided NPC
	// Opcode: TALK_TO_NPC(58)
	public class TalkToNPCPacket : OutPacket
	{
		public TalkToNPCPacket(int oid) : base((short)OutPacket.Opcode.TALK_TO_NPC)
		{
			write_int(oid);
		}
		public TalkToNPCPacket(int oid,string npcFileName) : base((short)OutPacket.Opcode.TALK_TO_NPC)
		{
			write_int(oid);
			write_string (npcFileName);
		}
	}

	// Packet which sends a response to an NPC dialog to the server
	// Opcode: NPC_TALK_MORE(60)
	public class NpcTalkMorePacket : OutPacket
	{
		public NpcTalkMorePacket(sbyte lastmsg, sbyte response) : base((short)OutPacket.Opcode.NPC_TALK_MORE)
		{
			write_byte(lastmsg);
			write_byte(response);
		}

		public NpcTalkMorePacket(string response) : this(2, 1)
		{
			write_string(response);
		}

		public NpcTalkMorePacket(int selection) : this(4, 1)
		{
			write_int(selection);
		}
	}

	// Packet which tells the server of an interaction with an NPC shop
	// Opcode: NPC_SHOP_ACTION(61)
	public class NpcShopActionPacket : OutPacket
	{
		// Requests that an item should be bought from or sold to a NPC shop
		public NpcShopActionPacket(short slot, int itemid, short qty, bool buy) : this(buy ? Mode.BUY : Mode.SELL)
		{
			write_short(slot);
			write_int(itemid);
			write_short(qty);
		}

		// Requests that an item should be recharged at a NPC shop
		public NpcShopActionPacket(short slot) : this(Mode.RECHARGE)
		{
			write_short(slot);
		}

		// Requests exiting from a NPC shop
		public NpcShopActionPacket() : this(Mode.LEAVE)
		{
		}

		protected enum Mode : sbyte
		{
			BUY,
			SELL,
			RECHARGE,
			LEAVE
		}

		protected NpcShopActionPacket(Mode mode) : base((short)OutPacket.Opcode.NPC_SHOP_ACTION)
		{
			write_byte((sbyte)mode);
		}
	}
}