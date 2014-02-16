using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Collision;
using HimaLib.Math;
using ccm.Collision;

namespace ccm.Camera
{
    public class CameraRayCollisionInfo : CollisionInfo
    {
        public Func<Vector3> Direction { set { Primitive.Direction = value; } }

        public Func<Vector3> Position { set { Primitive.Position = value; } }

        public Action<int, int, CollisionResult> Reaction { set { CollisionReactor.Reaction = value; } }

        RayCollisionPrimitive Primitive = new RayCollisionPrimitive();

        CollisionReactor CollisionReactor = new CollisionReactor();

        public CameraRayCollisionInfo()
        {
            Active = () => true;
            Group = () => (int)CollisionGroup.CameraRay;
            Reactor = CollisionReactor;

            Primitives.Add(Primitive);
        }
    }
}
