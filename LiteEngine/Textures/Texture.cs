using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteEngine.Math;

namespace LiteEngine.Textures
{
    public class Texture
    {
        string _name;
        RectangleI? _bounds;

        public Texture(string name, RectangleI? bounds = null)
        {
            _name = name;
            _bounds = bounds;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public RectangleI? Bounds
        {
            get
            {
                return _bounds;
            }
        }
    }
}
