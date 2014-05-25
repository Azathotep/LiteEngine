using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteEngine.Rendering
{
    class SimpleAnimation
    {
        double _startTime;
        bool _start;
        double _length;
        bool _loops;
        bool _running = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="length">length of the animation in seconds</param>
        public SimpleAnimation(double length, bool loops = false)
        {
            _length = length;
            _loops = loops;
        }

        public void Start()
        {
            _start = true;
            _running = true;
        }

        public void Draw(GameTime time, XnaRenderer renderer)
        {
            if (!_running)
                return;
            if (_start)
            {
                _startTime = time.TotalGameTime.TotalSeconds;
                _start = false;
            }
            double elapsed = time.TotalGameTime.TotalSeconds - _startTime;
            while (elapsed > _length)
            {
                if (!_loops)
                {
                    _running = false;
                    if (OnEnd != null)
                        OnEnd();
                    return;
                }
                elapsed -= _length;
            }
            //do frame
            if (OnDraw != null)
                OnDraw(elapsed, renderer);
        }

        /// <summary>
        /// Handler for animation OnDraw
        /// </summary>
        /// <param name="elapsedSeconds">number of seconds into the animation</param>
        public delegate void OnAnimationDrawHandler(double elapsedSeconds, XnaRenderer renderer);

        /// <summary>
        /// Called for the animation to render
        /// </summary>
        public OnAnimationDrawHandler OnDraw;

        
        public delegate void OnAnimationEndHandler();
        /// <summary>
        /// Called when the animation ends
        /// </summary>
        public OnAnimationEndHandler OnEnd;
    }
}
