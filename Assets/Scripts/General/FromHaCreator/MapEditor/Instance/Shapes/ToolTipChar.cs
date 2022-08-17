/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */


using MapleLib.WzLib.WzStructure.Data;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNA = Microsoft.Xna.Framework;

namespace HaCreator.MapEditor.Instance.Shapes
{
    /// <summary>
    /// The boundary, that shows the tooltip when the player gets close to it.
    /// </summary>
    public class ToolTipChar : MapleRectangle
    {
        private ToolTipInstance boundTooltip;

        public ToolTipChar(Board board, XNA.Rectangle rect, ToolTipInstance boundTooltip)
            : base(board, rect)
        {
            BoundTooltip = boundTooltip;
        }

        public override MapleDot CreateDot(int x, int y)
        {
            return new ToolTipDot(this, board, x, y);
        }

        public override MapleLine CreateLine(MapleDot a, MapleDot b)
        {
            return new ToolTipLine(board, a, b);
        }

        public ToolTipInstance BoundTooltip
        {
            get { return boundTooltip; }
            set { boundTooltip = value; if (value != null) value.CharacterToolTip = this; }
        }

        public override XNA.Color Color
        {
            get
            {
                return Selected ? UserSettings.ToolTipCharSelectedFill : UserSettings.ToolTipCharFill;
            }
        }

        public override ItemTypes Type
        {
            get { return ItemTypes.ToolTips; }
        }

        public override void Draw(SpriteBatch sprite, XNA.Color dotColor, int xShift, int yShift)
        {
            base.Draw(sprite, dotColor, xShift, yShift);

            if (boundTooltip != null) 
                Board.ParentControl.DrawLine(sprite, new XNA.Vector2(X + Width / 2 + xShift, Y + Height / 2 + yShift), new XNA.Vector2(boundTooltip.X + boundTooltip.Width / 2 + xShift, boundTooltip.Y + boundTooltip.Height / 2 + yShift), UserSettings.ToolTipBindingLine);
        }

      



    }
}
