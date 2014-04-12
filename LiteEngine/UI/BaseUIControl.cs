using LiteEngine.Math;
using LiteEngine.Rendering;
using LiteEngine.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteEngine.UI
{
    public abstract class BaseUIControl
    {
        public BaseUIControl Parent = null;
        List<BaseUIControl> _children = new List<BaseUIControl>();
        public IEnumerable<BaseUIControl> Children
        {
            get
            {
                return _children;
            }
        }

        public bool Visible;

        /// <summary>
        /// Position relative to parent
        /// </summary>
        public Vector2 Position;
        public Vector2 Size;

        /// <summary>
        /// Adds a new child control to this control
        /// </summary>
        /// <param name="child"></param>
        /// <param name="position">bounds of the child control relative to this control</param>
        public void AddChild(BaseUIControl child, RectangleF bounds)
        {
            child.Parent = this;
            _children.Add(child);
            child.Position = new Vector2(bounds.Left, bounds.Top);
            child.Size = new Vector2(bounds.Width, bounds.Height);
        }

        public void AddChild(BaseUIControl child, Vector2 position)
        {
            child.Parent = this;
            _children.Add(child);
            child.Position = position;
        }

        public abstract void Draw(XnaRenderer renderer);

        protected Vector2 ScreenPosition
        {
            get
            {
                Vector2 ret = Position;
                for (BaseUIControl parent = Parent; parent != null; parent = parent.Parent)
                    ret += parent.Position;
                return ret;
            }
        }

        public RectangleF Bounds
        {
            get
            {
                return new RectangleF(Position.X, Position.Y, Size.X, Size.Y);
            }
        }

        public RectangleF ScreenBounds
        {
            get
            {
                Vector2 screen = ScreenPosition;
                return new RectangleF(screen.X, screen.Y, Size.X, Size.Y);
            }
        }

        float Level
        {
            get
            {
                int ret = 0;
                for (var p = Parent; p != null; p = p.Parent)
                    ret++;
                return ret;
            }
        }

        protected float DrawDepth
        {
            get
            {
                return 1 - Level * 0.1f;
            }
        }
        
        public bool KeyboardFocus = true;

        /// <summary>
        /// Override this method to handle key presses in the dialog when it has key focus
        /// </summary>
        /// <param name="key">pressed key</param>
        /// <returns>delay before the key press is fired again if the key is held down. 
        /// To prevent the key press being fired again until the key is released return -1 </returns>
        public virtual int ProcessKey(Keys key)
        {
            return -1;
        }
    }
}
