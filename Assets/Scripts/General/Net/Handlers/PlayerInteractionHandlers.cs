


namespace ms
{
	// Handler for a packet which contains character info
	public class CharInfoHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle(InPacket recv)
		{
			int character_id = recv.read_int();
			sbyte character_level = recv.read_byte();
			short character_job_id = recv.read_short();
			short character_fame = recv.read_short();
			recv.skip_byte(); // character_marriage_ring

			string guild_name = recv.read_string();
			string alliance_name = recv.read_string();

			recv.skip_byte();

			sbyte pet_unique_id = recv.read_byte();

			while (pet_unique_id != 0)
			{
				recv.skip_int(); // pet_id
				recv.skip_string(); // pet_name
				recv.skip_byte(); // pet_level
				recv.skip_short(); // pet_closeness
				recv.skip_byte(); // pet_fullness

				recv.skip_short();

				recv.skip_int(); // pet_inventory_id

				pet_unique_id = recv.read_byte();
			}

			sbyte mount = recv.read_byte();

			if (mount != 0)
			{
				recv.skip_int(); // mount_level
				recv.skip_int(); // mount_exp
				recv.skip_int(); // mount_tiredness
			}

			sbyte wishlist_size = recv.read_byte();

			for (sbyte sn = 0; sn < wishlist_size; sn++)
			{
				recv.skip_int(); // wishlist_item
			}

			recv.skip_int(); // monster_book_level
			recv.skip_int(); // monster_book_card_normal
			recv.skip_int(); // monster_book_card_special
			recv.skip_int(); // monster_book_cards_total
			recv.skip_int(); // monster_book_cover

			recv.skip_int(); // medal

			short medal_quests_size = recv.read_short();

			for (short s = 0; s < medal_quests_size; s++)
			{
				recv.skip_short(); // medal_quest
			}

			var charinfo = UI.get().get_element<UICharInfo>();
			if (charinfo)
			{
				charinfo.get ().update_stats(character_id, character_job_id, character_level, character_fame, guild_name, alliance_name);
			}
		}
	}
}


