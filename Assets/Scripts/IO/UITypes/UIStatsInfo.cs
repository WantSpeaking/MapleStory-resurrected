﻿#define USE_NX

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
	public class UIStatsInfo : UIDragElement<PosSTATS>
	{
		public const Type TYPE = UIElement.Type.STATSINFO;
		public const bool FOCUSED = false;
		public const bool TOGGLED = true;

		public UIStatsInfo (CharStats st) : base (new Point<short> (212, 20))
		{
			//this.UIDragElement<PosSTATS> = new Point<short>(212, 20);
			this.stats = st;
			WzObject close = nl.nx.wzFile_ui["Basic.img"]["BtClose3"];
			WzObject Stat = nl.nx.wzFile_ui["UIWindow4.img"]["Stat"];
			WzObject main = Stat["main"];
			WzObject detail = Stat["detail"];
			WzObject abilityTitle = detail["abilityTitle"];
			WzObject metierLine = detail["metierLine"];

			sprites.Add (main["backgrnd"]);
			sprites.Add (main["backgrnd2"]);
			sprites.Add (main["backgrnd3"]);

			textures_detail.Add (detail["backgrnd"]);
			textures_detail.Add (detail["backgrnd2"]);
			textures_detail.Add (detail["backgrnd3"]);
			textures_detail.Add (detail["backgrnd4"]);

			abilities[(int)Ability.RARE] = abilityTitle["rare"]["0"];
			abilities[(int)Ability.EPIC] = abilityTitle["epic"]["0"];
			abilities[(int)Ability.UNIQUE] = abilityTitle["unique"]["0"];
			abilities[(int)Ability.LEGENDARY] = abilityTitle["legendary"]["0"];
			abilities[(int)Ability.NONE] = abilityTitle["normal"]["0"];

			inner_ability[true] = metierLine["activated"]["0"];
			inner_ability[false] = metierLine["disabled"]["0"];

			buttons[(int)Buttons.BT_CLOSE] = new MapleButton (close, new Point<short> (190, 6));
			buttons[(int)Buttons.BT_HP] = new MapleButton (main["BtHpUp"]);
			buttons[(int)Buttons.BT_MP] = new MapleButton (main["BtMpUp"]);
			buttons[(int)Buttons.BT_STR] = new MapleButton (main["BtStrUp"]);
			buttons[(int)Buttons.BT_DEX] = new MapleButton (main["BtDexUp"]);
			buttons[(int)Buttons.BT_INT] = new MapleButton (main["BtIntUp"]);
			buttons[(int)Buttons.BT_LUK] = new MapleButton (main["BtLukUp"]);
			buttons[(int)Buttons.BT_AUTO] = new MapleButton (main["BtAuto"]);
			buttons[(int)Buttons.BT_HYPERSTATOPEN] = new MapleButton (main["BtHyperStatOpen"]);
			buttons[(int)Buttons.BT_HYPERSTATCLOSE] = new MapleButton (main["BtHyperStatClose"]);
			buttons[(int)Buttons.BT_DETAILOPEN] = new MapleButton (main["BtDetailOpen"]);
			buttons[(int)Buttons.BT_DETAILCLOSE] = new MapleButton (main["BtDetailClose"]);
			buttons[(int)Buttons.BT_ABILITY] = new MapleButton (detail["BtAbility"], new Point<short> (212, 0));
			buttons[(int)Buttons.BT_DETAIL_DETAILCLOSE] = new MapleButton (detail["BtHpUp"], new Point<short> (212, 0));

			buttons[(int)Buttons.BT_HYPERSTATOPEN].set_active (false);
			buttons[(int)Buttons.BT_DETAILCLOSE].set_active (false);
			buttons[(int)Buttons.BT_ABILITY].set_active (false);
			buttons[(int)Buttons.BT_ABILITY].set_state (Button.State.DISABLED);
			buttons[(int)Buttons.BT_DETAIL_DETAILCLOSE].set_active (false);

			update_ap ();

			// Normal
			for (uint i = (int)StatLabel.NAME; i <= (ulong)StatLabel.LUK; i++)
			{
				statlabels[i] = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.EMPEROR);
			}

			statlabels[(int)StatLabel.AP] = new Text (Text.Font.A11M, Text.Alignment.RIGHT, Color.Name.EMPEROR);

			statoffsets[(int)StatLabel.NAME] = new Point<short> (73, 26);
			statoffsets[(int)StatLabel.JOB] = new Point<short> (74, 44);
			statoffsets[(int)StatLabel.GUILD] = new Point<short> (74, 63);
			statoffsets[(int)StatLabel.FAME] = new Point<short> (74, 80);
			statoffsets[(int)StatLabel.DAMAGE] = new Point<short> (74, 98);
			statoffsets[(int)StatLabel.HP] = new Point<short> (74, 116);
			statoffsets[(int)StatLabel.MP] = new Point<short> (74, 134);
			statoffsets[(int)StatLabel.AP] = new Point<short> (91, 175);
			statoffsets[(int)StatLabel.STR] = new Point<short> (73, 204);
			statoffsets[(int)StatLabel.DEX] = new Point<short> (73, 222);
			statoffsets[(int)StatLabel.INT] = new Point<short> (73, 240);
			statoffsets[(int)StatLabel.LUK] = new Point<short> (73, 258);

			// Detailed
			statlabels[(int)StatLabel.DAMAGE_DETAILED] = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.EMPEROR);
			statlabels[(int)StatLabel.DAMAGE_BONUS] = new Text (Text.Font.A11M, Text.Alignment.RIGHT, Color.Name.EMPEROR);
			statlabels[(int)StatLabel.BOSS_DAMAGE] = new Text (Text.Font.A11M, Text.Alignment.RIGHT, Color.Name.EMPEROR);
			statlabels[(int)StatLabel.FINAL_DAMAGE] = new Text (Text.Font.A11M, Text.Alignment.RIGHT, Color.Name.EMPEROR);
			statlabels[(int)StatLabel.IGNORE_DEFENSE] = new Text (Text.Font.A11M, Text.Alignment.RIGHT, Color.Name.EMPEROR);
			statlabels[(int)StatLabel.CRITICAL_RATE] = new Text (Text.Font.A11M, Text.Alignment.RIGHT, Color.Name.EMPEROR);
			statlabels[(int)StatLabel.CRITICAL_DAMAGE] = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.EMPEROR);
			statlabels[(int)StatLabel.STATUS_RESISTANCE] = new Text (Text.Font.A11M, Text.Alignment.RIGHT, Color.Name.EMPEROR);
			statlabels[(int)StatLabel.KNOCKBACK_RESISTANCE] = new Text (Text.Font.A11M, Text.Alignment.RIGHT, Color.Name.EMPEROR);
			statlabels[(int)StatLabel.DEFENSE] = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.EMPEROR);
			statlabels[(int)StatLabel.SPEED] = new Text (Text.Font.A11M, Text.Alignment.RIGHT, Color.Name.EMPEROR);
			statlabels[(int)StatLabel.JUMP] = new Text (Text.Font.A11M, Text.Alignment.RIGHT, Color.Name.EMPEROR);
			statlabels[(int)StatLabel.HONOR] = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.EMPEROR);

			statoffsets[(int)StatLabel.DAMAGE_DETAILED] = new Point<short> (73, 38);
			statoffsets[(int)StatLabel.DAMAGE_BONUS] = new Point<short> (100, 56);
			statoffsets[(int)StatLabel.BOSS_DAMAGE] = new Point<short> (196, 56);
			statoffsets[(int)StatLabel.FINAL_DAMAGE] = new Point<short> (100, 74);
			statoffsets[(int)StatLabel.IGNORE_DEFENSE] = new Point<short> (196, 74);
			statoffsets[(int)StatLabel.CRITICAL_RATE] = new Point<short> (100, 92);
			statoffsets[(int)StatLabel.CRITICAL_DAMAGE] = new Point<short> (73, 110);
			statoffsets[(int)StatLabel.STATUS_RESISTANCE] = new Point<short> (100, 128);
			statoffsets[(int)StatLabel.KNOCKBACK_RESISTANCE] = new Point<short> (196, 128);
			statoffsets[(int)StatLabel.DEFENSE] = new Point<short> (73, 146);
			statoffsets[(int)StatLabel.SPEED] = new Point<short> (100, 164);
			statoffsets[(int)StatLabel.JUMP] = new Point<short> (196, 164);
			statoffsets[(int)StatLabel.HONOR] = new Point<short> (73, 283);

			update_all_stats ();
			update_stat (MapleStat.Id.JOB);
			update_stat (MapleStat.Id.FAME);

			dimension = new Point<short> (212, 318);
			showdetail = false;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float alpha) const override
		public override void draw (float alpha)
		{
			base.draw_sprites (alpha);

			if (showdetail)
			{
				Point<short> detail_pos = position + new Point<short> (212, 0);

				textures_detail[0].draw (detail_pos + new Point<short> (0, -1));
				textures_detail[1].draw (detail_pos);
				textures_detail[2].draw (detail_pos);
				textures_detail[3].draw (detail_pos);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: abilities[Ability.NONE].draw(DrawArgument(detail_pos));
				abilities[(int)Ability.NONE].draw (detail_pos);

				inner_ability[false].draw (detail_pos);
				inner_ability[false].draw (detail_pos + new Point<short> (0, 19));
				inner_ability[false].draw (detail_pos + new Point<short> (0, 38));
			}

			var last = showdetail ? StatLabel.NUM_LABELS : StatLabel.NUM_NORMAL;

			for (var i = 0; i < (int)last; i++)
			{
				Point<short> labelpos = position + statoffsets[i];

				if (i >= (int)StatLabel.NUM_NORMAL)
				{
					labelpos.shift_x (213);
				}

				statlabels[i].draw (labelpos);
			}

			base.draw_buttons (alpha);
		}

		public override void send_key (int keycode, bool pressed, bool escape)
		{
			if (pressed && escape)
			{
				deactivate ();
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_in_range(Point<short> cursorpos) const override
		public override bool is_in_range (Point<short> cursorpos)
		{
			Point<short> pos_adj = new Point<short> ();

			if (showdetail)
			{
				pos_adj = new Point<short> (211, 25);
			}
			else
			{
				pos_adj = new Point<short> (0, 0);
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: var bounds = Rectangle<short>(position, position + dimension + pos_adj);
			var bounds = new Rectangle<short> (position, position + dimension + pos_adj);
			return bounds.contains (cursorpos);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement.Type get_type() const override
		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public void update_all_stats ()
		{
			update_simple (StatLabel.AP, MapleStat.Id.AP);

			if (hasap ^ (stats.get_stat (MapleStat.Id.AP) > 0))
			{
				update_ap ();
			}

			statlabels[(int)StatLabel.NAME].change_text (stats.get_name ());
			statlabels[(int)StatLabel.GUILD].change_text ("-");
			statlabels[(int)StatLabel.HP].change_text (Convert.ToString (stats.get_stat (MapleStat.Id.HP)) + " / " + Convert.ToString (stats.get_total (EquipStat.Id.HP)));
			statlabels[(int)StatLabel.MP].change_text (Convert.ToString (stats.get_stat (MapleStat.Id.MP)) + " / " + Convert.ToString (stats.get_total (EquipStat.Id.MP)));

			update_basevstotal (StatLabel.STR, MapleStat.Id.STR, EquipStat.Id.STR);
			update_basevstotal (StatLabel.DEX, MapleStat.Id.DEX, EquipStat.Id.DEX);
			update_basevstotal (StatLabel.INT, MapleStat.Id.INT, EquipStat.Id.INT);
			update_basevstotal (StatLabel.LUK, MapleStat.Id.LUK, EquipStat.Id.LUK);

			statlabels[(int)StatLabel.DAMAGE].change_text (Convert.ToString (stats.get_mindamage ()) + " ~ " + Convert.ToString (stats.get_maxdamage ()));

			if (stats.is_damage_buffed ())
			{
				statlabels[(int)StatLabel.DAMAGE].change_color (Color.Name.RED);
			}
			else
			{
				statlabels[(int)StatLabel.DAMAGE].change_color (Color.Name.EMPEROR);
			}

			statlabels[(int)StatLabel.DAMAGE_DETAILED].change_text (Convert.ToString (stats.get_mindamage ()) + " ~ " + Convert.ToString (stats.get_maxdamage ()));
			statlabels[(int)StatLabel.DAMAGE_BONUS].change_text ("0%");
			statlabels[(int)StatLabel.BOSS_DAMAGE].change_text (Convert.ToString ((int)(stats.get_bossdmg () * 100)) +"%");
			statlabels[(int)StatLabel.FINAL_DAMAGE].change_text ("0%");
			statlabels[(int)StatLabel.IGNORE_DEFENSE].change_text (Convert.ToString ((int)stats.get_ignoredef ()) +"%");
			statlabels[(int)StatLabel.CRITICAL_RATE].change_text (Convert.ToString ((int)(stats.get_critical () * 100)) +"%");
			statlabels[(int)StatLabel.CRITICAL_DAMAGE].change_text ("0.00%");
			statlabels[(int)StatLabel.STATUS_RESISTANCE].change_text (Convert.ToString ((int)stats.get_resistance ()));
			statlabels[(int)StatLabel.KNOCKBACK_RESISTANCE].change_text ("0%");

			update_buffed (StatLabel.DEFENSE, EquipStat.Id.WDEF);

			statlabels[(int)StatLabel.SPEED].change_text (Convert.ToString (stats.get_total (EquipStat.Id.SPEED)) + "%");
			statlabels[(int)StatLabel.JUMP].change_text (Convert.ToString (stats.get_total (EquipStat.Id.JUMP)) + "%");
			statlabels[(int)StatLabel.HONOR].change_text (Convert.ToString (stats.get_honor ()));
		}

		public void update_stat (MapleStat.Id stat)
		{
			switch (stat)
			{
				case MapleStat.Id.JOB:
					statlabels[(int)StatLabel.JOB].change_text (stats.get_jobname ());
					break;
				case MapleStat.Id.FAME:
					update_simple (StatLabel.FAME, MapleStat.Id.FAME);
					break;
			}
		}

		public override Button.State button_pressed (ushort id)
		{
			Player player = Stage.get ().get_player ();

			switch ((Buttons)id)
			{
				case Buttons.BT_CLOSE:
				{
					deactivate ();
					break;
				}
				case Buttons.BT_HP:
				{
					send_apup (MapleStat.Id.HP);
					break;
				}
				case Buttons.BT_MP:
				{
					send_apup (MapleStat.Id.MP);
					break;
				}
				case Buttons.BT_STR:
				{
					send_apup (MapleStat.Id.STR);
					break;
				}
				case Buttons.BT_DEX:
				{
					send_apup (MapleStat.Id.DEX);
					break;
				}
				case Buttons.BT_INT:
				{
					send_apup (MapleStat.Id.INT);
					break;
				}
				case Buttons.BT_LUK:
				{
					send_apup (MapleStat.Id.LUK);
					break;
				}
				case Buttons.BT_AUTO:
				{
					ushort varstr = 0;
					ushort vardex = 0;
					ushort varint = 0;
					ushort varluk = 0;
					ushort nowap = stats.get_stat (MapleStat.Id.AP);
					EquipStat.Id equipStatId = player.get_stats ().get_job ().get_primary (player.get_weapontype ());

					switch (equipStatId)
					{
						case EquipStat.Id.STR:
							varstr = nowap;
							break;
						case EquipStat.Id.DEX:
							vardex = nowap;
							break;
						case EquipStat.Id.INT:
							varint = nowap;
							break;
						case EquipStat.Id.LUK:
							varluk = nowap;
							break;
					}

					string message = "Your AP will be distributed as follows:\\r" + "\\nSTR : +" + Convert.ToString (varstr) + "\\nDEX : +" + Convert.ToString (vardex) + "\\nINT : +" + Convert.ToString (varint) + "\\nLUK : +" + Convert.ToString (varluk) + "\\r\\n";

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Only lambda expressions having all locals passed by reference can be converted to C#:
//ORIGINAL LINE: System.Action<bool> yesnohandler = [&, varstr, vardex, varint, varluk](bool yes)
					System.Action<bool> yesnohandler = (bool yes) =>
					{
						if (yes)
						{
							if (varstr > 0)
							{
								for (uint i = 0; i < varstr; i++)
								{
									send_apup (MapleStat.Id.STR);
								}
							}

							if (vardex > 0)
							{
								for (uint i = 0; i < vardex; i++)
								{
									send_apup (MapleStat.Id.DEX);
								}
							}

							if (varint > 0)
							{
								for (uint i = 0; i < varint; i++)
								{
									send_apup (MapleStat.Id.INT);
								}
							}

							if (varluk > 0)
							{
								for (uint i = 0; i < varluk; i++)
								{
									send_apup (MapleStat.Id.LUK);
								}
							}
						}
					};

					UI.get ().emplace<UIYesNo> (message, yesnohandler, Text.Alignment.LEFT);
					break;
				}
				case Buttons.BT_HYPERSTATOPEN:
				{
					break;
				}
				case Buttons.BT_HYPERSTATCLOSE:
				{
					if (player.get_level () < 140)
					{
						UI.get ().emplace<UIOk> ("You can use the Hyper Stat at Lv. 140 and above.", null);
					}

					break;
				}
				case Buttons.BT_DETAILOPEN:
				{
					set_detail (true);
					break;
				}
				case Buttons.BT_DETAILCLOSE:
				case Buttons.BT_DETAIL_DETAILCLOSE:
				{
					set_detail (false);
					break;
				}
				case Buttons.BT_ABILITY:
				default:
				{
					break;
				}
			}

			return Button.State.NORMAL;
		}


		void update_ap ()
		{
			bool nowap = stats.get_stat(MapleStat.Id.AP) > 0;
			Button.State newstate = nowap ? Button.State.NORMAL : Button.State.DISABLED;

			for (int i = (int)Buttons.BT_HP; i <= (int)Buttons.BT_AUTO; i++)
				buttons[(uint)i].set_state(newstate);

			hasap = nowap;
		}

		void update_simple (StatLabel label, MapleStat.Id stat)
		{
			statlabels[(int)label].change_text(stats.get_stat(stat).ToString());
		}

		void update_basevstotal (StatLabel label, MapleStat.Id bstat, EquipStat.Id tstat)
		{
			int baseStat = stats.get_stat(bstat);
			int total = stats.get_total(tstat);
			int delta = total - baseStat;

			string stattext = total.ToString();

			if (delta!=0)
			{
				stattext += " (" + baseStat;

				if (delta > 0)
					stattext += "+" + delta;
				else if (delta < 0)
					stattext += "-" + -delta;

				stattext += ")";
			}

			statlabels[(int)label].change_text(stattext);
		}

		void update_buffed (StatLabel label, EquipStat.Id stat)
		{
			int total = stats.get_total(stat);
			int delta = stats.get_buffdelta(stat);

			string stattext = total.ToString();

			if (delta!=0)
			{
				stattext += " (" + (total - delta);

				if (delta > 0)
				{
					stattext += "+" + (delta);

					statlabels[(int)label].change_color(Color.Name.RED);
				}
				else if (delta < 0)
				{
					stattext += "-" + (-delta);

					statlabels[(int)label].change_color(Color.Name.BLUE);
				}

				stattext += ")";
			}

			statlabels[(int)label].change_text(stattext);
		}
		void send_apup (MapleStat.Id stat)
		{
			new SpendApPacket(stat).dispatch();
			UI.get().disable();
		}
		void set_detail (bool enabled)
		{
			showdetail = enabled;

			buttons[(uint)Buttons.BT_DETAILOPEN].set_active(!enabled);
			buttons[(uint)Buttons.BT_DETAILCLOSE].set_active(enabled);
			buttons[(uint)Buttons.BT_ABILITY].set_active(enabled);
			buttons[(uint)Buttons.BT_DETAIL_DETAILCLOSE].set_active(enabled);
		}

		CharStats stats;


		Texture[] abilities = new Texture[(int)Ability.NUM_ABILITIES];
		BoolPair<Texture> inner_ability = new BoolPair<Texture> ();

		List<Texture> textures_detail = new List<Texture> ();
		bool showdetail;

		bool hasap;

		Text[] statlabels = new Text[(int)StatLabel.NUM_LABELS];
		Point<short>[] statoffsets = new Point<short>[(int)StatLabel.NUM_LABELS];
	}

	public enum StatLabel
	{
		// Normal
		NAME,
		JOB,
		GUILD,
		FAME,
		DAMAGE,
		HP,
		MP,
		AP,
		STR,
		DEX,
		INT,
		LUK,
		NUM_NORMAL,

		// Detailed
		DAMAGE_DETAILED,
		DAMAGE_BONUS,
		BOSS_DAMAGE,
		FINAL_DAMAGE,
		IGNORE_DEFENSE,
		CRITICAL_RATE,
		CRITICAL_DAMAGE,
		STATUS_RESISTANCE,
		KNOCKBACK_RESISTANCE,
		DEFENSE,
		SPEED,
		JUMP,
		HONOR,

		// Total
		NUM_LABELS
	}

	public enum Buttons
	{
		BT_CLOSE,
		BT_HP,
		BT_MP,
		BT_STR,
		BT_DEX,
		BT_INT,
		BT_LUK,
		BT_AUTO,
		BT_HYPERSTATOPEN,
		BT_HYPERSTATCLOSE,
		BT_DETAILOPEN,
		BT_DETAILCLOSE,
		BT_ABILITY,
		BT_DETAIL_DETAILCLOSE
	}

	public enum Ability
	{
		RARE,
		EPIC,
		UNIQUE,
		LEGENDARY,
		NONE,
		NUM_ABILITIES
	}
}


#if USE_NX
#endif

namespace ms
{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void UIStatsInfo.send_apup(MapleStat.Id stat) const
}