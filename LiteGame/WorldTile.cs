using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteEngine.Textures;
using LiteEngine.Rendering;
using LiteEngine.Core;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace LiteGame
{
    class WorldTile
    {
        static Texture _texture = new Texture("ground");
        float _angle;
        Vector2 _position;
        public WorldTile(float positionX, float positionY, float angle)
        {
            _angle = angle;
            _position = new Vector2(positionX, positionY);
        }

        public void Draw(XnaRenderer renderer)
        {
            renderer.DrawSprite(_texture, new LiteEngine.Math.RectangleF(_position.X, _position.Y, 1, 1), _angle);
        }
    }
}
