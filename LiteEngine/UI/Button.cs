using LiteEngine.Rendering;
using LiteEngine.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LiteEngine.Math;
using LiteEngine.Input;

namespace LiteEngine.UI
{
    public class Button : BaseUIControl
    {
        SimpleAnimation _clickAnimation = new SimpleAnimation(0.1, false);
        public Button()
        {
            BackgroundColor = Color.LightGray;
            BorderWidth = 1;
            _clickAnimation.OnDraw += (elapsedSeconds, renderer) =>
                {
                    BackgroundColor = Color.Lerp(Color.LightGray, Color.DarkGray, (float)elapsedSeconds / 0.1f);
                };
            _clickAnimation.OnEnd += () =>
                {
                    BackgroundColor = Color.LightGray;
                };
        }

        public Action OnClick;

        protected override bool OnMouseClick(MouseButton button, Vector2 position)
        {
            _clickAnimation.Start();
            if (OnClick != null)
                OnClick();
            return true;
        }

        public override void Draw(GameTime gameTime, XnaRenderer renderer)
        {
            _clickAnimation.Draw(gameTime, renderer);
        }
    }
}
