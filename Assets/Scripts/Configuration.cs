#define DEBUG

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;





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
			settings.emplace<DefaultPassword> ();
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
			settings.emplace<JSDebugOn> ();
			settings.emplace<PosBtnHeavyAttack> ();
			settings.emplace<PosBtnLightAttack> ();
			settings.emplace<PosBtnJump> ();
			settings.emplace<PosBtnPickUp> ();
			settings.emplace<PosBtnSkill1> ();
			settings.emplace<PosBtnSkill2> ();
			settings.emplace<PosBtnSkill3> ();
			settings.emplace<PosBtnSkill4> ();
			settings.emplace<PosBtnSkill5> ();
			settings.emplace<PosBtnSkill6> ();

            load ();
		}
		
		Dictionary<string, string> rawsettings = new Dictionary<string, string> ();

		// Save
		public override void Dispose ()
		{
			AppDebug.Log ("Dispose ");
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
            try
            {
				AppDebug.Log ($"start load config");
				var file = File.OpenText(Path.Combine(Constants.get().path_SettingFileFolder, FILENAME)) /*new ifstream (FILENAME)*/;

				//if (file.is_open ())
				{
					// Go through the file line by line
					string line;
					//getline (file, line);
					while (!file.EndOfStream)
					{
						line = file.ReadLine();
						if (string.IsNullOrEmpty(line)) continue;
						//AppDebug.Log ($"{line}");
						// If the setting is not empty, load the value.
						var split = line.IndexOf('=');

						if (split != -1 && split + 2 < line.Length)
						{
							//AppDebug.Log ($"key:{line.Substring (0, split - 1)}\t value:{line.Substring (split + 2)}");
							//AppDebug.Log ($"{line}");
							rawsettings.Add(line.Substring(0, split - 1).Trim(), line.Substring(split + 2).Trim());
							//AppDebug.Log ($"{line.Substring (0, split - 1).Trim ()}  {line.Substring (split + 2).Trim ()}");

						}
					}
				}
				file.Close ();
			}
			catch (Exception ex)
            {
				AppDebug.Log($"load config error:{ex.Message}");
            }

			
			
			// Replace default values with loaded values
			foreach (var pair in rawsettings)
			{
				if (settings.TryGetValue (pair.Key, out var entry))
				{
					entry.value = pair.Value;
				}
			}
			
			/*foreach (var setting in settings)
			{
				if (rawsettings.TryGetValue (setting.Value.name, out var rawSetting))
				{
					var old_Entry = settings[setting.Key];
					old_Entry.value = rawSetting;
					settings[setting.Key] = old_Entry;
				}
			}*/
		}

		// Save the current settings 
		public void save ()
		{
			// Open the settings file
			using (var config = File.Open (Path.Combine(Constants.get().path_SettingFileFolder, FILENAME), FileMode.OpenOrCreate))
			{
				using (var writer = new StreamWriter (config))
				{
					if (!config.CanWrite) return;
					// Save settings line by line
					foreach (var setting in settings)
					{
						writer.WriteLine ($"{setting.Value.name} = {setting.Value.value}");
					}
				}
				config.Close ();
			}
		}

		// Get private member SHOW_FPS
		public bool get_show_fps ()
		{
			return SHOW_FPS;
		}

		// Get private member SHOW_PACKETS
		public bool get_show_packets ()
		{
			return SHOW_PACKETS;
		}

		// Get private member var_LOGIN
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
		public string get_title ()
		{
			return TITLE;
		}

		// Get private member VERSION
		public string get_version ()
		{
			return VERSION;
		}

		// Get private member LoginMusic
		public string get_login_music ()
		{
			return LoginMusic;
		}

		// Get private member LoginMusicSEA
		public string get_login_music_sea ()
		{
			return LoginMusicSEA;
		}

		// Get private member LoginMusicNewtro
		public string get_login_music_newtro ()
		{
			return LoginMusicNewtro;
		}

		// Get private member JOINLINK
		public string get_joinlink ()
		{
			return JOINLINK;
		}

		// Get private member WEBSITE
		public string get_website ()
		{
			return WEBSITE;
		}

		// Get private member FINDID
		public string get_findid ()
		{
			return FINDID;
		}

		// Get private member FINDPASS
		public string get_findpass ()
		{
			return FINDPASS;
		}

		// Get private member RESETPIC
		public string get_resetpic ()
		{
			return RESETPIC;
		}

		// Get private member CHARGENX
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

		// Get the max Width allowed
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

			protected void save ()
			{
				Configuration.get ().save ();
			}
			public string name;
			public string value;

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
				base.save ();
			}

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
				base.save ();

			}

			public string load ()
			{
				return value;
			}

		}

		// Setting which converts to a Point<int16_t>
		public class PointEntry : Entry
		{
			protected PointEntry (string n, string v) : base (n, v)
			{
			}

			public void save (Point_short vec)
			{
				value = vec.to_string ();
				base.save ();

			}

			public Point_short load ()
			{
				string xstr = value.Substring (1, value.IndexOf (",") - 1);
				string ystr = value.Substring (value.IndexOf (",") + 1, value.IndexOf (")") - value.IndexOf (",") - 1);

				var x = string_conversion.or_zero<short> (xstr);
				var y = string_conversion.or_zero<short> (ystr);

				return new Point_short (x, y);
			}
		}

		// Setting which converts to an integer type
		public class IntegerEntry<T> : Entry
		{
			protected IntegerEntry (string n, string v) : base (n, v)
			{
			}

			public void save (T num)
			{
				value = Convert.ToString (num);
				base.save ();

			}

			public T load ()
			{
				return string_conversion.or_zero<T> (value);
			}

		}

		// Setting which converts to a byte
		public class ByteEntry : IntegerEntry<byte>
		{
			protected ByteEntry (string n, string v) : base (n, v)
			{
			}

		}

		// Setting which converts to a short
		public class ShortEntry : IntegerEntry<ushort>
		{
			protected ShortEntry (string n, string v) : base (n, v)
			{
			}

		}

		// Setting which converts to an int
		public class IntEntry : IntegerEntry<uint>
		{
			protected IntEntry (string n, string v) : base (n, v)
			{
			}

		}

		// Setting which converts to a long
		public class LongEntry : IntegerEntry<ulong>
		{
			protected LongEntry (string n, string v) : base (n, v)
			{
			}

		}

		private string FILENAME = "Settings";
		private string TITLE = "MapleStory";
		private string VERSION = "213.2";
		private string LoginMusic = "BgmUI.img/Title";
		private string LoginMusicSEA = "BgmGL.img/OldMaple";
		//private string LoginMusicNewtro = "BgmEvent2.img/Newtro_Login";//todo doesnt exist
		private string LoginMusicNewtro = "BgmUI.img/Title";
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
		private short MAXWIDTH = 1920;
		private short MAXHEIGHT = 1080;
		private string VolumeSerialNumber = "F02BF618";
		private byte worldid;
		private byte channelid;
		private bool admin;
		public TypeMap<Entry> settings = new TypeMap<Entry> ();

        public const string DefaultPos_Btn_HeavyAttack = "(1391,917)";
        public const string DefaultPos_Btn_LightAttack = "(1401,920)";
        public const string DefaultPos_Btn_Jump = "(1554,920)";
        public const string DefaultPos_Btn_PickUp = "(1707,920)";

        public const string DefaultPos_Btn_Skill1 = "(1396,618)";
        public const string DefaultPos_Btn_Skill2 = "(1566,618)";
        public const string DefaultPos_Btn_Skill3 = "(1736,618)";
        public const string DefaultPos_Btn_Skill4 = "(1344,768)";
        public const string DefaultPos_Btn_Skill5 = "(1514,768)";
        public const string DefaultPos_Btn_Skill6 = "(1682,768)";
    }

	// IP Address which the client will connect to
	public class ServerIP : Configuration.StringEntry
	{
		public ServerIP () : base ("ServerIP", "218.87.219.250")
		{
		}
	}

	// Port which the client will connect to
	public class ServerPort : Configuration.StringEntry
	{
		public ServerPort () : base ("ServerPort", "8485")
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

	// The Width of the screen
	public class Width : Configuration.ShortEntry
	{
		public Width () : base ("Width", "1280")
		{
		}
	}

	// The height of the screen
	public class Height : Configuration.ShortEntry
	{
		public Height () : base ("Height", "720")
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
		public DefaultAccount () : base ("DefaultAccount", "")
		{
		}
	}
	public class DefaultPassword : Configuration.StringEntry
	{
		public DefaultPassword () : base ("DefaultPassword", "")
		{
		}
	}
	// The last used world
	public class DefaultWorld : Configuration.ByteEntry
	{
		public DefaultWorld () : base ("DefaultWorld", "0")
		{
		}
	}

	// The last used channel
	public class DefaultChannel : Configuration.ByteEntry
	{
		public DefaultChannel () : base ("DefaultChannel", "0")
		{
		}
	}

	// The last used region
	public class DefaultRegion : Configuration.ByteEntry
	{
		public DefaultRegion () : base ("DefaultRegion", "7")
		{
		}
	}

	// The last used character
	public class DefaultCharacter : Configuration.ByteEntry
	{
		public DefaultCharacter () : base ("DefaultCharacter", "0")
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
		public PosOPTIONMENU () : base ("PosOPTIONMENU", "(170, -1)")
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

	// The default position of UIPartyMemberHP
	public class PosPartyMemberHP : Configuration.PointEntry
	{
		public PosPartyMemberHP () : base ("PosPartyMemberHP", "(604, 104)")
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

    public class JSDebugOn : Configuration.BoolEntry
    {
        public JSDebugOn() : base("JSDebugOn", "false")
        {

        }
    }

    
    public class PosBtnHeavyAttack : Configuration.PointEntry
    {
        public PosBtnHeavyAttack() : base("PosHeavyAttack", Configuration.DefaultPos_Btn_HeavyAttack)
        {
        }
    }
    public class PosBtnLightAttack : Configuration.PointEntry
    {
        public PosBtnLightAttack() : base("PosLightAttack", Configuration.DefaultPos_Btn_LightAttack)
        {
        }
    }
    public class PosBtnJump : Configuration.PointEntry
    {
        public PosBtnJump() : base("PosBtnJump", Configuration.DefaultPos_Btn_Jump)
        {
        }
    }
    public class PosBtnPickUp : Configuration.PointEntry
    {
        public PosBtnPickUp() : base("PosBtnPickUp", Configuration.DefaultPos_Btn_PickUp)
        {
        }
    }
    public class PosBtnSkill1 : Configuration.PointEntry
    {
        public PosBtnSkill1() : base("PosBtnSkill1", Configuration.DefaultPos_Btn_Skill1)
        {
        }
    }
    public class PosBtnSkill2 : Configuration.PointEntry
    {
        public PosBtnSkill2() : base("PosBtnSkill2", Configuration.DefaultPos_Btn_Skill2)
        {
        }
    }
    public class PosBtnSkill3 : Configuration.PointEntry
    {
        public PosBtnSkill3() : base("PosBtnSkill3", Configuration.DefaultPos_Btn_Skill3)
        {
        }
    }
    public class PosBtnSkill4 : Configuration.PointEntry
    {
        public PosBtnSkill4() : base("PosBtnSkill4", Configuration.DefaultPos_Btn_Skill4)
        {
        }
    }
    public class PosBtnSkill5 : Configuration.PointEntry
    {
        public PosBtnSkill5() : base("PosBtnSkill5", Configuration.DefaultPos_Btn_Skill5)
        {
        }
    }
    public class PosBtnSkill6 : Configuration.PointEntry
    {
        public PosBtnSkill6() : base("PosBtnSkill6", Configuration.DefaultPos_Btn_Skill6)
        {
        }
    }
 
    // Can be used to access settings
    public class Setting<T> where T : Configuration.Entry, new ()
	{
		public static T get ()
		{
			var entry = Configuration.get ().settings.get<T> ();

			if (entry != null)
			{
				return (T)entry;
			}
			else
			{
				return new T ();
			}
		}
	}
}