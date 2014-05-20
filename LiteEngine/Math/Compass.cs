using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteEngine.Math
{
    public class Compass
    {
        public static Vector2 GetVector2(CardinalDirection direction)
        {
            switch (direction)
            {
                case CardinalDirection.North:
                    return new Vector2(0, -1);
                case CardinalDirection.East:
                    return new Vector2(1, 0);
                case CardinalDirection.South:
                    return new Vector2(0, 1);
                case CardinalDirection.West:
                default:
                    return new Vector2(-1, 0);
            }
        }

        public static float GetAngle(CardinalDirection direction)
        {
            switch (direction)
            {
                case CardinalDirection.North:
                    return 0f;
                case CardinalDirection.East:
                    return MathHelper.PiOver2;
                case CardinalDirection.South:
                    return MathHelper.Pi;
                case CardinalDirection.West:
                default:
                    return MathHelper.Pi + MathHelper.PiOver2;
            }
        }

        public static CardinalDirection GetOppositeDirection(CardinalDirection direction)
        {
            switch (direction)
            {
                case CardinalDirection.North:
                    return CardinalDirection.South;
                case CardinalDirection.West:
                    return CardinalDirection.East;
                case CardinalDirection.East:
                    return CardinalDirection.West;
                case CardinalDirection.South:
                default:
                    return CardinalDirection.North;
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
