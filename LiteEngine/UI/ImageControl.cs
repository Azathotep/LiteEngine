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
        public ImageControl()
        {

        }

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
            if (Image != null)
                renderer.DrawSprite(Image, Bounds, 0);
        }
    }

    public class TextControl : BaseUIControl
    {
        bool _needsReformatting = false;
        string _formattedText="";
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
                _needsReformatting = true;
            }
        }

        public Texture Background
        {
            get;
            set;
        }

        public Color TextColor = Color.White;

        public override void Draw(XnaRenderer renderer)
        {
            if (_needsReformatting)
                _formattedText = renderer.GenerateFormattedString(_text, Bounds);

            if (Background != null)
                renderer.DrawSprite(Background, Bounds, 0);
            renderer.DrawString(_formattedText, Position, TextColor);
        }
    }
}
