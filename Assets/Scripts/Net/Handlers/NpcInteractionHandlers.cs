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
	// Handler for a packet which contains NPC dialog
	public class NpcDialogueHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
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

			//todo UI.get().emplace<UINpcTalk>();
			UI.get().enable();

			/*todo if (var npctalk = UI.get().get_element<UINpcTalk>())
			{
				npctalk.change_text(npcid, msgtype, style, speaker, text);
			}*/
		}
	}

	// Opens an NPC shop defined by the packet's contents
	public class OpenNpcShopHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle(InPacket recv)
		{
			int npcid = recv.read_int();
			//todo var oshop = UI.get().get_element<UIShop>();

			/*if (oshop == null)
			{
				return;
			}*/

			//todo UIShop shop = oshop.Indirection();

			//todo shop.reset(npcid);

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

					//todo shop.add_item(itemid, price, pitch, time, buyable);
				}
				else
				{
					recv.skip(4);

					short rechargeprice = recv.read_short();
					short slotmax = recv.read_short();

					//todo shop.add_rechargable(itemid, price, pitch, time, rechargeprice, slotmax);
				}
			}
		}
	}
}


