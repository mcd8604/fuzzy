using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WorldDemo
{
    class Triangle
    {
        protected Plane p;

        protected Vector3 v1;
        protected Vector3 v2;
        protected Vector3 v3;

        public Vector3 Normal
        {
            get { return p.Normal; }
        }

        public Triangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            v1 = p1;
            v2 = p2;
            v3 = p3;

            p = new Plane(v1, v2, v3);
        }

        public bool Intersects(BoundingSphere sphere)
        {
            throw new NotImplementedException();
        }
    }
}
