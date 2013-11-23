using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
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

        Dictionary<Keys, int> _keyTimeout = new Dictionary<Keys, int>();

        HashSet<Keys> _unpressedKeys = new HashSet<Keys>();
        KeyboardState _state;
        public void Update(GameTime gameTime)
        {
            _state = Keyboard.GetState();
            Keys[] pressedKeys = _state.GetPressedKeys();

            foreach (Keys key in pressedKeys)
            {
                int timeout;
                if (_keyTimeout.TryGetValue(key, out timeout))
                {
                    if (timeout != -1)
                    {
                        timeout--;
                        if (timeout <= 0)
                            _keyTimeout.Remove(key);
                        else
                            _keyTimeout[key] = timeout;
                    }
                }
                else
                {
                    timeout = OnKeyPressed(key, gameTime);
                    _keyTimeout[key] = timeout;
                }
            }

            foreach (Keys key in _keyTimeout.Keys.ToArray())
            {
                if (!_state.IsKeyDown(key))
                    _keyTimeout.Remove(key);
            }   
        }

        public event OnKeyPressedHandler OnKeyPressed;
        public delegate int OnKeyPressedHandler(Keys key, GameTime gameTime);

        internal void UnpressKeys()
        {
            foreach (Keys key in _keyTimeout.Keys)
                _keyTimeout[key] = -1;
        }
    }
}
