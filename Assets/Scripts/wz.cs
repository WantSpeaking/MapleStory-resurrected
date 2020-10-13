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

		public static WzFile wzFile_base, wzFile_character, wzFile_effect, wzFile_etc, wzFile_item, wzFile_map, wzFile_wzFile_mapPretty, wzFile_mapLatest, wzFile_map001, wzFile_mob, wzFile_morph, wzFile_npc, wzFile_quest, wzFile_reactor, wzFile_skill, wzFile_sound, wzFile_string, wzFile_tamingmob, wzFile_ui;
		//NXNode baseFile, character, effect, etc, item, map, mapPretty, mapLatest, map001, mob, morph, npc, quest, reactor, skill, sound, stringFile, tamingmob, ui;

		public static void load_all(string suffix)
		{
			
			//wzFile_base = add_file(suffix+"base.wz");
			wzFile_character = add_file(suffix+"character.wz");
			wzFile_effect = add_file(suffix+"effect.wz");
			wzFile_item = add_file(suffix+"item.wz");
			wzFile_map = add_file(suffix+"map.wz");
			wzFile_skill = add_file(suffix+"skill.wz");
			wzFile_sound = add_file(suffix+"sound.wz");
			wzFile_string = add_file(suffix+"string.wz");
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