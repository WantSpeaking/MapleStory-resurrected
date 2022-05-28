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
    public class ToolTipInstance : MapleRectangle // Renamed to ToolTipInstance to avoid ambiguity with System.Windows.Forms.ToolTip
    {
        private string title;
        private string desc;
        private ToolTipChar ttc = null;
        private int originalNum;

        public ToolTipInstance(Board board, XNA.Rectangle rect, string title, string desc, int originalNum = -1)
            : base(board, rect)
        {
            this.title = title;
            this.desc = desc;
            this.originalNum = originalNum;
        }

        public override MapleDot CreateDot(int x, int y)
        {
            return new ToolTipDot(this, board, x, y);
        }

        public override MapleLine CreateLine(MapleDot a, MapleDot b)
        {
            return new ToolTipLine(board, a, b);
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }

        public ToolTipChar CharacterToolTip
        {
            get { return ttc; }
            set { ttc = value; }
        }

        public int OriginalNumber
        {
            get { return originalNum; }
        }

        public override XNA.Color Color
        {
            get
            {
                return Selected ? UserSettings.ToolTipSelectedFill : UserSettings.ToolTipFill;
            }
        }

        public override ItemTypes Type
        {
            get { return ItemTypes.ToolTips; }
        }

        public override void Draw(SpriteBatch sprite, XNA.Color dotColor, int xShift, int yShift)
        {
            base.Draw(sprite, dotColor, xShift, yShift);
            /*if (title != null)
            {
                Board.ParentControl.FontEngine.DrawString(sprite, new MapleLib.WzLib.Point(X + xShift + 2, Y + yShift + 2), Microsoft.Xna.Framework.Color.Black, title, Width);
            }
            if (desc != null)
            {
                int titleHeight = (int)Math.Ceiling(Board.ParentControl.FontEngine.MeasureString(title).Height);
                Board.ParentControl.FontEngine.DrawString(sprite, new MapleLib.WzLib.Point(X + xShift + 2, Y + yShift + 2 + titleHeight), Microsoft.Xna.Framework.Color.Black, desc, Width);
            }*/
        }

     
   
        public new class SerializationForm : MapleRectangle.SerializationForm
        {
            public string title, desc;
            public int originalnum;
        }

     
       

        protected void UpdateSerializedForm(SerializationForm result)
        {
            base.UpdateSerializedForm(result);
            result.title = title;
            result.desc = desc;
            result.originalnum = originalNum;
        }

        private const string TTC_KEY = "ttc";

        public ToolTipInstance(Board board, SerializationForm json)
            : base(board, json)
        {
            title = json.title;
            desc = json.desc;
            originalNum = json.originalnum;
        }
    }
}
