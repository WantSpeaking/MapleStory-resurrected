#define USE_NX

using System;
using System.Collections.Generic;
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
	public class UIStatusBar : UIElement
	{
		public const Type TYPE = UIElement.Type.STATUSBAR;
		public const bool FOCUSED = false;
		public const bool TOGGLED = true;

		public enum MenuType
		{
			MENU,
			SETTING,
			COMMUNITY,
			CHARACTER,
			EVENT
		}

		public UIStatusBar (CharStats st)
		{
			this.stats = st;
			quickslot_active = false;
			quickslot_adj = new Point<short> (QUICKSLOT_MAX, 0);
			VWIDTH = Constants.get ().get_viewwidth ();
			VHEIGHT = Constants.get ().get_viewheight ();

			menu_active = false;
			setting_active = false;
			community_active = false;
			character_active = false;
			event_active = false;

			string stat = "status";

			if (VWIDTH == 800)
			{
				stat += "800";
			}

			WzObject mainBar = nl.nx.wzFile_ui["StatusBar3.img"]["mainBar"];
			WzObject status = mainBar[stat];
			WzObject EXPBar = mainBar["EXPBar"];
			WzObject EXPBarRes = EXPBar[VWIDTH.ToString ()];
			WzObject menu = mainBar["menu"];
			WzObject quickSlot = mainBar["quickSlot"];
			WzObject submenu = mainBar["submenu"];

			exp_pos = new Point<short> (0, 87);

			sprites.Add (new Sprite (EXPBar["backgrnd"], new DrawArgument (new Point<short> (0, 87), new Point<short> (VWIDTH, 0))));
			sprites.Add (new Sprite (EXPBarRes["layer:back"], exp_pos));

			short exp_max = (short)(VWIDTH - 16);

			expbar = new Gauge (Gauge.Type.GAME, EXPBarRes.resolve ("layer:gauge"), EXPBarRes.resolve ("layer:cover"), EXPBar.resolve ("layer:effect"), exp_max, 0.0f);

			short pos_adj = 0;

			if (VWIDTH == 1280)
			{
				pos_adj = 87;
			}
			else if (VWIDTH == 1366)
			{
				pos_adj = 171;
			}
			else if (VWIDTH == 1920)
			{
				pos_adj = 448;
			}

			if (VWIDTH == 1024)
			{
				quickslot_min = 1;
			}
			else
			{
				quickslot_min = 0;
			}

			if (VWIDTH == 800)
			{
				hpmp_pos = new Point<short> (412, 40);
				hpset_pos = new Point<short> (530, 70);
				mpset_pos = new Point<short> (528, 86);
				statset_pos = new Point<short> (427, 111);
				levelset_pos = new Point<short> (461, 48);
				namelabel_pos = new Point<short> (487, 40);
				quickslot_pos = new Point<short> (579, 0);

				// Menu
				menu_pos = new Point<short> (682, -280);
				setting_pos = menu_pos + new Point<short> (0, 168);
				community_pos = menu_pos + new Point<short> (-26, 196);
				character_pos = menu_pos + new Point<short> (-61, 168);
				event_pos = menu_pos + new Point<short> (-94, 252);
			}
			else
			{
				hpmp_pos = new Point<short> ((short)(416 + pos_adj), 40);
				hpset_pos = new Point<short> ((short)(550 + pos_adj), 70);
				mpset_pos = new Point<short> ((short)(546 + pos_adj), 86);
				statset_pos = new Point<short> ((short)(539 + pos_adj), 111);
				levelset_pos = new Point<short> ((short)(465 + pos_adj), 48);
				namelabel_pos = new Point<short> ((short)(493 + pos_adj), 40);
				quickslot_pos = new Point<short> ((short)(628 + pos_adj), 37);

				// Menu
				menu_pos = new Point<short> ((short)(720 + pos_adj), -280);
				setting_pos = menu_pos + new Point<short> (0, 168);
				community_pos = menu_pos + new Point<short> (-26, 196);
				character_pos = menu_pos + new Point<short> (-61, 168);
				event_pos = menu_pos + new Point<short> (-94, 252);
			}

			if (VWIDTH == 1280)
			{
				statset_pos = new Point<short> ((short)(580 + pos_adj), 111);
				quickslot_pos = new Point<short> ((short)(622 + pos_adj), 37);

				// Menu
				menu_pos += new Point<short> (-7, 0);
				setting_pos += new Point<short> (-7, 0);
				community_pos += new Point<short> (-7, 0);
				character_pos += new Point<short> (-7, 0);
				event_pos += new Point<short> (-7, 0);
			}
			else if (VWIDTH == 1366)
			{
				quickslot_pos = new Point<short> ((short)(623 + pos_adj), 37);

				// Menu
				menu_pos += new Point<short> (-5, 0);
				setting_pos += new Point<short> (-5, 0);
				community_pos += new Point<short> (-5, 0);
				character_pos += new Point<short> (-5, 0);
				event_pos += new Point<short> (-5, 0);
			}
			else if (VWIDTH == 1920)
			{
				quickslot_pos = new Point<short> ((short)(900 + pos_adj), 37);

				// Menu
				menu_pos += new Point<short> (272, 0);
				setting_pos += new Point<short> (272, 0);
				community_pos += new Point<short> (272, 0);
				character_pos += new Point<short> (272, 0);
				event_pos += new Point<short> (272, 0);
			}

			hpmp_sprites.Add (new Sprite (status["backgrnd"], hpmp_pos - new Point<short> (1, 0)));
			hpmp_sprites.Add (new Sprite (status["layer:cover"], hpmp_pos - new Point<short> (1, 0)));

			if (VWIDTH == 800)
			{
				hpmp_sprites.Add (new Sprite (status["layer:Lv"], hpmp_pos));
			}
			else
			{
				hpmp_sprites.Add (new Sprite (status["layer:Lv"], hpmp_pos - new Point<short> (1, 0)));
			}

			short hpmp_max = 139;

			if (VWIDTH > 800)
			{
				hpmp_max += 30;
			}

			hpbar = new Gauge (Gauge.Type.GAME, status.resolve ("gauge/hp/layer:0"), hpmp_max, 0.0f);
			mpbar = new Gauge (Gauge.Type.GAME, status.resolve ("gauge/mp/layer:0"), hpmp_max, 0.0f);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: statset = Charset(EXPBar["number"], Charset::Alignment::RIGHT);
			statset = new Charset (EXPBar["number"], Charset.Alignment.RIGHT);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: hpmpset = Charset(status["gauge"]["number"], Charset::Alignment::RIGHT);
			hpmpset = new Charset (status["gauge"]["number"], Charset.Alignment.RIGHT);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: levelset = Charset(status["lvNumber"], Charset::Alignment::LEFT);
			levelset = new Charset (status["lvNumber"], Charset.Alignment.LEFT);

			namelabel = new OutlinedText (Text.Font.A13M, Text.Alignment.LEFT, Color.Name.GALLERY, Color.Name.TUNA);

			quickslot[0] = quickSlot["backgrnd"];
			quickslot[1] = quickSlot["layer:cover"];

			Point<short> buttonPos = new Point<short> ((short)(591 + pos_adj), 73);

			if (VWIDTH == 1024)
			{
				buttonPos += new Point<short> (38, 0);
			}
			else if (VWIDTH == 1280)
			{
				buttonPos += new Point<short> (31, 0);
			}
			else if (VWIDTH == 1366)
			{
				buttonPos += new Point<short> (33, 0);
			}
			else if (VWIDTH == 1920)
			{
				buttonPos += new Point<short> (310, 0);
			}

			buttons[(int)Buttons.BT_CASHSHOP] = new MapleButton (menu["button:CashShop"], buttonPos);
			buttons[(int)Buttons.BT_MENU] = new MapleButton (menu["button:Menu"], buttonPos);
			buttons[(int)Buttons.BT_OPTIONS] = new MapleButton (menu["button:Setting"], buttonPos);
			buttons[(int)Buttons.BT_CHARACTER] = new MapleButton (menu["button:Character"], buttonPos);
			buttons[(int)Buttons.BT_COMMUNITY] = new MapleButton (menu["button:Community"], buttonPos);
			buttons[(int)Buttons.BT_EVENT] = new MapleButton (menu["button:Event"], buttonPos);

			if (quickslot_active && VWIDTH > 800)
			{
				buttons[(int)Buttons.BT_CASHSHOP].set_active (false);
				buttons[(int)Buttons.BT_MENU].set_active (false);
				buttons[(int)Buttons.BT_OPTIONS].set_active (false);
				buttons[(int)Buttons.BT_CHARACTER].set_active (false);
				buttons[(int)Buttons.BT_COMMUNITY].set_active (false);
				buttons[(int)Buttons.BT_EVENT].set_active (false);
			}

			string fold = "button:Fold";
			string extend = "button:Extend";

			if (VWIDTH == 800)
			{
				fold += "800";
				extend += "800";
			}

			if (VWIDTH == 1366)
			{
				quickslot_qs_adj = new Point<short> (213, 0);
			}
			else
			{
				quickslot_qs_adj = new Point<short> (211, 0);
			}

			if (VWIDTH == 800)
			{
				Point<short> quickslot_qs = new Point<short> (579, 0);

				buttons[(int)Buttons.BT_FOLD_QS] = new MapleButton (quickSlot[fold], quickslot_qs);
				buttons[(int)Buttons.BT_EXTEND_QS] = new MapleButton (quickSlot[extend], quickslot_qs + quickslot_qs_adj);
			}
			else if (VWIDTH == 1024)
			{
				Point<short> quickslot_qs = new Point<short> ((short)(627 + pos_adj), 37);

				buttons[(int)Buttons.BT_FOLD_QS] = new MapleButton (quickSlot[fold], quickslot_qs);
				buttons[(int)Buttons.BT_EXTEND_QS] = new MapleButton (quickSlot[extend], quickslot_qs + quickslot_qs_adj);
			}
			else if (VWIDTH == 1280)
			{
				Point<short> quickslot_qs = new Point<short> ((short)(621 + pos_adj), 37);

				buttons[(int)Buttons.BT_FOLD_QS] = new MapleButton (quickSlot[fold], quickslot_qs);
				buttons[(int)Buttons.BT_EXTEND_QS] = new MapleButton (quickSlot[extend], quickslot_qs + quickslot_qs_adj);
			}
			else if (VWIDTH == 1366)
			{
				Point<short> quickslot_qs = new Point<short> ((short)(623 + pos_adj), 37);

				buttons[(int)Buttons.BT_FOLD_QS] = new MapleButton (quickSlot[fold], quickslot_qs);
				buttons[(int)Buttons.BT_EXTEND_QS] = new MapleButton (quickSlot[extend], quickslot_qs + quickslot_qs_adj);
			}
			else if (VWIDTH == 1920)
			{
				Point<short> quickslot_qs = new Point<short> ((short)(900 + pos_adj), 37);

				buttons[(int)Buttons.BT_FOLD_QS] = new MapleButton (quickSlot[fold], quickslot_qs);
				buttons[(int)Buttons.BT_EXTEND_QS] = new MapleButton (quickSlot[extend], quickslot_qs + quickslot_qs_adj);
			}

			if (quickslot_active)
			{
				buttons[(int)Buttons.BT_EXTEND_QS].set_active (false);
			}
			else
			{
				buttons[(int)Buttons.BT_FOLD_QS].set_active (false);
			}

			#region Menu

			menubackground[0] = submenu["backgrnd"]["0"];
			menubackground[1] = submenu["backgrnd"]["1"];
			menubackground[2] = submenu["backgrnd"]["2"];

			buttons[(int)Buttons.BT_MENU_ACHIEVEMENT] = new MapleButton (submenu["menu"]["button:achievement"], menu_pos);
			buttons[(int)Buttons.BT_MENU_AUCTION] = new MapleButton (submenu["menu"]["button:auction"], menu_pos);
			buttons[(int)Buttons.BT_MENU_BATTLE] = new MapleButton (submenu["menu"]["button:battleStats"], menu_pos);
			buttons[(int)Buttons.BT_MENU_CLAIM] = new MapleButton (submenu["menu"]["button:Claim"], menu_pos);
			buttons[(int)Buttons.BT_MENU_FISHING] = new MapleButton (submenu["menu"]["button:Fishing"], menu_pos + new Point<short> (3, 1));
			buttons[(int)Buttons.BT_MENU_HELP] = new MapleButton (submenu["menu"]["button:Help"], menu_pos);
			buttons[(int)Buttons.BT_MENU_MEDAL] = new MapleButton (submenu["menu"]["button:medal"], menu_pos);
			buttons[(int)Buttons.BT_MENU_MONSTER_COLLECTION] = new MapleButton (submenu["menu"]["button:monsterCollection"], menu_pos);
			buttons[(int)Buttons.BT_MENU_MONSTER_LIFE] = new MapleButton (submenu["menu"]["button:monsterLife"], menu_pos);
			buttons[(int)Buttons.BT_MENU_QUEST] = new MapleButton (submenu["menu"]["button:quest"], menu_pos);
			buttons[(int)Buttons.BT_MENU_UNION] = new MapleButton (submenu["menu"]["button:union"], menu_pos);

			buttons[(int)Buttons.BT_SETTING_CHANNEL] = new MapleButton (submenu["setting"]["button:channel"], setting_pos);
			buttons[(int)Buttons.BT_SETTING_QUIT] = new MapleButton (submenu["setting"]["button:GameQuit"], setting_pos);
			buttons[(int)Buttons.BT_SETTING_JOYPAD] = new MapleButton (submenu["setting"]["button:JoyPad"], setting_pos);
			buttons[(int)Buttons.BT_SETTING_KEYS] = new MapleButton (submenu["setting"]["button:keySetting"], setting_pos);
			buttons[(int)Buttons.BT_SETTING_OPTION] = new MapleButton (submenu["setting"]["button:option"], setting_pos);

			buttons[(int)Buttons.BT_COMMUNITY_PARTY] = new MapleButton (submenu["community"]["button:bossParty"], community_pos);
			buttons[(int)Buttons.BT_COMMUNITY_FRIENDS] = new MapleButton (submenu["community"]["button:friends"], community_pos);
			buttons[(int)Buttons.BT_COMMUNITY_GUILD] = new MapleButton (submenu["community"]["button:guild"], community_pos);
			buttons[(int)Buttons.BT_COMMUNITY_MAPLECHAT] = new MapleButton (submenu["community"]["button:mapleChat"], community_pos);

			buttons[(int)Buttons.BT_CHARACTER_INFO] = new MapleButton (submenu["character"]["button:character"], character_pos);
			buttons[(int)Buttons.BT_CHARACTER_EQUIP] = new MapleButton (submenu["character"]["button:Equip"], character_pos);
			buttons[(int)Buttons.BT_CHARACTER_ITEM] = new MapleButton (submenu["character"]["button:Item"], character_pos);
			buttons[(int)Buttons.BT_CHARACTER_SKILL] = new MapleButton (submenu["character"]["button:Skill"], character_pos);
			buttons[(int)Buttons.BT_CHARACTER_STAT] = new MapleButton (submenu["character"]["button:Stat"], character_pos);

			buttons[(int)Buttons.BT_EVENT_DAILY] = new MapleButton (submenu["event"]["button:dailyGift"], event_pos);
			buttons[(int)Buttons.BT_EVENT_SCHEDULE] = new MapleButton (submenu["event"]["button:schedule"], event_pos);

			for (uint i = (int)Buttons.BT_MENU_QUEST; i <= (ulong)Buttons.BT_EVENT_DAILY; i++)
			{
				buttons[i].set_active (false);
			}

			menutitle[0] = submenu["title"]["character"];
			menutitle[1] = submenu["title"]["community"];
			menutitle[2] = submenu["title"]["event"];
			menutitle[3] = submenu["title"]["menu"];
			menutitle[4] = submenu["title"]["setting"];

			#endregion

			if (VWIDTH == 800)
			{
				position = new Point<short> (0, 480);
				position_x = 410;
				position_y = position.y ();
				dimension = new Point<short> ((short)(VWIDTH - position_x), 140);
			}
			else if (VWIDTH == 1024)
			{
				position = new Point<short> (0, 648);
				position_x = 410;
				position_y = (short)(position.y () + 42);
				dimension = new Point<short> ((short)(VWIDTH - position_x), 75);
			}
			else if (VWIDTH == 1280)
			{
				position = new Point<short> (0, 600);
				position_x = 500;
				position_y = (short)(position.y () + 42);
				dimension = new Point<short> ((short)(VWIDTH - position_x), 75);
			}
			else if (VWIDTH == 1366)
			{
				position = new Point<short> (0, 648);
				position_x = 585;
				position_y = (short)(position.y () + 42);
				dimension = new Point<short> ((short)(VWIDTH - position_x), 75);
			}
			else if (VWIDTH == 1920)
			{
				position = new Point<short> (0, (short)(960 + (VHEIGHT - 1080)));
				position_x = 860;
				position_y = (short)(position.y () + 40);
				dimension = new Point<short> ((short)(VWIDTH - position_x), 80);
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float alpha) const override
		public override void draw (float alpha)
		{
			base.draw_sprites (alpha);

			for (uint i = 0; i <= (ulong)Buttons.BT_EVENT; i++)
			{
				buttons[i].draw (position);
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: hpmp_sprites[0].draw(position, alpha);
			hpmp_sprites[0].draw (position, alpha);

			expbar.draw (position + exp_pos);
			hpbar.draw (position + hpmp_pos);
			mpbar.draw (position + hpmp_pos);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: hpmp_sprites[1].draw(position, alpha);
			hpmp_sprites[1].draw (position, alpha);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: hpmp_sprites[2].draw(position, alpha);
			hpmp_sprites[2].draw (position, alpha);

			short level = (short)stats.get_stat (MapleStat.Id.LEVEL);
			ushort hp = stats.get_stat (MapleStat.Id.HP);
			short mp = (short)stats.get_stat (MapleStat.Id.MP);
			int maxhp = stats.get_total (EquipStat.Id.HP);
			int maxmp = stats.get_total (EquipStat.Id.MP);
			long exp = stats.get_exp ();

			string expstring = Convert.ToString (100 * getexppercent ());

			statset.draw (Convert.ToString (exp) + "[" + expstring.Substring (0, expstring.IndexOf ('.') + 3) + "%]", position + statset_pos);

			hpmpset.draw ("[" + Convert.ToString (hp) + "/" + Convert.ToString (maxhp) + "]", position + hpset_pos);

			hpmpset.draw ("[" + Convert.ToString (mp) + "/" + Convert.ToString (maxmp) + "]", position + mpset_pos);

			levelset.draw (Convert.ToString (level), position + levelset_pos);

			namelabel.draw (position + namelabel_pos);

			buttons[(int)Buttons.BT_FOLD_QS].draw (position + quickslot_adj);
			buttons[(int)Buttons.BT_EXTEND_QS].draw (position + quickslot_adj - quickslot_qs_adj);

			if (VWIDTH > 800 && VWIDTH < 1366)
			{
				quickslot[0].draw (position + quickslot_pos + new Point<short> (-1, 0) + quickslot_adj);
				quickslot[1].draw (position + quickslot_pos + new Point<short> (-1, 0) + quickslot_adj);
			}
			else
			{
				quickslot[0].draw (position + quickslot_pos + quickslot_adj);
				quickslot[1].draw (position + quickslot_pos + quickslot_adj);
			}

			#region Menu

			Point<short> pos_adj = new Point<short> (0, 0);

			if (quickslot_active)
			{
				if (VWIDTH == 800)
				{
					pos_adj += new Point<short> (0, -73);
				}
				else
				{
					pos_adj += new Point<short> (0, -31);
				}
			}

			Point<short> pos = new Point<short> ();
			byte button_count;
			byte menutitle_index;

			if (character_active)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: pos = character_pos;
				pos = (character_pos);
				button_count = 5;
				menutitle_index = 0;
			}
			else if (community_active)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: pos = community_pos;
				pos = (community_pos);
				button_count = 4;
				menutitle_index = 1;
			}
			else if (event_active)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: pos = event_pos;
				pos = (event_pos);
				button_count = 2;
				menutitle_index = 2;
			}
			else if (menu_active)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: pos = menu_pos;
				pos = (menu_pos);
				button_count = 11;
				menutitle_index = 3;
			}
			else if (setting_active)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: pos = setting_pos;
				pos = (setting_pos);
				button_count = 5;
				menutitle_index = 4;
			}
			else
			{
				return;
			}

			Point<short> mid_pos = new Point<short> (0, 29);

			ushort end_y = (ushort)Math.Floor (28.2 * button_count);

			if (menu_active)
			{
				end_y -= 1;
			}

			ushort mid_y = (ushort)(end_y - mid_pos.y ());

			menubackground[0].draw (position + pos + pos_adj);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: menubackground[1].draw(DrawArgument(position + pos + pos_adj) + DrawArgument(mid_pos, Point<short>(0, mid_y)));
			menubackground[1].draw (new DrawArgument (position + pos + pos_adj) + new DrawArgument (mid_pos, new Point<short> (0, (short)mid_y)));
			menubackground[2].draw (position + pos + pos_adj + new Point<short> (0, (short)end_y));

			menutitle[menutitle_index].draw (position + pos + pos_adj);

			for (uint i = (int)Buttons.BT_MENU_QUEST; i <= (ulong)Buttons.BT_EVENT_DAILY; i++)
			{
				buttons[i].draw (position);
			}

			#endregion
		}

		public override void update ()
		{
			base.update ();

			foreach (var sprite in hpmp_sprites)
			{
				sprite.update ();
			}

			expbar.update (getexppercent ());
			hpbar.update (gethppercent ());
			mpbar.update (getmppercent ());

			namelabel.change_text (stats.get_name ());

			Point<short> pos_adj = get_quickslot_pos ();

			if (quickslot_active)
			{
				if (quickslot_adj.x () > quickslot_min)
				{
					short new_x = (short)(quickslot_adj.x () - Constants.TIMESTEP);

					if (new_x < quickslot_min)
					{
						quickslot_adj.set_x (quickslot_min);
					}
					else
					{
						quickslot_adj.shift_x (-Constants.TIMESTEP);
					}
				}
			}
			else
			{
				if (quickslot_adj.x () < QUICKSLOT_MAX)
				{
					short new_x = (short)(quickslot_adj.x () + Constants.TIMESTEP);

					if (new_x > QUICKSLOT_MAX)
					{
						quickslot_adj.set_x (QUICKSLOT_MAX);
					}
					else
					{
						quickslot_adj.shift_x ((short)Constants.TIMESTEP);
					}
				}
			}

			for (uint i = (int)Buttons.BT_MENU_QUEST; i <= (ulong)Buttons.BT_MENU_CLAIM; i++)
			{
				Point<short> menu_adj = new Point<short> (0, 0);

				if (i == (int)Buttons.BT_MENU_FISHING)
				{
					menu_adj = new Point<short> (3, 1);
				}

				buttons[i].set_position (menu_pos + menu_adj + pos_adj);
			}

			for (uint i = (int)Buttons.BT_SETTING_CHANNEL; i <= (ulong)Buttons.BT_SETTING_QUIT; i++)
			{
				buttons[i].set_position (setting_pos + pos_adj);
			}

			for (uint i = (int)Buttons.BT_COMMUNITY_FRIENDS; i <= (ulong)Buttons.BT_COMMUNITY_MAPLECHAT; i++)
			{
				buttons[i].set_position (community_pos + pos_adj);
			}

			for (uint i = (int)Buttons.BT_CHARACTER_INFO; i <= (ulong)Buttons.BT_CHARACTER_ITEM; i++)
			{
				buttons[i].set_position (character_pos + pos_adj);
			}

			for (uint i = (int)Buttons.BT_EVENT_SCHEDULE; i <= (ulong)Buttons.BT_EVENT_DAILY; i++)
			{
				buttons[i].set_position (event_pos + pos_adj);
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_in_range(Point<short> cursorpos) const override
		public override bool is_in_range (Point<short> cursorpos)
		{
			Point<short> pos = new Point<short> ();
			Rectangle<short> bounds = new Rectangle<short> ();

			if (!character_active && !community_active && !event_active && !menu_active && !setting_active)
			{
				pos = new Point<short> (position_x, position_y);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: bounds = Rectangle<short>(pos, pos + dimension);
				bounds = new Rectangle<short> (pos, pos + dimension);
			}
			else
			{
				byte button_count = 0;
				short pos_y_adj = 0;

				if (character_active)
				{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: pos = character_pos;
					pos = (character_pos);
					button_count = 5;
					pos_y_adj = 248;
				}
				else if (community_active)
				{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: pos = community_pos;
					pos = (community_pos);
					button_count = 4;
					pos_y_adj = 301;
				}
				else if (event_active)
				{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: pos = event_pos;
					pos = (event_pos);
					button_count = 2;
					pos_y_adj = 417;
				}
				else if (menu_active)
				{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: pos = menu_pos;
					pos = (menu_pos);
					button_count = 11;
					pos_y_adj = -90;
				}
				else if (setting_active)
				{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: pos = setting_pos;
					pos = (setting_pos);
					button_count = 5;
					pos_y_adj = 248;
				}

				pos_y_adj += (short)(VHEIGHT - 600);

				Point<short> pos_adj = get_quickslot_pos ();
				pos = new Point<short> (pos.x (), (short)(Math.Abs (pos.y ()) + pos_y_adj)) + pos_adj;

				var end_y = Math.Floor (28.2 * button_count);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: bounds = Rectangle<short>(pos, pos + Point<short>(113, end_y + 35));
				bounds = new Rectangle<short> (pos, pos + new Point<short> (113, (short)(end_y + 35)));
			}

			return bounds.contains (cursorpos);
		}

		public override void send_key (int keycode, bool pressed, bool escape)
		{
			if (pressed)
			{
				if (escape)
				{
					if (!menu_active && !setting_active && !community_active && !character_active && !event_active)
					{
						toggle_setting ();
					}
					else
					{
						remove_menus ();
					}
				}
				else if (keycode == (int)KeyAction.Id.RETURN)
				{
					for (uint i = (int)Buttons.BT_MENU_QUEST; i <= (ulong)Buttons.BT_EVENT_DAILY; i++)
					{
						if (buttons[i].get_state () == Button.State.MOUSEOVER)
						{
							button_pressed ((ushort)i);
						}
					}
				}
				else if (keycode == (int)KeyAction.Id.UP || keycode == (int)KeyAction.Id.DOWN)
				{
					ushort min_id = 0;
					ushort max_id = 0;

					if (menu_active)
					{
						min_id = (int)Buttons.BT_MENU_QUEST;
						max_id = (int)Buttons.BT_MENU_CLAIM;
					}
					else if (setting_active)
					{
						min_id = (int)Buttons.BT_SETTING_CHANNEL;
						max_id = (int)Buttons.BT_SETTING_QUIT;
					}
					else if (community_active)
					{
						min_id = (int)Buttons.BT_COMMUNITY_FRIENDS;
						max_id = (int)Buttons.BT_COMMUNITY_MAPLECHAT;
					}
					else if (character_active)
					{
						min_id = (int)Buttons.BT_CHARACTER_INFO;
						max_id = (int)Buttons.BT_CHARACTER_ITEM;
					}
					else if (event_active)
					{
						min_id = (int)Buttons.BT_EVENT_SCHEDULE;
						max_id = (int)Buttons.BT_EVENT_DAILY;
					}

					ushort id = min_id;

					for (uint i = min_id; i <= max_id; i++)
					{
						if (buttons[i].get_state () != Button.State.NORMAL)
						{
							id = (ushort)i;

							buttons[i].set_state (Button.State.NORMAL);
							break;
						}
					}

					if (keycode == (int)KeyAction.Id.DOWN)
					{
						if (id < max_id)
						{
							id++;
						}
						else
						{
							id = min_id;
						}
					}
					else if (keycode == (int)KeyAction.Id.UP)
					{
						if (id > min_id)
						{
							id--;
						}
						else
						{
							id = max_id;
						}
					}

					buttons[id].set_state (Button.State.MOUSEOVER);
				}
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public void toggle_qs ()
		{
			if (!menu_active && !setting_active && !community_active && !character_active && !event_active)
			{
				toggle_qs (!quickslot_active);
			}
		}

		public void toggle_menu ()
		{
			remove_active_menu (MenuType.MENU);

			menu_active = !menu_active;

			buttons[(int)Buttons.BT_MENU_ACHIEVEMENT].set_active (menu_active);
			buttons[(int)Buttons.BT_MENU_AUCTION].set_active (menu_active);
			buttons[(int)Buttons.BT_MENU_BATTLE].set_active (menu_active);
			buttons[(int)Buttons.BT_MENU_CLAIM].set_active (menu_active);
			buttons[(int)Buttons.BT_MENU_FISHING].set_active (menu_active);
			buttons[(int)Buttons.BT_MENU_HELP].set_active (menu_active);
			buttons[(int)Buttons.BT_MENU_MEDAL].set_active (menu_active);
			buttons[(int)Buttons.BT_MENU_MONSTER_COLLECTION].set_active (menu_active);
			buttons[(int)Buttons.BT_MENU_MONSTER_LIFE].set_active (menu_active);
			buttons[(int)Buttons.BT_MENU_QUEST].set_active (menu_active);
			buttons[(int)Buttons.BT_MENU_UNION].set_active (menu_active);

			if (menu_active)
			{
				buttons[(int)Buttons.BT_MENU_QUEST].set_state (Button.State.MOUSEOVER);

				new Sound (Sound.Name.DLGNOTICE).play ();
			}
		}

		public void remove_menus ()
		{
			if (menu_active)
			{
				toggle_menu ();
			}
			else if (setting_active)
			{
				toggle_setting ();
			}
			else if (community_active)
			{
				toggle_community ();
			}
			else if (character_active)
			{
				toggle_character ();
			}
			else if (event_active)
			{
				toggle_event ();
			}
		}

		public bool is_menu_active ()
		{
			return menu_active || setting_active || community_active || character_active || event_active;
		}

		public override Button.State button_pressed (ushort id)
		{
			switch ((Buttons)id)
			{
				case Buttons.BT_CASHSHOP:
					new EnterCashShopPacket ().dispatch ();
					break;
				case Buttons.BT_MENU:
					toggle_menu ();
					break;
				case Buttons.BT_OPTIONS:
					toggle_setting ();
					break;
				case Buttons.BT_CHARACTER:
					toggle_character ();
					break;
				case Buttons.BT_COMMUNITY:
					toggle_community ();
					break;
				case Buttons.BT_EVENT:
					toggle_event ();
					break;
				case Buttons.BT_FOLD_QS:
					toggle_qs (false);
					break;
				case Buttons.BT_EXTEND_QS:
					toggle_qs (true);
					break;
				case Buttons.BT_MENU_QUEST:
					UI.get ().emplace<UIQuestLog> (Stage.get ().get_player ().get_quests ());

					remove_menus ();
					break;
				case Buttons.BT_MENU_MEDAL:
				case Buttons.BT_MENU_UNION:
				case Buttons.BT_MENU_MONSTER_COLLECTION:
				case Buttons.BT_MENU_AUCTION:
				case Buttons.BT_MENU_MONSTER_LIFE:
				case Buttons.BT_MENU_BATTLE:
				case Buttons.BT_MENU_ACHIEVEMENT:
				case Buttons.BT_MENU_FISHING:
				case Buttons.BT_MENU_HELP:
				case Buttons.BT_MENU_CLAIM:
					remove_menus ();
					break;
				case Buttons.BT_SETTING_CHANNEL:
					UI.get ().emplace<UIChannel> ();

					remove_menus ();
					break;
				case Buttons.BT_SETTING_OPTION:
					UI.get ().emplace<UIOptionMenu> ();

					remove_menus ();
					break;
				case Buttons.BT_SETTING_KEYS:
					UI.get ().emplace<UIKeyConfig> (Stage.get ().get_player ().get_inventory (), Stage.get ().get_player ().get_skills ());

					remove_menus ();
					break;
				case Buttons.BT_SETTING_JOYPAD:
					UI.get ().emplace<UIJoypad> ();

					remove_menus ();
					break;
				case Buttons.BT_SETTING_QUIT:
					UI.get ().emplace<UIQuit> (stats);

					remove_menus ();
					break;
				case Buttons.BT_COMMUNITY_FRIENDS:
				case Buttons.BT_COMMUNITY_PARTY:
				{
					var userlist = UI.get ().get_element<UIUserList> ();
					var tab = (id == (int)Buttons.BT_COMMUNITY_FRIENDS) ? UIUserList.Tab.FRIEND : UIUserList.Tab.PARTY;

					if (userlist == null)
					{
						UI.get ().emplace<UIUserList> (tab);
					}
					else
					{
						var cur_tab = userlist.Dereference ().get_tab ();
						var is_active = userlist.Dereference ().is_active ();

						if (cur_tab == (int)tab)
						{
							if (is_active)
							{
								userlist.Dereference ().deactivate ();
							}
							else
							{
								userlist.Dereference ().makeactive ();
							}
						}
						else
						{
							if (!is_active)
							{
								userlist.Dereference ().makeactive ();
							}

							userlist.Dereference ().change_tab ((byte)tab);
						}
					}

					remove_menus ();
				}
					break;
				case Buttons.BT_COMMUNITY_GUILD:
					remove_menus ();
					break;
				case Buttons.BT_COMMUNITY_MAPLECHAT:
					UI.get ().emplace<UIChat> ();

					remove_menus ();
					break;
				case Buttons.BT_CHARACTER_INFO:
					UI.get ().emplace<UICharInfo> (Stage.get ().get_player ().get_oid ());

					remove_menus ();
					break;
				case Buttons.BT_CHARACTER_STAT:
					UI.get ().emplace<UIStatsInfo> (Stage.get ().get_player ().get_stats ());

					remove_menus ();
					break;
				case Buttons.BT_CHARACTER_SKILL:
					UI.get ().emplace<UISkillBook> (Stage.get ().get_player ().get_stats (), Stage.get ().get_player ().get_skills ());

					remove_menus ();
					break;
				case Buttons.BT_CHARACTER_EQUIP:
					UI.get ().emplace<UIEquipInventory> (Stage.get ().get_player ().get_inventory ());

					remove_menus ();
					break;
				case Buttons.BT_CHARACTER_ITEM:
					UI.get ().emplace<UIItemInventory> (Stage.get ().get_player ().get_inventory ());

					remove_menus ();
					break;
				case Buttons.BT_EVENT_SCHEDULE:
					UI.get ().emplace<UIEvent> ();

					remove_menus ();
					break;
				case Buttons.BT_EVENT_DAILY:
					remove_menus ();
					break;
			}

			return Button.State.NORMAL;
		}

		private const short QUICKSLOT_MAX = 211;

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float getexppercent() const
		private float getexppercent ()
		{
			short level = (short)stats.get_stat (MapleStat.Id.LEVEL);

			if (level >= ExpTable.LEVELCAP)
			{
				return 0.0f;
			}

			long exp = stats.get_exp ();

			return (float)((double)exp / ExpTable.values[level]);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float gethppercent() const
		private float gethppercent ()
		{
			short hp = (short)stats.get_stat (MapleStat.Id.HP);
			int maxhp = stats.get_total (EquipStat.Id.HP);

			return (float)hp / maxhp;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float getmppercent() const
		private float getmppercent ()
		{
			short mp = (short)stats.get_stat (MapleStat.Id.MP);
			int maxmp = stats.get_total (EquipStat.Id.MP);

			return (float)mp / maxmp;
		}

		private void toggle_qs (bool quick_slot_active)
		{
			if (quickslot_active == quick_slot_active)
			{
				return;
			}

			quickslot_active = quick_slot_active;
			buttons[(int)Buttons.BT_FOLD_QS].set_active (quickslot_active);
			buttons[(int)Buttons.BT_EXTEND_QS].set_active (!quickslot_active);

			if (VWIDTH > 800)
			{
				buttons[(int)Buttons.BT_CASHSHOP].set_active (!quickslot_active);
				buttons[(int)Buttons.BT_MENU].set_active (!quickslot_active);
				buttons[(int)Buttons.BT_OPTIONS].set_active (!quickslot_active);
				buttons[(int)Buttons.BT_CHARACTER].set_active (!quickslot_active);
				buttons[(int)Buttons.BT_COMMUNITY].set_active (!quickslot_active);
				buttons[(int)Buttons.BT_EVENT].set_active (!quickslot_active);
			}
		}

		private void toggle_setting ()
		{
			remove_active_menu (MenuType.SETTING);

			setting_active = !setting_active;

			buttons[(int)Buttons.BT_SETTING_CHANNEL].set_active (setting_active);
			buttons[(int)Buttons.BT_SETTING_QUIT].set_active (setting_active);
			buttons[(int)Buttons.BT_SETTING_JOYPAD].set_active (setting_active);
			buttons[(int)Buttons.BT_SETTING_KEYS].set_active (setting_active);
			buttons[(int)Buttons.BT_SETTING_OPTION].set_active (setting_active);

			if (setting_active)
			{
				buttons[(int)Buttons.BT_SETTING_CHANNEL].set_state (Button.State.MOUSEOVER);

				new Sound (Sound.Name.DLGNOTICE).play ();
			}
		}

		private void toggle_community ()
		{
			remove_active_menu (MenuType.COMMUNITY);

			community_active = !community_active;

			buttons[(int)Buttons.BT_COMMUNITY_PARTY].set_active (community_active);
			buttons[(int)Buttons.BT_COMMUNITY_FRIENDS].set_active (community_active);
			buttons[(int)Buttons.BT_COMMUNITY_GUILD].set_active (community_active);
			buttons[(int)Buttons.BT_COMMUNITY_MAPLECHAT].set_active (community_active);

			if (community_active)
			{
				buttons[(int)Buttons.BT_COMMUNITY_FRIENDS].set_state (Button.State.MOUSEOVER);

				new Sound (Sound.Name.DLGNOTICE).play ();
			}
		}

		private void toggle_character ()
		{
			remove_active_menu (MenuType.CHARACTER);

			character_active = !character_active;

			buttons[(int)Buttons.BT_CHARACTER_INFO].set_active (character_active);
			buttons[(int)Buttons.BT_CHARACTER_EQUIP].set_active (character_active);
			buttons[(int)Buttons.BT_CHARACTER_ITEM].set_active (character_active);
			buttons[(int)Buttons.BT_CHARACTER_SKILL].set_active (character_active);
			buttons[(int)Buttons.BT_CHARACTER_STAT].set_active (character_active);

			if (character_active)
			{
				buttons[(int)Buttons.BT_CHARACTER_INFO].set_state (Button.State.MOUSEOVER);

				new Sound (Sound.Name.DLGNOTICE).play ();
			}
		}

		private void toggle_event ()
		{
			remove_active_menu (MenuType.EVENT);

			event_active = !event_active;

			buttons[(int)Buttons.BT_EVENT_DAILY].set_active (event_active);
			buttons[(int)Buttons.BT_EVENT_SCHEDULE].set_active (event_active);

			if (event_active)
			{
				buttons[(int)Buttons.BT_EVENT_SCHEDULE].set_state (Button.State.MOUSEOVER);

				new Sound (Sound.Name.DLGNOTICE).play ();
			}
		}

		private void remove_active_menu (MenuType type)
		{
			for (uint i = (int)Buttons.BT_MENU_QUEST; i <= (ulong)Buttons.BT_EVENT_DAILY; i++)
			{
				buttons[i].set_state (Button.State.NORMAL);
			}

			if (menu_active && type != MenuType.MENU)
			{
				toggle_menu ();
			}
			else if (setting_active && type != MenuType.SETTING)
			{
				toggle_setting ();
			}
			else if (community_active && type != MenuType.COMMUNITY)
			{
				toggle_community ();
			}
			else if (character_active && type != MenuType.CHARACTER)
			{
				toggle_character ();
			}
			else if (event_active && type != MenuType.EVENT)
			{
				toggle_event ();
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> get_quickslot_pos() const
		private Point<short> get_quickslot_pos ()
		{
			if (quickslot_active)
			{
				if (VWIDTH == 800)
				{
					return new Point<short> (0, -73);
				}
				else
				{
					return new Point<short> (0, -31);
				}
			}

			return new Point<short> (0, 0);
		}

		private enum Buttons : ushort
		{
			BT_CASHSHOP,
			BT_MENU,
			BT_OPTIONS,
			BT_CHARACTER,
			BT_COMMUNITY,
			BT_EVENT,
			BT_FOLD_QS,
			BT_EXTEND_QS,
			BT_MENU_QUEST,
			BT_MENU_MEDAL,
			BT_MENU_UNION,
			BT_MENU_MONSTER_COLLECTION,
			BT_MENU_AUCTION,
			BT_MENU_MONSTER_LIFE,
			BT_MENU_BATTLE,
			BT_MENU_ACHIEVEMENT,
			BT_MENU_FISHING,
			BT_MENU_HELP,
			BT_MENU_CLAIM,
			BT_SETTING_CHANNEL,
			BT_SETTING_OPTION,
			BT_SETTING_KEYS,
			BT_SETTING_JOYPAD,
			BT_SETTING_QUIT,
			BT_COMMUNITY_FRIENDS,
			BT_COMMUNITY_PARTY,
			BT_COMMUNITY_GUILD,
			BT_COMMUNITY_MAPLECHAT,
			BT_CHARACTER_INFO,
			BT_CHARACTER_STAT,
			BT_CHARACTER_SKILL,
			BT_CHARACTER_EQUIP,
			BT_CHARACTER_ITEM,
			BT_EVENT_SCHEDULE,
			BT_EVENT_DAILY
		}

		private readonly CharStats stats;

		private Gauge expbar = new Gauge ();
		private Gauge hpbar = new Gauge ();
		private Gauge mpbar = new Gauge ();
		private Charset statset = new Charset ();
		private Charset hpmpset = new Charset ();
		private Charset levelset = new Charset ();
		private Texture[] quickslot = new Texture[2];
		private Texture[] menutitle =new Texture[5]; 
		private Texture[] menubackground =new Texture[3]; 
		private OutlinedText namelabel = new OutlinedText ();
		private List<Sprite> hpmp_sprites = new List<Sprite> ();

		private Point<short> exp_pos = new Point<short> ();
		private Point<short> hpmp_pos = new Point<short> ();
		private Point<short> hpset_pos = new Point<short> ();
		private Point<short> mpset_pos = new Point<short> ();
		private Point<short> statset_pos = new Point<short> ();
		private Point<short> levelset_pos = new Point<short> ();
		private Point<short> namelabel_pos = new Point<short> ();
		private Point<short> quickslot_pos = new Point<short> ();
		private Point<short> quickslot_adj = new Point<short> ();
		private Point<short> quickslot_qs_adj = new Point<short> ();
		private Point<short> menu_pos = new Point<short> ();
		private Point<short> setting_pos = new Point<short> ();
		private Point<short> community_pos = new Point<short> ();
		private Point<short> character_pos = new Point<short> ();
		private Point<short> event_pos = new Point<short> ();
		private short quickslot_min;
		private short position_x;
		private short position_y;

		private bool quickslot_active;
		private short VWIDTH;
		private short VHEIGHT;

		private bool menu_active;
		private bool setting_active;
		private bool community_active;
		private bool character_active;
		private bool event_active;
	}
}


#if USE_NX
#endif