using LiteEngine.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteEngine.Math
{
    public class Compass
    {
        public static CardinalDirection[] CardinalDirections = {CardinalDirection.North, CardinalDirection.East,
                                                                CardinalDirection.South, CardinalDirection.West};

        public static CompassDirection[] CompassDirections = {CompassDirection.North, CompassDirection.East,
                                                              CompassDirection.South, CompassDirection.West,
                                                              CompassDirection.NorthEast, CompassDirection.NorthWest,
                                                              CompassDirection.SouthEast, CompassDirection.SouthWest};

        public static Vector2 DirectionToVector2(CardinalDirection direction)
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

        /// <summary>
        /// Converts a direction into a one unit grid direction 
        /// </summary>
        public static Vector2I DirectionToVector2I(CardinalDirection direction)
        {
            switch (direction)
            {
                case CardinalDirection.North:
                    return new Vector2I(0, -1);
                case CardinalDirection.East:
                    return new Vector2I(1, 0);
                case CardinalDirection.South:
                    return new Vector2I(0, 1);
                case CardinalDirection.West:
                    return new Vector2I(-1, 0);
            }
            return new Vector2I(1, 0);
        }

        internal static Vector2I DirectionToVector2I(CompassDirection direction)
        {
            switch (direction)
            {
                case CompassDirection.North:
                    return new Vector2I(0, -1);
                case CompassDirection.East:
                    return new Vector2I(1, 0);
                case CompassDirection.South:
                    return new Vector2I(0, 1);
                case CompassDirection.West:
                    return new Vector2I(-1, 0);
                case CompassDirection.NorthEast:
                    return new Vector2I(1, -1);
                case CompassDirection.NorthWest:
                    return new Vector2I(-1, -1);
                case CompassDirection.SouthEast:
                    return new Vector2I(1, 1);
                case CompassDirection.SouthWest:
                    return new Vector2I(-1, 1);
            }
            return new Vector2I(0, 0);
        }

        /// <summary>
        /// Returns the direction a vector points. Assumes one of the components is zero.
        /// </summary>
        public static CardinalDirection GetDirection(Vector2I vector)
        {
            if (vector.X < 0)
                return CardinalDirection.West;
            if (vector.X > 0)
                return CardinalDirection.East;
            if (vector.Y < 0)
                return CardinalDirection.North;
            return CardinalDirection.South;
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

        public static CardinalDirection GetRandomCardinalDirection()
        {
            return CardinalDirections[Dice.Next(4)];
        }

        public static CompassDirection GetRandomCompassDirection()
        {
            return CompassDirections[Dice.Next(8)];
        }
    }

    public enum CardinalDirection
    {
        North,
        East,
        South,
        West
    }

    public enum CompassDirection
    {
        North,
        East,
        South,
        West,
        NorthEast,
        SouthEast,
        SouthWest,
        NorthWest
    }
}
