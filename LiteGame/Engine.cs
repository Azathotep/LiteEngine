using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LiteEngine.Textures;
using LiteEngine.Xna;
using LiteEngine.Math;
using LiteEngine.Input;

namespace LiteGame
{
    class Engine : LiteXnaEngine
    {
        Texture _shipTexture;
        public Engine()
        {
            Renderer.SetDeviceMode(800, 600, true);
            _shipTexture = new Texture("rocketship");
        }

        float _shipAngle=0;

        protected override void Draw(GameTime gameTime)
        {
            Renderer.Clear(Color.White);

            Matrix world = Matrix.Identity;
            Matrix projection = Matrix.CreateOrthographic(50, 30, -1000.5f, 100);
            Vector2 lookAt = new Vector2(20, 20);
            Matrix view = Matrix.CreateLookAt(new Vector3(lookAt, -1), new Vector3(lookAt, 0), new Vector3(0, -1, 0));

            Renderer.BeginDraw(world, projection, view);
            Renderer.DrawSprite(_shipTexture, new RectangleF(11, 11, 5, 5), _shipAngle);
            Renderer.EndDraw();
        }

        protected override void UpdateFrame(GameTime gameTime, XnaKeyboardHandler keyboardHandler)
        {
            if (keyboardHandler.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
 	            _shipAngle += 0.05f;
            if (keyboardHandler.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                _shipAngle -= 0.05f;
            if (keyboardHandler.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Exit();
        }
    }
}
