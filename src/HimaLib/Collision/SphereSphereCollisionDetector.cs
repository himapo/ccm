using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Collision
{
    public class SphereSphereCollisionDetector : ICollisionDetector
    {
        public SphereCollisionPrimitive ParamA { get; set; }

        public SphereCollisionPrimitive ParamB { get; set; }

        public bool Detect()
        {
            return ((ParamA.Center() - ParamB.Center()).Length() < ParamA.Radius() + ParamB.Radius());
        }
    }
}
