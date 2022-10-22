/*
    This file is part of the HeavenMS MapleStory Server
    Copyleft (L) 2016 - 2019 RonanLana

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
namespace server.quest.actions
{
	using MapleClient = client.MapleClient;
	using MapleCharacter = client.MapleCharacter;
	using MaplePet = client.inventory.MaplePet;
	using WzImageProperty = MapleLib.WzLib.WzImageProperty;
	using MapleDataTool = provider.MapleDataTool;

	/// 
	/// <summary>
	/// @author Ronan
	/// </summary>
	public class PetSpeedAction : MapleQuestAction
	{

		public PetSpeedAction(MapleQuest quest, WzImageProperty data) : base(MapleQuestActionType.PETTAMENESS, quest)
		{
			questID = quest.Id;
		}


		public override void processData(WzImageProperty data)
		{
		}
		public override void run(MapleCharacter chr, int? extSelection)
		{
					/*MapleClient c = chr.Client;

					MaplePet pet = chr.getPet(0); // assuming here only the pet leader will gain owner speed
					if (pet == null)
					{
						return;
					}

					c.lockClient();
					try
					{
							pet.addPetFlag(c.Player, MaplePet.PetFlag.OWNER_SPEED);
					}
					finally
					{
						c.unlockClient();
					}*/

		}
	}

}