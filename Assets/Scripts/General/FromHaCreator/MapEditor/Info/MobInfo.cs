﻿/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.MapEditor.Instance;
using HaCreator.Wz;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using MapleLib.WzLib.WzStructure;
using MapleLib.WzLib;

namespace HaCreator.MapEditor.Info
{
    public class MobInfo : MapleExtractableInfo
    {
        private readonly string id;
        private readonly string name;

        private WzImage _LinkedWzImage;

        public MobInfo(Bitmap image, MapleLib.WzLib.Point origin, string id, string name, WzObject parentObject)
            : base(image, origin, parentObject)
        {
            this.id = id;
            this.name = name;
        }

        private void ExtractPNGFromImage(WzImage image)
        {
            WzCanvasProperty mobImage = WzInfoTools.GetMobImage(image);
            if (mobImage != null)
            {
                Image = mobImage.GetLinkedWzCanvasBitmap();
                Origin = WzInfoTools.PointFToPoint(mobImage.GetCanvasOriginPosition());
            }
            else
            {
                Image = new Bitmap(1, 1);
                Origin = new MapleLib.WzLib.Point();
            }
        }

        public override void ParseImage()
        {
            if (LinkedWzImage != null) // load from here too
                ExtractPNGFromImage(_LinkedWzImage);
            else
                ExtractPNGFromImage((WzImage)ParentObject);
        }

        /// <summary>
        /// Get monster by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MobInfo Get(string id)
        {
            foreach (string mobWzFile in WzFileManager.MOB_WZ_FILES)
            {
                WzImage mobImage = (WzImage)Program.WzManager[mobWzFile.ToLower()]?[id + ".img"];
                if (mobImage == null)
                    continue;

                if (!mobImage.Parsed)
                {
                    mobImage.ParseImage();
                }
                if (mobImage.HCTag == null)
                {
                    mobImage.HCTag = MobInfo.Load(mobImage);
                }
                MobInfo result = (MobInfo)mobImage.HCTag;
                result.ParseImageIfNeeded();
                return result;
            }
            return null;
        }

        private static MobInfo Load(WzImage parentObject)
        {
            string id = WzInfoTools.RemoveExtension(parentObject.Name);
            return new MobInfo(null, new MapleLib.WzLib.Point(), id, WzInfoTools.GetMobNameById(id), parentObject);
        }

        public override BoardItem CreateInstance(Layer layer, Board board, int x, int y, int z, bool flip)
        {
            if (Image == null) 
                ParseImage();

            return new MobInstance(this, board, x, y, UserSettings.Mobrx0Offset, UserSettings.Mobrx1Offset, 20, null, UserSettings.defaultMobTime, flip, false, null, null);
        }

        public BoardItem CreateInstance(Board board, int x, int y, int rx0Shift, int rx1Shift, int yShift, string limitedname, int? mobTime, MapleBool flip, MapleBool hide, int? info, int? team)
        {
            if (Image == null) 
                ParseImage();

            return new MobInstance(this, board, x, y, rx0Shift, rx1Shift, yShift, limitedname, mobTime, flip, hide, info, team);
        }

        public string ID
        {
            get { return id; }
            private set { }
        }

        public string Name
        {
            get { return name; }
            private set { }
        }

        /// <summary>
        /// The source WzImage of the reactor
        /// </summary>
        public WzImage LinkedWzImage
        {
            get {
                WzStringProperty link = (WzStringProperty)((WzSubProperty)((WzImage)ParentObject)["info"])["link"];
                if (link != null)
                    _LinkedWzImage = Program.WzManager.FindMobImage(link.Value);
                else
                    _LinkedWzImage = Program.WzManager.FindMobImage(id); // default

                return _LinkedWzImage; 
            }
            set { this._LinkedWzImage = value; }
        }
    }
}
