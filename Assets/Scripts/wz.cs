using System.Collections.Generic;
using System.IO;
using HaCreator;
using HaCreator.Wz;
using MapleLib.WzLib;
using provider;

namespace ms
{
    public class wz
    {
        private static List<WzFile> files = new List<WzFile>();
        public static WzFileManager WzManager;

        private static bool exists(string name)
        {
            return WzManager.wzFiles.ContainsKey(name.ToLower());
        }

        private static WzFile add_file(string name)
        {
	        name = name.Replace ("wz", "").Replace ("/","").Replace (".","");
	        
            if (exists(name))
                return WzManager.wzFiles[name.ToLower()];

            //var file = new WzFile(name,WzMapleVersion.EMS);
             WzManager.LoadWzFile(name);
             var file = WzManager.wzFiles[name.ToLower()];


			files.Add(file);

            return file;
        }

        public static WzFile wzFile_base, wzFile_character, wzFile_effect, wzFile_etc, wzFile_item, wzFile_map, wzFile_wzFile_mapPretty, wzFile_mapLatest, wzFile_map001, wzFile_mob, wzFile_morph, wzFile_npc, wzFile_quest, wzFile_reactor, wzFile_skill, wzFile_SkillMy1, wzFile_sound, wzFile_string, wzFile_tamingmob, wzFile_ui, wzFile_UI_Endless;
        //NXNode baseFile, character, effect, etc, item, map, mapPretty, mapLatest, map001, mob, morph, npc, quest, reactor, skill, sound, stringFile, tamingmob, ui;

        public static MapleDataProvider wzProvider_base, wzProvider_character, wzProvider_effect, wzProvider_etc, wzProvider_item, wzProvider_map, wzProvider_wzProvider_mapPretty, wzProvider_mapLatest, wzProvider_map001, wzProvider_mob, wzProvider_morph, wzProvider_npc, wzProvider_quest, wzProvider_reactor, wzProvider_skill, wzProvider_SkillMy1, wzProvider_sound, wzProvider_string, wzProvider_tamingmob, wzProvider_ui, wzProvider_UI_Endless;

        public static WzFile get_file (string wzPath)
        {
	        return add_file (wzPath);
        }
        public static void load_all(string wzPath)
        {
	        WzManager ??= new WzFileManager(wzPath);

			wzFile_base = add_file("base");
            wzFile_character = add_file("character1");
            wzFile_effect = add_file("effect");
            wzFile_etc = add_file("etc");
            wzFile_item = add_file("item");
            //wzFile_map = add_file("map");
            wzFile_mob = add_file("mob");
            wzFile_npc = add_file("npc");
			wzFile_quest = add_file ("Quest");
			wzFile_reactor = add_file ("Reactor");
			wzFile_skill = add_file("skill");
			wzFile_sound = add_file("sound");
            wzFile_string = add_file("string");
            wzFile_ui = add_file("UI_083");
			wzFile_UI_Endless = add_file("UI_Endless");



            wzProvider_base = MapleDataProviderFactory.getDataProvider("base.wz") ;
            wzProvider_character = MapleDataProviderFactory.getDataProvider("character1.wz");
            wzProvider_effect = MapleDataProviderFactory.getDataProvider("effect.wz");
            wzProvider_etc = MapleDataProviderFactory.getDataProvider("etc.wz");
            wzProvider_item = MapleDataProviderFactory.getDataProvider("item.wz");
            wzProvider_map = MapleDataProviderFactory.getDataProvider("map.wz");
            wzProvider_mob = MapleDataProviderFactory.getDataProvider("mob.wz");
            wzProvider_npc = MapleDataProviderFactory.getDataProvider("npc.wz");
            wzProvider_quest = MapleDataProviderFactory.getDataProvider("Quest.wz");
            wzProvider_reactor = MapleDataProviderFactory.getDataProvider("Reactor.wz");
            wzProvider_skill = MapleDataProviderFactory.getDataProvider("skill.wz");
            wzProvider_sound = MapleDataProviderFactory.getDataProvider("sound.wz");
            wzProvider_string = MapleDataProviderFactory.getDataProvider("string.wz");
            wzProvider_ui = MapleDataProviderFactory.getDataProvider("UI_083.wz");
            wzProvider_UI_Endless = MapleDataProviderFactory.getDataProvider("UI_Endless.wz");

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
        public static void load_string(string wzPath)
        {
            WzManager ??= new WzFileManager(wzPath);
            wzFile_string = add_file("string");
        }

        public static void load_skill(string wzPath)
        {
            WzManager ??= new WzFileManager(wzPath);
            wzFile_skill = add_file("skill");
        }

        public static readonly string[] SKILL_WZ_FILES = {
			"Skill",
			"SkillMy1"
		};
		/// <summary>
		/// $"{jobid}.img"
		/// </summary>
		/// <param name="imgName"></param>
		/// <returns></returns>
		public static WzImage findSkillImage(string imgName)
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


	}
}