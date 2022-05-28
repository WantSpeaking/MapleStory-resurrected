using System;




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
						show_status (Color.Name.WHITE, "You have gained an item in the " + tab + " tab (" + name + ")");
					}
					else
					{
						show_status (Color.Name.WHITE, "You have gained items in the " + tab + " tab (" + name + " " + Convert.ToString (qty) + ")");
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
			/*sbyte type = recv.read_byte ();
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
				messenger.get ().show_status(Color.Name.WHITE, message);
			}*/
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

			if (string.CompareOrdinal (message.Substring (0, MAPLETIP.Length), "[MapleTip]") == 1)
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

			if (mode1 == 3)
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
			else if (mode1 == 13) // card effect
			{
				Stage.get ().get_player ().show_effect_id (CharEffect.Id.MONSTER_CARD);
			}
			else if (mode1 == 18) // intro effect
			{
				recv.read_string (); // path
			}
			else if (mode1 == 23) // info
			{
				recv.read_string (); // path
				recv.read_int (); // some int
			}
			else // Buff effect
			{
				int skillid = recv.read_int ();

				// More bytes, but we don't need them.
				Stage.get ().get_combat ().show_player_buff (skillid);
			}
		}
	}
}