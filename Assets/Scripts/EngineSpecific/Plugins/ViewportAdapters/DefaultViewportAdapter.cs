using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// ReSharper disable once CheckNamespace

namespace ms.ViewportAdapters
{
    public class DefaultViewportAdapter : ViewportAdapter
    {
        private readonly GraphicsDevice _graphicsDevice;

        public DefaultViewportAdapter(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        public override int VirtualWidth { get => _graphicsDevice.Viewport.Width; set { } }
        public override int VirtualHeight { get => _graphicsDevice.Viewport.Height; set { } }
        public override int ViewportWidth => _graphicsDevice.Viewport.Width;
        public override int ViewportHeight => _graphicsDevice.Viewport.Height;

        public override Matrix GetScaleMatrix()
        {
            return Matrix.Identity;
        }
    }
}