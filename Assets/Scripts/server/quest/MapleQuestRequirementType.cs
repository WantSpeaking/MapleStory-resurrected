using System.Collections.Generic;

/*
	This file is part of the OdinMS Maple Story Server
    Copyright (C) 2008 Patrick Huy <patrick.huy@frz.cc>
		       Matthias Butz <matze@odinms.de>
		       Jan Christian Meyer <vimes@odinms.de>

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
namespace server.quest
{
	/// 
	/// <summary>
	/// @author Matze
	/// </summary>
	public sealed class MapleQuestRequirementType
	{
		public static readonly MapleQuestRequirementType UNDEFINED = new MapleQuestRequirementType ("UNDEFINED", InnerEnum.UNDEFINED, -1);
		public static readonly MapleQuestRequirementType JOB = new MapleQuestRequirementType ("JOB", InnerEnum.JOB, 0);
		public static readonly MapleQuestRequirementType ITEM = new MapleQuestRequirementType ("ITEM", InnerEnum.ITEM, 1);
		public static readonly MapleQuestRequirementType QUEST = new MapleQuestRequirementType ("QUEST", InnerEnum.QUEST, 2);
		public static readonly MapleQuestRequirementType MIN_LEVEL = new MapleQuestRequirementType ("MIN_LEVEL", InnerEnum.MIN_LEVEL, 3);
		public static readonly MapleQuestRequirementType MAX_LEVEL = new MapleQuestRequirementType ("MAX_LEVEL", InnerEnum.MAX_LEVEL, 4);
		//public static readonly MapleQuestRequirementType END_DATE = new MapleQuestRequirementType("END_DATE", InnerEnum.END_DATE, 5);
		public static readonly MapleQuestRequirementType MOB = new MapleQuestRequirementType ("MOB", InnerEnum.MOB, 6);
		public static readonly MapleQuestRequirementType NPC = new MapleQuestRequirementType ("NPC", InnerEnum.NPC, 7);
		public static readonly MapleQuestRequirementType FIELD_ENTER = new MapleQuestRequirementType ("FIELD_ENTER", InnerEnum.FIELD_ENTER, 8);
		public static readonly MapleQuestRequirementType INTERVAL = new MapleQuestRequirementType ("INTERVAL", InnerEnum.INTERVAL, 9);
		public static readonly MapleQuestRequirementType SCRIPT = new MapleQuestRequirementType ("SCRIPT", InnerEnum.SCRIPT, 10);
		public static readonly MapleQuestRequirementType PET = new MapleQuestRequirementType ("PET", InnerEnum.PET, 11);
		public static readonly MapleQuestRequirementType MIN_PET_TAMENESS = new MapleQuestRequirementType ("MIN_PET_TAMENESS", InnerEnum.MIN_PET_TAMENESS, 12);
		public static readonly MapleQuestRequirementType MONSTER_BOOK = new MapleQuestRequirementType ("MONSTER_BOOK", InnerEnum.MONSTER_BOOK, 13);
		public static readonly MapleQuestRequirementType NORMAL_AUTO_START = new MapleQuestRequirementType ("NORMAL_AUTO_START", InnerEnum.NORMAL_AUTO_START, 14);
		public static readonly MapleQuestRequirementType INFO_NUMBER = new MapleQuestRequirementType ("INFO_NUMBER", InnerEnum.INFO_NUMBER, 15);
		public static readonly MapleQuestRequirementType INFO_EX = new MapleQuestRequirementType ("INFO_EX", InnerEnum.INFO_EX, 16);
		public static readonly MapleQuestRequirementType COMPLETED_QUEST = new MapleQuestRequirementType ("COMPLETED_QUEST", InnerEnum.COMPLETED_QUEST, 17);
		public static readonly MapleQuestRequirementType START = new MapleQuestRequirementType ("START", InnerEnum.START, 18);
		public static readonly MapleQuestRequirementType END = new MapleQuestRequirementType ("END", InnerEnum.END, 19);
		public static readonly MapleQuestRequirementType DAY_BY_DAY = new MapleQuestRequirementType ("DAY_BY_DAY", InnerEnum.DAY_BY_DAY, 20);
		public static readonly MapleQuestRequirementType MESO = new MapleQuestRequirementType ("MESO", InnerEnum.MESO, 21);
		public static readonly MapleQuestRequirementType BUFF = new MapleQuestRequirementType ("BUFF", InnerEnum.BUFF, 22);
		public static readonly MapleQuestRequirementType EXCEPT_BUFF = new MapleQuestRequirementType ("EXCEPT_BUFF", InnerEnum.EXCEPT_BUFF, 23);

		private static readonly IList<MapleQuestRequirementType> valueList = new List<MapleQuestRequirementType> ();

		static MapleQuestRequirementType ()
		{
			valueList.Add (UNDEFINED);
			valueList.Add (JOB);
			valueList.Add (ITEM);
			valueList.Add (QUEST);
			valueList.Add (MIN_LEVEL);
			valueList.Add (MAX_LEVEL);
			//valueList.Add(END_DATE);
			valueList.Add (MOB);
			valueList.Add (NPC);
			valueList.Add (FIELD_ENTER);
			valueList.Add (INTERVAL);
			valueList.Add (SCRIPT);
			valueList.Add (PET);
			valueList.Add (MIN_PET_TAMENESS);
			valueList.Add (MONSTER_BOOK);
			valueList.Add (NORMAL_AUTO_START);
			valueList.Add (INFO_NUMBER);
			valueList.Add (INFO_EX);
			valueList.Add (COMPLETED_QUEST);
			valueList.Add (START);
			valueList.Add (END);
			valueList.Add (DAY_BY_DAY);
			valueList.Add (MESO);
			valueList.Add (BUFF);
			valueList.Add (EXCEPT_BUFF);
		}

		public enum InnerEnum
		{
			UNDEFINED,
			JOB,
			ITEM,
			QUEST,
			MIN_LEVEL,
			MAX_LEVEL,
			//END_DATE,
			MOB,
			NPC,
			FIELD_ENTER,
			INTERVAL,
			SCRIPT,
			PET,
			MIN_PET_TAMENESS,
			MONSTER_BOOK,
			NORMAL_AUTO_START,
			INFO_NUMBER,
			INFO_EX,
			COMPLETED_QUEST,
			START,
			END,
			DAY_BY_DAY,
			MESO,
			BUFF,
			EXCEPT_BUFF
		}

		public readonly InnerEnum innerEnumValue;
		private readonly string nameValue;
		private readonly int ordinalValue;
		private static int nextOrdinal = 0;
		internal readonly sbyte type;

		private MapleQuestRequirementType (string name, InnerEnum innerEnum, int type)
		{
			this.type = (sbyte)type;

			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		public sbyte Type
		{
			get
			{
				return type;
			}
		}

		public static MapleQuestRequirementType getByWZName (string name)
		{
			if (name.Equals ("job"))
			{
				return JOB;
			}
			else if (name.Equals ("quest"))
			{
				return QUEST;
			}
			else if (name.Equals ("item"))
			{
				return ITEM;
			}
			else if (name.Equals ("lvmin"))
			{
				return MIN_LEVEL;
			}
			else if (name.Equals ("lvmax"))
			{
				return MAX_LEVEL;
			}
			else if (name.Equals ("mob"))
			{
				return MOB;
			}
			else if (name.Equals ("npc"))
			{
				return NPC;
			}
			else if (name.Equals ("fieldEnter"))
			{
				return FIELD_ENTER;
			}
			else if (name.Equals ("interval"))
			{
				return INTERVAL;
			}
			else if (name.Equals ("startscript"))
			{
				return SCRIPT;
			}
			else if (name.Equals ("endscript"))
			{
				return SCRIPT;
			}
			else if (name.Equals ("pet"))
			{
				return PET;
			}
			else if (name.Equals ("pettamenessmin"))
			{
				return MIN_PET_TAMENESS;
			}
			else if (name.Equals ("mbmin"))
			{
				return MONSTER_BOOK;
			}
			else if (name.Equals ("normalAutoStart"))
			{
				return NORMAL_AUTO_START;
			}
			else if (name.Equals ("infoNumber"))
			{
				return INFO_NUMBER;
			}
			else if (name.Equals ("infoex"))
			{
				return INFO_EX;
			}
			else if (name.Equals ("questComplete"))
			{
				return COMPLETED_QUEST;
			}
			else if (name.Equals ("start"))
			{
				return START;
			}
			else if (name.Equals ("end"))
			{
				return END;
			}
			else if (name.Equals ("daybyday"))
			{
				return DAY_BY_DAY;
			}
			else if (name.Equals ("money"))
			{
				return MESO;
			}
			else if (name.Equals ("buff"))
			{
				return BUFF;
			}
			else if (name.Equals ("exceptbuff"))
			{
				return EXCEPT_BUFF;
			}
			else
			{
				return UNDEFINED;
			}
		}

		public static IList<MapleQuestRequirementType> values ()
		{
			return valueList;
		}

		public int ordinal ()
		{
			return ordinalValue;
		}

		public override string ToString ()
		{
			return nameValue;
		}

		public static MapleQuestRequirementType valueOf (string name)
		{
			foreach (MapleQuestRequirementType enumInstance in MapleQuestRequirementType.valueList)
			{
				if (enumInstance.nameValue == name)
				{
					return enumInstance;
				}
			}
			throw new System.ArgumentException (name);
		}
	}

}