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

namespace LiteGame
{
    class Engine : LiteXnaEngine
    {
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

        protected override void Draw(GameTime gameTime)
        {
            Renderer.Clear(Color.LightBlue);

            Matrix world = Matrix.Identity;
            Matrix projection = Matrix.CreateOrthographic(50, 30, -1000.5f, 100);
            Vector2 lookAt = new Vector2(20, 20);
            Matrix view = Matrix.CreateLookAt(new Vector3(lookAt, -1), new Vector3(lookAt, 0), new Vector3(0, -1, 0));

            float facingAngle = (float)Math.Atan2(_ship.Facing.X, -_ship.Facing.Y);
            Renderer.BeginDraw();
            Renderer.DrawSprite(_shipTexture, new RectangleF(_ship.Position.X, _ship.Position.Y, 2, 2), facingAngle);
            Renderer.EndDraw();
        }

        protected override void UpdateFrame(GameTime gameTime, XnaKeyboardHandler keyHandler)
        {
            if (keyHandler.IsKeyDown(Keys.Right))
                _ship.Rotate(0.1f);
            if (keyHandler.IsKeyDown(Keys.Left))
                _ship.Rotate(-0.1f);
            if (keyHandler.IsKeyDown(Keys.Up))
                _ship.ApplyThrust(0.01f);
            if (keyHandler.IsKeyDown(Keys.Escape))
                Exit();
            _ship.Update();
        }
    }
}
