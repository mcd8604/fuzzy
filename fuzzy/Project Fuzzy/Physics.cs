﻿using System;
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

                // check collision     
           
                bool collision = false;
                Vector3 offset = Vector3.Zero;
                
                foreach (Triangle p in collidables)
                {
                    float? dist = p.Intersects(body.Bounds);
                    if (dist != null)
                    {
                        collision = true;
                        body.Normal += p.Normal;
                        offset += p.Normal * (body.Bounds.Radius - (float)dist);
                    }
                }

                // prevent clipping
                body.Position += offset;
                
                // apply gravity
                body.Velocity += gravity;

                // handle collision
                if(collision)
                {
                    body.Normal = Vector3.Normalize(body.Normal);
                    body.Velocity -= Vector3.Dot(body.Normal, body.Velocity) * body.Normal;
                }

                body.Update(gameTime);

                // reset body
                body.Velocity = Vector3.Zero;

            }

            base.Update(gameTime);
        }

    }
}
