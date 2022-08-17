/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.Exceptions;
using HaCreator.MapEditor.Info;

using MapleLib.WzLib.WzStructure.Data;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNA = Microsoft.Xna.Framework;

namespace HaCreator.MapEditor.Instance
{
    public class TileInstance : LayeredItem
    {
        private TileInfo baseInfo;

        public TileInstance(TileInfo baseInfo, Layer layer, Board board, int x, int y, int z, int zM)
            : base(board, layer, zM, x, y, z)
        {
            this.baseInfo = baseInfo;
            AttachToLayer(layer);
        }

        public void AttachToLayer(Layer layer)
        {
            lock (board.ParentControl)
            {
                if (layer.tS != null && layer.tS != baseInfo.tS)
                {
                    Board.BoardItems.TileObjs.Remove(this);
                    layer.Items.Remove(this);
                    throw new Exception("tile added to a layer with different tS");
                }
                else layer.tS = baseInfo.tS;
            }
        }


        public override ItemTypes Type
        {
            get { return ItemTypes.Tiles; }
        }

        public override MapleDrawableInfo BaseInfo
        {
            get { return baseInfo; }
        }


       

        public override void InsertItem()
        {
            lock (board.ParentControl)
            {
                base.InsertItem();
                AttachToLayer(Layer);
            }
        }

        public override void Draw(SpriteBatch sprite, XNA.Color color, int xShift, int yShift)
        {
            XNA.Rectangle destinationRectangle = new XNA.Rectangle((int)X + xShift - Origin.X, (int)Y + yShift - Origin.Y, Width, Height);
            sprite.Draw(baseInfo.GetTexture(sprite), destinationRectangle, null, color, 0f, new XNA.Vector2(0f, 0f), /*Flip ? SpriteEffects.FlipHorizontally : */SpriteEffects.None, 0 /*Layer.LayerNumber / 10f + Z / 1000f*/);
            base.Draw(sprite, color, xShift, yShift);
        }

        // Only to be used by layer TS changing, do not use this for ANYTHING else.
        public void SetBaseInfo(TileInfo newInfo)
        {
            this.baseInfo = newInfo;
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

       
       
    }
}
