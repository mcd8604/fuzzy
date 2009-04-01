using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Project_Fuzzy
{
    class Triangle
    {
        protected Plane p;

        protected Plane boundPlane12;
        protected Plane boundPlane23;
        protected Plane boundPlane31;

        protected Ray ray12;
        protected Ray ray23;
        protected Ray ray31;

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

            boundPlane12 = new Plane(v1, v2, v1 + p.Normal);
            boundPlane23 = new Plane(v2, v3, v2 + p.Normal);
            boundPlane31 = new Plane(v3, v1, v3 + p.Normal);

            ray12 = new Ray(v1, v2 - v1);
            ray23 = new Ray(v2, v3 - v2);
            ray31 = new Ray(v3, v1 - v3);
        }

        public bool Intersects(BoundingSphere sphere)
        {
            // First, check for planar intersection
            if (sphere.Intersects(p) != PlaneIntersectionType.Intersecting)
                return false;

            // Exit if the sphere is disjoint with the bounds of the triangle
            if (sphere.Intersects(boundPlane12) == PlaneIntersectionType.Back)
                return false;
            if (sphere.Intersects(boundPlane23) == PlaneIntersectionType.Back)
                return false;
            if (sphere.Intersects(boundPlane31) == PlaneIntersectionType.Back)
                return false;

            // Test if the center of the sphere is past the bounds of the triangle

            if (sphere.Intersects(boundPlane12) == PlaneIntersectionType.Intersecting &&
                boundPlane12.DotCoordinate(sphere.Center) < 0)
            {
                // Project the point onto the edge

                Vector3 edge = v1 - v2;
                Vector3 projPt = Vector3.Lerp(v1, v2, Vector3.Dot(v1 - sphere.Center, edge) / edge.LengthSquared());

                // Exit if the distance from the center to the triangle edge > radius

                if (Vector3.Distance(sphere.Center, projPt) > sphere.Radius)
                    return false;
                
            }

            if (sphere.Intersects(boundPlane23) == PlaneIntersectionType.Intersecting &&
                boundPlane23.DotCoordinate(sphere.Center) < 0)
            {
                Vector3 edge = v2 - v3;
                Vector3 projPt = Vector3.Lerp(v2, v3, Vector3.Dot(v2 - sphere.Center, edge) / edge.LengthSquared());

                if (Vector3.Distance(sphere.Center, projPt) > sphere.Radius)
                    return false;
            }
            
            if (sphere.Intersects(boundPlane23) == PlaneIntersectionType.Intersecting &&
                boundPlane31.DotCoordinate(sphere.Center) < 0)
            {
                Vector3 edge = v3 - v1;
                Vector3 projPt = Vector3.Lerp(v3, v1, Vector3.Dot(v3 - sphere.Center, edge) / edge.LengthSquared());

                if (Vector3.Distance(sphere.Center, projPt) > sphere.Radius)
                    return false;
            }
            
            return true;
        }
    }
}
