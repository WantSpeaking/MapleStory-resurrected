using System;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ms.Util;
//using SpriteFontPlus;
//using FairyGUI;





namespace ms
{
    public class Text : IDisposable
    {
        public enum Font
        {
            A11M,
            A11B,
            A12M,
            A12B,
            A13M,
            A13B,
            A14B,
            A15B,
            A18M,
            NUM_FONTS
        }

        public enum Alignment
        {
            LEFT,
            CENTER,
            RIGHT
        }

        public enum Background
        {
            NONE,
            NAMETAG
        }

        public class Layout : IEnumerable<Layout.Line>
        {
            public class Word
            {
                public uint first;
                public uint last;
                public Font font;
                public Color.Name color;

                public Word (uint _first, uint _last, Font _font, Color.Name _color)
                {
                    first = _first;
                    last = _last;
                    font = _font;
                    color = _color;
                }
            }

            public class Line
            {
                public List<Word> words = new List<Word> ();
                public Point_short position = new Point_short ();

                public Line (List<Word> _words, Point_short _position)
                {
                    words = _words;
                    position = _position;
                }
            }

            public Layout (List<Layout.Line> l, List<short> a, short w, short h, short ex, short ey)
            {
                this.lines = new List<Line> (l);
                this.advances = new List<short> (a);
                this.dimensions = new ms.Point_short (w, h);
                this.endoffset = new ms.Point_short (ex, ey);
            }
            public Layout () : this (new List<Layout.Line> (), new List<short> (), 0, 0, 0, 0)
            {
            }

            public short width ()
            {
                return dimensions.x ();
            }
            public short height ()
            {
                return dimensions.y ();
            }
            public short advance (uint index)
            {
                return (short)(index < advances.Count ? advances[(int)index] : 0);
            }
            public Point_short get_dimensions ()
            {
                return dimensions;
            }
            public Point_short get_endoffset ()
            {
                return endoffset;
            }

            /*public Text.Layout.iterator begin()
			{
				return lines.GetEnumerator();
			}
			public Text.Layout.iterator end()
			{
				return lines.end();
			}*/

            private List<Line> lines = new List<Line> ();
            private List<short> advances = new List<short> ();
            private Point_short dimensions = new Point_short ();
            private Point_short endoffset = new Point_short ();
            public IEnumerator<Line> GetEnumerator ()
            {
                return lines.GetEnumerator ();
            }

            IEnumerator IEnumerable.GetEnumerator ()
            {
                return GetEnumerator ();
            }
        }

        private static EnumMap<Font, SpriteFont> font_map = new EnumMap<Font, SpriteFont> ();
        public static SpriteFont Font_A11M;
        public static SpriteFont base_Font;
        public static SpriteFont base_BoldFont;
        private static float base_FontSize = 14;
        public static Microsoft.Xna.Framework.Color nameTagColor = new Microsoft.Xna.Framework.Color (0, 0, 0, 0.6f);
        private static float FontTypeToSize (Font fontType)
        {
            var result = base_FontSize;
            switch (fontType)
            {
                case Font.A11M:
                    result = 11;
                    break;
                case Font.A11B:
                    result = 11;
                    break;
                case Font.A12M:
                    result = 12;
                    break;
                case Font.A12B:
                    result = 12;
                    break;
                case Font.A13M:
                    result = 13;
                    break;
                case Font.A13B:
                    result = 13;
                    break;
                case Font.A14B:
                    result = 14;
                    break;
                case Font.A15B:
                    result = 15;
                    break;
                case Font.A18M:
                    result = 18;
                    break;
                case Font.NUM_FONTS:
                    break;
                default:
                    break;
            }
            return result;
        }
        /*public static void Init ()
        {
            base_Font = SpriteFontEX.InitChineseFont (GameUtil.Instance.GraphicsDevice, Constants.Instance.path_MapleStoryFolder, base_FontSize);
            base_BoldFont = SpriteFontEX.InitChineseFont (GameUtil.Instance.GraphicsDevice, Constants.Instance.path_MapleStoryFolder, base_FontSize, true);

            /*            font_map[Font.A11M] = SpriteFontEX.InitChineseFont(GameUtil.Instance.GraphicsDevice, Constants.Instance.path_MapleStoryFolder, base_FontSize);
                        font_map[Font.A11B] = SpriteFontEX.InitChineseFont(11, true);
                        font_map[Font.A12M] = SpriteFontEX.InitChineseFont(12);
                        font_map[Font.A12B] = SpriteFontEX.InitChineseFont(12, true);
                        font_map[Font.A13M] = SpriteFontEX.InitChineseFont(13);
                        font_map[Font.A13B] = SpriteFontEX.InitChineseFont(13, true);
                        font_map[Font.A14B] = SpriteFontEX.InitChineseFont(14, true);
                        font_map[Font.A15B] = SpriteFontEX.InitChineseFont(15, true);
                        font_map[Font.A18M] = SpriteFontEX.InitChineseFont(18);

                        Font_A11M = font_map[Font.A11M];#1#
        }*/
        public Text (Font f, Alignment a, Color.Name c, Background b, string t = "", ushort mw = 0, bool fm = true, short la = 0)
        {
            this.font = f;
            this.alignment = a;
            this.color = c;
            this.background = b;
            this.maxwidth = mw;
            this.formatted = fm;
            this.line_adj = la;

            CreateGRichText (t);

            change_text (t);
        }

        public Text (Font f, Alignment a, Color.Name c, string t = "", ushort mw = 0, bool fm = true, short la = 0) : this (f, a, c, Background.NONE, t, mw, fm, la)
        {
        }
        public Text () : this (Font.A11M, Alignment.LEFT, Color.Name.BLACK)
        {
        }

        public void draw (DrawArgument args)
        {
            draw (args, new Range_short (0, 0));
        }
        public void draw (DrawArgument args, Range_short vertical)
        {
            if (string.IsNullOrEmpty (text))
                return;

            var msColor = Color.colors[(int)color];

            var monoColor = new Microsoft.Xna.Framework.Color (msColor[0], msColor[1], msColor[2], 1f);

            var spriteFont = font.ToString ().Contains ("B") ? base_BoldFont : base_Font;

            var drawPosX = 0;
            var drawPosY = 0;

            if (useFguiText)
            {
                /*var transformedPos = ms.Window.get ().Point_VirtualToPhysics (args.getpos ().x (), args.getpos ().y ());

                drawPosX = transformedPos.X;
                drawPosY = transformedPos.Y;

                if (background == Background.NAMETAG)
                {
                    GraphicsGL.get ().drawrectangle (drawPosX, drawPosY, width (), height (), nameTagColor);
                }

                gRichTextField.SetPosition (drawPosX, drawPosY,0);
                if (!addedToGRoot)
                {
                    gRichTextField.richTextField.Draw (FairyGUI.Stage.inst.Batch);
                }*/
            }
            else
            {
                drawPosX = args.getpos ().x ();
                drawPosY = args.getpos ().y ();

                if (background == Background.NAMETAG)
                {
                    //GraphicsGL.get ().drawrectangle (drawPosX, drawPosY, width (), height (), nameTagColor);
                }
                GraphicsGL.get ().Batch.DrawString (spriteFont, @text, new Vector2 (drawPosX, drawPosY), monoColor, 0, Vector2.Zero, fontSizeScaler, SpriteEffects.None, 0);
            }
        }

        public void change_text (string t, bool setGRichText = true)
        {
            if (string.IsNullOrEmpty (t) || text == t)
            {
                return;
            }

            text = NpcTextParser.inst.Parse (t);

            reset_layout ();

            if (useFguiText && setGRichText)
            {
                gRichTextField.text = text;
            }

            //AppDebug.Log($@"change_text: {text}");//todo log change_text
        }
        public void change_color (Color.Name c)
        {
            if (color == c)
            {
                return;
            }

            color = c;

            reset_layout ();
        }
        public void set_background (Background b)
        {
            background = b;
        }

        public bool empty ()
        {
            return string.IsNullOrEmpty (text);
        }
        public uint length ()
        {
            return (uint)text.Length;
        }
        public short width ()
        {
            return (short)MaxWidth;
        }
        public short height ()
        {
            return useFguiText ? (short)gRichTextField.textHeight : layout.height ();
        }
        public ushort advance (uint pos)
        {
            return (ushort)layout.advance (pos);
        }
        public Point_short dimensions ()
        {
            return layout.get_dimensions ();
        }
        public Point_short endoffset ()
        {
            return layout.get_endoffset ();
        }
        public string get_text ()
        {
            return text;
        }

        private void reset_layout ()
        {
            if (string.IsNullOrEmpty (text))
            {
                return;
            }

            //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: layout = GraphicsGL::get().createlayout(text, font, alignment, maxwidth, formatted, line_adj);
            //todo 2 layout=(GraphicsGL.get().createlayout(text, font, alignment, maxwidth, formatted, line_adj));
            //layout = new Layout(GraphicsGL.get().createlayout(text, font, alignment, maxwidth, formatted, line_adj));//todo 2 text reset_layout
        }

        private Font font;
        private Alignment alignment;
        private Color.Name color;
        private Background background;
        private Layout layout = new Layout ();
        private ushort maxwidth;
        private bool formatted;
        private string text;
        private short line_adj;

        public ushort MaxWidth
        {
            get => (ushort)(maxwidth == 0 ? GetTextLength (text) : maxwidth);
            set => maxwidth = value;
        }

        private GRichTextField gRichTextField;

        private float fontSizeScaler => FontTypeToSize (font) / base_FontSize;
        private float letterSpacingScaler = 1.2f;
        public bool useFguiText = false;
        public bool addedToGRoot = false;

        public void CreateGRichText (string t)
        {
            useFguiText = true;

            gRichTextField = new GRichTextField ();

            gRichTextField.textFormat.size = (int)FontTypeToSize (font);
            var msColor = Color.colors[(int)color];
            var monoColor = new Microsoft.Xna.Framework.Color (msColor[0], msColor[1], msColor[2], 1f);
            gRichTextField.textFormat.color = new UnityEngine.Color(monoColor.R,monoColor.G,monoColor.B,monoColor.A);
            //gRichTextField.textFormat.align = (AlignType)alignment;
            gRichTextField.align = (AlignType)alignment;
            if (maxwidth != 0)
            {
                gRichTextField.autoSize = AutoSizeType.Height;

                gRichTextField.width = maxwidth;
            }

            //AppDebug.Log($"MaxWidth:{MaxWidth}\t text:{text}");
            //gRichTextField.relations.Add(FairyGUI.GRoot.inst, RelationType.Left_Left);
            //gRichTextField.relations.Add(FairyGUI.GRoot.inst, RelationType.Top_Top);
            gRichTextField.onClickLink.Add (onClickLink);
        }
        public void AddGRichTextToGRoot ()
        {
            //CreateGRichText();
            GRoot.inst.AddChild (gRichTextField);
            addedToGRoot = true;
        }
        public void RemoveChildFromGRoot ()
        {
            gRichTextField.onClickLink.Clear ();
            GRoot.inst.RemoveChild (gRichTextField);
        }

        public void OnActivityChange (bool isActive)
        {
            if (gRichTextField != null)
                gRichTextField.visible = isActive;
        }
        public void Dispose ()
        {
            gRichTextField.onClickLink.Clear ();
            GRoot.inst.RemoveChild (gRichTextField);
            gRichTextField.Dispose ();
            gRichTextField = null;
        }

        private void onClickLink (EventContext context)
        {
            int.TryParse ((string)context.data, out var selection);
            onClickLinkHandler?.Invoke (selection);
            AppDebug.Log ($"onClickLink,L{context.data}");
        }

        public Action<int> onClickLinkHandler;

        public double GetTextLength (string t)
        {
            if (string.IsNullOrEmpty (t))
                return 0;
            var contentSize = base_Font.MeasureString (t);
            //AppDebug.Log($"text:{t}\t contentSize:{contentSize}\t fontSizeScaler:{fontSizeScaler}");
            //return Math.Round(contentSize.X * fontSizeScaler);
            return Math.Round (contentSize.X * letterSpacingScaler);
        }
    }
}

