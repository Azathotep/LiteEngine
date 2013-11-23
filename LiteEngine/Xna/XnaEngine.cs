using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using LiteEngine.Rendering;
using LiteEngine.Input;
using LiteEngine.UI;

namespace LiteEngine.Xna
{
    public abstract class LiteXnaEngine : Game
    {
        XnaRenderer _renderer;
        XnaKeyboardHandler _keyboardHandler;
        GraphicsDeviceManager _graphics;
        UIManager _ui;

        public LiteXnaEngine()
        {
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
            UpdateFrame(gameTime, _keyboardHandler);
            base.Update(gameTime);
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
