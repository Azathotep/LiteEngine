﻿using LiteEngine.Input;
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
        static Texture _borderTexture = new Texture("border");
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

        public bool Visible = true;

        /// <summary>
        /// Position relative to parent
        /// </summary>
        public Vector2 Position;

        public event EventHandler OnSizeChanged;

        Color _backgroundColor = Color.Transparent;
        public Color BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                _backgroundColor = value;
            }
        }

        SizeF _size;
        public SizeF Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                DockReposition();
                if (OnSizeChanged != null)
                    OnSizeChanged(this, null);
            }
        }

        void DockReposition()
        {
            if (Parent == null)
                return;
            switch (_dock)
            {
                case DockPosition.Center:
                    Position = Parent.Size * 0.5f - Size * 0.5f;
                    break;
                case DockPosition.Top:
                    Position = new Vector2(Parent.Size.Width * 0.5f - Size.Width * 0.5f, 0);
                    break;
                case DockPosition.Bottom:
                    Position = new Vector2(Parent.Size.Width * 0.5f - Size.Width * 0.5f, Parent.Size.Height - Size.Height);
                    break;
                case DockPosition.Right:
                    Position = new Vector2(Parent.Size.Width - Size.Width, Parent.Size.Height * 0.5f - Size.Height * 0.5f);
                    break;
                case DockPosition.Left:
                    Position = new Vector2(0, Parent.Size.Height * 0.5f - Size.Height * 0.5f);
                    break;
                case DockPosition.TopLeft:
                    Position = new Vector2(0, 0);
                    break;
                case DockPosition.TopRight:
                    Position = new Vector2(Parent.Size.Width - Size.Width, 0);
                    break;
                case DockPosition.BottomRight:
                    Position = new Vector2(Parent.Size.Width - Size.Width, Parent.Size.Height - Size.Height);
                    break;
                case DockPosition.BottomLeft:
                    Position = new Vector2(0, Parent.Size.Height - Size.Height);
                    break;
            }
            foreach (var child in _children)
                child.DockReposition();
        }

        /// <summary>
        /// Called when a mouse click event occurs within this control
        /// </summary>
        /// <param name="position">position of the mouse click relative to this control</param>
        /// <returns>true if this control or a subcontrol handled the mouse click</returns>
        internal bool OnMouseClickInternal(MouseButton button, Vector2 position)
        {
            foreach (var control in Children)
            {
                Vector2 relPos = position - control.Position;
                if (relPos.X < 0 || relPos.X >= control.Size.Width)
                    continue;
                if (relPos.Y < 0 || relPos.Y >= control.Size.Height)
                    continue;
                if (control.OnMouseClickInternal(button, relPos))
                    return true;
            }
            return OnMouseClick(button, position);
        }

        /// <summary>
        /// Called when the mouse is clicked on this control. Override to provide handling.
        /// </summary>
        /// <returns>true if the event is handled by this control</returns>
        protected virtual bool OnMouseClick(MouseButton button, Vector2 position)
        {
            return false;
        }

        public virtual void AddChild(BaseUIControl child)
        {
            child.Parent = this;
            _children.Add(child);
            child.DockReposition();
        }

        /// <summary>
        /// Adds a new child control to this control
        /// </summary>
        /// <param name="child"></param>
        /// <param name="position">bounds of the child control relative to this control</param>
        public void AddChild(BaseUIControl child, RectangleF bounds)
        {
            child.Position = new Vector2(bounds.Left, bounds.Top);
            child.Size = new SizeF(bounds.Width, bounds.Height);
            AddChild(child);
        }

        public void AddChild(BaseUIControl child, Vector2 position)
        {
            child.Position = position;
            AddChild(child);
        }

        internal void DrawInternal(GameTime gameTime, XnaRenderer renderer)
        {
            if (!Visible)
                return;
            if (BackgroundColor != Color.Transparent)
                DrawBackground(renderer);
            if (BorderWidth > 0.01f)
                DrawBorder(renderer);
            Draw(gameTime, renderer);

            Vector2 drawOffset = renderer.DrawOffset;
            foreach (BaseUIControl child in Children)
            {
                renderer.DrawOffset = drawOffset + Position;
                child.DrawInternal(gameTime, renderer);
            }
            renderer.DrawOffset = drawOffset;
        }

        private void DrawBackground(XnaRenderer renderer)
        {
            renderer.DrawFilledRectangle(Bounds.Grow(BorderWidth), BackgroundColor);
        }

        public virtual void Draw(GameTime gameTime, XnaRenderer renderer)
        {
        }

        public void DrawBorder(XnaRenderer renderer)
        {
            RectangleF borderRect = Bounds.Grow(BorderWidth);
            renderer.DrawRectangle(borderRect, BorderWidth, Color.Black);
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
                return new RectangleF(Position.X, Position.Y, Size.Width, Size.Height);
            }
            set
            {
                Position = value.TopLeft;
                Size = new SizeF(value.Width, value.Height);
            }
        }

        public RectangleF ScreenBounds
        {
            get
            {
                Vector2 screen = ScreenPosition;
                return new RectangleF(screen.X, screen.Y, Size.Width, Size.Height);
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
        public virtual KeyPressResult ProcessKey(UserInterface manager, Keys key)
        {
            return KeyPressResult.NotHandled;
        }

        DockPosition _dock = DockPosition.None;
        public DockPosition Dock
        {
            get
            {
                return _dock;
            }
            set
            {
                _dock = value;
            }
        }

        public Vector2 Center
        {
            get
            {
                return Position + Size * 0.5f;
            }
        }
    }

    public enum DockPosition
    {
        None,
        Top,
        Left,
        Bottom,
        Right,
        Center,
        TopLeft,
        TopRight,
        BottomRight,
        BottomLeft
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
