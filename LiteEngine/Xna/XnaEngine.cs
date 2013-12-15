using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using LiteEngine.Rendering;
using LiteEngine.Input;
using LiteEngine.UI;
using LiteEngine.Physics;

namespace LiteEngine.Xna
{
    public abstract class LiteXnaEngine : Game
    {
        PhysicsCore _physics;
        XnaRenderer _renderer;
        XnaKeyboardHandler _keyboardHandler;
        GraphicsDeviceManager _graphics;
        UIManager _ui;

        public LiteXnaEngine()
        {
            _physics = new PhysicsCore();
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _renderer = new XnaRenderer(_graphics, Content, Window);
            _keyboardHandler = new XnaKeyboardHandler();
            _keyboardHandler.OnKeyPressed += _keyboardHandler_OnKeyPressed;
            _ui = new UIManager(this);
        }

        int _keyboardHandler_OnKeyPressed(Keys key, GameTime gameTime)
        {
            int repressDelay = 0;
            if (_ui.ProcessKey(key, out repressDelay))
                return repressDelay;
            return OnKeyPress(key, gameTime);
        }

        /// <summary>
        /// Returns an interface to the physics engine
        /// </summary>
        public PhysicsCore Physics
        {
            get
            {
                return _physics;
            }
        }

        protected virtual int OnKeyPress(Keys key, GameTime gameTime)
        {
            return 0;
        }

        protected override void Initialize()
        {
            _renderer.Initialize();
            base.Initialize();
        }

        protected sealed override void Update(GameTime gameTime)
        {
            _keyboardHandler.Update(gameTime);
            _physics.Update(System.Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
            UpdateFrame(gameTime, _keyboardHandler);
            base.Update(gameTime);
        }

        bool _physicsEnabled = true;
        /// <summary>
        /// If true physics updates are made by the engine each update frame
        /// </summary>
        public bool PhysicsEnabled
        {
            get
            {
                return _physicsEnabled;
            }
            set
            {
                _physicsEnabled = value;
            }
        }

        protected sealed override void Draw(GameTime gameTime)
        {
            DrawFrame(gameTime);
            _ui.RenderUI(_renderer);
            base.Draw(gameTime);
        }

        protected abstract void DrawFrame(GameTime gameTime);
        protected abstract void UpdateFrame(GameTime gameTime, XnaKeyboardHandler keyHandler);

        protected XnaRenderer Renderer
        {
            get
            {
                return _renderer;
            }
        }

        public XnaKeyboardHandler KeyboardHandler
        {
            get
            {
                return _keyboardHandler;
            }
        }

        public UIManager UIManager
        {
            get
            {
                return _ui;
            }
        }
    }
}
