using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace LiteEngine.Input
{
    /// <summary>
    /// Manages keyboard state
    /// </summary>
    public class XnaKeyboardHandler
    {
        public bool IsKeyDown(Keys key)
        {
            return _state.IsKeyDown(key);
        }

        KeyboardState _state;
        public void Update()
        {
            _state = Keyboard.GetState();
        }
    }
}
