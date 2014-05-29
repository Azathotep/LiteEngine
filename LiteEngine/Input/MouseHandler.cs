using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteEngine.Input
{
    class MouseHandler
    {
        MouseState _lastState;
        public void Update()
        {
            MouseState state = Mouse.GetState();
            if (state.LeftButton == ButtonState.Pressed && _lastState.LeftButton == ButtonState.Released)
                OnMouseClick(MouseButton.Left, state.Position);
            if (state.RightButton == ButtonState.Pressed && _lastState.RightButton == ButtonState.Released)
                OnMouseClick(MouseButton.Right, state.Position);
            _lastState = state;
        }

        public event OnMouseClickHandler OnMouseClick;
        public delegate int OnMouseClickHandler(MouseButton button, Point mousePosition);
    }

    public enum MouseButton
    {
        Left,
        Right
    }
}
