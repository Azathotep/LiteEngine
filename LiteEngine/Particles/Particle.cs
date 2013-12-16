﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using LiteEngine.Physics;
using LiteEngine.Rendering;
using LiteEngine.Textures;
using LiteEngine.Math;

namespace LiteEngine.Particles
{
    public class Particle
    {
        Body _body;
        public int Life;
        static Texture _particleTexture = new Texture("particle");

        public Vector2 Position
        {
            get
            {
                return _body.Position;
            }
        }

        public Vector2 Velocity
        {
            get
            {
                return _body.LinearVelocity;
            }
        }

        /// <summary>
        /// Initializes the particle in the world
        /// </summary>
        /// <param name="physics">physics</param>
        /// <param name="position">new position of the particle</param>
        /// <param name="velocity">new velocity of the particle</param>
        /// <param name="color">color of the particle</param>
        /// <param name="life">lifetime of the particle, before it is removed from the world</param>
        internal void Initialize(PhysicsCore physics, Vector2 position, Vector2 velocity, int life, bool collidesWithWorld)
        {
            if (_body == null)
            {
                _body = physics.CreateBody();
                Fixture f = FixtureFactory.AttachCircle(0.01f, 1f, _body, Vector2.Zero);
                f.CollisionCategories = Category.Cat2;
                f.CollidesWith = Category.Cat1;
            }
            _body.BodyType = BodyType.Dynamic;
            _body.Mass = 0.01f;
            _body.Friction = 1f;
            _body.Restitution = 0.1f;
            _body.Position = position;
            _body.LinearVelocity = velocity;
            _body.IgnoreGravity = true;
            _body.FixedRotation = true;
            _body.LinearDamping = 0.1f;
            if (collidesWithWorld)
            {
                _body.CollisionCategories = Category.Cat2;
                _body.CollidesWith = Category.Cat1;
            }
            else
            {
                _body.CollisionCategories = Category.None;
                _body.CollidesWith = Category.None;
            }
            //enable the particle in the physics system
            _body.Enabled = true;
            Life = life;
        }

        internal void Deinitialize()
        {
            //disable the body so the particle no longer interacts in the physics system
            _body.Enabled = false;
        }

        public void Draw(XnaRenderer renderer, float particleSize, Color color, float alpha)
        {
            renderer.DrawSprite(_particleTexture, new RectangleF(Position.X, Position.Y, particleSize, particleSize), 0, color, alpha);
        }
    }
}