using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using LiteEngine.Math;
using LiteEngine.Rendering;
using LiteEngine.Textures;

namespace LiteGame
{
    class Ship
    {
        Body _body;
        Texture _texture = new Texture("rocketship");

        public Ship(World world)
        {
            _body = BodyFactory.CreateBody(world);
            _body.BodyType = BodyType.Dynamic;
            _body.AngularDamping = 0.5f;
            _body.Friction = 1f;
            _body.Restitution = 0f; // 1f;
            _body.Mass = 0.5f;
            _body.Rotation = 0.1f;

            FixtureFactory.AttachPolygon(new Vertices(new Vector2[] { new Vector2(0f, -0.4f), new Vector2(0.35f, 0.4f), new Vector2(-0.35f, 0.4f) }), 1f, _body);

        }

        public float Rotation
        {
            get
            {
                return _body.Rotation;
            }
        }

        public void ApplyForwardThrust(float amount)
        {
            Vector2 facing = new Vector2((float)Math.Sin(Rotation), -(float)Math.Cos(Rotation));
            _body.ApplyForce(facing * 5f);
        }

        public Vector2 Position
        {
            get
            {
                return _body.Position;
            }
            set
            {
                _body.Position = value;
            }
        }

        internal void Draw(XnaRenderer renderer)
        {
            renderer.DrawSprite(_texture, new RectangleF(Position.X, Position.Y, 1, 1), Rotation);
        }

        internal void RotateThrusters(float amount)
        {
            _body.ApplyTorque(amount);
        }
    }
}
