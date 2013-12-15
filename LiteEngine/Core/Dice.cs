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
        /// Returns a random vector with components randomly in the range +-plusMinus
        /// </summary>
        /// <param name="plusMinus"></param>
        /// <returns></returns>
        public static Vector2 RandomVector(float plusMinus)
        {
            return new Vector2(Next() * plusMinus * 2 - plusMinus, Next() * plusMinus * 2 - plusMinus);
        }
    }
}
