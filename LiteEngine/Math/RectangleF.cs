using Microsoft.Xna.Framework;
namespace LiteEngine.Math
{
    /// <summary>
    /// Float rectangle
    /// </summary>
    public struct RectangleF
    {
        //it would be nice to make a generic Rectangle<T> struct but there are problems with eg the contains method
        float _x;
        float _y;
        float _width;
        float _height;

        public RectangleF(float x, float y, float width, float height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public bool Contains(Vector2F p)
        {
            return p.X >= _x && p.X <= _x + _width && p.Y > _y && p.Y < _y + _height;
        }

        /// <summary>
        /// Returns a rectangle resized in all directions by a specified amount
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public RectangleF Grow(float amount)
        {
            return new RectangleF(X - amount, Y - amount, Width + amount * 2, Height + amount * 2);
        }

        public float X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        public float Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }

        public float Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        public float Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        public float Left
        {
            get
            {
                return _x;
            }
        }

        public float Top
        {
            get
            {
                return _y;
            }
        }

        public float Right
        {
            get
            {
                return _x + _width;
            }
        }

        public float Bottom
        {
            get
            {
                return _y + _height;
            }
        }

        public Vector2 TopLeft
        {
            get
            {
                return new Vector2(Left, Top);
            }
        }

        public Vector2 TopRight
        {
            get
            {
                return new Vector2(Right, Top);
            }
        }

        public Vector2 BottomLeft
        {
            get
            {
                return new Vector2(Left, Bottom);
            }
        }

        public Vector2 BottomRight
        {
            get
            {
                return new Vector2(Right, Bottom);
            }
        }
    }  
}
