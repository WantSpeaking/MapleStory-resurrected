


namespace ms
{
	public class PingHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket&) const override
		public override void handle(InPacket UnnamedParameter1)
		{
			new PongPacket().dispatch();
		}
	}
}

