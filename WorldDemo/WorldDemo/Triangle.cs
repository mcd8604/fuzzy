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
            // First, check for planar intersection
            if (sphere.Intersects(p) == PlaneIntersectionType.Intersecting)
            {
                // Next, check barycentric coords

                // D = -(A*x0+B*y0+C*z0)
                float distance = -(p.Normal.X * sphere.Center.X + p.Normal.Y * sphere.Center.Y + p.Normal.Z * sphere.Center.Z);

                if (distance <= sphere.Radius)
                {
                    Vector3 intersection = sphere.Center + p.Normal * distance;
                    Vector3 x = v2 - v1;
                    Vector3 y = v3 - v1;
                    Vector3 intersectionPrime = intersection - v1;

                    Matrix a = new Matrix();
                    a.M11 = x.X;
                    a.M21 = x.Y;
                    a.M31 = x.Z;
                    a.M12 = y.X;
                    a.M22 = y.Y;
                    a.M32 = y.Z;

                    Vector3 bary = Vector3.Transform(intersectionPrime, Matrix.Invert(a));
                }
            }

            return false;
        }
    }
}
