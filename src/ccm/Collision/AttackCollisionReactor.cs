using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Collision;

namespace ccm.Collision
{
    public class AttackCollisionReactor : ICollisionReactor
    {
        // 攻撃に対する応答
        public Action<int, int, AttackCollisionActor, CollisionResult> AttackReaction { get; set; }

        public void React(int id, int count, ICollisionActor actor, CollisionResult result)
        {
            if (actor is AttackCollisionActor)
            {
                AttackReaction(id, count, actor as AttackCollisionActor, result);
            }
        }
    }
}
