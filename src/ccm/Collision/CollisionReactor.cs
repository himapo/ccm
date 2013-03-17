using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Collision;

namespace ccm.Collision
{
    public class CollisionReactor : ICollisionReactor
    {
        // 相手に依存しない応答
        public Action<int, int> Reaction { get; set; }

        public void React(int id, int count, ICollisionActor actor)
        {
            Reaction(id, count);
        }
    }
}
