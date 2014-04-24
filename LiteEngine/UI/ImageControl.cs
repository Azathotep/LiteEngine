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
}
