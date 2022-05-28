


namespace ms
{
	// Handler for entering the Cash Shop
	public class SetCashShopHandler : PacketHandler
	{
		public override void handle(InPacket recv)
		{
			CashShopParser.parseCharacterInfo(recv);

			recv.skip_byte(); // Not MTS
			recv.skip_string(); // account_name
			recv.skip_int();

			short specialcashitem_size = recv.read_short();

			for (uint i = 0; i < specialcashitem_size; i++)
			{
				recv.skip_int(); // sn
				recv.skip_int(); // mod
				recv.skip_byte(); // info
			}

			recv.skip(121);

			for (uint cat = 1; cat <= 8; cat++)
			{
				for (uint gender = 0; gender < 2; gender++)
				{
					for (uint com = 0; com < 5; com++)
					{
						recv.skip_int(); // category
						recv.skip_int(); // gender
						recv.skip_int(); // commoditysn
					}
				}
			}

			recv.skip_int();
			recv.skip_short();
			recv.skip_byte();
			recv.skip_int();

			transition();

			UI.get().change_state(UI.State.CASHSHOP);
		}

		private void transition()
		{
			float fadestep = 0.025f;

			Window.get().fadeout(fadestep, () =>
			{
					GraphicsGL.get().clear();

					Stage.get().load(-1, 0);

					UI.get().enable();
					Timer.get().start();
					GraphicsGL.get().unlock();
			});

			GraphicsGL.get().enlock();
			Stage.get().clear();
			Timer.get().start();
		}
	}
}


