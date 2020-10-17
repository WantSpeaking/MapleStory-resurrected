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
	// Handler for a packet which contains all character information on first login or warps the player to a different map
	public class SetFieldHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle (InPacket recv)
		{
			//Constants.get ().set_viewwidth (Setting<Width>.get ().load ());
			//Constants.get ().set_viewheight (Setting<Height>.get ().load ());

			int channel = recv.read_int ();
			sbyte mode1 = recv.read_byte ();
			sbyte mode2 = recv.read_byte ();

			if (mode1 == 0 && mode2 == 0)
			{
				change_map (recv, channel);
			}
			else
			{
				set_field (recv);
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void transition(int mapid, byte portalid) const
		public void transition (int mapid, byte portalid)
		{
			/*float fadestep = 0.025f;

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Only lambda expressions having all locals passed by reference can be converted to C#:
//ORIGINAL LINE: Window::get().fadeout(fadestep, [mapid, portalid]()
			Window.get().fadeout(fadestep, () =>
			{
					//GraphicsGL.get().clear();

					Stage.get().load(mapid, (sbyte)portalid);

					UI.get().enable();
					Timer.get().start();
					//GraphicsGL.get().unlock();

					Stage.get().transfer_player();
			});

			GraphicsGL.get().@lock();
			Stage.get().clear();
			Timer.get().start();*/

			float fadestep = 0.025f;


		

			//GraphicsGL.get ().@lock ();
			Stage.get ().clear ();
			Timer.get ().start ();
			
			//Window.get ().fadeout (fadestep, () =>
			//{
			//GraphicsGL.get().clear();

			Stage.get ().load (mapid, (sbyte)portalid);

			UI.get ().enable ();
			Timer.get ().start ();
			//GraphicsGL.get().unlock();

			Stage.get ().transfer_player ();
			//});
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void change_map(InPacket& recv, int map_id) const
		private void change_map (InPacket recv, int map_id)
		{
			/*recv.skip (3);

			int mapid = recv.read_int ();
			sbyte portalid = recv.read_byte ();

			transition (mapid, portalid);*/
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void set_field(InPacket& recv) const
		private void set_field (InPacket recv)
		{
			/*recv.skip (23);

			int cid = recv.read_int ();
			var charselect = UI.get ().get_element<UICharSelect> ();

			if (charselect == null)
			{
				return;
			}

			CharEntry playerentry = charselect.Dereference ().get_character (cid);

			if (playerentry.id != cid)
			{
				return;
			}

			Stage.get ().loadplayer (playerentry);

			LoginParser.parse_stats (recv);

			Player player = Stage.get ().get_player ();

			recv.read_byte (); // 'buddycap'

			if (recv.read_bool ())
			{
				recv.read_string (); // 'linkedname'
			}

			CharacterParser.parse_inventory (recv, player.get_inventory ());
			CharacterParser.parse_skillbook (recv, player.get_skills ());
			CharacterParser.parse_cooldowns (recv, player);
			CharacterParser.parse_questlog (recv, player.get_quests ());
			CharacterParser.parse_minigame (recv);
			CharacterParser.parse_ring1 (recv);
			CharacterParser.parse_ring2 (recv);
			CharacterParser.parse_ring3 (recv);
			CharacterParser.parse_teleportrock (recv, player.get_teleportrock ());
			CharacterParser.parse_monsterbook (recv, player.get_monsterbook ());
			CharacterParser.parse_nyinfo (recv);
			CharacterParser.parse_areainfo (recv);

			player.recalc_stats (true);

			PlayerUpdatePacket ().dispatch ();

			byte portalid = player.get_stats ().get_portal ();
			int mapid = player.get_stats ().get_mapid ();

			transition (mapid, portalid);

			Sound (Sound.Name.GAMESTART).play ();

			UI.get ().change_state (UI.State.GAME);*/
		}
	}
}