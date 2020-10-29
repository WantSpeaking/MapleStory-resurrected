#define USE_NX

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using Un4seen.Bass;

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


#if USE_NX
#else
#endif

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
			LENGTH
		}

		public Sound (Name name)
		{
			//id = soundids[name].ToString ();
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
					id = itemids["02000000"].ToString ();
				}
			}
		}

		public Sound (WzObject src)
		{
//ORIGINAL LINE: id = add_sound(src);
			id = add_sound (src);
		}

		public Sound ()
		{
			id = string.Empty;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void play() const
		public void play ()
		{
			/*if (id.Length > 0)
			{
				play (id);
			}*/
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
			Bass.BASS_Free ();
		}

		public static bool set_sfxvolume (byte vol)
		{
			return Bass.BASS_SetConfig (BASSConfig.BASS_CONFIG_GVOL_SAMPLE, vol * 100);
		}

		private string id;

		private static void play (string id)
		{
			if (samples.All (pair => pair.Key != id))
			{
				return;
			}

			var channel = Bass.BASS_SampleGetChannel (samples[id], false);
			Bass.BASS_ChannelPlay (channel, true);
		}

		private static string add_sound (WzObject src)
		{
			var ad = src;

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
			//var data = reinterpret_cast<object>(new ad.data());
			var data = ad.GetBytes ();

			if (data != null)
			{
				var id = ad.FullPath;

				if (samples.ContainsKey (ad.FullPath))
				{
					return string.Empty;
				}

				samples[id] = Bass.BASS_SampleLoad (data, 82, data.Length, 4, BASSFlag.BASS_SAMPLE_OVER_POS);

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

	public class Music
	{
		public Music (string p)
		{
			path = p;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This was formerly a static local variable declaration (not allowed in C#):
		private int play_stream = 0;

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This was formerly a static local variable declaration (not allowed in C#):
		private string play_bgmpath = "";

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void play() const
		public void play ()
		{
/*//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
//			static HSTREAM stream = 0;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
//			static string bgmpath = "";

			if (path == play_bgmpath)
			{
				return;
			}

			var ad = nl.nx.wzFile_sound.resolve (path);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
			var data = reinterpret_cast<object> (new ad.data ());

			if (data)
			{
				if (play_stream)
				{
					Bass.BASS_ChannelStop (play_stream);
					Bass.BASS_StreamFree (play_stream);
				}

				play_stream = Bass.BASS_StreamCreateFile (true, data, 82, ad.length (), BASS_SAMPLE_FLOAT | BASS_SAMPLE_LOOP);
				Bass.BASS_ChannelPlay (play_stream, true);

				play_bgmpath = path;
			}*/
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This was formerly a static local variable declaration (not allowed in C#):
		private int play_once_stream = 0;

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This was formerly a static local variable declaration (not allowed in C#):
		private string play_once_bgmpath = "";

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void play_once() const
		public void play_once ()
		{
/*//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
//			static HSTREAM stream = 0;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
//			static string bgmpath = "";

			if (path == play_once_bgmpath)
			{
				return;
			}

			var ad = nl.nx.wzFile_sound.resolve (path) as WzBinaryProperty;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
			var data = ad.GetBytes ();

			if (data != null)
			{
				if (play_once_stream != 0)
				{
					Bass.BASS_ChannelStop (play_once_stream);
					Bass.BASS_StreamFree (play_once_stream);
				}

				File.WriteAllBytes("D:\\test.dat", data);

				GCHandle pinnedObject = GCHandle.Alloc(data, GCHandleType.Pinned);
				IntPtr pinnedObjectPtr = pinnedObject.AddrOfPinnedObject();

				
				play_once_stream = Bass.BASS_StreamCreateFile("D:\\test.dat", 82, 0, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN);
				Bass.BASS_ChannelPlay (play_once_stream, true);
				

				play_once_bgmpath = path;
			}*/
		}

		public static Error init ()
		{
			byte volume = Setting<BGMVolume>.get ().load ();

			if (!set_bgmvolume (volume))
			{
				return Error.Code.AUDIO;
			}

			return Error.Code.NONE;
		}

		public static bool set_bgmvolume (byte vol)
		{
			return Bass.BASS_SetConfig (BASSConfig.BASS_CONFIG_GVOL_STREAM , vol * 100);
		}

		private string path;
	}
}


#if USE_NX
#endif