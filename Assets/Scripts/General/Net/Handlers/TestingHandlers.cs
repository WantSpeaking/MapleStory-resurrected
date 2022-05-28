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
	public class FieldEffectHandler : PacketHandler
	{
		public override void handle(InPacket recv)
		{
			int rand = recv.read_byte();

			// Effect
			if (rand == 3)
			{
				string path = recv.read_string();

				Stage.get().add_effect(path);

				return;
			}

			Console.Write("[FieldEffectHandler]: Unknown value: [");
			Console.Write(rand);
			Console.Write("]");
			Console.Write("\n");
		}
	}
}


