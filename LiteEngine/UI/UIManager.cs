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
            dialog.UI = this;
            _shownDialogs.Add(dialog);
            _engine.KeyboardHandler.UnpressKeys();
        }

        internal void RenderUI(XnaRenderer renderer)
        {
            Matrix world = Matrix.Identity;
            Matrix projection = Matrix.CreateOrthographic(renderer.ScreenWidth, renderer.ScreenHeight, -1000.5f, 500); //800, 600, -1000.5f, 500);
            Matrix view = Matrix.CreateLookAt(new Vector3(renderer.ScreenWidth / 2, renderer.ScreenHeight/2, -1), new Vector3(renderer.ScreenWidth / 2, renderer.ScreenHeight / 2, 0), new Vector3(0, -1, 0));

            renderer.Begin(world, projection, view);
            foreach (Dialog dialog in _shownDialogs)
                dialog.Render(renderer);
            renderer.End();
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
