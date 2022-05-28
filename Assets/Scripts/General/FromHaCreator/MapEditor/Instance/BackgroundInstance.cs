/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.MapEditor.Info;
using MapleLib.WzLib.WzStructure.Data;
using XNA = Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapleLib.WzLib;
using ms;

namespace HaCreator.MapEditor.Instance
{
    public class BackgroundInstance : BoardItem, IFlippable
    {
        private readonly BackgroundInfo baseInfo;
        private bool flip;
        private int _a; //alpha
        private int _cx; //copy x
        private int _cy; //copy y
        private int _rx;
        private int _ry;
        private bool _front;
        private int _screenMode;
        private string _spineAni;
        private bool _spineRandomStart;
        private BackgroundType _type;

        public BackgroundInstance(BackgroundInfo baseInfo, Board board, int x, int y, int z, int rx, int ry, int cx, int cy, BackgroundType type, int a, bool front, bool flip, int _screenMode, 
            string _spineAni, bool _spineRandomStart)
            : base(board, x, y, z)
        {
            this.baseInfo = baseInfo;
            this.flip = flip;
            this._rx = rx;
            this._ry = ry;
            this._cx = cx;
            this._cy = cy;
            this._a = a;
            this._type = type;
            this._front = front;
            this._screenMode = _screenMode;
            this._spineAni = _spineAni;
            this._spineRandomStart = _spineRandomStart;

            if (flip)
                BaseX -= Width - 2 * Origin.X;
        }

        public override ItemTypes Type
        {
            get { return ItemTypes.Backgrounds; }
        }

        public bool Flip
        {
            get
            {
                return flip;
            }
            set
            {
                if (flip == value) return;
                flip = value;
                int xFlipShift = Width - 2 * Origin.X;
                if (flip) BaseX -= xFlipShift;
                else BaseX += xFlipShift;
            }
        }

        public int UnflippedX
        {
            get
            {
                return flip ? (BaseX + Width - 2 * Origin.X) : BaseX;
            }
        }

        public override void Draw(SpriteBatch sprite, XNA.Color color, int xShift, int yShift)
        {
            if (sprite == null || baseInfo.GetTexture(sprite)==null)
                return;

            XNA.Rectangle destinationRectangle = new XNA.Rectangle((int)X + xShift - Origin.X, (int)Y + yShift - Origin.Y, Width, Height);
            sprite.Draw(baseInfo.GetTexture(sprite), destinationRectangle, null, color, 0f, new XNA.Vector2(0f, 0f), Flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1);
            
            base.Draw(sprite, color, xShift, yShift);
        }

        public override MapleDrawableInfo BaseInfo
        {
            get { return baseInfo; }
        }

        public override Bitmap Image
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

        // Parallax + Undo\Redo is troublesome. I don't like this way either.
        public int BaseX { get { return (int)base.position.X; } set { base.position.X = value; } }
        public int BaseY { get { return (int)base.position.Y; } set { base.position.Y = value; } }

        public int rx
        {
            get { return _rx; }
            set { _rx = value; }
        }

        public int ry
        {
            get { return _ry; }
            set { _ry = value; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public int cx
        {
            get { return _cx; }
            set { _cx = value; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public int cy
        {
            get { return _cy; }
            set { _cy = value; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public int a
        {
            get { return _a; }
            set { _a = value; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public BackgroundType type
        {
            get { return _type; }
            set { _type = value; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public bool front
        {
            get { return _front; }
            set { _front = value; }
        }

        /// <summary>
        /// The screen resolution to display this background object. (0 = all res)
        /// </summary>
        public int screenMode
        {
            get { return _screenMode; }
            set { _screenMode = value; }
        }

        /// <summary>
        /// Spine animation path 
        /// </summary>
        public string SpineAni
        {
            get { return _spineAni; }
            set { this._spineAni = value; }
        }

        public bool SpineRandomStart
        {
            get { return _spineRandomStart; }
            set { this._spineRandomStart = value; }
        }

        public int CalculateBackgroundPosX()
        {
            //double dpi = ScreenDPIUtil.GetScreenScaleFactor(); // dpi affected via window.. does not have to be calculated manually
            double dpi = 1;
            int width = (int)((Constants.get ().get_viewwidth () / 2) / dpi);// 400;

            return (rx * (Board.hScroll - Board.CenterPoint.X + width) / 100) + base.X /*- Origin.X*/ + width - Board.CenterPoint.X + Board.hScroll;
        }

        public int CalculateBackgroundPosY()
        {
            //double dpi = ScreenDPIUtil.GetScreenScaleFactor(); // dpi affected via window.. does not have to be calculated manually
            double dpi = 1;
            int height = (int) ((Constants.get ().get_viewheight () / 2) / dpi);// 300;

            return (ry * (Board.vScroll - Board.CenterPoint.Y + height) / 100) + base.Y /*- Origin.X*/ + height - Board.CenterPoint.Y + Board.vScroll;
        }


        public override int X
        {
            get
            {
                if (UserSettings.emulateParallax)
                    return CalculateBackgroundPosX();
                else 
                    return base.X;
            }
            set
            {
                int newX;
                /*if (UserSettings.emulateParallax)
                    newX = ReverseBackgroundPosX(value);
                else 
                    newX = value;*/
                newX = value;
                base.Move(newX, base.Y);
            }
        }

        public override int Y
        {
            get
            {
                if (UserSettings.emulateParallax)
                    return CalculateBackgroundPosY();
                else return base.Y;
            }
            set
            {
                int newY;
                /*if (UserSettings.emulateParallax)
                    newY = ReverseBackgroundPosY(value);
                else 
                    newY = value;*/
                newY = value;
                base.Move(base.X, newY);
            }
        }

        public override void Move(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void MoveBase(int x, int y)
        {
            this.BaseX = x;
            this.BaseY = y;
        }


    }
}
