using LiteEngine.Rendering;
using LiteEngine.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LiteEngine.Math;

namespace LiteEngine.UI
{
    public class ImageControl : BaseUIControl
    {
        public ImageControl(Texture image)
        {
            Image = image;
        }

        public Texture Image
        {
            get;
            set;
        }

        public override void Draw(XnaRenderer renderer)
        {
            renderer.DrawSprite(Image, Bounds, 0);
        }
    }

    public class TextControl : BaseUIControl
    {
        string _text = "";
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        public Texture Background
        {
            get;
            set;
        }

        public override void Draw(XnaRenderer renderer)
        {
            if (Background != null)
                renderer.DrawSprite(Background, Bounds, 0);
            renderer.DrawStringBox(_text, Bounds, Color.White);
        }
    }
}
