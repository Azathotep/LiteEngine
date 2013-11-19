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
using LiteGame.UI;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Common;

namespace LiteGame
{
    class Engine : LiteXnaEngine
    {
        Body _shipBody;

        PauseMenu _menu = new PauseMenu();        
        Ship _ship = new Ship();
        Texture _shipTexture;
        Texture _obstacleTexture;
        public Engine()
        {
            _shipTexture = new Texture("rocketship");
            _obstacleTexture = new Texture("pausemenu");
            Renderer.SetDeviceMode(800, 600, true);
            Renderer.Camera.SetViewField(40, 30);
            Renderer.Camera.LookAt(15, 10);
            _ship.Position = new Vector2(15, 10);

            Physics.Gravity = new Vector2(0, 2.5f);
            Body b = BodyFactory.CreateRectangle(Physics, 15f, 5f, 1f);
            b.IsStatic = true;
            b.Restitution = 0.2f;
            b.Friction = 0.2f;
            b.Position = new Vector2(5f,20f);
            
            _shipBody = BodyFactory.CreateBody(Physics, _ship.Position);
            _shipBody.BodyType = BodyType.Dynamic;
            _shipBody.AngularDamping = 0;
            _shipBody.Friction = 1f;
            _shipBody.Restitution = 1f;
            _shipBody.Mass = 10;
            
            //FixtureFactory.AttachRectangle(1, 1, 1f, new Vector2(0, 0), _shipBody);
            FixtureFactory.AttachPolygon(new Vertices(new Vector2[] { new Vector2(0f,-0.4f), new Vector2(0.35f,0.4f), new Vector2(-0.35f, 0.4f)}), 1f, _shipBody);
            _shipBody.OnCollision += _shipBody_OnCollision;
            Physics.ContactManager.PostSolve += PostSolve;
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

        protected override void DrawFrame(GameTime gameTime)
        {
            Renderer.Clear(Color.LightBlue);
            float facingAngle = (float)Math.Atan2(_ship.Facing.X, -_ship.Facing.Y);
            Renderer.BeginDraw();
            //Renderer.DrawSprite(_shipTexture, new RectangleF(_ship.Position.X, _ship.Position.Y, 2, 2), facingAngle);

            Renderer.DrawSprite(_shipTexture, new RectangleF(_shipBody.Position.X, _shipBody.Position.Y, 1, 1), _shipBody.Rotation);


            Renderer.DrawSprite(_obstacleTexture, new RectangleF(5,20,15,5), 0);
            Renderer.EndDraw();
        }

        protected override int OnKeyPress(Keys key)
        {
            Vector2 facing = new Vector2((float)Math.Sin(_shipBody.Rotation), -(float)Math.Cos(_shipBody.Rotation));
            //Vector3 dir = Vector3.Cross(new Vector3(0,0,1), new Vector3(facing.X, facing.Y, 0));
            //Vector3 dir2 = Vector3.Cross(new Vector3(0, 0, -1), new Vector3(facing.X, facing.Y, 0));

            switch (key)
            {
                case Keys.Left:
                    //_shipBody.ApplyForce(facing * 3f, _shipBody.Position - facing * -1f + new Vector2(dir.X, dir.Y) * 0.03f);
                    _shipBody.ApplyTorque(-0.2f);
                    return 0;
                case Keys.Right:
                    //_shipBody.ApplyForce(facing * 3f, _shipBody.Position - facing * -1f + new Vector2(dir2.X, dir2.Y) * 0.03f);
                    _shipBody.ApplyTorque(0.2f); 
                    //_ship.Rotate(0.1f);
                    return 0;
                case Keys.Up:
                    _shipBody.ApplyForce(facing * 3f, _shipBody.Position - facing);//, _shipBody.Position - facing * -5f);
                    //_ship.ApplyThrust(0.01f);
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

            //_shipBody.ApplyForce(new Vector2(0, 0.7f), _shipBody.Position + new Vector2(0, 0.5f));

            Physics.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));

            _ship.Update();
        }
    }
}
