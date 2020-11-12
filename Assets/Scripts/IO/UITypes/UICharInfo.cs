#define USE_NX

using System;
using System.Collections.Generic;
using Helper;
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
	public class UICharInfo : UIDragElement<PosCHARINFO>
	{
		public const Type TYPE = UIElement.Type.CHARINFO;
		public const bool FOCUSED = false;
		public const bool TOGGLED = true;

		public UICharInfo (params object[] args) : this ((int)args[0])
		{
		}

		public UICharInfo (int cid)
		{
			//this.UIDragElement<PosCHARINFO> = new <type missing>();
			this.is_loading = true;
			this.timestep = Constants.TIMESTEP;
			this.personality_enabled = false;
			this.collect_enabled = false;
			this.damage_enabled = false;
			this.item_enabled = false;
			WzObject close = nl.nx.wzFile_ui["Basic.img"]["BtClose3"];
			WzObject UserInfo = nl.nx.wzFile_ui["UIWindow2.img"]["UserInfo"];
			WzObject character = UserInfo["character"];
			WzObject backgrnd = character["backgrnd"];

			/// Main Window
			sprites.Add (backgrnd);
			sprites.Add (character["backgrnd2"]);
			sprites.Add (character["name"]);

			Point<short> backgrnd_dim = new Texture (backgrnd).get_dimensions ();
			Point<short> close_dimensions = new Point<short> ((short)(backgrnd_dim.x () - 21), 6);

			buttons[(int)Buttons.BtClose] = new MapleButton (close, close_dimensions);
			buttons[(int)Buttons.BtCollect] = new MapleButton (character["BtCollect"]);
			buttons[(int)Buttons.BtDamage] = new MapleButton (character["BtDamage"]);
			buttons[(int)Buttons.BtFamily] = new MapleButton (character["BtFamily"]);
			buttons[(int)Buttons.BtItem] = new MapleButton (character["BtItem"]);
			buttons[(int)Buttons.BtParty] = new MapleButton (character["BtParty"]);
			buttons[(int)Buttons.BtPersonality] = new MapleButton (character["BtPersonality"]);
			buttons[(int)Buttons.BtPet] = new MapleButton (character["BtPet"]);
			buttons[(int)Buttons.BtPopDown] = new MapleButton (character["BtPopDown"]);
			buttons[(int)Buttons.BtPopUp] = new MapleButton (character["BtPopUp"]);
			buttons[(int)Buttons.BtRide] = new MapleButton (character["BtRide"]);
			buttons[(int)Buttons.BtTrad] = new MapleButton (character["BtTrad"]);

			name = new Text (Text.Font.A12M, Text.Alignment.CENTER, Color.Name.WHITE);
			job = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.EMPEROR);
			level = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.EMPEROR);
			fame = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.EMPEROR);
			guild = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.EMPEROR);
			alliance = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.EMPEROR);

			// TODO: Check if player has a mount or pet, disable if they don't
			buttons[(int)Buttons.BtPet].set_state (Button.State.DISABLED);
			buttons[(int)Buttons.BtRide].set_state (Button.State.DISABLED);

			/// Farm
			WzObject farm = UserInfo["farm"];
			WzObject farm_backgrnd = farm["backgrnd"];

			loading = farm["loading"];

			farm_dim = new Texture (farm_backgrnd).get_dimensions ();
			farm_adj = new Point<short> ((short)-farm_dim.x (), 0);

			sprites.Add (new Sprite (farm_backgrnd, farm_adj));
			sprites.Add (new Sprite (farm["backgrnd2"], farm_adj));
			sprites.Add (new Sprite (farm["default"], farm_adj));
			sprites.Add (new Sprite (farm["cover"], farm_adj));

			buttons[(int)Buttons.BtFriend] = new MapleButton (farm["btFriend"], farm_adj);
			buttons[(int)Buttons.BtVisit] = new MapleButton (farm["btVisit"], farm_adj);

			farm_name = new Text (Text.Font.A11M, Text.Alignment.CENTER, Color.Name.SUPERNOVA);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: farm_level = Charset(farm["number"], Charset::Alignment::LEFT);
			farm_level = new Charset (farm["number"], Charset.Alignment.LEFT);

			#region BottomWindow

			bottom_window_adj = new Point<short> (0, (short)(backgrnd_dim.y () + 1));

			/// Personality
			WzObject personality = UserInfo["personality"];
			WzObject personality_backgrnd = personality["backgrnd"];

			personality_sprites.Add (new Sprite (personality_backgrnd, bottom_window_adj));
			personality_sprites.Add (new Sprite (personality["backgrnd2"], bottom_window_adj));

			personality_sprites_enabled[true].Add (new Sprite (personality["backgrnd3"], bottom_window_adj));
			personality_sprites_enabled[true].Add (new Sprite (personality["backgrnd4"], bottom_window_adj));
			personality_sprites_enabled[true].Add (new Sprite (personality["center"], bottom_window_adj));
			personality_sprites_enabled[false].Add (new Sprite (personality["before30level"], bottom_window_adj));

			personality_dimensions = new Texture (personality_backgrnd).get_dimensions ();

			/// Collect
			WzObject collect = UserInfo["collect"];
			WzObject collect_backgrnd = collect["backgrnd"];

			collect_sprites.Add (new Sprite (collect_backgrnd, bottom_window_adj));
			collect_sprites.Add (new Sprite (collect["backgrnd2"], bottom_window_adj));

			default_medal = collect["icon1"];

			buttons[(int)Buttons.BtArrayGet] = new MapleButton (collect["BtArrayGet"], bottom_window_adj);
			buttons[(int)Buttons.BtArrayName] = new MapleButton (collect["BtArrayName"], bottom_window_adj);

			medal_text = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.EMPEROR, "Junior Adventurer");
			medal_total = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.EMPEROR, "2");

			collect_dimensions = new Texture (collect_backgrnd).get_dimensions ();

			/// Damage
			WzObject damage = UserInfo["damage"];
			WzObject damage_backgrnd = damage["backgrnd"];

			damage_sprites.Add (new Sprite (damage_backgrnd, bottom_window_adj));
			damage_sprites.Add (new Sprite (damage["backgrnd2"], bottom_window_adj));
			damage_sprites.Add (new Sprite (damage["backgrnd3"], bottom_window_adj));

			buttons[(int)Buttons.BtFAQ] = new MapleButton (damage["BtFAQ"], bottom_window_adj);
			buttons[(int)Buttons.BtRegist] = new MapleButton (damage["BtRegist"], bottom_window_adj);

			damage_dimensions = new Texture (damage_backgrnd).get_dimensions ();

			#endregion

			#region RightWindow

			right_window_adj = new Point<short> (backgrnd_dim.x (), 0);

			/// Item
			WzObject item = UserInfo["item"];
			WzObject item_backgrnd = item["backgrnd"];

			item_sprites.Add (new Sprite (item_backgrnd, right_window_adj));
			item_sprites.Add (new Sprite (item["backgrnd2"], right_window_adj));

			item_dimensions = new Texture (item_backgrnd).get_dimensions ();

			#endregion


//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: dimension = backgrnd_dim;
			dimension = (backgrnd_dim);
			dragarea = new Point<short> (dimension.x (), 20);

			target_character = Stage.get ().get_character (cid).get ();

			new CharInfoRequestPacket (cid).dispatch ();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float inter) const override
		public override void draw (float inter)
		{
			base.draw_sprites (inter);

			for (int i = 0; i < (int)Buttons.BtArrayGet; i++)
			{
				var button = buttons[(ushort)i];
				if (button != null)
				{
					button.draw (position);
				}
			}

			/// Main Window
			short row_height = 18;
			Point<short> text_pos1 = position + new Point<short> (153, 65);

			target_character.draw_preview (position + new Point<short> (63, 129), inter);

			name.draw (position + new Point<short> (59, 131));
			level.draw (text_pos1 + new Point<short> (0, (short)(row_height * 0)));
			job.draw (text_pos1 + new Point<short> (0, (short)(row_height * 1)));
			fame.draw (text_pos1 + new Point<short> (0, (short)(row_height * 2)));
			guild.draw (text_pos1 + new Point<short> (0, (short)(row_height * 3)) + new Point<short> (0, 1));
			alliance.draw (text_pos1 + new Point<short> (0, (short)(row_height * 4)));

			/// Farm
			Point<short> farm_pos = position + farm_adj;

			if (is_loading)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: loading.draw(farm_pos, inter);
				loading.draw (farm_pos, inter);
			}

			farm_name.draw (farm_pos + new Point<short> (136, 51));
			farm_level.draw (farm_level_text, farm_pos + new Point<short> (126, 34));

			/// Personality
			if (personality_enabled)
			{
				foreach (Sprite sprite in personality_sprites)
				{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: sprite.draw(position, inter);
					sprite.draw (position, inter);
				}

				bool show_personality = (target_character.get_level () >= 30);

				foreach (Sprite sprite in personality_sprites_enabled[show_personality])
				{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: sprite.draw(position, inter);
					sprite.draw (position, inter);
				}
			}

			/// Collect
			if (collect_enabled)
			{
				foreach (Sprite sprite in collect_sprites)
				{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: sprite.draw(position, inter);
					sprite.draw (position, inter);
				}

				for (int i = 0; i < 15; i++)
				{
					std.div_t div = std.div (i, 5);

					default_medal.draw (position + bottom_window_adj + new Point<short> (61, 66) + new Point<short> ((short)(38 * div.rem), (short)(38 * div.quot)), inter);
				}

				for (ushort i = (ushort)Buttons.BtArrayGet; i < (ushort)Buttons.BtFAQ; i++)
				{
					var button = buttons[i];
					if (button != null)
					{
						button.draw (position);
					}
				}

				Point<short> text_pos2 = new Point<short> (121, 8);

				medal_text.draw (position + bottom_window_adj + text_pos2);
				medal_total.draw (position + bottom_window_adj + text_pos2 + new Point<short> (0, 19));
			}

			/// Damage
			if (damage_enabled)
			{
				foreach (Sprite sprite in damage_sprites)
				{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: sprite.draw(position, inter);
					sprite.draw (position, inter);
				}

				for (int i = (int)Buttons.BtFAQ; i < buttons.Count; i++)
				{
					var button = buttons[(ushort)i];
					if (button != null)
					{
						button.draw (position);
					}
				}
			}

			/// Item
			if (item_enabled)
			{
				foreach (Sprite sprite in item_sprites)
				{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: sprite.draw(position, inter);
					sprite.draw (position, inter);
				}
			}
		}

		public override void update ()
		{
			if (timestep >= Constants.TIMESTEP * limits.UCHAR_MAX)
			{
				is_loading = false;
			}
			else
			{
				loading.update ();
				timestep += Constants.TIMESTEP;
			}
		}

		public override Button.State button_pressed (ushort buttonid)
		{
			switch ((Buttons)buttonid)
			{
				case Buttons.BtClose:
					deactivate ();
					return Button.State.NORMAL;
				case Buttons.BtFamily:
				case Buttons.BtParty:
					break;
				case Buttons.BtItem:
					show_right_window (buttonid);
					return Button.State.NORMAL;
				case Buttons.BtCollect:
				case Buttons.BtPersonality:
				case Buttons.BtRide:
				case Buttons.BtPet:
				case Buttons.BtDamage:
					show_bottom_window (buttonid);
					return Button.State.NORMAL;
				case Buttons.BtPopDown:
				case Buttons.BtPopUp:
				case Buttons.BtTrad:
				case Buttons.BtFriend:
				case Buttons.BtVisit:
				default:
					break;
			}

			return Button.State.DISABLED;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_in_range(Point<short> cursorpos) const override
		public override bool is_in_range (Point<short> cursorpos)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: Rectangle<short> bounds = Rectangle<short>(position, position + dimension);
			Rectangle<short> bounds = new Rectangle<short> (position, position + dimension);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: Rectangle<short> farm_bounds = Rectangle<short>(position, position + farm_dim);
			Rectangle<short> farm_bounds = new Rectangle<short> (position, position + farm_dim);
			farm_bounds.shift (farm_adj);

			Rectangle<short> bottom_bounds = new Rectangle<short> (new Point<short> (0, 0), new Point<short> (0, 0));
			Rectangle<short> right_bounds = new Rectangle<short> (new Point<short> (0, 0), new Point<short> (0, 0));

			short cur_x = cursorpos.x ();
			short cur_y = cursorpos.y ();

			if (personality_enabled)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: bottom_bounds = Rectangle<short>(position, position + personality_dimensions);
				bottom_bounds = new Rectangle<short> (position, position + personality_dimensions);
				bottom_bounds.shift (bottom_window_adj);
			}

			if (collect_enabled)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: bottom_bounds = Rectangle<short>(position, position + collect_dimensions);
				bottom_bounds = new Rectangle<short> (position, position + collect_dimensions);
				bottom_bounds.shift (bottom_window_adj);
			}

			if (damage_enabled)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: bottom_bounds = Rectangle<short>(position, position + damage_dimensions);
				bottom_bounds = new Rectangle<short> (position, position + damage_dimensions);
				bottom_bounds.shift (bottom_window_adj);
			}

			if (item_enabled)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: right_bounds = Rectangle<short>(position, position + item_dimensions);
				right_bounds = new Rectangle<short> (position, position + item_dimensions);
				right_bounds.shift (right_window_adj);
			}

			return bounds.contains (cursorpos) || farm_bounds.contains (cursorpos) || bottom_bounds.contains (cursorpos) || right_bounds.contains (cursorpos);
		}

		public override void send_key (int keycode, bool pressed, bool escape)
		{
			if (pressed && escape)
			{
				deactivate ();
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public void update_stats (int character_id, short job_id, sbyte lv, short f, string g, string a)
		{
			int player_id = Stage.get ().get_player ().get_oid ();

			if (character_id == player_id)
			{
				buttons[(int)Buttons.BtParty].set_state (Button.State.DISABLED);
				buttons[(int)Buttons.BtPopDown].set_state (Button.State.DISABLED);
				buttons[(int)Buttons.BtPopUp].set_state (Button.State.DISABLED);
				buttons[(int)Buttons.BtFriend].set_state (Button.State.DISABLED);
			}

			Job character_job = new Job ((ushort)job_id);

			name.change_text (target_character.get_name ());
			job.change_text (character_job.get_name ());
			level.change_text (Convert.ToString (lv));
			fame.change_text (Convert.ToString (f));
			guild.change_text ((g == "" ? "-" : g));
			alliance.change_text (a);

			farm_name.change_text ("");
			farm_level_text = "1";
		}

		private void show_bottom_window (ushort buttonid)
		{
			personality_enabled = false;
			collect_enabled = false;
			damage_enabled = false;

			switch ((Buttons)buttonid)
			{
				case Buttons.BtPersonality:
					personality_enabled = true;
					break;
				case Buttons.BtCollect:
					collect_enabled = true;
					break;
				case Buttons.BtDamage:
					damage_enabled = true;
					break;
			}
		}

		private void show_right_window (ushort buttonid)
		{
			item_enabled = false;

			switch ((Buttons)buttonid)
			{
				case Buttons.BtItem:
					item_enabled = true;
					break;
			}
		}

		private enum Buttons : ushort
		{
			BtClose,
			BtCollect,
			BtDamage,
			BtFamily,
			BtItem,
			BtParty,
			BtPersonality,
			BtPet,
			BtPopDown,
			BtPopUp,
			BtRide,
			BtTrad,
			BtFriend,
			BtVisit,
			BtArrayGet,
			BtArrayName,
			BtFAQ,
			BtRegist
		}

		/// Main Window
		private Text name = new Text ();

		private Text job = new Text ();
		private Text level = new Text ();
		private Text fame = new Text ();
		private Text guild = new Text ();
		private Text alliance = new Text ();

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C# does not have an equivalent to pointers to value types:
//ORIGINAL LINE: char* target_character;
		private Char target_character;

		/// Sub Windows
		private Point<short> bottom_window_adj = new Point<short> ();

		private Point<short> right_window_adj = new Point<short> ();

		/// Farm
		private Text farm_name = new Text ();

		private Sprite loading = new Sprite ();
		private bool is_loading;
		private ushort timestep;
		private Charset farm_level = new Charset ();
		private Point<short> farm_dim = new Point<short> ();
		private Point<short> farm_adj = new Point<short> ();
		private string farm_level_text;

		/// Personality
		private bool personality_enabled;

		private List<Sprite> personality_sprites = new List<Sprite> ();
		private BoolPair<List<Sprite>> personality_sprites_enabled = new BoolPair<List<Sprite>> ();
		private Point<short> personality_dimensions = new Point<short> ();

		/// Collect
		private bool collect_enabled;

		private List<Sprite> collect_sprites = new List<Sprite> ();
		private Point<short> collect_dimensions = new Point<short> ();
		private Point<short> collect_adj = new Point<short> ();
		private Sprite default_medal = new Sprite ();
		private Text medal_text = new Text ();
		private Text medal_total = new Text ();

		/// Damage
		private bool damage_enabled;

		private List<Sprite> damage_sprites = new List<Sprite> ();
		private Point<short> damage_dimensions = new Point<short> ();

		/// Item
		private bool item_enabled;

		private List<Sprite> item_sprites = new List<Sprite> ();
		private Point<short> item_dimensions = new Point<short> ();
	}
}


#if USE_NX
#endif