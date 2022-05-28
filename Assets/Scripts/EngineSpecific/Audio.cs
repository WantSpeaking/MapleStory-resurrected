/*using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using Microsoft.Xna.Framework.Audio;

namespace ms
{
	public class Sound
	{
		// Preloaded sounds
		public enum Name
		{
			/// UI
			BUTTONCLICK,
			BUTTONOVER,
			CHARSELECT,
			DLGNOTICE,
			MENUDOWN,
			MENUUP,
			RACESELECT,
			SCROLLUP,
			SELECTMAP,
			TAB,
			WORLDSELECT,
			DRAGSTART,
			DRAGEND,
			WORLDMAPOPEN,
			WORLDMAPCLOSE,

			/// Login
			GAMESTART,

			/// Game
			JUMP,
			DROP,
			PICKUP,
			PORTAL,
			LEVELUP,
			TOMBSTONE,
		}

		public Sound (Name name)
		{
			id = soundids[name];
		}

		public Sound (int itemid)
		{
			var fitemid = format_id (itemid);

			if (itemids.ContainsKey (fitemid))
			{
				id = itemids[fitemid].ToString ();
			}
			else
			{
				var pid = (10000 * (itemid / 10000));
				var fpid = format_id (pid);

				if (itemids.ContainsKey (fpid))
				{
					id = itemids[fpid].ToString ();
				}
				else
				{
					id = itemids["02000000"].ToString (); //todo 2 02000000 might not exist
				}
			}
}

public Sound (WzObject src)
		{
			id = add_sound (src);
		}

		public Sound ()
		{
			id = string.Empty;
		}

		public void play ()
		{
			if (!string.IsNullOrEmpty(id))
			{
				play (id);
			}
		}

		public static Error init ()
		{
			if (!Bass.BASS_Init (-1, 44100, 0, IntPtr.Zero, Guid.Empty))
			{
				return Error.Code.AUDIO;
			}

			WzObject uisrc = nl.nx.wzFile_sound["UI.img"];


			add_sound (Sound.Name.BUTTONCLICK, uisrc["BtMouseClick"]);

			add_sound (Sound.Name.BUTTONOVER, uisrc["BtMouseOver"]);

			add_sound (Sound.Name.CHARSELECT, uisrc["CharSelect"]);

			add_sound (Sound.Name.DLGNOTICE, uisrc["DlgNotice"]);

			add_sound (Sound.Name.MENUDOWN, uisrc["MenuDown"]);

			add_sound (Sound.Name.MENUUP, uisrc["MenuUp"]);

			add_sound (Sound.Name.RACESELECT, uisrc["RaceSelect"]);

			add_sound (Sound.Name.SCROLLUP, uisrc["ScrollUp"]);

			add_sound (Sound.Name.SELECTMAP, uisrc["SelectMap"]);

			add_sound (Sound.Name.TAB, uisrc["Tab"]);

			add_sound (Sound.Name.WORLDSELECT, uisrc["WorldSelect"]);

			add_sound (Sound.Name.DRAGSTART, uisrc["DragStart"]);

			add_sound (Sound.Name.DRAGEND, uisrc["DragEnd"]);

			add_sound (Sound.Name.WORLDMAPOPEN, uisrc["WorldmapOpen"]);

			add_sound (Sound.Name.WORLDMAPCLOSE, uisrc["WorldmapClose"]);

			WzObject gamesrc = nl.nx.wzFile_sound["Game.img"];


			add_sound (Sound.Name.GAMESTART, gamesrc["GameIn"]);

			add_sound (Sound.Name.JUMP, gamesrc["Jump"]);

			add_sound (Sound.Name.DROP, gamesrc["DropItem"]);

			add_sound (Sound.Name.PICKUP, gamesrc["PickUpItem"]);

			add_sound (Sound.Name.PORTAL, gamesrc["Portal"]);

			add_sound (Sound.Name.LEVELUP, gamesrc["LevelUp"]);

			add_sound (Sound.Name.TOMBSTONE, gamesrc["Tombstone"]);

			WzObject itemsrc = nl.nx.wzFile_sound["Item.img"];

			foreach (var node in itemsrc)
			{
				add_sound (node.Name, node["Use"]);
			}

			byte volume = Setting<SFXVolume>.get ().load ();

			if (!set_sfxvolume (volume))
			{
				return Error.Code.AUDIO;
			}

			return Error.Code.NONE;
		}

		public static void close ()
		{
			foreach (var soundEffectInstance in samples) soundEffectInstance.Value.Dispose ();
		}

		public static bool set_sfxvolume (byte vol)
		{
			_sfxVolume = vol;
			return true;
		}

		private string id;

		private static void play (string id)
		{
			if (!samples.ContainsKey (id)) return;
			var sample = samples[id];
			_sfxVolume = Setting<SFXVolume>.get ().load ();
			set_sfxvolume (_sfxVolume);
			sample.Volume = _sfxVolume * 0.01f;
			sample.Play ();
		}

		private static string add_sound (WzObject src)
		{
			var data = src.GetWavData (out var info);
			if (!(data?.Length > 0)) return null;
			var id = src.FullPath;
			if (samples.ContainsKey (id)) return id;
			using var stream = new MemoryStream (data);
			var se = SoundEffect.FromStream (stream);
			samples[id] = se.CreateInstance ();
			return string.Empty;
		}

		private static void add_sound (Name name, WzObject src)
		{
			var id = add_sound (src);

			if (id.Length != 0)
			{
				soundids[name] = id;
			}
		}

		private static void add_sound (string itemid, WzObject src)
		{
			var id = add_sound (src);

			if (!string.IsNullOrEmpty (id))
			{
				itemids[itemid] = id;
			}
		}

		private static string format_id (int itemid)
		{
			string strid = Convert.ToString (itemid);
			strid = strid.insert (0, 8 - strid.Length, '0');

			return strid;
		}

		private static Dictionary<string, SoundEffectInstance> samples = new Dictionary<string, SoundEffectInstance> ();
		private static EnumMap<Name, string> soundids = new EnumMap<Name, string> ();
		private static Dictionary<string, string> itemids = new Dictionary<string, string> ();
		private static byte _sfxVolume;
	}

	public class Music : Singleton<Music>
	{
		private int play_stream = 0;

		private string play_bgmpath = "";
		private static byte _bgmVolume = 50;

		private SoundEffectInstance _player;
		private int _time;
		private bool _isPlaying;

		public void play (string path ,bool looping = true)
		{
			if (path == play_bgmpath)
			{
				return;
			}

			if (!path.Equals (play_bgmpath))
			{
				_bgmVolume = Setting<SFXVolume>.get ().load ();
				if (_bgmVolume == 0) return;
				var wzObj = nl.nx.wzFile_sound.resolve (path);
				var wzSound = (WzBinaryProperty)wzObj;
				if (wzSound == null)
				{
					AppDebug.Log ($"Music is null, path:{play_bgmpath}");
					return;
				}

				_time = wzSound.Time ();
				_player?.Dispose ();
				using var stream = new MemoryStream (wzSound.Wav ());
				var se = SoundEffect.FromStream (stream);
				stream.Dispose ();
				_player = se.CreateInstance ();
				play_bgmpath = path;
				_isPlaying = false;
				set_bgmvolume (_bgmVolume);
			}

			_player.Play ();
			_isPlaying = true;
			_player.IsLooped = looping;
		}

		public void play_once (string path)
		{
			play (path, false);
		}

		public static Error init ()
		{
			Music.get ().set_bgmvolume (Setting<BGMVolume>.get ().load ());

			return Error.Code.NONE;
		}

		public bool set_bgmvolume (byte volume)
		{
			if (volume != _bgmVolume)
			{
				Setting<BGMVolume>.get ().save (volume);
				_bgmVolume = volume;
			}

			if (_player != null)
			{
				_player.Volume = _bgmVolume * 0.01f;
				if (volume == 0)
					_player.Pause();
				else
					_player.Resume();
			}


			return true;
		}

		public void Pause ()
		{
			_player?.Pause ();;
		}
		public void Resume ()
		{
			_player?.Resume();
		}
	}
}*/




using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using Un4seen.Bass;

namespace ms
{
    public class Sound
    {
        // Preloaded sounds
        public enum Name
        {
            /// UI
            BUTTONCLICK,
            BUTTONOVER,
            CHARSELECT,
            DLGNOTICE,
            MENUDOWN,
            MENUUP,
            RACESELECT,
            SCROLLUP,
            SELECTMAP,
            TAB,
            WORLDSELECT,
            DRAGSTART,
            DRAGEND,
            WORLDMAPOPEN,
            WORLDMAPCLOSE,

            /// Login
            GAMESTART,

            /// Game
            JUMP,
            DROP,
            PICKUP,
            PORTAL,
            LEVELUP,
            TOMBSTONE,
        }

        public Sound ()
        {

        }

        public Sound (Name name)
        {
            id = soundids[name];
        }

        public Sound (int itemid)
        {
            var fitemid = format_id (itemid);

            if (itemids.ContainsKey (fitemid))
            {
                id = itemids[fitemid].ToString ();
            }
            else
            {
                var pid = (10000 * (itemid / 10000));
                var fpid = format_id (pid);

                if (itemids.ContainsKey (fpid))
                {
                    id = itemids[fpid].ToString ();
                }
                else
                {
                    id = itemids["02000000"].ToString ();//todo 2 02000000 might not exist
                }
            }
        }

        public Sound (WzObject src)
        {
            id = add_sound (src);
        }

        public void play ()
        {
            if (!string.IsNullOrEmpty (id))
                play (id);
        }

        public static Error init ()
        {
            if (!Bass.BASS_Init (-1, 44100, 0, IntPtr.Zero, Guid.Empty))
            {
                return Error.Code.AUDIO;
            }

            WzObject uisrc = ms.wz.wzFile_sound["UI.img"];


            add_sound (Sound.Name.BUTTONCLICK, uisrc["BtMouseClick"]);

            add_sound (Sound.Name.BUTTONOVER, uisrc["BtMouseOver"]);

            add_sound (Sound.Name.CHARSELECT, uisrc["CharSelect"]);

            add_sound (Sound.Name.DLGNOTICE, uisrc["DlgNotice"]);

            add_sound (Sound.Name.MENUDOWN, uisrc["MenuDown"]);

            add_sound (Sound.Name.MENUUP, uisrc["MenuUp"]);

            add_sound (Sound.Name.RACESELECT, uisrc["RaceSelect"]);

            add_sound (Sound.Name.SCROLLUP, uisrc["ScrollUp"]);

            add_sound (Sound.Name.SELECTMAP, uisrc["SelectMap"]);

            add_sound (Sound.Name.TAB, uisrc["Tab"]);

            add_sound (Sound.Name.WORLDSELECT, uisrc["WorldSelect"]);

            add_sound (Sound.Name.DRAGSTART, uisrc["DragStart"]);

            add_sound (Sound.Name.DRAGEND, uisrc["DragEnd"]);

            add_sound (Sound.Name.WORLDMAPOPEN, uisrc["WorldmapOpen"]);

            add_sound (Sound.Name.WORLDMAPCLOSE, uisrc["WorldmapClose"]);

            WzObject gamesrc = ms.wz.wzFile_sound["Game.img"];


            add_sound (Sound.Name.GAMESTART, gamesrc["GameIn"]);

            add_sound (Sound.Name.JUMP, gamesrc["Jump"]);

            add_sound (Sound.Name.DROP, gamesrc["DropItem"]);

            add_sound (Sound.Name.PICKUP, gamesrc["PickUpItem"]);

            add_sound (Sound.Name.PORTAL, gamesrc["Portal"]);

            add_sound (Sound.Name.LEVELUP, gamesrc["LevelUp"]);

            add_sound (Sound.Name.TOMBSTONE, gamesrc["Tombstone"]);

            WzObject itemsrc = ms.wz.wzFile_sound["Item.img"];

            foreach (var node in itemsrc)
            {
                add_sound (node.Name, node["Use"]);
            }

            byte volume = Setting<SFXVolume>.get ().load ();

            if (!set_sfxvolume (volume))
            {
                return Error.Code.AUDIO;
            }

            return Error.Code.NONE;
        }

        public static void close ()
        {
            Bass.BASS_Free ();
        }

        public static bool set_sfxvolume (byte vol)
        {
            return Bass.BASS_SetConfig (BASSConfig.BASS_CONFIG_GVOL_SAMPLE, vol * 100);
        }

        private string id;

        private static void play (string id)
        {
            if (!samples.ContainsKey (id))
            {
                return;
            }

            var channel = Bass.BASS_SampleGetChannel (samples[id], false);
            Bass.BASS_ChannelPlay (channel, true);
        }

        private static string add_sound (WzObject src)
        {
            var ad = src;

            var data = ad.GetBytes ();

            if (data != null)
            {
                var id = ad.FullPath;

                if (samples.ContainsKey (ad.FullPath))
                {
                    return string.Empty;
                }

                samples[id] = Bass.BASS_SampleLoad (data, 0, data.Length, 65535, BASSFlag.BASS_SAMPLE_OVER_POS);

                return id;
            }
            else
            {
                return string.Empty;
            }
        }

        private static void add_sound (Name name, WzObject src)
        {
            //ORIGINAL LINE: uint id = add_sound(src);
            var id = add_sound (src);

            if (id.Length != 0)
            {
                soundids[name] = id;
            }
        }

        private static void add_sound (string itemid, WzObject src)
        {
            //ORIGINAL LINE: uint id = add_sound(src);
            if (src == null)
            {
                AppDebug.Log ($"add_sound src is null, name:{itemid}");
            }
            var id = add_sound (src);

            if (id.Length != 0)
            {
                itemids[itemid] = id;
            }
        }

        private static string format_id (int itemid)
        {
            string strid = Convert.ToString (itemid);
            strid = strid.insert (0, 8 - strid.Length, '0');

            return strid;
        }

        private static Dictionary<string, int> samples = new Dictionary<string, int> ();
        private static EnumMap<Name, string> soundids = new EnumMap<Name, string> ();
        private static Dictionary<string, string> itemids = new Dictionary<string, string> ();
    }

    public class Music : Singleton<Music>
    {
        public Music ()
        {

        }

        /* public Music(string p)
         {
             path = p;
         }*/

        private int play_stream = 0;

        private string play_bgmpath = "";
        GCHandle _hGCFile;
        public void play (string path)
        {
 /*           if (path == play_bgmpath)
            {
                return;
            }

            var ad = ms.wz.wzFile_sound.GetObjectFromPath (Path.Combine ("Sound.wz", path).Replace ('\\', '/'));
            var data = ad?.GetBytes ();

            if (data != null)
            {
                if (play_stream != 0)
                {
                    Bass.BASS_ChannelStop (play_stream);
                    Bass.BASS_StreamFree (play_stream);
                    _hGCFile.Free ();
                }

                _hGCFile = GCHandle.Alloc (data, GCHandleType.Pinned);
                play_stream = Bass.BASS_StreamCreateFile (_hGCFile.AddrOfPinnedObject (), 0, data.Length, BASSFlag.BASS_SAMPLE_LOOP | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_SAMPLE_MONO);
                Bass.BASS_ChannelPlay (play_stream, true);

                play_bgmpath = path;
            }*/
        }

        private int play_once_stream = 0;

        private string play_once_bgmpath = "";

        public void play_once (string path)
        {
            //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
            //			static HSTREAM stream = 0;
            //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
            //			static string bgmpath = "";

            if (path == play_once_bgmpath)
            {
                return;
            }

            var ad = ms.wz.wzFile_sound.GetObjectFromPath (path);
            var data = ad?.GetBytes ();

            if (data != null)
            {
                if (play_once_stream != 0)
                {
                    Bass.BASS_ChannelStop (play_once_stream);
                    Bass.BASS_StreamFree (play_once_stream);
                }

                IntPtr pData = Marshal.UnsafeAddrOfPinnedArrayElement (data, 0);
                play_once_stream = Bass.BASS_StreamCreateFile (pData, 0, data.Length, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_SAMPLE_LOOP);
                Bass.BASS_ChannelPlay (play_once_stream, true);

                /*File.WriteAllBytes("D:\\test.dat", data);

                GCHandle pinnedObject = GCHandle.Alloc(data, GCHandleType.Pinned);
                IntPtr pinnedObjectPtr = pinnedObject.AddrOfPinnedObject();


                play_once_stream = Bass.BASS_StreamCreateFile("D:\\test.dat", 82, 0, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN);
                Bass.BASS_ChannelPlay(play_once_stream, true);*/


                play_once_bgmpath = path;
            }
        }

        public void Pause ()
        {

        }
        public void Resume ()
        {

        }

        public static Error init ()
        {
            byte volume = Setting<BGMVolume>.get ().load ();

            if (!get ().set_bgmvolume (volume))
            {
                return Error.Code.AUDIO;
            }

            return Error.Code.NONE;
        }

        public bool set_bgmvolume (byte vol)
        {
            return Bass.BASS_SetConfig (BASSConfig.BASS_CONFIG_GVOL_STREAM, vol * 100);
        }

        //private string path;
    }
}
