using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteEngine.Textures;
using LiteEngine.Core;
using LiteEngine.Rendering;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace LiteGame
{
    class Planet
    {
        WorldTile[,] _tiles;
        int _height;
        int _width;
        public Planet(World world, int width, int height)
        {
            _tiles = new WorldTile[width, height];
            _height = height;
            _width = width;


            for (int y = 1; y < _height; y++)
                for (int x = 0; x < _width; x++) //-5; x < 10; x++) //
                {
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
                    if (Dice.Next(2) == 0)
                        _tiles[lx, y].Ground = true;
                    //Body b = BodyFactory.CreateRectangle(world, 1f, 1f, 1f);
                    //b.IsStatic = true;
                    //b.Restitution = 0.5f;
                    //b.Friction = 0.3f;
                    //b.Rotation = angle;
                    //b.Position = new Vector2(px, py);
                }
        }

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            

        internal void Draw(XnaRenderer Renderer, float angle)
        {
            sw.Reset();
            sw.Start();
            //float angle = Renderer.Camera.Angle;
            //if (angle >= Math.PI)
            //    angle -= (float)Math.PI;
            //int viewCenterX = (int)(MathHelper.WrapAngle(angle) / (Math.PI * 2) * _width); // (int)Renderer.Camera.Position.X;

            //float angle = 0;// (float)Math.Atan2(Renderer.Camera.Position.X, -Renderer.Camera.Position.Y);
            List<WorldTile> grass = new List<WorldTile>();
            List<WorldTile> ground = new List<WorldTile>();

            int side = 2;
            _height = 40;
            int viewCenterX = (int)(angle / (Math.PI * 2) * _width);
            for (int y = 0; y < _height; y++)
                for (int x = viewCenterX - (int)Renderer.Camera.ViewWidth / 2 - side; x <= viewCenterX + Renderer.Camera.ViewWidth / 2 + side; x++)
                {
                    int realX = (x % _width + _width) % _width;
                    WorldTile tile = _tiles[realX, y];
                    if (tile == null)
                        continue;
                    if (tile.Ground)
                        ground.Add(tile);
                    else
                        grass.Add(tile);
                    //tile.Draw(Renderer);
                }
            foreach (WorldTile tile in grass) //.Take(2500)
                tile.Draw(Renderer);
            foreach (WorldTile tile in ground)
                tile.Draw(Renderer);

            int c = grass.Count + ground.Count;

            sw.Stop();
            ms.Add(sw.ElapsedMilliseconds);
        }
        List<long> ms = new List<long>();
    }
}
