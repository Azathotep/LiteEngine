using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using LiteEngine.Textures;
using LiteEngine.Xna;
using LiteEngine.Math;
using LiteEngine.Input;
using LiteEngine.Core;
using LiteEngine.Rendering;
using LiteGame.UI;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using RenderTarget2D = Microsoft.Xna.Framework.Graphics.RenderTarget2D;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

namespace LiteGame
{
    class Engine : LiteXnaEngine
    {
        World _world;
        PauseMenu _menu = new PauseMenu();
        Ship _ship;
        Texture _fireTexture;
        ParticleList _exhaustParticles = new ParticleList(100);
        Planet _planet;

        public Engine()
        {
            _fireTexture = new Texture("fireparticle");
            Renderer.SetDeviceMode(800, 600, true);
            Renderer.Camera.SetViewField(80, 60); //40,30
            Renderer.Camera.LookAt(new Vector2(15, 10));
            _world = new World(Vector2.Zero);
            _ship = new Ship(_world);
            //_shipBody.OnCollision += _shipBody_OnCollision;
            //Physics.ContactManager.PostSolve += PostSolve;
            CreatePlanet();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        void CreatePlanet()
        {
            float circ = 5000;
            //circ = 500;
            _planet = new Planet(_world, (int)circ, 60); //60); //55);


            float rad = circ / (float)Math.PI / 2;

            //_planet = new Planet(_world, (int)circ, 100);
            _ship.Position = new Vector2(0, -rad - 3);
            Renderer.Camera.LookAt(_ship.Position);
            GravityController gc = new GravityController(rad*5, 10000, 10);
            gc.Enabled = true;
            gc.AddPoint(new Vector2(0, 0));
            _world.AddController(gc);
        }

        private void PostSolve(Contact contact, ContactVelocityConstraint impulse)
        {
            float maxImpulse = 0.0f;
            int count = contact.Manifold.PointCount;

            for (int i = 0; i < count; ++i)
            {
                maxImpulse = Math.Max(maxImpulse, impulse.points[i].normalImpulse);
            }

            if (maxImpulse > 2)
            {
                int a = 1;
            }
        }

        bool _shipBody_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            float maxImpulse = 0.0f;
            int count = contact.Manifold.PointCount;

            for (int i = 0; i < count; ++i)
            {
                //maxImpulse = Math.Max(maxImpulse contact.FixtureA.I impulse.points[i].normalImpulse);
            }
            return true;
        }

        Queue<RenderTarget2D> _regionTargetQueue = new Queue<RenderTarget2D>();

        RegionManager _regionManager = new RegionManager();

        int i = 0;
        Vector2 pos;
        float angle;
        List<long> time = new List<long>();
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        protected override void DrawFrame(GameTime gameTime)
        {
            i++;
            sw.Restart();
            if (i % 30 == 50)
            {
                pos = Renderer.Camera.Position;
                angle = Renderer.Camera.Angle;

                Renderer.Clear(Color.Black);

                Renderer.BeginDraw();
                //_ship.Draw(Renderer);

                Renderer.DrawDepth = 0.4f;
                _planet.Draw(Renderer, (float)Math.Atan2(pos.X, -pos.Y));

                //System.Threading.Thread.Sleep(10);
                Renderer.EndDraw();

            }


            _regionManager.Update(Renderer, _ship.Position, _planet);

            //Vector2 v = new Vector2((int)Math.Floor(_ship.Position.X / ww) * ww, (int)Math.Floor(_ship.Position.Y / ww) * ww);

            //if (i % 30 == 1)
            //{
            //    DrawWorldToRenderTarget(_rtarget[1, 0], new Rectangle((int)v.X, (int)v.Y, ww, ww));
            //}
            
            
            _numFrames++;

            Vector2 offset = Renderer.Camera.Position - pos;
            Renderer.UseRenderTarget(null);
            Renderer.Clear(Color.Black);
            Renderer.BeginDraw();
            //Renderer.DrawSprite((Microsoft.Xna.Framework.Graphics.Texture2D)_rtarget1, new Rectangle(0, 0, _rtarget1.Width, _rtarget1.Height), new RectangleF(_ship.Position.X - offset.X, _ship.Position.Y - offset.Y, 120, 90), 0.5f, 0, new Vector2(0.5f, 0.5f), Color.White);
             
            Renderer.DrawDepth = 0.4f;
            //_planet.Draw(Renderer);


            _regionManager.Draw(Renderer);


            //Microsoft.Xna.Framework.Graphics.Texture2D target = (Microsoft.Xna.Framework.Graphics.Texture2D)_rtarget[0,0];

            //Renderer.DrawSprite(target, new Rectangle(0, 0, target.Width, target.Height), new RectangleF(pos.X, pos.Y, 80, 60), 0.5f, angle, new Vector2(0.5f, 0.5f), Color.White);

            //Texture2D target2 = (Texture2D)_rtarget[1, 0];
            //Renderer.DrawSprite(target2, new Rectangle(0, 0, target2.Width, target2.Height), new RectangleF(v.X, v.Y-2, ww, ww), 0.4f, 0, new Vector2(0f, 0f), Color.White);

            //Texture2D target3 = (Texture2D)_rtarget[2, 0];
            //Renderer.DrawSprite(target3, new Rectangle(0, 0, target3.Width, target3.Height), new RectangleF(v2.X, v2.Y - 2, ww, ww), 0.4f, 0, new Vector2(0f, 0f), Color.White);

            Renderer.DrawDepth = 0.3f;
            _ship.Draw(Renderer);
            
            Renderer.DrawDepth = 0.2f;
            foreach (Particle p in _exhaustParticles.Particles)
            {
                float particleSize = 0.25f * (p.Life / 50f); // p.Life / 100f * 0.4f; // 0.2f;
                float alpha = (float)p.Life * p.Life / (50 * 50);
                Color color = new Color(1, 1, (float)p.Life / 60f);
                //Renderer.DrawSprite(_fireTexture, new RectangleF(p.Position.X, p.Position.Y, particleSize, particleSize), 0.1f, 0f, new Vector2(0.5f, 0.5f), new Color(1, (float)p.Life/50, (float)p.Life / 30, (float)p.Life / 50)); //, alpha);
                Renderer.DrawSprite(_fireTexture, new RectangleF(p.Position.X, p.Position.Y, particleSize, particleSize), 0.1f, color, alpha); //, alpha);
            }

            Renderer.EndDraw();

            Renderer.BeginDrawToScreen();
            string frameRate = _frameRate + " FPS";
            Renderer.DrawStringBox(frameRate, new RectangleF(10, 10, 120, 10), Color.White);
            Renderer.EndDraw();

            sw.Stop();
            time.Add(sw.ElapsedMilliseconds);
        }

        protected override int OnKeyPress(Keys key, GameTime gameTime)
        {
            switch (key)
            {
                case Keys.Left:
                    _ship.RotateThrusters(-0.1f);
                    return 0;
                case Keys.Right:
                    _ship.RotateThrusters(0.1f);
                    return 0;
                case Keys.Up:
                    _ship.ApplyForwardThrust(0.01f);
                    float m = Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f));
                    Vector2 vel = _ship.Velocity * m - _ship.Facing * 0.1f;
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 v = vel + -_ship.Facing * i * 0.01f;
                        v.X += Dice.Next() * 0.01f - 0.005f;
                        v.Y += Dice.Next() * 0.01f - 0.005f;
                        float f = Dice.Next();
                        Color color = Color.Yellow;
                        //if (Dice.Next(2) == 0)
                            color = new Color(1, f, f, 1f);
                        //else
                        //    color = new Color(1, 1, f, 1f);
                        _exhaustParticles.AddParticle(new Particle(_ship.Position - _ship.Facing * 0.4f, v, color, 50)); // - _ship.Facing*0.1f
                    }
                    return 0;
                case Keys.M:
                    UIManager.ShowDialog(_menu);
                    return -1;
            }
            return base.OnKeyPress(key, gameTime);
        }

        protected override void UpdateFrame(GameTime gameTime, XnaKeyboardHandler keyHandler)
        {
            if (keyHandler.IsKeyDown(Keys.Escape))
                Exit();

            _exhaustParticles.Update();

            Vector2 dir = _ship.Position - Renderer.Camera.Position;
            if (dir.LengthSquared() > 0)
            {
                Renderer.Camera.MoveBy(dir * 0.07f);
            }
            float angle = (float)Math.Atan2(_ship.Position.X, -_ship.Position.Y);
            Renderer.Camera.Angle = angle;
            
            _world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
            
            _frameTime += gameTime.ElapsedGameTime;

            while (_frameTime > TimeSpan.FromSeconds(1))
            {
                _frameTime -= TimeSpan.FromSeconds(1);
                _frameRate = _numFrames;
                _numFrames = 0;
            }
        }

        int _frameRate,_numFrames=0;
        TimeSpan _frameTime = TimeSpan.Zero;
    }

    class RegionManager
    {
        List<Region> _regions = new List<Region>();
        HashSet<Vector2> _regionsToDraw = new HashSet<Vector2>();

        //loaded regions are regions that can be drawn
        Dictionary<Vector2, Region> _loadedRegions = new Dictionary<Vector2, Region>();

        //returns a region that isn't in the list of regions to draw
        Region GetUnusedRegion()
        {
            foreach (Region region in _regions)
                if (!_regionsToDraw.Contains(region.Position))
                    return region;
            return null;
        }

        int ww = 60;

        public void Draw(XnaRenderer renderer)
        {
            foreach (Vector2 v in _regionsToDraw)
            {
                Region region = null;
                if (_loadedRegions.TryGetValue(v, out region))
                {
                    Texture2D target = (Texture2D)region.Target;
                    renderer.DrawSprite(target, new Rectangle(0, 0, target.Width, target.Height), new RectangleF((int)region.Position.X, (int)region.Position.Y - 2, ww, ww), 0.4f, 0, new Vector2(0f, 0f), Color.White);
                }
            }
        }

        public void Update(XnaRenderer renderer, Vector2 position, Planet planet)
        {
            _regionsToDraw.Clear();
            
            int centerRegionX = (int)Math.Floor(position.X / ww) * ww;
            int centerRegionY = (int)Math.Floor(position.Y / ww) * ww;
            for (int y = -2; y <= 2; y++)
                for (int x = -2; x <= 2; x++)
                {
                    _regionsToDraw.Add(new Vector2(centerRegionX + x * ww, centerRegionY + y * ww));
                }

            //load a region if needed
            foreach (Vector2 region in _regionsToDraw)
            {
                if (!_loadedRegions.ContainsKey(region))
                {
                    //load region
                    //find an unused region
                    Region unused = GetUnusedRegion();
                    if (unused == null)
                    {
                        if (_regions.Count > 20)
                            break;
                        unused = new Region();
                        unused.Init(renderer);
                        _regions.Add(unused);
                    }
                    else
                        _loadedRegions.Remove(unused.Position);

                    unused.Position = region;
                    LoadRegion(unused, renderer, planet);
                    break;
                }
            }
        }

        void LoadRegion(Region region, XnaRenderer renderer, Planet planet)
        {
            renderer.UseRenderTarget(region.Target);
            renderer.Camera.SetViewField(ww, ww);

            Vector2 pos = renderer.Camera.Position;
            renderer.Camera.LookAt(new Vector2(region.Position.X, region.Position.Y));
            float angle = renderer.Camera.Angle;
            renderer.Camera.Angle = 0;
            renderer.BeginDraw();
            renderer.Clear(Color.Black);
            renderer.DrawDepth = 0.4f;
            planet.Draw(renderer, (float)Math.Atan2(renderer.Camera.Position.X, -renderer.Camera.Position.Y));

            //Renderer.DrawSprite(_fireTexture, new RectangleF(world.X, world.Y, world.Width, world.Height), 0.5f, 0f, new Vector2(0f,0f), new Color(Color.White, 0.1f));

            renderer.EndDraw();
            renderer.UseRenderTarget(null);

            renderer.Camera.SetViewField(120,90); //300, 200); //120, 90); //80, 60);
            renderer.Camera.LookAt(pos);
            renderer.Camera.Angle = angle;

            _loadedRegions.Add(region.Position, region);
        }
    }

    class Region
    {
        public Vector2 Position;
        public RenderTarget2D Target;
        public void Init(XnaRenderer renderer)
        {
            Target = renderer.CreateRenderTarget(256, 256);
        }
    }
}
