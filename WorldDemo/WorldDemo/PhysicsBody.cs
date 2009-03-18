using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WorldDemo
{
    class PhysicsBody
    {
        /// <summary>
        /// Mass (kg)
        /// </summary>
        protected float mass = 1f;

        /// <summary>
        /// Current net force
        /// </summary>
        protected Vector3 force = Vector3.Zero;

        public Vector3 Force
        {
            get { return force; }
        }

        /// <summary>
        /// Current momentum
        /// </summary>
        //protected Vector3 momentum = Vector3.Zero;

        /// <summary>
        /// Current acceleration
        /// </summary>
        protected Vector3 acceleration = Vector3.Zero;

        /// <summary>
        /// Current velocity
        /// </summary>
        protected Vector3 velocity = Vector3.Zero;
        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
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

        public void Update(GameTime time)
        {
            float dT = (float)time.ElapsedGameTime.TotalSeconds;

            //momentum = dT * force;

            // Euler integration

            acceleration = force / mass;

            velocity += acceleration * dT;

            // fake friction
            //velocity += Vector3.Negate(velocity) * .01f;

            bounds.Center += velocity * dT;
        }

        internal void ResetForce()
        {
            force = Vector3.Zero;
        }

        public void ApplyForce(Vector3 force)
        {
            this.force += force;
        }
    }
}
