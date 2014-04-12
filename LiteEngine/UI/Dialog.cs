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
        public Dialog()
        {
            
        }
        
        public event EventHandler OnClose;

        public void Close()
        {
            if (OnClose != null)
                OnClose(this, new EventArgs());
        }
    }
}

