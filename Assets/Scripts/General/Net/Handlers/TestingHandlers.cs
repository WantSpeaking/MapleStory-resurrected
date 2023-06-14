using System;




namespace ms
{
	// TODO: Comment
	public class CheckSpwResultHandler : PacketHandler
	{
		public override void handle(InPacket recv)
		{
			int reason = recv.read_byte();

			if (reason == 0)
			{
				UI.get().emplace<UILoginNotice>(UILoginNotice.Message.INCORRECT_PIC);
			}
			else
			{
				Console.Write("[CheckSpwResultHandler]: Unknown reason: [");
				Console.Write(reason);
				Console.Write("]");
				Console.Write("\n");
			}

			UI.get().enable();
		}
	}

	// TODO: Comment
	/// <summary>
	/// Boss hp opcode 138
	/// </summary>
	public class FieldEffectHandler : PacketHandler
	{
		public override void handle(InPacket recv)
		{
			int rand = recv.read_byte();

			if (rand == 3)//mapEffect
            {
				string path = recv.read_string();

				Stage.get().add_effect(path);

				return;
			}
            else if (rand == 4) //mapSound
            {

			}
            else if (rand == 5)//show Boss hp
            {
				var oid = recv.readInt();
				var currHP = recv.readInt();
                var maxHP = recv.readInt();
				var tagColor = recv.readByte();
				var tagBgColor = recv.readByte();

                Stage.get().get_mobs().send_bosshp(oid, currHP, maxHP);
            }
			
			Console.Write("[FieldEffectHandler]: Unknown value: [");
			Console.Write(rand);
			Console.Write("]");
			Console.Write("\n");
		}
	}
}


