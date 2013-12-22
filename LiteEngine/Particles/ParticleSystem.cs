using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LiteEngine.Physics;

namespace LiteEngine.Particles
{
    /// <summary>
    /// Manages particles. Particles are reused
    /// </summary>
    public class ParticleSystem
    {
        PhysicsCore _physics;
        LinkedList<Particle> _activeParticles = new LinkedList<Particle>();
        Queue<Particle> _unusedParticles = new Queue<Particle>();
        int _maxActiveParticles = 800;

        public ParticleSystem(PhysicsCore physics)
        {
            _physics = physics;
        }

        public ParticlePool CreateParticleFactory()
        {
            return new ParticlePool(this);
        }

        public Particle CreateParticle(Vector2 position, Vector2 velocity, int life)
        {
            Particle newParticle;
            if (_unusedParticles.Count == 0)
            {
                //no unused particles so need to create a new one
                //if the maximum capacity of active particles has been reached then exit
                //TODO destroy and reuse the oldest particle instead
                if (_activeParticles.Count >= _maxActiveParticles)
                    return null;
                newParticle = new Particle();
            }
            else
                newParticle = _unusedParticles.Dequeue();
            //setup the particle properties
            newParticle.Initialize(_physics, position, velocity, life);
            _activeParticles.AddFirst(newParticle);
            return newParticle;
        }

        /// <summary>
        /// Updates the particle system reducing the life of all particles
        /// </summary>
        public void Update()
        {
            LinkedListNode<Particle> next;
            for (var node = _activeParticles.First; node != null; node = next)
            {
                next = node.Next;
                Particle particle = node.Value;
                particle.Life--;
                if (particle.Life <= 0)
                {
                    _activeParticles.Remove(node);
                    particle.Deinitialize();
                    _unusedParticles.Enqueue(particle);
                    if (OnRemoveParticle != null)
                        OnRemoveParticle(particle);
                }
            }
        }

        public delegate void OnRemoveParticleHandler(Particle removedParticle);
        public event OnRemoveParticleHandler OnRemoveParticle; 
    }
}
