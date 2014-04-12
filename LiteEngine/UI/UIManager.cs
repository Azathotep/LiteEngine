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

        public void ShowDialog(Dialog dialog)
        {
            dialog.OnClose += dialog_OnClose;
            //center the dialog at the center of the screen
            dialog.Position = new Vector2(_engine.Renderer.ScreenWidth, _engine.Renderer.ScreenHeight) * 0.5f - dialog.Size * 0.5f;
            _shownDialogs.Add(dialog);
            _engine.KeyboardHandler.UnpressKeys();
        }

        void dialog_OnClose(object sender, EventArgs e)
        {
            Dialog dialog = sender as Dialog;
            if (dialog == null)
                return;
            dialog.OnClose -= dialog_OnClose;
            _shownDialogs.Remove(dialog);
        }

        internal void RenderUI(XnaRenderer renderer)
        {
            Matrix world = Matrix.Identity;
            Matrix projection = Matrix.CreateOrthographic(renderer.ScreenWidth, renderer.ScreenHeight, -1000.5f, 500); //800, 600, -1000.5f, 500);
            Matrix view = Matrix.CreateLookAt(new Vector3(renderer.ScreenWidth / 2, renderer.ScreenHeight/2, -1), new Vector3(renderer.ScreenWidth / 2, renderer.ScreenHeight / 2, 0), new Vector3(0, -1, 0));
            
            renderer.Begin(world, projection, view, Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred);
            foreach (Dialog dialog in _shownDialogs)
            {
                renderer.DrawOffset = Vector2.Zero;
                DrawControl(dialog, renderer);
            }
            renderer.End();
        }

        internal void DrawControl(BaseUIControl control, XnaRenderer renderer)
        {
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
                    repressDelay = dialog.ProcessKey(key);
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
