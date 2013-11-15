using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LiteEngine.Rendering;
using LiteEngine.Input;

namespace LiteEngine.Xna
{
    public abstract class LiteXnaEngine : Game
    {
        XnaRenderer _renderer;
        XnaKeyboardHandler _keyboardHandler;
        GraphicsDeviceManager _graphics;

        public LiteXnaEngine()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _renderer = new XnaRenderer(_graphics, Content, Window);
            _keyboardHandler = new XnaKeyboardHandler();
        }

        protected override void Initialize()
        {
            _renderer.Initialize();
            base.Initialize();
        }

        protected sealed override void Update(GameTime gameTime)
        {
            _keyboardHandler.Update();
            UpdateFrame(gameTime, _keyboardHandler);
            base.Update(gameTime);
        }

        protected abstract void UpdateFrame(GameTime gameTime, XnaKeyboardHandler keyboardHandler);

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
    }
}
