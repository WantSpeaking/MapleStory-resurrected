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

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ms
{
	public class LoginParser
	{
		public static Account parse_account (InPacket recv)
		{
			Account account;

			recv.skip_short ();

			account.accid = recv.read_int ();
			account.female = recv.read_byte ();
			account.admin = recv.read_bool ();

			recv.skip_byte (); // Admin
			recv.skip_byte (); // Country Code

			account.name = recv.read_string ();

			recv.skip_byte ();

			account.muted = recv.read_bool ();

			recv.skip_long (); // muted until
			recv.skip_long (); // creation date

			recv.skip_int (); // Remove "Select the world you want to play in"

			account.pin = recv.read_bool (); // 0 - Enabled, 1 - Disabled
			account.pic = recv.read_byte (); // 0 - Register, 1 - Ask, 2 - Disabled

			return account;
		}

		public static World parse_world (InPacket recv)
		{
			var wid = recv.read_byte ();

			if (wid == -1)
				return new World (string.Empty, string.Empty, new List<int> (), 0, 0, wid);


			string name = recv.read_string ();
			byte flag = (byte)recv.read_byte ();
			string message = recv.read_string ();

			recv.skip (5);

			List<int> chloads = new List<int> ();
			byte channelcount = (byte)recv.read_byte ();

			for (byte i = 0; i < channelcount; ++i)
			{
				recv.read_string (); // channel name

				chloads.Add (recv.read_int ());

				recv.skip (1);
				recv.skip (2);
			}

			recv.skip (2);
			return new World (name, message, chloads, channelcount, flag, wid);
		}

		public static RecommendedWorld parse_recommended_world (InPacket recv)
		{
			int wid = recv.read_int ();

			if (wid == -1)
				return new RecommendedWorld (string.Empty, wid);

			string message = recv.read_string ();
			return new RecommendedWorld (message, wid);
		}

		public static CharEntry parse_charentry (InPacket recv)
		{
			int cid = recv.read_int ();
			StatsEntry stats = parse_stats (recv);
			LookEntry look = parse_look (recv);

			recv.read_bool (); // 'rankinfo' bool

			if (recv.read_bool ())
			{
				int currank = recv.read_int ();
				int rankmv = recv.read_int ();
				int curjobrank = recv.read_int ();
				int jobrankmv = recv.read_int ();
				sbyte rankmc = (sbyte)((rankmv > 0) ? '+' : (rankmv < 0) ? '-' : '=');
				sbyte jobrankmc = (sbyte)((jobrankmv > 0) ? '+' : (jobrankmv < 0) ? '-' : '=');

				stats.rank = new Tuple<int, sbyte> (currank, rankmc);
				stats.jobrank = new Tuple<int, sbyte> (curjobrank, jobrankmc);
			}

			return new CharEntry (stats, look, cid);
		}

		public static StatsEntry parse_stats (InPacket recv)
		{
			// TODO: This is similar to CashShopParser.cpp, try and merge these.
			StatsEntry statsentry = new StatsEntry ();

			statsentry.name = recv.read_padded_string (13);
			statsentry.female = recv.read_bool ();

			recv.read_byte (); // skin
			recv.read_int (); // face
			recv.read_int (); // hair

			for (int i = 0; i < 3; i++)
				statsentry.petids.Add (recv.read_long ());

			statsentry.stats[MapleStat.Id.LEVEL] = (ushort)recv.read_byte (); // TODO: Change to recv.read_short(); to increase level cap
			statsentry.stats[MapleStat.Id.JOB] = (ushort)recv.read_short ();
			statsentry.stats[MapleStat.Id.STR] = (ushort)recv.read_short ();
			statsentry.stats[MapleStat.Id.DEX] = (ushort)recv.read_short ();
			statsentry.stats[MapleStat.Id.INT] = (ushort)recv.read_short ();
			statsentry.stats[MapleStat.Id.LUK] = (ushort)recv.read_short ();
			statsentry.stats[MapleStat.Id.HP] = (ushort)recv.read_short ();
			statsentry.stats[MapleStat.Id.MAXHP] = (ushort)recv.read_short ();
			statsentry.stats[MapleStat.Id.MP] = (ushort)recv.read_short ();
			statsentry.stats[MapleStat.Id.MAXMP] = (ushort)recv.read_short ();
			statsentry.stats[MapleStat.Id.AP] = (ushort)recv.read_short ();
			statsentry.stats[MapleStat.Id.SP] = (ushort)recv.read_short ();
			statsentry.exp = recv.read_int ();
			statsentry.stats[MapleStat.Id.FAME] = (ushort)recv.read_short ();

			recv.skip (4); // gachaexp

			statsentry.mapid = recv.read_int ();
			statsentry.portal = (byte)recv.read_byte ();

			recv.skip (4); // timestamp

			return statsentry;
		}

		public static LookEntry parse_look (InPacket recv)
		{
			LookEntry look = new LookEntry ();

			look.female = recv.read_bool ();
			look.skin = (byte)recv.read_byte ();
			look.faceid = recv.read_int ();

			recv.read_bool (); // megaphone

			look.hairid = recv.read_int ();

			byte eqslot = (byte)recv.read_byte ();

			while (eqslot != 0xFF)
			{
				look.equips[(sbyte)eqslot] = recv.read_int ();
				eqslot = (byte)recv.read_byte ();
			}

			byte mskeqslot = (byte)recv.read_byte ();

			while (mskeqslot != 0xFF)
			{
				look.maskedequips[(sbyte)mskeqslot] = recv.read_int ();
				mskeqslot = (byte)recv.read_byte ();
			}

			look.maskedequips[-111] = recv.read_int ();

			for (byte i = 0; i < 3; i++)
				look.petids.Add (recv.read_int ());

			return look;
		}

		public static void parse_login (InPacket recv)
		{
			recv.skip_byte ();

			// Read the IPv4 address in a string
			StringBuilder addrstr = new StringBuilder ();

			for (int i = 0; i < 4; i++)
			{
				byte num = (byte)(recv.read_byte ());
				addrstr.Append (num);

				if (i < 3)
					addrstr.Append ('.');
			}

			// Read the port address in a string
			string portstr = (recv.read_short ()).ToString ();

			// Attempt to reconnect to the server
			Debug.Log ($"Attempt to reconnect to the server (addrstr:{addrstr}\r portstr:{portstr})");
			Session.get ().reconnect (addrstr.ToString (), portstr);
		}
	}
}