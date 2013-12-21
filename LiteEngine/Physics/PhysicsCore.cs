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
            IPhysicsObject pb1 = contact.FixtureA.Body.UserData as IPhysicsObject;
            IPhysicsObject pb2 = contact.FixtureB.Body.UserData as IPhysicsObject;
            if (pb1 != null || pb2 != null)
            {
                float maxImpulse = 0;
                int count = contact.Manifold.PointCount;
                for (int i = 0; i < count; ++i)
                    maxImpulse = System.Math.Max(maxImpulse, impulse.points[i].normalImpulse);
                if (pb2 != null)
                    pb2.OnCollideWith(pb2, pb1, maxImpulse);
                if (pb1 != null)
                    pb1.OnCollideWith(pb1, pb2, maxImpulse);
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

        public Body CreateBody(IPhysicsObject obj)
        {
            Body body = BodyFactory.CreateBody(_world);
            body.UserData = obj;
            return body;
        }

        public Body CreateRectangleBody(IPhysicsObject obj, float width, float height, float density)
        {
            Body body = BodyFactory.CreateRectangle(_world, width, height, density);
            body.UserData = obj;
            return body;
        }
    }
}
