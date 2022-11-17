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
	using WzImageProperty = MapleLib.WzLib.WzImageProperty;
	using MapleCharacter = client.MapleCharacter;

	/// <summary>
	/// Base class for a Quest Requirement. Quest system uses it for all requirements.
	/// @author Tyler (Twdtwd)
	/// </summary>
	public abstract class MapleQuestRequirement
	{
		private readonly MapleQuestRequirementType type;
		protected ms.Player player => ms.Stage.get ().get_player ();

		public MapleQuestRequirement(MapleQuestRequirementType type)
		{
			this.type = type;
		}

		/// <summary>
		/// Checks the requirement to see if the player currently meets it. </summary>
		/// <param name="chr">	The <seealso cref="MapleCharacter"/> to check on. </param>
		/// <param name="npcid">	The NPC ID it was called from. </param>
		/// <returns> boolean	If the check was passed or not. </returns>
		public abstract bool check(MapleCharacter chr, int? npcid);

		/// <summary>
		/// Processes the data and stores it in the class for future use. </summary>
		/// <param name="data"> The data to process. </param>
		public abstract void processData(WzImageProperty data);

		public virtual MapleQuestRequirementType Type
		{
			get
			{
					return type;
			}
		}
	}
}