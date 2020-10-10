using System.Collections.Generic;
using HaRepacker;
using MapleLib.WzLib;

namespace nl
{
    public class nx
	{
		static List<WzFile> files = new List<WzFile>();
		static WzFileManager wzFileManager = new WzFileManager();
		static bool exists(string name)
		{
			return true;
		}

		static WzFile add_file(string name)
		{
			if (!exists(name))
				return null;

			//var file = new WzFile(name,WzMapleVersion.EMS);
			var file = wzFileManager.LoadWzFile(name); ;
			files.Add(file);

			return file;
		}

		public static WzFile baseWz, character, effect, etc, item, map, mapPretty, mapLatest, map001, mob, morph, npc, quest, reactor, skill, sound, stringFile, tamingmob, ui;
		//NXNode baseFile, character, effect, etc, item, map, mapPretty, mapLatest, map001, mob, morph, npc, quest, reactor, skill, sound, stringFile, tamingmob, ui;

		public static void load_all(string suffix)
		{
			map = add_file(suffix+"Map.wz");
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
				sound = add_file("Sound.nx");
				stringFile = add_file("String.nx");
				tamingmob = add_file("TamingMob.nx");
				ui = add_file("UI.nx");
			}
			else if (exists("Data.nx"))
			{
				*//*baseFile = add_file("Data.nx");
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
				ui = base["UI"];*//*
			}
			else
			{
				//throw runtime_error("Failed to locate nx files.");
			}*/
		}
	}
}