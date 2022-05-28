/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.MapEditor.Info;

using MapleLib.WzLib.WzStructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNA = Microsoft.Xna.Framework;

namespace HaCreator.MapEditor.Instance.Shapes
{
    public class RopeAnchor : MapleDot, IContainsLayerInfo
    {
        private Rope parentRope;

        public RopeAnchor(Board board, int x, int y, Rope parentRope)
            : base(board, x, y)
        {
            this.parentRope = parentRope;
        }

        public override XNA.Color Color
        {
            get
            {
                return UserSettings.RopeColor;
            }
        }

        public override XNA.Color InactiveColor
        {
            get { return MultiBoard.RopeInactiveColor; }
        }

        public override ItemTypes Type
        {
            get { return ItemTypes.Ropes; }
        }

        public int LayerNumber
        {
            get { return parentRope.LayerNumber; }
            set { parentRope.LayerNumber = value; }
        }
        public int PlatformNumber { get { return -1; } set { return; } }

        protected override bool RemoveConnectedLines
        {
            // This should never happen because RemoveItem is overridden to remove through parentRope
            get { throw new NotImplementedException(); }
        }


        public Rope ParentRope { get { return parentRope; } }

      

    }
}
