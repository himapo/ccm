using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Collision;
using HimaLib.Math;
using ccm.Collision;

namespace ccm.Player
{
    /// <summary>
    /// データ読みに対応するまでのプログラムによる構築
    /// </summary>
    class PlayerBodyCollisionInfo : CollisionInfo
    {
        public Func<Vector3> Base { set { Primitive.Base = value; } }

        public Action<int, int, Vector3> Reaction { set { CollisionReactor.Reaction = value; } }

        CylinderCollisionPrimitive Primitive = new CylinderCollisionPrimitive();

        CollisionReactor CollisionReactor = new CollisionReactor();

        public PlayerBodyCollisionInfo()
        {
            Active = () => true;
            Group = () => (int)ccm.Collision.CollisionGroup.PlayerBody;
            Reactor = CollisionReactor;

            Primitive.Radius = () => 3.0f;
            Primitive.Height = () => 12.0f;
            Primitives.Add(Primitive);
        }
    }
}
