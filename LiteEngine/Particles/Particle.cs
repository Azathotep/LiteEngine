using System;
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
    public class Particle : IPhysicsObject
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

        /// <summary>
        /// Returns the dynamic collision body for this particle or null if the particle does
        /// not support collisions
        /// </summary>
        public Body Body
        {
            get
            {
                return _body;
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
        internal void Initialize(PhysicsCore physics, Vector2 position, Vector2 velocity, int life)
        {
            if (_body == null)
            {
                _body = physics.CreateBody(this);
                Fixture f = FixtureFactory.AttachCircle(0.01f, 1f, _body, Vector2.Zero);
                f.CollisionCategories = Category.Cat2;
                f.CollidesWith = Category.Cat1;
            }

            Body body = _body;
            //disable the body so that it doesn't collide before it has been initialized
            body.Enabled = false;
            body.BodyType = BodyType.Dynamic;
            body.Mass = 0.0001f;
            body.Friction = 1f;
            body.Restitution = 0.1f;
            body.Position = position;
            body.LinearVelocity = velocity;
            body.IgnoreGravity = true;
            body.FixedRotation = true;
            body.LinearDamping = 0.1f;
            body.CollidesWith = Category.None;
            body.CollisionCategories = Category.None;
            //enable the particle in the physics system
            body.Enabled = true;
            Life = life;
        }

        internal void Deinitialize()
        {
            //disable the body so the particle no longer interacts in the physics system
            _body.Enabled = false;
            OnCollideWithOther = null;
        }

        public void Draw(XnaRenderer renderer, float particleSize, Color color, float alpha)
        {
            renderer.DrawSprite(_particleTexture, Position, new Vector2(particleSize, particleSize), 0, color, alpha);
        }

        public void OnCollideWith(IPhysicsObject self, IPhysicsObject other, float impulse)
        {
            if (OnCollideWithOther != null)
            {
                Particle particle = self as Particle;
                OnCollideWithOther(particle, other, impulse);
                return;
            }
            Life = 0;
        }

        public event OnCollideWithHandler OnCollideWithOther;
    }

    public delegate void OnCollideWithHandler(Particle particle, IPhysicsObject other, float impulse);
}
