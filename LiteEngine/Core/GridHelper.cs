using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteEngine.Core
{
    public class GridHelper
    {
        /// <summary>
        /// Provides 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="callback"></param>
        public static void Foreach<T>(T[,] array, Action<int, int> callback)
        {
            for (int y = 0; y <= array.GetUpperBound(1); y++)
                for (int x = 0; x <= array.GetUpperBound(0); x++)
                    callback(x, y);
        }
    }
}
