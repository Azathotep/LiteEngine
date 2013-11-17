using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using LiteEngine.UI;
using LiteEngine.Math;
using LiteEngine.Rendering;
using LiteEngine.Textures;

namespace LiteGame.UI
{
    class PauseMenu : Dialog
    {
        Texture _texture = new Texture("pausemenu");
        public override void Render(XnaRenderer renderer)
        {
            renderer.DrawSprite(_texture, new RectangleF(renderer.ScreenWidth / 2, renderer.ScreenHeight / 2, 400, 300), 0);
        }

        public override int ProcessKey(Keys key)
        {
            switch (key)
            {
                case Keys.M:
                    Close();
                    return -1;
            }
            return 100;
        }

        public override bool KeyboardFocus
        {
            get
            {
                return true;
            }
        }
    }
}
