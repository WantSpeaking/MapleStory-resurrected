


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
		}

		public static EnumMap<Type, string> messages =new EnumMap<Type, string> ()
		{
			[Type.NONE] = "",
			[Type.SKILL_WEAPONTYPE] = "你不能用这个技能来使用这个武器。",
			[Type.SKILL_HPCOST] = "您没有足够的HP来使用此技能。",
			[Type.SKILL_MPCOST] = "您没有足够的MP来使用此技能。",
			[Type.SKILL_NOARROWS] = "您没有足够的箭矢来使用此攻击。",
			[Type.SKILL_NOBULLETS] = "您没有足够的子弹来使用此攻击。",
            [Type.SKILL_NOSTARS] = "您没有足够的飞镖来使用此攻击。",
			[Type.SKILL_COOLDOWN] = "您不能使用此技能，因为它在冷却时间。",
			[Type.SCROLL_SUCCESS] = "卷轴亮起，它的神秘力量已经转移到物品上。",
			[Type.SCROLL_FAILURE] = "卷轴亮起，但装备保持原样，好像什么都没发生。",
			[Type.SCROLL_DESTROYED] = "由于卷轴的压倒性力量，该物品已被摧毁。"
		};
	}

	public class InChatMessage
	{
		public InChatMessage (Messages.Type t)
		{
			type = t;
		}

		public void drop ()
		{
			if (type == Messages.Type.NONE)
			{
				return;
			}

			/*var chatbar = UI.get ().get_element<UIChatBar> ();
			if (chatbar)
			{
				chatbar.get().display_message (type, UIChatBar.LineType.RED);
			}*/
			var statusMessenger =  UI.get().get_element<UIStatusMessenger>();
			if (statusMessenger)
			{
				statusMessenger.get().show_status(Color.Name.WHITE, Messages.messages[type]);

            }
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