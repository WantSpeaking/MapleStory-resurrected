using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ms.ViewportAdapters
{
    public class ScalingViewportAdapter : ViewportAdapter
    {
        public ScalingViewportAdapter(GraphicsDevice graphicsDevice, int virtualWidth, int virtualHeight)
            : base(graphicsDevice)
        {
            _virtualWidth = virtualWidth;
            _virtualHeight = virtualHeight;
        }

        public override int ViewportWidth => Viewport.Width;
        public override int ViewportHeight => Viewport.Height;

        public override Matrix GetScaleMatrix()
        {
            var scaleX = (float)ViewportWidth / VirtualWidth;
            var scaleY = (float)ViewportHeight / VirtualHeight;
            return Matrix.CreateScale(scaleX, scaleY, 1.0f);
        }
    }
}