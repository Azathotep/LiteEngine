using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LiteEngine.Math;

namespace LiteGame
{
    class Ship
    {
        Vector2 _position;
        Vector2 _velocity;
        Vector2 _facing;

        public Ship()
        {
            _facing = new Vector2(1, 0);
        }

        public void Rotate(float angle)
        {
            Matrix rot = Microsoft.Xna.Framework.Matrix.CreateRotationZ(angle);
            _facing = Vector2.Transform(_facing, rot);
        }

        public void Update()
        {
            _position += _velocity;
        }

        public void ApplyThrust(float amount)
        {
            _velocity += _facing * amount;
        }

        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public Vector2 Facing
        {
            get
            {
                return _facing;
            }
        }
    }
}
