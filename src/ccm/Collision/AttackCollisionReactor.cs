using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Collision;
using HimaLib.Math;

namespace ccm.Collision
{
    public class AttackCollisionReactor : ICollisionReactor
    {
        // 攻撃に対する応答
        public Action<int, int, AttackCollisionActor, Vector3> AttackReaction { get; set; }

        public void React(int id, int count, ICollisionActor actor, Vector3 overlap)
        {
            if (actor is AttackCollisionActor)
            {
                AttackReaction(id, count, actor as AttackCollisionActor, overlap);
            }
        }
    }
}
