/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using System;
using System.Collections.Generic;
using MapleLib.WzLib;
using System.IO;
using MapleLib.WzLib.Util;

namespace HaRepacker
{
    public class WzFileManager
    {
        #region Constants
        public static readonly string[] MOB_WZ_FILES = { "Mob", "Mob001", "Mob2" };
        public static readonly string[] MAP_WZ_FILES = { "Map", "Map001",
            "Map002", //kms now stores main map key here
            "Map2" };
        public static readonly string[] SOUND_WZ_FILES = { "Sound", "Sound001" };

        public static readonly string[] COMMON_MAPLESTORY_DIRECTORY = new string[] {
            @"C:\Nexon\MapleStory",
            @"C:\Program Files\WIZET\MapleStory",
            @"C:\MapleStory",
            @"C:\Program Files (x86)\Wizet\MapleStorySEA"
        };
        #endregion

        private List<WzFile> wzFiles = new List<WzFile>();

        public WzFileManager()
        {
        }

        private bool OpenWzFile(string path, WzMapleVersion encVersion, short version, out WzFile file)
        {
            try
            {
                WzFile f = new WzFile(path, version, encVersion);
                lock (wzFiles)
                {
                    wzFiles.Add(f);
                }
                string parseErrorMessage = string.Empty;
                bool parseSuccess = f.ParseWzFile(out parseErrorMessage);
                if (!parseSuccess)
                {
                    file = null;
                    //Warning.Error("Error initializing " + Path.GetFileName(path) + " (" + parseErrorMessage + ").");
                    return false;
                }

                file = f;
                return true;
            }
            catch (Exception e)
            {
                //Warning.Error("Error initializing " + Path.GetFileName(path) + " (" + e.Message + ").\r\nAlso, check that the directory is valid and the file is not in use.");
                file = null;
                return false;
            }
        }
        
        /// <summary>
        /// Load a WZ file from path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public WzFile LoadWzFile(string path)
        {
            short fileVersion = -1;
            bool isList = WzTool.IsListFile(path);

            return LoadWzFile(path, WzTool.DetectMapleVersion(path, out fileVersion), fileVersion);
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherWzFileToLoadAt"></param>
        /// <param name="path"></param>
        /// <param name="encVersion"></param>
        /// <param name="version"></param>
        /// <param name="panel"></param>
        /// <param name="currentDispatcher">Dispatcher thread</param>
        /// <returns></returns>
        private WzFile LoadWzFile(string path, WzMapleVersion encVersion, short version)
        {
            WzFile newFile;
            if (!OpenWzFile(path, encVersion, version, out newFile))
            {
                return null;
            }
            return newFile;
        }       
    }
}