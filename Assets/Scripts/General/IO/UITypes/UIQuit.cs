#define USE_NX

using System;
using Beebyte.Obfuscator;
using MapleLib.WzLib;




namespace ms
{
    [Skip]
    public class UIQuit : UIElement
    {
        public const Type TYPE = UIElement.Type.QUIT;
        public const bool FOCUSED = true;
        public const bool TOGGLED = false;

        public UIQuit(params object[] args) : this((CharStats)args[0])
        {
        }

        public UIQuit(CharStats st)
        {
            /*this.screen_adj = new ms.Point_short(212, 114);
            this.stats = st;
            WzObject askReward = ms.wz.wzFile_ui["UIWindow6.img"]["askReward"];
            WzObject userLog = askReward["userLog"];
            WzObject exp = userLog["exp"];
            WzObject level = userLog["level"];
            WzObject time = userLog["time"];
            WzObject backgrnd = userLog["backgrnd"];

            sprites.Add(new Sprite(backgrnd, -screen_adj));

            buttons[(int)Buttons.NO] = new MapleButton(askReward["btNo"], new Point_short(0, 37));
            buttons[(int)Buttons.YES] = new MapleButton(askReward["btYes"], new Point_short(0, 37));

            Stage stage = Stage.get();

            /// Time
            long uptime = stage.get_uptime() / 1000 / 1000;
            minutes = uptime / 60;
            hours = minutes / 60;

            minutes -= hours * 60;

            time_minutes = new Charset(time["number"], Charset.Alignment.LEFT);
            time_minutes_pos = time["posM"];
            time_minutes_text = pad_time(minutes);

            time_hours = new Charset(time["number"], Charset.Alignment.LEFT);
            time_hours_pos = time["posH"];
            time_hours_text = pad_time(hours);

            time_number_width = time["numberWidth"];

            time_lt = time["tooltip"]["lt"];
            time_rb = time["tooltip"]["rb"];

            /// Level
            levelupEffect = level["levelupEffect"];

            uplevel = stage.get_uplevel();

            levelBefore = new Charset(level["number"], Charset.Alignment.LEFT);
            levelBeforePos = level["posBefore"];
            levelBeforeText = Convert.ToString(uplevel);

            cur_level = stats.get_stat(MapleStat.Id.LEVEL);

            levelAfter = new Charset(level["number"], Charset.Alignment.LEFT);
            levelAfterPos = level["posAfter"];
            levelAfterText = Convert.ToString(cur_level);

            levelNumberWidth = level["numberWidth"];

            level_adj = new Point_short(40, 0);

            /// Experience
            long upexp = stage.get_upexp();
            float expPercentBefore = getexppercent(uplevel, upexp);
            string expBeforeString = Convert.ToString(100 * expPercentBefore);
            //string expBeforeText = expBeforeString.Substring (0, expBeforeString.IndexOf ('.') + 3) + '%';//todo2 indexOutOfRange
            string expBeforeText = expBeforeString;

            expBefore = new Text(Text.Font.A11M, Text.Alignment.LEFT, Color.Name.WHITE, expBeforeText);
            expBeforePos = exp["posBefore"];

            long cur_exp = stats.get_exp();
            float expPercentAfter = getexppercent(cur_level, cur_exp);
            string expAfterString = Convert.ToString(100 * expPercentAfter);
            //string expAfterText = expAfterString.Substring (0, expAfterString.IndexOf ('.') + 3) + '%';//todo2 indexOutOfRange
            string expAfterText = expAfterString;

            expAfter = new Text(Text.Font.A11M, Text.Alignment.LEFT, Color.Name.ELECTRICLIME, expAfterText);
            expAfterPos = exp["posAfter"];

            exp_adj = new Point_short(0, 6);

            short width = Constants.get().get_viewwidth();
            short height = Constants.get().get_viewheight();

            background = new ColorBox(width, height, Color.Name.BLACK, 0.5f);
            position = new Point_short((short)(width / 2), (short)(height / 2));
            dimension = new Texture(backgrnd).get_dimensions();*/
        }

        //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void draw(float inter) const override
        public override void draw(float inter)
        {
            /*background.draw(new Point_short(0, 0));

            base.draw(inter);

            time_minutes.draw(time_minutes_text, (short)time_number_width, position + time_minutes_pos - screen_adj);
            time_hours.draw(time_hours_text, (short)time_number_width, position + time_hours_pos - screen_adj);

            levelBefore.draw(levelBeforeText, (short)levelNumberWidth, position + levelBeforePos + level_adj - screen_adj);
            levelAfter.draw(levelAfterText, (short)levelNumberWidth, position + levelAfterPos + level_adj - screen_adj);

            if (cur_level > uplevel)
            {
                levelupEffect.draw(position - screen_adj, inter);
            }

            expBefore.draw(position + expBeforePos - exp_adj - screen_adj);
            expAfter.draw(position + expAfterPos - exp_adj - screen_adj);*/
        }

        public override void update()
        {
            /*base.update();

            levelupEffect.update();*/
        }

        public override Cursor.State send_cursor(bool clicked, Point_short cursorpos)
        {
            var lt = position + time_lt - screen_adj;
            var rb = position + time_rb - screen_adj;

            var bounds = new Rectangle_short(lt, rb);

            if (bounds.contains(cursorpos))
            {
                UI.get().show_text(Tooltip.Parent.TEXT, Convert.ToString(hours) + "Hour " + Convert.ToString(minutes) + "Minute");
            }
            else
            {
                UI.get().clear_tooltip(Tooltip.Parent.TEXT);
            }

            return base.send_cursor(clicked, new Point_short(cursorpos));
        }

        public override void send_key(int keycode, bool pressed, bool escape)
        {
            if (pressed)
            {
                if (escape)
                {
                    close();
                }
                else if (keycode == (int)KeyAction.Id.RETURN)
                {
                    button_pressed((ushort)Buttons.YES);
                }
            }
        }

        //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: UIElement::Type get_type() const override
        public override UIElement.Type get_type()
        {
            return TYPE;
        }

        public override Button.State button_pressed(ushort buttonid)
        {
            switch ((Buttons)buttonid)
            {
                case Buttons.NO:
                    close();
                    break;
                case Buttons.YES:
                    {
                       MapleStory.Instance.BackToLogin ();
                    }
                    break;
                default:
                    break;
            }

            return Button.State.NORMAL;
        }

        private readonly CharStats stats;

        private string pad_time(long time)
        {
            string ctime = Convert.ToString(time);
            uint length = (uint)ctime.Length;

            if (length > 2)
            {
                return "99";
            }

            return (2 - length, '0') + ctime;
        }

        //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: float getexppercent(ushort level, long exp) const
        private float getexppercent(ushort level, long exp)
        {
            if (level >= ExpTable.LEVELCAP)
            {
                return 0.0f;
            }

            return (float)((double)exp / ExpTable.values[level]);
        }

        private void close()
        {
            deactivate();

            UI.get().clear_tooltip(Tooltip.Parent.TEXT);
        }

        private enum Buttons : ushort
        {
            NO,
            YES
        }

        private Point_short screen_adj = new Point_short();
        private ColorBox background = new ColorBox();

        /// Time
        private long minutes;

        private long hours;

        private Charset time_minutes = new Charset();
        private Point_short time_minutes_pos = new Point_short();
        private string time_minutes_text;

        private Charset time_hours = new Charset();
        private Point_short time_hours_pos = new Point_short();
        private string time_hours_text;

        private long time_number_width;

        private Point_short time_lt = new Point_short();
        private Point_short time_rb = new Point_short();

        /// Level
        private Sprite levelupEffect = new Sprite();

        private ushort uplevel;

        private Charset levelBefore = new Charset();
        private Point_short levelBeforePos = new Point_short();
        private string levelBeforeText;

        private ushort cur_level;

        private Charset levelAfter = new Charset();
        private Point_short levelAfterPos = new Point_short();
        private string levelAfterText;

        private long levelNumberWidth;
        private Point_short level_adj = new Point_short();

        /// Experience
        private Text expBefore = new Text();

        private Point_short expBeforePos = new Point_short();

        private Text expAfter = new Text();
        private Point_short expAfterPos = new Point_short();

        private Point_short exp_adj = new Point_short();
    }
}


#if USE_NX
#endif