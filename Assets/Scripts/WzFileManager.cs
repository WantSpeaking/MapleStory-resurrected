/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using System.Collections;
using System.IO;
using MapleLib.WzLib;
using MapleLib.WzLib.WzStructure.Data;
using MapleLib.Helpers;
using MapleLib.WzLib.Util;

namespace HaCreator.Wz
{
    public class WzFileManager
    {
        #region Constants
        public static readonly string[] MOB_WZ_FILES = {
            "Mob",
            "Mob001",
            "Mob2" };
        public static readonly string[] MAP_WZ_FILES = {
            "Map",
            "Map001",
            "Map002", //kms now stores main map key here
            "Map2",
            "MapLatest"
        };
        public static readonly string[] SOUND_WZ_FILES = {
            "Sound",
            "Sound001",
            "Sound2",
            "Sound002"
        };

        public static readonly string[] COMMON_MAPLESTORY_DIRECTORY = new string[] {
            @"C:\Nexon\MapleStory",
            @"D:\Nexon\Maple",
            @"C:\Program Files\WIZET\MapleStory",
            @"C:\MapleStory",
            @"C:\Program Files (x86)\Wizet\MapleStorySEA"
        };
        #endregion


        private string baseDir;
        public Dictionary<string, WzFile> wzFiles = new Dictionary<string, WzFile> ();
        public Dictionary<WzFile, bool> wzFilesUpdated = new Dictionary<WzFile, bool> (); // flag for the list of WZ files changed to be saved later via Repack 
        public HashSet<WzImage> updatedImages = new HashSet<WzImage> ();
        public Dictionary<string, WzMainDirectory> wzDirs = new Dictionary<string, WzMainDirectory> ();
        private readonly WzMapleVersion version;

        private static WzFileManager instance;

        /*public static WzFileManager Instance
        {
	        get
	        {
		        if (instance == null)
		        {
			        instance = new WzFileManager ();
		        }
		        return instance;
	        }
        }

        public static WzFileManager get () => Instance;*/

        public WzFileManager (string directory, WzMapleVersion version)
        {
            baseDir = directory;
            this.version = version;
            instance = this;
        }

        public WzFileManager (string directory)
        {
            baseDir = directory;
            this.version = WzMapleVersion.GENERATE;
            instance = this;
        }

        private string Capitalize (string x)
        {
            if (x.Length > 0 && char.IsLower (x[0]))
                return new string (new char[] { char.ToUpper (x[0]) }) + x.Substring (1);
            return x;
        }

        /// <summary>
        /// Cleanup 
        /// </summary>
        public void Clear ()
        {
            wzFiles.Clear ();
            wzFilesUpdated.Clear ();
            updatedImages.Clear ();
            wzDirs.Clear ();
        }

        public bool LoadWzFile (string name)
        {
            try
            {
	            var filePath = Path.Combine(baseDir, Capitalize(name) + ".wz");
                WzFile wzf = new WzFile (filePath, WzTool.DetectMapleVersion (filePath, out var fileVersion));

                WzFileParseStatus parseStatus = wzf.ParseWzFile ();
                if (parseStatus != WzFileParseStatus.Success)
                {
                    //MessageBox.Show ("Error parsing " + name + ".wz (" + parseStatus.GetErrorDescription () + ")");
                    return false;
                }

                name = name.ToLower ();
                wzFiles[name] = wzf;
                wzFilesUpdated[wzf] = false;
                wzDirs[name] = new WzMainDirectory (wzf);
                return true;
            }
            catch (Exception ex)
            {
                //HaRepackerLib.Warning.Error("Error initializing " + name + ".wz (" + e.Message + ").\r\nCheck that the directory is valid and the file is not in use.");
                AppDebug.LogError($"Message:{ex.Message} + Source:{ex.Source} + InnerException:{ex.InnerException} + StackTrace:{ex.StackTrace}");
                return false;
            }
        }

        public bool LoadDataWzFile (string name)
        {
            try
            {
                WzFile wzf = new WzFile (Path.Combine (baseDir, Capitalize (name) + ".wz"), version);

                WzFileParseStatus parseStatus = wzf.ParseWzFile ();
                if (parseStatus != WzFileParseStatus.Success)
                {
                    //MessageBox.Show ("Error parsing " + name + ".wz (" + parseStatus.GetErrorDescription () + ")");
                    return false;
                }

                name = name.ToLower ();
                wzFiles[name] = wzf;
                wzFilesUpdated[wzf] = false;
                wzDirs[name] = new WzMainDirectory (wzf);
                foreach (WzDirectory mainDir in wzf.WzDirectory.WzDirectories)
                {
                    wzDirs[mainDir.Name.ToLower ()] = new WzMainDirectory (wzf, mainDir);
                }
                return true;
            }
            catch (Exception e)
            {
                //MessageBox.Show ("Error initializing " + name + ".wz (" + e.Message + ").\r\nCheck that the directory is valid and the file is not in use.");
                return false;
            }
        }

        /// <summary>
        /// Sets WZ file as updated for saving
        /// </summary>
        /// <param name="name"></param>
        /// <param name="img"></param>
        public void SetWzFileUpdated (string name, WzImage img)
        {
            img.Changed = true;
            updatedImages.Add (img);
            wzFilesUpdated[GetMainDirectoryByName (name).File] = true;
        }

        /// <summary>
        /// Gets WZ by name from the list of loaded files
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public WzMainDirectory GetMainDirectoryByName (string name)
        {
            name = name.ToLower ();

            if (name.EndsWith (".wz"))
                name = name.Replace (".wz", "");

            return wzDirs[name];
        }

        public WzDirectory this[string name]
        {
            get { return (wzDirs.ContainsKey (name.ToLower ()) ? wzDirs[name.ToLower ()].MainDir : null); }    //really not very useful to return null in this case
        }

        public WzDirectory String
        {
            get { return GetMainDirectoryByName ("string").MainDir; }
        }

        //data.wz is wildly inconsistent between versions now, just avoid at all costs
        public bool HasDataFile
        {
            get { return false; }//return File.Exists(Path.Combine(baseDir, "Data.wz")); }
        }

        public string BaseDir
        {
            get { return baseDir; }
        }



        /// <summary>
        /// Finds a map image from the list of Map.wzs
        /// </summary>
        /// <param name="mapid"></param>
        /// <param name="mapcat"></param>
        /// <returns></returns>
        public WzImage FindMapImage (string mapid, string mapcat)
        {
            foreach (string mapWzFile in MAP_WZ_FILES)
            {
                string mapWzFile_ = mapWzFile.ToLower ();
                if (this.wzFiles.ContainsKey (mapWzFile_))
                {
                    WzObject mapImage = (WzImage)this[mapWzFile_]?["Map"]?[mapcat]?[mapid + ".img"];

                    if (mapImage != null)
                    {
                        return (WzImage)mapImage;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Finds a suitable (Map.wz, Map001.wz, Map2.wz) for storing the newly created map
        /// </summary>
        /// <param name="cat">Map01, Map02, Map001.wz</param>
        /// <returns></returns>
        public WzDirectory FindMapWz (string cat)
        {
            foreach (string mapWzFile in MAP_WZ_FILES)
            {
                string mapWzFile_ = mapWzFile.ToLower ();
                WzDirectory mapDir = (WzDirectory)this[mapWzFile_]?["Map"];
                if (mapDir != null)
                {
                    WzDirectory catDir = (WzDirectory)mapDir[cat];
                    if (catDir != null)
                        return catDir;
                }
            }
            return null;
        }


        /*        public void ExtractItems()
                {
                    WzImage consImage = (WzImage)String["Consume.img"];
                    if (!consImage.Parsed) consImage.ParseImage();
                    foreach (WzSubProperty item in consImage.WzProperties)
                    {
                        WzStringProperty nameProp = (WzStringProperty)item["name"];
                        string name = nameProp == null ? "" : nameProp.Value;
                        Program.InfoManager.Items.Add(WzInfoTools.AddLeadingZeros(item.Name, 7), name);
                    }
                }*/
    }
}
