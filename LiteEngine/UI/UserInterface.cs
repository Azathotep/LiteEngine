using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using LiteEngine.Xna;
using LiteEngine.Rendering;
using LiteEngine.Math;
using LiteEngine.Input;

namespace LiteEngine.UI
{
    public class UserInterface : BaseUIControl
    {
        Camera2D _camera;
        LiteXnaEngine _engine;
        public UserInterface(LiteXnaEngine engine)
        {
            _engine = engine;
            SetResolution(800, 600);
        }

        public void SetResolution(int width, int height)
        {
            Size = new SizeF(width, height);
            _camera = new Camera2D(Size * 0.5f, Size);
        }

        List<Dialog> _shownDialogs = new List<Dialog>();
        List<BaseUIControl> _controls = new List<BaseUIControl>();

        /// <summary>
        /// Display dialog centered on the screen
        /// </summary>
        public void ShowDialog(Dialog dialog)
        {
            Vector2 v = _engine.ScreenSize * 0.5f - dialog.Size * 0.5f;
            ShowDialog(dialog, _engine.ScreenSize * 0.5f - dialog.Size * 0.5f);
        }

        public void ShowDialog(Dialog dialog, Vector2 position)
        {
            dialog.Position = position;
            dialog.Initialize();
            _shownDialogs.Add(dialog);
            _engine.KeyboardHandler.UnpressKeys();
        }

        List<Dialog> _closeList = new List<Dialog>();

        internal void RenderUI(GameTime gameTime, XnaRenderer renderer)
        {
            renderer.BeginDraw(_camera, Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred);

            DrawInternal(gameTime, renderer);

            foreach (Dialog dialog in _shownDialogs)
            {
                if (dialog.IsClosing)
                    _closeList.Add(dialog);
                renderer.DrawOffset = Vector2.Zero;
                dialog.DrawInternal(gameTime, renderer);
            }

            if (ShowMouseCursor)
                DrawMouseCursor(renderer);

            renderer.EndDraw();

            foreach (Dialog dialog in _closeList)
                _shownDialogs.Remove(dialog);
            _closeList.Clear();
        }

        public bool ShowMouseCursor = false;

        void DrawMouseCursor(XnaRenderer renderer)
        {
            Vector2 mPos = _engine.GetMousePosition();
            mPos = _camera.ViewToWorld(mPos);
            renderer.DrawPoint(mPos, 10f, Color.Green, 1f);
        }

        /// <summary>
        /// Processes a mouse click occurs by sending it to the correct control in the control tree
        /// </summary>
        /// <param name="button">button clicked</param>
        /// <param name="viewPosition">mouse click position in view coordinates (-1..1)</param>
        /// <returns>true if the mouse click was handled by a control in the UI</returns>
        internal bool ProcessMouseClick(MouseButton button, Vector2 viewPosition)
        {
            //get the mouse position in UI coordinates
            Vector2 mousePos = _camera.ViewToWorld(viewPosition);

            bool handled;
            foreach (Dialog dialog in _shownDialogs)
            {
                Vector2 relPos = mousePos - dialog.Position;
                handled = dialog.OnMouseClickInternal(button, relPos);
                if (handled)
                    return handled;
            }
            handled = OnMouseClickInternal(button, mousePos);
            return handled;
        }

        internal bool ProcessKey(Keys key, out int repressDelay)
        {
            repressDelay = 0;
            if (_shownDialogs.Count == 0)
                return false;
            foreach (Dialog dialog in _shownDialogs.Reverse<Dialog>())
            {
                if (dialog.KeyboardFocus)
                {
                    KeyPressResult res = dialog.ProcessKey(this, key);
                    repressDelay = res.ReprocessDelay;
                    return true;
                }
            }
            return false;
        }

        internal void HideDialog(Dialog dialog)
        {
            _shownDialogs.Remove(dialog);
            _engine.KeyboardHandler.UnpressKeys();
        }

        public ICamera Camera 
        {
            get
            {
                return _camera;
            }
        }
    }
}
