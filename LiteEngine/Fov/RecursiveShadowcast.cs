using LiteEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteEngine.Fov
{
    /// <summary>
    /// Implementation of the recursive shadowcast field-of-vision algorithm as described at roguebasin
    /// (http://roguebasin.roguelikedevelopment.org/index.php?title=FOV_using_recursive_shadowcasting)
    /// </summary>
    public class RecursiveShadowcast
    {
        IFovInfo _fovInfo;
        Vector2I _eye;
        int _viewRadius;

        /// <summary>
        /// Runs the FOV algorithm calling the provided callbacks. The visibleTileCallback will be called for each tile
        /// calculated to be in the field of vision
        /// </summary>
        /// <param name="eye">eye position of view</param>
        /// <param name="viewRadius">max radius of view</param>
        /// <param name="blocksLightCallback">the algorithm will invoke this callback passing the position of a tile (x,y). It expects the return value
        /// to be true if the tile at that coordinate blocks light (is opaque) or false it if lets light pass through (transparent)</param>
        /// <param name="visibleTileCallback">the algorithm invokes this callback for each visible tile it calculates is in the view</param>
        public void CalculateFov(Vector2I eye, int viewRadius, IFovInfo fovInfo)
        {
            _eye = eye;
            _viewRadius = viewRadius;
            _fovInfo = fovInfo;
            OctScan(1, 1, 1, 0);
            OctScan(2, 1, -1, 0);
            OctScan(3, 1, -1, 0);
            OctScan(4, 1, 1, 0);
            OctScan(5, 1, 1, 0);
            OctScan(6, 1, -1, 0);
            OctScan(7, 1, -1, 0);
            OctScan(8, 1, 1, 0);

            //need to explicitly report eye tile as visible
            fovInfo.OnTileVisible(eye.X, eye.Y);
        }

        private void OctScan(int oct, int depth, float slopeStart, float slopeEnd)
        {
            int gx = 0;
            int gy = 0;
            float endX = 0.5f;
            float endY = 0.5f;
            float startX = 0.5f;
            float startY = 0.5f;
            switch (oct)
            {
                case 1:
                    gx = 1;
                    startX = -0.5f;
                    startY = -0.5f;
                    endX = -0.5f;
                    break;
                case 2:
                    gx = -1;
                    startY = -0.5f;
                    break;
                case 3:
                    gy = 1;
                    startY = -0.5f;
                    endX = -0.5f;
                    endY = -0.5f;
                    break;
                case 4:
                    gy = -1;
                    endX = -0.5f;
                    break;
                case 5:
                    endY = -0.5f;
                    gx = -1;
                    break;
                case 6:
                    startX = -0.5f;
                    endX = -0.5f;
                    endY = -0.5f;
                    gx = 1;
                    break;
                case 7:
                    startX = -0.5f;
                    gy = -1;
                    break;
                case 8:
                    startX = -0.5f;
                    startY = -0.5f;
                    endY = -0.5f;
                    gy = 1;
                    break;
            }

            int dx = 0;
            int dy = 0;
            switch (oct)
            {
                case 1:
                case 2:
                    dx = -(int)System.Math.Round(depth * slopeStart);
                    dy = -depth;
                    break;
                case 3:
                case 4:
                    dx = depth;
                    dy = (int)System.Math.Round(depth * slopeStart);
                    break;
                case 5:
                case 6:
                    dx = (int)System.Math.Round(depth * slopeStart);
                    dy = depth;
                    break;
                case 7:
                case 8:
                    dx = -depth;
                    dy = -(int)System.Math.Round(depth * slopeStart);
                    break;
            }
            int y;
            int x;
            bool first = true;
            while (true)
            {
                x = _eye.X + dx;
                y = _eye.Y + dy;
                bool atEnd = false;
                switch (oct)
                {
                    case 1:
                    case 5:
                        if (GetSlope(_eye.X, _eye.Y, x, y) < slopeEnd)
                            atEnd = true;
                        break;
                    case 2:
                    case 6:
                        if (GetSlope(_eye.X, _eye.Y, x, y) > slopeEnd)
                            atEnd = true;
                        break;
                    case 3:
                    case 7:
                        if (GetSlopeInv(_eye.X, _eye.Y, x, y) > slopeEnd)
                            atEnd = true;
                        break;
                    case 4:
                    case 8:
                        if (GetSlopeInv(_eye.X, _eye.Y, x, y) < slopeEnd)
                            atEnd = true;
                        break;
                }
                if (atEnd)
                    break;

                if (!first)
                {
                    if (TileBlocksLight(x, y))
                    {
                        if (!TileBlocksLight(x - gx, y - gy))
                        {
                            if (oct == 1 || oct == 2 || oct == 5 || oct == 6)
                                OctScan(oct, depth + 1, slopeStart, GetSlope(_eye.X, _eye.Y, x + endX, y + endY));
                            else
                                OctScan(oct, depth + 1, slopeStart, GetSlopeInv(_eye.X, _eye.Y, x + endX, y + endY));
                        }
                    }
                    else
                    {
                        if (TileBlocksLight(x - gx, y - gy))
                        {
                            if (oct == 1 || oct == 2 || oct == 5 || oct == 6)
                                slopeStart = GetSlope(_eye.X, _eye.Y, x + startX, y + startY);
                            else
                                slopeStart = GetSlopeInv(_eye.X, _eye.Y, x + startX, y + startY);
                        }
                    }
                }
                if (dx * dx + dy * dy <= _viewRadius * _viewRadius)
                {
                    //notify that this tile is visible
                    _fovInfo.OnTileVisible(x, y);
                }
                dx += gx;
                dy += gy;
                first = false;
            }
            dx -= gx;
            dy -= gy;

            if (depth < _viewRadius)
            {
                if (!TileBlocksLight(_eye.X + dx, _eye.Y + dy))
                    OctScan(oct, depth + 1, slopeStart, slopeEnd);
            }
        }

        bool TileBlocksLight(int x, int y)
        {
            return _fovInfo.TileBlocksLight(x, y);
        }

        private float GetSlope(float x, float y, float x2, float y2)
        {
            return (x2 - x) / (y2 - y);
        }

        private float GetSlopeInv(float x, float y, float x2, float y2)
        {
            return (y2 - y) / (x2 - x);
        }
    }

    public interface IFovInfo
    {
        /// <summary>
        /// Returns whether the tile at the specified coordinates blocks light
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true if the tile blocks light, false if it doesn't</returns>
        bool TileBlocksLight(int x, int y);

        /// <summary>
        /// Called to indicate the tile at the specified coordinates is visible
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void OnTileVisible(int x, int y);
    }
}
