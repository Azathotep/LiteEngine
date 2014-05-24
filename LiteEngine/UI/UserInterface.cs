using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using LiteEngine.Xna;
using LiteEngine.Rendering;
using LiteEngine.Math;

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

        //public void AddControl(BaseUIControl control)
        //{
        //    AddChild(control);
        //   // _controls.Add(control);
        //}

        public void ShowDialog(Dialog dialog, Vector2 position)
        {
            dialog.Position = position;
            dialog.Initialize();
            _shownDialogs.Add(dialog);
            _engine.KeyboardHandler.UnpressKeys();
        }

        List<Dialog> _closeList = new List<Dialog>();

        internal void RenderUI(XnaRenderer renderer)
        {
            renderer.BeginDraw(_camera, Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred);

            DrawInternal(renderer);
            //foreach (BaseUIControl control in _controls)
            //{
            //    renderer.DrawOffset = Vector2.Zero;
            //    DrawControl(control, renderer);
            //}
            //renderer.EndDraw();

            foreach (Dialog dialog in _shownDialogs)
            {
                if (dialog.IsClosing)
                    _closeList.Add(dialog);
                renderer.DrawOffset = Vector2.Zero;
                DrawControl(dialog, renderer);
            }
            renderer.EndDraw();

            foreach (Dialog dialog in _closeList)
                _shownDialogs.Remove(dialog);
            _closeList.Clear();
        }

        internal void DrawControl(BaseUIControl control, XnaRenderer renderer)
        {
            //control.DrawInternal(renderer);

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

        public ICamera Camera 
        {
            get
            {
                return _camera;
            }
        }
    }
}
