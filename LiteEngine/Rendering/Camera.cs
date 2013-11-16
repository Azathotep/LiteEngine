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
            SetViewField(20, 15);
        }

        Vector2F _position;

        public Vector2F Position
        {
            get
            {
                return _position;
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

        public void LookAt(float x, float y)
        {
            _position = new Vector2F(x,y);
            _view = Matrix.CreateLookAt(new Vector3(x, y, -1), new Vector3(x, y, 0), new Vector3(0, -1, 0));
        }

        public void MoveBy(float x, float y)
        {
            LookAt(_position.X + x, _position.Y + y);
        }

        public void SetViewField(float viewWidth, float viewHeight)
        {
            _world = Matrix.Identity;
            _projection = Matrix.CreateOrthographic(viewWidth, viewHeight, -1000.5f, 100);
        }
    }
}
