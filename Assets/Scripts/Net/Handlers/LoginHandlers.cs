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
	// Handler for a packet that contains the response to an attempt at logging in
	public class LoginResultHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle (InPacket recv)
		{
			var loginwait = UI.get ().get_element<UILoginWait> ();

			if (loginwait != null && loginwait.Dereference ().is_active ())
			{
				// Remove previous UIs
				UI.get ().remove (UIElement.Type.LOGINNOTICE);
				UI.get ().remove (UIElement.Type.LOGINWAIT);
				UI.get ().remove (UIElement.Type.TOS);
				UI.get ().remove (UIElement.Type.GENDER);

				System.Action okhandler = loginwait.Dereference ().get_handler ();

				// The packet should contain a 'reason' integer which can signify various things
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
				int reason = recv.read_int ();
				if (reason != 0)
				{
					// Login unsuccessful
					// The LoginNotice displayed will contain the specific information
					switch (reason)
					{
						case 2:
							UI.get ().emplace<UILoginNotice> (UILoginNotice.Message.BLOCKED_ID, okhandler);
							break;
						case 5:
							UI.get ().emplace<UILoginNotice> (UILoginNotice.Message.NOT_REGISTERED, okhandler);
							break;
						case 7:
							UI.get ().emplace<UILoginNotice> (UILoginNotice.Message.ALREADY_LOGGED_IN, okhandler);
							break;
						case 13:
							UI.get ().emplace<UILoginNotice> (UILoginNotice.Message.UNABLE_TO_LOGIN_WITH_IP, okhandler);
							break;
						case 23:
							UI.get ().emplace<UITermsOfService> (okhandler);
							break;
						default:
							// Other reasons
							if (reason > 0)
							{
								var reasonbyte = (sbyte)(reason - 1);

								UI.get ().emplace<UILoginNotice> (reasonbyte, okhandler);
							}

							break;
					}
				}
				else
				{
					// Login successful
					// The packet contains information on the account, so we initialize the account with it.
					Account account = LoginParser.parse_account (recv);

					Configuration.get ().set_admin (account.admin);

					if (account.female == 10)
					{
						UI.get ().emplace<UIGender> (okhandler);
					}
					else
					{
						// Save the "Login ID" if the box for it on the login screen is checked
						if (Setting<SaveLogin>.get ().load ())
						{
							Setting<DefaultAccount>.get ().save (account.name);
						}

						// Request the list of worlds and channels online
						new ServerRequestPacket ().dispatch ();
					}
				}
			}
		}
	}

	// Handler for a packet that contains the status of the requested world
	public class ServerStatusHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle (InPacket recv)
		{
			// Possible values for status:
			// 0 - Normal
			// 1 - Highly populated
			// 2 - Full
			recv.read_short (); // status

			// TODO: I believe it shows a warning message if it's 1 and blocks enter into the world if it's 2. Need to find those messages.
		}
	}

	// Handles the packet that contains information on worlds and channels
	public class ServerlistHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle (InPacket recv)
		{
			var worldselect = UI.get ().get_element<UIWorldSelect> ();

			if (worldselect == null)
			{
				worldselect = UI.get ().emplace<UIWorldSelect> ();
			}

			// Parse all worlds
			while (recv.available ())
			{
				World world = LoginParser.parse_world (recv);

				if (world.wid != -1)
				{
					worldselect.Dereference ().add_world (world);
				}
				else
				{
					// Remove previous UIs
					UI.get ().remove (UIElement.Type.LOGIN);

					// Add the world selection screen to the UI
					worldselect.Dereference ().draw_world ();

					// End of packet
					return;
				}
			}
		}
	}

	// Handler for a packet that contains information on all chars on this world
	public class CharlistHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle (InPacket recv)
		{
			var loginwait = UI.get ().get_element<UILoginWait> ();

			if (loginwait != null && loginwait.Dereference ().is_active ())
			{
				byte channel_id = (byte)recv.read_byte ();

				// Parse all characters
				List<CharEntry> characters = new List<CharEntry> ();
				sbyte charcount = (sbyte)recv.read_byte ();

				for (byte i = 0; i < charcount; ++i)
				{
					characters.Add (LoginParser.parse_charentry (recv));
				}

				sbyte pic = (sbyte)recv.read_byte ();
				int slots = recv.read_int ();

				// Remove previous UIs
				UI.get ().remove (UIElement.Type.LOGINNOTICE);
				UI.get ().remove (UIElement.Type.LOGINWAIT);

				// Remove the world selection screen
				var worldselect = UI.get ().get_element<UIWorldSelect> ();
				if (worldselect != null)
				{
					worldselect.get ().remove_selected ();
				}

				// Add the character selection screen
				UI.get ().emplace<UICharSelect> (characters, charcount, slots, pic);
			}
		}
	}

	// Handles the packet which contains the IP of a channel server to connect to
	public class ServerIPHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle (InPacket recv)
		{
			recv.skip_byte ();

			LoginParser.parse_login (recv);

			int cid = recv.read_int ();
			new PlayerLoginPacket (cid).dispatch ();
		}
	}

	// Handler for a packet which responds to the request for a character name
	public class CharnameResponseHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle (InPacket recv)
		{
			// Read the name and if it is already in use
			string name = recv.read_string ();
			bool used = recv.read_bool ();

			// Notify the character creation screen
			var raceselect = UI.get ().get_element<UIRaceSelect> ();
			if (raceselect != null)
			{
				raceselect.get ().send_naming_result (used);
			}
		}
	}

	// Handler for the packet that notifies that a char was successfully created
	public class AddNewCharEntryHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle (InPacket recv)
		{
			recv.skip (1);

			// Parse info on the new character
			CharEntry character = LoginParser.parse_charentry (recv);

			// Read the updated character selection
			var charselect = UI.get ().get_element<UICharSelect> ();
			if (charselect != null)
			{
				charselect.get ().add_character ((character));
			}
		}
	}

	// Handler for a packet that responds to the request to the delete a character
	public class DeleteCharResponseHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle (InPacket recv)
		{
			// Read the character id and if deletion was successful (PIC was correct)
			int cid = recv.read_int ();
			byte state = (byte)recv.read_byte ();

			// Extract information from the state byte
			if (state != 0)
			{
				UILoginNotice.Message message;

				switch (state)
				{
					case 10:
						message = UILoginNotice.Message.BIRTHDAY_INCORRECT;
						break;
					case 20:
						message = UILoginNotice.Message.INCORRECT_PIC;
						break;
					default:
						message = UILoginNotice.Message.UNKNOWN_ERROR;
						break;
				}

				UI.get ().emplace<UILoginNotice> (message);
			}
			else
			{
				var charselect = UI.get ().get_element<UICharSelect> ();
				if (charselect != null)
				{
					charselect.get ().remove_character (cid);
				}
			}
		}
	}

	// Handles the packet that contains information on recommended worlds
	public class RecommendedWorldsHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle (InPacket recv)
		{
			var worldselect = UI.get ().get_element<UIWorldSelect> ();
			if (worldselect != null)
			{
				short count = recv.read_byte ();

				for (uint i = 0; i < count; i++)
				{
					RecommendedWorld world = LoginParser.parse_recommended_world (recv);

					if (world.wid != -1 && !string.IsNullOrEmpty (world.message))
					{
						worldselect.get ().add_recommended_world (world);
					}
				}
			}
		}
	}
}