using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WorldDemo
{
    class Physics : GameComponent
    {
        protected Vector3 gravity = new Vector3(0, -9.8f, 0);

        /// <summary>
        /// Arbitrary frictional coefficient for two sliding surfaces
        /// </summary>
        protected const float friction = 0.2f;

        protected List<PhysicsBody> bodies = new List<PhysicsBody>();

        protected List<Triangle> collidables = new List<Triangle>();

        public Physics(Game game)
            : base(game) { }

        public void AddBody(PhysicsBody body)
        {
            bodies.Add(body);            
        }

        public void AddCollidable(Triangle p)
        {
            collidables.Add(p);
        }

        public void AddCollidables(List<Triangle> p)
        {
            collidables.AddRange(p);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (PhysicsBody body in bodies)
            {
                body.Normal = Vector3.Zero;
                body.ApplyForce(gravity);

                // check collisions
                foreach (Triangle p in collidables)
                {
                    if (p.Intersects(body.Bounds))
                    {
                        if (body.Force.Length() > 0)
                        {
                            body.ApplyForce(Vector3.Dot(p.Normal, Vector3.Normalize(Vector3.Negate(gravity))) * gravity.Length() * p.Normal);
                            body.Normal = Vector3.Normalize(body.Normal + p.Normal);
                        }
                        if (body.Velocity.Length() > 0)
                        {
                            body.Velocity = Vector3.Dot(p.Normal, Vector3.Normalize(body.Velocity)) * friction * Vector3.Reflect(body.Velocity, p.Normal);
                        }
                    }
                }

                body.Update(gameTime);

                // reset force
                body.ResetForce();
            }

            base.Update(gameTime);
        }

    }
}
