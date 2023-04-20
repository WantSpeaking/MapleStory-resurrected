using System.Collections.Generic;
using MapleLib.WzLib;
using ms;

namespace ms
{
	[Beebyte.Obfuscator.Skip]
	public class UIPartyMember_HP : UIDragElement<PosPartyMemberHP>
	{
		public const Type TYPE = UIElement.Type.PartyMember_HP;
		public const bool FOCUSED = false;
		public const bool TOGGLED = false;

		public UIPartyMember_HP () : base (new Point_short (63, 90))
		{
			WzObject src = ms.wz.wzFile_ui["UIWindow.img"]["UserList"]["Main"]["Party"]["PartyHP"];
			var GaugeBar = src["GaugeBar"];
			for (int i = 0; i < Constants.get ().MAX_PartyMemberCount; i++)
			{
				party_member_hpBars[i] = new Gauge (Gauge.Type.GAME, GaugeBar["gauge"], GaugeBar["graduation"], GaugeBar["bar"], 63, 1);
				party_member_hpBars[i].Setbarfront_PosOffset (new Point_short (3, 3));
			}

			char_HeadHPBar = new Gauge (Gauge.Type.GAME, GaugeBar["gauge"], GaugeBar["graduation"], GaugeBar["bar"], 63, 1);
			char_HeadHPBar.Setbarfront_PosOffset (new Point_short (3, 3));

			text_CharacterName = new Text (Text.Font.A12M, Text.Alignment.LEFT, Color.Name.BLACK, "", 0);
		}


		public override void update ()
		{
			//AppDebug.Log ($"dimension:{dimension}\t dragarea:{dragarea}");
			if (isStatusChanged)
			{
				isStatusChanged = false;
				OnPartyUIChanged ();
				UpdateHpBar_Player ();
			}

			base.update ();
		}

		public override void draw (float alpha)
		{
			if (partyMembers == null || validCharCount == 0) return;

			Point_short startRelativePos = new Point_short (10, 10);
			for (int i = 0; i < Constants.get ().MAX_PartyMemberCount; i++)
			{
				var partyMember = partyMembers[i];
				var party_member_hpBar = party_member_hpBars[i];
				party_member_hpBar.active = partyMember.isValid;
				if (party_member_hpBar.active == false) continue;

				//draw partyMember ui 
				{
					text_CharacterName.change_text (partyMember.Name);
					text_CharacterName.draw (position + startRelativePos);

					party_member_hpBar.draw (position + startRelativePos.shift_x (textWidth));

					startRelativePos.shift_x ((short)-textWidth);
					startRelativePos.shift_y (hpBarHeight);
				}

				//draw char headBar ui
				{
					if (!Stage.get ().is_player (partyMember.id))
					{
						var character = Stage.get ().get_character (partyMember.id);
						if (character)
						{
							AppDebug.Log ($"headBarPos: {character.get ().headBarPos ()}");
							party_member_hpBar.draw (character.get ().headBarPos () + new Point_short ((short)((-hpBarWidth / 2) + 8), 0));
						}
					}
				}
			}

			base.draw (alpha);
		}

		public void UpdateHpBar_Char (int cid, float percent)
		{
			if (partyMembers == null || validCharCount == 0) return;

			for (int i = 0; i < Constants.get ().MAX_PartyMemberCount; i++)
			{
				if (partyMembers[i].id == cid)
				{
					party_member_hpBars[i].update (percent);
				}
			}
		}

		public void UpdateHpBar_Char (int cid, int curHp, int maxHp)
		{
			UpdateHpBar_Char (cid, (float)curHp / maxHp);
		}

		public void UpdateHpBar_Player ()
		{
			var player = Stage.get ().get_player ();
			var cid = player.get_oid ();
			var percent = player.gethppercent ();
			UpdateHpBar_Char (cid, percent);
		}

		public override Type get_type ()
		{
			return TYPE;
		}

		private void OnPartyUIChanged ()
		{
			dimension = new Point_short (centerWidth, centerHeight);
			dragarea = new Point_short (centerWidth, centerHeight);
		}

		public static void OnPartyDataChanged (List<MaplePartyCharacter> partyMemberArray, int leaderId)
		{
			partyMembers.Clear ();
			partyMembers.AddRange (partyMemberArray);

			PartyLeaderId = leaderId;

			isStatusChanged = true;

			validCharCount = partyMembers.count (partyMember => partyMember.isValid);
		}

		private Text text_CharacterName;
		private readonly Gauge[] party_member_hpBars = new Gauge[Constants.get ().MAX_PartyMemberCount];
		private readonly Gauge char_HeadHPBar;
		private short hpBarHeight = 15;
		private short hpBarWidth = 69;
		private short textWidth = 69;
		private static int validCharCount = 0;
		private short centerHeight => (short)(validCharCount * hpBarHeight);
		private short centerWidth => (short)(textWidth + hpBarWidth);

		private static readonly List<MaplePartyCharacter> partyMembers = new List<MaplePartyCharacter> ();

		private static int PartyLeaderId;

		private static bool isStatusChanged;
	}
}