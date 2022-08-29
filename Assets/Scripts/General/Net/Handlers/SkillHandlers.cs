



namespace ms
{
	//Opcode.SKILL_EFFECT 190
	public class SKILL_EFFECT_Handler : PacketHandler
	{
		public override void handle(InPacket recv)
		{
			int characterid = recv.read_int();
			int skillid = recv.read_int();
			byte level = (byte)recv.read_byte();
			byte flags = (byte)recv.read_byte();
			byte speed = (byte)recv.read_byte();
			byte direction = (byte)recv.read_byte();

			
		}

	}

	//Opcode.CANCEL_SKILL_EFFECT 191
	public class CANCEL_SKILL_EFFECT_Handler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			int characterid = recv.read_int ();
			int skillid = recv.read_int ();
			

		}

	}
}

