using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using ms.ViewportAdapters;
using EndlessStory;

namespace ms
{
    public class GameUtil : Singleton<GameUtil>
    {
        public SampleTemplateGame Game { get; set; }

        public GraphicsDeviceManager GraphicsManager => Game.GraphicsManager;
        public GraphicsDevice GraphicsDevice => Game.GraphicsDevice;
        public GameWindow GameWindow => Game.Window;
        public SpriteBatch Batch => Game.Batch;

        public bool enableDebugPacket = true;

        public int DrawOrder { get; set; }
    }
}