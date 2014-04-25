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
        static Texture _borderTexture = new Texture("shipparts", new RectangleI(0, 0, 32, 32));
        public float BorderWidth = 0;

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

        public event EventHandler OnSizeChanged;

        Vector2 _size;
        public Vector2 Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                if (OnSizeChanged != null)
                    OnSizeChanged(this, null);
            }
        }

        public virtual void AddChild(BaseUIControl child)
        {
            child.Parent = this;
            _children.Add(child);
        }

        /// <summary>
        /// Adds a new child control to this control
        /// </summary>
        /// <param name="child"></param>
        /// <param name="position">bounds of the child control relative to this control</param>
        public void AddChild(BaseUIControl child, RectangleF bounds)
        {
            child.Position = new Vector2(bounds.Left, bounds.Top);
            child.Size = new Vector2(bounds.Width, bounds.Height);
            AddChild(child);
        }

        public void AddChild(BaseUIControl child, Vector2 position)
        {
            child.Position = position;
            AddChild(child);
        }

        public virtual void Draw(XnaRenderer renderer)
        {
        }

        public void DrawBorder(XnaRenderer renderer)
        {
            if (BorderWidth > 0.01f)
                renderer.DrawSprite(_borderTexture, Bounds.Grow(BorderWidth));
        }

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
        /// <returns>Structure indicating whether the key press was handled and if so what the delay should
        /// be before the key can be reprocessed</returns>
        public virtual KeyPressResult ProcessKey(UIManager manager, Keys key)
        {
            return KeyPressResult.NotHandled;
        }
    }

    /// <summary>
    /// Return structure returned from ProcessKey which contains both whether the key press was handled
    /// and if it was handled what the delay should be until the same key can be processed again.
    /// There can be no delay, or a latched setting where the key must be depressed and pressed again before
    /// the key can be processed again.
    /// </summary>
    public struct KeyPressResult
    {
        public bool Handled;
        public short ReprocessDelay;
        public KeyPressResult(short refireDelay)
        {
            Handled = true;
            ReprocessDelay = refireDelay;
        }

        public KeyPressResult(bool handled)
        {
            Handled = handled;
            ReprocessDelay = -1;
        }

        public static KeyPressResult HandledNoDelay = new KeyPressResult(0);
        public static KeyPressResult HandledLatched = new KeyPressResult(-1);
        public static KeyPressResult NotHandled = new KeyPressResult(false);
    }
}
