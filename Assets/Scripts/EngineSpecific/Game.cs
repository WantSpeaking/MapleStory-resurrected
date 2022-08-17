using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
//using MonoGame.Extended.Input.InputListeners;
using ms.ViewportAdapters;
//using MonoGame.Extended.ViewportAdapters;
using ms;
using Util;
using ms.Util;
using Spine;

namespace EndlessStory
{
    public class SampleTemplateGame : Game
    {
        public bool enableDebugPacket = true;
        public SpriteFont Font_Arial { get; set; }
        public GraphicsDeviceManager GraphicsManager => _graphics;
        public SpriteBatch Batch => _spriteBatch;

        /*public readonly TouchListener _touchListener;
        public readonly GamePadListener _gamePadListener;
        public readonly KeyboardListener _keyboardListener;
        public readonly MouseListener _mouseListener;
        */

        private MapleStory mapleStory;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public SkeletonMeshRenderer skeletonMeshRenderer;

        //public BoxingViewportAdapter _boxingViewportAdapter;
        //public ViewportAdapter _currentViewportAdapter => ms.Window.get().CurrentViewportAdapter;

        public SampleTemplateGame(string path_dataFolder)
        {
            GameUtil.get().Game = this;

            _graphics = new GraphicsDeviceManager(this);
            /*_graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.PreferMultiSampling = true;
            _graphics.SynchronizeWithVerticalRetrace = false;*/

            //_graphics


            /*_graphics.SynchronizeWithVerticalRetrace = false;
            base.IsFixedTimeStep = false;*/

            //TouchPanel.EnableMouseTouchPoint = true;



            /*_graphics.PreferredBackBufferWidth = 1136;
            _graphics.PreferredBackBufferHeight = 640;*/

            /*_graphics.PreferredBackBufferWidth = 800;
			_graphics.PreferredBackBufferHeight = 600;*/
            //_graphics.IsFullScreen = true;
            //_graphics.ApplyChanges();
            //ms.Window.get().ToFullscreen();

            Content.RootDirectory = "Content";

            IsMouseVisible = false;
            //IsFixedTimeStep = false;

            /*_keyboardListener = new KeyboardListener();
            _gamePadListener = new GamePadListener();
            _mouseListener = new MouseListener();
            _touchListener = new TouchListener();*/

            Constants.get().path_MapleStoryFolder = path_dataFolder;
            Constants.get().path_SettingFileFolder = path_dataFolder;
            mapleStory = new MapleStory();

            MessageCenter.get().ConstructGame(this);

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //Components.Add(new FairyGUI.Stage(this, null));

            //Components.Add(new InputListenerComponent(this, _keyboardListener, _gamePadListener, _mouseListener, _touchListener));

            MessageCenter.get().InitializeGame(this);


            base.Initialize();
        }

        protected override void LoadContent()
        {
            mapleStory.init();
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //FairyGUI.UIContentScaler.SetContentScaleFactor(1280, 720, FairyGUI.UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);

            // TODO: use this.Content to load your game content here
            Font_Arial = Content.Load<SpriteFont>("Fonts/Arial");

            //GraphicsGL.get().Font_Arial = Font_Arial;
            //GraphicsGL.get().Batch = _spriteBatch;
            MessageCenter.get().LoadContentGame(this);
			skeletonMeshRenderer = new SkeletonMeshRenderer (GraphicsDevice);
            /*_boxingViewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 600);
            AppDebug.Log($"bound before:{Window.ClientBounds}\t {GraphicsDevice.Viewport.Bounds}");
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            AppDebug.Log($"bound after:{Window.ClientBounds}\t {GraphicsDevice.Viewport.Bounds}");*/
            base.LoadContent();
        }

        private long timestep => (long)(Constants.TIMESTEP * multiplier_timeStep);

        //private static long timestep = (long)(8 * 1000 * 1);
        private long accumulator = 0;
        protected override void Update(GameTime gameTime)
        {
            /*        foreach (var touchState in TouchPanel.GetState())
                    {
                        AppDebug.Log($"{touchState.State}");

                    }*/
            /*if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Keys.Escape))
                 Exit();*/
            // TODO: Add your update logic here
            if (mapleStory.running())
            {
                var elapsed = Timer.get().stop();

                var accumulator_before = accumulator;
                var accumulator_plus_elapsed = accumulator + elapsed;
                // Update game with constant timestep as many times as possible.
                for (accumulator += elapsed; accumulator >= timestep; accumulator -= timestep)
                {
                    mapleStory.update();
                    //AppDebug.Log ("update");bt`
                }
            }
            else
            {
                Exit();
            }

            //AppDebug.Log ($"fps:{1/gameTime.ElapsedGameTime.TotalSeconds}");
            base.Update(gameTime);
            MessageCenter.get().UpdateGame(this);
            //AppDebug.Log($"{_graphics.PreferredBackBufferWidth}\t {_graphics.PreferredBackBufferHeight}");
        }


        public float multiplier_timeStep = 1f;
        public float multiplier_elapsed = 1f;
        private double accumulator_draw;
        private double timestep_draw = 18;
        public GameTime gameTime;
        protected override void Draw(GameTime gameTime)
        {
	        this.gameTime = gameTime;
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);

            //FairyGUI.Stage.inst.Batch.SpriteBatch.Begin();

            _spriteBatch.Begin(/*transformMatrix: _currentViewportAdapter.GetScaleMatrix()*/);
            // TODO: Add your drawing code here
            if (mapleStory.running())
            {
                var elapsed = Timer.get().stop();

                var accumulator_before = accumulator;
                var accumulator_plus_elapsed = accumulator + elapsed;
                // Update game with constant timestep as many times as possible.
                for (accumulator_draw += gameTime.ElapsedGameTime.TotalMilliseconds; accumulator_draw >= timestep_draw; accumulator_draw -= timestep_draw)
                {
                    
                }
                // Draw the game. Interpolate to account for remaining time.
                float alpha = MathUtil.Clamp(accumulator * multiplier_elapsed / timestep, 0, 1);
                //AppDebug.Log ($"elapsed:{elapsed} \t timestep:{timestep} \t accumulator:{accumulator} \t alpha:{alpha}");
                mapleStory.draw(alpha);
                //AppDebug.Log ($"deltaTime:{Time.deltaTime * 1000}\t accumulator_before:{accumulator_before}\t elapsed:{elapsed}\t  accumulator_plus_elapsed:{accumulator_plus_elapsed}\t accumulator:{accumulator}\t  alpha:{alpha}");
            }
            //GraphicsGL.Instance.DrawWireRectangle(100, 100, 258, 41, Microsoft.Xna.Framework.Color.Purple, 1);
            //GraphicsGL.Instance.DrawWireRectangle(0, 0, 100, 100, Microsoft.Xna.Framework.Color.Purple, 1);
            _spriteBatch.DrawString(Font_Arial, $"{(1 / gameTime.ElapsedGameTime.TotalSeconds):F0}", Vector2.Zero, Microsoft.Xna.Framework.Color.Yellow, 0, Vector2.Zero, 3, SpriteEffects.None, 0);

            _spriteBatch.End();
            //FairyGUI.Stage.inst.Batch.SpriteBatch.End();
            base.Draw(gameTime);
            MessageCenter.get().DrawGame(this);
            
        }
    }
}
