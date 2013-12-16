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
            Action<float> callback;
            if (!_collisionCallbacks.TryGetValue(body, out callback))
                return;
            float maxImpulse=0;
            int count = contact.Manifold.PointCount;
            for (int i = 0; i < count; ++i)
            {
                maxImpulse = System.Math.Max(maxImpulse, impulse.points[i].normalImpulse);
            }
            callback.Invoke(maxImpulse);
        }

        public void SetGlobalGravity(Vector2 force)
        {
            _world.Gravity = force;
        }

        internal void Update(float step)
        {
            _world.Step(step);    
        }

        public Body CreateBody()
        {
            return BodyFactory.CreateBody(_world);
        }

        public Body CreateRectangleBody(float width, float height, float density)
        {
            return BodyFactory.CreateRectangle(_world, width, height, density);
        }


        Dictionary<Body, Action<float>> _collisionCallbacks = new Dictionary<Body, Action<float>>();
        public void RegisterCollisionCallback(Body body, Action<float> collisionCallback)
        {
            _collisionCallbacks[body] = collisionCallback;
        }
    }
}
