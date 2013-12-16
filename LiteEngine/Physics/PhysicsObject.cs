using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;

namespace LiteEngine.Physics
{
    /// <summary>
    /// A wrapper around a Farseer physics body. Provides collision feedback.
    /// </summary>
    public class PhysicsObject
    {
        Body _body;
        public PhysicsObject(Body body)
        {
            _body = body;
            _body.UserData = this;
        }

        /// <summary>
        /// Called internally when this object collides with something
        /// </summary>
        /// <param name="maxImpulse"></param>
        internal void Collision(float maxImpulse)
        {
            //Callback if one is defined
            if (_collisionCallback != null)
                _collisionCallback.Invoke(maxImpulse);
        }

        /// <summary>
        /// Returns the underlying farseer Body for this object
        /// </summary>
        public Body Body
        {
            get
            {
                return _body;
            }
        }

        //maybe this should be an event...
        CollisionCallbackHandler _collisionCallback;

        /// <summary>
        /// Sets a callback which will be invoked when this object collides with another object
        /// </summary>
        /// <param name="handler">callback</param>
        public void SetCollisionCallback(CollisionCallbackHandler handler)
        {
            _collisionCallback = handler;   
        }
    }
}
