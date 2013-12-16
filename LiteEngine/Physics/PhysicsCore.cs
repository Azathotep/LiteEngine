using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

namespace LiteEngine.Physics
{
    /// <summary>
    /// Provides an abstraction layer over the farseer physics engine
    /// </summary>
    public class PhysicsCore
    {
        World _world;
        public PhysicsCore()
        {
            _world = new World(Vector2.Zero);
            _world.ContactManager.PostSolve += PostSolve;
        }

        private void PostSolve(Contact contact, ContactVelocityConstraint impulse)
        {
            Body body = contact.FixtureA.Body;
            PhysicsObject pb1 = contact.FixtureA.Body.UserData as PhysicsObject;
            PhysicsObject pb2 = contact.FixtureB.Body.UserData as PhysicsObject;
            if (pb1 != null || pb2 != null)
            {
                float maxImpulse = 0;
                int count = contact.Manifold.PointCount;
                for (int i = 0; i < count; ++i)
                    maxImpulse = System.Math.Max(maxImpulse, impulse.points[i].normalImpulse);
                if (pb2 != null)
                    pb2.Collision(maxImpulse);
                if (pb1 != null)
                    pb1.Collision(maxImpulse);
            }
        }

        public void SetGlobalGravity(Vector2 force)
        {
            _world.Gravity = force;
        }

        internal void Update(float step)
        {
            _world.Step(step);    
        }

        public PhysicsObject CreateBody()
        {
            return new PhysicsObject(BodyFactory.CreateBody(_world));
        }

        public PhysicsObject CreateRectangleBody(float width, float height, float density)
        {
            return new PhysicsObject(BodyFactory.CreateRectangle(_world, width, height, density));
        }
    }

    /// <summary>
    /// Delegate for collision handlers
    /// </summary>
    /// <param name="impulse">impulse of the collision, more = greater impact</param>
    /// <returns>if true the collision handler is deregistered for the body</returns>
    public delegate void CollisionCallbackHandler(float impulse);
}
