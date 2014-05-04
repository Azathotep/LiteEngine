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

        Vector2 WorldToView(Vector2 world);

        float Zoom { get; set; }

        Vector2 ViewToWorld(Vector2 targetScreenPos);
    }
}
