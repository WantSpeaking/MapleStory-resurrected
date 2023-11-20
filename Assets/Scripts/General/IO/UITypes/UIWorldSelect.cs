#define USE_NX

using System;
using System.Collections.Generic;
using Helper;
using MapleLib.WzLib;
using ms.Helper;




namespace ms
{
	[Beebyte.Obfuscator.Skip]
	public class UIWorldSelect : UIElement
	{
		public const Type TYPE = UIElement.Type.WORLDSELECT;
		public const bool FOCUSED = false;
		public const bool TOGGLED = false;

		public UIWorldSelect (params object[] args) : this ()
		{
		}

		public UIWorldSelect () : base (new Point_short (0, 0), new Point_short (800, 600))
		{
			/*worldcount = 0;
			recommended_worldcount = 0;
			recommended_worldid = 0;
			world_selected = false;
			use_recommended = true;
			show_recommended = false;
			draw_chatballoon = true;

			string version_text = Configuration.get ().get_version ();
			version = new Text (Text.Font.A11B, Text.Alignment.LEFT, Color.Name.LEMONGRASS, "Ver. " + version_text);

			recommended_message = new Text (Text.Font.A11M, Text.Alignment.CENTER, Color.Name.JAMBALAYA, "", 100, true, 5);

			//Point_short background_pos = new Point_short (400, 301);todo why 301
			Point_short background_pos = new Point_short (400, 300);
			channelsrc_pos = new Point_short (203, 164);

			worldid = Setting<DefaultWorld>.get ().load ();
			channelid = Setting<DefaultChannel>.get ().load ();
			byte regionid = Setting<DefaultRegion>.get ().load ();

			WzObject obj = ms.wz.wzFile_mapLatest["Obj"]["login.img"];
			WzObject login = ms.wz.wzProvider_ui["Login.img"];
			worldselect = (login["WorldSelect"]);
			worldsrc = (worldselect["BtWorld"]["release"]);
			channelsrc = (worldselect["BtChannel"]);
			WzObject common = login["Common"];
			WzObject frame = ms.wz.wzFile_mapLatest["Obj"]["login.img"]["Common"]["frame"]["2"]["0"];

			set_region (regionid);

			sprites.Add (new Sprite (obj["WorldSelect"]["default"][0.ToString ()], background_pos));

			List<string> backgrounds = new List<string> () {"16thNewtro"};
			var backgrounds_size = backgrounds.Count;

			if (backgrounds_size > 0)
			{
				if (backgrounds_size > 1)
				{
					var randomizer = new Randomizer ();
					int index = randomizer.next_int (backgrounds_size);

					sprites.Add (new Sprite (obj["WorldSelect"][backgrounds[index]][0.ToString ()], background_pos));
				}
				else
				{
					sprites.Add (new Sprite (obj["WorldSelect"][backgrounds[0]][0.ToString ()], background_pos));
				}
			}

			sprites.Add (new Sprite (frame, new Point_short (400, 300)));
			sprites.Add (new Sprite (common["frame"], new Point_short (400, 300)));
			sprites.Add (new Sprite (common["step"]["1"], new Point_short (40, 0)));

			buttons[(int)Buttons.BT_VIEWALL] = new MapleButton (worldselect["BtViewAll"], new Point_short (0, 53));
			buttons[(int)Buttons.BT_VIEWRECOMMENDED] = new MapleButton (worldselect["BtViewChoice"], new Point_short (0, 53));
			buttons[(int)Buttons.BT_VIEWRECOMMENDED_SELECT] = new MapleButton (worldselect["alert"]["BtChoice"], new Point_short (349, 327));
			buttons[(int)Buttons.BT_VIEWRECOMMENDED_CANCEL] = new MapleButton (worldselect["alert"]["BtClose"], new Point_short (407, 327));
			buttons[(int)Buttons.BT_VIEWRECOMMENDED_PREV] = new MapleButton (worldselect["alert"]["BtArrowL"], new Point_short (338, 244));
			buttons[(int)Buttons.BT_VIEWRECOMMENDED_NEXT] = new MapleButton (worldselect["alert"]["BtArrowR"], new Point_short (439, 244));

			buttons[(int)Buttons.BT_VIEWALL].set_active (false);
			buttons[(int)Buttons.BT_VIEWRECOMMENDED].set_active (use_recommended ? true : false);
			buttons[(int)Buttons.BT_VIEWRECOMMENDED_SELECT].set_active (false);
			buttons[(int)Buttons.BT_VIEWRECOMMENDED_CANCEL].set_active (false);
			buttons[(int)Buttons.BT_VIEWRECOMMENDED_PREV].set_active (false);
			buttons[(int)Buttons.BT_VIEWRECOMMENDED_NEXT].set_active (false);

			buttons[(int)Buttons.BT_VIEWRECOMMENDED].set_state (Button.State.DISABLED);

			recommended_textures.Add (worldselect["alert"]["backgrd"]);

			buttons[(int)Buttons.BT_CHANGEREGION] = new MapleButton (worldselect["BtRegion"], new Point_short (3, 127));
			buttons[(int)Buttons.BT_QUITGAME] = new MapleButton (common["BtExit"], new Point_short (0, 515));

			for (uint i = 0; i < Buttons.BT_ENTERWORLD - Buttons.BT_CHANNEL0; i++)
			{
				string ch = Convert.ToString (i);

				buttons[(int)Buttons.BT_CHANNEL0 + i] = new TwoSpriteButton (channelsrc["button:" + ch]["normal"]["0"], channelsrc["button:" + ch]["keyFocused"]["0"], channelsrc_pos);
				buttons[(int)Buttons.BT_CHANNEL0 + i].set_active (false);
			}

			channels_background = channelsrc["layer:bg"];

			buttons[(int)Buttons.BT_ENTERWORLD] = new MapleButton (channelsrc["button:GoWorld"], channelsrc_pos);
			buttons[(int)Buttons.BT_ENTERWORLD].set_active (false);

			chatballoon.change_text ("Please select the World you would like to play in.");

			if (Configuration.get ().get_var_login ())
			{
				var world = Configuration.get ().get_var_world ();
				var channel = Configuration.get ().get_var_channel ();

				Configuration.get ().set_worldid (world);
				Configuration.get ().set_channelid (channel);

				UI.get ().emplace<UILoginWait> ();
				var loginwait = UI.get ().get_element<UILoginWait> ();

				if (loginwait && loginwait.get ().is_active ())
				{
					new CharlistRequestPacket (world, channel).dispatch ();
				}
			}*/

			enter_world ();
		}

		/*public override void draw (float alpha)
		{
			base.draw_sprites (alpha);

			worlds_background.draw (position + worldsrc_pos);

			if (show_recommended)
			{
				recommended_textures[0].draw (position + new Point_short (302, 152));
				recommended_world_textures[recommended_worldid].draw (position + new Point_short (336, 187));
				recommended_message.draw (position + new Point_short (401, 259));
			}

			if (world_selected)
			{
				channels_background.draw (position + channelsrc_pos);
				world_textures[worldid].draw (position + channelsrc_pos);
			}

			base.draw_buttons (alpha);

			version.draw (position + new Point_short (707, 4));

			if (draw_chatballoon)
			{
				chatballoon.draw (position + new Point_short (501, 105));
			}
		}*/

		public override Cursor.State send_cursor (bool clicked, Point_short cursorpos)
		{
			Rectangle_short channels_bounds = new Rectangle_short (position + channelsrc_pos, position + channelsrc_pos + channels_background.get_dimensions ());

			Rectangle_short worlds_bounds = new Rectangle_short (position + worldsrc_pos, position + worldsrc_pos + worlds_background.get_dimensions ());

			if (world_selected && !channels_bounds.contains (cursorpos) && !worlds_bounds.contains (cursorpos))
			{
				if (clicked)
				{
					world_selected = false;
					clear_selected_world ();
				}
			}

			Cursor.State ret = clicked ? Cursor.State.CLICKING : Cursor.State.IDLE;

			foreach (var btit in buttons)
			{
				if (btit.Value.is_active () && btit.Value.bounds (position).contains (cursorpos))
				{
					if (btit.Value.get_state () == Button.State.NORMAL)
					{
						new Sound (Sound.Name.BUTTONOVER).play ();

						btit.Value.set_state (Button.State.MOUSEOVER);
						ret = Cursor.State.CANCLICK;
					}
					else if (btit.Value.get_state () == Button.State.PRESSED)
					{
						if (clicked)
						{
							new Sound (Sound.Name.BUTTONCLICK).play ();

							btit.Value.set_state (button_pressed ((ushort)btit.Key));

							ret = Cursor.State.IDLE;
						}
						else
						{
							ret = Cursor.State.CANCLICK;
						}
					}
					else if (btit.Value.get_state () == Button.State.MOUSEOVER)
					{
						if (clicked)
						{
							new Sound (Sound.Name.BUTTONCLICK).play ();

							btit.Value.set_state (button_pressed ((ushort)btit.Key));

							ret = Cursor.State.IDLE;
						}
						else
						{
							ret = Cursor.State.CANCLICK;
						}
					}
				}
				else if (btit.Value.get_state () == Button.State.MOUSEOVER)
				{
					btit.Value.set_state (Button.State.NORMAL);
				}
			}

			return ret;
		}

		public override void send_key (int keycode, bool pressed, bool escape)
		{
			if (pressed)
			{
				if (world_selected)
				{
					World selectedWorld = worlds[worldid];

					byte selected_channel = channelid;
					byte channel_total = selectedWorld.channelcount;

					byte COLUMNS = 5;
					byte columns = Math.Min (channel_total, COLUMNS);

					byte rows = (byte)(Math.Floor ((decimal)((channel_total - 1) / COLUMNS)) + 1);

					std.div_t div = std.div (selected_channel, columns);
					var current_col = div.rem;
					//var current_row = div.quot;

					if (keycode == (int)KeyAction.Id.UP)
					{
						var next_channel = (selected_channel - COLUMNS < 0 ? (selected_channel - COLUMNS) + rows * COLUMNS : selected_channel - COLUMNS);

						if (next_channel == channelid)
						{
							return;
						}

						if (next_channel > channel_total)
						{
							button_pressed ((ushort)(next_channel - COLUMNS + (int)Buttons.BT_CHANNEL0));
						}
						else
						{
							button_pressed ((ushort)(next_channel + (int)Buttons.BT_CHANNEL0));
						}
					}
					else if (keycode == (int)KeyAction.Id.DOWN)
					{
						var next_channel = (selected_channel + COLUMNS >= channel_total ? current_col : selected_channel + COLUMNS);

						if (next_channel == channelid)
						{
							return;
						}

						if (next_channel > channel_total)
						{
							button_pressed ((ushort)(next_channel + COLUMNS + (int)Buttons.BT_CHANNEL0));
						}
						else
						{
							button_pressed ((ushort)(next_channel + (int)Buttons.BT_CHANNEL0));
						}
					}
					else if (keycode == (int)KeyAction.Id.LEFT || keycode == (int)KeyAction.Id.TAB)
					{
						if (selected_channel != 0)
						{
							selected_channel--;
						}
						else
						{
							selected_channel = (byte)(channel_total - 1);
						}

						button_pressed ((ushort)(selected_channel + Buttons.BT_CHANNEL0));
					}
					else if (keycode == (int)KeyAction.Id.RIGHT)
					{
						if (selected_channel != channel_total - 1)
						{
							selected_channel++;
						}
						else
						{
							selected_channel = 0;
						}

						button_pressed ((ushort)(selected_channel + Buttons.BT_CHANNEL0));
					}
					else if (escape)
					{
						world_selected = false;

						clear_selected_world ();
					}
					else if (keycode == (int)KeyAction.Id.RETURN)
					{
						button_pressed ((ushort)Buttons.BT_ENTERWORLD);
					}
				}
				else if (show_recommended)
				{
					if (escape || keycode == (int)KeyAction.Id.RETURN)
					{
						toggle_recommended (false);
					}
				}
				else
				{
					var selected_world = worldid;
					var world_count = worldcount - 1;

					if (keycode == (int)KeyAction.Id.LEFT || keycode == (int)KeyAction.Id.RIGHT || keycode == (int)KeyAction.Id.UP || keycode == (int)KeyAction.Id.DOWN || keycode == (int)KeyAction.Id.TAB)
					{
						bool world_found = false;
						bool forward = keycode == (int)KeyAction.Id.LEFT || keycode == (int)KeyAction.Id.UP;

						while (!world_found)
						{
							selected_world = (byte)get_next_world (selected_world, forward);

							foreach (var world in worlds)
							{
								if (world.wid == selected_world)
								{
									world_found = true;
									break;
								}
							}
						}

						buttons[(uint)((int)Buttons.BT_WORLD0 + worldid)].set_state (Button.State.NORMAL);

						worldid = (byte)selected_world;

						buttons[(uint)((int)Buttons.BT_WORLD0 + worldid)].set_state (Button.State.PRESSED);
					}
					else if (escape)
					{
						var quitconfirm = UI.get ().get_element<UIQuitConfirm> ();

						if (quitconfirm && quitconfirm.get ().is_active ())
						{
							UI.get ().send_key (keycode, pressed);
							return;
						}
						else
						{
							button_pressed ((ushort)Buttons.BT_QUITGAME);
						}
					}
					else if (keycode == (int)KeyAction.Id.RETURN)
					{
						var quitconfirm = UI.get ().get_element<UIQuitConfirm> ();

						if (quitconfirm && quitconfirm.get ().is_active ())
						{
							UI.get ().send_key (keycode, pressed);
							return;
						}
						else
						{
							bool found = false;

							for (uint i = (int)Buttons.BT_WORLD0; i < (ulong)Buttons.BT_CHANNEL0; i++)
							{
								var state = buttons[(int)Buttons.BT_WORLD0 + i].get_state ();

								if (state == Button.State.PRESSED)
								{
									found = true;
									break;
								}
							}

							if (found)
							{
								button_pressed ((ushort)(selected_world + Buttons.BT_WORLD0));
							}
							else
							{
								buttons[(uint)((int)Buttons.BT_WORLD0 + selected_world)].set_state (Button.State.PRESSED);
							}
						}
					}
				}
			}
		}

		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public void draw_world ()
		{
			/*if (worldcount <= 0)
			{
				return; // TODO: Send the user back to the login screen? Otherwise, I think the screen will be blank with no worlds, or throw a UILoginNotice up with failed to communite to server?
			}

			foreach (var world in worlds)
			{
				if (world.channelcount < 2)
				{
					return; // I remove the world if there is only one channel because the graphic for the channel selection is defaulted to at least 2
				}

				buttons[(uint)((int)Buttons.BT_WORLD0 + world.wid)].set_active (true);

				if (channelid >= world.channelcount)
				{
					channelid = 0;
				}
			}*/
		}

		public void add_world (World world)
		{
			worlds.Add ((world));
			worldcount++;
		}

		public void add_recommended_world (RecommendedWorld world)
		{
			if (use_recommended)
			{
				recommended_worlds.Add ((world));
				recommended_worldcount++;

				buttons[(int)Buttons.BT_VIEWRECOMMENDED].set_state (Button.State.NORMAL);
			}
		}

		public void change_world (World selectedWorld)
		{
			buttons[(uint)(Buttons.BT_WORLD0 + (ushort)selectedWorld.wid)].set_state (Button.State.PRESSED);

			for (uint i = 0; i < selectedWorld.channelcount; ++i)
			{
				buttons[(int)Buttons.BT_CHANNEL0 + i].set_active (true);

				if (i == channelid)
				{
					buttons[(int)Buttons.BT_CHANNEL0 + i].set_state (Button.State.PRESSED);
				}
			}

			buttons[(int)Buttons.BT_ENTERWORLD].set_active (true);
		}

		public void remove_selected ()
		{
			deactivate ();

			new Sound (Sound.Name.SCROLLUP).play ();

			world_selected = false;

			clear_selected_world ();

			draw_chatballoon = false;
		}

		public void set_region (byte regionid)
		{
/* 			world_map[(int)Buttons.BT_WORLD0] = (ushort)(regionid == 5 ? Worlds.SCANIA : Worlds.LUNA);
			world_map[(int)Buttons.BT_WORLD1] = (int)Worlds.BERA;
			world_map[(int)Buttons.BT_WORLD2] = (int)Worlds.AURORA;
			world_map[(int)Buttons.BT_WORLD3] = (int)Worlds.ELYSIUM1;
			world_map[(int)Buttons.BT_WORLD4] = (int)Worlds.REBOOT1;

			WzObject region = worldsrc["index"][regionid.ToString ()];

			worlds_background = region["layer:bg"];

			worldsrc_pos = region["pos"];

			for (uint i = (int)Buttons.BT_WORLD0; i <= (ulong)Buttons.BT_WORLD4; i++)
			{
				string world = Convert.ToString (world_map[(ushort)i]);
				world_textures.Add (channelsrc["release"]["layer:" + world]);
				recommended_world_textures.Add (worldselect["world"][world]);

				WzObject worldbtn = worldsrc["button:" + world];

				buttons[(int)Buttons.BT_WORLD0 + i] = new TwoSpriteButton (worldbtn["normal"]["0"], worldbtn["keyFocused"]["0"], worldsrc_pos + region["origin"][(i + 1).ToString ()].GetPoint ().ToMSPoint ());
				buttons[(int)Buttons.BT_WORLD0 + i].set_active (false);
			} */

			world_map[(int)Buttons.BT_WORLD0] = 0;
			WzObject region = worldsrc["index"][regionid.ToString ()];

			worlds_background = region["layer:bg"];

			worldsrc_pos = region["pos"];
for (uint i = (int)Buttons.BT_WORLD0; i <= (ulong)Buttons.BT_WORLD0; i++)
{
string world = Convert.ToString (world_map[(ushort)i]);
				world_textures.Add (channelsrc["release"]["layer:" + world]);
				recommended_world_textures.Add (worldselect["world"][world]);

				WzObject worldbtn = worldsrc["button:" + world];

				buttons[(int)Buttons.BT_WORLD0 + i] = new TwoSpriteButton (worldbtn["normal"]["0"], worldbtn["keyFocused"]["0"], worldsrc_pos + region["origin"][(i + 1).ToString ()].GetPoint ().ToMSPoint ());
				buttons[(int)Buttons.BT_WORLD0 + i].set_active (false);
}

		}

		public ushort get_worldbyid (ushort worldid)
		{
			return world_map.TryGetValue(worldid);
		}

		public override Button.State button_pressed (ushort id)
		{
			if (id == (int)Buttons.BT_ENTERWORLD)
			{
				enter_world ();

				return Button.State.NORMAL;
			}
			else if (id == (int)Buttons.BT_QUITGAME)
			{
				UI.get ().emplace<UIQuitConfirm> ();

				return Button.State.NORMAL;
			}
			else if (id == (int)Buttons.BT_VIEWRECOMMENDED)
			{
				world_selected = false;
				clear_selected_world ();
				toggle_recommended (true);

				return Button.State.NORMAL;
			}
			else if (id == (int)Buttons.BT_VIEWALL)
			{
				toggle_recommended (false);

				return Button.State.NORMAL;
			}
			else if (id == (int)Buttons.BT_VIEWRECOMMENDED_SELECT)
			{
				buttons[(uint)(Buttons.BT_WORLD0 + worldid)].set_state (Button.State.NORMAL);

				worldid = recommended_worldid;

				buttons[(uint)(Buttons.BT_WORLD0 + worldid)].set_state (Button.State.PRESSED);

				toggle_recommended (false);

				return Button.State.NORMAL;
			}
			else if (id == (int)Buttons.BT_VIEWRECOMMENDED_CANCEL)
			{
				toggle_recommended (false);

				return Button.State.NORMAL;
			}
			else if (id == (int)Buttons.BT_VIEWRECOMMENDED_PREV)
			{
				if (recommended_worldid > 0)
				{
					recommended_worldid--;
				}
				else
				{
					recommended_worldid = (byte)(recommended_worldcount - 1);
				}

				recommended_message.change_text (recommended_worlds[recommended_worldid].message);

				return Button.State.NORMAL;
			}
			else if (id == (int)Buttons.BT_VIEWRECOMMENDED_NEXT)
			{
				if (recommended_worldid < recommended_worldcount - 1)
				{
					recommended_worldid++;
				}
				else
				{
					recommended_worldid = 0;
				}

				recommended_message.change_text (recommended_worlds[recommended_worldid].message);

				return Button.State.NORMAL;
			}
			else if (id == (int)Buttons.BT_CHANGEREGION)
			{
				UI.get().emplace<UIRegion>();

				deactivate ();

				return Button.State.NORMAL;
			}
			else if (id >= ((int)Buttons.BT_WORLD0) && id < (int)Buttons.BT_CHANNEL0)
			{
				toggle_recommended (false);

				buttons[(uint)(Buttons.BT_WORLD0 + worldid)].set_state (Button.State.NORMAL);

				worldid = (byte)(id - Buttons.BT_WORLD0);

				new ServerStatusRequestPacket (worldid).dispatch ();

				world_selected = true;
				clear_selected_world ();
				change_world (worlds[worldid]);

				return Button.State.PRESSED;
			}
			else if (id >= ((int)Buttons.BT_CHANNEL0) && id < (int)Buttons.BT_ENTERWORLD)
			{
				byte selectedch = (byte)(id - Buttons.BT_CHANNEL0);

				if (selectedch != channelid)
				{
					buttons[(uint)(Buttons.BT_CHANNEL0 + channelid)].set_state (Button.State.NORMAL);
					channelid = (byte)(id - Buttons.BT_CHANNEL0);
					buttons[(uint)(Buttons.BT_CHANNEL0 + channelid)].set_state (Button.State.PRESSED);
					new Sound (Sound.Name.WORLDSELECT).play ();
				}
				else
				{
					enter_world ();
				}

				return Button.State.PRESSED;
			}
			else
			{
				return Button.State.NORMAL;
			}
		}

		private void enter_world ()
		{
			Configuration.get ().set_worldid (worldid);
			Configuration.get ().set_channelid (channelid);

			UI.get ().emplace<UILoginWait> ();
			var loginwait = UI.get ().get_element<UILoginWait> ();

			if (loginwait && loginwait.get ().is_active ())
			{
				new CharlistRequestPacket (worldid, channelid).dispatch ();
			}
		}

		private void toggle_recommended (bool active)
		{
			if (recommended_worldcount > 0)
			{
				recommended_worldid = 0;
				show_recommended = active;

				buttons[(int)Buttons.BT_VIEWALL].set_active (active);
				buttons[(int)Buttons.BT_VIEWRECOMMENDED].set_active (!active);
				buttons[(int)Buttons.BT_VIEWRECOMMENDED_SELECT].set_active (active);
				buttons[(int)Buttons.BT_VIEWRECOMMENDED_CANCEL].set_active (active);
				buttons[(int)Buttons.BT_VIEWRECOMMENDED_PREV].set_active (active);
				buttons[(int)Buttons.BT_VIEWRECOMMENDED_NEXT].set_active (active);

				if (recommended_worldcount <= 1)
				{
					buttons[(int)Buttons.BT_VIEWRECOMMENDED_PREV].set_state (Button.State.DISABLED);
					buttons[(int)Buttons.BT_VIEWRECOMMENDED_NEXT].set_state (Button.State.DISABLED);
				}
				else
				{
					buttons[(int)Buttons.BT_VIEWRECOMMENDED_PREV].set_state (Button.State.NORMAL);
					buttons[(int)Buttons.BT_VIEWRECOMMENDED_NEXT].set_state (Button.State.NORMAL);
				}

				if (!active)
				{
					recommended_message.change_text ("");
				}
				else
				{
					recommended_message.change_text (recommended_worlds[recommended_worldid].message);
				}
			}
		}

		private void clear_selected_world ()
		{
			/*channelid = 0;

			for (uint i = (int)Buttons.BT_CHANNEL0; i < (ulong)Buttons.BT_ENTERWORLD; i++)
			{
				buttons[i].set_state (Button.State.NORMAL);
			}

			buttons[(int)Buttons.BT_CHANNEL0].set_state (Button.State.PRESSED);

			for (uint i = 0; i < Buttons.BT_ENTERWORLD - Buttons.BT_CHANNEL0; i++)
			{
				buttons[(int)Buttons.BT_CHANNEL0 + i].set_active (false);
			}

			buttons[(int)Buttons.BT_ENTERWORLD].set_active (false);*/
		}

		private ushort get_next_world (ushort id, bool upward)
		{
			ushort next_world = 0;

			if (world_map[(int)Buttons.BT_WORLD0] == (int)Worlds.SCANIA)
			{
				switch ((Buttons)id)
				{
					case Buttons.BT_WORLD0:
						next_world = (ushort)((upward) ? Worlds.REBOOT1 : Worlds.BERA);
						break;
					case Buttons.BT_WORLD1:
						next_world = (ushort)((upward) ? Worlds.SCANIA : Worlds.AURORA);
						break;
					case Buttons.BT_WORLD2:
						next_world = (ushort)((upward) ? Worlds.BERA : Worlds.ELYSIUM1);
						break;
					case Buttons.BT_WORLD3:
						next_world = (ushort)((upward) ? Worlds.AURORA : Worlds.REBOOT1);
						break;
					case Buttons.BT_WORLD4:
						next_world = (ushort)((upward) ? Worlds.ELYSIUM1 : Worlds.SCANIA);
						break;
					default:
						break;
				}
			}
			else
			{
				switch ((Buttons)id)
				{
					case Buttons.BT_WORLD0:
						next_world = (ushort)((upward) ? Worlds.REBOOT1 : Worlds.REBOOT1);
						break;
					case Buttons.BT_WORLD4:
						next_world = (ushort)((upward) ? Worlds.SCANIA : Worlds.SCANIA);
						break;
					default:
						break;
				}
			}

			var world = world_map.GetEnumerator ();

			while (world.MoveNext ())
			{
				if (world.Current.Value == next_world)
				{
					return world.Current.Key;
				}
			}

			return (ushort)Worlds.SCANIA;
		}

		private enum Buttons : ushort
		{
			BT_WORLD0,
			BT_WORLD1,
			BT_WORLD2,
			BT_WORLD3,
			BT_WORLD4,
			BT_CHANNEL0,
			BT_CHANNEL1,
			BT_CHANNEL2,
			BT_CHANNEL3,
			BT_CHANNEL4,
			BT_CHANNEL5,
			BT_CHANNEL6,
			BT_CHANNEL7,
			BT_CHANNEL8,
			BT_CHANNEL9,
			BT_CHANNEL10,
			BT_CHANNEL11,
			BT_CHANNEL12,
			BT_CHANNEL13,
			BT_CHANNEL14,
			BT_CHANNEL15,
			BT_CHANNEL16,
			BT_CHANNEL17,
			BT_CHANNEL18,
			BT_CHANNEL19,
			BT_ENTERWORLD,
			BT_VIEWALL,
			BT_VIEWRECOMMENDED,
			BT_VIEWRECOMMENDED_SELECT,
			BT_VIEWRECOMMENDED_CANCEL,
			BT_VIEWRECOMMENDED_PREV,
			BT_VIEWRECOMMENDED_NEXT,
			BT_CHANGEREGION,
			BT_QUITGAME
		}

		private enum Worlds : ushort
		{
			SCANIA,
			BERA,
			BROA,
			WINDIA,
			KHAINI,
			BELLOCAN,
			MARDIA,
			KRADIA,
			YELLONDE,
			DEMETHOS,
			GALICIA,
			ELNIDO,
			ZENITH,
			ARCANIA,
			CHAOS,
			NOVA,
			RENEGADES,
			AURORA,
			ELYSIUM1,
			KOREAN1 = 29,
			LUNA,
			ELYSIUM2,
			LAB = 40,
			KOREAN2 = 43,
			KOREAN3,
			REBOOT1,
			REBOOT2,
			PINKBEAN = 48,
			BURNING,
			KOREAN4,
			KOREAN5,
			TESPIA = 100
		}

		private Text version = new Text ();
		private Text recommended_message = new Text ();
		private Texture worlds_background = new Texture ();
		private Texture channels_background = new Texture ();
		private Point_short worldsrc_pos = new Point_short ();
		private Point_short channelsrc_pos = new Point_short ();
		private ChatBalloonHorizontal chatballoon = new ChatBalloonHorizontal ();

		private byte worldid;
		private byte recommended_worldid;
		private byte channelid;
		private byte worldcount;
		private byte recommended_worldcount;

		private List<World> worlds = new List<World> ();
		private List<RecommendedWorld> recommended_worlds = new List<RecommendedWorld> ();
		private List<Texture> world_textures = new List<Texture> ();
		private List<Texture> recommended_world_textures = new List<Texture> ();
		private List<Texture> recommended_textures = new List<Texture> ();
		private SortedDictionary<ushort, ushort> world_map = new SortedDictionary<ushort, ushort> ();

		private bool world_selected;
		private bool use_recommended;
		private bool show_recommended;
		private bool draw_chatballoon;

		private WzObject worldselect;
		private WzObject worldsrc;
		private WzObject channelsrc;
	}
}


#if USE_NX
#endif