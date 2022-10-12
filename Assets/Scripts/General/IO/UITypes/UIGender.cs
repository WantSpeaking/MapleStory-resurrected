#define USE_NX

using System.Collections.Generic;
using Beebyte.Obfuscator;
using MapleLib.WzLib;




namespace ms
{
	[Skip]
	public class UIGender : UIElement
	{
		public const Type TYPE = UIElement.Type.GENDER;
		public const bool FOCUSED = true;
		public const bool TOGGLED = false;

		public UIGender (params object[] args) : this ((System.Action)args[0])
		{
		}
		
		public UIGender(System.Action oh) : base(new Point_short(0, 15), new Point_short(0, 0))
		{
			this.okhandler = oh;
			CUR_TIMESTEP = 0;

			WzObject Gender = ms.wz.wzFile_ui["Login.img"]["Gender"];

			for (uint i = 0; i < 3; i++)
			{
				gender_sprites[i] = Gender["scroll"]["0"][i.ToString ()];
			}

			sprites.Add(new Sprite(Gender["text"]["0"], new Point_short(601, 326)));

			List<string> options = new List<string>();
			options.Add("Male");
			options.Add("Female");

			ushort default_option = 0;

			buttons[(int)Buttons.NO] = new MapleButton(Gender["BtNo"], new Point_short(650, 349));
			buttons[(int)Buttons.YES] = new MapleButton(Gender["BtYes"], new Point_short(578, 349));
			buttons[(int)Buttons.SELECT] =new MapleComboBox(MapleComboBox.Type.DEFAULT, options, default_option, position, new Point_short(510, 283), 65);

			dimension = gender_sprites[2].get_dimensions();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float inter) const override
		public override void draw(float inter)
		{
			Point_short gender_pos = new Point_short(355, 185);

			if (CUR_TIMESTEP == 0)
			{
				gender_sprites[0].draw(position + gender_pos);
			}
			else if (CUR_TIMESTEP == Constants.TIMESTEP * 3)
			{
				gender_sprites[1].draw(position + gender_pos);
			}
			else if (CUR_TIMESTEP >= Constants.TIMESTEP * 6)
			{
				gender_sprites[2].draw(position + gender_pos);

				base.draw(inter);
			}
		}
		public override void update()
		{
			base.update();

			if (CUR_TIMESTEP <= Constants.TIMESTEP * 6)
			{
				CUR_TIMESTEP += Constants.TIMESTEP;
			}
		}

		public override Cursor.State send_cursor(bool clicked, Point_short cursorpos)
		{
			var combobox = buttons[(int)Buttons.SELECT];

			if (combobox.is_pressed() && combobox.in_combobox(cursorpos))
			{
				Cursor.State new_state = combobox.send_cursor (clicked, cursorpos);
				if (new_state!= Cursor.State.IDLE)
				{
					return new_state;
				}
			}

			return base.send_cursor(clicked, new Point_short (cursorpos));
		}

		public override UIElement.Type get_type()
		{
			return TYPE;
		}

		public override Button.State button_pressed(ushort buttonid)
		{
			switch ((Buttons)buttonid)
			{
				case Buttons.NO:
				{
					deactivate();
					okhandler();

					return Button.State.NORMAL;
				}
				case Buttons.YES:
				{
					UI.get().emplace<UILoginWait>();

					ushort selected_value = buttons[(int)Buttons.SELECT].get_selected();
					new GenderPacket(selected_value == 1).dispatch();

					return Button.State.NORMAL;
				}
				case Buttons.SELECT:
				{
					buttons[(int)Buttons.SELECT].toggle_pressed();

					return Button.State.NORMAL;
				}
				default:
				{
					return Button.State.DISABLED;
				}
			}
		}

		private enum Buttons : ushort
		{
			NO,
			YES,
			SELECT
		}

		private Texture[] gender_sprites =new Texture[3];
		private ushort CUR_TIMESTEP;
		private System.Action okhandler;
	}
}




#if USE_NX
#endif
