


namespace ms
{
	// Handler for a packet which contains NPC dialog
	public class NpcDialogueHandler : PacketHandler
	{
		public override void handle(InPacket recv)
		{
			recv.skip(1);

			int npcid = recv.read_int();
			sbyte msgtype = recv.read_byte(); // 0 - textonly, 1 - yes/no, 4 - selection, 12 - accept/decline
			sbyte speaker = recv.read_byte();
			string text = recv.read_string();

			short style = 0;

			if (msgtype == 0 && recv.length() > 0)
			{
				style = recv.read_short();
			}

			UI.get().emplace<UINpcTalk>();
			UI.get().enable();

			var npctalk = UI.get ().get_element<UINpcTalk> ();
			if (npctalk)
			{
				npctalk.get ().change_text(npcid, msgtype, style, speaker, text);
			}
		}
	}

	// Opens an NPC shop defined by the packet's contents
	public class OpenNpcShopHandler : PacketHandler
	{
		public override void handle(InPacket recv)
		{
			int npcid = recv.read_int();
			var oshop = UI.get().get_element<UIShop>();

			if (oshop == false)
			{
				return;
			}

			UIShop shop = oshop.get ();

			shop.reset(npcid);

			short size = recv.read_short();

			for (short i = 0; i < size; i++)
			{
				int itemid = recv.read_int();
				int price = recv.read_int();
				int pitch = recv.read_int();
				int time = recv.read_int();

				recv.skip(4);

				bool norecharge = recv.read_short() == 1;

				if (norecharge)
				{
					short buyable = recv.read_short();

					shop.add_item(itemid, price, pitch, time, buyable);
				}
				else
				{
					recv.skip(4);

					short rechargeprice = recv.read_short();
					short slotmax = recv.read_short();

					shop.add_rechargable(itemid, price, pitch, time, rechargeprice, slotmax);
				}
			}
		}
	}
}


