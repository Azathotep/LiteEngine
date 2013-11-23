using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using LiteEngine.Math;
using LiteEngine.Textures;

namespace LiteEngine.Rendering
{
    public class XnaRenderer
    {
        GraphicsDeviceManager _deviceManager;
        ContentManager _contentManager;
        SpriteBatch _spriteBatch;
        GameWindow _window;

        public XnaRenderer(GraphicsDeviceManager deviceManager, ContentManager contentManager, GameWindow window)
        {
            _deviceManager = deviceManager;
            _contentManager = contentManager;
            _window = window;
            DrawDepth = 1;
        }

        public void SetDeviceMode(int width, int height, bool fullscreen)
        {
            //fullscreen in windows is not implemented in monogame
            //_deviceManager.IsFullScreen = fullscreen
            //alternative is to use a borderless window and resize
            //downside is that the resolution of the fullscreen window is fixed to the screen resolution
            //and cannot be changed
            if (fullscreen)
            {
                _window.IsBorderless = true;
                _deviceManager.PreferredBackBufferWidth = Screen.PrimaryScreen.Bounds.Width;
                _deviceManager.PreferredBackBufferHeight = Screen.PrimaryScreen.Bounds.Height;
                _window.Position = Point.Zero;
                _deviceManager.IsFullScreen = true;
            }
            else
            {
                _window.IsBorderless = false;
                //Strange, when NOT running under the debugger changing the Position of the Window causes the OnClientSizeChanged
                //event to be raised which monogame catches and resets the backbufferwidth and height. So need to set position here first.
                _window.Position = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - width / 2,
                                             Screen.PrimaryScreen.Bounds.Height / 2 - height / 2);
                _deviceManager.PreferredBackBufferWidth = width;
                _deviceManager.PreferredBackBufferHeight = height;
            }
            _deviceManager.ApplyChanges();
        }

        public float ScreenWidth
        {
            get
            {
                return _deviceManager.PreferredBackBufferWidth;
            }
        }

        public float ScreenHeight
        {
            get
            {
                return _deviceManager.PreferredBackBufferHeight;
            }
        }

        Camera _camera = new Camera();
        public Camera Camera
        {
            get
            {
                return _camera;
            }
        }

        public void Clear(Color color)
        {
            _deviceManager.GraphicsDevice.Clear(ClearOptions.DepthBuffer | ClearOptions.Target, color, 1, 0);
            _deviceManager.GraphicsDevice.SetRenderTarget(null);
        }

        public void BeginDraw()
        {
            Begin(_camera.World, _camera.Projection, _camera.View);
        }

        public void EndDraw()
        {
            End();
        }

        public float DrawDepth
        {
            get;
            set;
        }

        public void DrawSprite(LiteEngine.Textures.Texture texture, RectangleF center, float rotation, float alpha)
        {
            DrawSprite(texture, center, DrawDepth, rotation, new Vector2F(0.5f, 0.5f), Color.White * 3);
        }

        public void DrawSprite(LiteEngine.Textures.Texture texture, RectangleF center, float rotation)
        {
            DrawSprite(texture, center, DrawDepth, rotation, new Vector2F(0.5f, 0.5f), Color.White);
        }

        public void DrawExactSprite(LiteEngine.Textures.Texture texture, RectangleF position, float rotation)
        {
            DrawSprite(texture, position, DrawDepth, rotation, new Vector2F(0f, 0f), Color.White);
        }

        public void DrawSprite(LiteEngine.Textures.Texture texture, RectangleF position, float drawDepth, float rotation, Vector2F origin, Color color, bool flipHorizontal = false)
        {
            Texture2D xnaTexture = _contentManager.Load<Texture2D>(texture.Name);

            Rectangle sourceRect = xnaTexture.Bounds;
            if (texture.Bounds.HasValue)
                sourceRect = new Rectangle(texture.Bounds.Value.X, texture.Bounds.Value.Y, texture.Bounds.Value.Width, texture.Bounds.Value.Height);
            float scaleW = 1 / (float)sourceRect.Width * position.Width;
            float scaleH = 1 / (float)sourceRect.Height * position.Height;

            origin.X *= sourceRect.Width;
            origin.Y *= sourceRect.Height;

            SpriteEffects effects = SpriteEffects.None;
            if (flipHorizontal)
                effects = SpriteEffects.FlipHorizontally;

            _spriteBatch.Draw(xnaTexture, new Vector2(position.X, position.Y), sourceRect, color, rotation, new Vector2(origin.X, origin.Y), new Vector2(scaleW, scaleH), effects, drawDepth);
        }

        internal void Initialize()
        {
            _spriteBatch = new SpriteBatch(_deviceManager.GraphicsDevice);
        }

        internal void Begin(Matrix world, Matrix projection, Matrix view)
        {
            Effect effect = _contentManager.Load<Effect>("basicshader.mgfxo");
            effect.Parameters["xWorld"].SetValue(world);
            effect.Parameters["xProjection"].SetValue(projection);
            effect.Parameters["xView"].SetValue(view);   // blendstate was null before
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, effect, Matrix.Identity);
        }

        internal void End()
        {
            _spriteBatch.End();
        }
    }
}
