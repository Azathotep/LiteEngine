using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteEngine.Math
{
    public class Util
    {
        /// <summary>
        /// Distance in radians to go from a1 to a2
        /// </summary>
        /// <param name="a1">angle in radians</param>
        /// <param name="a2">angle in radians</param>
        /// <returns></returns>
        public static float AngleBetween(float a1, float a2)
        {
            float dif = MathHelper.WrapAngle(a2 - a1);
            return dif;
        }

        /// <summary>
        /// Returns the angle in radians from start to destination
        /// </summary>
        /// <param name="start"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static float AngleBetween(Vector2 start, Vector2 destination)
        {
            Vector2 v = destination - start;
            return (float)System.Math.Atan2(v.X, -v.Y);
        }

        public static Vector2 AngleToVector(float angle)
        {
            if (angle == 0)
                return new Vector2(0, -1);
            return new Vector2((float)System.Math.Sin(angle), -(float)System.Math.Cos(angle));
        }

        /// <summary>
        /// Projects vector1 onto vector2
        /// </summary>
        /// <param name="force"></param>
        /// <param name="momentArm"></param>
        /// <returns></returns>
        public static Vector2 Project(Vector2 vector1, Vector2 vector2)
        {
            return (Vector2.Dot(vector1, vector2) / Vector2.Dot(vector2, vector2)) * vector2;
        }

        public static Vector2 RotateVector(Vector2 vector, float angle)
        {
            return Vector2.Transform(vector, Matrix.CreateRotationZ(angle));
        }
    }
}
