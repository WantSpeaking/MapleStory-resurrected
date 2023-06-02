


namespace ms
{
	// Handler for a packet which contains all character information on first login or warps the player to a different map
	public class SetFieldHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
            //Constants.get ().set_viewwidth (Setting<Width>.get ().load ());
            //Constants.get ().set_viewheight (Setting<Height>.get ().load ());
            GameUtil.Instance.stopwatch.Restart();
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
            GameUtil.Instance.stopwatch.Stop();
            AppDebug.Log($"SetFieldHandler time:{GameUtil.Instance.stopwatch.ElapsedMilliseconds}");
        }

		public void transition (int mapid, byte portalid)
		{
			float fadestep = 0.025f;

			Window.get().fadeout(fadestep, () =>
			{
					
			});

			//GraphicsGL.get().@lock();
			Stage.get().clear();
			//还在GameUI ui不用clear dispose
			TestURPBatcher.Instance.Clear();
            TestURPBatcher.Instance.FillParentDict();
            TestURPBatcher.Instance.CLearDestoryObjs();
            Timer.get().start();
			UI.get().disable ();
			UI.get().disable_draw ();




			//GraphicsGL.get().clear();
			UnityEngine.SceneManagement.SceneManager.LoadScene("Game", UnityEngine.SceneManagement.LoadSceneMode.Single);

            Stage.get().load(mapid, (sbyte)portalid);

            UI.get().enable();
            UI.get().enable_draw();
            Timer.get().start();
            //GraphicsGL.get().unlock();

            Stage.get().transfer_player();
        }

		private void change_map (InPacket recv, int map_id)
		{
			recv.skip (3);

			int mapid = recv.read_int ();
			sbyte portalid = recv.read_byte ();

			transition (mapid, (byte)portalid);
		}

		private void set_field (InPacket recv)
		{
			recv.skip (23);

			int cid = recv.read_int ();
			var charselect = UI.get ().get_element<UICharSelect> ();

			if (charselect == false)
			{
				return;
			}

			CharEntry playerentry = charselect.get ().get_character (cid);
			//CharEntry playerentry = UICharSelect.get_character (cid);

			if (playerentry.id != cid)
			{
				return;
			}

			Stage.get ().loadplayer (playerentry);

			LoginParser.parse_stats (recv);

			Player player = Stage.get ().get_player ();
			//player.TrySet_CharShadowLook ();
			
			recv.read_byte (); // 'buddycap'

			if (recv.read_bool ())
			{
				recv.read_string (); // 'linkedname'
			}

			CharacterParser.parse_inventory (recv, player.get_inventory ());
			CharacterParser.parse_skillbook (recv, player.get_skills ());
			CharacterParser.parse_cooldowns (recv, player);
			CharacterParser.parse_questlog (recv, player.get_questlog ());
			CharacterParser.parse_minigame (recv);
			CharacterParser.parse_ring1 (recv);
			CharacterParser.parse_ring2 (recv);
			CharacterParser.parse_ring3 (recv);
			CharacterParser.parse_teleportrock (recv, player.get_teleportrock ());
			CharacterParser.parse_monsterbook (recv, player.get_monsterbook ());
			CharacterParser.parse_nyinfo (recv);
			CharacterParser.parse_areainfo (recv);

			player.recalc_stats (true);

			new PlayerUpdatePacket ().dispatch ();

			byte portalid = player.get_stats ().get_portal ();
			int mapid = player.get_stats ().get_mapid ();

			transition (mapid, portalid);

			new Sound (Sound.Name.GAMESTART).play ();

			UI.get ().change_state (UI.State.GAME);
		}
	}
}