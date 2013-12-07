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

        Vector2 _position;

        public Vector2 Position
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

        public void LookAt(Vector2 position)
        {
            _position = position;
            _view = Matrix.CreateLookAt(new Vector3(position, -1), new Vector3(position, 0), new Vector3(0, -1, 0));
        }

        public void MoveBy(Vector2 offset)
        {
            LookAt(_position + offset);
        }

        Vector2 _viewField;

        public void SetViewField(float viewWidth, float viewHeight)
        {
            _viewField = new Vector2(viewWidth, viewHeight);
            _world = Matrix.Identity;
            _projection = Matrix.CreateOrthographic(viewWidth, viewHeight, -1000.5f, 100);
        }

        public float ViewWidth
        {
            get
            {
                return _viewField.X;
            }
        }

        public float ViewHeight
        {
            get
            {
                return _viewField.Y;
            }
        }

        float _angle;
        public float Angle 
        { 
            set
            {
                _angle = value;
                _view = Matrix.CreateLookAt(new Vector3(_position, -1), new Vector3(_position, 0), new Vector3((float)System.Math.Sin(value), -(float)System.Math.Cos(value), 0));
            }
            get
            {
                return _angle;
            }
        }
    }
}
