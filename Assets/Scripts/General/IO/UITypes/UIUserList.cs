#define USE_NX

using System;
using System.Collections.Generic;
using MapleLib.WzLib;
using provider;

namespace ms
{
	[Beebyte.Obfuscator.Skip]
	public class UIUserList : UIDragElement<PosUSERLIST>
	{
		public const Type TYPE = UIElement.Type.USERLIST;
		public const bool FOCUSED = false;
		public const bool TOGGLED = true;

		public UIUserList (params object[] args) : this ((ushort)args[0])
		{
		}

	
		public UIUserList (ushort t) : base (new Point_short (260, 20))
		{
			this.tab = t;
			MapleData close = ms.wz.wzProvider_ui["Basic.img"]["BtClose2"];
			UserList = ms.wz.wzProvider_ui["UIWindow.img"]["UserList"];
			MapleData Main = UserList["Main"];

			sprites.Add (Main["backgrnd"]);

			buttons[(int)Buttons.BT_CLOSE] = new MapleButton (close, new Point_short (244, 7));

			MapleData taben = Main["Tab"]["enabled"];
			MapleData tabdis = Main["Tab"]["disabled"];

			buttons[(int)Buttons.BT_TAB_FRIEND] = new TwoSpriteButton (tabdis["0"], taben["0"]);
			buttons[(int)Buttons.BT_TAB_PARTY] = new TwoSpriteButton (tabdis["1"], taben["1"]);
			buttons[(int)Buttons.BT_TAB_BOSS] = new TwoSpriteButton (tabdis["2"], taben["2"]);
			buttons[(int)Buttons.BT_TAB_BLACKLIST] = new TwoSpriteButton (tabdis["3"], taben["3"]);

			// Party Tab
			MapleData Party = Main["Party"];
			MapleData PartySearch = Party["PartySearch"];

			party_tab = (int)Tab.PARTY_MINE;
			party_title = Party["title"];

			for (int i = 0; i < Constants.get ().MAX_PartyMemberCount; i++)
			{
				party_mine_gridButtons[i] = new MapleButton (UserList["Sheet2"]["1"], UserList["Sheet2"]["2"], UserList["Sheet2"]["3"], UserList["Sheet2"]["0"]);
			}

			for (uint i = 0; i <= 4; i++)
			{
				party_mine_grid[i] = UserList["Sheet2"][i.ToString ()];
			}

			party_mine_name = new Text (Text.Font.A12M, Text.Alignment.LEFT, Color.Name.BLACK, "none", 0);

			MapleData party_taben = Party["Tab"]["enabled"];
			MapleData party_tabdis = Party["Tab"]["disabled"];

			buttons[(int)Buttons.BT_PARTY_CREATE] = new MapleButton (Party["BtPartyMake"]);
			buttons[(int)Buttons.BT_PARTY_INVITE] = new MapleButton (Party["BtPartyInvite"]);
			buttons[(int)Buttons.BT_PARTY_LEAVE] = new MapleButton (Party["BtPartyOut"]);
			buttons[(int)Buttons.BT_PARTY_SETTINGS] = new MapleButton (Party["BtPartySettings"]);
			buttons[(int)Buttons.BT_PARTY_CREATE].set_active (false);
			buttons[(int)Buttons.BT_PARTY_INVITE].set_active (false);
			buttons[(int)Buttons.BT_PARTY_LEAVE].set_active (false);
			buttons[(int)Buttons.BT_PARTY_SETTINGS].set_active (true);

			buttons[(int)Buttons.BT_TAB_PARTY_MINE] = new TwoSpriteButton (party_tabdis["0"], party_taben["0"]);
			buttons[(int)Buttons.BT_TAB_PARTY_SEARCH] = new TwoSpriteButton (party_tabdis["1"], party_taben["1"]);
			buttons[(int)Buttons.BT_TAB_PARTY_MINE].set_active (false);
			buttons[(int)Buttons.BT_TAB_PARTY_SEARCH].set_active (false);

			party_search_grid[0] = PartySearch["partyName"];
			party_search_grid[1] = PartySearch["request"];
			party_search_grid[2] = PartySearch["table"];

			buttons[(int)Buttons.BT_PARTY_SEARCH_LEVEL] = new MapleButton (PartySearch["BtPartyLevel"]);
			buttons[(int)Buttons.BT_PARTY_SEARCH_LEVEL].set_active (false);

			short party_x = 243;
			short party_y = 114;
			short party_height = (short)(party_y + 168);
			short party_unitrows = 6;
			short party_rowmax = 6;
			party_slider = new Slider ((int)Slider.Type.DEFAULT_SILVER, new Range_short (party_y, party_height), party_x, party_unitrows, party_rowmax, (bool UnnamedParameter1) => { });

			party_Flag_Online = Party["icon0"]?["0"];
			party_Flag_Offline = Party["icon0"]?["1"];
			// Buddy Tab
			MapleData Friend = Main["Friend"];

			friend_tab = (int)Tab.FRIEND_ALL;
			friend_sprites.Add (Friend["title"]);
			friend_sprites.Add (Friend["CbCondition"]["text"]);
			friend_sprites.Add (new Sprite (UserList["line"], new DrawArgument (new Point_short (132, 115), new Point_short (230, 0))));

			buttons[(int)Buttons.BT_FRIEND_GROUP_0] = new MapleButton (UserList["BtSheetIClose"], new Point_short (13, 118));
			buttons[(int)Buttons.BT_FRIEND_GROUP_0].set_active (false);

			for (uint i = 0; i <= 3; i++)
			{
				friend_grid[i] = UserList["Sheet1"][i.ToString ()];
			}

			string text = "(" + Convert.ToString (friend_count) + "/" + Convert.ToString (friend_total) + (")");
			friends_online_text = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.WHITE, text, 0);

			friends_cur_location = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.LIGHTGREY, "My Location - " + get_cur_location (), 0);
			friends_name = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.BLACK, "none", 0);
			friends_group_name = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.WHITE, "Default Group (0/0)", 0);

			buttons[(int)Buttons.BT_FRIEND_ADD] = new MapleButton (Friend["BtAddFriend"]);
			buttons[(int)Buttons.BT_FRIEND_ADD_GROUP] = new MapleButton (Friend["BtAddGroup"]);
			buttons[(int)Buttons.BT_FRIEND_EXPAND] = new MapleButton (Friend["BtPlusFriend"]);
			buttons[(int)Buttons.BT_FRIEND_ADD].set_active (false);
			buttons[(int)Buttons.BT_FRIEND_ADD_GROUP].set_active (false);
			buttons[(int)Buttons.BT_FRIEND_EXPAND].set_active (false);

			buttons[(int)Buttons.BT_TAB_FRIEND_ALL] = new MapleButton (Friend["TapShowAll"]);
			buttons[(int)Buttons.BT_TAB_FRIEND_ONLINE] = new MapleButton (Friend["TapShowOnline"]);
			buttons[(int)Buttons.BT_TAB_FRIEND_ALL].set_active (false);
			buttons[(int)Buttons.BT_TAB_FRIEND_ONLINE].set_active (false);

			short friends_x = 243;
			short friends_y = 115;
			short friends_height = (short)(friends_y + 148);
			short friends_unitrows = 6;
			short friends_rowmax = 6;
			friends_slider = new Slider ((int)Slider.Type.DEFAULT_SILVER, new Range_short (friends_y, friends_height), friends_x, friends_unitrows, friends_rowmax, (bool UnnamedParameter1) => { });

			// Boss tab
			MapleData Boss = Main["Boss"];

			boss_sprites.Add (Boss["base"]);
			boss_sprites.Add (Boss["base3"]);
			boss_sprites.Add (Boss["base2"]);

			buttons[(int)Buttons.BT_BOSS_0] = new TwoSpriteButton (Boss["BossList"]["0"]["icon"]["disabled"]["0"], Boss["BossList"]["0"]["icon"]["normal"]["0"]);
			buttons[(int)Buttons.BT_BOSS_1] = new TwoSpriteButton (Boss["BossList"]["1"]["icon"]["disabled"]["0"], Boss["BossList"]["1"]["icon"]["normal"]["0"]);
			buttons[(int)Buttons.BT_BOSS_2] = new TwoSpriteButton (Boss["BossList"]["2"]["icon"]["disabled"]["0"], Boss["BossList"]["2"]["icon"]["normal"]["0"]);
			buttons[(int)Buttons.BT_BOSS_3] = new TwoSpriteButton (Boss["BossList"]["3"]["icon"]["disabled"]["0"], Boss["BossList"]["3"]["icon"]["normal"]["0"]);
			buttons[(int)Buttons.BT_BOSS_4] = new TwoSpriteButton (Boss["BossList"]["4"]["icon"]["disabled"]["0"], Boss["BossList"]["4"]["icon"]["normal"]["0"]);
			buttons[(int)Buttons.BT_BOSS_L] = new MapleButton (Boss["BtArrow"]["Left"]);
			buttons[(int)Buttons.BT_BOSS_R] = new MapleButton (Boss["BtArrow"]["Right"]);
			buttons[(int)Buttons.BT_BOSS_DIFF_L] = new MapleButton (Boss["BtArrow2"]["Left"]);
			buttons[(int)Buttons.BT_BOSS_DIFF_R] = new MapleButton (Boss["BtArrow2"]["Right"]);
			buttons[(int)Buttons.BT_BOSS_GO] = new MapleButton (Boss["BtEntry"]);
			buttons[(int)Buttons.BT_BOSS_0].set_active (false);
			buttons[(int)Buttons.BT_BOSS_1].set_active (false);
			buttons[(int)Buttons.BT_BOSS_2].set_active (false);
			buttons[(int)Buttons.BT_BOSS_3].set_active (false);
			buttons[(int)Buttons.BT_BOSS_4].set_active (false);
			buttons[(int)Buttons.BT_BOSS_L].set_active (false);
			buttons[(int)Buttons.BT_BOSS_R].set_active (false);
			buttons[(int)Buttons.BT_BOSS_DIFF_L].set_active (false);
			buttons[(int)Buttons.BT_BOSS_DIFF_R].set_active (false);
			buttons[(int)Buttons.BT_BOSS_GO].set_active (false);

			// Blacklist tab
			MapleData BlackList = Main["BlackList"];

			blacklist_title = BlackList["base"];

			for (uint i = 0; i < 3; i++)
			{
				blacklist_grid[i] = UserList["Sheet6"][i.ToString ()];
			}

			blacklist_name = new Text (Text.Font.A12M, Text.Alignment.LEFT, Color.Name.BLACK, "none", 0);

			/*MapleData blacklist_taben = BlackList["Tab"]["enabled"];
			MapleData blacklist_tabdis = BlackList["Tab"]["disabled"];*/

			buttons[(int)Buttons.BT_BLACKLIST_ADD] = new MapleButton (BlackList["BtAdd"]);
			buttons[(int)Buttons.BT_BLACKLIST_DELETE] = new MapleButton (BlackList["BtDelete"]);
			buttons[(int)Buttons.BT_TAB_BLACKLIST_INDIVIDUAL] = new MapleButton (BlackList["TapShowIndividual"]);
			buttons[(int)Buttons.BT_TAB_BLACKLIST_GUILD] = new MapleButton (BlackList["TapShowGuild"]);
			buttons[(int)Buttons.BT_BLACKLIST_ADD].set_active (false);
			buttons[(int)Buttons.BT_BLACKLIST_DELETE].set_active (false);
			buttons[(int)Buttons.BT_TAB_BLACKLIST_INDIVIDUAL].set_active (false);
			buttons[(int)Buttons.BT_TAB_BLACKLIST_GUILD].set_active (false);

			change_tab ((byte)tab);

			dimension = new Point_short (276, 390);
		}

		public override void draw (float alpha)
		{
			base.draw_sprites (alpha);

			background.draw (position);

			if (tab == (int)Buttons.BT_TAB_PARTY)
			{
				party_title.draw (position);

				if (party_tab == (int)Buttons.BT_TAB_PARTY_MINE)
				{
					party_mine_grid[0].draw (position + new Point_short (10, 115));

					DrawPartyMember ();
					/*party_mine_grid[4].draw (position + new Point_short (10, 133));
					party_mine_name.draw (position + new Point_short (27, 130));*/
				}
				else if (party_tab == (int)Buttons.BT_TAB_PARTY_SEARCH)
				{
					party_search_grid[0].draw (position);
					party_search_grid[1].draw (position);
					party_search_grid[2].draw (position);
					party_slider.draw (new Point_short (position));
				}
			}
			else if (tab == (int)Buttons.BT_TAB_FRIEND)
			{
				foreach (var sprite in friend_sprites)
				{
					sprite.draw (new Point_short (position), alpha);
				}

				friends_online_text.draw (position + new Point_short (211, 62));
				friends_cur_location.draw (position + new Point_short (9, 279));
				friend_grid[0].draw (position + new Point_short (10, 116));
				friend_grid[2].draw (position + new Point_short (10, 135));
				friends_name.draw (position + new Point_short (24, 134));
				friends_group_name.draw (position + new Point_short (29, 114));
				friends_slider.draw (new Point_short (position));
			}
			else if (tab == (int)Buttons.BT_TAB_BOSS)
			{
				foreach (var sprite in boss_sprites)
				{
					sprite.draw (new Point_short (position), alpha);
				}
			}
			else if (tab == (int)Buttons.BT_TAB_BLACKLIST)
			{
				blacklist_title.draw (position + new Point_short (24, 104));
				blacklist_grid[0].draw (position + new Point_short (24, 134));
				blacklist_name.draw (position + new Point_short (24, 134));
			}

			base.draw_buttons (alpha);
		}

		public void DrawPartyMember ()
		{
			if (partyMembers == null || partyMembers.Count == 0) return;

			Point_short startRelativePos = new Point_short (10, 133);
			for (int i = 0; i < Constants.get ().MAX_PartyMemberCount; i++)
			{
				var partyMember = partyMembers[i];
				if (partyMember.id == 0) continue;


				party_mine_gridButtons[i].set_position (startRelativePos);
				party_mine_gridButtons[i].draw (position);
				//party_mine_grid[4].draw (pos);

				var isValid = partyMember.isValid;
				var drawColor = isValid ? ms.Color.Name.BLACK : ms.Color.Name.GRAY;
				party_mine_name.change_color (drawColor);

				if (partyMember.id == PartyLeaderId)
				{
					if (isValid)
					{
						party_Flag_Online.draw (position + startRelativePos);
					}
					else
					{
						party_Flag_Offline.draw (position + startRelativePos);
					}
				}

				party_mine_name.change_text (partyMember.Name);
				party_mine_name.draw (position + startRelativePos.shift_x (20));

				party_mine_name.change_text (Job.get_name ((ushort)partyMember.jobid));
				party_mine_name.draw (position + startRelativePos.shift_x (100));

				party_mine_name.change_text (partyMember.level.ToString ());
				party_mine_name.draw (position + startRelativePos.shift_x (75));

				startRelativePos.shift_x (-195);
				startRelativePos.shift_y (20);
			}
		}


		public override void update ()
		{
			base.update ();

			if (tab == (int)Buttons.BT_TAB_FRIEND)
			{
				foreach (var sprite in friend_sprites)
				{
					sprite.update ();
				}
			}

			if (tab == (int)Buttons.BT_TAB_BOSS)
			{
				foreach (var sprite in boss_sprites)
				{
					sprite.update ();
				}
			}

			if (isStatusChanged)
			{
				isStatusChanged = false;
				SetCreateOrLeaveButtonActivity ();
			}
		}

		public override void send_key (int keycode, bool pressed, bool escape)
		{
			if (pressed)
			{
				if (escape)
				{
					deactivate ();
				}
				else if (keycode == (int)KeyAction.Id.TAB)
				{
					ushort new_tab = tab;

					if (new_tab < (int)Buttons.BT_TAB_BLACKLIST)
					{
						new_tab++;
					}
					else
					{
						new_tab = (int)Buttons.BT_TAB_FRIEND;
					}

					change_tab ((byte)new_tab);
				}
			}
		}

		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public enum Tab
		{
			FRIEND,
			PARTY,
			BOSS,
			BLACKLIST,
			PARTY_MINE,
			PARTY_SEARCH,
			FRIEND_ALL,
			FRIEND_ONLINE
		}

		public void change_tab (byte tabid)
		{
			byte oldtab = (byte)tab;
			tab = tabid;

			background = tabid == (int)((int)Buttons.BT_TAB_BOSS) ? UserList["Main"]["Boss"]["backgrnd3"] : UserList["Main"]["backgrnd2"];

			if (oldtab != tab)
			{
				buttons[(uint)((int)Buttons.BT_TAB_FRIEND + oldtab)].set_state (Button.State.NORMAL);
			}

			buttons[(uint)((int)Buttons.BT_TAB_FRIEND + tab)].set_state (Button.State.PRESSED);

			if (tab == (int)Buttons.BT_TAB_PARTY)
			{
				buttons[(int)Buttons.BT_PARTY_CREATE].set_active (true);
				buttons[(int)Buttons.BT_PARTY_INVITE].set_active (true);
				buttons[(int)Buttons.BT_TAB_PARTY_MINE].set_active (true);
				buttons[(int)Buttons.BT_TAB_PARTY_SEARCH].set_active (true);

				change_party_tab ((byte)Tab.PARTY_MINE);
			}
			else
			{
				buttons[(int)Buttons.BT_PARTY_CREATE].set_active (false);
				buttons[(int)Buttons.BT_PARTY_INVITE].set_active (false);
				buttons[(int)Buttons.BT_TAB_PARTY_MINE].set_active (false);
				buttons[(int)Buttons.BT_TAB_PARTY_SEARCH].set_active (false);
				buttons[(int)Buttons.BT_PARTY_SEARCH_LEVEL].set_active (false);
			}

			if (tab == (int)Buttons.BT_TAB_FRIEND)
			{
				buttons[(int)Buttons.BT_FRIEND_ADD].set_active (true);
				buttons[(int)Buttons.BT_FRIEND_ADD_GROUP].set_active (true);
				buttons[(int)Buttons.BT_FRIEND_EXPAND].set_active (true);
				buttons[(int)Buttons.BT_TAB_FRIEND_ALL].set_active (true);
				buttons[(int)Buttons.BT_TAB_FRIEND_ONLINE].set_active (true);
				buttons[(int)Buttons.BT_FRIEND_GROUP_0].set_active (true);

				change_friend_tab ((byte)Tab.FRIEND_ALL);
			}
			else
			{
				buttons[(int)Buttons.BT_FRIEND_ADD].set_active (false);
				buttons[(int)Buttons.BT_FRIEND_ADD_GROUP].set_active (false);
				buttons[(int)Buttons.BT_FRIEND_EXPAND].set_active (false);
				buttons[(int)Buttons.BT_TAB_FRIEND_ALL].set_active (false);
				buttons[(int)Buttons.BT_TAB_FRIEND_ONLINE].set_active (false);
				buttons[(int)Buttons.BT_FRIEND_GROUP_0].set_active (false);
			}

			if (tab == (int)Buttons.BT_TAB_BOSS)
			{
				buttons[(int)Buttons.BT_BOSS_0].set_active (true);
				buttons[(int)Buttons.BT_BOSS_1].set_active (true);
				buttons[(int)Buttons.BT_BOSS_2].set_active (true);
				buttons[(int)Buttons.BT_BOSS_3].set_active (true);
				buttons[(int)Buttons.BT_BOSS_4].set_active (true);
				buttons[(int)Buttons.BT_BOSS_L].set_active (true);
				buttons[(int)Buttons.BT_BOSS_R].set_active (true);
				buttons[(int)Buttons.BT_BOSS_DIFF_L].set_active (true);
				buttons[(int)Buttons.BT_BOSS_DIFF_R].set_active (true);
				buttons[(int)Buttons.BT_BOSS_GO].set_active (true);
				buttons[(int)Buttons.BT_BOSS_L].set_state (Button.State.DISABLED);
				buttons[(int)Buttons.BT_BOSS_R].set_state (Button.State.DISABLED);
				buttons[(int)Buttons.BT_BOSS_GO].set_state (Button.State.DISABLED);
				buttons[(int)Buttons.BT_BOSS_DIFF_L].set_state (Button.State.DISABLED);
				buttons[(int)Buttons.BT_BOSS_DIFF_R].set_state (Button.State.DISABLED);
			}
			else
			{
				buttons[(int)Buttons.BT_BOSS_0].set_active (false);
				buttons[(int)Buttons.BT_BOSS_1].set_active (false);
				buttons[(int)Buttons.BT_BOSS_2].set_active (false);
				buttons[(int)Buttons.BT_BOSS_3].set_active (false);
				buttons[(int)Buttons.BT_BOSS_4].set_active (false);
				buttons[(int)Buttons.BT_BOSS_L].set_active (false);
				buttons[(int)Buttons.BT_BOSS_R].set_active (false);
				buttons[(int)Buttons.BT_BOSS_DIFF_L].set_active (false);
				buttons[(int)Buttons.BT_BOSS_DIFF_R].set_active (false);
				buttons[(int)Buttons.BT_BOSS_GO].set_active (false);
			}

			if (tab == (int)Buttons.BT_TAB_BLACKLIST)
			{
				buttons[(int)Buttons.BT_BLACKLIST_ADD].set_active (true);
				buttons[(int)Buttons.BT_BLACKLIST_DELETE].set_active (true);
				buttons[(int)Buttons.BT_TAB_BLACKLIST_INDIVIDUAL].set_active (true);
				buttons[(int)Buttons.BT_TAB_BLACKLIST_GUILD].set_active (true);
			}
			else
			{
				buttons[(int)Buttons.BT_BLACKLIST_ADD].set_active (false);
				buttons[(int)Buttons.BT_BLACKLIST_DELETE].set_active (false);
				buttons[(int)Buttons.BT_TAB_BLACKLIST_INDIVIDUAL].set_active (false);
				buttons[(int)Buttons.BT_TAB_BLACKLIST_GUILD].set_active (false);
			}
		}

		public ushort get_tab ()
		{
			return tab;
		}

		public override Button.State button_pressed (ushort buttonid)
		{
			switch ((Buttons)buttonid)
			{
				case Buttons.BT_CLOSE:
					deactivate ();
					break;
				case Buttons.BT_TAB_FRIEND:
				case Buttons.BT_TAB_PARTY:
				case Buttons.BT_TAB_BOSS:
				case Buttons.BT_TAB_BLACKLIST:
					change_tab ((byte)buttonid);
					return Button.State.PRESSED;
				case Buttons.BT_TAB_PARTY_MINE:
				case Buttons.BT_TAB_PARTY_SEARCH:
					change_party_tab ((byte)buttonid);
					return Button.State.PRESSED;
				case Buttons.BT_TAB_FRIEND_ALL:
				case Buttons.BT_TAB_FRIEND_ONLINE:
					change_friend_tab ((byte)buttonid);
					return Button.State.PRESSED;
				case Buttons.BT_TAB_BLACKLIST_INDIVIDUAL:
				case Buttons.BT_TAB_BLACKLIST_GUILD:
					change_blacklist_tab ((byte)buttonid);
					return Button.State.PRESSED;
				case Buttons.BT_PARTY_CREATE:
					OnClick_BT_PARTY_CREATE ();
					break;
				case Buttons.BT_PARTY_LEAVE:
					OnClick_BT_PARTY_LEAVE ();
					break;
				default:
					return Button.State.NORMAL;
			}

			return Button.State.NORMAL;
		}

		public static void OnPartyDataChanged (List<MaplePartyCharacter> arg1, int arg2)
		{
			updatePartyData (arg1, arg2);
			isStatusChanged = true;
		}
		
		private void OnClick_BT_PARTY_LEAVE ()
		{
			/*PartyLeaderId = 0;
			partyMembers.Clear ();*/

			new LeavePartyPacket ().dispatch ();
		}

		private void OnClick_BT_PARTY_CREATE ()
		{
			new CreatePartyPacket ().dispatch ();
		}

	

		public override void rightclick (Point_short cursorpos)
		{
			if (partyMembers == null || partyMembers.Count == 0) return;
			var gridButtonPos = position + new Point_short (10, 133);

			for (int i = 0; i < Constants.get ().MAX_PartyMemberCount; i++)
			{
				var party_mine_gridButton = party_mine_gridButtons[i];
				var partyMember = partyMembers[i];
				if (party_mine_gridButton.is_active () && party_mine_gridButton.bounds (position).contains (cursorpos) && !Stage.get ().is_player (partyMember.id))
				{
					var partySideMenu = UI.get ().emplace<UIPartySideMenu> ();
					if (partySideMenu)
					{
						partySideMenu.get ().SetDisplayInfo (cursorpos, partyMembers.TryGet (i), PartyLeaderId);
					}
				}

				//gridButtonPos.shift_y (20);
			}
		}

		public override void OnGet ()
		{
			isStatusChanged = true;
		}

		private void SetCreateOrLeaveButtonActivity ()
		{
			if (buttons.Count > 0)
			{
				if (PartyLeaderId != 0) //has a party
				{
					buttons[(int)Buttons.BT_PARTY_CREATE].set_active (false);
					buttons[(int)Buttons.BT_PARTY_LEAVE].set_active (true);
				}
				else
				{
					buttons[(int)Buttons.BT_PARTY_CREATE].set_active (true);
					buttons[(int)Buttons.BT_PARTY_LEAVE].set_active (false);
				}
			}
		}

		private void change_party_tab (byte tabid)
		{
			byte oldtab = (byte)party_tab;
			party_tab = tabid;

			if (oldtab != party_tab)
			{
				buttons[(uint)((int)Buttons.BT_TAB_FRIEND + oldtab)].set_state (Button.State.NORMAL);
			}

			buttons[(uint)((int)Buttons.BT_TAB_FRIEND + party_tab)].set_state (Button.State.PRESSED);

			if (party_tab == (int)Buttons.BT_TAB_PARTY_SEARCH)
			{
				buttons[(int)Buttons.BT_PARTY_SEARCH_LEVEL].set_active (true);
			}
			else
			{
				buttons[(int)Buttons.BT_PARTY_SEARCH_LEVEL].set_active (false);
			}
		}

		private void change_friend_tab (byte tabid)
		{
			byte oldtab = (byte)friend_tab;
			friend_tab = tabid;

			if (oldtab != friend_tab)
			{
				buttons[(uint)((int)Buttons.BT_TAB_FRIEND + oldtab)].set_state (Button.State.NORMAL);
			}

			buttons[(uint)((int)Buttons.BT_TAB_FRIEND + friend_tab)].set_state (Button.State.PRESSED);
		}

		private void change_blacklist_tab (byte tabid)
		{
			byte oldtab = (byte)blacklist_tab;
			blacklist_tab = tabid;

			if (oldtab != blacklist_tab)
			{
				buttons[(uint)((int)Buttons.BT_TAB_FRIEND + oldtab)].set_state (Button.State.NORMAL);
			}

			buttons[(uint)((int)Buttons.BT_TAB_FRIEND + blacklist_tab)].set_state (Button.State.PRESSED);
		}

		private string get_cur_location ()
		{
			return "Henesys Market";
		}

		private enum Buttons
		{
			BT_TAB_FRIEND,
			BT_TAB_PARTY,
			BT_TAB_BOSS,
			BT_TAB_BLACKLIST,
			BT_TAB_PARTY_MINE,
			BT_TAB_PARTY_SEARCH,
			BT_TAB_FRIEND_ALL,
			BT_TAB_FRIEND_ONLINE,
			BT_TAB_BLACKLIST_INDIVIDUAL,
			BT_TAB_BLACKLIST_GUILD,
			BT_CLOSE,
			BT_PARTY_CREATE,
			BT_PARTY_INVITE,
			BT_PARTY_LEAVE,
			BT_PARTY_SETTINGS,
			BT_PARTY_SEARCH_LEVEL,
			BT_PARTY_SEARCH_INVITE,
			BT_FRIEND_ADD,
			BT_FRIEND_ADD_GROUP,
			BT_FRIEND_EXPAND,
			BT_FRIEND_GROUP_0,
			BT_BOSS_0,
			BT_BOSS_1,
			BT_BOSS_2,
			BT_BOSS_3,
			BT_BOSS_4,
			BT_BOSS_L,
			BT_BOSS_R,
			BT_BOSS_DIFF_L,
			BT_BOSS_DIFF_R,
			BT_BOSS_GO,
			BT_BLACKLIST_ADD,
			BT_BLACKLIST_DELETE
		}

		private ushort tab;
		private MapleData UserList;
		private Texture background = new Texture ();

		// Party tab
		private ushort party_tab;
		private Texture party_title = new Texture ();
		private Texture[] party_mine_grid = new Texture[5];
		private MapleButton[] party_mine_gridButtons = new MapleButton[Constants.get ().MAX_PartyMemberCount];
		private Texture[] party_search_grid = new Texture[3];
		private Text party_mine_name = new Text ();
		private Slider party_slider = new Slider ();

		private Texture party_Flag_Online;

		private Texture party_Flag_Offline;

		// Buddy tab
		private ushort friend_tab;
		private int friend_count = 0;
		private int friend_total = 50;
		private List<Sprite> friend_sprites = new List<Sprite> ();
		private Texture[] friend_grid = new Texture[4];
		private Text friends_online_text = new Text ();
		private Text friends_cur_location = new Text ();
		private Text friends_name = new Text ();
		private Text friends_group_name = new Text ();
		private Slider friends_slider = new Slider ();

		// Boss tab
		private List<Sprite> boss_sprites = new List<Sprite> ();

		// Blacklist tab
		private ushort blacklist_tab;
		private Texture blacklist_title = new Texture ();
		private Texture[] blacklist_grid = new Texture[3];
		private Text blacklist_name = new Text ();


		static List<MaplePartyCharacter> partyMembers = new List<MaplePartyCharacter> ();

		private static int PartyLeaderId
		{
			get => partyLeaderId;
			set
			{
				//isStatusChanged = true;
				partyLeaderId = value;
			}
		}

		private static int partyLeaderId;

		private static bool isStatusChanged;

		public static void updatePartyData (List<MaplePartyCharacter> partyMemberArray, int leaderId)
		{

			partyMembers.Clear ();
			partyMembers.AddRange (partyMemberArray);

			PartyLeaderId = leaderId;
		}

		public override void Dispose ()
		{
			base.Dispose ();
			//UserList?.Dispose ();
			background?.Dispose ();
			party_title?.Dispose ();
			foreach (var t in party_mine_grid)
			{
				t?.Dispose ();
			}
			foreach (var t in party_mine_gridButtons)
			{
				t?.Dispose ();
			}
			foreach (var t in party_search_grid)
			{
				t?.Dispose ();
			}
			
			party_slider?.Dispose ();
			party_Flag_Online?.Dispose ();
			party_Flag_Offline?.Dispose ();
			foreach (var t in friend_sprites)
			{
				t?.Dispose ();
			}
			foreach (var t in friend_grid)
			{
				t?.Dispose ();
			}
			
			friends_slider?.Dispose ();
			foreach (var t in boss_sprites)
			{
				t?.Dispose ();
			}
			blacklist_title?.Dispose ();
			foreach (var t in blacklist_grid)
			{
				t?.Dispose ();
			}
			
		}
	}
}


#if USE_NX
#endif