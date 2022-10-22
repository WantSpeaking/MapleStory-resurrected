using System;
using System.Collections.Generic;
using Beebyte.Obfuscator;
using client;
using MapleLib.WzLib;
using ms_Unity;
using server.quest;

namespace ms
{
    [Skip]
    public class UIQuestLog : UIDragElement<PosQUEST>
    {
        public const Type TYPE = UIElement.Type.QUESTLOG;
        public const bool FOCUSED = false;
        public const bool TOGGLED = true;

        public UIQuestLog(params object[] args) : this((QuestLog)args[0])
        {
        }

        public UIQuestLog(QuestLog ql)
        {
            this.questlog = ql;
            tab = (int)Buttons.TAB0;

            WzObject close = ms.wz.wzFile_ui["Basic.img"]["BtClose3"];
            WzObject quest = ms.wz.wzFile_ui["UIWindow2.img"]["Quest"];
            WzObject list = quest["list"];

            WzObject backgrnd = list["backgrnd"];

            sprites.Add(backgrnd);
            //sprites.Add(list["backgrnd2"]);

            notice_sprites.Add(list["notice0"]);
            notice_sprites.Add(list["notice1"]);
            notice_sprites.Add(list["notice2"]);

            WzObject taben = list["Tab"]["enabled"];
            WzObject tabdis = list["Tab"]["disabled"];

            buttons[(int)Buttons.TAB0] = new TwoSpriteButton(tabdis["0"], taben["0"]);
            buttons[(int)Buttons.TAB1] = new TwoSpriteButton(tabdis["1"], taben["1"]);
            buttons[(int)Buttons.TAB2] = new TwoSpriteButton(tabdis["2"], taben["2"]);
            buttons[(int)Buttons.CLOSE] = new MapleButton(close, new Point_short(275, 6));
            buttons[(int)Buttons.SEARCH] = new MapleButton(list["BtSearch"]);
            buttons[(int)Buttons.ALL_LEVEL] = new MapleButton(list["BtAllLevel"]);
            buttons[(int)Buttons.MY_LOCATION] = new MapleButton(list["BtMyLocation"]);

            search_area = list["searchArea"];
            var search_area_dim = search_area.get_dimensions();
            var search_area_origin = search_area.get_origin().abs();

            var search_pos_adj = new Point_short(29, 0);
            var search_dim_adj = new Point_short(-80, 0);

            var search_pos = position + search_area_origin + search_pos_adj;
            var search_dim = search_pos + search_area_dim + search_dim_adj;

            search = new Textfield(Text.Font.A15B, Text.Alignment.LEFT, Color.Name.BLACK, new Rectangle_short(search_pos, search_dim), 19);
            placeholder = new Text(Text.Font.A15B, Text.Alignment.LEFT, Color.Name.BLACK, "Enter the quest name.");

            slider = new Slider((int)Slider.Type.DEFAULT_SILVER, new Range_short(0, 279), 150, ROWS, ROWS, (bool upwards) =>
            {
                /*short shift = (short)(upwards ? -1 : 1);
                bool above = offset + shift >= 0;
                bool below = offset + shift <= event_count - 3;

                if (above && below)
                {
                    offset += shift;
                }*/

                short shift = (short)(upwards ? -COLUMNS : COLUMNS);
                bool above = slotrange[(Buttons)tab].Item1 + shift > 0;
                bool below = slotrange[(Buttons)tab].Item2 + shift < ROWS + 1 + COLUMNS;

                if (above && below)
                {
                    var oldValue = slotrange[(Buttons)tab];
                    oldValue.Item1 += shift;
                    oldValue.Item2 += shift;
                    slotrange[(Buttons)tab] = oldValue;
                }
            });

            change_tab(tab);

            dimension = new Texture(backgrnd).get_dimensions();
            dragarea = new Point_short(dimension.x(), 20);
            sprites.Add(new Sprite(list["backgrnd2"], new Point_short(0, 0)));
            sprites.Add(new Sprite(list["backgrnd2"], new Point_short(dimension.x(), 0)));
            questInfo_position = new Point_short((short) (dimension.x() + 10), 80);

            text_QuestEntryName = new Text(Text.Font.A13M, Text.Alignment.LEFT, Color.Name.BLACK, "");
            text_Info = new Text(Text.Font.A13M, Text.Alignment.LEFT, Color.Name.BLACK, "",300);

            slotrange[Buttons.TAB0] = new ValueTuple<short, short>(0, 9);
            slotrange[Buttons.TAB1] = new ValueTuple<short, short>(0, 9);
            slotrange[Buttons.TAB2] = new ValueTuple<short, short>(0, 9);

        }

        Point_short notice_position = new Point_short(0, 26);
        Point_short questEntry_position = new Point_short(9, 80);
        Point_short questInfo_position;
        public override void draw(float alpha)
        {
            base.draw_sprites(alpha);

            if (tab == (int)Buttons.TAB0)
            {
                if (MapleCharacter.Player.getStartedQuests ().Count > 0)
                {
                    var index = 0;
                    var range = slotrange[(Buttons)tab];
                    uint firstslot = (uint)(range.Item1);
                    uint lastslot = (uint)range.Item2;
                    foreach (var pair in MapleCharacter.Player.getStartedQuests ())
                    {
                        if (index >= firstslot && index <= lastslot)
                        {
                            var questName = MapleQuest.getInstance (pair.QuestID).Name;
                            text_QuestEntryName.change_text(questName);
                            var tempPoint = position + questEntry_position + new Point_short(0, (short)(index * 30));
                            //AppDebug.Log($"{questName}:{tempPoint}");
                            text_QuestEntryName.draw(position + questEntry_position + new Point_short(9, (short)(index * ICON_HEIGHT)));
                        }

                        index++;
                    }

                    event_count = (short)MapleCharacter.Player.getStartedQuests ().Count;
                    for (uint i = 0; i < ROWS; i++)
                    {
                        short slot = (short)(i + offset);

                        if (slot >= event_count)
                        {
                            break;
                        }

                        var event_pos = new Point_short(9, (short)(87 + 30 * i));

                    }
                }
                else
                {
                    notice_sprites[tab].draw(position + notice_position + new Point_short(9, 0), alpha);
                }

                if (currentSelected_questId != -1)
                {
                    var questInfo = MapleQuest.getInstance (currentSelected_questId);
                    text_Info.change_text(questInfo.Info_started);
                    text_Info.draw(position + questInfo_position);
                }
            }
            else if (tab == (int)Buttons.TAB1)
            {
                notice_sprites[tab].draw(position + notice_position + new Point_short(0, 0), alpha);
            }
            else
            {
                notice_sprites[tab].draw(position + notice_position + new Point_short(-10, 0), alpha);
            }

            if (tab != (int)Buttons.TAB2)
            {
                search_area.draw(position);
                search.draw(new Point_short(0, 0));

                if (search.get_state() == Textfield.State.NORMAL && search.empty())
                {
                    placeholder.draw(position + new Point_short(39, 51));
                }
            }

            slider.draw(position + new Point_short(126, 75));

            base.draw_buttons(alpha);
        }

        public override void send_key(int keycode, bool pressed, bool escape)
        {
            if (pressed)
            {
                if (escape)
                {
                    deactivate();
                }
                else if (keycode == (int)KeyAction.Id.TAB)
                {
                    ushort new_tab = tab;

                    if (new_tab < (int)Buttons.TAB2)
                    {
                        new_tab++;
                    }
                    else
                    {
                        new_tab = (int)Buttons.TAB0;
                    }

                    change_tab(new_tab);
                }
            }
        }

        public override void update()
        {
            base.update();

            if (event_count != MapleCharacter.Player.getStartedQuests ().Count)
            {
                event_count = (short)MapleCharacter.Player.getStartedQuests ().Count;
                slider.setrows(ROWS, event_count);
            }
        }

        private short currentSelected_questId = -1;
        public override Cursor.State send_cursor(bool clicking, Point_short cursorpos)
        {
            Cursor.State new_state = search.send_cursor(new Point_short(cursorpos), clicking);
            if (new_state != Cursor.State.IDLE)
            {
                return new_state;
            }

            Point_short cursor_relative = cursorpos - position;
            Cursor.State sstate = slider.send_cursor(new Point_short(cursor_relative), clicking);

            if (sstate != Cursor.State.IDLE)
            {
                return sstate;
            }

            short slot = slot_by_position(new Point_short(cursor_relative));
            var questId = get_QuestId(slot);
            bool is_icon = is_visible(slot);
            if (is_icon)
            {
                if (clicking)
                {
                    currentSelected_questId = questId;
                }
            }
            return base.send_cursor(clicking, new Point_short(cursorpos));
        }

        public override void OnActivityChange (bool isActiveAfterChange)
        {
            if (isActiveAfterChange)
            {
                ms_Unity.FGUI_Manager.Instance.OpenFGUI<ms_Unity.FGUI_QuestLog> ().OnVisiblityChanged (true);
            }
            else
            {
                ms_Unity.FGUI_Manager.Instance.CloseFGUI<ms_Unity.FGUI_QuestLog> ().OnVisiblityChanged (false);
            }
        }

        private short offset;
        private short event_count;
        private const short ROWS = 8;
        private const ushort COLUMNS = 1;
        private const ushort MAXSLOTS = ROWS * COLUMNS;
        private const ushort MAXFULLSLOTS = COLUMNS * MAXSLOTS;
        private const ushort ICON_WIDTH = 80;
        private const ushort ICON_HEIGHT = 30;
        private SortedDictionary<Buttons, System.ValueTuple<short, short>> slotrange = new SortedDictionary<Buttons, System.ValueTuple<short, short>>();
        private SortedDictionary<int, MapleQuest> icons = new SortedDictionary<int, MapleQuest>();
        private List<short> questIds_Started = new List<short>();
       
        
        private short get_QuestId(short slot)
        {
            var index = 0;
            foreach (var pair in MapleCharacter.Player.getStartedQuests ())
            {
                if (index == slot)
                {
                    return pair.QuestID;
                }
                index++;
            }

            return -1;
        }
        private short slot_by_position(Point_short cursorpos)
        {
            short xoff = (short)(cursorpos.x() - 9);
            short yoff = (short)(cursorpos.y() - 80);

            /*  if (xoff < 1 || xoff > 143 || yoff < 1)
              {
                  return 0;
              }*/

            short slot = (short)((slotrange[(Buttons)tab].Item1) /*+ (xoff / ICON_WIDTH)*/ + COLUMNS * (yoff / ICON_HEIGHT));

            return (short)(is_visible(slot) ? slot : 0);
        }

        private bool is_visible(short slot)
        {
            var range = slotrange[(Buttons)tab];

            return slot >= range.Item1 && slot <= range.Item2;
        }

        private Point_short get_slotpos(short slot)
        {
            short absslot = (short)(slot - slotrange[(Buttons)tab].Item1);

            var x = (short)(10 + (absslot % COLUMNS) * ICON_WIDTH);//-26
            var y = (short)(51 + (absslot / COLUMNS) * ICON_HEIGHT);//51
            return new Point_short((short)(10 + (absslot % COLUMNS) * ICON_WIDTH), (short)(51 + (absslot / COLUMNS) * ICON_HEIGHT));
        }

        
        public override UIElement.Type get_type()
        {
            return TYPE;
        }

        public override Button.State button_pressed(ushort buttonid)
        {
            switch ((Buttons)buttonid)
            {
                case Buttons.TAB0:
                case Buttons.TAB1:
                case Buttons.TAB2:
                    change_tab(buttonid);

                    return Button.State.IDENTITY;
                case Buttons.CLOSE:
                    deactivate();
                    break;
                default:
                    break;
            }

            return Button.State.NORMAL;
        }

        private void change_tab(ushort tabid)
        {
            ushort oldtab = tab;
            tab = tabid;

            if (oldtab != tab)
            {
                buttons[(uint)((int)Buttons.TAB0 + oldtab)].set_state(Button.State.NORMAL);
                buttons[(int)Buttons.MY_LOCATION].set_active(tab == (int)Buttons.TAB0);
                buttons[(int)Buttons.ALL_LEVEL].set_active(tab == (int)Buttons.TAB0);
                buttons[(int)Buttons.SEARCH].set_active(tab != (int)Buttons.TAB2);

                if (tab == (int)Buttons.TAB2)
                {
                    search.set_state(Textfield.State.DISABLED);
                }
                else
                {
                    search.set_state(Textfield.State.NORMAL);
                }
            }

            buttons[(uint)((int)Buttons.TAB0 + tab)].set_state(Button.State.PRESSED);
        }

        private enum Buttons : ushort
        {
            TAB0,
            TAB1,
            TAB2,
            CLOSE,
            SEARCH,
            ALL_LEVEL,
            MY_LOCATION
        }

        private readonly QuestLog questlog;

        private ushort tab;
        private List<Sprite> notice_sprites = new List<Sprite>();
        private Textfield search = new Textfield();
        private Text placeholder = new Text();
        private Slider slider = new Slider();
        private Texture search_area = new Texture();

        private Point drawRange;
        private Text text_QuestEntryName;
        private Text text_Info;
        public class QuestButton : Button
        {
            public override Rectangle_short bounds(Point_short parentpos)
            {
                throw new System.NotImplementedException();
            }

            public override void draw(Point_short parentpos)
            {
                throw new System.NotImplementedException();
            }

            public override Point_short origin()
            {
                throw new System.NotImplementedException();
            }

            public override Cursor.State send_cursor(bool clicked, Point_short cursorpos)
            {
                throw new System.NotImplementedException();
            }

            public override void update()
            {
                throw new System.NotImplementedException();
            }

            public override short width()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}