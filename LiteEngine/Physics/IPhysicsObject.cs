using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;

namespace LiteEngine.Physics
{
    public interface IPhysicsObject
    {
        Body Body
        {
            get;
        }

        void OnCollideWith(IPhysicsObject self, IPhysicsObject other, float impulse);
    }
}
