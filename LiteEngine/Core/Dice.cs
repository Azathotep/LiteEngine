using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LiteEngine.Core
{
    /// <summary>
    /// Provides access to random numbers
    /// </summary>
    public class Dice
    {
        static Random _r = new Random();

        public static int Next(int maxValue)
        {
            return _r.Next(maxValue);
        }

        public static float Next()
        {
            return (float)_r.NextDouble();
        }

        /// <summary>
        /// Returns a float in the range +-plusMinus
        /// </summary>
        public static float RandomFloat(float plusMinus)
        {
            return Next() * plusMinus * 2 - plusMinus;
        }

        /// <summary>
        /// Returns a Vector2 with random components in the range +-plusMinus
        /// </summary>
        public static Vector2 RandomVector2(float plusMinus)
        {
            return new Vector2(RandomFloat(plusMinus), RandomFloat(plusMinus));
        }

        /// <summary>
        /// Returns a Vector3 with random components in the range +-plusMinus
        /// </summary>
        public static Vector3 RandomVector3(float plusMinus)
        {
            return new Vector3(RandomFloat(plusMinus), RandomFloat(plusMinus), RandomFloat(plusMinus));
        }
    }
}
