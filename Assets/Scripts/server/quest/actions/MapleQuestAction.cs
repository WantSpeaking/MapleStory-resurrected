using System.Collections.Generic;
using MapleLib.WzLib;
using provider;

/*
	This file is part of the MapleSolaxia Maple Story Server

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation version 3 as published by
    the Free Software Foundation. You may not use, modify or distribute
    this program under any other version of the GNU Affero General Public
    License.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace server.quest.actions
{
	using MapleCharacter = client.MapleCharacter;
	using WzImageProperty = MapleLib.WzLib.WzImageProperty;


	/// 
	/// <summary>
	/// @author Tyler (Twdtwd)
	/// </summary>
	public abstract class MapleQuestAction
	{
		private readonly MapleQuestActionType type;
		protected internal int questID;
		protected ms.Player player => ms.Stage.get ().get_player ();
		public MapleQuestAction(MapleQuestActionType action, MapleQuest quest)
		{
			this.type = action;
			this.questID = quest.Id;
		}

		public abstract void run(MapleCharacter chr, int? extSelection);
		public abstract void processData(MapleData data);

		public virtual bool check(MapleCharacter chr, int? extSelection)
		{
			return true;
		}

		public virtual MapleQuestActionType Type
		{
			get
			{
						return type;
			}
		}

			public static IList<int> getJobBy5ByteEncoding(int encoded)
			{
					IList<int> ret = new List<int>();
					if ((encoded & 0x1) != 0)
					{
						ret.Add(0);
					}
					if ((encoded & 0x2) != 0)
					{
						ret.Add(100);
					}
					if ((encoded & 0x4) != 0)
					{
						ret.Add(200);
					}
					if ((encoded & 0x8) != 0)
					{
						ret.Add(300);
					}
					if ((encoded & 0x10) != 0)
					{
						ret.Add(400);
					}
					if ((encoded & 0x20) != 0)
					{
						ret.Add(500);
					}
					if ((encoded & 0x400) != 0)
					{
						ret.Add(1000);
					}
					if ((encoded & 0x800) != 0)
					{
						ret.Add(1100);
					}
					if ((encoded & 0x1000) != 0)
					{
						ret.Add(1200);
					}
					if ((encoded & 0x2000) != 0)
					{
						ret.Add(1300);
					}
					if ((encoded & 0x4000) != 0)
					{
						ret.Add(1400);
					}
					if ((encoded & 0x8000) != 0)
					{
						ret.Add(1500);
					}
					if ((encoded & 0x20000) != 0)
					{
						ret.Add(2001); //im not sure of this one
						ret.Add(2200);
					}
					if ((encoded & 0x100000) != 0)
					{
						ret.Add(2000);
						ret.Add(2001); //?
					}
					if ((encoded & 0x200000) != 0)
					{
						ret.Add(2100);
					}
					if ((encoded & 0x400000) != 0)
					{
						ret.Add(2001); //?
						ret.Add(2200);
					}

					if ((encoded & 0x40000000) != 0)
					{ //i haven't seen any higher than this o.o
						ret.Add(3000);
						ret.Add(3200);
						ret.Add(3300);
						ret.Add(3500);
					}
					return ret;
			}

			public static IList<int> getJobBySimpleEncoding(int encoded)
			{
					IList<int> ret = new List<int>();
					if ((encoded & 0x1) != 0)
					{
						ret.Add(200);
					}
					if ((encoded & 0x2) != 0)
					{
						ret.Add(300);
					}
					if ((encoded & 0x4) != 0)
					{
						ret.Add(400);
					}
					if ((encoded & 0x8) != 0)
					{
						ret.Add(500);
					}
					return ret;
			}
	}

}