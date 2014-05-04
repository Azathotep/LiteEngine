using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LiteEngine.Math;

namespace LiteEngine.Rendering
{
    public class Camera
    {
        Matrix _world = Matrix.Identity;
        Matrix _projection;
        Matrix _view;

        public Camera()
        {
            SetAspect(20, 15);
        }

        Vector2 _position;
        public Vector2 Position
        {
            get
            {
                return _position;
            }
        }

        float _angle;
        public float Angle
        {
            get
            {
                return _angle;
            }
        }

        float _zoom = 1;
        public float Zoom
        {
            get
            {
                return _zoom;
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

        public void LookAt(Vector2 position, float angle=0)
        {
            _position = position;
            _angle = angle;
            Vector3 up = new Vector3((float)System.Math.Sin(angle), -(float)System.Math.Cos(angle), 0);
            _view = Matrix.CreateLookAt(new Vector3(position, -1), new Vector3(position, 0), up);
        }

        public void ChangeZoom(float newZoom)
        {
            _zoom = newZoom;
            Vector2 viewExtent = _aspect * _zoom;
            _projection = Matrix.CreateOrthographic(viewExtent.X, viewExtent.Y, -1000.5f, 100);
        }

        public void MoveBy(Vector2 offset)
        {
            LookAt(_position + offset, _angle);
        }

        Vector2 _aspect;
        /// <summary>
        /// Sets the view width and height (in world space) for zoom=1
        /// </summary>
        /// <param name="viewWidth"></param>
        /// <param name="viewHeight"></param>
        public void SetAspect(float width, float height)
        {
            _aspect = new Vector2(width, height);
            //force projection recalculation
            ChangeZoom(_zoom);
        }

        public float ViewWidth
        {
            get
            {
                return _aspect.X * _zoom;
            }
        }

        public float ViewHeight
        {
            get
            {
                return _aspect.Y * _zoom;
            }
        }

        /// <summary>
        /// Converts screen coordinates (range -1..1) to world coordinates
        /// </summary>
        public Vector2 ScreenToWorld(Vector2 screen)
        {
            Matrix wvp = World * View * Projection;
            Matrix inv = Matrix.Invert(wvp);
            return Vector2.Transform(screen, inv);
        }

        /// <summary>
        /// Converts world coordinates to screen coordinates (range screenWidth..ScreenHeight)
        /// </summary>
        /// <param name="world"></param>
        /// <param name="screenSize">size of screen in screen units, eg 640x480</param>
        /// <returns></returns>
        public Vector2 WorldToScreen(Vector2 world, Vector2 screenSize)
        {
            Matrix wvp = World * View * Projection;
            Vector2 ret = Vector2.Transform(world, wvp);
            //ret is in range (-1..1)
            ret.X = (ret.X + 1) * 0.5f * screenSize.X;
            ret.Y = (2 - (ret.Y + 1)) * 0.5f * screenSize.Y;
            return ret;
        }

        /// <summary>
        /// Returns the side of a line that a point is on
        /// </summary>
        int LineSide(Vector2 a, Vector2 b, Vector2 point)
        {
            return System.Math.Sign((b.X - a.X) * (point.Y - a.Y) - (b.Y - a.Y) * (point.X - a.X));
        }

        /// <summary>
        /// Returns whether a world point is on the screen
        /// </summary>
        public bool IsPointOnScreen(Vector2 worldPoint)
        {
            Matrix wvp = World * View * Projection;
            Vector2 screenPoint = Vector2.Transform(worldPoint, wvp);
            if (screenPoint.X > -1 && screenPoint.X < 1 && screenPoint.Y > -1 && screenPoint.Y < 1)
                return true;
            return false;
        }
    }
}
