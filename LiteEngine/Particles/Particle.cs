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
    public class Particle
    {
        PhysicsObject _physicsBody;
        public int Life;
        static Texture _particleTexture = new Texture("particle");

        public Vector2 Position
        {
            get
            {
                return Body.Position;
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
                return _physicsBody.Body;
            }
        }

        public Vector2 Velocity
        {
            get
            {
                return Body.LinearVelocity;
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
            if (_physicsBody == null)
            {
                _physicsBody = physics.CreateBody();
                Fixture f = FixtureFactory.AttachCircle(0.01f, 1f, _physicsBody.Body, Vector2.Zero);
                f.CollisionCategories = Category.Cat2;
                f.CollidesWith = Category.Cat1;
            }
            else
            {
                //reset the collision callback
                _physicsBody.SetCollisionCallback(null);
            }
            Body body = _physicsBody.Body;
            //disable the body so that it doesn't collide before it has been initialized
            body.Enabled = false;
            body.BodyType = BodyType.Dynamic;
            body.Mass = 0.01f;
            body.Friction = 1f;
            body.Restitution = 0.1f;
            body.Position = position;
            body.LinearVelocity = velocity;
            body.IgnoreGravity = true;
            body.FixedRotation = true;
            body.LinearDamping = 0.1f;
            if (collidesWithWorld)
            {
                body.CollisionCategories = Category.Cat2;
                body.CollidesWith = Category.Cat1;
            }
            else
            {
                body.CollisionCategories = Category.None;
                body.CollidesWith = Category.None;
            }
            //enable the particle in the physics system
            body.Enabled = true;
            Life = life;
        }

        internal void Deinitialize()
        {
            //disable the body so the particle no longer interacts in the physics system
            _physicsBody.Body.Enabled = false;
        }

        public void Draw(XnaRenderer renderer, float particleSize, Color color, float alpha)
        {
            renderer.DrawSprite(_particleTexture, new RectangleF(Position.X, Position.Y, particleSize, particleSize), 0, color, alpha);
        }

        /// <summary>
        /// Sets a callback to invoke when the particle collides with another body
        /// </summary>
        /// <param name="collisionCallbackHandler"></param>
        public void SetCollisionCallback(CollisionCallbackHandler collisionCallbackHandler)
        {
            _physicsBody.SetCollisionCallback(collisionCallbackHandler);
        }
    }
}
