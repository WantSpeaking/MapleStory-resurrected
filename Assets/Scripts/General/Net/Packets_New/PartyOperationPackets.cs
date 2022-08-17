namespace ms
{
	public class PartyOperationPackets : OutPacket
	{
		public PartyOperationPackets (short opc) : base ((short)OutPacket.Opcode.LOGIN)
		{
			
		}
	}
}