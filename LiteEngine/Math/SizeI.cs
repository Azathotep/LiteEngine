using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteEngine.Math
{
    public struct SizeI
    {
        Vector2I _v;
        public SizeI(int width, int height)
        {
            _v.X = width;
            _v.Y = height;
        }

        public int Width
        {
            get
            {
                return _v.X;
            }
            set
            {
                _v.X = value;
            }
        }

        public int Height
        {
            get
            {
                return _v.Y;
            }
            set
            {
                _v.Y = value;
            }
        }

        public static bool operator ==(SizeI a, SizeI b)
        {
            return a.Width == b.Width && a.Height == b.Height;
        }

        public static bool operator !=(SizeI a, SizeI b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
