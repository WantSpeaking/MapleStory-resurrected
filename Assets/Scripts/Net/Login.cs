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


namespace ms
{
	public struct Account
	{
		public int accid;
		public sbyte female;
		public bool admin;
		public string name;
		public bool muted;
		public bool pin;
		public sbyte pic;
	}

	public struct World
	{
		public World (string name, string message, List<int> chloads, byte channelcount, byte flag, sbyte wid)
		{
			this.name = name;
			this.message = message;
			this.chloads = chloads;
			this.channelcount = channelcount;
			this.flag = flag;
			this.wid = wid;
		}

		public string name;
		public string message;
		public List<int> chloads;
		public byte channelcount;
		public byte flag;
		public sbyte wid;
	}

	public class RecommendedWorld
	{
		public string message;
		public int wid;

		public RecommendedWorld (string message, int wid)
		{
			this.message = message;
			this.wid = wid;
		}
	}

	public class StatsEntry
	{
		public string name;
		public bool female;
		public List<long> petids = new List<long> ();
		public EnumMap<MapleStat.Id, ushort> stats = new EnumMap<MapleStat.Id, ushort> ();
		public long exp;
		public int mapid;
		public byte portal;
		public System.Tuple<int, sbyte> rank = new System.Tuple<int, sbyte> (0, 0);
		public System.Tuple<int, sbyte> jobrank = new System.Tuple<int, sbyte> (0, 0);
	}

	public class LookEntry
	{
		public bool female;
		public byte skin;
		public int faceid;
		public int hairid;
		public SortedDictionary<sbyte, int> equips = new SortedDictionary<sbyte, int> ();
		public SortedDictionary<sbyte, int> maskedequips = new SortedDictionary<sbyte, int> ();
		public List<int> petids = new List<int> ();
	}

	public class CharEntry
	{
		public StatsEntry stats = new StatsEntry ();
		public LookEntry look = new LookEntry ();
		public int id;

		public CharEntry (StatsEntry stats, LookEntry look, int cid)
		{
			this.stats = stats;
			this.look = look;
			this.id = cid;
		}
	}
}