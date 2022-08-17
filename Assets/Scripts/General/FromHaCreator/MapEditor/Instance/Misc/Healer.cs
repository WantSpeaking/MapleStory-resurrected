﻿/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.MapEditor.Info;
using MapleLib.WzLib.WzStructure.Data;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNA = Microsoft.Xna.Framework;

namespace HaCreator.MapEditor.Instance.Misc
{
    public class Healer : BoardItem, INamedMisc
    {
        private ObjectInfo baseInfo;
        public int yMin;
        public int yMax;
        public int healMin;
        public int healMax;
        public int fall;
        public int rise;

        public Healer(ObjectInfo baseInfo, Board board, int x, int yMin, int yMax, int healMin, int healMax, int fall, int rise)
            : base(board, x, (yMax + yMin) / 2, -1)
        {
            this.baseInfo = baseInfo;
            this.yMin = yMin;
            this.yMax = yMax;
            this.healMin = healMin;
            this.healMax = healMax;
            this.fall = fall;
            this.rise = rise;
        }

        public override int Y
        {
            get
            {
                return (yMax + yMin) / 2;
            }
            set
            {
                lock (board.ParentControl)
                {
                    int offs = value - Y;
                    yMax += offs;
                    yMin += offs;
                }
            }
        }

        public override void Move(int x, int y)
        {
            lock (board.ParentControl)
            {
                position.X = x;
                int offs = y - Y;
                yMax += offs;
                yMin += offs;
            }
        }

        public override ItemTypes Type
        {
            get { return ItemTypes.Misc; }
        }

        public override MapleDrawableInfo BaseInfo
        {
            get { return baseInfo; }
        }

       

        public override void Draw(SpriteBatch sprite, XNA.Color color, int xShift, int yShift)
        {
            XNA.Rectangle destinationRectangle = new XNA.Rectangle((int)X + xShift - Origin.X, (int)Y + yShift - Origin.Y, Width, Height);
            sprite.Draw(baseInfo.GetTexture(sprite), destinationRectangle, null, color, 0f, new XNA.Vector2(0, 0), SpriteEffects.None, 0);
        }

        public override MapleLib.WzLib.Bitmap Image
        {
            get
            {
                return baseInfo.Image;
            }
        }

        public override int Width
        {
            get { return baseInfo.Width; }
        }

        public override int Height
        {
            get { return baseInfo.Height; }
        }

        public override MapleLib.WzLib.Point Origin
        {
            get
            {
                return baseInfo.Origin;
            }
        }

        public string Name
        {
            get
            {
                return "Special: Healer";
            }
        }

      

       

     
    }
}
