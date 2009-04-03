using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_Fuzzy
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

        protected List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();
        protected VertexPositionNormalTexture[] vertexArray;
        public VertexPositionNormalTexture[] Vertices
        {
            get { return vertexArray; }
        }

        public Physics(Game game)
            : base(game) { }

        public void AddBody(PhysicsBody body)
        {
            bodies.Add(body);            
        }

        public void AddCollidable(Triangle p)
        {
            collidables.Add(p);
            vertices.Add(new VertexPositionNormalTexture(p.V1, p.Normal, Vector2.Zero));
            vertices.Add(new VertexPositionNormalTexture(p.V2, p.Normal, Vector2.Zero));
            vertices.Add(new VertexPositionNormalTexture(p.V3, p.Normal, Vector2.Zero));
            vertexArray = vertices.ToArray();
        }

        public void AddCollidables(List<Triangle> p)
        {
            collidables.AddRange(p);
            foreach (Triangle t in p)
            {
                vertices.Add(new VertexPositionNormalTexture(t.V1, t.Normal, Vector2.Zero));
                vertices.Add(new VertexPositionNormalTexture(t.V2, t.Normal, Vector2.Zero));
                vertices.Add(new VertexPositionNormalTexture(t.V3, t.Normal, Vector2.Zero));
            }
            vertexArray = vertices.ToArray();
        }

        public void AddCollidables(List<BoundingBox> boxes)
        {
            //collidables.AddRange(boxes);
            
            //vertexArray = vertices.ToArray();
        }


        public override void Update(GameTime gameTime)
        {
            foreach (PhysicsBody body in bodies)
            {
                body.Normal = Vector3.Zero;
                body.ApplyForce(gravity * body.Mass);

                // check collisions
                foreach (Triangle p in collidables)
                {
                    if (p.Intersects(body.Bounds))
                    {
                        if (body.Force.Length() > 0)
                        {
                            body.Normal += p.Normal;
                        }
                    }
                }
                // Apply Normal Force
                if (body.Normal != Vector3.Zero)
                {
                    body.Normal = Vector3.Normalize(body.Normal);

                    if (body.Velocity.Length() > 0)
                    {
                        body.Velocity = Vector3.Dot(body.Normal, Vector3.Normalize(body.Velocity)) * friction * Vector3.Reflect(body.Velocity, body.Normal);
                    }

                    float dot = Math.Abs(Vector3.Dot(body.Normal, body.Force ));
                    float dotN = Vector3.Dot(body.Normal, Vector3.Normalize(body.Force));
                    float dot2 = dotN * body.Force.Length();

                    //body.ApplyForce(Vector3.Dot(body.Normal, Vector3.Normalize(body.Force)) * body.Force.Length() * body.Normal);
                    //body.ApplyForce(body.Force.Length() * body.Normal);
                    body.ApplyForce(dot * body.Normal);
                    //body.ApplyForce(Vector3.Negate(body.Force));
                }
                body.Update(gameTime);

                // reset force
                body.ResetForce();
            }

            base.Update(gameTime);
        }

    }
}
