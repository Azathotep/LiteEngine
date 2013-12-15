using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LiteEngine.Particles
{
    /// <summary>
    /// Manages a set of particles in the particle system
    /// </summary>
    public class ParticlePool
    {
        ParticleSystem _system;
        HashSet<Particle> _activeParticles = new HashSet<Particle>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="system"></param>
        public ParticlePool(ParticleSystem system)
        {
            _system = system;
            _system.OnRemoveParticle += _system_OnRemoveParticle;
        }

        /// <summary>
        /// Returns the active particles in the pool
        /// </summary>
        public IEnumerable<Particle> Particles
        {
            get
            {
                return _activeParticles;
            }
        }

        /// <summary>
        /// called when a particle is removed from the particle systemm
        /// </summary>
        /// <param name="removedParticle"></param>
        void _system_OnRemoveParticle(Particle removedParticle)
        {
            //unregister the particle from this factory
            _activeParticles.Remove(removedParticle);
        }

        /// <summary>
        /// Create a new particle in this pool
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="life"></param>
        /// <param name="collidesWithWorld">whether the particle should collide with world objects</param>
        public void CreateParticle(Vector2 position, Vector2 velocity, int life, bool collidesWithWorld)
        {
            Particle particle = _system.CreateParticle(position, velocity, life, collidesWithWorld);
            if (particle != null)
                _activeParticles.Add(particle);
        }
    }
}
