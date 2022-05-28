﻿/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.MapEditor.Instance;
using HaCreator.Wz;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using MapleLib.WzLib.WzStructure;
using System;
using System.Collections.Generic;
using MapleLib.WzLib;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaCreator.MapEditor.Info
{
    public class NpcInfo : MapleExtractableInfo
    {
        private readonly string id;
        private readonly string name;

        private WzImage _LinkedWzImage;

        public NpcInfo(Bitmap image, MapleLib.WzLib.Point origin, string id, string name, WzObject parentObject)
            : base(image, origin, parentObject)
        {
            this.id = id;
            this.name = name;
            if(image!=null && image.Width==1 && image.Height==1)
                image = new Bitmap(1,1); 
                //image = global::HaCreator.Properties.Resources.placeholder; 
        }

        private void ExtractPNGFromImage(WzImage image)
        {
            WzCanvasProperty npcImage = WzInfoTools.GetNpcImage(image);
            if (npcImage != null)
            {
                Image = npcImage.GetLinkedWzCanvasBitmap();
                if(Image.Width==1 && Image.Height == 1)
                {
                    Image = new Bitmap(1,1);
                    //Image = global::HaCreator.Properties.Resources.placeholder;
                }
                Origin = WzInfoTools.PointFToPoint(npcImage.GetCanvasOriginPosition());
            }
            else
            {
                Image = new Bitmap(1, 1);
                Origin = new MapleLib.WzLib.Point();
            }
        }

        public override void ParseImage()
        {
            if (LinkedWzImage != null) // attempt to load from here too
                ExtractPNGFromImage(LinkedWzImage);
            else
                ExtractPNGFromImage((WzImage)ParentObject);
        }

        public static NpcInfo Get(string id)
        {
            WzImage npcImage = (WzImage)Program.WzManager["npc"][id + ".img"];
            if (npcImage == null)
                return null;
            if (!npcImage.Parsed)
                npcImage.ParseImage();
            if (npcImage.HCTag == null)
                npcImage.HCTag = NpcInfo.Load(npcImage);
            NpcInfo result = (NpcInfo)npcImage.HCTag;
            result.ParseImageIfNeeded();
            return result;
        }

        private static NpcInfo Load(WzImage parentObject)
        {
            string id = WzInfoTools.RemoveExtension(parentObject.Name);
            return new NpcInfo(null, new MapleLib.WzLib.Point(), id, WzInfoTools.GetNpcNameById(id), parentObject);
        }

        public override BoardItem CreateInstance(Layer layer, Board board, int x, int y, int z, bool flip)
        {
            if (Image == null) ParseImage();
            return new NpcInstance(this, board, x, y, UserSettings.Npcrx0Offset, UserSettings.Npcrx1Offset, 8, null, 0, flip, false, null, null);
        }

        public BoardItem CreateInstance(Board board, int x, int y, int rx0Shift, int rx1Shift, int yShift, string limitedname, int? mobTime, MapleBool flip, MapleBool hide, int? info, int? team)
        {
            if (Image == null) ParseImage();
            return new NpcInstance(this, board, x, y, rx0Shift, rx1Shift, yShift, limitedname, mobTime, flip, hide, info, team);
        }

        public string ID
        {
            get { return id; }
            private set { }
        }

        public string Name
        {
            get { return name; }
            private set {  }
        }

        /// <summary>
        /// The source WzImage of the reactor or default
        /// </summary>
        public WzImage LinkedWzImage
        {
            get {
                if (_LinkedWzImage == null)
                {
                    WzStringProperty link = (WzStringProperty)((WzSubProperty)((WzImage)ParentObject)["info"])["link"];
                    if (link != null)
                        _LinkedWzImage = (WzImage)Program.WzManager["npc"][link.Value + ".img"];
                    else
                        _LinkedWzImage = (WzImage)Program.WzManager["npc"][id + ".img"]; // default
                }
                return _LinkedWzImage; 
            }
            set { this._LinkedWzImage = value; }
        }
    }
}
