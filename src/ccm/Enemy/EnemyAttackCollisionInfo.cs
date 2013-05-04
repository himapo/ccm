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
    class EnemyAttackCollisionInfo : CollisionInfo
    {
        public Func<Vector3> Center { set { Primitive.Center = value; } }

        public Func<float> Radius { set { Primitive.Radius = value; } }

        public int Power { set { AttackCollisionActor.Power = value; } }

        SphereCollisionPrimitive Primitive = new SphereCollisionPrimitive();

        AttackCollisionActor AttackCollisionActor = new AttackCollisionActor();

        public EnemyAttackCollisionInfo()
        {
            Active = () => false;
            Group = () => (int)ccm.Collision.CollisionGroup.EnemyAttack;
            AttackCollisionActor.Power = 1;
            AttackCollisionActor.Shock = 500;
            Actor = AttackCollisionActor;

            Primitive.Center = () => Vector3.Zero;
            Primitive.Radius = () => 4.0f;
            Primitives.Add(Primitive);
        }
    }
}
