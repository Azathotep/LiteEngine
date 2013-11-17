using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using LiteEngine.Rendering;

namespace LiteEngine.UI
{
    public abstract class Dialog
    {
        public abstract void Render(XnaRenderer renderer);

        public bool IsVisible;

        public virtual int ProcessKey(Keys key)
        {
            return -1;
        }

        internal UIManager UI
        {
            get;
            set;
        }

        public void Close()
        {
            UI.HideDialog(this);
        }

        public virtual bool KeyboardFocus
        {
            get
            {
                return true;
            }
        }
    }
}
