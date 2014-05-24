using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteEngine.Math
{
    public struct SizeF
    {
        Vector2 _v;
        public SizeF(float width, float height)
        {
            _v.X = width;
            _v.Y = height;
        }

        public SizeF(Vector2 v)
        {
            _v = v;
        }

        public float Width
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

        public float Height
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

        public static bool operator ==(SizeF a, SizeF b)
        {
            return a.Width == b.Width && a.Height == b.Height;
        }

        public static bool operator !=(SizeF a, SizeF b)
        {
            return !(a == b);
        }

        public static SizeF operator *(SizeF s, float f)
        {
            return new SizeF(s.Width * f, s.Height * f);
        }

        public static SizeF operator -(SizeF a, SizeF b)
        {
            return new SizeF(a.Width - b.Width, a.Height - b.Height);
        }

        public static implicit operator Vector2(SizeF s)
        {
            return new Vector2(s.Width, s.Height);
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
