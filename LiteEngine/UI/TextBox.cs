using LiteEngine.Math;
using LiteEngine.Rendering;
using LiteEngine.Textures;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteEngine.UI
{
    public class TextBox : BaseUIControl
    {
        bool _needsReformatting = false;
        string _formattedText = "";
        string _text = "";
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    _needsReformatting = true;
                }
            }
        }

        public Texture Background
        {
            get;
            set;
        }

        float _textScale=1f;
        public float TextScale
        {
            get
            {
                return _textScale;
            }
            set
            {
                if (_textScale != value)
                {
                    _textScale = value;
                    _needsReformatting = true;
                }
            }
        }

        public Color TextColor = Color.Black;

        public bool AutoSize = false;

        public override void Draw(GameTime gameTime, XnaRenderer renderer)
        {
            if (_needsReformatting)
            {
                SizeF formattedSize;
                float maxWidth = Bounds.Width;
                if (AutoSize)
                {
                    if (Parent != null)
                        maxWidth = Parent.Size.Width;
                    else
                        maxWidth = 10000;
                }
                _formattedText = renderer.GenerateFormattedString(_text, _textScale, maxWidth, out formattedSize);
                if (!AutoSize)
                {
                    //keep the width fixed but allow the height to grow or shrink
                    formattedSize.Width = Size.Width;
                }
                Size = formattedSize;
                _needsReformatting = false;
            }
            if (Background != null)
                renderer.DrawSprite(Background, Bounds, 0);
            renderer.DrawString(_formattedText, Position, TextColor, _textScale);
        }
    }
}
