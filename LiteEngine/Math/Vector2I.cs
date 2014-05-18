using Microsoft.Xna.Framework;
namespace LiteEngine.Math
{
    public struct Vector2I
    {
        public int X;
        public int Y;

        public Vector2I(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Vector2I a, Vector2I b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector2I a, Vector2I b)
        {
            return !(a == b);
        }

        public static Vector2I operator +(Vector2I a, Vector2 b)
        {
            return new Vector2I(a.X + (int)b.X, a.Y + (int)b.Y);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
