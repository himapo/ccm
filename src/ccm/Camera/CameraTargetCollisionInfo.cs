using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Collision;
using HimaLib.Math;
using ccm.Collision;

namespace ccm.Camera
{
    public class CameraTargetCollisionInfo : CollisionInfo
    {
        public Func<Vector3> Center { set { Primitive.Center = value; } }

        public Func<float> Radius { set { Primitive.Radius = value; } }

        public Action<int, int, CollisionResult> Reaction { set { CollisionReactor.Reaction = value; } }

        SphereCollisionPrimitive Primitive = new SphereCollisionPrimitive();

        CollisionReactor CollisionReactor = new CollisionReactor();

        public CameraTargetCollisionInfo()
        {
            Active = () => true;
            Group = () => (int)ccm.Collision.CollisionGroup.CameraTarget;
            Reactor = CollisionReactor;

            Primitive.Center = () => Vector3.Zero;
            Primitive.Radius = () => 2.0f;
            Primitives.Add(Primitive);
        }
    }
}
