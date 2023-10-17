﻿using provider;
using System.Collections.Generic;

/*
 This file is part of the OdinMS Maple Story Server
 Copyright (C) 2008 Patrick Huy <patrick.huy@frz.cc>
 Matthias Butz <matze@odinms.de>
 Jan Christian Meyer <vimes@odinms.de>

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU Affero General Public License as
 published by the Free Software Foundation version 3 as published by
 the Free Software Foundation. You may not use, modify or distribute
 this program under any other version of the GNU Affero General Public
 License.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU Affero General Public License for more details.

 You should have received a copy of the GNU Affero General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
namespace server.quest.requirements
{
	using MapleCharacter = client.MapleCharacter;
	using MaplePet = client.inventory.MaplePet;
	using WzImageProperty = MapleLib.WzLib.WzImageProperty;
	using MapleDataTool = provider.MapleDataTool;

	/// 
	/// <summary>
	/// @author Tyler (Twdtwd)
	/// </summary>
	public class PetRequirement : MapleQuestRequirement
	{
		internal IList<int> petIDs = new List<int> ();


		public PetRequirement (MapleQuest quest, MapleData data) : base (MapleQuestRequirementType.PET)
		{
			processData (data);
		}


		public override void processData (MapleData data)
		{
			foreach (var petData in data)
			{
				petIDs.Add (MapleDataTool.getInt (petData.getChildByPath("id")));
			}
		}


		public override bool check (MapleCharacter chr, int? npcid)
		{

			foreach (var pet in player.pets)
			{
				if (pet == null)
				{
					continue; // thanks Arufonsu for showing a NPE occurring here
				}

				if (petIDs.Contains (pet.get_itemid()))
				{
					return true;
				}
			}

			return false;
		}
	}

}