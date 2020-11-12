#define USE_NX

using System;
using MapleLib.WzLib;

//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////


namespace ms
{
	public class UIQuit : UIElement
	{
		public const Type TYPE = UIElement.Type.QUIT;
		public const bool FOCUSED = true;
		public const bool TOGGLED = false;

		public UIQuit (params object[] args) : this ((CharStats)args[0])
		{
		}

		public UIQuit (CharStats st)
		{
			this.screen_adj = new ms.Point<short> (212, 114);
			this.stats = st;
			WzObject askReward = nl.nx.wzFile_ui["UIWindow6.img"]["askReward"];
			WzObject userLog = askReward["userLog"];
			WzObject exp = userLog["exp"];
			WzObject level = userLog["level"];
			WzObject time = userLog["time"];
			WzObject backgrnd = userLog["backgrnd"];

			sprites.Add (new Sprite (backgrnd, -screen_adj));

			buttons[(int)Buttons.NO] = new MapleButton (askReward["btNo"], new Point<short> (0, 37));
			buttons[(int)Buttons.YES] = new MapleButton (askReward["btYes"], new Point<short> (0, 37));

			Stage stage = Stage.get ();

			/// Time
			long uptime = stage.get_uptime () / 1000 / 1000;
			minutes = uptime / 60;
			hours = minutes / 60;

			minutes -= hours * 60;

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: time_minutes = Charset(time["number"], Charset::Alignment::LEFT);
			time_minutes = new Charset (time["number"], Charset.Alignment.LEFT);
			time_minutes_pos = time["posM"];
			time_minutes_text = pad_time (minutes);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: time_hours = Charset(time["number"], Charset::Alignment::LEFT);
			time_hours = new Charset (time["number"], Charset.Alignment.LEFT);
			time_hours_pos = time["posH"];
			time_hours_text = pad_time (hours);

			time_number_width = time["numberWidth"];

			time_lt = time["tooltip"]["lt"];
			time_rb = time["tooltip"]["rb"];

			/// Level
			levelupEffect = level["levelupEffect"];

			uplevel = stage.get_uplevel ();

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: levelBefore = Charset(level["number"], Charset::Alignment::LEFT);
			levelBefore = new Charset (level["number"], Charset.Alignment.LEFT);
			levelBeforePos = level["posBefore"];
			levelBeforeText = Convert.ToString (uplevel);

			cur_level = stats.get_stat (MapleStat.Id.LEVEL);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: levelAfter = Charset(level["number"], Charset::Alignment::LEFT);
			levelAfter = new Charset (level["number"], Charset.Alignment.LEFT);
			levelAfterPos = level["posAfter"];
			levelAfterText = Convert.ToString (cur_level);

			levelNumberWidth = level["numberWidth"];

			level_adj = new Point<short> (40, 0);

			/// Experience
			long upexp = stage.get_upexp ();
			float expPercentBefore = getexppercent (uplevel, upexp);
			string expBeforeString = Convert.ToString (100 * expPercentBefore);
			string expBeforeText = expBeforeString.Substring (0, expBeforeString.IndexOf ('.') + 3) + '%';

			expBefore = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.WHITE, expBeforeText);
			expBeforePos = exp["posBefore"];

			long cur_exp = stats.get_exp ();
			float expPercentAfter = getexppercent (cur_level, cur_exp);
			string expAfterString = Convert.ToString (100 * expPercentAfter);
			string expAfterText = expAfterString.Substring (0, expAfterString.IndexOf ('.') + 3) + '%';

			expAfter = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.ELECTRICLIME, expAfterText);
			expAfterPos = exp["posAfter"];

			exp_adj = new Point<short> (0, 6);

			short width = Constants.get ().get_viewwidth ();
			short height = Constants.get ().get_viewheight ();

			background = new ColorBox (width, height, Color.Name.BLACK, 0.5f);
			position = new Point<short> ((short)(width / 2), (short)(height / 2));
			dimension = new Texture (backgrnd).get_dimensions ();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float inter) const override
		public override void draw (float inter)
		{
			background.draw (new Point<short> (0, 0));

			base.draw (inter);

			time_minutes.draw (time_minutes_text, (short)time_number_width, position + time_minutes_pos - screen_adj);
			time_hours.draw (time_hours_text, (short)time_number_width, position + time_hours_pos - screen_adj);

			levelBefore.draw (levelBeforeText, (short)levelNumberWidth, position + levelBeforePos + level_adj - screen_adj);
			levelAfter.draw (levelAfterText, (short)levelNumberWidth, position + levelAfterPos + level_adj - screen_adj);

			if (cur_level > uplevel)
			{
				levelupEffect.draw (position - screen_adj, inter);
			}

			expBefore.draw (position + expBeforePos - exp_adj - screen_adj);
			expAfter.draw (position + expAfterPos - exp_adj - screen_adj);
		}

		public override void update ()
		{
			base.update ();

			levelupEffect.update ();
		}

		public override Cursor.State send_cursor (bool clicked, Point<short> cursorpos)
		{
			var lt = position + time_lt - screen_adj;
			var rb = position + time_rb - screen_adj;

			var bounds = new Rectangle<short> (lt, rb);

			if (bounds.contains (cursorpos))
			{
				UI.get ().show_text (Tooltip.Parent.TEXT, Convert.ToString (hours) + "Hour " + Convert.ToString (minutes) + "Minute");
			}
			else
			{
				UI.get ().clear_tooltip (Tooltip.Parent.TEXT);
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: return UIElement::send_cursor(clicked, cursorpos);
			return base.send_cursor (clicked, cursorpos);
		}

		public override void send_key (int keycode, bool pressed, bool escape)
		{
			if (pressed)
			{
				if (escape)
				{
					close ();
				}
				else if (keycode == (int)KeyAction.Id.RETURN)
				{
					button_pressed ((ushort)Buttons.YES);
				}
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public override Button.State button_pressed (ushort buttonid)
		{
			switch ((Buttons)buttonid)
			{
				case Buttons.NO:
					close ();
					break;
				case Buttons.YES:
				{
					Constants.get ().set_viewwidth (800);
					Constants.get ().set_viewheight (600);

					float fadestep = 0.025f;

					/*Window.get().fadeout(fadestep, () =>
					{
							GraphicsGL.get().clear();
	
							UI.get().change_state(UI.State.LOGIN);
							UI.get().set_scrollnotice("");
							Session.get().reconnect();
	
							UI.get().enable();
							Timer.get().start();
							GraphicsGL.get().unlock();
					});
	
					GraphicsGL.get().@lock();
					Stage.get().clear();
					Timer.get().start();*/


					Stage.get ().clear ();
					Timer.get ().start ();
					UI.get ().change_state (UI.State.LOGIN);
					UI.get ().set_scrollnotice ("");
					Session.get ().reconnect ();

					UI.get ().enable ();
					Timer.get ().start ();
				}
					break;
				default:
					break;
			}

			return Button.State.NORMAL;
		}

		private readonly CharStats stats;

		private string pad_time (long time)
		{
			string ctime = Convert.ToString (time);
			uint length = (uint)ctime.Length;

			if (length > 2)
			{
				return "99";
			}

			return (2 - length, '0') + ctime;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float getexppercent(ushort level, long exp) const
		private float getexppercent (ushort level, long exp)
		{
			if (level >= ExpTable.LEVELCAP)
			{
				return 0.0f;
			}

			return (float)((double)exp / ExpTable.values[level]);
		}

		private void close ()
		{
			deactivate ();

			UI.get ().clear_tooltip (Tooltip.Parent.TEXT);
		}

		private enum Buttons : ushort
		{
			NO,
			YES
		}

		private Point<short> screen_adj = new Point<short> ();
		private ColorBox background = new ColorBox ();

		/// Time
		private long minutes;

		private long hours;

		private Charset time_minutes = new Charset ();
		private Point<short> time_minutes_pos = new Point<short> ();
		private string time_minutes_text;

		private Charset time_hours = new Charset ();
		private Point<short> time_hours_pos = new Point<short> ();
		private string time_hours_text;

		private long time_number_width;

		private Point<short> time_lt = new Point<short> ();
		private Point<short> time_rb = new Point<short> ();

		/// Level
		private Sprite levelupEffect = new Sprite ();

		private ushort uplevel;

		private Charset levelBefore = new Charset ();
		private Point<short> levelBeforePos = new Point<short> ();
		private string levelBeforeText;

		private ushort cur_level;

		private Charset levelAfter = new Charset ();
		private Point<short> levelAfterPos = new Point<short> ();
		private string levelAfterText;

		private long levelNumberWidth;
		private Point<short> level_adj = new Point<short> ();

		/// Experience
		private Text expBefore = new Text ();

		private Point<short> expBeforePos = new Point<short> ();

		private Text expAfter = new Text ();
		private Point<short> expAfterPos = new Point<short> ();

		private Point<short> exp_adj = new Point<short> ();
	}
}


#if USE_NX
#endif