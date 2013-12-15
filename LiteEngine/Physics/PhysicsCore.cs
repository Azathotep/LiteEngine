using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;

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
    }
}
