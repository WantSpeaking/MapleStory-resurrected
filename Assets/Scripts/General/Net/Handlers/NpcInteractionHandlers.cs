


using ms_Unity;
using System.Collections.Generic;

namespace ms
{
    // Handler for a packet which contains NPC dialog
    public class NpcTalkHandler : PacketHandler
    {
        public override void handle(InPacket recv)
        {
            recv.skip(1);//nSpeakerTypeID most times is 4

            int npcid = recv.read_int();
            sbyte msgtype = recv.read_byte(); // 0 - textonly, 1 - sendYesNo, 4 - sendSimple (selection),7 - getNPCTalkStyle, 12 - sendAcceptDecline
            sbyte speaker = recv.read_byte();
            string text = recv.read_string();

            short style = 0;//00 00(0) - sendOk,00 01(256)-sendNext, 01 00(1)-sendPrev,01 01(257)-sendNextPrev,

            if (msgtype == 0 && recv.length() > 0)
            {
                style = recv.read_short();
            }
            //AppDebug.Log ($"style: {style}");

            UI.get().emplace<UINpcTalk>();
            UI.get().enable();

            var npctalk = UI.get().get_element<UINpcTalk>();
            if (npctalk)
            {
                npctalk.get().change_text(npcid, msgtype, style, speaker, text);
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

            UIShop shop = oshop.get();

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



    /// <summary>
    /// 确认购买
    /// </summary>
    public class CONFIRM_SHOP_TRANSACTION_Handler : PacketHandler
    { /* 00 = /
         * 01 = You don't have enough in stock
         * 02 = You do not have enough mesos
         * 03 = Please check if your inventory is full or not
         * 05 = You don't have enough in stock
         * 06 = Due to an error, the trade did not happen
         * 07 = Due to an error, the trade did not happen
         * 08 = /
         * 13 = You need more items
         * 14 = CRASH; LENGTH NEEDS TO BE LONGER :O
         */

        private readonly static Dictionary<byte, string> errorCodes = new Dictionary<byte, string>()
        {
            { 1, "您的库存不足" },
            { 2, "金币不够" },
            { 3, "背包空间不够" } ,
             { 5, "您的库存不足" },
             { 6, "交易失败" },
             { 7, "交易失败" },
            { 8, "成功" },
             { 13, "您需要更多物品" },
             { 14, "未知的错误" },
        };

        private const string unknowError = "成功";

        public override void handle(InPacket recv)
        {
            var code = recv.readByte();
            if (!errorCodes.TryGetValue(code, out var errorMessage))
            {
                errorMessage = unknowError;
            }

            FGUI_OK.ShowNotice(errorMessage);
        }
    }
}


