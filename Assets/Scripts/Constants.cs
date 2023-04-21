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

#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE

#endif

using System.Reflection;

namespace ms
{
	public class Constants : Singleton<Constants>
	{
		public Constants ()
		{
			//VIEWWIDTH = 800;
			//VIEWHEIGHT = 600;
		}

		public new void Dispose ()
		{
			base.Dispose ();
		}

		public short get_viewwidth ()
		{
			return VIEWWIDTH;
		}

		public void set_viewwidth (short width)
		{
			VIEWWIDTH = width;
		}

		public short get_viewheight ()
		{
			return VIEWHEIGHT;
		}

		public void set_viewheight (short height)
		{
			VIEWHEIGHT = height;
		}

		// Window and screen Width.
		private short VIEWWIDTH = 800;

		// Window and screen height.
		private short VIEWHEIGHT = 600;

		public short VIEWWIDTH_Login { get; set; } = 800;

		public short VIEWHEIGHT_Login { get; set; } = 600;

		public short VIEWWIDTH_CashShop { get; set; } = 1024;

		public short VIEWHEIGHT_CashShop { get; set; } = 768;

		//public const ushort TIMESTEP = 8;//original is 8
		public const ushort TIMESTEP = 8;//original is 8

		public float walkSpeed = 1;
		public float jumpSpeed = 1;
		public float climbSpeed = 1;
		public float flySpeed = 1;
		public float fallSpeed = 1;
		public float animSpeed = 1;

		public float frameDelay = 0.5f;
		public float multiplier_timeStep = 1f;

		public string defaultSpriteDrawerName = "SpriteDrawer";
		public BindingFlags bindingFlags_UIElementInfo = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
		public int MAX_PartyMemberCount = 6;

#if UNITY_EDITOR || WINDOWS
		/*public string path_MapleStoryFolder = @"G:\Program Files (x86)\MapleStory";
		public string path_SettingFileFolder = @"G:\Program Files (x86)\MapleStory";*/
		
		public string path_MapleStoryFolder = $"{UnityEngine.Application.persistentDataPath}";
		public string path_SettingFileFolder = $"{UnityEngine.Application.persistentDataPath}";

#elif UNITY_ANDROID || ANDROID
		public string path_MapleStoryFolder = $"{UnityEngine.Application.persistentDataPath}";
		public string path_SettingFileFolder = $"{UnityEngine.Application.persistentDataPath}";

		//public string path_MapleStoryFolder = $"/storage/emulated/0/Download";
		//public string path_SettingFileFolder = $"/storage/emulated/0/Download";

		//public string path_MapleStoryFolder = $"/storage/emulated/0/ForeverStory";
		//public string path_SettingFileFolder = $"/storage/emulated/0/ForeverStory";
#else
		//public string path_MapleStoryFolder = $"/storage/emulated/0/Android/data/MapleStory_resurrected_MonoGame_Android.MapleStory_resurrected_MonoGame_Android/files";
		//public string path_MapleStoryFolder = $"/storage/emulated/0/Download";
		//public string path_SettingFileFolder = $"/storage/emulated/0/Download";

		public string path_MapleStoryFolder = @"F:\Program Files (x86)\MapleStory";
		public string path_SettingFileFolder = @"F:\Program Files (x86)\MapleStory";
#endif
	}
}