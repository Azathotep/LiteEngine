using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteEngine.Core
{
    /// <summary>
    /// Provides access to random numbers
    /// </summary>
    class Dice
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
    }
}
