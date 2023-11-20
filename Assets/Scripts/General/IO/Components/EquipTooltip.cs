#define USE_NX

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapleLib.WzLib;




namespace ms
{
	public class EquipTooltip : Tooltip
	{
		public EquipTooltip ()
		{
			/*WzObject Item = ms.wz.wzProvider_ui["UIToolTip.img"]["Item"];
			WzObject Frame = Item["Frame"];
			WzObject ItemIcon = Item["ItemIcon"];
			WzObject Equip = Item["Equip"];
			WzObject EquipCan = Equip["Can"];
			WzObject EquipCannot = Equip["Cannot"];

			top = Frame["top"];
			mid = Frame["line"];
			line = Frame["dotline"];
			bot = Frame["bottom"];
			baseTexture = ItemIcon["base"];
			cover = Frame["cover"];
			itemcover = ItemIcon["cover"];
			type[true] = ItemIcon["new"];
			type[false] = ItemIcon["old"];

			potential[ms.Equip.Potential.POT_NONE] = new Texture ();
			potential[ms.Equip.Potential.POT_HIDDEN] = ItemIcon["0"];
			potential[ms.Equip.Potential.POT_RARE] = ItemIcon["1"];
			potential[ms.Equip.Potential.POT_EPIC] = ItemIcon["2"];
			potential[ms.Equip.Potential.POT_UNIQUE] = ItemIcon["3"];
			potential[ms.Equip.Potential.POT_LEGENDARY] = ItemIcon["4"];*/ 

			requirements.Add (MapleStat.Id.LEVEL);
			requirements.Add (MapleStat.Id.STR);
			requirements.Add (MapleStat.Id.DEX);
			requirements.Add (MapleStat.Id.INT);
			requirements.Add (MapleStat.Id.LUK);

            /*reqstattextures[MapleStat.Id.LEVEL][false] = EquipCannot["reqLEV"];
			reqstattextures[MapleStat.Id.LEVEL][true] = EquipCan["reqLEV"];
			reqstattextures[MapleStat.Id.FAME][false] = EquipCannot["reqPOP"];
			reqstattextures[MapleStat.Id.FAME][true] = EquipCan["reqPOP"];
			reqstattextures[MapleStat.Id.STR][false] = EquipCannot["reqSTR"];
			reqstattextures[MapleStat.Id.STR][true] = EquipCan["reqSTR"];
			reqstattextures[MapleStat.Id.DEX][false] = EquipCannot["reqDEX"];
			reqstattextures[MapleStat.Id.DEX][true] = EquipCan["reqDEX"];
			reqstattextures[MapleStat.Id.INT][false] = EquipCannot["reqINT"];
			reqstattextures[MapleStat.Id.INT][true] = EquipCan["reqINT"];
			reqstattextures[MapleStat.Id.LUK][false] = EquipCannot["reqLUK"];
			reqstattextures[MapleStat.Id.LUK][true] = EquipCan["reqLUK"];

			reqstatpositions[MapleStat.Id.LEVEL] = new Point_short (97, 47);
			reqstatpositions[MapleStat.Id.STR] = new Point_short (97, 62);
			reqstatpositions[MapleStat.Id.LUK] = new Point_short (177, 62);
			reqstatpositions[MapleStat.Id.DEX] = new Point_short (97, 71);
			reqstatpositions[MapleStat.Id.INT] = new Point_short (177, 71);


			reqset[false] = new Charset (EquipCannot, Charset.Alignment.LEFT);
			reqset[true] = new Charset (EquipCan, Charset.Alignment.LEFT);
			lvset[false] = new Charset (EquipCannot, Charset.Alignment.LEFT);
			lvset[true] = new Charset (Equip["YellowNumber"], Charset.Alignment.LEFT);
			atkincset[false] = new Charset (Equip["Summary"]["decline"], Charset.Alignment.RIGHT);
			atkincset[true] = new Charset (Equip["Summary"]["incline"], Charset.Alignment.RIGHT);

            jobsback = Equip["Job"]["normal"];
			jobs[false][0] = Equip["Job"]["disable"]["0"];
			jobs[false][1] = Equip["Job"]["disable"]["1"];
			jobs[false][2] = Equip["Job"]["disable"]["2"];
			jobs[false][3] = Equip["Job"]["disable"]["3"];
			jobs[false][4] = Equip["Job"]["disable"]["4"];
			jobs[false][5] = Equip["Job"]["disable"]["5"];
			jobs[true][0] = Equip["Job"]["enable"]["0"];
			jobs[true][1] = Equip["Job"]["enable"]["1"];
			jobs[true][2] = Equip["Job"]["enable"]["2"];
			jobs[true][3] = Equip["Job"]["enable"]["3"];
			jobs[true][4] = Equip["Job"]["enable"]["4"];
			jobs[true][5] = Equip["Job"]["enable"]["5"];

			invpos = 0;
			invpos_preview = 0;*/
        }

        public bool hasEquip;
		public void set_equip (Tooltip.Parent parent, short ivp)
		{
			/*if (invpos == ivp)
			{
				return;
			}*/

			invpos = ivp;
			invpos_preview = 0;

			Player player = Stage.get ().get_player ();

			InventoryType.Id invtype;

			switch (parent)
			{
				case Tooltip.Parent.ITEMINVENTORY:
				case Tooltip.Parent.SHOP:
					invtype = InventoryType.Id.EQUIP;
					break;
				case Tooltip.Parent.EQUIPINVENTORY:
					invtype = InventoryType.Id.EQUIPPED;
					break;
				default:
					invtype = InventoryType.Id.NONE;
					break;
			}

			Inventory inventory = player.get_inventory ();
			var oequip = inventory.get_equip (invtype, invpos);
			CharStats stats = player.get_stats ();
			hasEquip = oequip == true;

			if (oequip == false)
			{
				return;
			}

			if (invtype == InventoryType.Id.EQUIP)
			{
				int item_id1 = oequip.get ().get_item_id ();
				EquipData equipdata1 = EquipData.get (item_id1);
				EquipSlot.Id eqslot = equipdata1.get_eqslot ();

				if (inventory.has_equipped (eqslot))
				{
					var eequip = inventory.get_equip (InventoryType.Id.EQUIPPED, (short)eqslot);

					if (eequip)
					{
						Equip equip1 = eequip.get ();

						int item_id2 = equip1.get_item_id ();

						EquipData equipdata2 = EquipData.get (item_id2);
						ItemData itemdata1 = equipdata2.get_itemdata ();

						height_preview = 540;

						itemicon_preview = new Texture(itemdata1.get_icon (false));

						foreach (var ms in requirements)
						{
							canequip_preview[ms] = stats.get_stat (ms) >= equipdata1.get_reqstat (ms);
							string reqstr = Convert.ToString (equipdata1.get_reqstat (ms));

							if (ms != MapleStat.Id.LEVEL)
							{
								reqstr = string_format.extend_id (equipdata1.get_reqstat (ms), 3);
								//reqstr = reqstr.Insert(0, 3 - reqstr.Length, '0');
							}

							reqstatstrings_preview[ms] = reqstr;
						}

						okjobs_preview.Clear ();

						switch (equipdata2.get_reqstat (MapleStat.Id.JOB)/100)
						{
							case 0:
								okjobs_preview.Add (0);
								okjobs_preview.Add (1);
								okjobs_preview.Add (2);
								okjobs_preview.Add (3);
								okjobs_preview.Add (4);
								okjobs_preview.Add (5);
								canequip_preview[MapleStat.Id.JOB] = true;
								break;
							case 1:
								okjobs_preview.Add (1);
								canequip_preview[MapleStat.Id.JOB] = (stats.get_stat (MapleStat.Id.JOB) / 100 == 1) || (stats.get_stat (MapleStat.Id.JOB) / 100 >= 20);
								break;
							case 2:
								okjobs_preview.Add (2);
								canequip_preview[MapleStat.Id.JOB] = stats.get_stat (MapleStat.Id.JOB) / 100 == 2;
								break;
							case 4:
								okjobs_preview.Add (3);
								canequip_preview[MapleStat.Id.JOB] = stats.get_stat (MapleStat.Id.JOB) / 100 == 3;
								break;
							case 8:
								okjobs_preview.Add (4);
								canequip_preview[MapleStat.Id.JOB] = stats.get_stat (MapleStat.Id.JOB) / 100 == 4;
								break;
							case 16:
								okjobs_preview.Add (5);
								canequip_preview[MapleStat.Id.JOB] = stats.get_stat (MapleStat.Id.JOB) / 100 == 5;
								break;
							default:
								canequip_preview[MapleStat.Id.JOB] = false;
								break;
						}

						prank_preview = equip1.get_potrank ();

						switch (prank_preview)
						{
							case Equip.Potential.POT_HIDDEN:
								potflag_preview = new Text (Text.Font.A11M, Text.Alignment.CENTER, Color.Name.RED);
								potflag_preview.change_text ("(Hidden Potential)");
								break;
							case Equip.Potential.POT_RARE:
								potflag_preview = new Text (Text.Font.A11M, Text.Alignment.CENTER, Color.Name.WHITE);
								potflag_preview.change_text ("(Rare Item)");
								break;
							case Equip.Potential.POT_EPIC:
								potflag_preview = new Text (Text.Font.A11M, Text.Alignment.CENTER, Color.Name.WHITE);
								potflag_preview.change_text ("(Epic Item)");
								break;
							case Equip.Potential.POT_UNIQUE:
								potflag_preview = new Text (Text.Font.A11M, Text.Alignment.CENTER, Color.Name.WHITE);
								potflag_preview.change_text ("(Unique Item)");
								break;
							case Equip.Potential.POT_LEGENDARY:
								potflag_preview = new Text (Text.Font.A11M, Text.Alignment.CENTER, Color.Name.WHITE);
								potflag_preview.change_text ("(Legendary Item)");
								break;
							default:
								height_preview -= 16;
								break;
						}

						Color.Name namecolor1;

						switch (equip1.get_quality ())
						{
							case EquipQuality.Id.GREY:
								namecolor1 = Color.Name.LIGHTGREY;
								break;
							case EquipQuality.Id.ORANGE:
								namecolor1 = Color.Name.ORANGE;
								break;
							case EquipQuality.Id.BLUE:
								namecolor1 = Color.Name.MEDIUMBLUE;
								break;
							case EquipQuality.Id.VIOLET:
								namecolor1 = Color.Name.VIOLET;
								break;
							case EquipQuality.Id.GOLD:
								namecolor1 = Color.Name.YELLOW;
								break;
							default:
								namecolor1 = Color.Name.WHITE;
								break;
						}

						var namestr1 = new StringBuilder (itemdata1.get_name ());
						sbyte reqGender1 = itemdata1.get_gender ();
						bool female1 = stats.get_female ();

						switch (reqGender1)
						{
							case 0: // Male
								//namestr += " (M)";
								namestr1.Append (" (M)");
								break;
							case 1: // Female
								//namestr += " (F)";
								namestr1.Append (" (F)");
								break;
							case 2: // Unisex
							default:
								break;
						}

						if (equip1.get_level () > 0)
						{
							namestr1.Append (" (+");
							namestr1.Append (Convert.ToString (equip1.get_level ()));
							namestr1.Append (")");
						}

						name_preview = new Text (Text.Font.A12B, Text.Alignment.LEFT, namecolor1, namestr1.ToString (), 400);
						atkinc_preview = new Text (Text.Font.A11M, Text.Alignment.RIGHT, Color.Name.DUSTYGRAY, "ATT INCREASE");

						string desctext1 = itemdata1.get_desc ();
						hasdesc_preview = desctext1.Length > 0;

						if (hasdesc_preview)
						{
							desc_preview = new Text (Text.Font.A12M, Text.Alignment.LEFT, Color.Name.WHITE, desctext1, 250);
							height_preview += (short)(desc_preview.height () + 10);
						}

						category_preview = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.WHITE, "Type: " + equipdata2.get_type ());

						is_weapon_preview = equipdata2.is_weapon ();

						if (is_weapon_preview)
						{
							WeaponData weapon = WeaponData.get (item_id2);
							wepspeed_preview = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.WHITE, "Attack Speed: " + weapon.getspeedstring ());
						}
						else
						{
							height_preview -= 18;
						}

						hasslots_preview = (equip1.get_slots () > 0) || (equip1.get_level () > 0);

						if (hasslots_preview)
						{
							slots_preview = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.WHITE, "Remaining Enhancements: " + Convert.ToString (equip1.get_slots ()));

							string vicious = Convert.ToString (equip1.get_vicious ());

							if (equip1.get_vicious () > 1)
							{
								vicious += (" (MAX) ");
							}

							hammers_preview = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.WHITE, "Hammers Applied: " + vicious);
						}
						else
						{
							height_preview -= 36;
						}

						statlabels_preview.Clear ();

						for (EquipStat.Id es = EquipStat.Id.STR; es <= EquipStat.Id.JUMP; es = (EquipStat.Id)(es + 1))
						{
							if (equip1.get_stat (es) > 0)
							{
								short delta = (short)(equip1.get_stat (es) - equipdata2.get_defstat (es));
								var statstr = new StringBuilder (Convert.ToString (equip1.get_stat (es)));

								if (delta != 0)
								{
									statstr.Append (" (");
									statstr.Append ((delta < 0) ? "-" : "+");
									statstr.Append (Convert.ToString (Math.Abs (delta)) + ")");
								}

								statlabels_preview[es] = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.WHITE, EquipStat.names[(int)es] + ": " + statstr.ToString ());
							}
							else
							{
								height_preview -= 18;
							}
						}

						invpos_preview = 1;
					}
				}
			}


		

			Equip equip2 = oequip.get ();

			int item_id3 = equip2.get_item_id ();

			EquipData equipdata3 = EquipData.get (item_id3);
			ItemData itemdata2 = equipdata3.get_itemdata ();

			height = 540;

			itemicon = new Texture(itemdata2.get_icon (false));

			foreach (var ms in requirements)
			{
				canequip[ms] = stats.get_stat (ms) >= equipdata3.get_reqstat (ms);
				string reqstr = Convert.ToString (equipdata3.get_reqstat (ms));

				if (ms != MapleStat.Id.LEVEL)
				{
					//reqstr = string_format.extend_id (equipdata3.get_reqstat (ms), 3);
					//reqstr = reqstr.insert (0, 3 - reqstr.Length, '0');
				}

				reqstatstrings[ms] = reqstr;
			}

			okjobs.Clear ();

			switch (equipdata3.get_reqstat (MapleStat.Id.JOB))
			{
				case 0:
					okjobs.Add (0);
					okjobs.Add (1);
					okjobs.Add (2);
					okjobs.Add (3);
					okjobs.Add (4);
					okjobs.Add (5);
					canequip[MapleStat.Id.JOB] = true;
					break;
				case 1:
					okjobs.Add (1);
					canequip[MapleStat.Id.JOB] = (stats.get_stat (MapleStat.Id.JOB) / 100 == 1) || (stats.get_stat (MapleStat.Id.JOB) / 100 >= 20);
					break;
				case 2:
					okjobs.Add (2);
					canequip[MapleStat.Id.JOB] = stats.get_stat (MapleStat.Id.JOB) / 100 == 2;
					break;
				case 4:
					okjobs.Add (3);
					canequip[MapleStat.Id.JOB] = stats.get_stat (MapleStat.Id.JOB) / 100 == 3;
					break;
				case 8:
					okjobs.Add (4);
					canequip[MapleStat.Id.JOB] = stats.get_stat (MapleStat.Id.JOB) / 100 == 4;
					break;
				case 16:
					okjobs.Add (5);
					canequip[MapleStat.Id.JOB] = stats.get_stat (MapleStat.Id.JOB) / 100 == 5;
					break;
				default:
					canequip[MapleStat.Id.JOB] = false;
					break;
			}

			prank = equip2.get_potrank ();//稀有 传奇 物品的标记

			switch (prank)
			{
				case Equip.Potential.POT_HIDDEN:
					potflag = new Text (Text.Font.A11M, Text.Alignment.CENTER, Color.Name.RED);
					potflag.change_text ("(Hidden Potential)");
					break;
				case Equip.Potential.POT_RARE:
					potflag = new Text (Text.Font.A11M, Text.Alignment.CENTER, Color.Name.WHITE);
					potflag.change_text ("(Rare Item)");
					break;
				case Equip.Potential.POT_EPIC:
					potflag = new Text (Text.Font.A11M, Text.Alignment.CENTER, Color.Name.WHITE);
					potflag.change_text ("(Epic Item)");
					break;
				case Equip.Potential.POT_UNIQUE:
					potflag = new Text (Text.Font.A11M, Text.Alignment.CENTER, Color.Name.WHITE);
					potflag.change_text ("(Unique Item)");
					break;
				case Equip.Potential.POT_LEGENDARY:
					potflag = new Text (Text.Font.A11M, Text.Alignment.CENTER, Color.Name.WHITE);
					potflag.change_text ("(Legendary Item)");
					break;
				default:
					height -= 16;
					break;
			}

			Color.Name namecolor;

			switch (equip2.get_quality ())
			{
				case EquipQuality.Id.GREY:
					namecolor = Color.Name.LIGHTGREY;
					break;
				case EquipQuality.Id.ORANGE:
					namecolor = Color.Name.ORANGE;
					break;
				case EquipQuality.Id.BLUE:
					namecolor = Color.Name.MEDIUMBLUE;
					break;
				case EquipQuality.Id.VIOLET:
					namecolor = Color.Name.VIOLET;
					break;
				case EquipQuality.Id.GOLD:
					namecolor = Color.Name.YELLOW;
					break;
				default:
					namecolor = Color.Name.WHITE;
					break;
			}

			var namestr2 = new StringBuilder (itemdata2.get_name ());
			sbyte reqGender2 = itemdata2.get_gender ();
			bool female2 = stats.get_female ();

			switch (reqGender2)
			{
				case 0: // Male
					namestr2.Append ($" {TestLuban.Get().GetL10nText("(M)")}");
					break;
				case 1: // Female
					namestr2.Append ($" {TestLuban.Get().GetL10nText("(F)")}");
					break;
				case 2: // Unisex
				default:
					break;
			}

			if (equip2.get_level () > 0)
			{
				namestr2.Append (" (+");
				namestr2.Append (Convert.ToString (equip2.get_level ()));
				namestr2.Append (")");
			}

			name = new Text (Text.Font.A12B, Text.Alignment.LEFT, namecolor, namestr2.ToString (), 400);
			atkinc = new Text (Text.Font.A11M, Text.Alignment.RIGHT, Color.Name.DUSTYGRAY, "ATT INCREASE");

			string desctext2 = itemdata2.get_desc ();
			hasdesc = desctext2.Length > 0;

			if (hasdesc)
			{
				desc = new Text (Text.Font.A12M, Text.Alignment.LEFT, Color.Name.WHITE, desctext2, 250);
				height += (short)(desc.height () + 10);
			}

			category = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.WHITE, TestLuban.Get().GetL10nText("Type")+": " + TestLuban.Get().GetL10nText(equipdata3.get_type()));

			is_weapon = equipdata3.is_weapon ();

			if (is_weapon)
			{
				WeaponData weapon = WeaponData.get (item_id3);
				wepspeed = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.WHITE, TestLuban.Get().GetL10nText("Attack Speed") +": " + weapon.getspeedstring ());
			}
			else
			{
				height -= 18;
			}

			hasslots = (equip2.get_slots () > 0) || (equip2.get_level () > 0);

			if (hasslots)
			{
				slots = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.WHITE, TestLuban.Get().GetL10nText("Remaining Enhancements")+": " + Convert.ToString (equip2.get_slots ()));

				string vicious = Convert.ToString (equip2.get_vicious ());

				if (equip2.get_vicious () > 1)
				{
					vicious += (" (MAX) ");
				}

				hammers = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.WHITE, TestLuban.Get().GetL10nText("Hammers Applied") + ": " + vicious);
			}
			else
			{
				height -= 36;
			}

			statlabels.Clear ();

			for (EquipStat.Id es = EquipStat.Id.STR; es <= EquipStat.Id.JUMP; es = (EquipStat.Id)(es + 1))
			{
				if (equip2.get_stat (es) > 0)
				{
					short delta = (short)(equip2.get_stat (es) - equipdata3.get_defstat (es));
					var statstr = new StringBuilder ().Append(equip2.get_stat (es));

					if (delta != 0)
					{
						statstr.Append (" (");
						statstr.Append ((delta < 0) ? "-" : "+");
						statstr.Append (Convert.ToString (Math.Abs (delta)) + ")");
					}

					statlabels[es] = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.WHITE, TestLuban.Get().GetL10nText(EquipStat.names[(int)es])  + ": " + statstr.ToString());
				}
				else
				{
					height -= 18;
				}
			}
		}

		public override void draw (Point_short pos)
		{
			/*if (invpos == 0)
			{
				return;
			}


			draw_preview (new Point_short (pos));

			short max_width = Constants.get ().get_viewwidth ();
			short max_height = Constants.get ().get_viewheight ();
			short cur_width = (short)(pos.x () + top.width ());
			short cur_height = (short)(pos.y () + 36);

			if (invpos_preview == 1)
			{
				cur_width += top.width ();
			}

			if (invpos_preview == 1)
			{
				cur_height += (height > height_preview) ? height : height_preview;
			}
			else
			{
				cur_height += height;
			}

			short adj_x = (short)(cur_width - max_width);
			short adj_y = (short)(cur_height - max_height);

			if (adj_x > 0)
			{
				pos.shift_x ((short)(adj_x * -1));
			}

			if (adj_y > 0)
			{
				pos.shift_y ((short)(adj_y * -1));
			}

			top.draw (pos);
			mid.draw (new DrawArgument (pos + new Point_short (0, 13), new Point_short (0, height)));
			bot.draw (pos + new Point_short (0, (short)(height + 13)));
			cover.draw (pos);

			name.draw (pos + new Point_short (17, 7));

			if (prank != Equip.Potential.POT_NONE)
			{
				potflag.draw (pos + new Point_short (130, 20));
				pos.shift_y (16);
			}

			pos.shift_y (44);

			line.draw (pos);

			atkinc.draw (pos + new Point_short (248, 4));
			baseTexture.draw (pos + new Point_short (12, 10));
			type[false].draw (pos + new Point_short (12, 10));
			itemicon.draw (new DrawArgument (pos + new Point_short (18, 82), 2.0f, 2.0f));
			potential[prank].draw (pos + new Point_short (12, 10));
			itemcover.draw (pos + new Point_short (12, 10));

			short atkincnum = 0;
			string atkincstr = Convert.ToString (atkincnum);
			bool atkinc_pos = true;

			if (canequip[MapleStat.Id.JOB])
			{
				if (atkincnum < 0)
				{
					atkincstr = "m" + atkincstr;
					atkinc_pos = false;
				}
				else if (atkincnum > 0)
				{
					atkincstr = "p" + atkincstr;
					atkinc_pos = true;
				}
				else
				{
					atkinc_pos = true;
				}
			}
			else
			{
				atkincstr = "m";
				atkinc_pos = false;
			}

			atkincset[atkinc_pos].draw (atkincstr, 11, pos + new Point_short (239, 26));

			pos.shift_y (12);

			foreach (MapleStat.Id ms in requirements)
			{
				Point_short reqpos = reqstatpositions[ms];
				bool reqok = canequip[ms];
				reqstattextures[ms][reqok].draw (pos + reqpos);

				if (ms != MapleStat.Id.LEVEL)
				{
					reqset[reqok].draw (reqstatstrings[ms], 6, pos + reqpos + new Point_short (54, 0));
				}
				else
				{
					lvset[reqok].draw (reqstatstrings[ms], 6, pos + reqpos + new Point_short (54, 0));
				}
			}

			pos.shift_y (88);

			Point_short job_position = (pos + new Point_short (10, 14));
			jobsback.draw (job_position);

			foreach (var jbit in okjobs)
			{
				jobs[canequip[MapleStat.Id.JOB]][jbit].draw (job_position);
			}

			line.draw (pos + new Point_short (0, 47));

			pos.shift_y (49);

			short stat_x = 13;
			short stat_y = 15;

			category.draw (pos + new Point_short (stat_x, 0));

			pos.shift_y (stat_y);

			if (is_weapon)
			{
				wepspeed.draw (pos + new Point_short (stat_x, 0));
				pos.shift_y (stat_y);
			}

			foreach (Text label in statlabels.dict.Values)
			{
				if (label.empty ())
				{
					continue;
				}

				label.draw (pos + new Point_short (stat_x, 0));
				pos.shift_y (stat_y);
			}

			if (hasslots)
			{
				slots.draw (pos + new Point_short (stat_x, 0));
				pos.shift_y (stat_y);
				hammers.draw (pos + new Point_short (stat_x, 0));
				pos.shift_y (stat_y);
			}

			if (hasdesc)
			{
				pos.shift_y (13);
				line.draw (pos);
				desc.draw (pos + new Point_short (9, 3));
			}*/
		}

		public void draw_preview (Point_short pos)
		{
			/*if (invpos_preview == 0)
			{
				return;
			}

			pos.shift_x (top.width ());

			short max_width = Constants.get ().get_viewwidth ();
			short max_height = Constants.get ().get_viewheight ();
			short cur_width = (short)(pos.x () + top.width ());
			short cur_height = (short)(pos.y () + height_preview + 36);

			short adj_x = (short)(cur_width - max_width);
			short adj_y = (short)(cur_height - max_height);

			if (adj_x > 0)
			{
				pos.shift_x ((short)(adj_x * -1));
			}

			if (adj_y > 0)
			{
				pos.shift_y ((short)(adj_y * -1));
			}

			top.draw (pos);
			mid.draw (new DrawArgument (pos + new Point_short (0, 13), new Point_short (0, height_preview)));
			bot.draw (pos + new Point_short (0, (short)(height_preview + 13)));
			cover.draw (pos);

			name_preview.draw (pos + new Point_short (17, 7));

			if (prank_preview != Equip.Potential.POT_NONE)
			{
				potflag_preview.draw (pos + new Point_short (130, 20));
				pos.shift_y (16);
			}

			pos.shift_y (44);

			line.draw (pos);

			atkinc_preview.draw (pos + new Point_short (248, 4));
			baseTexture.draw (pos + new Point_short (12, 10));
			type[false].draw (pos + new Point_short (12, 10));
			itemicon_preview.draw (new DrawArgument (pos + new Point_short (18, 82), 2.0f, 2.0f));
			potential[prank_preview].draw (pos + new Point_short (12, 10));
			itemcover.draw (pos + new Point_short (12, 10));

			short atkincnum = 0;
			string atkincstr = Convert.ToString (atkincnum);
			bool atkinc_pos = true;

			if (canequip_preview[MapleStat.Id.JOB] != null)
			{
				if (atkincnum < 0)
				{
					atkincstr = "m" + atkincstr;
					atkinc_pos = false;
				}
				else if (atkincnum > 0)
				{
					atkincstr = "p" + atkincstr;
					atkinc_pos = true;
				}
				else
				{
					atkinc_pos = true;
				}
			}
			else
			{
				atkincstr = "m";
				atkinc_pos = false;
			}

			atkincset[atkinc_pos].draw (atkincstr, 11, pos + new Point_short (239, 26));

			pos.shift_y (12);

			foreach (MapleStat.Id ms in requirements)
			{
				Point_short reqpos = reqstatpositions[ms];
				bool reqok = canequip_preview[ms];
				reqstattextures[ms][reqok].draw (pos + reqpos);

				if (ms != MapleStat.Id.LEVEL)
				{
					reqset[reqok].draw (reqstatstrings_preview[ms], 6, pos + reqpos + new Point_short (54, 0));
				}
				else
				{
					lvset[reqok].draw (reqstatstrings_preview[ms], 6, pos + reqpos + new Point_short (54, 0));
				}
			}

			pos.shift_y (88);

			Point_short job_position = (pos + new Point_short (10, 14));
			jobsback.draw (job_position);

			foreach (var jbit in okjobs_preview)
			{
				jobs[canequip_preview[MapleStat.Id.JOB]][jbit].draw (job_position);
			}

			line.draw (pos + new Point_short (0, 47));

			pos.shift_y (49);

			short stat_x = 13;
			short stat_y = 15;

			category_preview.draw (pos + new Point_short (stat_x, 0));

			pos.shift_y (stat_y);

			if (is_weapon_preview)
			{
				wepspeed_preview.draw (pos + new Point_short (stat_x, 0));
				pos.shift_y (stat_y);
			}

			foreach (Text label in statlabels_preview.dict.Values)
			{
				if (label.empty ())
				{
					continue;
				}

				label.draw (pos + new Point_short (stat_x, 0));
				pos.shift_y (stat_y);
			}

			if (hasslots_preview)
			{
				slots_preview.draw (pos + new Point_short (stat_x, 0));
				pos.shift_y (stat_y);
				hammers_preview.draw (pos + new Point_short (stat_x, 0));
				pos.shift_y (stat_y);
			}

			if (hasdesc_preview)
			{
				pos.shift_y (-4);
				line.draw (pos);
				desc_preview.draw (pos + new Point_short (9, 8));
			}*/
		}

		public short invpos;
		public short invpos_preview;
		public short height;
		public short height_preview;
		public bool hasdesc;
		public bool hasdesc_preview;
		public bool hasslots;
		public bool hasslots_preview;
		public bool is_weapon;
		public bool is_weapon_preview;
		public EnumMap<MapleStat.Id, string> reqstatstrings = new EnumMap<MapleStat.Id, string> ();
		public EnumMap<MapleStat.Id, string> reqstatstrings_preview = new EnumMap<MapleStat.Id, string> ();
		public Texture itemicon = new Texture ();
		public Texture itemicon_preview = new Texture ();

		public Text name = new Text ();
		public Text name_preview = new Text ();
		public Text desc = new Text ();
		public Text desc_preview = new Text ();
		public Text potflag = new Text ();
		public Text potflag_preview = new Text ();
		public Text category = new Text ();
		public Text category_preview = new Text ();
		public Text wepspeed = new Text ();
		public Text wepspeed_preview = new Text ();
		public Text slots = new Text ();
		public Text slots_preview = new Text ();
		public Text hammers = new Text ();
		public Text hammers_preview = new Text ();
		public Text atkinc = new Text ();
		public Text atkinc_preview = new Text ();
		public EnumMap<EquipStat.Id, Text> statlabels = new EnumMap<EquipStat.Id, Text> ();
		public EnumMap<EquipStat.Id, Text> statlabels_preview = new EnumMap<EquipStat.Id, Text> ();

		public Texture top = new Texture ();
		public Texture mid = new Texture ();
		public Texture line = new Texture ();
		public Texture bot = new Texture ();
		public Texture baseTexture = new Texture ();

		public EnumMap<Equip.Potential, Texture> potential = new EnumMap<Equip.Potential, Texture> ();
		public Equip.Potential prank;
		public Equip.Potential prank_preview;

		public Texture cover = new Texture ();
		public Texture itemcover = new Texture ();
		public BoolPairNew<Texture> type = new BoolPairNew<Texture> ();

		public List<MapleStat.Id> requirements = new List<MapleStat.Id> ();
		public EnumMapNew<MapleStat.Id, BoolPairNew<Texture>> reqstattextures = new EnumMapNew<MapleStat.Id, BoolPairNew<Texture>> ();
		public EnumMap<MapleStat.Id, bool> canequip = new EnumMap<MapleStat.Id, bool> ();
		public EnumMap<MapleStat.Id, bool> canequip_preview = new EnumMap<MapleStat.Id, bool> ();
		public EnumMap<MapleStat.Id, Point_short> reqstatpositions = new EnumMap<MapleStat.Id, Point_short> ();
		public BoolPairNew<Charset> reqset = new BoolPairNew<Charset> ();
		public BoolPairNew<Charset> lvset = new BoolPairNew<Charset> ();
		public BoolPairNew<Charset> atkincset = new BoolPairNew<Charset> ();

		public Texture jobsback = new Texture ();
		public BoolPairNew<SortedDictionary<byte, Texture>> jobs = new BoolPairNew<SortedDictionary<byte, Texture>> (new SortedDictionary<byte, Texture> (), new SortedDictionary<byte, Texture> ());
		public List<byte> okjobs = new List<byte> ();
		public List<byte> okjobs_preview = new List<byte> ();
	}
}


#if USE_NX
#endif