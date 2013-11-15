using System;

namespace Plan9.Utils
{
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
