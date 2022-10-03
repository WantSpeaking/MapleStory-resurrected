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

		public class Layout : IEnumerable<Layout.Line>, IEnumerable
		{
			public class Word
			{
				public uint first;

				public uint last;

				public Font font;

				public ms.Color.Name color;

				public Word (uint _first, uint _last, Font _font, ms.Color.Name _color)
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

			private List<Line> lines = new List<Line> ();

			private List<short> advances = new List<short> ();

			private Point_short dimensions = new Point_short ();

			private Point_short endoffset = new Point_short ();

			public Layout (List<Line> l, List<short> a, short w, short h, short ex, short ey)
			{
				lines = new List<Line> (l);
				advances = new List<short> (a);
				dimensions = new Point_short (w, h);
				endoffset = new Point_short (ex, ey);
			}

			public Layout ()
				: this (new List<Line> (), new List<short> (), 0, 0, 0, 0)
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
				return (short)((index < advances.Count) ? advances[(int)index] : 0);
			}

			public Point_short get_dimensions ()
			{
				return dimensions;
			}

			public Point_short get_endoffset ()
			{
				return endoffset;
			}

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

		private static float base_FontSize = 14f;

		public static Microsoft.Xna.Framework.Color nameTagColor = new Microsoft.Xna.Framework.Color (0f, 0f, 0f, 0.6f);

		private Font font;

		private Alignment alignment;

		private ms.Color.Name color;

		private Background background;

		private Layout layout = new Layout ();

		private ushort maxwidth;

		private bool formatted;

		private string text;

		private short line_adj;

		private GRichTextField gRichTextField;

		private float letterSpacingScaler = 1.2f;

		public bool useFguiText = false;

		public bool addedToGRoot = false;

		public Action<int> onClickLinkHandler;

		public ushort MaxWidth
		{
			get
			{
				return (ushort)((maxwidth == 0) ? GetTextLength (text) : ((double)(int)maxwidth));
			}
			set
			{
				maxwidth = value;
			}
		}

		private float fontSizeScaler => FontTypeToSize (font) / base_FontSize;

		private static float FontTypeToSize (Font fontType)
		{
			float result = base_FontSize;
			switch (fontType)
			{
				case Font.A11M:
					result = 11f;
					break;
				case Font.A11B:
					result = 11f;
					break;
				case Font.A12M:
					result = 12f;
					break;
				case Font.A12B:
					result = 12f;
					break;
				case Font.A13M:
					result = 13f;
					break;
				case Font.A13B:
					result = 13f;
					break;
				case Font.A14B:
					result = 14f;
					break;
				case Font.A15B:
					result = 15f;
					break;
				case Font.A18M:
					result = 18f;
					break;
			}
			return result;
		}

		private static Dictionary<Text, GTextField> gTextFields = new Dictionary<Text, GTextField> ();
		public static void Init ()
		{
		}
		public static void HideAllGText ()
		{
			foreach (var gTextField in gTextFields.Values)
			{
				gTextField.visible = false;
			}
		}
		private int FontToSize (Font font)
		{
			var sizeStr = font.ToString ().Substring (1, 2);
			int.TryParse (sizeStr, out var size);
			return (int)(size / 11f * 30);
		}
		public Text (Font f, Alignment a, ms.Color.Name c, Background b, string t = "", ushort mw = 0, bool fm = true, short la = 0)
		{
			font = f;
			alignment = a;
			color = c;
			background = b;
			maxwidth = mw;
			formatted = fm;
			line_adj = la;

			change_text (t);
		}

		public Text (Font f, Alignment a, ms.Color.Name c, string t = "", ushort mw = 0, bool fm = true, short la = 0)
			: this (f, a, c, Background.NONE, t, mw, fm, la)
		{
		}

		public Text ()
			: this (Font.A11M, Alignment.LEFT, ms.Color.Name.BLACK, "", 0, fm: true, 0)
		{
		}

		public void draw (DrawArgument args)
		{
			draw (args, new Range_short (0, 0));
		}

		public void draw (DrawArgument args, Range_short vertical)
		{
			TextManager.Instance.TryDraw (this, text, args, vertical);


			/*if (string.IsNullOrEmpty (text))
			{
				return;
			}
			float[] msColor = ms.Color.colors[(uint)color];
			Microsoft.Xna.Framework.Color monoColor = new Microsoft.Xna.Framework.Color (msColor[0], msColor[1], msColor[2], 1f);
			SpriteFont spriteFont = (font.ToString ().Contains ("B") ? base_BoldFont : base_Font);
			short drawPosX = args.getpos ().x ();
			int drawPosY = -args.getpos ().y ();
			useFguiText = true;
			if (useFguiText)
			{
				gRichTextField.visible = true;

				var screenPos = UnityEngine.Camera.main.WorldToScreenPoint (new UnityEngine.Vector3 (args.getpos ().x (), args.getpos ().y (), 1));
				screenPos.y = screenPos.y - UnityEngine.Screen.height;
				gRichTextField.position = GRoot.inst.GlobalToLocal (screenPos);

				//gRichTextField.SetPosition ((float)drawPosX * Singleton<ms.Window>.Instance.ratio, (float)(-drawPosY) * Singleton<ms.Window>.Instance.ratio, -99f);
				gRichTextField.text = text;
				return;
			}
			drawPosX = args.getpos ().x ();
			drawPosY = args.getpos ().y ();
			if (background == Background.NAMETAG)
			{
			}
			Singleton<ms.GraphicsGL>.get ().Batch.DrawString (spriteFont, text, new Microsoft.Xna.Framework.Vector2 (drawPosX, drawPosY), monoColor, 0f, Microsoft.Xna.Framework.Vector2.Zero, fontSizeScaler, SpriteEffects.None, 0f);*/
		}

		public void change_text (string t, bool setGRichText = true)
		{
			if (!string.IsNullOrEmpty (t) && !(text == t))
			{
				text = NpcTextParser.inst.Parse (t);
				reset_layout ();
				if (useFguiText && setGRichText)
				{
					gRichTextField.text = text;
				}
			}
		}

		public void change_color (ms.Color.Name c)
		{
			if (color != c)
			{
				color = c;
				reset_layout ();
			}
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
			return useFguiText ? ((short)gRichTextField.textHeight) : layout.height ();
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
			if (!string.IsNullOrEmpty (text))
			{
			}
		}

		public GRichTextField CreateGRichText ()
		{
			useFguiText = true;
			gRichTextField = new GRichTextField ();
			//gRichTextField.textFormat.size = (int)FontTypeToSize (font);
			//Microsoft.Xna.Framework.Color monoColor = new Microsoft.Xna.Framework.Color (msColor[0], msColor[1], msColor[2], 1f);
			//gRichTextField.textFormat.color = new UnityEngine.Color (msColor[0], msColor[1], msColor[2], 1f);

			if (maxwidth != 0)
			{
				gRichTextField.autoSize = AutoSizeType.Height;
				gRichTextField.width = (int)maxwidth;
			}
			gRichTextField.align = (AlignType)alignment;
			gRichTextField.onClickLink.Add (onClickLink);
			gRichTextField.pivotX = 0.5f;
			gRichTextField.pivotAsAnchor = true;

			var textFormat = gRichTextField.textFormat;
			float[] msColor = ms.Color.colors[(uint)color];
			textFormat.color = new UnityEngine.Color (msColor[0], msColor[1], msColor[2], 1f);
			textFormat.size = FontToSize (font);
			gRichTextField.textFormat = textFormat;

			GRoot.inst.AddChild (gRichTextField);

			return gRichTextField;
		}

		public void AddGRichTextToGRoot ()
		{
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
			{
				gRichTextField.visible = isActive;
			}
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

		public double GetTextLength (string t)
		{
			if (string.IsNullOrEmpty (t))
			{
				return 0.0;
			}
			return Math.Round (new Microsoft.Xna.Framework.Vector2 (0f, 0f).X * letterSpacingScaler);
		}
	}

}

