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

        protected List<PhysicsBody> bodies = new List<PhysicsBody>();

        protected List<Plane> collidables = new List<Plane>();

        public Physics(Game game)
            : base(game) { }

        public void AddBody(PhysicsBody body)
        {
            bodies.Add(body);            
        }

        public void AddCollidable(Plane p)
        {
            collidables.Add(p);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (PhysicsBody body in bodies)
            {
                body.ApplyForce(gravity);

                // check collisions
                foreach (Plane p in collidables)
                {
                    if (body.Bounds.Intersects(p) == PlaneIntersectionType.Intersecting)
                    {
                        if (body.Force.Length() > 0)
                        {
                            body.ApplyForce(Vector3.Dot(p.Normal, Vector3.Normalize(body.Force)) * Vector3.Negate(body.Force));
                        }
                        if (body.Velocity.Length() > 0)
                        {
                            body.Velocity = Vector3.Dot(p.Normal, Vector3.Normalize(body.Velocity)) * Vector3.Reflect(body.Velocity, p.Normal);
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
