using System.Collections.Generic;
using ms.Util;
using ms_Unity;

namespace ms
{
	public class PartyOperationHandlers : PacketHandler
	{
		public enum PartyOperation
		{
			JOIN,
			LEAVE,
			EXPEL,
			DISBAND,
			SILENT_UPDATE,
			LOG_ONOFF,
			CHANGE_LEADER
		}

		private int MAX_PartyMemberCount => Constants.get ().MAX_PartyMemberCount;

		public override void handle (InPacket recv)
		{
			var operation = recv.read_byte ();

			AppDebug.Log ($"PartyOperation:{operation}");

			switch (operation)
			{
				case 1:
				case 5:
				case 6:
				case 11:
				case 14:
					ShowStatusMessage (Color.Name.WHITE, "Your request for a party didn't work due to an unexpected error.");
					break;
				case 4: //partyInvite partySearchInvite
				{
					var partyId = recv.read_int ();
					var fromCharName = recv.read_string ();

					var temp0 = recv.read_byte ();

					var fromChar = Stage.get ().get_character (fromCharName);
					if (fromChar)
					{
						/*var uiFadePartyInvite = UI.get ().get_element<UIFadeYesNo_PartyInvite> ();
						if (!uiFadePartyInvite)
						{
							uiFadePartyInvite = UI.get ().emplace<UIFadeYesNo_PartyInvite> ();
						}*/
						var uiFadePartyInvite = UI.get ().emplace<UIFadeYesNo_PartyInvite> ();
						uiFadePartyInvite.get ()?.SetInviteInfo (partyId, fromChar);
					}
				}
					break;
				case 7: //SILENT_UPDATE LOG_ONOFF
				{
					var partyId = recv.read_int ();
					//AppDebug.Log ($"partyId:{partyId}");
					ParsePartyStatus (recv);
					UpdatePartyStatus (partymembers, partyLeaderId);

					break;
				}
				case 8: //partyCreated
				{
					var partyId = recv.read_int ();
					MapleDoor partyDoor = new MapleDoor ();
					partyDoor.toMapId = recv.read_int ();
					partyDoor.fromMapId = recv.read_int ();
					partyDoor.positionX = recv.read_int ();
					partyDoor.positionY = recv.read_int ();

					break;
				}
				case 10:
					ShowStatusMessage (Color.Name.WHITE, "A beginner can't create a party");
					break;
				case 12: //DISBAND EXPEL LEAVE
				{
					var partyId = recv.read_int ();
					var targetId = recv.read_int ();

					bool DISBAND = recv.read_byte () == 0;
					bool EXPEL = false;
					var targetName = string.Empty;
					if (DISBAND)
					{
						var partyId2 = recv.read_int ();
						ShowStatusMessage (Color.Name.WHITE, "DISBAND");
					}
					else
					{
						EXPEL = recv.read_byte () == 1;
						if (EXPEL)
						{
							ShowStatusMessage (Color.Name.WHITE, "EXPEL");
						}
						else
						{
							ShowStatusMessage (Color.Name.WHITE, "LEAVE");
						}

						targetName = recv.read_string ();

						//AppDebug.Log ($"partyId:{partyId}\t targetId:{targetId}\t targetName:{targetName}\t playerId:{Stage.get ().get_player ().get_oid ()}");
						ParsePartyStatus (recv);
					}

					if (DISBAND)
					{
						ResetPartyStatus ();
						UpdatePartyStatus (partymembers, 0);
					}
					else /*if (EXPEL)*/
					{
						if (Stage.get ().is_player (targetId) && partyLeaderId != targetId) //if this player was not leader, when EXPEL or LEAVE ,then clear partyList, set partyLeaderId =0
						{
							ResetPartyStatus ();
							UpdatePartyStatus (partymembers, 0);
						}
						else
						{
							UpdatePartyStatus (partymembers, partyLeaderId);
						}
					}
					/*else//LEAVE
					{
						ResetPartyStatus ();
						UpdatePartyStatus (partymembers, 0);
					}*/

					break;
				}
				case 13:
				{
					ShowStatusMessage (Color.Name.WHITE, "You have yet to join a party");
					break;
				}
				case 15: //JOIN
				{
					var partyId = recv.read_int ();
					var targetName = recv.read_string ();
					ParsePartyStatus (recv);
					UpdatePartyStatus (partymembers, partyLeaderId);
					break;
				}

				case 16:
					ShowStatusMessage (Color.Name.WHITE, "Already have joined a party");
					break;
				case 17:
					ShowStatusMessage (Color.Name.WHITE, "The party you're trying to join is already in full capacity.");
					break;
				case 19:
					ShowStatusMessage (Color.Name.WHITE, "Unable to find the requested character in this channel.");
					break;
				case 21:
					ShowStatusMessage (Color.Name.WHITE, "Player is blocking any party invitations.");
					break;
				case 22:
					ShowStatusMessage (Color.Name.WHITE, "Player is taking care of another invitation.");
					break;
				case 23:
					ShowStatusMessage (Color.Name.WHITE, "Player have denied request to the party.");
					break;
				case 25:
					ShowStatusMessage (Color.Name.WHITE, "Cannot kick another user in this map.");
					break;
				case 27: //CHANGE_LEADER
				{
					var leaderId = recv.read_int ();
					var temp0 = recv.read_byte ();
					partyLeaderId = leaderId;
					UpdatePartyStatus (partymembers, partyLeaderId);
					break;
				}
				case 28:
				case 29:
					ShowStatusMessage (Color.Name.WHITE, "Leadership can only be given to a party member in the vicinity.");
					break;
				case 30:
					ShowStatusMessage (Color.Name.WHITE, "Change leadership only on same channel.");
					break;
				case 35: //partyPortal
				{
					break;
				}
			}
		}

		private void ShowStatusMessage (Color.Name colorName, string message)
		{
			UI.get ().get_element<UIStatusMessenger> ().get ().show_status (colorName, message);
		}

		public static List<MaplePartyCharacter> partymembers;
		public static List<MapleDoor> partyDoors;
		public static int partyLeaderId;

		private void ParsePartyStatus (InPacket recv)
		{
			ResetPartyStatus ();

			/*if (partymembers == null)
			{
				partymembers = new List<MaplePartyCharacter> (MAX_PartyMemberCount);
				for (int i = 0; i < MAX_PartyMemberCount; i++)
				{
					partymembers.Add (new MaplePartyCharacter ());
				}
			}
			else
			{
				for (int i = 0; i < MAX_PartyMemberCount; i++)
				{
					partymembers[i].Reset ();
				}
			}
			if (partyDoors == null)
			{
				partyDoors = new List<MapleDoor> (MAX_PartyMemberCount);
				for (int i = 0; i < MAX_PartyMemberCount; i++)
				{
					partyDoors.Add (new MapleDoor ());
				}
			}
			else
			{
				for (int i = 0; i < MAX_PartyMemberCount; i++)
				{
					partyDoors[i].Reset ();
				}
			}*/

			for (int i = 0; i < MAX_PartyMemberCount; i++)
			{
				partymembers[i].id = recv.read_int ();
			}

			for (int i = 0; i < MAX_PartyMemberCount; i++)
			{
				partymembers[i].Name = recv.read_padded_string (13);
			}

			for (int i = 0; i < MAX_PartyMemberCount; i++)
			{
				partymembers[i].jobid = recv.read_int ();
			}

			for (int i = 0; i < MAX_PartyMemberCount; i++)
			{
				partymembers[i].level = recv.read_int ();
			}

			for (int i = 0; i < MAX_PartyMemberCount; i++)
			{
				partymembers[i].channel = recv.read_int ();
			}

			partyLeaderId = recv.read_int ();
			for (int i = 0; i < MAX_PartyMemberCount; i++)
			{
				partymembers[i].mapid = recv.read_int ();
			}


			for (int i = 0; i < MAX_PartyMemberCount; i++)
			{
				partyDoors[i].townId = recv.read_int ();
				partyDoors[i].areaId = recv.read_int ();
				partyDoors[i].positionX = recv.read_int ();
				partyDoors[i].positionY = recv.read_int ();
			}
		}

		private void ResetPartyStatus ()
		{
			partymembers = new List<MaplePartyCharacter> (MAX_PartyMemberCount);
			for (int i = 0; i < MAX_PartyMemberCount; i++)
			{
				partymembers.Add (new MaplePartyCharacter ());
			}

			partyDoors = new List<MapleDoor> (MAX_PartyMemberCount);
			for (int i = 0; i < MAX_PartyMemberCount; i++)
			{
				partyDoors.Add (new MapleDoor ());
			}
		}

		private void UpdatePartyStatus (List<MaplePartyCharacter> partyMemberArray, int leaderId)
		{
			MessageCenter.get ().PartyDataChanged?.Invoke (partyMemberArray, leaderId);
			//UIUserList.updateParty (partyMemberArray, leaderId);
		}
	}

	public class UPDATE_PARTYMEMBER_HPHandlers : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			var cid = recv.read_int ();
			var curhp = recv.read_int ();
			var maxhp = recv.read_int ();

			/*var partyHP = UI.get ().get_element<UIPartyMember_HP> ();
			if (partyHP == false)
			{
				partyHP = UI.get ().emplace<UIPartyMember_HP> ();
			}

			partyHP.get ().UpdateHpBar_Char (cid, curhp, maxhp);*/
            FGUI_Manager.Instance.GetFGUI<FGUI_StatusBar>()._QuestLogMini.UpdateHpBar_Char(cid, curhp, maxhp);

        }
	}
}