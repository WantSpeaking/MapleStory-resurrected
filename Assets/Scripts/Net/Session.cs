using System;
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


#if USE_ASIO
#define BOOST_DATE_TIME_NO_LIB
#define BOOST_REGEX_NO_LIB
#else
#endif

namespace ms
{
	public class Session : Singleton<Session>
	{
		public Session ()
		{
			connected = false;
			length = 0;
			pos = 0;
		}

		public new void Dispose ()
		{
			if (connected)
			{
				socket.close ();
			}

			base.Dispose ();
		}

		// Connect using host and port from the configuration file
		public Error init ()
		{
			string HOST = Setting<ServerIP>.get ().load ();
			string PORT = Setting<ServerPort>.get ().load ();

			if (!init (HOST, PORT))
			{
				return Error.Code.CONNECTION;
			}

			return Error.Code.NONE;
		}

		// Closes the current connection and opens a new one
		public void reconnect (string address, string port)
		{
			// Close the current connection and open a new one
			bool success = socket.close ();

			if (success)
			{
				init (address, port);
			}
			else
			{
				connected = false;
			}
		}

		private int index = 0;

		private void process (byte[] bytes, int available)
		{
			if (pos == 0)
			{
				index += NetConstants.HEADER_LENGTH;
				// Position is zero, meaning this is the start of a new packet. Start by determining length.
				length = cryptography.check_length (bytes);
				// Reading the length means we processed the header. Move forward by the header length.
				byte[] newBytes = new byte[bytes.Length - NetConstants.HEADER_LENGTH];
				Array.Copy (bytes, NetConstants.HEADER_LENGTH, newBytes, 0, newBytes.Length);
				bytes = newBytes;
				available -= NetConstants.HEADER_LENGTH;
			}

			// Determine how much we can write. Write data into the buffer.
			var towrite = length - pos;

			if (towrite > available)
			{
				towrite = available;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
			Array.Copy (bytes, 0, buffer, pos, towrite);
			//memcpy (buffer.Substring (pos), bytes, towrite);
			pos += towrite;

			// Check if the current packet has been fully processed
			if (pos >= length)
			{
				cryptography.decrypt (buffer, length);

				try
				{
					packetswitch.forward (buffer, length);
				}
				catch (PacketError err)
				{
					Console.Write (err.Message);
					Console.Write ("\n");
				}

				pos = 0;
				length = 0;

				// Check if there is more available
				int remaining = available - towrite;

				if (remaining >= NetConstants.MIN_PACKET_LENGTH)
				{
					byte[] newBytes = new byte[bytes.Length - towrite];
					Array.Copy (bytes, towrite, newBytes, 0, newBytes.Length);
					bytes = newBytes;
					// More packets are available, so we start over.
					process (bytes, remaining);
				}
			}
		}

		// Send a packet to the server
		public void write (byte[] packet_bytes, int packet_length)
		{
			if (!connected)
			{
				return;
			}

			byte[] header = new byte[NetConstants.HEADER_LENGTH];
			cryptography.create_header ( header, packet_length);
			cryptography.encrypt (packet_bytes, packet_length);

			socket.dispatch (header, NetConstants.HEADER_LENGTH);
			socket.dispatch (packet_bytes, packet_length);
		}

		// Check for incoming packets and handle them
		public void read ()
		{
			// Check if a packet has arrived. Handle if data is sufficient: 4 bytes (header) + 2 bytes (opcode) = 6 bytes.
			int result = socket.receive (connected);

			if (result >= NetConstants.MIN_PACKET_LENGTH || length > 0)
			{
				// Retrieve buffer from the socket and process it
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C# does not have an equivalent to pointers to value types:
//ORIGINAL LINE: const sbyte* bytes = socket.get_buffer();
				var bytes = socket.get_buffer ();
				process (bytes, result);
			}
		}

		// Closes the current connection and opens a new one with default connection settings
		public void reconnect ()
		{
			string HOST = Setting<ServerIP>.get ().load ();
			string PORT = Setting<ServerPort>.get ().load ();

			reconnect (HOST, PORT);
		}

		// Check if the connection is alive
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_connected() const
		public bool is_connected ()
		{
			return connected;
		}

		private bool init (string host, string port)
		{
			// Connect to the server
			connected = socket.open (host, port);

			if (connected)
			{
				// Read keys necessary for communicating with the server
				cryptography = new Cryptography (socket.get_buffer ());
			}

			return connected;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Pointer arithmetic is detected on the parameter 'bytes', so pointers on this parameter are left unchanged:


		private Cryptography cryptography = new Cryptography ();
		private PacketSwitch packetswitch = new PacketSwitch ();

		private byte[] buffer = new byte[NetConstants.MAX_PACKET_LENGTH];
		private int length;
		private int pos;
		private bool connected;

#if USE_ASIO
		private SocketAsio socket = new SocketAsio();
#else
		private SocketWinsock socket = new SocketWinsock ();
#endif
	}
}