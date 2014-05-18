using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteEngine.Math
{
    public class Compass
    {
        public static Vector2 GetVector(CardinalDirection direction)
        {
            switch (direction)
            {
                case CardinalDirection.North:
                    return new Vector2(0, -1f);
                case CardinalDirection.East:
                    return new Vector2(1, 0);
                case CardinalDirection.South:
                    return new Vector2(0, 1f);
                case CardinalDirection.West:
                default:
                    return new Vector2(-1f, 0);
            }
        }
    }

    public enum CardinalDirection
    {
        North,
        East,
        South,
        West
    }
}
