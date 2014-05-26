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
using LiteEngine.Particles;
using Microsoft.Xna.Framework.Audio;
using LiteEngine.Textures;
using LiteEngine.Math;

namespace LiteEngine.Xna
{
    public abstract class LiteXnaEngine : Game
    {
        PhysicsCore _physics;
        XnaRenderer _renderer;
        XnaKeyboardHandler _keyboardHandler;
        MouseHandler _mouseHandler;
        GraphicsDeviceManager _graphics;
        UserInterface _ui;
        ParticleSystem _particleSystem;

        public LiteXnaEngine()
        {
            _physics = new PhysicsCore();
            _particleSystem = new ParticleSystem(_physics);
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _renderer = new XnaRenderer(_graphics, Content, Window);
            _keyboardHandler = new XnaKeyboardHandler();
            _keyboardHandler.OnKeyPressed += _keyboardHandler_OnKeyPressed;
            _mouseHandler = new MouseHandler();
            _mouseHandler.OnMouseClick += _mouseHandler_OnMouseClick;
        }

        public SizeF ScreenSize
        {
            get
            {
                return new SizeF(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            }
        }

        public Vector2 CenterScreen
        {
            get
            {
                return ScreenSize * 0.5f;
            }
        }

        int _keyboardHandler_OnKeyPressed(Keys key, GameTime gameTime)
        {
            int repressDelay = 0;
            if (_ui.ProcessKey(key, out repressDelay))
                return repressDelay;
            return OnKeyPress(key, gameTime);
        }

        int _mouseHandler_OnMouseClick(MouseButton button, Point mousePosition)
        {
            float x = (float)mousePosition.X / ScreenSize.Width;
            float y = (float)mousePosition.Y / ScreenSize.Height;
            Vector2 position = new Vector2(x * 2 - 1, (1 - y) * 2 - 1);

            bool handled = _ui.ProcessMouseClick(button, position);

            OnMouseClick(button, position);
            return 0;
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

        public ParticleSystem ParticleSystem
        {
            get
            {
                return _particleSystem;
            }
        }

        protected abstract int OnKeyPress(Keys key, GameTime gameTime);
        /// <summary>
        /// Called when the mouse is clicked
        /// </summary>
        /// <param name="button">button that was clicked</param>
        /// <param name="mousePosition">position of the mouse on the screen in the range (x,y) = (-1..1, -1..1)</param>
        protected virtual void OnMouseClick(MouseButton button, Vector2 mousePosition)
        {

        }

        public XnaRenderer Renderer
        {
            get
            {
                return _renderer;
            }
        }

        protected sealed override void Initialize()
        {
            _renderer.Initialize();
            //todo - need to know the screen size to initialize the UserInterface, but the user doesn't specify it until
            //the Initialize() method, and that needs the UserInterface to be created already in case they want to add controls
            _ui = new UserInterface(this);
            Initialize(_renderer);
            base.Initialize();
        }

        protected abstract void Initialize(XnaRenderer renderer);

        protected sealed override void Update(GameTime gameTime)
        {
            _keyboardHandler.Update(gameTime);
            _mouseHandler.Update();
            _physics.Update(System.Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
            _particleSystem.Update();
            UpdateFrame(gameTime);
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

        int _numFrames;
        TimeSpan _frameTime;
        int _frameRate;

        /// <summary>
        /// Returns the framerate in FPS
        /// </summary>
        public int FrameRate
        {
            get
            {
                return _frameRate;
            }
        }

        protected sealed override void Draw(GameTime gameTime)
        {
            _numFrames++;
            _frameTime += gameTime.ElapsedGameTime;
            while (_frameTime > TimeSpan.FromSeconds(1))
            {
                _frameTime -= TimeSpan.FromSeconds(1);
                _frameRate = _numFrames;
                _numFrames = 0;
            }

            if (_autoClear)
                _renderer.Clear(Color.Black);
            DrawFrame(gameTime, _renderer);
            _ui.RenderUI(gameTime, _renderer);
            base.Draw(gameTime);
        }

        bool _autoClear = true;
        public bool AutoClear
        {
            get
            {
                return _autoClear;
            }
            set
            {
                _autoClear = value;
            }
        }

        protected abstract void DrawFrame(GameTime gameTime, XnaRenderer renderer);
        protected abstract void UpdateFrame(GameTime gameTime);

        public XnaKeyboardHandler KeyboardHandler
        {
            get
            {
                return _keyboardHandler;
            }
        }

        public UserInterface UserInterface
        {
            get
            {
                return _ui;
            }
        }

        /// <summary>
        /// Returns the position of the mouse on the screen in the range (x,y) = (-1..1, -1..1)
        /// </summary>
        public Vector2 GetMousePosition()
        {
            float x = (float)Mouse.GetState().Position.X / ScreenSize.Width;
            float y = (float)Mouse.GetState().Position.Y / ScreenSize.Height;
            return new Vector2(x * 2 - 1, (1 - y) * 2 - 1);
        }

        public SoundEffect GetSoundEffect(string soundName)
        {
            SoundEffect sound = Content.Load<SoundEffect>(soundName);
            return sound;
        }
    }
}
