using LiteEngine.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteEngine.Rendering
{
    public class Animation : Timeline
    {
        public void AddFrame(Texture texture, int duration)
        {
            AddEvent(() =>
            {
                _currentTexture = texture;
            }, duration);
            if (_currentTexture == null)
                _currentTexture = texture;
        }

        Texture _currentTexture;
        public Texture CurrentTexture
        {
            get
            {
                return _currentTexture;
            }
        }

        public bool IsRunning { get; set; }
    }
}
