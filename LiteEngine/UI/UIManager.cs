using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using LiteEngine.Xna;
using LiteEngine.Rendering;

namespace LiteEngine.UI
{
    public class UIManager
    {
        LiteXnaEngine _engine;
        public UIManager(LiteXnaEngine engine)
        {
            _engine = engine;
        }

        List<Dialog> _shownDialogs = new List<Dialog>();

        /// <summary>
        /// Display dialog centered on the screen
        /// </summary>
        public void ShowDialog(Dialog dialog)
        {
            ShowDialog(dialog, new Vector2(_engine.Renderer.ScreenWidth, _engine.Renderer.ScreenHeight) * 0.5f - dialog.Size * 0.5f);
        }

        public void ShowDialog(Dialog dialog, Vector2 position)
        {
            dialog.Position = position;
            _shownDialogs.Add(dialog);
            _engine.KeyboardHandler.UnpressKeys();
        }

        List<Dialog> _closeList = new List<Dialog>();

        internal void RenderUI(XnaRenderer renderer)
        {
            Matrix world = Matrix.Identity;
            Matrix projection = Matrix.CreateOrthographic(renderer.ScreenWidth, renderer.ScreenHeight, -1000.5f, 500); //800, 600, -1000.5f, 500);
            Matrix view = Matrix.CreateLookAt(new Vector3(renderer.ScreenWidth / 2, renderer.ScreenHeight/2, -1), new Vector3(renderer.ScreenWidth / 2, renderer.ScreenHeight / 2, 0), new Vector3(0, -1, 0));
            
            renderer.Begin(world, projection, view, Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred);
            foreach (Dialog dialog in _shownDialogs)
            {
                if (dialog.IsClosing)
                    _closeList.Add(dialog);
                renderer.DrawOffset = Vector2.Zero;
                DrawControl(dialog, renderer);
            }
            renderer.End();

            foreach (Dialog dialog in _closeList)
                _shownDialogs.Remove(dialog);
            _closeList.Clear();
        }

        internal void DrawControl(BaseUIControl control, XnaRenderer renderer)
        {
            if (control.BorderWidth > 0.01f)
                control.DrawBorder(renderer);
            control.Draw(renderer);
            Vector2 drawOffset = renderer.DrawOffset + control.Position;
            foreach (BaseUIControl child in control.Children)
            {
                renderer.DrawOffset = drawOffset;
                DrawControl(child, renderer);
            }
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
    }
}
