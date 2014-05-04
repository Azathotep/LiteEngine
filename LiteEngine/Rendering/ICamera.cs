using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteEngine.Rendering
{
    public interface ICamera
    {
        Matrix World
        {
            get;
        }

        Matrix Projection
        {
            get;
        }

        Matrix View
        {
            get;
        }
    }
}
