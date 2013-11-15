using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LiteEngine.Rendering;

namespace LiteEngine.Xna
{
    public class LiteXnaEngine : Game
    {
        XnaRenderer _renderer;
        GraphicsDeviceManager _graphics;

        public LiteXnaEngine()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _renderer = new XnaRenderer(_graphics, Content, Window);
        }

        protected override void Initialize()
        {
            _renderer.Initialize();
            base.Initialize();
        }

        protected XnaRenderer Renderer
        {
            get
            {
                return _renderer;
            }
        }
    }
}
