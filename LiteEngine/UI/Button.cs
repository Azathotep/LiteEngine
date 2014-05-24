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
        public Button()
        {
            BackgroundColor = Color.LightGray;
            BorderWidth = 1;
        }

        public Action OnClick;

        protected override bool OnMouseClick(MouseButton button, Vector2 position)
        {
            if (OnClick != null)
                OnClick();
            return true;
        }

        public override void Draw(XnaRenderer renderer)
        {
        }
    }
}
