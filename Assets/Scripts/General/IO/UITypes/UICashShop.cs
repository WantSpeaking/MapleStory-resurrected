#define USE_NX

using System;
using System.Collections.Generic;
using Beebyte.Obfuscator;
using MapleLib.WzLib;



namespace ms
{
    [Skip]
    public class UICashShop : UIElement
    {
        public const Type TYPE = UIElement.Type.CASHSHOP;
        public const bool FOCUSED = true;
        public const bool TOGGLED = false;

        /*public UICashShop (params object[] args) : this ()
		{
		}*/

        public UICashShop()
        {
            this.preview_index = 0;
            this.menu_index = 1;
            this.promotion_index = 0;
            this.mvp_grade = 1;
            this.mvp_exp = 0.07f;
            this.list_offset = 0;
            WzObject CashShop = ms.wz.wzFile_ui["CashShop.img"];
            WzObject Base = CashShop["Base"];
            WzObject backgrnd = Base["backgrnd"];
            WzObject BestNew = Base["BestNew"];
            WzObject Preview = Base["Preview"];
            WzObject CSTab = CashShop["CSTab"];
            WzObject CSGLChargeNX = CSTab["CSGLChargeNX"];
            WzObject CSStatus = CashShop["CSStatus"];
            WzObject CSPromotionBanner = CashShop["CSPromotionBanner"];
            WzObject CSMVPBanner = CashShop["CSMVPBanner"];
            WzObject CSItemSearch = CashShop["CSItemSearch"];
            WzObject CSChar = CashShop["CSChar"];
            WzObject CSList = CashShop["CSList"];
            WzObject CSEffect = CashShop["CSEffect"];

            sprites.Add(backgrnd);
            sprites.Add(new Sprite(BestNew, new Point_short(139, 346)));

            BestNew_dim = new Texture(BestNew).get_dimensions();

            for (uint i = 0; i < 3; i++)
            {
                preview_sprites[i] = Preview[i.ToString()];
            }

            for (ushort i = 0; i < 3; i++)
            {
                buttons[(ushort)(Buttons.BtPreview1 + i)] = new TwoSpriteButton(Base["Tab"]["Disable"][i.ToString()], Base["Tab"]["Enable"][i.ToString()], new Point_short((short)(957 + (i * 17)), 46));
            }

            buttons[(int)Buttons.BtPreview1].set_state(Button.State.PRESSED);

            buttons[(int)Buttons.BtExit] = new MapleButton(CSTab["BtExit"], new Point_short(5, 728));
            buttons[(int)Buttons.BtChargeNX] = new MapleButton(CSGLChargeNX["BtChargeNX"], new Point_short(5, 554));
            buttons[(int)Buttons.BtChargeRefresh] = new MapleButton(CSGLChargeNX["BtChargeRefresh"], new Point_short(92, 554));

            for (uint i = 0; i < 9; i++)
            {
                menu_tabs[i] = CSTab["Tab"][i.ToString()];
            }

            buttons[(int)Buttons.BtChargeRefresh] = new MapleButton(CSGLChargeNX["BtChargeRefresh"], new Point_short(92, 554));
            buttons[(int)Buttons.BtWish] = new MapleButton(CSStatus["BtWish"], new Point_short(226, 6));
            buttons[(int)Buttons.BtMileage] = new MapleButton(CSStatus["BtMileage"], new Point_short(869, 4));
            buttons[(int)Buttons.BtHelp] = new MapleButton(CSStatus["BtHelp"], new Point_short(997, 4));
            buttons[(int)Buttons.BtCoupon] = new MapleButton(CSStatus["BtCoupon"], new Point_short(950, 4));

            Charset tab = new Charset();

            job_label = new Text(Text.Font.A11B, Text.Alignment.LEFT, Color.Name.SUPERNOVA, "Illium");
            name_label = new Text(Text.Font.A11B, Text.Alignment.LEFT, Color.Name.WHITE, "ShomeiZekkou");

            promotion_pos = new Point_short(138, 40);
            sprites.Add(new Sprite(CSPromotionBanner["shadow"], promotion_pos));

            promotion_sprites.Add(CSPromotionBanner["basic"]);

            buttons[(int)Buttons.BtNext] = new MapleButton(CSPromotionBanner["BtNext"], promotion_pos);
            buttons[(int)Buttons.BtPrev] = new MapleButton(CSPromotionBanner["BtPrev"], promotion_pos);

            for (uint i = 0; i < 7; i++)
            {
                mvp_sprites[i] = CSMVPBanner["grade"][i.ToString()];
            }

            mvp_pos = new Point_short(63, 681);
            buttons[(int)Buttons.BtDetailPackage] = new MapleButton(CSMVPBanner["BtDetailPackage"], mvp_pos);
            buttons[(int)Buttons.BtNonGrade] = new MapleButton(CSMVPBanner["BtNonGrade"], mvp_pos);

            buttons[(int)Buttons.BtDetailPackage].set_active(mvp_grade == 1);
            buttons[(int)Buttons.BtNonGrade].set_active(mvp_grade == 0);

            mvp_gauge = new Gauge(Gauge.Type.CASHSHOP, CSMVPBanner["gage"][0.ToString()], CSMVPBanner["gage"][2.ToString()], CSMVPBanner["gage"][1.ToString()], 84, 0.0f);

            Point_short search_pos = new Point_short(0, 36);
            sprites.Add(new Sprite(CSItemSearch["backgrnd"], search_pos));
            sprites.Add(new Sprite(CSItemSearch["search"], search_pos + new Point_short(35, 8)));

            buttons[(int)Buttons.BtBuyAvatar] = new MapleButton(CSChar["BtBuyAvatar"], new Point_short(642, 305));
            buttons[(int)Buttons.BtDefaultAvatar] = new MapleButton(CSChar["BtDefaultAvatar"], new Point_short(716, 305));
            buttons[(int)Buttons.BtInventory] = new MapleButton(CSChar["BtInventory"], new Point_short(938, 305));
            buttons[(int)Buttons.BtSaveAvatar] = new MapleButton(CSChar["BtSaveAvatar"], new Point_short(864, 305));
            buttons[(int)Buttons.BtTakeoffAvatar] = new MapleButton(CSChar["BtTakeoffAvatar"], new Point_short(790, 305));

            charge_charset = new Charset(CSGLChargeNX["Number"], Charset.Alignment.RIGHT);

            item_base = CSList["Base"];
            item_line = Base["line"];
            item_none = Base["noItem"];

            foreach (var item_label in CSEffect)
            {
                item_labels.Add(item_label);
            }

            items.Add(new Item(5220000, Item.Label.HOT, 34000, 11));
            items.Add(new Item(5220000, Item.Label.HOT, 34000, 11));
            items.Add(new Item(5220000, Item.Label.HOT, 0, 0));
            items.Add(new Item(5220000, Item.Label.HOT, 0, 0));
            items.Add(new Item(5220000, Item.Label.HOT, 10000, 11));
            items.Add(new Item(5220000, Item.Label.NEW, 0, 0));
            items.Add(new Item(5220000, Item.Label.SALE, 7000, 0));
            items.Add(new Item(5220000, Item.Label.NEW, 13440, 0));
            items.Add(new Item(5220000, Item.Label.NEW, 7480, 0));
            items.Add(new Item(5220000, Item.Label.NEW, 7480, 0));
            items.Add(new Item(5220000, Item.Label.NEW, 7480, 0));
            items.Add(new Item(5220000, Item.Label.NONE, 12000, 11));
            items.Add(new Item(5220000, Item.Label.NONE, 22000, 11));
            items.Add(new Item(5220000, Item.Label.NONE, 0, 0));
            items.Add(new Item(5220000, Item.Label.NONE, 0, 0));
            items.Add(new Item(5220000, Item.Label.MASTER, 0, 15));

            for (int i = 0; i < MAX_ITEMS; i++)
            {
                var quot = i / 7;
                var rem = i % 7;
                buttons[(ushort)(Buttons.BtBuy + (ushort)i)] = new MapleButton(CSList["BtBuy"], new Point_short(146, 523) + new Point_short((short)(124 * rem), (short)(205 * quot)));

                item_name[i] = new Text(Text.Font.A11B, Text.Alignment.CENTER, Color.Name.MINESHAFT);
                item_price[i] = new Text(Text.Font.A11M, Text.Alignment.CENTER, Color.Name.GRAY);
                item_discount[i] = new Text(Text.Font.A11M, Text.Alignment.CENTER, Color.Name.SILVERCHALICE);
                item_percent[i] = new Text(Text.Font.A11M, Text.Alignment.CENTER, Color.Name.TORCHRED);
            }

            Point_short slider_pos = new Point_short(1007, 372);

            list_slider = new Slider((int)Slider.Type.THIN_MINESHAFT, new Range_short(slider_pos.y(), (short)(slider_pos.y() + 381)), slider_pos.x(), 2, 7, (bool upwards) =>
       {
           short shift = (short)(upwards ? -7 : 7);
           bool above = list_offset >= 0;
           bool below = list_offset + shift < items.Count;

           if (above && below)
           {
               list_offset += shift;

               update_items();
           }
       });

            update_items();

            dimension = new Texture(backgrnd).get_dimensions();
        }

        public override void draw(float inter)
        {
            preview_sprites[preview_index].draw(position + new Point_short(644, 65), inter);

            base.draw_sprites(inter);

            menu_tabs[menu_index].draw(position + new Point_short(0, 63), inter);

            Point_short label_pos = position + new Point_short(4, 3);
            job_label.draw(label_pos);

            var length = job_label.width();
            name_label.draw(label_pos + new Point_short((short)(length + 10), 0));

            promotion_sprites[promotion_index].draw(position + promotion_pos, inter);

            mvp_sprites[mvp_grade].draw(position + mvp_pos, inter);
            mvp_gauge.draw(position + mvp_pos);

            Point_short charge_pos = position + new Point_short(107, 388);

            charge_charset.draw("0", charge_pos + new Point_short(0, 30 * 1));
            charge_charset.draw("3,300", charge_pos + new Point_short(0, 30 * 2));
            charge_charset.draw("0", charge_pos + new Point_short(0, 30 * 3));
            charge_charset.draw("8,698,565", charge_pos + new Point_short(0, 30 * 4));
            charge_charset.draw("0", charge_pos + new Point_short(0, 30 * 5));

            if (items.Count > 0)
            {
                item_line.draw(position + new Point_short(139, 566), inter);
            }
            else
            {
                item_none.draw(position + new Point_short(137, 372) + new Point_short((short)(BestNew_dim.x() / 2), (short)(list_slider.getvertical().length() / 2)) - item_none.get_dimensions() / (short)2, inter);
            }

            for (uint i = 0; i < MAX_ITEMS; i++)
            {
                short index = (short)(i + list_offset);

                if (index < items.Count)
                {
                    var quot = i / 7;
                    var rem = i % 7;
                    Item item = items[index];

                    item_base.draw(position + new Point_short(137, 372) + new Point_short((short)(124 * rem), (short)(205 * quot)), inter);
                    item.draw(new DrawArgument(position + new Point_short(164, 473) + new Point_short((short)(124 * rem), (short)(205 * quot)), 2.0f, 2.0f));

                    if (item.label != Item.Label.NONE)
                    {
                        item_labels[(int)(item.label + 1)].draw(position + new Point_short(152, 372) + new Point_short((short)(124 * rem), (short)(205 * quot)), inter);
                    }

                    item_name[i].draw(position + new Point_short(192, 480) + new Point_short((short)(124 * rem), (short)(205 * quot)));

                    if (item_discount[i].get_text() == "")
                    {
                        item_price[i].draw(position + new Point_short(195, 499) + new Point_short((short)(124 * rem), (short)(205 * quot)));
                    }
                    else
                    {
                        item_price[i].draw(position + new Point_short(196, 506) + new Point_short((short)(124 * rem), (short)(205 * quot)));

                        item_discount[i].draw(position + new Point_short(185, 495) + new Point_short((short)(124 * rem), (short)(205 * quot)));
                        item_percent[i].draw(position + new Point_short((short)(198 + (item_discount[i].width() / 2)), 495) + new Point_short((short)(124 * rem), (short)(205 * quot)));
                    }
                }
            }

            list_slider.draw(new Point_short(position));

            base.draw_buttons(inter);
        }

        public override void update()
        {
            base.update();

            mvp_gauge.update(mvp_exp);
        }

        public override Button.State button_pressed(ushort buttonid)
        {
            switch ((Buttons)buttonid)
            {
                case Buttons.BtPreview1:
                case Buttons.BtPreview2:
                case Buttons.BtPreview3:
                    buttons[preview_index].set_state(Button.State.NORMAL);

                    preview_index = (byte)buttonid;
                    return Button.State.PRESSED;
                case Buttons.BtExit:
                    {
                        float fadestep = 0.025f;

                        Window.get().fadeout(fadestep, () =>
                        {
                          GraphicsGL.get().clear();
                          new ChangeMapPacket().dispatch();
                        });

                        GraphicsGL.get().enlock();
                        Stage.get().clear();
                        Timer.get().start();
                        UI.get().change_state(UI.State.GAME);
                        TestURPBatcher.Instance.Clear();

                        return Button.State.NORMAL;
                    }
                case Buttons.BtNext:
                    {
                        uint size = (uint)(promotion_sprites.Count - 1);

                        promotion_index++;

                        if (promotion_index > size)
                        {
                            promotion_index = 0;
                        }

                        return Button.State.NORMAL;
                    }
                case Buttons.BtPrev:
                    {
                        uint size = (uint)(promotion_sprites.Count - 1);

                        promotion_index--;

                        if (promotion_index < 0)
                        {
                            promotion_index = (sbyte)size;
                        }

                        return Button.State.NORMAL;
                    }
                case Buttons.BtChargeNX:
                    {
                        string url = Configuration.get().get_chargenx();

                        //todo 2 ShellExecuteA (null, "open", url, null, null, SW_SHOWNORMAL);

                        return Button.State.NORMAL;
                    }
                default:
                    break;
            }

            if (buttonid >= (int)Buttons.BtBuy)
            {
                short index = (short)(buttonid - Buttons.BtBuy + (ushort)list_offset);

                Item item = items[index];

                // TODO: Purchase item

                return Button.State.NORMAL;
            }

            return Button.State.DISABLED;
        }

        public override Cursor.State send_cursor(bool clicked, Point_short cursorpos)
        {
            Point_short cursor_relative = cursorpos - position;

            if (list_slider.isenabled())
            {
                Cursor.State state = list_slider.send_cursor(new Point_short(cursor_relative), clicked);

                if (state != Cursor.State.IDLE)
                {
                    return state;
                }
            }

            return base.send_cursor(clicked, new Point_short(cursorpos));
        }

        public override UIElement.Type get_type()
        {
            return TYPE;
        }

        public void exit_cashshop()
        {
            UI ui = UI.get();
            ui.change_state(UI.State.GAME);

            Stage stage = Stage.get();
            Player player = stage.get_player();

            new PlayerLoginPacket(player.get_oid()).dispatch();

            int mapid = player.get_stats().get_mapid();
            byte portalid = player.get_stats().get_portal();

            stage.load(mapid, (sbyte)portalid);
            stage.transfer_player();

            TestURPBatcher.Instance.Clear();

            ui.enable();
            Timer.get().start();
            //todo 2 GraphicsGL.get ().unlock ();
        }

        public override void Dispose ()
        {
            base.Dispose ();
            foreach (var sprite in preview_sprites)
            {
                sprite.Dispose ();
            }
            foreach (var sprite in menu_tabs)
            {
                sprite.Dispose ();
            }
            foreach (var sprite in promotion_sprites)
            {
                sprite.Dispose ();
            }
            foreach (var sprite in mvp_sprites)
            {
                sprite.Dispose ();
            }
            charge_charset.Dispose ();
            item_line?.Dispose ();
            item_base?.Dispose ();
            item_none?.Dispose ();
            foreach (var sprite in item_labels)
            {
                sprite.Dispose ();
            }
            foreach (var sprite in items)
            {
                sprite.Dispose ();
            }
            list_slider?.Dispose ();
        }

        private void update_items()
        {
            for (uint i = 0; i < MAX_ITEMS; i++)
            {
                short index = (short)(i + list_offset);
                bool found_item = index < items.Count;

                buttons[(ushort)((int)Buttons.BtBuy + i)].set_active(found_item);

                string name = "";
                string price_text = "";
                string discount_text = "";
                string percent_text = "";

                if (found_item)
                {
                    Item item = items[index];

                    name = item.get_name();

                    int price = item.get_price();
                    price_text = Convert.ToString(price);

                    if (item.discount_price > 0 && price > 0)
                    {
                        discount_text = price_text;

                        uint discount = (uint)item.discount_price;
                        price_text = Convert.ToString(discount);

                        var percent = (float)discount / price;
                        string percent_str = Convert.ToString(percent);
                        percent_text = "(" + percent_str.Substring(2, 1) + "%)";
                    }

                    string_format.split_number(price_text);
                    string_format.split_number(discount_text);

                    price_text += " NX";

                    if (discount_text != "")
                    {
                        discount_text += " NX";
                    }

                    if (item.count > 0)
                    {
                        price_text += "(" + Convert.ToString(item.count) + ")";
                    }
                }

                item_name[i].change_text(name);
                item_price[i].change_text(price_text);
                item_discount[i].change_text(discount_text);
                item_percent[i].change_text(percent_text);

                string_format.format_with_ellipsis(item_name[i], 92);
            }
        }

        private const byte MAX_ITEMS = (byte)(7u * 2u + 1u);

        private class Item:IDisposable
        {
            public enum Label : byte
            {
                ACTION,
                BOMB_SALE,
                BONUS,
                EVENT = 4,
                HOT,
                LIMITED,
                LIMITED_BRONZE,
                LIMITED_GOLD,
                LIMITED_SILVER,
                LUNA_CRYSTAL,
                MASTER = 12,
                MUST,
                NEW,
                SALE = 17,
                SPEICAL,
                SPECIAL_PRICE,
                TIME,
                TODAY,
                WEEKLY,
                WONDER_BERRY,
                WORLD_SALE,
                NONE
            }

            public Item(int itemid, Label label, int discount, ushort count)
            {
                this.label = label;
                this.discount_price = discount;
                this.count = count;
                this.data = ItemData.get(itemid);
            }

            public Label label;
            public int discount_price;
            public ushort count;

            public void draw(DrawArgument args)
            {
                data.get_icon(false).draw(args);
            }

            public string get_name()
            {
                return data.get_name();
            }

            public int get_price()
            {
                return data.get_price();
            }

            private readonly ItemData data;
            public void Dispose ()
            {
                
            }
        }

        private enum Buttons : ushort
        {
            BtPreview1,
            BtPreview2,
            BtPreview3,
            BtExit,
            BtChargeNX,
            BtChargeRefresh,
            BtWish,
            BtMileage,
            BtHelp,
            BtCoupon,
            BtNext,
            BtPrev,
            BtDetailPackage,
            BtNonGrade,
            BtBuyAvatar,
            BtDefaultAvatar,
            BtInventory,
            BtSaveAvatar,
            BtTakeoffAvatar,
            BtBuy
        }

        private Point_short BestNew_dim = new Point_short();

        private Sprite[] preview_sprites = new Sprite[3];
        private byte preview_index;

        private Sprite[] menu_tabs = new Sprite[9];
        private byte menu_index;

        private Text job_label = new Text();
        private Text name_label = new Text();

        private List<Sprite> promotion_sprites = new List<Sprite>();
        private Point_short promotion_pos = new Point_short();
        private sbyte promotion_index;

        private Sprite[] mvp_sprites = new Sprite[7];
        private Point_short mvp_pos = new Point_short();
        private byte mvp_grade;
        private Gauge mvp_gauge = new Gauge();
        private float mvp_exp;

        private Charset charge_charset = new Charset();

        private Sprite item_base = new Sprite();
        private Sprite item_line = new Sprite();
        private Sprite item_none = new Sprite();
        private List<Sprite> item_labels = new List<Sprite>();
        private List<Item> items = new List<Item>();
        private Text[] item_name = new Text[MAX_ITEMS];
        private Text[] item_price = new Text[MAX_ITEMS];
        private Text[] item_discount = new Text[MAX_ITEMS];
        private Text[] item_percent = new Text[MAX_ITEMS];

        private Slider list_slider = new Slider();
        private short list_offset;
    }
}


#if USE_NX
#endif