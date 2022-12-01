using System.Collections.Generic;
using System.IO;
using HaCreator;
using HaCreator.Wz;
using MapleLib.WzLib;
using provider;
using UnityEngine;

namespace ms
{
	public class wz
	{
		private static List<WzFile> files = new List<WzFile> ();
		public static WzFileManager WzManager;

		private static bool exists (string name)
		{
			return true;
		}

		private static WzFile add_file (string name)
		{
			if (!exists (name))
				return null;

			//var file = new WzFile(name,WzMapleVersion.EMS);
			WzManager.LoadWzFile (name);
			var file = WzManager.wzFiles[name.ToLower ()];


			files.Add (file);

			return file;
		}

		public static WzFile wzFile_base, wzFile_character, wzFile_effect, wzFile_etc, wzFile_item, wzFile_map, wzFile_wzFile_mapPretty, wzFile_mapLatest, wzFile_map001, wzFile_mob, wzFile_morph, wzFile_npc, wzFile_quest, wzFile_reactor, wzFile_skill, wzFile_SkillMy1, wzFile_sound, wzFile_string, wzFile_tamingmob, wzFile_ui, wzFile_UI_Endless;
		//NXNode baseFile, character, effect, etc, item, map, mapPretty, mapLatest, map001, mob, morph, npc, quest, reactor, skill, sound, stringFile, tamingmob, ui;

		public static MapleDataProvider _xml_map;
		public static MapleDataProvider xml_map => _xml_map ??= MapleDataProviderFactory.getDataProvider (new System.IO.DirectoryInfo (Constants.get ().path_WzXmlFolder + "/Map.wz"));
		//public static MapleDataProvider xml_character = MapleDataProviderFactory.getDataProvider (new System.IO.DirectoryInfo (Constants.get ().path_WzXmlFolder + "/Character.wz"));

		public static void load_all (string wzPath)
		{
			WzManager ??= new WzFileManager (wzPath);

			wzFile_base = add_file ("base");
			wzFile_character = add_file ("character1");
			wzFile_effect = add_file ("effect");
			wzFile_etc = add_file ("etc");
			wzFile_item = add_file ("item");
			wzFile_map = add_file ("map");
			wzFile_wzFile_mapPretty = add_file ("map");
			wzFile_mapLatest = add_file ("MapLatest");
			wzFile_map001 = add_file ("Map001");
			wzFile_mob = add_file ("mob");
			wzFile_npc = add_file ("npc");
			wzFile_quest = add_file ("Quest");
			wzFile_reactor = add_file ("Reactor");
			wzFile_skill = add_file ("skill");
			wzFile_SkillMy1 = add_file ("SkillMy1");
			wzFile_sound = add_file ("sound");
			wzFile_string = add_file ("string");
			wzFile_ui = add_file ("UI_New");
			wzFile_UI_Endless = add_file ("UI_Endless");

			/*if (exists("Base.nx"))
			{
				baseWz = add_file("Base.nx");
				character = add_file("Character.nx");
				effect = add_file("Effect.nx");
				etc = add_file("Etc.nx");
				item = add_file("Item.nx");
				map = add_file("Map.nx");
				mapPretty = add_file("MapPretty.nx");
				mapLatest = add_file("MapLatest.nx");
				map001 = add_file("Map001.nx");
				mob = add_file("Mob.nx");
				morph = add_file("Morph.nx");
				npc = add_file("Npc.nx");
				quest = add_file("Quest.nx");
				reactor = add_file("Reactor.nx");
				skill = add_file("Skill.nx");
				sound = add_file("Sound.nx");1
				stringFile = add_file("String.nx");
				tamingmob = add_file("TamingMob.nx");
				ui = add_file("UI.nx");
			}
			else if (exists("Data.nx"))
			{
				*/ /*baseFile = add_file("Data.nx");
				character = base["Character"];
				effect = base["Effect"];
				etc = base["Etc"];
				item = base["Item"];
				map = base["Map"];
				mob = base["Mob"];
				morph = base["Morph"];
				npc = base["Npc"];
				quest = base["Quest"];
				reactor = base["Reactor"];
				skill = base["Skill"];
				sound = base["Sound"];
				string = base["String"];
				tamingmob = base["TamingMob"];
				ui = base["UI"];*/ /*
			}
			else
			{
				//throw runtime_error("Failed to locate nx files.");
			}*/
		}

		public static readonly string[] SKILL_WZ_FILES =
		{
			"Skill",
			"SkillMy1"
		};

		/// <summary>
		/// $"{jobid}.img"
		/// </summary>
		/// <param name="imgName"></param>
		/// <returns></returns>
		public static WzImage findSkillImage (string imgName)
		{
			foreach (string skillWzFile in SKILL_WZ_FILES)
			{
				string mapWzFile_ = skillWzFile.ToLower ();
				if (WzManager.wzFiles.ContainsKey (mapWzFile_))
				{
					WzObject mapImage = (WzImage)WzManager[mapWzFile_]?[imgName];

					if (mapImage != null)
					{
						return (WzImage)mapImage;
					}
				}
			}

			return null;
		}

		public static bool CheckLoadXml ()
		{
#if UNITY_EDITOR && UNITY_ANDROID
#endif

			Debug.Log (Application.targetFrameRate);
			if (Application.platform == RuntimePlatform.Android)
			{
				//BetterStreamingAssets.Initialize ();
				var paths = BetterStreamingAssets.GetFiles ("/", null, SearchOption.AllDirectories);
				//Debug.Log (paths.ToDebugLog ());

				for (int i = 0; i < paths.Length; i++)
				{
					var relative_FilePath = paths[i];
					var splashIndex = relative_FilePath.LastIndexOf ('/');
					var relative_DirectoryPath = string.Empty;
					if (splashIndex <= 0)
					{
						//Debug.Log (relative_FilePath);
						relative_DirectoryPath = string.Empty;
						//continue;
					}
					else
					{
						 relative_DirectoryPath = relative_FilePath.Substring (0,splashIndex);
					}

					var full_FilePath = Path.Combine (Application.persistentDataPath, relative_FilePath);
					var full_DirectoryPath = Path.Combine (Application.persistentDataPath, relative_DirectoryPath);
					//Debug.Log (paths.ToDebugLog ());
					if (File.Exists (full_FilePath))
					{
						continue;
					}
					//Debug.Log ($"File.Exists:{full_FilePath}");

					if (!Directory.Exists (full_DirectoryPath))
					{
						Directory.CreateDirectory (full_DirectoryPath);
					}
					//Debug.Log ($"File.Directory:{full_DirectoryPath}");

					using (var readStream = BetterStreamingAssets.OpenRead (relative_FilePath))
					{
						using (var writeSteam = System.IO.File.OpenWrite (full_FilePath))
						{
							readStream.CopyTo (writeSteam);
						}
					}
				}
			}


			return true;
		}
	}
}