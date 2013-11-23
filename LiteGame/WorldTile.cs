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
    class LoopedTerrain
    {
        Texture _texture;

        WorldTile[,] _tiles;
        int _height;
        int _width;
        public LoopedTerrain(World world, int width, int height)
        {
            _tiles = new WorldTile[width, height];
            _height = height;
            _width = width;
            _texture = new Texture("pausemenu");


            for (int y = 1; y < _height; y++)
                for (int x = 0; x < _width; x++) //-5; x < 10; x++) //
                {
                    //if (x == 2)
                      //  continue;
                    if (Dice.Next(5) == 0 && y == 1)
                        continue;
                    int lx = x;
                    if (lx < 0)
                        lx += _width;
                    
                    float angle = 2f * (float)Math.PI * lx / _width;
                    float radius = _width / (2f * (float)Math.PI) - y;
                    float px = (float)Math.Sin(angle) * radius;
                    float py = -(float)Math.Cos(angle) * radius;
                    _tiles[lx, y] = new WorldTile(px, py, angle);

                    Body b = BodyFactory.CreateRectangle(world, 1f, 1f, 1f);
                    b.IsStatic = true;
                    b.Restitution = 0.5f;
                    b.Friction = 0.3f;
                    b.Rotation = angle;
                    b.Position = new Vector2(px, py);
                }
        }

        internal void Draw(XnaRenderer Renderer)
        {
            //float angle = Renderer.Camera.Angle;
            //if (angle >= Math.PI)
            //    angle -= (float)Math.PI;
            //int viewCenterX = (int)(MathHelper.WrapAngle(angle) / (Math.PI * 2) * _width); // (int)Renderer.Camera.Position.X;

            float angle = (float)Math.Atan2(Renderer.Camera.Position.X, -Renderer.Camera.Position.Y);

            int viewCenterX = (int)(angle / (Math.PI * 2) * _width);

            for (int y = 0; y < _height; y++)
                for (int x = viewCenterX - (int)Renderer.Camera.ViewWidth / 2 - 1; x <= viewCenterX + Renderer.Camera.ViewWidth / 2 + 1; x++)
                {
                    int realX = (x % _width + _width) % _width;

                    WorldTile tile = _tiles[realX, y];
                    if (tile == null)
                        continue;
                    tile.Draw(Renderer);
                }

            //for (int y = 0; y < _height; y++)
            //    for (int x = 0; x < _width; x++)
            //    {
            //        WorldTile tile = _tiles[x, y];
            //        if (tile == null)
            //            continue;
            //        tile.Draw(Renderer);
            //    }
        }
    }

    class WorldTile
    {
        static Texture _texture = new Texture("pausemenu");
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
