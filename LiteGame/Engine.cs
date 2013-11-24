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
using LiteGame.UI;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;

namespace LiteGame
{
    class Engine : LiteXnaEngine
    {
        World _world;
        PauseMenu _menu = new PauseMenu();
        Ship _ship;
        Texture _fireTexture;
        public Engine()
        {
            _fireTexture = new Texture("fireparticle");
            Renderer.SetDeviceMode(800, 600, true);
            Renderer.Camera.SetViewField(40, 30);
            Renderer.Camera.LookAt(new Vector2(15, 10));

            _world = new World(Vector2.Zero);

            _ship = new Ship(_world);
            
            //_shipBody.OnCollision += _shipBody_OnCollision;

            //Physics.ContactManager.PostSolve += PostSolve;

            CreatePlanet();
        }

        ParticleList _exhaustParticles = new ParticleList(100);

        Planet _planet;
        void CreatePlanet()
        {
            float circ = 400;
            float rad = circ / (float)Math.PI / 2;
            _planet = new Planet(_world, (int)circ, 3);
            _ship.Position = new Vector2(0, -rad - 10);
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

        bool[,] tiles = new bool[40, 30];

        protected override void DrawFrame(GameTime gameTime)
        {
            Renderer.Clear(Color.Black);

            Renderer.BeginDraw();
            _ship.Draw(Renderer);           
            _planet.Draw(Renderer);

            foreach (Particle p in _exhaustParticles.Particles)
            {
                float particleSize = 0.25f * (p.Life / 50f); // p.Life / 100f * 0.4f; // 0.2f;
                float alpha = (float)p.Life * p.Life / (50 * 50);
                Color color = new Color(1, 1, (float)p.Life / 60f);
                //Renderer.DrawSprite(_fireTexture, new RectangleF(p.Position.X, p.Position.Y, particleSize, particleSize), 0.1f, 0f, new Vector2(0.5f, 0.5f), new Color(1, (float)p.Life/50, (float)p.Life / 30, (float)p.Life / 50)); //, alpha);
                Renderer.DrawSprite(_fireTexture, new RectangleF(p.Position.X, p.Position.Y, particleSize, particleSize), 0.1f, color, alpha); //, alpha);
            }

            Renderer.EndDraw();
            _numFrames++;            
            Renderer.BeginDrawToScreen();
            string frameRate = _frameRate + " FPS";
            Renderer.DrawStringBox(frameRate, new RectangleF(10, 10, 120, 10), Color.White);
            Renderer.EndDraw();
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
                        _exhaustParticles.AddParticle(new Particle(_ship.Position - _ship.Facing * 0.5f, v, color, 50)); // - _ship.Facing*0.1f
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
            //Vector2 g = new Vector2(-_shipBody.Position.X, -_shipBody.Position.Y);
            //g.Normalize();
            //Physics.Gravity = g * 5;

            _exhaustParticles.Update();

            Vector2 dir = _ship.Position - Renderer.Camera.Position;
            if (dir.LengthSquared() > 0)
            {
                Renderer.Camera.MoveBy(dir * 0.07f);
            }
            float angle = (float)Math.Atan2(_ship.Position.X, _ship.Position.Y);
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
}
