using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Project_Fuzzy
{
    class PhysicsBody
    {
        protected float maxSpeedSq = 25f;

        /// <summary>
        /// Current velocity
        /// </summary>
        protected Vector3 velocity = Vector3.Zero;
        public Vector3 Velocity
        {
            get { return velocity; }
            set 
            {
                if (value.LengthSquared() > maxSpeedSq)
                    velocity = Vector3.Normalize(value) * maxSpeedSq;
                else 
                    velocity = value; 
            }
        }

        protected Vector3 lastVelocity = Vector3.Zero;
        
        /// <summary>
        /// Applied acceleration
        /// </summary>
        protected Vector3 accel = Vector3.Zero;
        public Vector3 Accel
        {
            get { return accel; }
            set
            {
                accel = value;
            }
        }

        /// <summary>
        /// Current position
        /// </summary>
        public Vector3 Position
        {
            get { return bounds.Center; }
            set { bounds.Center = value; }
        }

        public float Radius
        {
            get { return bounds.Radius; }
            set { bounds.Radius = value; }
        }

        protected BoundingSphere bounds = new BoundingSphere();

        public BoundingSphere Bounds
        {
            get { return bounds; }
        }

        protected bool isColliding = false;
        public bool IsColliding
        {
            get { return isColliding; }
        }

        public void Update(float dT, List<CustomModel> collidables, Vector3 gravity)
        {
            bounds.Center += lastVelocity * dT;

            // check collisions
            Vector3 clipOffset = Vector3.Zero;
            int numCollisions = 0;

            foreach(CustomModel model in collidables)
            //foreach (Triangle p in collidables)
            {
                foreach(ModelPart part in model.ModelParts) 
                {
                    if (part.Collidable)
                    {
                        foreach (Triangle p in part.CollisionTriangles)
                        {
                            float? dist = p.Intersects(bounds);
                            if (dist != null && dist >= 0)
                            {
                                // handle collision
                                ++numCollisions;
                                float dot = Vector3.Dot(p.Normal, velocity);
                                if (dot < 0)
                                {
                                    clipOffset += p.Normal * (bounds.Radius - (float)dist);
                                    velocity -= dot * p.Normal;
                                }
                            }
                        }
                    }
                }
            }

            if (numCollisions > 0)
            {
                isColliding = true;
                bounds.Center += clipOffset / numCollisions;
            }
            else
            {
                isColliding = false;
                velocity += gravity;
            }

            velocity += accel * dT;

            lastVelocity = velocity;
            velocity = Vector3.Zero;
        }

    }
}
