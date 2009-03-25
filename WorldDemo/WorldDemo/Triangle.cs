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

        protected Plane boundPlane1;
        protected Plane boundPlane2;
        protected Plane boundPlane3;

        protected Vector3 v1;
        public Vector3 V1
        {
            get { return v1; }
        }
        
        protected Vector3 v2;
        public Vector3 V2
        {
            get { return v2; }
        }

        protected Vector3 v3;
        public Vector3 V3
        {
            get { return v3; }
        }

        public Vector3 Normal
        {
            get { return p.Normal; }
        }

        public Triangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            v1 = p1;
            v2 = p2;
            v3 = p3;

            p = new Plane(v2, v1, v3);

            boundPlane1 = new Plane(v1, v2, v1 + p.Normal * 1);
            boundPlane2 = new Plane(v2, v3, v2 + p.Normal * 1);
            boundPlane2 = new Plane(v3, v1, v3 + p.Normal * 1);
        }

        public bool Intersects(BoundingSphere sphere)
        {
            // First, check for planar intersection
            if (sphere.Intersects(p) != PlaneIntersectionType.Intersecting)
                return false;

            // Check distance 
            float dist = Vector3.Dot(p.Normal, sphere.Center);

            if (dist > sphere.Radius)
                return false;

            // Check each bound plane for intersection

            float b1Dist = Vector3.Dot(boundPlane1.Normal, sphere.Center);

            if (b1Dist > sphere.Radius)
                return false;

            float b2Dist = Vector3.Dot(boundPlane2.Normal, sphere.Center);

            if (b2Dist > sphere.Radius)
                return false;

            float b3Dist = Vector3.Dot(boundPlane3.Normal, sphere.Center);

            if (b3Dist > sphere.Radius)
                return false;

            //Vector3 intersection = sphere.Center + p.Normal * distance;
            //Vector3 x = v2 - v1;
            //Vector3 y = v3 - v1;
            //Vector3 intersectionPrime = intersection - v1;

            //Matrix a = new Matrix();
            //a.M11 = x.X;
            //a.M21 = x.Y;
            //a.M31 = x.Z;
            //a.M12 = y.X;
            //a.M22 = y.Y;
            //a.M32 = y.Z;

            //Vector3 bary = Vector3.Transform(intersectionPrime, Matrix.Invert(a));
            
            return true;
        }
    }
}
