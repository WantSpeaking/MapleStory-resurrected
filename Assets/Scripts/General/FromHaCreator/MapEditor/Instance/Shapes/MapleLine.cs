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
    //it is important to remember that if the line is connecting mouse and a MapleDot, the mouse is ALWAYS the second dot.
    public abstract class MapleLine
    {
        private Board board;
        protected MapleDot firstDot;
        protected MapleDot secondDot;
        private bool beforeConnecting;
        private bool _xBind = false;
        private bool _yBind = false;

        public MapleLine(Board board, MapleDot firstDot)
        {
            this.board = board;
            this.firstDot = firstDot;
            this.firstDot.connectedLines.Add(this);
            //this.secondDot = board.Mouse;
            this.secondDot.connectedLines.Add(this);
            this.beforeConnecting = true;
            firstDot.PointMoved += OnFirstDotMoved;
        }

        protected MapleLine(Board board)
        {
            this.board = board;
            this.beforeConnecting = false;
        }

        public MapleLine(Board board, MapleDot firstDot, MapleDot secondDot)
        {
            this.board = board;
            this.firstDot = firstDot;
            this.firstDot.connectedLines.Add(this);
            this.secondDot = secondDot;
            this.secondDot.connectedLines.Add(this);
            this.beforeConnecting = false;
            firstDot.PointMoved += OnFirstDotMoved;
            secondDot.PointMoved += OnSecondDotMoved;
        }

        public void ConnectSecondDot(MapleDot secondDot)
        {
            if (!beforeConnecting) return;
            this.secondDot.connectedLines.Clear();
            this.secondDot = secondDot;
            this.secondDot.connectedLines.Add(this);
            secondDot.PointMoved += OnSecondDotMoved;
        }

        public double CalculateY(int x)
        {
            return ((double)(FirstDot.Y - SecondDot.Y) / (double)(FirstDot.X - SecondDot.X)) * (double)(x - FirstDot.X) + FirstDot.Y; // y-y1=m(x-x1) => y=(d/dx)(x-x1)+y1
        }

        public bool Selected { get { return firstDot != null && firstDot.Selected && secondDot != null && secondDot.Selected; } }

        public Board Board { get { return board; } set { board = value; } }

        public abstract XNA.Color Color { get; }
        public abstract XNA.Color InactiveColor { get; }
        public abstract ItemTypes Type { get; }

        public bool xBind
        {
            get { return _xBind; }
            set { _xBind = value; }
        }

        public bool yBind
        {
            get { return _yBind; }
            set { _yBind = value; }
        }

        public MapleDot FirstDot
        {
            get { return firstDot; }
            set { firstDot = value; }
        }

        public MapleDot SecondDot
        {
            get { return secondDot; }
            set { secondDot = value; }
        }

        public void OnFirstDotMoved()
        {
            if (xBind)
                secondDot.MoveSilent(firstDot.X, secondDot.Y);
            if (yBind)
                secondDot.MoveSilent(secondDot.X, firstDot.Y);
        }

        public void OnSecondDotMoved()
        {
            if (xBind)
                firstDot.MoveSilent(secondDot.X, firstDot.Y);
            if (yBind)
                firstDot.MoveSilent(firstDot.X, secondDot.Y);
        }

       

        public virtual void Draw(SpriteBatch sprite, XNA.Color color, int xShift, int yShift)
        {
            board.ParentControl.DrawLine(sprite, new XNA.Vector2(firstDot.X + xShift, firstDot.Y + yShift), new XNA.Vector2(secondDot.X + xShift, secondDot.Y + yShift), color);
        }

        public MapleDot GetOtherDot(MapleDot x)
        {
            if (firstDot == x)
                return secondDot;
            else if (secondDot == x)
                return firstDot;
            else
                throw new Exception("GetOtherDot: line is not properly connected");
        }
    }
}
