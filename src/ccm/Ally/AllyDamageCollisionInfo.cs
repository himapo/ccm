using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Collision;
using HimaLib.Math;
using ccm.Collision;

namespace ccm.Ally
{
    /// <summary>
    /// データ読みに対応するまでのプログラムによる構築
    /// </summary>
    class AllyDamageCollisionInfo : CollisionInfo
    {
        public Func<Vector3> Center { set { Primitive.Center = value; } }

        public Func<float> Radius { set { Primitive.Radius = value; } }

        public Action<int, int, AttackCollisionActor, CollisionResult> AttackReaction { set { AttackCollisionReactor.AttackReaction = value; } }

        SphereCollisionPrimitive Primitive = new SphereCollisionPrimitive();

        AttackCollisionReactor AttackCollisionReactor = new AttackCollisionReactor();

        public AllyDamageCollisionInfo()
        {
            Active = () => true;
            Group = () => (int)ccm.Collision.CollisionGroup.AllyDamage;

            AttackReaction = (id, count, actor, result) => { };
            Reactor = AttackCollisionReactor;

            Center = () => Vector3.Zero;
            Radius = () => 3.0f;
            Primitives.Add(Primitive);
        }
    }
}
