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
	// Opcode: QuestAction(107)
	public enum QuestActionType
	{
		RestoreLostItem,
		StartQuest,
		CompleteQuest,
		ForfeitQuest,
		ScriptedStartQuest,
		ScriptedEndQuest,
	}

	public class QuestActionPacket : OutPacket
	{
		public QuestActionPacket (QuestActionType actionType, short questid, int itemid = 0, int npc = 0, int selection = -1) : base ((short)OutPacket.Opcode.QUEST_ACTION)
		{
			var action = (sbyte)actionType;

			write_byte (action);
			write_short (questid);

			if (action == 0)// Restore lost item, Credits Darter ( Rajan )
			{
				write_int (0);
				write_int (itemid);
			}
			else if (action == 1)//Start Quest
			{
				write_int (npc);
				write_PLyerPos ();
			}
			else if (action == 2)// Complete Quest
			{
				write_int (npc);
				write_PLyerPos ();

				if (selection != -1)
				{
					write_int (selection);
				}
			}
			else if (action == 3)// forfeit quest
			{

			}
			else if (action == 4)// scripted start quest
			{
				write_int (npc);
				write_PLyerPos ();

			}
			else if (action == 5)// scripted end quests
			{
				write_int (npc);
				write_PLyerPos ();

			}
		}

	}

	public class StartQuestPacket : OutPacket
	{
		public StartQuestPacket (short questid, int npc) : base ((short)OutPacket.Opcode.QUEST_ACTION)
		{
			AppDebug.Log ($"开始任务：{questid}，npc：{npc}");

			var action = (sbyte)QuestActionType.StartQuest;

			write_byte (action);
			write_short (questid);

			write_int (npc);
			write_PLyerPos ();
		}

	}

	public class CompleteQuestPacket : OutPacket
	{
		public CompleteQuestPacket (short questid, int npc, int selection = -1) : base ((short)OutPacket.Opcode.QUEST_ACTION)
		{
			AppDebug.Log ($"完成任务：{questid}，npc：{npc},selection:{selection}");

			var action = (sbyte)QuestActionType.CompleteQuest;

			write_byte (action);
			write_short (questid);

			write_int (npc);
			write_PLyerPos ();

			if (selection != -1)
			{
				write_int (selection);
			}
		}

	}

	public class ForfeitQuestPacket : OutPacket
	{
		public ForfeitQuestPacket (short questid) : base ((short)OutPacket.Opcode.QUEST_ACTION)
		{
			AppDebug.Log ($"放弃任务：{questid}");

			var action = (sbyte)QuestActionType.ForfeitQuest;

			write_byte (action);
			write_short (questid);
		}

	}

	public class ScriptedStartQuest : OutPacket
	{
		public ScriptedStartQuest (short questid, int npc) : base ((short)OutPacket.Opcode.QUEST_ACTION)
		{
			AppDebug.Log ($"ScriptedStartQuest：{questid}");

			var action = (sbyte)QuestActionType.ScriptedStartQuest;

			write_byte (action);
			write_short (questid);

			write_int (npc);
			write_PLyerPos ();
		}
	}

	public class ScriptedEndQuest : OutPacket
	{
		public ScriptedEndQuest (short questid, int npc) : base ((short)OutPacket.Opcode.QUEST_ACTION)
		{
			AppDebug.Log ($"ScriptedEndQuest：{questid}");

			var action = (sbyte)QuestActionType.ScriptedEndQuest;

			write_byte (action);
			write_short (questid);

			write_int (npc);
			write_PLyerPos ();
		}
	}
}