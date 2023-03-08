using System;
using System.Collections.Generic;





namespace ms
{
	// A packet to be sent to the server
	// Used as a base class to create specific packets
	public class OutPacket
	{
		// Construct a packet by writing its opcode
		public OutPacket (short opc)
		{
			this.opcode = opc;
			write_short (opcode);
		}

		public void dispatch ()
		{
			Session.get ().write (bytes.ToArray (), (int)bytes.Count);

			if (Configuration.get ().get_show_packets ())
			{
				if (opcode == (int)Opcode.PONG)
				{
					AppDebug.Log ("Sent Packet: PONG");
					Console.Write ("\n");
				}
				else
				{
					/*Console.Write ("Sent Packet: ");
					Console.Write (Convert.ToString (opcode));
					Console.Write ("\n");*/

                    if (GameUtil.Get ().enableDebugPacket && (Opcode)opcode != Opcode.MOVE_PLAYER && (Opcode)opcode != Opcode.MOVE_MONSTER)
                    {
                        AppDebug.Log($"Sent Packet: {(Opcode)opcode} = dec:{opcode} hex:{opcode:X} \t PacketSize:{bytes.Count} \t {bytes.ToDebugLog()}");
                    }
                }
            }
		}

		// Opcodes for OutPackets associated with version 83 of the game
		public enum Opcode : ushort
		{
			/// Login
			LOGIN = 1,
			CHARLIST_REQUEST = 5,
			SERVERSTATUS_REQUEST = 6,
			ACCEPT_TOS = 7,
			SET_GENDER = 8,
			SERVERLIST_REQUEST = 11,
			SELECT_CHAR = 19,
			PLAYER_LOGIN = 20,
			NAME_CHAR = 21,
			CREATE_CHAR = 22,
			DELETE_CHAR = 23,
			PONG = 24,
			REGISTER_PIC = 29,
			SELECT_CHAR_PIC = 30,
			LOGIN_START = 35, // Custom name

			/// Gameplay 1
			CHANGEMAP = 38,
			ENTER_CASHSHOP = 40,
			MOVE_PLAYER = 41,
			Close_Range_Attack = 44,
			RANGED_ATTACK = 45,
			MAGIC_ATTACK = 46,
			TAKE_DAMAGE = 48,

			/// Messaging
			GENERAL_CHAT = 49,

			/// NPC Interaction
			TALK_TO_NPC = 58,
			NPC_TALK_MORE = 60,
			NPC_SHOP_ACTION = 61,

			MESO_DROP = 94,

			/// Player Interaction
			CHAR_INFO_REQUEST = 97,
			CHANGE_MAP_SPECIAL = 100,

			/// Inventory
			GATHER_ITEMS = 69,
			SORT_ITEMS = 70,
			MOVE_ITEM = 71,
			USE_ITEM = 72,
			SCROLL_EQUIP = 86,

			/// Player
			SPEND_AP = 87,
			SPEND_SP = 90,
			CHANGE_KEYMAP = 135,

			/// Skill
			Special_Move = 91,//buff
			Cancel_Buff = 92,
			Skill_Effect = 93,

			//Quest
			QUEST_ACTION = 107,

			/// Gameplay 2
			PARTY_OPERATION = 124,
			DENY_PARTY_REQUEST = 125,
			ADMIN_COMMAND = 128,
			MOVE_MONSTER = 188,
			PICKUP_ITEM = 202,
			DAMAGE_REACTOR = 205,
			PLAYER_MAP_TRANSFER = 207,
			PLAYER_UPDATE = 223
		}

		// Skip a number of bytes (filled with zeros)
		protected void skip (uint count)
		{
			for (uint i = 0; i < count; i++)
			{
				bytes.Add (0);
			}
		}

		// Write a byte
		protected void write_byte (sbyte ch)
		{
			bytes.Add ((sbyte)ch);
		}

		// Write a short
		protected void write_short (short sh)
		{
			for (uint i = 0; i < sizeof (short); i++)
			{
				write_byte ((sbyte)sh);
				sh >>= 8;
			}
		}

		// Write an int
		protected void write_int (int value)
		{
			for (uint i = 0; i < sizeof (int); i++)
			{
				write_byte ((sbyte)value);
				value >>= 8;
			}
		}

		// Write a long
		protected void write_long (long lg)
		{
			for (uint i = 0; i < sizeof (int); i++)
			{
				write_byte ((sbyte)lg);
				lg >>= 8;
			}
		}

		// Write a point
		// One short for x and one for y
		protected void write_point (Point_short position)
		{
			write_short (position.x ());
			write_short (position.y ());
		}

		// Write a timestamp as an integer
		protected void write_time ()
		{
			/*var duration = chrono.steady_clock.now ().time_since_epoch ();
			var since_epoch = chrono.duration_cast<chrono.milliseconds> (duration);
			int timestamp = (int)since_epoch.count ();*/

			write_int ((int)((DateTime.Now.ToUniversalTime ().Ticks - 621355968000000000) / 10000));
		}

		// Write a string
		// Writes the length as a short and then each individual character as a byte
		protected void write_string (string str)
		{
			if (string.IsNullOrEmpty (str))
				str = string.Empty;

			short length = (short)str.Length;

			write_short (length);

			for (short i = 0; i < length; i++)
			{
				write_byte ((sbyte)str[i]);
			}
		}

		// Write a random int
		protected void write_random ()
		{
			var randomizer = new Randomizer ();
			int rand = randomizer.next_int (Int32.MaxValue);

			write_int (rand);
		}

		// Write the MACS and then write the HWID of the computer
		protected void write_hardware_info ()
		{
			string macs = Configuration.get ().get_macs ();
			string hwid = Configuration.get ().get_hwid ();
			/*string macs = "00-FF-27-AC-9C-D6";
			string hwid = "2EFDB98799DD_CB4F4F88";*/

			write_string (macs);
			write_string (hwid);
		}

		protected void write_Bytes (sbyte[] sbytes)
		{
			bytes.AddRange (sbytes);
		}

		public void write_PLyerPos()
		{
			var playerPos = ms.Stage.get ().get_player ().get_position ();
			write_short (playerPos.x ());
			write_short (playerPos.y ());
		}
		// Function to convert hexadecimal to decimal
		protected int hex_to_dec (string hexVal)
		{
			int len = Convert.ToString (hexVal).Length;
			int @base = 1;
			int dec_val = 0;

			for (int i = len - 1; i >= 0; i--)
			{
				if (hexVal[i] >= '0' && hexVal[i] <= '9')
				{
					dec_val += (hexVal[i] - 48) * @base;
					@base = @base * 16;
				}
				else if (hexVal[i] >= 'A' && hexVal[i] <= 'F')
				{
					dec_val += (hexVal[i] - 55) * @base;
					@base = @base * 16;
				}
			}

			return dec_val;
		}

		private List<sbyte> bytes = new List<sbyte> ();
		private short opcode;
	}
}