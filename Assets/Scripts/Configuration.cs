#define DEBUG

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

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
	// Manages the 'Settings' file which contains configurations set by user behavior
	public class Configuration : Singleton<Configuration>
	{
		// Add the settings which will be used and load them
		public Configuration ()
		{
			settings.emplace<ServerIP> ();
			settings.emplace<ServerPort> ();
			settings.emplace<Fullscreen> ();
			settings.emplace<Width> ();
			settings.emplace<Height> ();
			settings.emplace<VSync> ();
			settings.emplace<FontPathNormal> ();
			settings.emplace<FontPathBold> ();
			settings.emplace<BGMVolume> ();
			settings.emplace<SFXVolume> ();
			settings.emplace<SaveLogin> ();
			settings.emplace<DefaultAccount> ();
			settings.emplace<DefaultWorld> ();
			settings.emplace<DefaultChannel> ();
			settings.emplace<DefaultRegion> ();
			settings.emplace<DefaultCharacter> ();
			settings.emplace<Chatopen> ();
			settings.emplace<PosSTATS> ();
			settings.emplace<PosEQINV> ();
			settings.emplace<PosINV> ();
			settings.emplace<PosSKILL> ();
			settings.emplace<PosQUEST> ();
			settings.emplace<PosMAP> ();
			settings.emplace<PosUSERLIST> ();
			settings.emplace<PosCHAT> ();
			settings.emplace<PosMINIMAP> ();
			settings.emplace<PosSHOP> ();
			settings.emplace<PosNOTICE> ();
			settings.emplace<PosMAPLECHAT> ();
			settings.emplace<PosCHANNEL> ();
			settings.emplace<PosJOYPAD> ();
			settings.emplace<PosEVENT> ();
			settings.emplace<PosKEYCONFIG> ();
			settings.emplace<PosOPTIONMENU> ();
			settings.emplace<PosCHARINFO> ();
			settings.emplace<MiniMapType> ();
			settings.emplace<MiniMapSimpleMode> ();
			settings.emplace<MiniMapDefaultHelpers> ();

			load ();
		}

		Dictionary<string, string> rawsettings = new Dictionary<string, string> ();

		// Save
		public new void Dispose ()
		{
#if ! DEBUG
			save();
#endif
			base.Dispose ();
		}

		// Load all settings
		// If something is missing, set the default value.
		// Can be used for reloading
		public void load ()
		{
			var file = File.OpenText ($"{Constants.get ().path_SettingFileFolder}{FILENAME}") /*new ifstream (FILENAME)*/;

			//if (file.is_open ())
			{
				// Go through the file line by line
				string line;
				//getline (file, line);
				while (!file.EndOfStream)
				{
					line = file.ReadLine ();
					if (line == null) continue;
					// If the setting is not empty, load the value.
					var split = line.IndexOf ('=');

					if (split != -1 && split + 2 < line.Length)
					{
						//Debug.Log ($"{line}");
						rawsettings.Add (line.Substring (0, split - 1), line.Substring (split + 2));
					}
				}
			}

			// Replace default values with loaded values
			foreach (var setting in settings)
			{
				if (rawsettings.TryGetValue (setting.Value.name, out var rawSetting))
				{
					var old_Entry = settings[setting.Key];
					old_Entry.value = rawSetting;
					settings[setting.Key] = old_Entry;
				}
			}
		}

		// Save the current settings 
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void save() const
		public void save ()
		{
			// Open the settings file
			using (var config = File.Open (FILENAME, FileMode.OpenOrCreate))
			{
				using (var writer = new StreamWriter (config))
				{
					if (!config.CanWrite) return;
					// Save settings line by line
					foreach (var setting in settings)
					{
						writer.WriteLine (setting.Value.ToString ());
					}
				}
			}
		}

		// Get private member SHOW_FPS
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool get_show_fps() const
		public bool get_show_fps ()
		{
			return SHOW_FPS;
		}

		// Get private member SHOW_PACKETS
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool get_show_packets() const
		public bool get_show_packets ()
		{
			return SHOW_PACKETS;
		}

		// Get private member var_LOGIN
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool get_var_login() const
		public bool get_var_login ()
		{
			return var_LOGIN;
		}

		// Get the world to login with
		public byte get_var_world ()
		{
			return var_world;
		}

		// Get the channel to login with
		public byte get_var_channel ()
		{
			return var_channel;
		}

		// Get the account to login with
		public string get_var_acc ()
		{
			return var_acc;
		}

		// Get the password to login with
		public string get_var_pass ()
		{
			return var_pass;
		}

		// Get the pic to login with
		public string get_var_pic ()
		{
			return var_pic;
		}

		// Get the character id to login with
		public int get_var_cid ()
		{
			return var_cid;
		}

		// Get private member TITLE
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string get_title() const
		public string get_title ()
		{
			return TITLE;
		}

		// Get private member VERSION
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string get_version() const
		public string get_version ()
		{
			return VERSION;
		}

		// Get private member LoginMusic
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string get_login_music() const
		public string get_login_music ()
		{
			return LoginMusic;
		}

		// Get private member LoginMusicSEA
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string get_login_music_sea() const
		public string get_login_music_sea ()
		{
			return LoginMusicSEA;
		}

		// Get private member LoginMusicNewtro
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string get_login_music_newtro() const
		public string get_login_music_newtro ()
		{
			return LoginMusicNewtro;
		}

		// Get private member JOINLINK
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string get_joinlink() const
		public string get_joinlink ()
		{
			return JOINLINK;
		}

		// Get private member WEBSITE
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string get_website() const
		public string get_website ()
		{
			return WEBSITE;
		}

		// Get private member FINDID
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string get_findid() const
		public string get_findid ()
		{
			return FINDID;
		}

		// Get private member FINDPASS
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string get_findpass() const
		public string get_findpass ()
		{
			return FINDPASS;
		}

		// Get private member RESETPIC
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string get_resetpic() const
		public string get_resetpic ()
		{
			return RESETPIC;
		}

		// Get private member CHARGENX
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string get_chargenx() const
		public string get_chargenx ()
		{
			return CHARGENX;
		}

		// Set private member MACS
		public void set_macs (string macs)
		{
			MACS = macs;
		}

		// Set private member HWID
		public void set_hwid (string hwid, ref string volumeSerialNumber)
		{
			VolumeSerialNumber = hwid;

			StringBuilder newHWID = new StringBuilder ();

			newHWID.Append (hwid);
			newHWID.Append ("_");

			string part1 = VolumeSerialNumber.Substring (0, 2);
			string part2 = VolumeSerialNumber.Substring (2, 2);
			string part3 = VolumeSerialNumber.Substring (4, 2);
			string part4 = VolumeSerialNumber.Substring (6, 2);

			newHWID.Append (part4);
			newHWID.Append (part3);
			newHWID.Append (part2);
			newHWID.Append (part1);

			HWID = newHWID.ToString ();
		}

		// Set private member MAXWIDTH
		public void set_max_width (short max_width)
		{
			MAXWIDTH = max_width;
		}

		// Set private member MAXHEIGHT
		public void set_max_height (short max_height)
		{
			MAXHEIGHT = max_height;
		}

		// Get private member MACS
		public string get_macs ()
		{
			return MACS;
		}

		// Get private member HWID
		public string get_hwid ()
		{
			return HWID;
		}

		// Get the Hard Drive Volume Serial Number
		public string get_vol_serial_num ()
		{
			return VolumeSerialNumber;
		}

		// Get the max width allowed
		public short get_max_width ()
		{
			return MAXWIDTH;
		}

		// Get the max height allowed
		public short get_max_height ()
		{
			return MAXHEIGHT;
		}

		// Get the shop's "Right-click to sell item" boolean
		public bool get_rightclicksell ()
		{
			return rightclicksell;
		}

		// Set the shop's "Right-click to sell item" boolean
		public void set_rightclicksell (bool value)
		{
			rightclicksell = value;
		}

		// Whether to show the weekly maple star in Maple Chat
		public bool get_show_weekly ()
		{
			return show_weekly;
		}

		// Set whether to show the weekly maple star in Maple Chat
		public void set_show_weekly (bool value)
		{
			show_weekly = value;
		}

		// Whether to show the start screen
		public bool get_start_shown ()
		{
			return start_shown;
		}

		// Set whether to show the start screen
		public void set_start_shown (bool value)
		{
			start_shown = value;
		}

		// Get the character's selected world
		public byte get_worldid ()
		{
			return worldid;
		}

		// Set the character's selected world
		public void set_worldid (byte id)
		{
			worldid = id;
		}

		// Get the character's selected channel
		public byte get_channelid ()
		{
			return channelid;
		}

		// Set the character's selected channel
		public void set_channelid (byte id)
		{
			channelid = id;
		}

		// Check if the current account is an admin account
		public bool get_admin ()
		{
			return admin;
		}

		// Check whether the current account is an admin account
		public void set_admin (bool value)
		{
			admin = value;
		}

		// Base class for an entry in the settings file
		public class Entry
		{
			public Entry ()
			{
			}

			protected Entry (string n, string v)
			{
				this.name = n;
				this.value = v;
			}

			public string name;
			public string value;

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C# has no concept of a 'friend' class:
//			friend class Configuration;

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string to_string() const
			private string to_string ()
			{
				return name + " = " + value;
			}
		}

		// Setting which converts to a boolean
		public class BoolEntry : Entry
		{
			protected BoolEntry (string n, string v) : base (n, v)
			{
			}

			public void save (bool b)
			{
				value = b ? "true" : "false";
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool load() const
			public bool load ()
			{
				return value == "true";
			}
		}

		// Setting which uses the raw string
		public class StringEntry : Entry
		{
			protected StringEntry (string n, string v) : base (n, v)
			{
			}

			public void save (string str)
			{
				value = str;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string load() const
			public string load ()
			{
				return value;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no equivalent in C# to C++ 'using' declarations which operate on base class members:
		}

		// Setting which converts to a Point<int16_t>
		public class PointEntry : Entry
		{
			protected PointEntry (string n, string v) : base (n, v)
			{
			}

			public void save (Point<short> vec)
			{
				value = vec.to_string ();
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> load() const
			public Point<short> load ()
			{
				string xstr = value.Substring (1, value.IndexOf (",") - 1);
				string ystr = value.Substring (value.IndexOf (",") + 1, value.IndexOf (")") - value.IndexOf (",") - 1);

				var x = string_conversion.or_zero<short> (xstr);
				var y = string_conversion.or_zero<short> (ystr);

				return new Point<short> (x, y);
			}
		}

		// Setting which converts to an integer type
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
//ORIGINAL LINE: template <class T>
		public class IntegerEntry<T> : Entry
		{
			protected IntegerEntry (string n, string v) : base (n, v)
			{
			}

			public void save (T num)
			{
				value = Convert.ToString (num);
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: T load() const
			public T load ()
			{
				return string_conversion.or_zero<T> (value);
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no equivalent in C# to C++ 'using' declarations which operate on base class members:
		}

		// Setting which converts to a byte
		public class ByteEntry : IntegerEntry<byte>
		{
			protected ByteEntry (string n, string v) : base (n, v)
			{
			}

			//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no equivalent in C# to C++ 'using' declarations which operate on base class members:
		}

		// Setting which converts to a short
		public class ShortEntry : IntegerEntry<ushort>
		{
			protected ShortEntry (string n, string v) : base (n, v)
			{
			}

			//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no equivalent in C# to C++ 'using' declarations which operate on base class members:
		}

		// Setting which converts to an int
		public class IntEntry : IntegerEntry<uint>
		{
			protected IntEntry (string n, string v) : base (n, v)
			{
			}

			//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no equivalent in C# to C++ 'using' declarations which operate on base class members:
		}

		// Setting which converts to a long
		public class LongEntry : IntegerEntry<ulong>
		{
			protected LongEntry (string n, string v) : base (n, v)
			{
			}

			//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no equivalent in C# to C++ 'using' declarations which operate on base class members:
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
//ORIGINAL LINE: template <typename T>
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C# has no concept of a 'friend' class:
//		friend struct Setting;

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no equivalent in C# to templates on variables:
		private string FILENAME = "Settings";
		private string TITLE = "MapleStory";
		private string VERSION = "213.2";
		private string LoginMusic = "BgmUI.img/Title";
		private string LoginMusicSEA = "BgmGL.img/OldMaple";
		private string LoginMusicNewtro = "BgmEvent2.img/Newtro_Login";
		private string JOINLINK = "https://www.nexon.com/account/en/create";
		private string WEBSITE = "http://maplestory.nexon.net/";
		private string FINDID = "https://www.nexon.com/account/en/login";
		private string FINDPASS = "https://www.nexon.com/account/en/reset-password";
		private string RESETPIC = "https://www.nexon.com/account/en/login";
		private string CHARGENX = "https://billing.nexon.net/PurchaseNX";
		private const bool SHOW_FPS = false;
		private const bool SHOW_PACKETS = true;
		private const bool var_LOGIN = false;
		private const byte var_world = 0;
		private const byte var_channel = 0;
		private const string var_acc = "";
		private const string var_pass = "";
		private const string var_pic = "";
		private const int var_cid = 0;
		private bool rightclicksell = false;
		private bool show_weekly = true;
		private bool start_shown = false;
		private string MACS = "00-FF-27-AC-9C-D6";
		private string HWID = "2EFDB98799DD_CB4F4F88";
		private short MAXWIDTH;
		private short MAXHEIGHT;
		private string VolumeSerialNumber = "2EFDB98799DD_CB4F4F88";
		private byte worldid;
		private byte channelid;
		private bool admin;
		public TypeMap<Entry> settings = new TypeMap<Entry> ();
	}

	// IP Address which the client will connect to
	public class ServerIP : Configuration.StringEntry
	{
		public ServerIP () : base ("ServerIP", "127.0.0.1")
		{
		}
	}

	// Port which the client will connect to
	public class ServerPort : Configuration.StringEntry
	{
		public ServerPort () : base ("ServerPort", "8484")
		{
		}
	}

	// Whether to start in full screen mode
	public class Fullscreen : Configuration.BoolEntry
	{
		public Fullscreen () : base ("Fullscreen", "false")
		{
		}
	}

	// The width of the screen
	public class Width : Configuration.ShortEntry
	{
		public Width () : base ("Width", "800")
		{
		}
	}

	// The height of the screen
	public class Height : Configuration.ShortEntry
	{
		public Height () : base ("Height", "600")
		{
		}
	}

	// Whether to use VSync
	public class VSync : Configuration.BoolEntry
	{
		public VSync () : base ("VSync", "true")
		{
		}
	}

	// The normal font which will be used
	public class FontPathNormal : Configuration.StringEntry
	{
		public FontPathNormal () : base ("FontPathNormal", "fonts/Arial/Arial.ttf")
		{
		}
	}

	// The bold font which will be used
	public class FontPathBold : Configuration.StringEntry
	{
		public FontPathBold () : base ("FontPathBold", "fonts/Arial/Arial-Bold.ttf")
		{
		}
	}

	// Music Volume
	// Number from 0 to 100
	public class BGMVolume : Configuration.ByteEntry
	{
		public BGMVolume () : base ("BGMVolume", "50")
		{
		}
	}

	// Sound Volume
	// Number from 0 to 100
	public class SFXVolume : Configuration.ByteEntry
	{
		public SFXVolume () : base ("SFXVolume", "50")
		{
		}
	}

	// Whether to save the last used account name
	public class SaveLogin : Configuration.BoolEntry
	{
		public SaveLogin () : base ("SaveLogin", "false")
		{
		}
	}

	// The last used account name
	public class DefaultAccount : Configuration.StringEntry
	{
		public DefaultAccount () : base ("Account", "")
		{
		}
	}

	// The last used world
	public class DefaultWorld : Configuration.ByteEntry
	{
		public DefaultWorld () : base ("World", "0")
		{
		}
	}

	// The last used channel
	public class DefaultChannel : Configuration.ByteEntry
	{
		public DefaultChannel () : base ("Channel", "0")
		{
		}
	}

	// The last used region
	public class DefaultRegion : Configuration.ByteEntry
	{
		public DefaultRegion () : base ("Region", "5")
		{
		}
	}

	// The last used character
	public class DefaultCharacter : Configuration.ByteEntry
	{
		public DefaultCharacter () : base ("Character", "0")
		{
		}
	}

	// Whether to show UIChatBar
	public class Chatopen : Configuration.BoolEntry
	{
		public Chatopen () : base ("Chatopen", "false")
		{
		}
	}

	// The default position of UIStatsInfo
	public class PosSTATS : Configuration.PointEntry
	{
		public PosSTATS () : base ("PosSTATS", "(72,72)")
		{
		}
	}

	// The default position of UIEquipInventory
	public class PosEQINV : Configuration.PointEntry
	{
		public PosEQINV () : base ("PosEQINV", "(250,160)")
		{
		}
	}

	// The default position of UIItemInventory
	public class PosINV : Configuration.PointEntry
	{
		public PosINV () : base ("PosINV", "(300,160)")
		{
		}
	}

	// The default position of UISkillBook
	public class PosSKILL : Configuration.PointEntry
	{
		public PosSKILL () : base ("PosSKILL", "(96,96)")
		{
		}
	}

	// The default position of UIQuestLog
	public class PosQUEST : Configuration.PointEntry
	{
		public PosQUEST () : base ("PosQUEST", "(300,160)")
		{
		}
	}

	// The default position of UIWorldMap
	public class PosMAP : Configuration.PointEntry
	{
		public PosMAP () : base ("PosMAP", "(100,35)")
		{
		}
	}

	// The default position of UIUserList
	public class PosUSERLIST : Configuration.PointEntry
	{
		public PosUSERLIST () : base ("PosUSERLIST", "(104, 104)")
		{
		}
	}

	// The default position of UIChatBar
	public class PosCHAT : Configuration.PointEntry
	{
		public PosCHAT () : base ("PosCHAT", "(0, 572)")
		{
		}
	}

	// The default position of UIMiniMap
	public class PosMINIMAP : Configuration.PointEntry
	{
		public PosMINIMAP () : base ("PosMINIMAP", "(0, 0)")
		{
		}
	}

	// The default position of UIShop
	public class PosSHOP : Configuration.PointEntry
	{
		public PosSHOP () : base ("PosSHOP", "(146, 48)")
		{
		}
	}

	// The default position of UINotice
	public class PosNOTICE : Configuration.PointEntry
	{
		public PosNOTICE () : base ("PosNOTICE", "(400, 285)")
		{
		}
	}

	// The default position of UIChat and UIRank
	public class PosMAPLECHAT : Configuration.PointEntry
	{
		public PosMAPLECHAT () : base ("PosMAPLECHAT", "(50, 46)")
		{
		}
	}

	// The default position of UIChannel
	public class PosCHANNEL : Configuration.PointEntry
	{
		public PosCHANNEL () : base ("PosCHANNEL", "(215, 100)")
		{
		}
	}

	// The default position of UIJoypad
	public class PosJOYPAD : Configuration.PointEntry
	{
		public PosJOYPAD () : base ("PosJOYPAD", "(312, 134)")
		{
		}
	}

	// The default position of UIEvent
	public class PosEVENT : Configuration.PointEntry
	{
		public PosEVENT () : base ("PosEVENT", "(99, 100)")
		{
		}
	}

	// The default position of UIKeyConfig
	public class PosKEYCONFIG : Configuration.PointEntry
	{
		public PosKEYCONFIG () : base ("PosKEYCONFIG", "(65, 50)")
		{
		}
	}

	// The default position of UIOptionMenu
	public class PosOPTIONMENU : Configuration.PointEntry
	{
		public PosOPTIONMENU () : base ("PosUSERLIST", "(170, -1)")
		{
		}
	}

	// The default position of UICharInfo
	public class PosCHARINFO : Configuration.PointEntry
	{
		public PosCHARINFO () : base ("PosCHARINFO", "(264, 264)")
		{
		}
	}

	// The default type of UIMiniMap
	public class MiniMapType : Configuration.ByteEntry
	{
		public MiniMapType () : base ("MiniMapType", "0")
		{
		}
	}

	// Whether to use a simple version of UIMiniMap
	public class MiniMapSimpleMode : Configuration.BoolEntry
	{
		public MiniMapSimpleMode () : base ("MiniMapSimpleMode", "false")
		{
		}
	}

	// Whether to use default helpers for UIMiniMap
	public class MiniMapDefaultHelpers : Configuration.BoolEntry
	{
		public MiniMapDefaultHelpers () : base ("MiniMapDefaultHelpers", "false")
		{
		}
	}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
//ORIGINAL LINE: template <typename T>
	// Can be used to access settings
	public class Setting<T> where T : Configuration.Entry, new ()
	{
		// Access a setting
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This was formerly a static local variable declaration (not allowed in C#):
		private static T get_defaultentry = new T ();

		public static T get ()
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no equivalent in C# to 'static_assert':
//			static_assert(is_base_of<Configuration::Entry, T>::value, "template parameter T for Setting must inherit from Configuration::Entry.");

			var entry = Configuration.get ().settings.get<T> ();

			if (entry != null)
			{
				return (T)entry;
			}
			else
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
//				static T defaultentry;
				return get_defaultentry;
			}
		}
	}
}