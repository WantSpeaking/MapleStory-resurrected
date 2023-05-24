#define USE_NX

using System;
using Beebyte.Obfuscator;
using MapleLib.WzLib;





namespace ms
{
	[Skip]
	public class UIChat : UIDragElement<PosMAPLECHAT>
	{
		public const Type TYPE = UIElement.Type.CHAT;
		public const bool FOCUSED = false;
		public const bool TOGGLED = true;

		public UIChat (params object[] args) : this ()
		{
		}
		
		public UIChat()
		{
			//this.UIDragElement<PosMAPLECHAT> = new <type missing>();
			show_weekly = Configuration.get().get_show_weekly();

			WzObject socialChatEnter = ms.wz.wzFile_ui["UIWindow2.img"]["socialChatEnter"];

			WzObject backgrnd = socialChatEnter["backgrnd"];
			WzObject backgrnd4 = socialChatEnter["backgrnd4"];
			WzObject backgrnd5 = socialChatEnter["backgrnd5"];

			rank_shift = new Point_short(86, 130);
			name_shift = new Point_short(50, 5);

			origin_left = new Texture(backgrnd4).get_origin();
			origin_right = new Texture(backgrnd5).get_origin();

			origin_left = new Point_short(Math.Abs(origin_left.x()), Math.Abs(origin_left.y()));
			origin_right = new Point_short(Math.Abs(origin_right.x()), Math.Abs(origin_right.y()));

			sprites.Add(socialChatEnter["ribbon"]);
			sprites.Add(backgrnd);
			sprites.Add(socialChatEnter["backgrnd2"]);
			sprites.Add(socialChatEnter["backgrnd3"]);
			sprites.Add(backgrnd4);
			sprites.Add(backgrnd5);

			buttons[(int)Buttons.CLOSE] = new MapleButton(socialChatEnter["btX"]);
			buttons[(int)Buttons.CHAT_DUO] = new MapleButton(socialChatEnter["duoChat"]);
			buttons[(int)Buttons.CHAT_FRIEND] = new MapleButton(socialChatEnter["groupChatFrd"]);
			buttons[(int)Buttons.CHAT_RANDOM] = new MapleButton(socialChatEnter["groupChatRnd"]);

			charset = new Charset(socialChatEnter["number"], Charset.Alignment.RIGHT);

			name_left = new Text(Text.Font.A12B, Text.Alignment.CENTER, Color.Name.WHITE);
			name_right = new Text(Text.Font.A12B, Text.Alignment.CENTER, Color.Name.WHITE);

			dimension = new Texture(backgrnd).get_dimensions();

			if (show_weekly)
			{
				UI.get().emplace<UIRank>();
			}
		}

		public override void draw(float inter)
		{
			base.draw(inter);

			charset.draw("0", position + origin_left + rank_shift);
			charset.draw("0", position + origin_right + rank_shift);

			name_left.draw(position + origin_left + name_shift);
			name_right.draw(position + origin_right + name_shift);
		}

		public override void send_key(int keycode, bool pressed, bool escape)
		{
			if (pressed && escape)
			{
				close();
			}
		}

		public override UIElement.Type get_type()
		{
			return TYPE;
		}

		public override Button.State button_pressed(ushort buttonid)
		{
			switch ((Buttons)buttonid)
			{
			case Buttons.CLOSE:
				close();
				break;
			case Buttons.CHAT_DUO:
				break;
			case Buttons.CHAT_FRIEND:
				break;
			case Buttons.CHAT_RANDOM:
				break;
			default:
				break;
			}

			return Button.State.NORMAL;
		}

		private void close()
		{
			deactivate();
		}

		private enum Buttons : ushort
		{
			CLOSE,
			CHAT_DUO,
			CHAT_FRIEND,
			CHAT_RANDOM
		}

		private bool show_weekly;
		private Point_short rank_shift = new Point_short();
		private Point_short name_shift = new Point_short();
		private Point_short origin_left = new Point_short();
		private Point_short origin_right = new Point_short();
		private Charset charset = new Charset();
		private Text name_left = new Text();
		private Text name_right = new Text();
	}

	[Skip]
	public class UIRank : UIDragElement<PosMAPLECHAT>
	{
		public const Type TYPE = UIElement.Type.CHATRANK;
		public const bool FOCUSED = false;
		public const bool TOGGLED = true;

		public UIRank (params object[] args) : this ()
		{
		}
		
		public UIRank()
		{
			//this.UIDragElement<PosMAPLECHAT> = new <type missing>();
			Configuration.get().set_show_weekly(false);

			WzObject socialRank = ms.wz.wzFile_ui["UIWindow2.img"]["socialRank"];

			WzObject backgrnd = socialRank["backgrnd"];
			WzObject backgrnd4 = socialRank["backgrnd4"];
			WzObject backgrnd5 = socialRank["backgrnd5"];

			rank_shift = new Point_short(86, 130);
			name_shift = new Point_short(52, 4);

			origin_left = new Texture(backgrnd4).get_origin();
			origin_right = new Texture(backgrnd5).get_origin();

			origin_left = new Point_short((short)(Math.Abs(origin_left.x()) - 1), Math.Abs(origin_left.y()));
			origin_right = new Point_short(Math.Abs(origin_right.x()), Math.Abs(origin_right.y()));

			sprites.Add(socialRank["ribbon"]);
			sprites.Add(backgrnd);
			sprites.Add(socialRank["backgrnd2"]);
			sprites.Add(socialRank["backgrnd3"]);
			sprites.Add(backgrnd4);
			sprites.Add(backgrnd5);

			buttons[(int)Buttons.CLOSE] = new MapleButton(socialRank["btX"]);

			charset = new Charset(socialRank["number"], Charset.Alignment.RIGHT);

			name_left = new Text(Text.Font.A12B, Text.Alignment.CENTER, Color.Name.WHITE);
			name_right = new Text(Text.Font.A12B, Text.Alignment.CENTER, Color.Name.WHITE);

			dimension = new Texture(backgrnd).get_dimensions();
			position=(position + new Point_short(211, 124));
		}

		public override void draw(float inter)
		{
			base.draw(inter);

			charset.draw("0", position + origin_left + rank_shift);
			charset.draw("0", position + origin_right + rank_shift);

			name_left.draw(position + origin_left + name_shift);
			name_right.draw(position + origin_right + name_shift);
		}

		public override void send_key(int keycode, bool pressed, bool escape)
		{
			if (pressed && escape)
			{
				close();
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
			case Buttons.CLOSE:
				close();
				break;
			default:
				break;
			}

			return Button.State.NORMAL;
		}

		private void close()
		{
			deactivate();
		}

		private enum Buttons : ushort
		{
			CLOSE
		}

		private Point_short rank_shift = new Point_short();
		private Point_short name_shift = new Point_short();
		private Point_short origin_left = new Point_short();
		private Point_short origin_right = new Point_short();
		private Charset charset = new Charset();
		private Text name_left = new Text();
		private Text name_right = new Text();

		public override void Dispose ()
		{
			base.Dispose ();
			charset?.Dispose ();
		}
	}
}



#if USE_NX
#endif
