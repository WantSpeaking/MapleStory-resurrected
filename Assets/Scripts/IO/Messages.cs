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
	public class Messages
	{
		public enum Type
		{
			NONE,

			// Cannot use a skill
			SKILL_WEAPONTYPE,
			SKILL_HPCOST,
			SKILL_MPCOST,
			SKILL_NOARROWS,
			SKILL_NOBULLETS,
			SKILL_NOSTARS,
			SKILL_COOLDOWN,

			// Scrolling result
			SCROLL_SUCCESS,
			SCROLL_FAILURE,
			SCROLL_DESTROYED,
			LENGTH
		}

		public static EnumMap<Type, string> messages =new EnumMap<Type, string> ()
		{
			[Type.NONE] = "",
			[Type.SKILL_WEAPONTYPE] = "You cannot use this skill with this weapon.",
			[Type.SKILL_HPCOST] = "You do not have enough hp to use this skill.",
			[Type.SKILL_MPCOST] = "You do not have enough mp to use this skill.",
			[Type.SKILL_NOARROWS] = "You do not have enough arrows to use this attack.",
			[Type.SKILL_NOBULLETS] = "You do not have enough bullets to use this attack.",
			[Type.SKILL_NOSTARS] = "You do not have enough throwing stars to use this attack.",
			[Type.SKILL_COOLDOWN] = "You cannot use this skill as it is on cooldown.",
			[Type.SCROLL_SUCCESS] = "The scroll lights up and it's mysterious powers have been transferred to the item.",
			[Type.SCROLL_FAILURE] = "The scroll lights up but the item remains as if nothing happened.",
			[Type.SCROLL_DESTROYED] = "The item has been destroyed due to the overwhelming power of the scroll."
		};
	}

	public class InChatMessage
	{
		public InChatMessage (Messages.Type t)
		{
			type = t;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void drop() const
		public void drop ()
		{
			if (type == Messages.Type.NONE)
			{
				return;
			}

			/*todo var chatbar = UI.get ().get_element<UIChatBar> ();
			if (chatbar!=null)
			{
				chatbar.display_message (type, UIChatBar.RED);
			}*/
		}

		private Messages.Type type;
	}

	public class ForbidSkillMessage : InChatMessage
	{
		public ForbidSkillMessage (SpecialMove.ForbidReason reason, Weapon.Type weapon) : base (message_by_reason (reason, weapon))
		{
		}

		private static Messages.Type message_by_reason (SpecialMove.ForbidReason reason, Weapon.Type weapon)
		{
			switch (reason)
			{
				case SpecialMove.ForbidReason.FBR_WEAPONTYPE:
					return Messages.Type.SKILL_WEAPONTYPE;
				case SpecialMove.ForbidReason.FBR_HPCOST:
					return Messages.Type.SKILL_HPCOST;
				case SpecialMove.ForbidReason.FBR_MPCOST:
					return Messages.Type.SKILL_MPCOST;
				case SpecialMove.ForbidReason.FBR_COOLDOWN:
					return Messages.Type.SKILL_COOLDOWN;
				case SpecialMove.ForbidReason.FBR_BULLETCOST:
					return message_by_weapon (weapon);
				default:
					return Messages.Type.NONE;
			}
		}

		private static Messages.Type message_by_weapon (Weapon.Type weapon)
		{
			switch (weapon)
			{
				case Weapon.Type.BOW:
				case Weapon.Type.CROSSBOW:
					return Messages.Type.SKILL_NOARROWS;
				case Weapon.Type.CLAW:
					return Messages.Type.SKILL_NOSTARS;
				default:
					return Messages.Type.SKILL_NOBULLETS;
			}
		}
	}
}