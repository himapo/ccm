using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Collision;
using ccm.Collision;

namespace ccm.Map
{
    public class MapCollisionInfo : CollisionInfo
    {
        CollisionReactor CollisionReactor = new CollisionReactor();

        public MapCollisionInfo()
        {
            Active = () => true;
            Group = () => (int)CollisionGroup.Floor;
            CollisionReactor.Reaction = (id, count) => { };
            Reactor = CollisionReactor;
        }

        public void AddAABB(Vector3 corner, Vector3 width)
        {
            var primitive = new AABBCollisionPrimitive()
            {
                Corner = corner,
                Width = width,
            };

            primitive.Corner = corner;
            primitive.Width = width;
            Primitives.Add(primitive);
        }
    }
}
