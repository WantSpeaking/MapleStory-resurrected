using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Gaskellgames.AudioController;
using MapleLib.WzLib;
using provider;
using UnityEngine;

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
			//SoundManager.instance.
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
                    AppDebug.LogWarning ($"Sound can't find itemid:{itemid} format_id:{fpid}");
                    //id = itemids["02000000"].ToString ();//todo 2 02000000 might not exist
                }
            }
        }

        public Sound (WzObject src)
        {
            id = add_sound (src);
        }
        public Sound(MapleData src)
        {
            id = add_sound(src);
        }
        public void play ()
        {
			if (!string.IsNullOrEmpty(id))
				play(id);
		}

		public static Error init()
		{
			var uisrc = ms.wz.wzProvider_sound["UI.img"];


			add_sound(Sound.Name.BUTTONCLICK, uisrc["BtMouseClick"]);

			add_sound(Sound.Name.BUTTONOVER, uisrc["BtMouseOver"]);

			add_sound(Sound.Name.CHARSELECT, uisrc["CharSelect"]);

			add_sound(Sound.Name.DLGNOTICE, uisrc["DlgNotice"]);

			add_sound(Sound.Name.MENUDOWN, uisrc["MenuDown"]);

			add_sound(Sound.Name.MENUUP, uisrc["MenuUp"]);

			add_sound(Sound.Name.RACESELECT, uisrc["RaceSelect"]);

			add_sound(Sound.Name.SCROLLUP, uisrc["ScrollUp"]);

			add_sound(Sound.Name.SELECTMAP, uisrc["SelectMap"]);

			add_sound(Sound.Name.TAB, uisrc["Tab"]);

			add_sound(Sound.Name.WORLDSELECT, uisrc["WorldSelect"]);

			add_sound(Sound.Name.DRAGSTART, uisrc["DragStart"]);

			add_sound(Sound.Name.DRAGEND, uisrc["DragEnd"]);

			add_sound(Sound.Name.WORLDMAPOPEN, uisrc["WorldmapOpen"]);

			add_sound(Sound.Name.WORLDMAPCLOSE, uisrc["WorldmapClose"]);

			var gamesrc = ms.wz.wzProvider_sound["Game.img"];


			add_sound(Sound.Name.GAMESTART, gamesrc["GameIn"]);

			add_sound(Sound.Name.JUMP, gamesrc["Jump"]);

			add_sound(Sound.Name.DROP, gamesrc["DropItem"]);

			add_sound(Sound.Name.PICKUP, gamesrc["PickUpItem"]);

			add_sound(Sound.Name.PORTAL, gamesrc["Portal"]);

			add_sound(Sound.Name.LEVELUP, gamesrc["LevelUp"]);

			add_sound(Sound.Name.TOMBSTONE, gamesrc["Tombstone"]);

			var itemsrc = ms.wz.wzProvider_sound["Item.img"];

			foreach (var node in itemsrc)
			{
				add_sound(node.Name, node["Use"]);
			}

			byte volume = Setting<SFXVolume>.get().load();

			if (!set_sfxvolume(volume))
			{
				return Error.Code.AUDIO;
			}

			return Error.Code.NONE;
		}

		public static void close ()
        {
            //Bass.BASS_Free ();
        }

        public static bool set_sfxvolume (byte vol)
        {
            return true;
        }

        private string id;

        private static void play (string id)
        {
			if (!samples.ContainsKey(id))
			{
				return;
			}
			SoundController.Instance.PlaySoundFX(samples[id]);

            /*if (!samples.ContainsKey (id))
            {
                return;
            }

            var channel = Bass.BASS_SampleGetChannel (samples[id], false);
            Bass.BASS_ChannelPlay (channel, true);*/
        }
        private static string add_sound(MapleData src)
        {
            string clipFullName = src;//sound\\skill_9001001\\use
            if (string.IsNullOrEmpty(clipFullName))
            {
                AppDebug.LogWarning("AudioclipFullName IS NULL");
                return string.Empty;
            }

            if (samples.ContainsKey(clipFullName))
            {
                return clipFullName;
            }



            /*clipFullName = clipFullName.ToLower();//sound_skill.img,2301004_Use
			clipFullName = clipFullName.Replace("_", "/"); //sound/skill.img,2301004/Use
			clipFullName = clipFullName.Replace(".img", ""); //sound/skill,2301004/Use*/

            /*var paths = clipFullName.Split('_');
			if (paths.Length != 2)
			{
				return string.Empty;
			}*/

            //clipFullName Sound\\Item\\04170000\\Use
            var abName = clipFullName.Substring(clipFullName.IndexOf("Sound") + 6).Replace("\\","_").ToLower();// Sound\\Item\\04170000
			var assetName = clipFullName.Substring(clipFullName.LastIndexOf("\\")+1);// Use

			var abPath = $"wzpng\\Sound\\{abName}";// wzpng\\Sound\\Item\\04170000\\use
			var clip = AssetBundleLoaderMgr.Instance.LoadAsset<AudioClip>(abPath, assetName); 

			if (clip != null)
			{
				samples[clipFullName] = clip;

				return clipFullName;
			}
			else
			{
				return string.Empty;
			}
		}
		private static string add_sound (WzObject src)
        {
            if (samples.ContainsKey(src.FullPath))
            {
                return src.FullPath;
            }

            var ad = src;

            var data = ad.GetBytes ();

			if (data != null)
			{
				var id = ad.FullPath;

				//samples[id] = Bass.BASS_SampleLoad (data, 0, data.Length, 65535, BASSFlag.BASS_SAMPLE_OVER_POS);

				return id;
			}
			else
			{
                return string.Empty;
            }
        }
		private static void add_sound(Name name, MapleData src)
		{
			//ORIGINAL LINE: uint id = add_sound(src);
			var id = add_sound(src);

			if (id.Length != 0)
			{
				soundids[name] = id;
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
		private static void add_sound(string itemid, MapleData src)
		{
			//ORIGINAL LINE: uint id = add_sound(src);
			if (src == null)
			{
				AppDebug.Log($"add_sound src is null, name:{itemid}");
			}
			var id = add_sound(src);

			if (id.Length != 0)
			{
				itemids[itemid] = id;
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

        private static Dictionary<string, AudioClip> samples = new Dictionary<string, AudioClip> ();
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
        private AudioClip play_clip;

		private string play_bgmpath = "";
        GCHandle _hGCFile;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path">BgmUI.img/Title</param>
		public void play (string path)//BgmUI.img/Title
		{
			if (path == play_bgmpath)
			{
				return;
			}

			if (string.IsNullOrEmpty(path))
			{
				return ;
			}

            path = path.Replace(".img", "");//BgmUI/Title
			var abName = path.Replace("/", "_").ToLower();// bgmui_title 

			var assetName = path.Substring(path.LastIndexOf("/") + 1);// Title

			var abPath = $"wzpng\\Sound\\{abName}";// wzpng\\Sound\\bgmui_title 
			var clip = AssetBundleLoaderMgr.Instance.LoadAsset<AudioClip>(abPath, assetName);
			

			if (clip != null)
			{
				if (play_clip != null)
				{
					play_clip = null;
					AssetBundleLoaderMgr.Instance.UnloadAssetBundle(play_bgmpath);
				}

				play_clip = clip;
				SoundController.Instance.PlayMusic(clip);

				play_bgmpath = path;
			}

			/*if (path == play_bgmpath)
			{
				return;
			}

			var ad = ms.wz.wzFile_sound.GetObjectFromPath(Path.Combine("Sound.wz", path).Replace('\\', '/'));
			var data = ad?.GetBytes();

			if (data != null)
			{
				if (play_stream != 0)
				{
					Bass.BASS_ChannelStop(play_stream);
					Bass.BASS_StreamFree(play_stream);
					_hGCFile.Free();
				}

				_hGCFile = GCHandle.Alloc(data, GCHandleType.Pinned);
				play_stream = Bass.BASS_StreamCreateFile(_hGCFile.AddrOfPinnedObject(), 0, data.Length, BASSFlag.BASS_SAMPLE_LOOP | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_SAMPLE_MONO);
				Bass.BASS_ChannelPlay(play_stream, true);

				play_bgmpath = path;
			}*/
		}

        private int play_once_stream = 0;

        private string play_once_bgmpath = "";

        public void play_once (string path)
        {
            /*if (path == play_once_bgmpath)
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
				
				*//*File.WriteAllBytes("D:\\test.dat", data);

                GCHandle pinnedObject = GCHandle.Alloc(data, GCHandleType.Pinned);
                IntPtr pinnedObjectPtr = pinnedObject.AddrOfPinnedObject();


                play_once_stream = Bass.BASS_StreamCreateFile("D:\\test.dat", 82, 0, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN);
                Bass.BASS_ChannelPlay(play_once_stream, true);*//*


				play_once_bgmpath = path;
            }*/
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
            return true;
        }
		public void Quit()
		{
			//Bass.BASS_Free ();
		}
        //private string path;
    }
}
