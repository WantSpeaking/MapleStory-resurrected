using System;
using client;
using server.quest;

namespace ms
{
	// Show a status message
	// Opcode: SHOW_STATUS_INFO(39)
	public class ShowStatusInfoHandler : PacketHandler
	{
		// Modes:
		// 0 - Item(0) or Meso(1) 
		// 3 - Exp gain
		// 4 - Fame
		// 5 - Mesos
		// 6 - Guild points
		public override void handle (InPacket recv)
		{
			sbyte mode = recv.read_byte ();

			if (mode == 0)
			{
				sbyte mode2 = recv.read_byte ();

				if (mode2 == -1)
				{
					show_status (Color.Name.WHITE, "You can't get anymore items.");
				}
				else if (mode2 == 0)
				{
					int itemid = recv.read_int ();
					int qty = recv.read_int ();

					ItemData idata = ItemData.get (itemid);

					if (!idata.is_valid ())
					{
						return;
					}

					string name = idata.get_name ();

					if (name.Length > 21)
					{
						name.Substring (0, 21);
						name += "..";
					}

					InventoryType.Id type = InventoryType.by_item_id (itemid);

					string tab = "";

					switch (type)
					{
						case InventoryType.Id.EQUIP:
							tab = "Eqp";
							break;
						case InventoryType.Id.USE:
							tab = "Use";
							break;
						case InventoryType.Id.SETUP:
							tab = "Setup";
							break;
						case InventoryType.Id.ETC:
							tab = "Etc";
							break;
						case InventoryType.Id.CASH:
							tab = "Cash";
							break;
						default:
							tab = "UNKNOWN";
							break;
					}

					// TODO: show_status(Color::Name::WHITE, "You have lost items in the " + tab + " tab (" + name + " " + to_string(qty) + ")");

					if (qty < 0)
					{
						show_status (Color.Name.WHITE, "You have lost an item in the " + tab + " tab (" + name + ")");
					}
					else if (qty == 1)
					{
						show_status (Color.Name.WHITE, "You have gained an item in the " + tab + " tab (" + name + ")"+ " id:"+itemid);
					}
					else
					{
						show_status (Color.Name.WHITE, "You have gained items in the " + tab + " tab (" + name + " " + Convert.ToString (qty) + ")" + " id:" + itemid);
					}
				}
				else if (mode2 == 1)
				{
					recv.skip (1);

					int gain = recv.read_int ();
					string sign = (gain < 0) ? "-" : "+";

					show_status (Color.Name.WHITE, "You have gained mesos (" + sign + Convert.ToString (gain) + ")");
				}
				else
				{
					show_status (Color.Name.RED, "Mode: 0, Mode 2: " + Convert.ToString (mode2) + " is not handled.");
				}
			}
			else if (mode == 1)//updateQuest
			{
				var quest = Stage.get ().get_player ().get_quest ();

				short qid = recv.read_short ();

				var mapleQuest = MapleQuest.getInstance(qid);
				if (mapleQuest == null)
				{
					return;
				}

                MapleQuestStatus qs = MapleCharacter.Player.getQuest (qid);

				var status = recv.readByte ();
				if (status == 0)
				{
					qs.status = MapleQuestStatus.Status.NOT_STARTED;
				}
				else if(status == 1)
				{
					var progressData = recv.read_string ();
					//AppDebug.Log ($"questId:{questId}\t statusId:{statusId}\t ProgressData:{ProgressData}");
					qs.status = MapleQuestStatus.Status.STARTED;
					qs.setProgress (qid, progressData);
				}
				else if (status == 2)
				{
					var progressData = recv.read_long ();
					//AppDebug.Log ($"questId:{questId}\t statusId:{statusId}\t ProgressData:{ProgressData}");
					qs.status = MapleQuestStatus.Status.COMPLETED;
					qs.CompletionTime = long.Parse (progressData.ToString ());
				}

				ms.Stage.get ().UpdateQuest ();
			}
			else if (mode == 3)
			{
				bool white = recv.read_bool ();
				int gain = recv.read_int ();
				bool inchat = recv.read_bool ();
				int bonus1 = recv.read_int ();

				recv.read_short ();
				recv.read_int (); // bonus 2
				recv.read_bool (); // 'event or party'
				recv.read_int (); // bonus 3
				recv.read_int (); // bonus 4
				recv.read_int (); // bonus 5

				string message = "You have gained experience (+" + Convert.ToString (gain) + ")";

				if (inchat)
				{
					show_status (Color.Name.RED, "Mode: 3, inchat is not handled.");
				}
				else
				{
					show_status (white ? Color.Name.WHITE : Color.Name.YELLOW, message);

					if (bonus1 > 0)
					{
						show_status (Color.Name.YELLOW, "+ Bonus EXP (+" + Convert.ToString (bonus1) + ")");
					}
				}
			}
			else if (mode == 4)
			{
				int gain = recv.read_int ();
				string sign = (gain < 0) ? "-" : "+";

				// TODO: Lose fame?
				show_status (Color.Name.WHITE, "You have gained fame. (" + sign + Convert.ToString (gain) + ")");
			}
			else
			{
				show_status (Color.Name.RED, "Mode: " + Convert.ToString (mode) + " is not handled.");
			}
		}

		private void show_status (Color.Name color, string message)
		{
			var messenger = UI.get ().get_element<UIStatusMessenger> ();
			if (messenger)
			{
				messenger.get ().show_status(color, message);
			}
		}
	}

	// Show a server message
	// Opcode: SERVER_MESSAGE(68)
	public class ServerMessageHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			//todo 2 recv.inspect_bool ();
			sbyte type = recv.read_byte ();
			bool servermessage = recv.inspect_bool ();

			if (servermessage)
			{
				recv.skip (1);
			}

			string message = recv.read_string ();

			if (type == 3)
			{
				recv.read_byte (); // channel
				recv.read_bool (); // megaphone
			}
			else if (type == 4)
			{
				UI.get ().set_scrollnotice (message);
			}
			else if (type == 7)
			{
				recv.read_int (); // npcid
			}

			var messenger = UI.get ().get_element<UIStatusMessenger> ();
			if (messenger)
			{
				messenger.get ().show_status (Color.Name.WHITE, message);
			}
		}
	}

	// Show another type of server message
	// Opcode: WEEK_EVENT_MESSAGE(77)
	public class WeekEventMessageHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			recv.read_byte (); // TODO: Always 0xFF, Check this!

			string message = recv.read_string ();

			const string MAPLETIP = "[MapleTip]";

			//if (string.CompareOrdinal (message.Substring (0, MAPLETIP.Length), "[MapleTip]") == 1)
			{
				message = "[Notice] " + message;
			}

			UI.get ().get_element<UIChatBar> ().get ().send_chatline (message, UIChatBar.LineType.YELLOW);
		}
	}

	// Show a chat message
	// CHAT_RECEIVED(162)
	public class ChatReceivedHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			int charid = recv.read_int ();

			recv.read_bool (); // 'gm'

			string message = recv.read_string ();
			sbyte type = recv.read_byte ();

			var character = Stage.get ().get_character (charid);
			if (character)
			{
				//message = character.get ().get_name() + ": " + message;
				character.get ().speak(message);
			}

			var linetype = (UIChatBar.LineType)type;

			var chatbar = UI.get ().get_element<UIChatBar> ();
			if (chatbar)
			{
				chatbar.get ().send_chatline(message, linetype);
			}
		}
	}

	// Shows the effect of a scroll
	// Opcode: SCROLL_RESULT(167)
	public class ScrollResultHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			int cid = recv.read_int ();
			bool success = recv.read_bool ();
			bool destroyed = recv.read_bool ();

			recv.read_short (); // Legendary spirit if 1

			CharEffect.Id effect;
			Messages.Type message;

			if (success)
			{
				effect = CharEffect.Id.SCROLL_SUCCESS;
				message = Messages.Type.SCROLL_SUCCESS;
			}
			else
			{
				effect = CharEffect.Id.SCROLL_FAILURE;

				if (destroyed)
				{
					message = Messages.Type.SCROLL_DESTROYED;
				}
				else
				{
					message = Messages.Type.SCROLL_FAILURE;
				}
			}

			/*todo Stage.get ().show_character_effect (cid, effect);

			if (Stage.get ().is_player (cid))
			{
				if (var chatbar = UI.get ().get_element<UIChatBar> ())
				{
					chatbar.display_message (message, UIChatBar.LineType.RED);
				}

				UI.get ().enable ();
			}*/
		}
	}

	// Can contain numerous different effects and messages
	// Opcode: SHOW_ITEM_GAIN_INCHAT(206)
	public class ShowItemGainInChatHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			sbyte mode1 = recv.read_byte ();
			AppDebug.Log ($"ShowItemGainInChatHandler:{mode1}");
			if (mode1 == 0)//showSpecialEffect Levelup
			{
				AppDebug.Log ($"todo showSpecialEffect Levelup");

			}
			else if (mode1 == 1)//showOwnBerserk
			{
				AppDebug.Log ($"todo showOwnBerserk");
			}
			else if (mode1 == 3)
			{
				sbyte mode2 = recv.read_byte ();

				if (mode2 == 1) // This is actually 'item gain in chat'
				{
					int itemid = recv.read_int ();
					int qty = recv.read_int ();

					ItemData idata = ItemData.get (itemid);

					if (!idata.is_valid ())
					{
						return;
					}

					string name = idata.get_name ();
					string sign = (qty < 0) ? "-" : "+";
					string message = "Gained an item: " + name + " (" + sign + Convert.ToString (qty) + ")";
					/*todo var chatbar = UI.get ().get_element<UIChatBar> ();
					if (chatbar)
					{
						chatbar.send_chatline (message, UIChatBar.LineType.BLUE);
					}*/
				}
			}
			else if (mode1 == 4)//showOwnPetLevelUp
			{
				AppDebug.Log ($"todo showSpecialEffect showOwnPetLevelUp");


			}
			else if (mode1 == 6)//showSpecialEffect Exp did not drop (Safety Charms) 
			{
				AppDebug.Log ($"todo showSpecialEffect Exp did not drop");

			}
			else if (mode1 == 7)//showSpecialEffect Enter portal sound
			{
				AppDebug.Log ($"todo showSpecialEffect Enter portal sound");

			}
			else if (mode1 == 8)//showSpecialEffect Job change
			{
				AppDebug.Log ($"todo showSpecialEffect Job change");

			}
			else if (mode1 == 9)//showSpecialEffect Quest complete
			{
				AppDebug.Log ($"todo showSpecialEffect Quest complete");

			}
			else if (mode1 == 10) //showSpecialEffect showOwnRecovery Recovery
			{
				recv.read_byte ();//some byte
				AppDebug.Log ($"todo showSpecialEffect Recovery");
			}
			else if (mode1 == 11)//showSpecialEffect Buff effect 
			{
				AppDebug.Log ($"todo showSpecialEffect Buff effect ");

			}
			else if (mode1 == 12)//showIntro
			{
				AppDebug.Log ($"todo ShowItemGainInChatHandler showIntro");

			}
			else if (mode1 == 13) // card effect
			{
				Stage.get ().get_player ().show_effect_id (CharEffect.Id.MONSTER_CARD);
			}
			else if (mode1 == 14)//showSpecialEffect Monster book pickup
			{
				AppDebug.Log ($"todo showSpecialEffect Monster book pickup");

			}
			else if (mode1 == 15)//showSpecialEffect Equipment levelup
			{
				AppDebug.Log ($"todo showSpecialEffect Equipment levelup");

			}
			else if (mode1 == 16)//showSpecialEffect Maker Skill Success
			{
				AppDebug.Log ($"todo showSpecialEffect Maker Skill Success");

			}
			else if (mode1 == 17)//showSpecialEffect Buff effect w/ sfx
			{
				AppDebug.Log ($"todo showSpecialEffect Buff effect w/ sfx");

			}
			else if (mode1 == 18) // intro effect
			{
				recv.read_string (); // path
			}
			else if (mode1 == 19)//showSpecialEffect Exp card [500, 200, 50]
			{
				AppDebug.Log ($"todo showSpecialEffect Exp card [500, 200, 50]");

			}
			else if (mode1 == 21)//showSpecialEffect showWheelsLeft ,Wheel of destiny
			{
				AppDebug.Log ($"todo showSpecialEffect Wheel of destiny");

			}
			else if (mode1 == 23) // info
			{
				recv.read_string (); // path
				recv.read_int (); // some int
			}
			else if (mode1 == 26)//showSpecialEffect Spirit Stone
			{
				AppDebug.Log ($"todo showSpecialEffect Spirit Stone");

			}
			else // Buff effect
			{
				int skillid = recv.read_int ();

				// More bytes, but we don't need them.
				Stage.get ().get_combat ().show_player_buff (skillid);
			}
		}
	}
	
	// Can contain numerous different effects and messages
	// Opcode: SHOW_ITEM_GAIN_INCHAT(206)
	public class DOJO_WARP_UPHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			int a = recv.read_byte ();
			int b = recv.read_byte ();

			{
				Point_short spawnpoint = Stage.get ().get_portals ().get_portalPos_by_id (8);
				Point_short startpos = Stage.get ().get_Physics ().get_y_below (spawnpoint);
				Stage.get ().get_player ().respawn (new Point_short (startpos), false);
			}
		}
	}
}