using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using LiteEngine.Rendering;
using Microsoft.Xna.Framework;
using LiteEngine.Textures;
using LiteEngine.Math;

namespace LiteEngine.UI
{
    public abstract class Dialog : BaseUIControl
    {
        OnDialogCompleteHandler _onComplete;
        Texture _background;
        public Dialog(OnDialogCompleteHandler onComplete=null)
        {
            _onComplete = onComplete;
            _background = new Texture("solid");
            BackgroundColor = Color.White;
        }

        bool _isClosing = false;
        public bool IsClosing
        {
            get
            {
                return _isClosing;
            }
        }

        internal void Initialize()
        {
            _isClosing = false;
        }

        public virtual void Close()
        {
            _isClosing = true;
            if (_onComplete != null)
                _onComplete(this);
        }

        public override void Draw(GameTime gameTime, XnaRenderer renderer)
        {
            renderer.DrawSprite(_background, Bounds, BackgroundColor);
        }

        public delegate void OnDialogCompleteHandler(Dialog dialog);
    }
}

