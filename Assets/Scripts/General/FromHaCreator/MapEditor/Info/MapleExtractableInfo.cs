﻿/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using MapleLib.WzLib;
using System;
using System.Collections.Generic;
using MapleLib.WzLib;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaCreator.MapEditor.Info
{
    public abstract class MapleExtractableInfo : MapleDrawableInfo
    {
        public MapleExtractableInfo(Bitmap image, MapleLib.WzLib.Point origin, WzObject parentObject)
            : base(image, origin, parentObject)
        {
        }

        public override Bitmap Image
        {
            get
            {
                if (base.Image == null)
                    ParseImage();

                if (base.Image == null || (base.Image.Width == 1 && base.Image.Height == 1))
                {
                    //return global::HaCreator.Properties.Resources.placeholder;
                    return new Bitmap(1,1);
                }
                return base.Image;
            }
            set
            {
                base.Image = value;
            }
        }

        public void ParseImageIfNeeded()
        {
            if (Image == null)
                ParseImage();
        }

        public abstract void ParseImage();
    }
}
