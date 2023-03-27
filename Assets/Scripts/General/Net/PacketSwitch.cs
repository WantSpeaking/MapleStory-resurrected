using System;
using System.Collections.Generic;
using ms.Helper;

namespace ms
{
	// Class which contains the array of handler classes to use
	public class PacketSwitch
	{
		//private HelloHandler helloHandler = new HelloHandler();
		// Register all handlers
		public PacketSwitch ()
		{
			// Common handlers
			emplace (Opcode.PING, new PingHandler ());

			// Login handlers
			emplace (Opcode.LOGIN_RESULT, new LoginResultHandler ());
			emplace (Opcode.SERVERSTATUS, new ServerStatusHandler ());
			emplace (Opcode.SERVERLIST, new ServerlistHandler ());
			emplace (Opcode.CHARLIST, new CharlistHandler ());
			emplace (Opcode.SERVER_IP, new ServerIPHandler ());
			emplace (Opcode.CHARNAME_RESPONSE, new CharnameResponseHandler ());
			emplace (Opcode.ADD_NEWCHAR_ENTRY, new AddNewCharEntryHandler ());
			emplace (Opcode.DELCHAR_RESPONSE, new DeleteCharResponseHandler ());
			emplace (Opcode.RECOMMENDED_WORLDS, new RecommendedWorldsHandler ());

			// SetField handlers
			emplace (Opcode.SET_FIELD, new SetFieldHandler ());

			// MapObject handlers
			emplace (Opcode.SPAWN_CHAR, new SpawnCharHandler ());
			emplace (Opcode.CHAR_MOVED, new CharMovedHandler ());
			emplace (Opcode.UPDATE_CHARLOOK, new UpdateCharLookHandler ());
			emplace (Opcode.SHOW_FOREIGN_EFFECT, new ShowForeignEffectHandler ());
			emplace (Opcode.REMOVE_CHAR, new RemoveCharHandler ());
			emplace (Opcode.SPAWN_PET, new SpawnPetHandler ());
			emplace (Opcode.SPAWN_NPC, new SpawnNpcHandler ());
			emplace (Opcode.SPAWN_NPC_C, new SpawnNpcControllerHandler ());
			emplace (Opcode.SPAWN_MOB, new SpawnMobHandler ());
			emplace (Opcode.SPAWN_MOB_C, new SpawnMobControllerHandler ());
			emplace (Opcode.MOB_MOVED, new MobMovedHandler ());
			emplace (Opcode.SHOW_MOB_HP, new ShowMobHpHandler ());
			emplace (Opcode.KILL_MOB, new KillMobHandler ());
			emplace (Opcode.DROP_LOOT, new DropLootHandler ());
			emplace (Opcode.REMOVE_LOOT, new RemoveLootHandler ());
			emplace (Opcode.HIT_REACTOR, new HitReactorHandler ());
			emplace (Opcode.SPAWN_REACTOR, new SpawnReactorHandler ());
			emplace (Opcode.REMOVE_REACTOR, new RemoveReactorHandler ());

			// Attack handlers
			emplace (Opcode.ATTACKED_CLOSE, new CloseAttackHandler ());
			emplace (Opcode.ATTACKED_RANGED, new RangedAttackHandler ());
			emplace (Opcode.ATTACKED_MAGIC, new MagicAttackHandler ());

			// Skill handlers
			emplace (Opcode.SKILL_EFFECT, new SKILL_EFFECT_Handler ());
			emplace (Opcode.CANCEL_SKILL_EFFECT, new CANCEL_SKILL_EFFECT_Handler ());
			
			//Char handleds
			emplace (Opcode.PARTY_OPERATION, new PartyOperationHandlers ());
			emplace (Opcode.UPDATE_PARTYMEMBER_HP, new UPDATE_PARTYMEMBER_HPHandlers ());
			
			// Player handlers
			emplace (Opcode.CHANGE_CHANNEL, new ChangeChannelHandler ());
			emplace (Opcode.KEYMAP, new KeymapHandler ());
			emplace (Opcode.SKILL_MACROS, new SkillMacrosHandler ());
			emplace (Opcode.CHANGE_STATS, new ChangeStatsHandler ());
			emplace (Opcode.GIVE_BUFF, new ApplyBuffHandler ());
			emplace (Opcode.CANCEL_BUFF, new CancelBuffHandler ());
			emplace (Opcode.FORCED_STAT_RESET, new RecalculateStatsHandler ());
			emplace (Opcode.UPDATE_SKILL, new UpdateSkillHandler ());
			emplace (Opcode.ADD_COOLDOWN, new AddCooldownHandler ());

			// Messaging handlers
			emplace (Opcode.SHOW_STATUS_INFO, new ShowStatusInfoHandler ());
			emplace (Opcode.CHAT_RECEIVED, new ChatReceivedHandler ());
			emplace (Opcode.SCROLL_RESULT, new ScrollResultHandler ());
			emplace (Opcode.SERVER_MESSAGE, new ServerMessageHandler ());
			emplace (Opcode.WEEK_EVENT_MESSAGE, new WeekEventMessageHandler ());
			emplace (Opcode.SHOW_ITEM_GAIN_INCHAT, new ShowItemGainInChatHandler ());

			// Inventory Handlers
			emplace (Opcode.MODIFY_INVENTORY, new ModifyInventoryHandler ());
			emplace (Opcode.GATHER_RESULT, new GatherResultHandler ());
			emplace (Opcode.SORT_RESULT, new SortResultHandler ());

			// NPC Interaction Handlers
			emplace (Opcode.NPC_TALK, new NpcTalkHandler ());
			emplace (Opcode.OPEN_NPC_SHOP, new OpenNpcShopHandler ());

			// Player Interaction
			emplace (Opcode.CHAR_INFO, new CharInfoHandler ());

			// Cash Shop
			emplace (Opcode.SET_CASH_SHOP, new SetCashShopHandler ());

			// TODO: New handlers, they need coded and moved to a proper file.
			emplace (Opcode.CHECK_SPW_RESULT, new CheckSpwResultHandler ());
			emplace (Opcode.FIELD_EFFECT, new FieldEffectHandler ());

			//Quest
			emplace (Opcode.UPDATE_QUEST_INFO, new UpdateQuestInfoHandler ());

		}

		// Forward a packet to the correct handler
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: void forward(const sbyte* bytes, uint length) const
		public void forward (byte[] bytes, int length)
		{
			//AppDebug.Log ($"forward bytes: {bytes.Length}");
			// Wrap the bytes with a parser
			var sByteArray = bytes.ToSbyteArray ();
			InPacket recv = new InPacket (bytes, length);

			// Read the opcode to determine handler responsible
			var opcode = recv.read_short ();

			if (Configuration.get ().get_show_packets ())
			{
				/*		if (opcode == (int)Opcode.PING)
						{
							Console.Write ("Received Packet: PING");
							Console.Write ("\n");
						}
						else
						{
							Console.Write ("Received Packet: ");
							Console.Write (Convert.ToString (opcode));
							Console.Write ("\n");
						}*/

				if (GameUtil.Get ().enableDebugPacket && opcode != 198 && opcode != 239 && opcode != 240)
				//if (GameUtil.get ().enableDebugPacket && (Opcode)opcode != Opcode.SHOW_ITEM_GAIN_INCHAT && opcode != 198 && opcode != 239 && opcode != 240)
				{
					//AppDebug.Log ($"\t Received Packet: {(Opcode)opcode} = {opcode} \t PacketSize:{bytes.Length} \t {bytes.ToDebugLog ()}");
					AppDebug.Log ($"\t Received Packet: {(Opcode)opcode} = {opcode} \t PacketSize:{bytes.Length}");
                }
			}

			if (opcode < NUM_HANDLERS)
			{
				ms.PacketHandler handler;
		/*		if (opcode == 14 && recv.length()<=23)
				{
					handler = helloHandler;
                }
				else*/
				{
                     handler = handlers.TryGetValue((Opcode)opcode);
                }

                if (handler != null)
				{
					// Handler is good, packet is passed on

					try
					{
						handler.handle (recv);
					}
					catch (PacketError err)
					{
						// Notice about an error
						warn (err.Message, (uint)opcode);
					}
				}
				else
				{
					if (GameUtil.Get ().enableDebugPacket && opcode != 239 && opcode != 240)
						// Warn about an unhandled packet
						warn (MSG_UNHANDLED, (uint)opcode);
				}
			}
			else
			{
				if (GameUtil.Get ().enableDebugPacket && opcode != 239 && opcode != 240)
					// Warn about a packet with opcode out of bounds
					warn (MSG_OUTOFBOUNDS, (uint)opcode);
			}
		}

		// Print a warning
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void warn(const string& message, uint opcode) const
		private void warn (string message, uint opcode)
		{
			Console.Write ("Opcode [");
			Console.Write (opcode);
			Console.Write ("] Error: ");
			Console.Write (message);
			Console.Write ("\n");
		}

		// Opcodes for which handlers can be registered
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The implementation of the following type could not be found:
//		enum Opcode : ushort;

		// Message when an unhandled packet is received
		private const string MSG_UNHANDLED = "Unhandled packet detected";

		// Message when a packet with a larger opcode than the array size is received
		private const string MSG_OUTOFBOUNDS = "Large opcode detected";

		// Message when a packet with a larger opcode than the array size is received
		private const string MSG_REREGISTER = "Handler was registered twice";

		// Maximum number of handlers needed
		private const uint NUM_HANDLERS = 500;

		private Dictionary<Opcode, PacketHandler> handlers = new Dictionary<Opcode, PacketHandler> ((int)NUM_HANDLERS);
		//private PacketHandler[] handlers = new PacketHandler[NUM_HANDLERS];

		// Register a handler for the specified opcode
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C++ template specifiers with non-type parameters cannot be converted to C#:
//ORIGINAL LINE: template (uint O, typename T, typename...Args>
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no equivalent in C# to C++11 variadic templates:
		private void emplace (Opcode opcode, PacketHandler packetHandler)
		{
/*//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no equivalent in C# to 'static_assert':
//			static_assert(O ( NUM_HANDLERS, "PacketSwitch::emplace - Opcode out of array bounds");
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no equivalent in C# to 'static_assert':
//			static_assert(is_base_of(PacketHandler, T>::value, "Error: Packet handlers must derive from PacketHandler");

			if (handlers[O] != null)
			{
				warn(MSG_REREGISTER, O);
			}

			handlers[O] = (T>(forward(Args>(args)...);*/
			handlers.TryAdd (opcode, packetHandler);
		}
	}
}


namespace ms
{
	// Opcodes for InPackets
	public enum Opcode : ushort
	{
		/// Login 1
		LOGIN_RESULT = 0,
		SERVERSTATUS = 3,
		SERVERLIST = 10,
		CHARLIST = 11,
		SERVER_IP = 12,
		CHARNAME_RESPONSE = 13,
		ADD_NEWCHAR_ENTRY = 14,
		DELCHAR_RESPONSE = 15,
		PING = 17,

		/// Login 2
		RECOMMENDED_WORLDS = 27,
		CHECK_SPW_RESULT = 28,

		/// Inventory 1
		MODIFY_INVENTORY = 29,

		/// Player 2
		CHANGE_CHANNEL = 16,
		CHANGE_STATS = 31,
		GIVE_BUFF = 32,
		CANCEL_BUFF = 33,
		/// <summary>
		/// RECALCULATE_STATS
		/// </summary>
		FORCED_STAT_RESET = 35,
		UPDATE_SKILL = 36,

		CLAIM_STATUS_CHANGED = 47,

		/// Messaging 1
		SHOW_STATUS_INFO = 39,

		QUEST_CLEAR = 49,

		/// Inventory 2
		GATHER_RESULT = 52,
		SORT_RESULT = 53,

		/// Char
		PARTY_OPERATION = 62,

		/// Player 3
		/// Messaging 2
		SERVER_MESSAGE = 68,
		WEEK_EVENT_MESSAGE = 77,

		SKILL_MACROS = 124,
		SET_FIELD = 125,
		FIELD_EFFECT = 138,

		SET_QUEST_CLEAR = 150,
		SET_QUEST_TIME = 151,

		/// MapObject
		SPAWN_CHAR = 160,
		REMOVE_CHAR = 161,

		/// Messaging
		CHAT_RECEIVED = 162,
		SCROLL_RESULT = 167,

		/// MapObject
		SPAWN_PET = 168,
		CHAR_MOVED = 185,

		/// Attack
		ATTACKED_CLOSE = 186,
		ATTACKED_RANGED = 187,
		ATTACKED_MAGIC = 188,

		SKILL_EFFECT = 190,
		CANCEL_SKILL_EFFECT = 191,
		DAMAGE_PLAYER = 192,

		UPDATE_CHARLOOK = 197,
		SHOW_FOREIGN_EFFECT = 198,
		UPDATE_PARTYMEMBER_HP = 201,
		SHOW_ITEM_GAIN_INCHAT = 206, // TODO: Rename this (Terribly named)

		UPDATE_QUEST_INFO = 211,

		/// Player
		ADD_COOLDOWN = 234,

		/// MapObject
		SPAWN_MOB = 236,
		KILL_MOB = 237,
		SPAWN_MOB_C = 238,
		MOB_MOVED = 239,
		DAMAGE_MONSTER = 246,
		SHOW_MOB_HP = 250,
		SPAWN_NPC = 257,
		SPAWN_NPC_C = 259,
		DROP_LOOT = 268,
		REMOVE_LOOT = 269,
		HIT_REACTOR = 277,
		SPAWN_REACTOR = 279,
		REMOVE_REACTOR = 280,

		/// NPC Interaction
		NPC_TALK = 304,
		OPEN_NPC_SHOP = 305,
		CONFIRM_SHOP_TRANSACTION = 306,
		KEYMAP = 335,

		/// Player Interaction
		CHAR_INFO = 61,

		/// Cash Shop
		SET_CASH_SHOP = 127
	}
}