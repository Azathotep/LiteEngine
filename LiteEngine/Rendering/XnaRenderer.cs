using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

        RenderTarget2D _target1;

        public XnaRenderer(GraphicsDeviceManager deviceManager, ContentManager contentManager, GameWindow window)
        {
            _deviceManager = deviceManager;
            _contentManager = contentManager;
            _window = window;
            DrawDepth = 1;    
            _camera = new Camera(this);
        }

        public RenderTarget2D CreateRenderTarget(int width, int height)
        {
            var pp = _deviceManager.GraphicsDevice.PresentationParameters;
            RenderTarget2D ret = new RenderTarget2D(_deviceManager.GraphicsDevice,
                                 width, //Same width as backbuffer
                                 height, //Same height
                                 false, //No mip-mapping
                                 pp.BackBufferFormat, //Same colour format
                                 pp.DepthStencilFormat); //Same depth stencil
            return ret;
        }

        public void UseRenderTarget(RenderTarget2D target)
        {
            _deviceManager.GraphicsDevice.SetRenderTarget(target);
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

        public VertexBuffer CreateVertexBuffer(int vertexCount)
        {
            VertexBuffer ret = new VertexBuffer(_deviceManager.GraphicsDevice, VertexPositionColorTexture.VertexDeclaration, vertexCount, BufferUsage.WriteOnly);
            return ret;
        }

        public GraphicsDevice GraphicsDevice
        {
            get
            {
                return _deviceManager.GraphicsDevice;
            }
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

        Camera _camera;
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

        public void DrawSprite(LiteEngine.Textures.Texture texture, Vector2 centerPosition, Vector2 size, float rotation)
        {
            DrawSprite(texture, new RectangleF(centerPosition.X, centerPosition.Y, size.X, size.Y), DrawDepth, rotation, new Vector2(0.5f, 0.5f), Color.White);
        }

        public void DrawSprite(LiteEngine.Textures.Texture texture, Vector2 centerPosition, Vector2 size, float rotation, float alpha)
        {
            Color color = Color.White;
            color.A = (byte)(alpha * 255);
            DrawSprite(texture, new RectangleF(centerPosition.X, centerPosition.Y, size.X, size.Y), DrawDepth, rotation, new Vector2(0.5f, 0.5f), color);
        }

        /// <summary>
        /// Draw sprite centered on specific point. Setting alpha to 0 will cause additive blend.
        /// </summary>
        public void DrawSprite(LiteEngine.Textures.Texture texture, Vector2 centerPosition, Vector2 size, float rotation, Color color, float alpha)
        {
            color.A = (byte)(alpha * 255);
            DrawSprite(texture, new RectangleF(centerPosition.X, centerPosition.Y, size.X, size.Y), DrawDepth, rotation, new Vector2(0.5f, 0.5f), color);
        }

        public void DrawSprite(LiteEngine.Textures.Texture texture, RectangleF position)
        {
            DrawSprite(texture, position, DrawDepth, 0, new Vector2(0f, 0f), Color.White);
        }

        public void DrawSprite(LiteEngine.Textures.Texture texture, RectangleF position, Color color)
        {
            DrawSprite(texture, position, DrawDepth, 0, new Vector2(0f, 0f), color);
        }

        public void DrawSprite(LiteEngine.Textures.Texture texture, RectangleF position, float rotation)
        {
            DrawSprite(texture, position, DrawDepth, rotation, new Vector2(0f, 0f), Color.White);
        }

        public void DrawSprite(LiteEngine.Textures.Texture texture, RectangleF position, Color color, float rotation)
        {
            DrawSprite(texture, position, DrawDepth, rotation, new Vector2(0f, 0f), color);
        }

        public void DrawSprite(LiteEngine.Textures.Texture texture, RectangleF position, float drawDepth, float rotation, Vector2 origin, Color color, bool flipHorizontal = false, bool wrapped = false)
        {
            Texture2D xnaTexture = _contentManager.Load<Texture2D>(texture.Name);
            Rectangle sourceRect = xnaTexture.Bounds;
            if (texture.Bounds.HasValue)
                sourceRect = new Rectangle(texture.Bounds.Value.X, texture.Bounds.Value.Y, texture.Bounds.Value.Width, texture.Bounds.Value.Height);
            DrawSprite(xnaTexture, sourceRect, position, drawDepth, rotation, origin, color, flipHorizontal, wrapped);
        }

        public void DrawSprite(Texture2D texture, Rectangle sourceRect, RectangleF destRect, float drawDepth, float rotation, Vector2 origin, Color color, bool flipHorizontal = false, bool wrapped = false)
        {
            Vector2 scale = Vector2.One;
            if (!wrapped)
            {
                float scaleW = 1 / (float)sourceRect.Width * destRect.Width;
                float scaleH = 1 / (float)sourceRect.Height * destRect.Height;
                scale = new Vector2(scaleW, scaleH);
            }
            else
            {
                //scale = new Vector2(0.1f, 0.1f);
                sourceRect = new Rectangle(0, 0, 2000, 900);
            }
            origin.X *= sourceRect.Width;
            origin.Y *= sourceRect.Height;

            SpriteEffects effects = SpriteEffects.None;
            if (flipHorizontal)
                effects = SpriteEffects.FlipHorizontally;
            Vector2 position = DrawOffset + new Vector2(destRect.X, destRect.Y);
            _spriteBatch.Draw(texture, position, sourceRect, color, rotation, new Vector2(origin.X, origin.Y), scale, effects, drawDepth);
        }

        public Vector2 DrawOffset
        {
            get;
            set;
        }

        public ContentManager ContentManager
        {
            get
            {
                return _contentManager;
            }
        }

        internal void Initialize()
        {
            _spriteBatch = new SpriteBatch(_deviceManager.GraphicsDevice);
        }

        public void Begin(Matrix world, Matrix projection, Matrix view, SpriteSortMode sortMode = SpriteSortMode.BackToFront)
        {
            DrawOffset = Vector2.Zero;
            Effect effect = _contentManager.Load<Effect>("basicshader.mgfxo");
            effect.Parameters["xWorld"].SetValue(world);
            effect.Parameters["xProjection"].SetValue(projection);
            effect.Parameters["xView"].SetValue(view);   // blendstate was null before
            //linearClamp LinearWrap
            _spriteBatch.Begin(sortMode, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, effect, Matrix.Identity);
        }

        internal void End()
        {
            _spriteBatch.End();
        }

        public Vector2 MeasureString(string text)
        {
            SpriteFont font = _contentManager.Load<SpriteFont>("Font");
            return font.MeasureString(text);
        }

        public void DrawString(string text, Vector2 position, Color color)
        {
            SpriteFont font = _contentManager.Load<SpriteFont>("Font");
            position += DrawOffset;
            _spriteBatch.DrawString(font, text, position, color, 0, Vector2.Zero, 1f, SpriteEffects.None, DrawDepth);
        }

        /// <summary>
        /// Formats a string so that it will fit within the specified bounds when drawn 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="bounds"></param>
        /// <param name="rightAlign"></param>
        /// <returns>formatted string</returns>
        public string GenerateFormattedString(string text, float maxWidth, out Vector2 formattedSize, bool rightAlign = false)
        {
            SpriteFont font = _contentManager.Load<SpriteFont>("Font");
            //todo bounds.Height ignored
            string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string drawString = "";
            string lineSoFar = "";
            string linePlusWord = "";
            formattedSize = new Vector2(0, 0);
            foreach (string word in words)
            {
                if (lineSoFar.Length > 0)
                    linePlusWord += " ";
                linePlusWord += word;
                Vector2 newSize = font.MeasureString(linePlusWord);
                if (newSize.X >= maxWidth)
                {
                    drawString += lineSoFar + Environment.NewLine;
                    lineSoFar = word;
                    linePlusWord = lineSoFar;
                }
                else
                    lineSoFar = linePlusWord;
            }
            if (lineSoFar.Length > 0)
            {
                drawString += lineSoFar;
                formattedSize = font.MeasureString(drawString);
            }
            return drawString;
        }

        public RectangleF DrawStringBox(string text, RectangleF bounds, Color color, bool rightAlign = false)
        {
            SpriteFont font = _contentManager.Load<SpriteFont>("Font");
            float screenWidth = bounds.Width; //ViewWidth;
            float screenHeight = bounds.Height; //ViewHeight;
            string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string drawString = "";
            string lineSoFar = "";
            string linePlusWord = "";
            foreach (string word in words)
            {
                if (lineSoFar.Length > 0)
                    linePlusWord += " ";
                linePlusWord += word;
                Vector2 newSize = font.MeasureString(linePlusWord);
                if (newSize.X >= screenWidth)
                {
                    drawString += lineSoFar + Environment.NewLine;
                    lineSoFar = word;
                    linePlusWord = lineSoFar;
                }
                else
                    lineSoFar = linePlusWord;
            }
            if (lineSoFar.Length > 0)
                drawString += lineSoFar;

            //Vector2 worldPos = ScreenToWorld(bounds.X, bounds.Y);
            Vector2 worldPos = new Vector2(bounds.X, bounds.Y);
            float _worldScreenScale = 1f; // 0.04f;
            worldPos += DrawOffset;
            _spriteBatch.DrawString(font, drawString, worldPos, color, 0, Vector2.Zero, _worldScreenScale, SpriteEffects.None, DrawDepth);
            return bounds;
        }

        public void BeginDrawToScreen()
        {
            DrawOffset = Vector2.Zero;

            //Matrix scale = Matrix.CreateScale(0.5f); //Matrix.Identity; // 
            //_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, scale);
            _spriteBatch.Begin();
        }

        public Vector2 CenterScreen 
        {
            get
            {
                return new Vector2(ScreenWidth, ScreenHeight) * 0.5f;
            }
        }
    }
}
