using LiteEngine.Math;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteEngine.Rendering
{
    /// <summary>
    /// Provides methods to set up the view in 2D space
    /// </summary>
    public class Camera2D : ICamera
    {
        Matrix _world = Matrix.Identity;
        Matrix _projection;
        Matrix _view;
        public Camera2D(Vector2 position, Vector2 viewSize)
        {
            LookAt(position, 0);
            SetViewSize(viewSize);
        }

        Vector2 _position;
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (_position != value)
                    LookAt(value, _angle);
            }
        }

        Vector2 _size;
        public Vector2 Size
        {
            get
            {
                return _size;
            }
        }

        float _angle;
        /// <summary>
        /// Rotation angle of the camera
        /// </summary>
        public float Angle
        {
            get
            {
                return _angle;
            }
        }

        float _zoom = 1f;
        /// <summary>
        /// View size is scaled by this amount
        /// </summary>
        public float Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                if (_zoom != value)
                    SetViewSize(Size, value);
            }
        }

        public Matrix World
        {
            get
            {
                return _world;
            }
        }

        public Matrix Projection
        {
            get
            {
                return _projection;
            }
        }

        public Matrix View
        {
            get
            {
                return _view;
            }
        }

        public void SetViewSize(Vector2 size, float zoom=1f)
        {
            _zoom = zoom;
            _size = size;
            _projection = Matrix.CreateOrthographic(size.X * zoom, size.Y * zoom, 0, 100);
        }

        public void LookAt(Vector2 position, float angle = 0)
        {
            _position = position;
            _angle = angle;
            Vector3 up = new Vector3(Util.AngleToVector(angle), 0);
            _view = Matrix.CreateLookAt(new Vector3(position, -1), new Vector3(position, 0), up);
        }

        /// <summary>
        /// Converts world coordinates to view coordinates (range -1..+1)
        /// </summary>
        /// <param name="world"></param>
        /// <returns></returns>
        public Vector2 WorldToView(Vector2 world)
        {
            Matrix wvp = World * View * Projection;
            Vector2 ret = Vector2.Transform(world, wvp);
            //ret is in range (-1..1)
            return ret;
        }

        /// <summary>
        /// Converts screen coordinates (range -1..1) to world coordinates
        /// </summary>
        public Vector2 ViewToWorld(Vector2 screen)
        {
            Matrix wvp = World * View * Projection;
            Matrix inv = Matrix.Invert(wvp);
            return Vector2.Transform(screen, inv);
        }
    }
}
