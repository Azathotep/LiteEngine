using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using LiteEngine.Textures;
using LiteEngine.Xna;
using LiteEngine.Math;
using LiteEngine.Input;
using LiteGame.UI;

namespace LiteGame
{
    class Engine : LiteXnaEngine
    {
        PauseMenu _menu = new PauseMenu();        
        Ship _ship = new Ship();
        Texture _shipTexture;
        public Engine()
        {
            _shipTexture = new Texture("rocketship");
            Renderer.SetDeviceMode(800, 600, true);
            Renderer.Camera.SetViewField(40, 30);
            Renderer.Camera.LookAt(15, 10);
            _ship.Position = new Vector2(15, 10);
        }

        protected override void DrawFrame(GameTime gameTime)
        {
            Renderer.Clear(Color.LightBlue);
            float facingAngle = (float)Math.Atan2(_ship.Facing.X, -_ship.Facing.Y);
            Renderer.BeginDraw();
            Renderer.DrawSprite(_shipTexture, new RectangleF(_ship.Position.X, _ship.Position.Y, 2, 2), facingAngle);
            Renderer.EndDraw();
        }

        protected override int OnKeyPress(Keys key)
        {
            switch (key)
            {
                case Keys.Left:
                    _ship.Rotate(-0.1f);
                    return 0;
                case Keys.Right:
                    _ship.Rotate(0.1f);
                    return 10;
                case Keys.Up:
                    _ship.ApplyThrust(0.01f);
                    return 0;
                case Keys.M:
                    UIManager.ShowDialog(_menu);
                    return -1;
            }
            return base.OnKeyPress(key);
        }

        protected override void UpdateFrame(GameTime gameTime, XnaKeyboardHandler keyHandler)
        {
                
            if (keyHandler.IsKeyDown(Keys.Escape))
                Exit();
            _ship.Update();
        }
    }
}
