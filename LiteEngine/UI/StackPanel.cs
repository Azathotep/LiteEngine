using LiteEngine.Math;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteEngine.UI
{
    public class StackPanel : BaseUIControl
    {
        public override void AddChild(BaseUIControl child)
        {
            child.Size = new SizeF(Size.Width, child.Size.Height);
            base.AddChild(child);
            child.OnSizeChanged += child_OnSizeChanged;
        }

        void child_OnSizeChanged(object sender, EventArgs e)
        {
            float y = 0;
            //reorganize child controls
            foreach (var child in Children)
            {
                child.Position = new Vector2(0, y);
                y += child.Size.Height;
            }
        }
    }
}
