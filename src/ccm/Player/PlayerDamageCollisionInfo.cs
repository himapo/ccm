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
    class PlayerDamageCollisionInfo : CollisionInfo
    {
        public Func<Vector3> Center { set { Primitive.Center = value; } }

        public Func<float> Radius { set { Primitive.Radius = value; } }

        public Action<int, int, AttackCollisionActor> AttackReaction { set { AttackCollisionReactor.AttackReaction = value; } }

        SphereCollisionPrimitive Primitive = new SphereCollisionPrimitive();

        AttackCollisionReactor AttackCollisionReactor = new AttackCollisionReactor();

        public PlayerDamageCollisionInfo()
        {
            Active = () => false;
            Group = () => (int)ccm.Collision.CollisionGroup.PlayerDamage;

            AttackReaction = (id, count, actor) => { };
            Reactor = AttackCollisionReactor;

            Center = () => Vector3.Zero;
            Radius = () => 0.0f;
            Primitives.Add(Primitive);
        }
    }
}
