using System;




namespace ms
{
	// Opcode: UPDATE_QUEST_INFO(211)
	public class UpdateQuestInfoHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			var mode1 = recv.readByte ();

			if (mode1 == 6)//addQuestTimeLimit
			{
				recv.readShort ();
				var questId = recv.readShort ();
				var time = recv.readInt ();
				AppDebug.Log ($"todo UpdateQuestInfoHandler addQuestTimeLimit");

			}
			else
			if (mode1 == 7)//removeQuestTimeLimit
			{
				recv.readShort ();
				var questId = recv.readShort ();
				AppDebug.Log ($"todo UpdateQuestInfoHandler removeQuestTimeLimit");

			}
			else
			if (mode1 == 8)//updateQuestInfo updateQuestFinish //0x0A in v95
			{
				var questId = recv.readShort ();
				var npcId = recv.readInt ();
				var nextquest = recv.read_short ();//0 means has no next quest

				var logMessage = nextquest == 0 ? "updateQuestInfo" : "updateQuestFinish";
				AppDebug.Log ($"todo UpdateQuestInfoHandler {logMessage} nextquest:{nextquest}");

			}
			else
			if (mode1 == 10)//questError
			{
				var questId = recv.readShort ();
				AppDebug.Log ($"todo UpdateQuestInfoHandler questError");

			}
			else
			if (mode1 == 11)//questFailure No meso
			{
				AppDebug.Log ($"todo UpdateQuestInfoHandler questFailure No meso");

			}
			else
			if (mode1 == 13)//questFailure Worn by character
			{
				AppDebug.Log ($"todo UpdateQuestInfoHandler questFailure Worn by character");

			}
			else
			if (mode1 == 14)//questFailure Not having the item
			{
				AppDebug.Log ($"todo UpdateQuestInfoHandler questFailure Not having the item");

			}
			else
			if (mode1 == 15)//questExpire
			{
				var questId = recv.readShort ();
				AppDebug.Log ($"todo UpdateQuestInfoHandler questExpire");

			}
		}

	}
}