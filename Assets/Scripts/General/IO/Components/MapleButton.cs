


using System;
using System.Linq;
using MapleLib.WzLib;
using provider;

namespace ms
{
    // A standard MapleStory button with 4 states and a texture for each state
    public class MapleButton : Button
    {
        public MapleButton(Texture normal, Texture pressed, Texture mouseOver, Texture disabled) : this(normal, pressed, mouseOver, disabled, Point_short.zero)
        {

        }
        public MapleButton(Texture normal, Texture pressed, Texture mouseOver, Texture disabled, Point_short pos)
        {
            textures[(int)Button.State.NORMAL] = normal;
            textures[(int)Button.State.PRESSED] = pressed;
            //textures[(int)Button.State.DOWN] = pressed;
            textures[(int)Button.State.MOUSEOVER] = mouseOver;
            textures[(int)Button.State.DISABLED] = disabled;

            active = true;
            position = new Point_short(pos);
            state = Button.State.NORMAL;
        }

        public MapleButton(WzObject src, Point_short pos)
        {
            srcCache = src;
            if (src == null)
            {
                AppDebug.Log($"MapleButton(src):{src}");
                animations[(int)Button.State.NORMAL] = new Animation();
                return;
            }

            WzObject normal = src["normal"];

            if (normal.Count() > 1)
            {
                animations[(int)Button.State.NORMAL] = normal;
            }
            else
            {
                textures[(int)Button.State.NORMAL] = normal["0"];
            }

            textures[(int)Button.State.PRESSED] = src["pressed"]?["0"];
            //textures[(int)Button.State.MOUSEOVER] = src["pressed"]?["0"];
            textures[(int)Button.State.MOUSEOVER] = src["mouseOver"]?["0"]; //todo 2 mouseOver has _inlink property which can't be read now
            textures[(int)Button.State.DISABLED] = src["disabled"]?["0"];

            active = true;
            position = new Point_short(pos);
            state = Button.State.NORMAL;
        }
        public MapleButton(MapleData src, Point_short pos)
        {
            srcCache_MapleData = src;
            if (src == null)
            {
                AppDebug.Log($"MapleButton(src):{src}");
                animations[(int)Button.State.NORMAL] = new Animation();
                return;
            }

            var normal = src["normal"];

            if (normal.Count() > 1)
            {
                animations[(int)Button.State.NORMAL] = normal;
            }
            else
            {
                textures[(int)Button.State.NORMAL] = normal["0"];
            }

            textures[(int)Button.State.PRESSED] = src["pressed"]?["0"];
            //textures[(int)Button.State.MOUSEOVER] = src["pressed"]?["0"];
            textures[(int)Button.State.MOUSEOVER] = src["mouseOver"]?["0"]; //todo 2 mouseOver has _inlink property which can't be read now
            textures[(int)Button.State.DISABLED] = src["disabled"]?["0"];

            active = true;
            position = new Point_short(pos);
            state = Button.State.NORMAL;
        }
        public MapleButton(WzObject src, short x, short y) : this(src, new Point_short(x, y))
        {
        }

        public MapleButton(WzObject src) : this(src, new Point_short())
        {
        }

        public MapleButton(Point_short pos, int downEffect, float downEffectValue, Texture tex_BG = null, Texture tex_FG = null, Icon icon_FG = null, Point_short offset_FG = null)
        {
            active = true;
            position = new Point_short(pos);
            state = Button.State.NORMAL;
            this.downEffect = downEffect;
            this.downEffectValue = downEffectValue;
            this.downColor = new Color(downEffectValue, downEffectValue, downEffectValue, 1f);
            this.tex_BG = tex_BG;
            this.Tex_FG = tex_FG;
            this.Icon_FG = icon_FG;
            this.offset_FG = offset_FG;
        }

        public override void draw(Point_short parentpos)
        {
            if (active)
            {
                switch (downEffect)
                {
                    case 0:
                        tex_BG?.draw(position + parentpos);

                        textures[(int)state]?.draw(position + parentpos);
                        animations[(int)state]?.draw(position + parentpos, 1.0f);
                        break;
                    case 1:
                    case 2:
                        DrawArgument arg;

                        if (state != State.PRESSED)
                        {
                            arg = new DrawArgument(position + parentpos);
                        }
                        else
                        {
                            if (downEffect == 1)
                            {
                                arg = new DrawArgument(position + parentpos, downColor);
                            }
                            else
                            {
                                arg = new DrawArgument(position + parentpos, downEffectValue, downEffectValue);
                            }
                        }

                        tex_BG?.draw(arg);
                        tex_FG?.draw(arg + offset_FG);
                        icon_FG?.draw(arg + offset_FG);
                        break;
                }

            }
        }

        public override void update()
        {
            if (active)
            {
                animations[(int)state]?.update(6);
            }
        }

        public override Rectangle_short bounds(Point_short parentpos)
        {
            Point_short lt = new Point_short();
            Point_short rb = new Point_short();

            switch (downEffect)
            {
                case 0:
                    if (textures[(int)state]?.is_valid() ?? false)
                    {
                        lt = parentpos + position - textures[(int)state].get_origin();
                        rb = lt + textures[(int)state].get_dimensions();
                    }
                    else
                    {
                        lt = parentpos + position - animations[(int)state]?.get_origin()??Point_short.zero;
                        rb = lt + animations[(int)state]?.get_dimensions() ?? Point_short.zero;
                    }
                    break;
                case 1:
                case 2:
                    lt = parentpos + position - tex_BG.get_origin();
                    rb = lt + tex_BG.get_dimensions();
                    break;
            }

            return new Rectangle_short(new Point_short(lt), new Point_short(rb));
        }

        public override short width()
        {
            return textures[(int)state].width();
        }

        public override Point_short origin()
        {
            return textures[(int)state].get_origin();
        }

        public override Cursor.State send_cursor(bool UnnamedParameter1, Point_short UnnamedParameter2)
        {
            return Cursor.State.IDLE;
        }

        private Texture[] textures = new Texture[(int)Button.State.NUM_STATES];
        private Animation[] animations = new Animation[(int)Button.State.NUM_STATES];

        private Texture tex_BG;
        private Texture tex_FG;
        private Icon icon_FG;

        /// <summary>
        /// 0:multi texture; 1:one texture with different color;2:one texture with scale
        /// </summary>
        private int downEffect;
        private float downEffectValue;
        private bool downScaled;
        private Color downColor;
        private Point_short offset_FG = Point_short.zero;

        public int DownEffect { get => downEffect; set => downEffect = value; }
        public float DownEffectValue { get => downEffectValue; set => downEffectValue = value; }
        public bool DownScaled { get => downScaled; set => downScaled = value; }
        public Color DownColor { get => downColor; set => downColor = value; }
        public Texture Tex_BG { get => tex_BG; set => tex_BG = value; }
        public Texture Tex_FG { get => tex_FG; set => tex_FG = value; }

        public Icon Icon_FG { get => icon_FG; set => icon_FG = value; }

        public Point_short Offset_FG { get => offset_FG; set => offset_FG = value; }

        public override void Dispose ()
        {
            base.Dispose ();
            //AppDebug.Log("MapleButton Dispose");

            foreach (var t in textures)
            {
                t?.Dispose ();
            }
            foreach (var a in animations)
            {
                a?.Dispose ();
            }
            Tex_BG?.Dispose ();
            Tex_FG?.Dispose ();
            Icon_FG?.Dispose ();
        }


        #region to be removed

        private WzObject srcCache;
        private MapleData srcCache_MapleData;



        public override string ToString()
        {
            return $"MapleButton:[{srcCache?.Name}]";
        }

        #endregion
    }
}