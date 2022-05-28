using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ms.ViewportAdapters
{
    public abstract class ViewportAdapter : IDisposable
    {
        protected ViewportAdapter(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            viewport = GraphicsDevice.Viewport;
        }

        public virtual void Dispose()
        {
        }

        public GraphicsDevice GraphicsDevice { get; }
        //public Viewport Viewport => GraphicsDevice.Viewport;
        private Viewport viewport;
        public Viewport Viewport
        {
            get => viewport;
            set
            {
                viewport = value;
                GraphicsDevice.Viewport = value;
            }
        }
        protected int _virtualWidth;
        protected int _virtualHeight;

        public virtual int VirtualWidth { get => _virtualWidth; set { _virtualWidth = value; OnVirtualWidthChanged(); } }
        public virtual int VirtualHeight { get => _virtualHeight; set { _virtualHeight = value; OnVirtualHeightChanged(); } }
        public abstract int ViewportWidth { get; }
        public abstract int ViewportHeight { get; }

        public Rectangle BoundingRectangle => new Rectangle(0, 0, VirtualWidth, VirtualHeight);
        public Point Center => BoundingRectangle.Center;
        public abstract Matrix GetScaleMatrix();

        public Point PointToScreen(Point point)
        {
            return PointToScreen(point.X, point.Y);
        }

        public virtual Point PointToScreen(int x, int y)
        {
            var scaleMatrix = GetScaleMatrix();
            var invertedMatrix = Matrix.Invert(scaleMatrix);
            return Vector2.Transform(new Vector2(x, y), invertedMatrix).ToPoint();
        }

        public virtual Point ScreenToPoint(int x, int y)
        {
            var scaleMatrix = GetScaleMatrix();
            return Vector2.Transform(new Vector2(x, y), scaleMatrix).ToPoint();
        }


        public virtual Point Size_PhysicsToVirtual(int x, int y)
        {
            return PointToScreen(x, y);
        }

        public virtual Point Size_VirtualToPhysics(int x, int y)
        {
            return ScreenToPoint(x, y);
        }

        public virtual void Reset()
        {
        }

        public virtual void OnVirtualWidthChanged()
        {

        }

        public virtual void OnVirtualHeightChanged()
        {

        }
    }
}