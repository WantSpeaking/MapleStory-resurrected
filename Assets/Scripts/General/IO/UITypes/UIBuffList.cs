#define USE_NX

using System.Collections.Generic;
using MapleLib.WzLib;
using System.Linq;





namespace ms
{
    public class BuffIcon
    {
        public BuffIcon(int buff, int dur)
        {
            this.cover = new ms.IconCover(IconCover.Type.BUFF, dur - FLASH_TIME);
            buffid = buff;
            duration = dur;
            opacity.set(1.0f);
            opcstep = -0.05f;

            if (buffid >= 0)
            {
                string strid = string_format.extend_id(buffid, 7);
                WzObject src = ms.wz.wzFile_skill[$"{strid.Substring(0, 3)}.img"]["skill"][strid];
                icon = src?["icon"];
            }
            else
            {
                icon = new Texture(ItemData.get(-buffid).get_icon(true));
            }
        }

        public void draw(Point_short position, float alpha)
        {
            icon.draw(new DrawArgument(new Point_short(position), opacity.get(alpha)));
            cover.draw(position + new Point_short(1, -31), alpha);
        }

        public bool update()
        {
            if (duration <= FLASH_TIME)
            {
                opacity += opcstep;

                bool fadedout = opcstep < 0.0f && opacity.last() <= 0.0f;
                bool fadedin = opcstep > 0.0f && opacity.last() >= 1.0f;

                if (fadedout || fadedin)
                {
                    opcstep = -opcstep;
                }
            }

            cover.update();

            duration -= Constants.TIMESTEP;

            return duration < Constants.TIMESTEP;
        }

        private const ushort FLASH_TIME = 3_000;

        private Texture icon;
        private IconCover cover;
        private int buffid;
        private int duration;
        private Linear_float opacity = new Linear_float();
        private float opcstep;
    }


    public class UIBuffList : UIElement
    {
        public const Type TYPE = UIElement.Type.BUFFLIST;
        public const bool FOCUSED = false;
        public const bool TOGGLED = false;

        public UIBuffList()
        {
            short height = Constants.get().get_viewheight();
            short width = Constants.get().get_viewwidth();

            update_screen(width, height);
        }

        public override void draw(float alpha)
        {
            Point_short icpos = new Point_short(position);

            foreach (var icon in icons)
            {
                icon.Value.draw(icpos, alpha);
                icpos.shift_x(-32);
            }
        }

        private readonly List<KeyValuePair<int, BuffIcon>> cache = new List<KeyValuePair<int, BuffIcon>>();

        public override void update()
        {
            cache.Clear();
            foreach (var pair in icons)
            {
                if (pair.Value.update())
                {
                    cache.Add(pair);
                }
            }

            foreach (var pair in cache)
            {
                icons.Remove(pair.Key);
            }
        }

        public override void update_screen(short new_width, short new_height)
        {
            position = new Point_short((short)(new_width - 35), 55);
            dimension = new Point_short(position.x(), 32);
        }

        public override Cursor.State send_cursor(bool pressed, Point_short cursorposition)
        {
            return base.send_cursor(pressed, new Point_short(cursorposition));
        }

        public override UIElement.Type get_type()
        {
            return TYPE;
        }

        public void add_buff(int skillid, int duration)
        {
            icons.TryAdd(skillid, new BuffIcon(skillid, duration), true);
        }

        private Dictionary<int, BuffIcon> icons = new Dictionary<int, BuffIcon>();
    }
}


#if USE_NX
#endif