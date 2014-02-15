using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Collision;
using HimaLib.Math;
using ccm.Collision;

namespace ccm.Enemy
{
    /// <summary>
    /// データ読みに対応するまでのプログラムによる構築
    /// </summary>
    class EnemyDamageCollisionInfo : CollisionInfo
    {
        public Func<Vector3> Center { set { Primitive.Center = value; } }

        public Func<float> Radius { set { Primitive.Radius = value; } }

        public Action<int, int, AttackCollisionActor, Vector3> AttackReaction { set { AttackCollisionReactor.AttackReaction = value; } }

        SphereCollisionPrimitive Primitive = new SphereCollisionPrimitive();

        AttackCollisionReactor AttackCollisionReactor = new AttackCollisionReactor();

        public EnemyDamageCollisionInfo()
        {
            Active = () => true;
            Group = () => (int)ccm.Collision.CollisionGroup.EnemyDamage;

            AttackReaction = (id, count, actor, overlap) => { };
            Reactor = AttackCollisionReactor;

            Center = () => Vector3.Zero;
            Radius = () => 3.0f;
            Primitives.Add(Primitive);
        }
    }
}
