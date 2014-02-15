using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Collision
{
    public class RaySphereCollisionDetector : ICollisionDetector
    {
        public RayCollisionPrimitive Ray { get; set; }

        public SphereCollisionPrimitive Sphere { get; set; }

        public bool Detect(out Vector3 overlap)
        {
            overlap = Vector3.Zero;
            return Ray.Ray.Intersects(new Sphere(Sphere.Center(), Sphere.Radius())) != null;
        }
    }
}
