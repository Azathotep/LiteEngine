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
        Texture _obstacleTexture;
        public Engine()
        {
            _obstacleTexture = new Texture("pausemenu");
            Renderer.SetDeviceMode(800, 600, true);
            Renderer.Camera.SetViewField(40, 30);
            Renderer.Camera.LookAt(new Vector2(15, 10));

            _world = new World(Vector2.Zero);

            _ship = new Ship(_world);
            
            //_shipBody.OnCollision += _shipBody_OnCollision;

            //Physics.ContactManager.PostSolve += PostSolve;

            CreateMap();
        }

        LoopedTerrain _terrain;
        void CreateMap()
        {
            float circ = 100;
            float rad = circ / (float)Math.PI / 2;
            _terrain = new LoopedTerrain(_world, (int)circ, 10);
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
            Renderer.Clear(Color.LightBlue);

            Renderer.BeginDraw();
            _ship.Draw(Renderer);           
            _terrain.Draw(Renderer);
            Renderer.EndDraw();
        }

        protected override int OnKeyPress(Keys key)
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
                    return 0;
                case Keys.M:
                    UIManager.ShowDialog(_menu);
                    return -1;
            }
            return base.OnKeyPress(key);
        }

        protected override void UpdateFrame(GameTime gameTime, XnaKeyboardHandler keyHandler)
        {
            if (keyHandler.IsKeyDown(Keys.Escape))
                Exit();

            //Vector2 g = new Vector2(-_shipBody.Position.X, -_shipBody.Position.Y);
            //g.Normalize();
            //Physics.Gravity = g * 5;

            Vector2 dir = _ship.Position - Renderer.Camera.Position;
            if (dir.LengthSquared() > 0)
            {
                //dir.Normalize();
                Renderer.Camera.MoveBy(dir * 0.07f);
            }
            float angle = (float)Math.Atan2(_ship.Position.X, _ship.Position.Y);
            Renderer.Camera.Angle = angle;
            _world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
        }
    }
}
