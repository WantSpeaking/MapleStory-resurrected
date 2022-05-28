using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// ReSharper disable once CheckNamespace

namespace ms.ViewportAdapters
{
    public enum BoxingMode
    {
        None,
        Letterbox,
        Pillarbox
    }

    public class BoxingViewportAdapter : ScalingViewportAdapter
    {
        private readonly GameWindow _window;
        private readonly GraphicsDevice _graphicsDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxingViewportAdapter" />.
        /// </summary>
        public BoxingViewportAdapter(GameWindow window, GraphicsDevice graphicsDevice, int virtualWidth, int virtualHeight, int horizontalBleed = 0, int verticalBleed = 0)
            : base(graphicsDevice, virtualWidth, virtualHeight)
        {
            _window = window;
            _graphicsDevice = graphicsDevice;
            window.ClientSizeChanged += OnClientSizeChanged;
            HorizontalBleed = horizontalBleed;
            VerticalBleed = verticalBleed;
            //OnClientSizeChanged(this, EventArgs.Empty);
        }

        public override void Dispose()
        {
            _window.ClientSizeChanged -= OnClientSizeChanged;
            base.Dispose();
        }

        /// <summary>
        ///     Size of horizontal bleed areas (from left and right edges) which can be safely cut off
        /// </summary>
        public int HorizontalBleed { get; }

        /// <summary>
        ///     Size of vertical bleed areas (from top and bottom edges) which can be safely cut off
        /// </summary>
        public int VerticalBleed { get; }

        public BoxingMode BoxingMode { get; private set; }

        /*        private int _Width_Viewport;
                public int Width_Viewport
                {
                    get => _Width_Viewport;
                    set
                    {
                        if (_Width_Viewport != value)
                        {
                            _Width_Viewport = value;
                            var viewport = GraphicsDevice.Viewport;
                            viewport.Width = _Width_Viewport;
                            GraphicsDevice.Viewport = viewport;
                        }
                    }
                }

                private int _Height_Viewport;
                public int Height_Viewport
                {
                    get => _Height_Viewport;
                    set
                    {
                        if (_Height_Viewport != value)
                        {
                            _Height_Viewport = value;
                            var viewport = GraphicsDevice.Viewport;
                            viewport.Height = _Height_Viewport;
                            GraphicsDevice.Viewport = viewport;
                        }
                    }
                }*/

       

        public void OnClientSizeChanged(object sender, EventArgs eventArgs)
        {
            var clientBounds = _window.ClientBounds;

            var worldScaleX = (float)clientBounds.Width / VirtualWidth;
            var worldScaleY = (float)clientBounds.Height / VirtualHeight;

            var safeScaleX = (float)clientBounds.Width / (VirtualWidth - HorizontalBleed);
            var safeScaleY = (float)clientBounds.Height / (VirtualHeight - VerticalBleed);

            var worldScale = MathHelper.Max(worldScaleX, worldScaleY);
            var safeScale = MathHelper.Min(safeScaleX, safeScaleY);
            var scale = MathHelper.Min(worldScale, safeScale);

            var width = (int)(scale * VirtualWidth + 0.5f);
            var height = (int)(scale * VirtualHeight + 0.5f);

            if (height >= clientBounds.Height && width < clientBounds.Width)
                BoxingMode = BoxingMode.Pillarbox;
            else
            {
                if (width >= clientBounds.Height && height <= clientBounds.Height)
                    BoxingMode = BoxingMode.Letterbox;
               else
                    BoxingMode = BoxingMode.None;
            }

            var x = clientBounds.Width / 2 - width / 2;
            var y = clientBounds.Height / 2 - height / 2;
            Viewport = new Viewport(x, y, width, height);
            //GraphicsDevice.Viewport = new Viewport(x, y, Width, height);
        }

        public override void Reset()
        {
            base.Reset();
            OnClientSizeChanged(this, EventArgs.Empty);
        }

        public override void OnVirtualWidthChanged()
        {
            OnClientSizeChanged(this, EventArgs.Empty);
        }

        public override void OnVirtualHeightChanged()
        {
            OnClientSizeChanged(this, EventArgs.Empty);
        }
      
        public override Point PointToScreen(int x, int y)
        {
            var viewport = Viewport;
            return base.PointToScreen(x - viewport.X, y - viewport.Y);
        }

        public override Point ScreenToPoint(int x, int y)
        {
            var viewport = Viewport;
            //return base.ScreenToPoint(x + viewport.X, y + viewport.Y);
            return base.ScreenToPoint(x  , y  ) + new Point(viewport.X, viewport.Y);
        }

        public override Point Size_PhysicsToVirtual(int x, int y)
        {
            return base.PointToScreen(x, y);
        }

        public override Point Size_VirtualToPhysics(int x, int y)
        {
            return base.ScreenToPoint(x, y);
        }

     /*   public override Matrix GetScaleMatrix()
        {
            var scale = MathHelper.Min((float)ViewportWidth / VirtualWidth, (float)ViewportHeight / VirtualHeight);
            return Matrix.CreateScale(scale, scale, 1.0f);
        }*/
    }
}