﻿using System;
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

        protected Vector3 collisionNormal = Vector3.Zero;

        /// <summary>
        /// Average of collision surface normals
        /// </summary>
        public Vector3 Normal
        {
            get { return collisionNormal; }
            set { collisionNormal = value; }
        }

        public void Update(float dT, List<Triangle> collidables)
        {
            collisionNormal = Vector3.Zero;
            bounds.Center += lastVelocity * dT;

            // check collision     
            bool collision = false;

            foreach (Triangle p in collidables)
            {
                float? dist = p.Intersects(bounds);
                if (dist != null)
                {
                    // handle collision
                    collision = true;
                    collisionNormal += p.Normal;
                    bounds.Center += p.Normal * (bounds.Radius - (float)dist);
                    velocity -= Vector3.Dot(p.Normal, velocity) * p.Normal;
                }
            }

            if (collision)
            {
                collisionNormal = Vector3.Normalize(collisionNormal);
            }

            //velocity += accel * dT;
            
            // reset velocity
            lastVelocity = velocity;
            velocity = Vector3.Zero;
        }

    }
}
